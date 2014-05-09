Shader "Curve/Transparent/Transparent Diffuse Curve" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
    	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_NearCurve ("Near Curve", Vector) = (0, 0, 0, 0)
		_FarCurve ("Far Curve", Vector) = (0, 0, 0, 0)
		_Dist ("Distance Mod", Float) = 0.0
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Alphatest Greater 0 ZWrite Off ColorMask RGB
        Pass {
			
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			#pragma multi_compile_fwdbase LIGHTMAP_OFF LIGHTMAP_ON
			#pragma multi_compile UNITY_EDITOR_SHADOW_ON UNITY_EDITOR_SHADOW_OFF
			#include "UnityCG.cginc"
			#ifndef SHADOWS_OFF		
			#include "AutoLight.cginc"	
			#endif
						
			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform half4 _MainTex_ST;
			uniform half4 _MainTex_TexelSize;
			uniform float4 _NearCurve;
			uniform float4 _FarCurve;
			uniform float _Dist;
			uniform float4 _LightColor0;
			
			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				#ifndef SHADOWS_OFF
		        LIGHTING_COORDS(2,3)
				#endif
				#ifdef DIRECTIONAL
            	fixed4 color : COLOR;
            	#endif
			};
						
			fragmentInput vert(appdata_full v)
			{
				fragmentInput o;

				// Apply the curve
                float4 pos = mul(UNITY_MATRIX_MV, v.vertex);
                float distanceSquared = pos.z * pos.z * _Dist;
                pos.x += (_FarCurve.x - max(1.0 - distanceSquared / _NearCurve.x, 0.0) * _FarCurve.x);
                pos.y += (_FarCurve.y - max(1.0 - distanceSquared / _NearCurve.y, 0.0) * _FarCurve.y);
                o.pos = mul(UNITY_MATRIX_P, pos); 
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1.0-o.uv.y;
				#endif
				
				#ifndef SHADOWS_OFF			  	
      				#if UNITY_EDITOR_SHADOW_ON	 
	      			o._ShadowCoord = ComputeScreenPos(mul(UNITY_MATRIX_MVP, v.vertex));
	      			#else
	      			TRANSFER_VERTEX_TO_FRAGMENT(o);
	      			#endif
				#endif
				
				float4x4 modelMatrix = _Object2World;
            	float4x4 modelMatrixInverse = _World2Object; 
	            
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
				float4 color = tex2D(_MainTex, i.uv) * _Color;
				
				#ifndef SHADOWS_OFF			  	
				fixed atten = LIGHT_ATTENUATION(i);
				color.rgb *= atten;
				#endif
				
				#ifdef DIRECTIONAL
				float3 lightColor = _LightColor0.rgb;
		        float3 lightDir = _WorldSpaceLightPos0;
		        
		        float3 n = float3(0.0f, 1.0f, 0.0f);
		        float  sat = saturate(dot(n, lightDir));
		 
		        color.rgb *= lightColor * sat * i.color * 2;
		        #endif

				return color;
			}
			ENDCG
		}
		
	} 
	FallBack "Transparent/Diffuse"
	CustomEditor "CurveMaterialEditor"
}
