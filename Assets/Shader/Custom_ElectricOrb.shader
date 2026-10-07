// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/ElectricOrb" {
	Properties {
		[Header(Configuracion General)] _TotalOpacity ("Opacidad General", Range(0, 1)) = 1
		[Header(Colores HDR)] _CoreColor ("Color del Nucleo", Color) = (1,1,1,1)
		_ElecColor ("Color Electricidad", Color) = (0.2,0.8,1,1)
		[Header(Forma)] _CoreSize ("Radio del Orbe", Range(0, 0.5)) = 0.15
		_CoreBlur ("Suavizado del Orbe", Range(0, 0.2)) = 0.05
		[Header(Electricidad)] _NoiseScale ("Escala Ruido (Cantidad Rayos)", Float) = 20
		_Jaggedness ("Quebrado (ZigZag)", Float) = 10
		_Speed ("Velocidad Animacion", Float) = 10
		_RayThinness ("Finez de Rayos (Alto=Fino)", Float) = 20
		_Flicker ("Parpadeo", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		Pass {
			Blend SrcAlpha One
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _TotalOpacity, _CoreSize, _CoreBlur, _NoiseScale, _Jaggedness, _Speed, _RayThinness, _Flicker;
			float4 _CoreColor, _ElecColor;

			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			float hash (float2 p) {
				return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
			}

			float valueNoise (float2 p) {
				float2 i = floor(p), f = frac(p);
				float2 u = f * f * (3.0 - 2.0 * f);
				float a = hash(i), b = hash(i + float2(1, 0)), c = hash(i + float2(0, 1)), d = hash(i + 1.0);
				return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
			}

			fixed4 frag (v2f i) : SV_Target {
				float2 c = i.uv * 2.0 - 1.0;
				float dist = length(c);
				float angle = atan2(c.y, c.x);

				float n = valueNoise(float2(angle * _NoiseScale, dist * _Jaggedness - _Time.y * _Speed));
				float ray = pow(max(1.0 - abs(n - 0.5) * 2.0, 0.0), _RayThinness);
				ray *= lerp(1.0, frac(sin(_Time.y * 259.796) * 43758.5453), _Flicker);

				float3 elec = ray * _ElecColor.rgb;
				elec *= smoothstep(0.0, 0.05, dist - _CoreSize);
				elec *= 1.0 - smoothstep(0.6, 1.0, dist);
				float core = 1.0 - smoothstep(0.0, _CoreBlur, dist - _CoreSize);

				return float4((core * _CoreColor.rgb + elec) * _TotalOpacity, _TotalOpacity);
			}
			ENDCG
		}
	}
}
