// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/LiquidParticlesSky" {
	Properties {
		[Header(Texturas)] _ParticleTex ("Patron de Particulas (Blanco y Negro)", 2D) = "black" {}
		_NoiseTex ("Ruido del Liquido (Distorsion)", 2D) = "gray" {}
		[Header(Colores)] _BgColor ("Color del Liquido (Fondo)", Color) = (0,0.2,0.4,1)
		_ParticleColor ("Color de las Particulas", Color) = (0.6,0.9,1,1)
		[Header(Animacion y Control)] _SpeedX ("Velocidad Horizontal", Float) = 0.02
		_SpeedY ("Velocidad Vertical", Float) = 0.05
		_Distortion ("Fuerza de la Viscosidad", Range(0, 0.5)) = 0.08
		_Scale ("Escala General", Float) = 1
		_Cutoff ("Densidad (Corte)", Range(0, 1)) = 0.5
		_EdgeSoftness ("Suavidad del Borde", Range(0.001, 0.5)) = 0.05
	}
	SubShader {
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
		Pass {
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _NoiseTex, _ParticleTex;
			half4 _BgColor, _ParticleColor;
			half _SpeedX, _SpeedY, _Distortion, _Scale, _Cutoff, _EdgeSoftness;

			struct v2f { float4 pos : SV_POSITION; half2 uv : TEXCOORD0; };

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy * _Scale;
				return o;
			}

			half4 frag (v2f i) : SV_Target {
				float2 scroll = _Time.y * float2(_SpeedX, _SpeedY);
				half2 noise = tex2D(_NoiseTex, i.uv + scroll * 0.6).xy * 2.0 - 1.0;
				half particle = tex2D(_ParticleTex, i.uv + scroll + noise * _Distortion).r;
				half m = smoothstep(_Cutoff, _Cutoff + _EdgeSoftness, particle);
				return lerp(_BgColor, _ParticleColor, m);
			}
			ENDCG
		}
	}
}
