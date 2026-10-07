// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "SFHologram/HologramShader_Grid_ProceduralFlicker" {
	Properties {
		_Brightness ("Brightness", Range(0.1, 6)) = 3
		_Alpha ("Alpha", Range(0, 1)) = 1
		_Direction ("Direction", Vector) = (0,1,0,0)
		_MainTex ("MainTexture", 2D) = "white" {}
		_MainColor ("MainColor", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", Range(0.1, 10)) = 5
		[Toggle(_GRID_ON)] _EnableGrid ("Enable Grid", Float) = 1
		_GridDensity ("Grid Density (X,Y,Z)", Vector) = (15,15,15,0)
		_GridThickness ("Grid Thickness", Range(0.001, 0.5)) = 0.05
		_GridSpeed ("Grid Speed (X,Y,Z)", Vector) = (0,1,1,0)
		[Toggle(_GLOW_ON)] _EnableGlow ("Enable Glow", Float) = 1
		_GlowTiling ("Glow Tiling", Range(0.01, 1)) = 0.05
		_GlowSpeed ("Glow Speed", Range(-10, 10)) = 1
		[Toggle(_GLITCH_ON)] _EnableGlitch ("Enable Glitch", Float) = 1
		_GlitchSpeed ("Glitch Wave Speed", Range(0, 50)) = 10
		_GlitchIntensity ("Glitch Intensity", Float) = 0.2
		_GlitchFrequency ("Glitch Frequency", Range(0.1, 10)) = 1
		_GlitchDuration ("Glitch Duration", Range(0.01, 1)) = 0.1
		_FlickerFrequency ("Flicker Frequency", Range(0.1, 10)) = 0.5
		_FlickerSpeed ("Flicker Speed", Range(10, 150)) = 80
		_FlickerDuration ("Flicker Duration", Range(0.01, 1)) = 0.15
		_FlickerIntensity ("Flicker Intensity", Range(0, 1)) = 0.8
		_Fold ("__fld", Float) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100

		CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float4 _MainTex_ST, _Direction, _MainColor, _RimColor, _GridDensity, _GridSpeed;
		float _RimPower, _Brightness, _Alpha, _GridThickness, _GlowTiling, _GlowSpeed;
		float _GlitchSpeed, _GlitchIntensity, _GlitchFrequency, _GlitchDuration;
		float _FlickerFrequency, _FlickerSpeed, _FlickerDuration, _FlickerIntensity;

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float4 worldPos : TEXCOORD1;
			float3 viewDir : TEXCOORD2;
			float3 normal : NORMAL;
		};

		// 1 for the first (frequency * duration) of every 1/frequency period, else 0.
		float window (float frequency, float duration) {
			return step(frac(_Time.y * frequency), frequency * duration);
		}

		v2f vert (appdata_base v) {
			#ifdef _GLITCH_ON
			float band = step(0.5, sin(v.vertex.y * 10.0 + _Time.y * _GlitchSpeed)) * 2.0 - 1.0;
			v.vertex.x += band * _GlitchIntensity * window(_GlitchFrequency, _GlitchDuration);
			#endif
			v2f o;
			o.worldPos = mul(unity_ObjectToWorld, v.vertex);
			o.pos = UnityWorldToClipPos(o.worldPos.xyz);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos.xyz);
			o.normal = UnityObjectToWorldNormal(v.normal);
			return o;
		}
		ENDCG

		Pass {
			ColorMask 0

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragDepth
			#pragma shader_feature _GLITCH_ON
			fixed4 fragDepth (v2f i) : SV_Target { return 0; }
			ENDCG
		}
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _GRID_ON
			#pragma shader_feature _GLOW_ON
			#pragma shader_feature _GLITCH_ON

			fixed4 frag (v2f i) : SV_Target {
				float rimBase = 1.0 - saturate(dot(i.viewDir, i.normal));
				float shape = rimBase;
				float3 glowRgb = 0;

				#ifdef _GRID_ON
				// Object-centred 3D grid lines; an axis with ~0 density contributes no lines.
				float3 g = frac((i.worldPos.xyz - unity_ObjectToWorld._m03_m13_m23) * _GridDensity.xyz - _Time.y * _GridSpeed.xyz);
				float3 lines = step(g, _GridThickness) * step(0.001, abs(_GridDensity.xyz));
				shape += max(lines.x, max(lines.y, lines.z)) * 0.8;
				#endif
				#ifdef _GLOW_ON
				float proj = (dot(i.worldPos, normalize(float4(_Direction.xyz, 1.0))) + 1.0) * 0.5;
				float glow = frac(proj * _GlowTiling - _Time.x * _GlowSpeed);
				shape += glow;
				glowRgb = glow * 0.35 * _MainColor.rgb;
				#endif

				float flicker = 1.0 - window(_FlickerFrequency, _FlickerDuration) * (1.0 - (sin(_Time.y * _FlickerSpeed) + 1.0) * 0.5) * _FlickerIntensity;

				fixed4 tex = tex2D(_MainTex, i.uv);
				float3 rgb = (tex.rgb * _MainColor.rgb + glowRgb + _RimColor.rgb * pow(rimBase, _RimPower)) * _Brightness;
				return fixed4(rgb, tex.a * _Alpha * shape * flicker);
			}
			ENDCG
		}
	}
}
