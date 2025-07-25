Shader "Draw/OutlineShader_BlueGlow" {
	Properties {
		_OutlineColor ("Outline Color", Color) = (0,1,1,1) // 푸른빛
		_Outline ("Outline width", Range (0, 1)) = .1
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float4 color : COLOR;
	};

	uniform float _Outline;
	uniform float4 _OutlineColor;

	v2f vert(appdata v) {
		v2f o;

		// 외곽선 확장 (기존 방식)
		v.vertex *= (1 + _Outline);
		o.pos = UnityObjectToClipPos(v.vertex);

		// 푸른빛 강조
		o.color = _OutlineColor * 1.5;
		return o;
	}
	ENDCG

	SubShader {
		Tags { "DisableBatching" = "True" "Queue" = "Transparent" }

		Pass {
			Name "OUTLINE"
			Tags { "LightMode" = "Always" }
			Cull Front
			ZWrite Off
			ColorMask RGB
			Blend SrcAlpha One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) : SV_Target {
				return half4(i.color.rgb, 0.5); // 푸른빛 Glow
			}
			ENDCG
		}
	}

	Fallback "Diffuse"
}
