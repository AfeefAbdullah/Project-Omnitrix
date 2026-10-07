// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/ShockwaveEmitter" {
	Properties {
		[Header(Base Settings)] _Color ("Color & Emision", Color) = (0,2,1,1)
		_Radius ("Progreso Expansion (Radio)", Range(0, 1.5)) = 0.5
		_Width ("Grosor de Onda", Range(0, 0.5)) = 0.05
		_Softness ("Suavizado Bordes", Range(0.001, 0.2)) = 0.02
		[Header(Zig Zag Main Shape)] _ZigZagFreq ("Frecuencia Base (Puntas)", Float) = 20
		_ZigZagAmp ("Amplitud Base (Intensidad)", Range(0, 0.5)) = 0.15
		_ZigZagSpeed ("Velocidad Rotacion Base", Float) = 2
		[Header(Irregularity Noise)] _NoiseScale ("Escala del Ruido (Caos)", Float) = 5
		_NoiseSpeed ("Velocidad del Ruido", Float) = 3
		_NoiseInfluence ("Influencia del Ruido", Range(0, 1)) = 0.8
		[Header(Blending Options)] [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 1
		[Toggle] _ZWrite ("Z Write", Float) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent+100" "RenderType"="Transparent" "IgnoreProjector"="True" }
		LOD 100
		Pass {
			Blend [_SrcBlend] [_DstBlend]
			ZWrite [_ZWrite]
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			half4 _Color;
			float _Radius, _Width, _Softness, _ZigZagFreq, _ZigZagAmp, _ZigZagSpeed, _NoiseScale, _NoiseSpeed, _NoiseInfluence;

			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				float2 c = i.uv - 0.5;
				float dist = length(c);
				float angle = atan2(c.y, c.x);
				float2 t = _Time.y * float2(_ZigZagSpeed, _NoiseSpeed);

				// Triangle wave around the ring, amplitude wobbled by two sine "noise" terms.
				float zig = (abs(frac(angle * _ZigZagFreq / UNITY_TWO_PI + t.x) - 0.5) - 0.25) * 2.0;
				float na = angle * _NoiseScale;
				float noise = sin(na * 1.1 + t.y) + cos(na * 1.7 - t.y * 0.7);
				float amp = max(noise * _NoiseInfluence * 0.5 + 1.0, 0.1) * _ZigZagAmp;
				float d = dist - zig * amp;

				float outer = smoothstep(0.0, -_Softness, d - _Radius);
				float inner = smoothstep(0.0, -_Softness, d - (_Radius - _Width));
				return (outer - inner) * _Color;
			}
			ENDCG
		}
	}
}
