// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/ShineEffect" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		[Header(Control Manual)] _ShineLocation ("Posicion del Brillo (0 a 1)", Range(0, 1)) = 0
		[Header(Configuracion del Brillo)] _ShineColor ("Color del Brillo", Color) = (1,1,1,1)
		_ShineWidth ("Grosor de la Franja", Range(0, 1)) = 0.1
		_ShineSoftness ("Suavidad de Bordes", Range(0.01, 1)) = 0.1
		_Slope ("Inclinacion", Range(-5, 5)) = 1
		[Header(Visibilidad del Objeto)] _BaseOpacity ("Opacidad Base (0 = Solo Luz)", Range(0, 1)) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _ShineColor;
			float _ShineWidth, _ShineLocation, _ShineSoftness, _Slope, _BaseOpacity;

			struct appdata { float4 vertex : POSITION; float4 color : COLOR; float2 uv : TEXCOORD0; };
			struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				// Sweep the band from fully off one edge to fully off the other as _ShineLocation goes 0 -> 1.
				float center = _ShineLocation * (abs(_Slope) + 1.0 + 2.0 * _ShineWidth) - _ShineWidth;
				float coord = i.uv.y * _Slope + i.uv.x;
				float shine = 1.0 - smoothstep(0.0, _ShineSoftness, abs(coord - center) - _ShineWidth);

				fixed4 tex = tex2D(_MainTex, i.uv) * i.color;
				shine *= _ShineColor.a * tex.a;
				float baseA = tex.a * _BaseOpacity;
				float alpha = saturate(baseA + shine);
				float3 rgb = alpha > 0.0001 ? (tex.rgb * baseA + shine * _ShineColor.rgb) / alpha : 0;
				return float4(rgb, alpha);
			}
			ENDCG
		}
	}
}
