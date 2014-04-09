Shader "Curve/Diffuse Specular Curve Reflective" {
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
			#ifdef DIRECTIONAL
			#include "Lighting.cginc"
			#endif
			
			#define CUSTOM_REFLECTIVE
			#define CUSTOM_SPECULAR_LIGHT
			#define CUSTOM_DIFFUSE_LIGHT
						
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D unity_Lightmap;
			uniform half4 	_MainTex_TexelSize;
			uniform half4 unity_LightmapST;
			uniform float4 _NearCurve;
			uniform float4 _FarCurve;
			uniform float _Dist;
			#ifdef CUSTOM_REFLECTIVE
			uniform samplerCUBE _Cube;
			uniform float _ReflectVisible;
			#endif
			
			#ifdef CUSTOM_SPECULAR_LIGHT
			uniform float _Shininess;
			#endif
			
			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 uvLM : TEXCOORD1;
				
				#if defined(CUSTOM_SPECULAR_LIGHT) || defined(CUSTOM_REFLECTIVE) 
				float3 normalDir: TEXCOORD3;
				#endif
				#if defined(CUSTOM_REFLECTIVE) 
	            float3 viewDir: TEXCOORD4;
	            #endif
	            
				#ifndef SHADOWS_OFF
				float4 _ShadowCoord : TEXCOORD2;
		        //LIGHTING_COORDS(2,3)
				#endif
				
				#if defined(CUSTOM_SPECULAR_LIGHT) 
				fixed4 posWorld;
				#endif
				
	            #if defined(CUSTOM_DIFFUSE_LIGHT) && defined(DIRECTIONAL)
				fixed4 color: COLOR;
				#endif
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
      				#if UNITY_EDITOR_SHADOW_ON	 
	      			o._ShadowCoord = ComputeScreenPos(mul(UNITY_MATRIX_MVP, v.vertex));
	      			#else
	      			TRANSFER_VERTEX_TO_FRAGMENT(o);
	      			#endif
				#endif
				
	            float4x4 modelMatrix = _Object2World;
	            float4x4 modelMatrixInverse = _World2Object; 
	            
	            #if defined(CUSTOM_SPECULAR_LIGHT)
	 				o.posWorld = mul(modelMatrix, v.vertex);
	 			#endif
	 			
	 			
	 			// ПРОЭКЦИЯ ДЛЯ КУБА РЕФЛЕКСИИ
	 			#if defined(CUSTOM_SPECULAR_LIGHT) || defined(CUSTOM_REFLECTIVE) 
	            	o.normalDir = float3(mul(modelMatrix, v.vertex) - float4(_WorldSpaceCameraPos, 1.0)).xyz;
	            #endif
	            #if defined(CUSTOM_REFLECTIVE) 
	            	o.viewDir = normalize(float3(mul(float4(v.normal, 0.0), modelMatrixInverse))).xyz;
	            #endif
	            // ПРОЭКЦИЯ ДЛЯ КУБА РЕФЛЕКСИИ
	            
	            ///////////////////
	            
	            //ОСВЕЩЕНИЕ И НАПРАВЛЕНИЕ СВЕТА (ГЛОБАЛЬНОЕ ИЛИ ТОЧЕЧНОЕ ИЛИ ОТ ЛАМПЫ)
                //fixed3 lightDirection;
                //fixed attenuation;
                // add diffuse
                //if(unity_LightPosition[0].w == 0.0)//directional light
                //{ 
                //  attenuation = 2;
                //   lightDirection = normalize(mul(unity_LightPosition[0],UNITY_MATRIX_IT_MV).xyz);
                //}
                //else// point or spot light
                //{
                //   lightDirection = normalize(mul(unity_LightPosition[0],UNITY_MATRIX_IT_MV).xyz - v.vertex.xyz);
                //   attenuation = 1.0/(length(mul(unity_LightPosition[0],UNITY_MATRIX_IT_MV).xyz - v.vertex.xyz)) * 0.5;
                //}
                //fixed3 normalDirction = normalize(v.normal);
                //fixed3 diffuseLight =  unity_LightColor[0].xyz * max(dot(normalDirction,lightDirection),0);
                // combine the lights (diffuse + ambient)
                //o.color.xyz = diffuseLight * attenuation + UNITY_LIGHTMODEL_AMBIENT;
                //ОСВЕЩЕНИЕ И НАПРАВЛЕНИЕ СВЕТА (ГЛОБАЛЬНОЕ ИЛИ ТОЧЕЧНОЕ ИЛИ ОТ ЛАМПЫ)
                
               #ifdef CUSTOM_DIFFUSE_LIGHT
	               #ifdef DIRECTIONAL
	               // ОСВЕЩЕНИЕ И НАПРАВЛЕНИЕ СВЕТА 
						float3 normalDirection = normalize(float3(mul(float4(v.normal, 0.0f), modelMatrixInverse)));
						float3 lightDirection = normalize(float3(_WorldSpaceLightPos0));
						float3 diffuseReflection = float3(_LightColor0) * max(0.0f, dot(normalDirection, lightDirection));
						o.color = float4(diffuseReflection, 1.0f) + UNITY_LIGHTMODEL_AMBIENT;
				   // ОСВЕЩЕНИЕ И НАПРАВЛЕНИЕ СВЕТА 
	               #endif
               #endif
	            //////////////////
				return o;
			}
			
			fixed4 frag(fragmentInput i) : COLOR
			{
				// СЧИТЫВАНИЕ ЦВЕТА С СНОВНОЙ ТЕКСТУРЫ
				fixed4 color = tex2D(_MainTex, i.uv);
				// СЧИТЫВАНИЕ ЦВЕТА С СНОВНОЙ ТЕКСТУРЫ
				
				// ДЕКОДИРОВАНИЯ КАРТЫ ОСВЕЩЕНИЯ
				#ifdef LIGHTMAP_ON
					fixed3 lm = DecodeLightmap (tex2D (unity_Lightmap, i.uvLM));
					color.rgb *= lm;
				#endif
				// ДЕКОДИРОВАНИЯ КАРТЫ ОСВЕЩЕНИЯ
				
				///////////////////////////		
				#ifdef CUSTOM_SPECULAR_LIGHT	
				
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
		            
	            #endif
				//////////////////////////				
				
				// ОСВЕЩЕНИЕ И СИЛА СВЕТА 
				#ifndef CUSTOM_SPECULAR_LIGHT	
					#ifdef DIRECTIONAL
						float3 lightColor = _LightColor0.rgb;
				        float3 lightDir = _WorldSpaceLightPos0;
				        
				        float3 n = float3(0.0f, 1.0f, 0.0f);
				        float  sat = saturate(dot(n, lightDir));
				 
				        color.rgb *= lightColor * sat;
			        #endif
		        #endif
		        // ОСВЕЩЕНИЕ И СИЛА СВЕТА 
		        
				#ifndef SHADOWS_OFF
					#ifdef DIRECTIONAL
					
					// КАСТОМНАЯ РЕАЛИЗАЦИЯ ФУНКЦИИ unitySampleShadow (УЖЕ НЕ ТРЕБУЕТСЯ, ОСТАВИЛ НА ВСЯКИЙ ПОЖАРНЫЙ)
					//fixed3 t = i._ShadowCoord.xyw;
					//fixed2 ti = t.xy / t.z;
					//fixed shadow = tex2D(_ShadowMapTexture, ti).r;
					//color.rgb *= shadow;
					// КАСТОМНАЯ РЕАЛИЗАЦИЯ ФУНКЦИИ unitySampleShadow (УЖЕ НЕ ТРЕБУЕТСЯ, ОСТАВИЛ НА ВСЯКИЙ ПОЖАРНЫЙ)
					
					
					#ifdef CUSTOM_SPECULAR_LIGHT
						color.rgb *= unitySampleShadow(i._ShadowCoord);
					#else
						// ТЕНЬ (СИЛА ЗАВИСИТ ОТ СВЕТА), sat - СИЛА СВЕТА
						color.rgb *= unitySampleShadow(i._ShadowCoord) * sat;
						// ТЕНЬ
					#endif
					
					
					#endif
				#endif
				
				#ifdef CUSTOM_REFLECTIVE
				// РЕФЛЕКСИЯ КУБИКА
				float3 reflectedDir = reflect(i.viewDir, normalize(i.normalDir));
            	color += texCUBE(_Cube, reflectedDir) * _ReflectVisible;
				// РЕФЛЕКСИЯ КУБИКА
				#endif
				
				
				#ifdef CUSTOM_SPECULAR_LIGHT
		            return color;
	            #else
	            	#ifdef CUSTOM_DIFFUSE_LIGHT
	            		return color * i.color;
	            	#else
	            		return color;
	            	#endif
	            #endif
			}
			
			ENDCG
        } // end pass 
        
	} 
	FallBack "Curve/Diffuse Curve"
	CustomEditor "CurveMaterialEditor"
}
