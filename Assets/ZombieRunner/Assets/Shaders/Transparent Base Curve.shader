Shader "Curve/Transparent/Transparent Base Curve" {
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
			#include "UnityCG.cginc"
						
			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform half4 _MainTex_ST;
			uniform half4 _MainTex_TexelSize;
			uniform float4 _NearCurve;
			uniform float4 _FarCurve;
			uniform float _Dist;
			
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
				float4 color = tex2D(_MainTex, i.uv) * _Color;
				return color;
			}
			ENDCG
		}
		
	} 
	FallBack "Curve/Base Curve"
	CustomEditor "CurveMaterialEditor"
}
