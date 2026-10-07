// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/SurfaceReflections" {
	Properties {
		[Header(Configuracion del Fluido)] _DistortionStrength ("Deformacion de Reflejos", Range(0, 0.2)) = 0.05
		_DistortionSpeed ("Velocidad del Fluido", Range(0, 10)) = 2
		_DistortionScale ("Escala de las Ondas", Range(1, 50)) = 15
		[Header(Configuracion de Reflejos)] _ReflectionColor ("Color del Reflejo", Color) = (1,1,1,1)
		_ReflectionThreshold ("Agudeza del Reflejo (Sharpness)", Range(1, 100)) = 40
		_ReflectionIntensity ("Intensidad del Reflejo", Range(0, 5)) = 1.5
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		Pass {
			Blend One One
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _DistortionStrength, _DistortionSpeed, _DistortionScale, _ReflectionThreshold, _ReflectionIntensity;
			float4 _ReflectionColor;

			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				float t = _Time.y * _DistortionSpeed;
				float s = _DistortionScale;
				float diag = sin((i.uv.x + i.uv.y) * s * 0.5 - t * 0.8);
				float2 wave = diag + float2(sin(i.uv.x * s + t), cos(i.uv.y * s + t));
				float2 p = i.uv + wave * _DistortionStrength;

				float v = sin(p.x * s * 2.0 + t * 1.5) * cos(p.y * s * 2.0 + t * 1.2);
				float r = pow(v * 0.5 + 0.5, _ReflectionThreshold) * _ReflectionIntensity;
				return r * _ReflectionColor;
			}
			ENDCG
		}
	}
}
