// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/LightShafts" {
	Properties {
		[Header(Configuracion General)] _MainColor ("Color Principal", Color) = (1,0.8,0.4,1)
		[Header(Animacion)] _AppearProgress ("Progreso Aparicion (0 a 1)", Range(0, 1)) = 1
		[Header(Configuracion de Rayos)] _RayCount ("Cantidad de Rayos", Float) = 10
		_Speed ("Velocidad de Rotacion", Float) = 0.5
		_Density ("Grosor de Rayos", Range(0, 1)) = 0.3
		_Falloff ("Atenuacion General", Range(0.1, 10)) = 3
		[Header(Nuevo Brillo Central)] _CoreSize ("Tamano del Centro", Range(0, 0.4)) = 0.05
		_CoreBlur ("Difuminado del Borde", Range(0.01, 0.5)) = 0.15
		_CoreIntensity ("Intensidad del Centro", Float) = 2
	}
	SubShader {
		Tags { "Queue"="Overlay" "RenderType"="Transparent" }
		LOD 100
		Pass {
			Blend SrcAlpha One
			ZTest Always
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _MainColor;
			float _AppearProgress, _RayCount, _Speed, _Density, _Falloff, _CoreSize, _CoreBlur, _CoreIntensity;

			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

			// View-aligned billboard: object origin in view space plus the quad's xy scaled by the object scale.
			v2f vert (appdata_base v) {
				v2f o;
				float2 scale = float2(length(unity_ObjectToWorld._m00_m10_m20), length(unity_ObjectToWorld._m01_m11_m21));
				float4 viewPos = mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1));
				viewPos.xy += v.vertex.xy * scale;
				o.pos = mul(UNITY_MATRIX_P, viewPos);
				o.uv = v.texcoord.xy;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				float2 c = i.uv - 0.5;
				float dist = length(c);
				float angle = atan2(c.y, c.x);
				float t = _Time.y * _Speed;

				float rays = sin(angle * _RayCount + t) + sin(angle * _RayCount * 0.5 - 2.0 * t);
				rays = smoothstep(_Density, _Density + 0.2, rays);
				rays *= clamp(1.0 / (dist * _Falloff + 0.1), 0.0, 5.0);

				float core = smoothstep(_CoreSize + _CoreBlur, _CoreSize, dist) * _CoreIntensity;
				float edge = 1.0 - smoothstep(0.4, 0.7, dist);
				float shafts = rays * edge + core;

				float appear = 1.0 - smoothstep(_AppearProgress * 0.8 - 0.1, _AppearProgress * 0.8, dist);
				appear *= _AppearProgress >= 0.01 ? 1.0 : 0.0;

				return appear * shafts * _MainColor;
			}
			ENDCG
		}
	}
}
