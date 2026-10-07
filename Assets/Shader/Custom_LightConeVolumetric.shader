// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/LightConeVolumetric" {
	Properties {
		[Header(Configuracion Basica)] _MainColor ("Color Principal", Color) = (1,1,0,0.5)
		_EmissionColor ("Color Emisivo", Color) = (1,1,0,1)
		_EmissionPower ("Intensidad Emision", Range(0, 10)) = 1
		[Header(Geometria)] _TopScale ("Escala Cara Superior", Range(0, 5)) = 1
		_BottomScale ("Escala Cara Inferior", Range(0, 5)) = 0.2
		[Header(Desvanecimiento)] _FadeStart ("Inicio Desvanecimiento (Eje Y)", Range(0, 1)) = 0
		_FadePower ("Suavidad del Fade", Range(0.1, 5)) = 1
		[Toggle] _InvertFade ("Invertir Direccion Fade", Float) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		LOD 100
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _MainColor, _EmissionColor;
			float _EmissionPower, _TopScale, _BottomScale, _FadeStart, _FadePower, _InvertFade;

			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; float side : TEXCOORD1; };

			// Tapers a cylinder into a cone: xz scaled from _BottomScale to _TopScale along uv.y.
			v2f vert (appdata_base v) {
				v2f o;
				float s = lerp(_BottomScale, _TopScale, v.texcoord.y);
				o.pos = UnityObjectToClipPos(float4(v.vertex.x * s, v.vertex.y, v.vertex.z * s, 1.0));
				o.uv = v.texcoord.xy;
				o.side = abs(v.normal.y) >= 0.9 ? 0.0 : 1.0; // caps are discarded
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				if (i.side < 0.5) discard;
				float f = _InvertFade > 0.5 ? 1.0 - i.uv.y : i.uv.y;
				f = pow(smoothstep(_FadeStart, 1.0, f), _FadePower);
				return float4(_MainColor.rgb + _EmissionColor.rgb * _EmissionPower, f * _MainColor.a);
			}
			ENDCG
		}
	}
}
