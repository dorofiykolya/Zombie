Shader "Curve/Transparent/Transparent Base Curve" {
	Properties {
		_MainTex ("Main Texture", 2D) = "black" {}
		_NearCurve ("Near Curve", Vector) = (0, 0, 0, 0)
		_FarCurve ("Far Curve", Vector) = (0, 0, 0, 0)
		_Dist ("Distance Mod", Float) = 0.0
		_Alpha ("Alpha", Range (0.0, 1)) = 1
	}
	SubShader {
		Tags { "RenderType" = "Transparent" }
		LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Pass { // pass 0

			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
			//#pragma multi_compile_fwdbase LIGHTMAP_OFF LIGHTMAP_ON
			#include "UnityCG.cginc"
						
			uniform sampler2D _MainTex;
			uniform half4 _MainTex_ST;
			uniform half4 	_MainTex_TexelSize;
			uniform float4 _NearCurve;
			uniform float4 _FarCurve;
			uniform float _Dist;
			uniform float _Alpha;
			
			struct fragmentInput
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
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

				return o;
			}
			
			fixed4 frag(fragmentInput i) : COLOR
			{
				fixed4 color = tex2D(_MainTex, i.uv);
				color.a *= _Alpha; 
				return color;
			}
			
			ENDCG
        } // end pass
	} 
	FallBack "Diffuse"
	CustomEditor "CurveMaterialEditor"
}
