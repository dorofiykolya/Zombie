Shader "Curve/Specular Curve Reflective" {
	Properties {
		_MainTex ("Main Texture", 2D) = "black" {}
		_NearCurve ("Near Curve", Vector) = (0, 0, 0, 0)
		_FarCurve ("Far Curve", Vector) = (0, 0, 0, 0)
		_Dist ("Distance Mod", Float) = 0.0
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
		_ReflectVisible ("Reflection Visible", Range (0.0, 1)) = 1
		_SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
        _Shininess ("Shininess", Float) = 10
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
        Pass { // pass 0

			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#pragma multi_compile_fwdbase LIGHTMAP_OFF LIGHTMAP_ON
			#include "UnityCG.cginc"
			#ifndef SHADOWS_OFF		
			#include "AutoLight.cginc"	
			#endif
			#include "Lighting.cginc"
						
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D unity_Lightmap;
			uniform half4 	_MainTex_TexelSize;
			uniform half4 unity_LightmapST;
			uniform float4 _NearCurve;
			uniform float4 _FarCurve;
			uniform float _Dist;
			uniform samplerCUBE _Cube;
			uniform float _ReflectVisible;
			uniform float _Shininess;
			
			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 uvLM : TEXCOORD1;
				
				float3 normalDir : TEXCOORD4;
	            float3 viewDir : TEXCOORD5;
	            
				#ifndef SHADOWS_OFF
		        LIGHTING_COORDS(2,3)
				#endif
				fixed4 posWorld : TEXCOORD6;
				fixed4 color: COLOR;
			};
						
			fragmentInput vert(appdata_full v)
			{
				fragmentInput o;

				// ПРИМЕНЯЕТСЯ ИЗГИБ
                float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
                float distanceSquared = pos.z * pos.z * _Dist;
                float x = (_FarCurve.x - max(1.0 - distanceSquared / _NearCurve.x, 0.0) * _FarCurve.x);
                float y = (_FarCurve.y - max(1.0 - distanceSquared / _NearCurve.y, 0.0) * _FarCurve.y);
                pos.x += x;
                pos.y += y;
                o.pos = mul(UNITY_MATRIX_P, pos); 
                // ПРИМЕНЯЕТСЯ ИЗГИБ
                
                // ЗАДАНИЕ UV ТЕКСТУРЫ
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uvLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				// ЗАДАНИЕ UV ТЕКСТУРЫ

				// СМЕЩЕНИЕ UV
				#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv.y = 1.0-o.uv.y;
				#endif
				// СМЕЩЕНИЕ UV
				
				#ifndef SHADOWS_OFF			  	
      			TRANSFER_VERTEX_TO_FRAGMENT(o);
				#endif
				
	            float4x4 modelMatrix = _Object2World;
	            float4x4 modelMatrixInverse = _World2Object; 
	            
	 			o.posWorld = mul(modelMatrix, v.vertex);
	 			
	            o.normalDir = float3(mul(modelMatrix, v.vertex) - float4(_WorldSpaceCameraPos, 1.0)).xyz;
	            o.viewDir = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse))).xyz;
	            
	            #ifdef DIRECTIONAL
	            float3 normalDirection = normalize(float3(mul(float4(v.normal, 0.0f), modelMatrixInverse)));
				float3 lightDirection = normalize(float3(_WorldSpaceLightPos0));
				float3 diffuseReflection = float3(_LightColor0) * max(0.0f, dot(normalDirection, lightDirection));
				o.color = float4(diffuseReflection, 1.0f) + UNITY_LIGHTMODEL_AMBIENT;
				#endif
	            
				return o;
			}
			
			fixed4 frag(fragmentInput i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv);
				
				#ifdef LIGHTMAP_ON
				fixed3 lm = DecodeLightmap (tex2D (unity_Lightmap, i.uvLM));
				color.rgb *= lm;
				#endif
				
				#ifndef SHADOWS_OFF
				fixed atten = LIGHT_ATTENUATION(i);
				color.rgb *= atten;
				#endif
				
	            float3 normalDirection = normalize(i.normalDir);
    		
	            float3 viewDirection = normalize(_WorldSpaceCameraPos - float3(i.posWorld));
	            float3 lightDirection;
	            float attenuation;
				if (0.0 == _WorldSpaceLightPos0.w) // directional light?
	            {
	               attenuation = 1.0; // no attenuation
	               lightDirection = normalize(float3(_WorldSpaceLightPos0));
	            } 
	            else // point or spot light
	            {
	               float3 vertexToLightSource = float3(_WorldSpaceLightPos0 - i.posWorld);
	               float distance = length(vertexToLightSource);
	               attenuation = 1.0 / distance; // linear attenuation 
	               lightDirection = normalize(vertexToLightSource);
	            }
	 
	            float3 ambientLighting = float3(UNITY_LIGHTMODEL_AMBIENT);
	            float3 diffuseReflection = attenuation * float3(_LightColor0) * max(0.0, dot(normalDirection, lightDirection));
	            float3 specularReflection;
	            if (dot(normalDirection, lightDirection) < 0.0) 
	               // light source on the wrong side?
	            {
	               specularReflection = float3(0.0, 0.0, 0.0); 
	                  // no specular reflection
	            }
	            else // light source on the right side
	            {
	               specularReflection = attenuation * float3(_LightColor0) 
	                  * float3(_SpecColor) * pow(max(0.0, dot(
	                  reflect(-lightDirection, normalDirection), 
	                  viewDirection)), _Shininess);
	            }
	            color *= float4(ambientLighting + diffuseReflection + specularReflection, 1.0);
	            
	            #ifdef DIRECTIONAL
				float3 lightColor = _LightColor0.rgb;
		        float3 lightDir = _WorldSpaceLightPos0;
		        
		        float3 n = float3(0.0f, 1.0f, 0.0f);
		        float  sat = saturate(dot(n, lightDir));
		 
		        color.rgb *= lightColor * sat * i.color;
		        #endif
	            
				float3 reflectedDir = reflect(i.viewDir, normalize(i.normalDir));
				#ifdef DIRECTIONAL
            	color += (texCUBE(_Cube, reflectedDir) * _ReflectVisible) * sat;
            	#else
            	color += texCUBE(_Cube, reflectedDir) * _ReflectVisible;
            	#endif
            	
            	return color;
			}
			
			ENDCG
        } // end pass 
        
	} 
	FallBack "Curve/Diffuse Curve"
}
