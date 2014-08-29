Shader "Curve/Diffuse Curve" {
	Properties {
		_MainTex ("Main Texture", 2D) = "black" {}
		_NearCurve ("Near Curve", Vector) = (0, 0, 0, 0)
		_FarCurve ("Far Curve", Vector) = (0, 0, 0, 0)
		_Dist ("Distance Mod", Float) = 0.0
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
			#pragma multi_compile UNITY_EDITOR_SHADOW_ON UNITY_EDITOR_SHADOW_OFF
			#include "UnityCG.cginc"
			#ifndef SHADOWS_OFF		
			#include "AutoLight.cginc"	
			#endif
						
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform sampler2D unity_Lightmap;
			uniform half4 	_MainTex_TexelSize;
			uniform half4 unity_LightmapST;
			uniform float4 _NearCurve;
			uniform float4 _FarCurve;
			uniform float _Dist;

			uniform float4 _LightColor0;
			
			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 uvLM : TEXCOORD1;
				#ifndef SHADOWS_OFF
		        LIGHTING_COORDS(2,3)
				#endif
			};
						
			fragmentInput vert(appdata_full v)
			{
				fragmentInput o;

				// Apply the curve
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				#ifndef SHADOWS_OFF	
					#if UNITY_EDITOR_SHADOW_ON
						TRANSFER_VERTEX_TO_FRAGMENT(o);
					#endif
				#endif

				float distanceSquared = o.pos.z * o.pos.z * _Dist;
				o.pos.x += (distanceSquared / _NearCurve.x * _FarCurve.x);
				o.pos.y += (distanceSquared / _NearCurve.y * _FarCurve.y);
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uvLM = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

				#if UNITY_UV_STARTS_AT_TOP
					if (_MainTex_TexelSize.y < 0)
						o.uv.y = 1.0-o.uv.y;
				#endif

				#ifndef SHADOWS_OFF		
					#ifndef UNITY_EDITOR_SHADOW_ON	 
						TRANSFER_VERTEX_TO_FRAGMENT(o);
					#endif
				#endif

				return o;
			}
			
			fixed4 frag(fragmentInput i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv);

				#ifdef LIGHTMAP_ON
					color.rgb *= DecodeLightmap (tex2D (unity_Lightmap, i.uvLM));
				#endif
				
				#ifndef SHADOWS_OFF			  	
					color.rgb *= LIGHT_ATTENUATION(i);
				#endif

				return color;
			}
			
			ENDCG
        } // end pass
	} 
	FallBack "Diffuse"
	CustomEditor "CurveMaterialEditor"
}
