// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/PlasmaTubeEffect" {
	Properties {
		[Header(General Settings)] _Zoom ("Zoom (Scale)", Range(0.1, 5)) = 1
		_MainAlpha ("Global Transparency", Range(0, 1)) = 1
		[Header(Tube Geometry)] _TubeAngle ("Tube Angle (Degrees)", Range(-180, 180)) = 0
		_TubeWidth ("Tube Width", Range(0, 1)) = 0.3
		_TubeEdgeSoftness ("Tube Edge Blur (Alpha)", Range(0.001, 0.5)) = 0.1
		_TubeEdgeColorWidth ("Edge Color Width", Range(0, 1)) = 0.5
		[Header(Tube Colors)] _TubeColorA ("Tube Color 1 (Dark)", Color) = (0,0,0.5,1)
		_TubeColorB ("Tube Color 2 (Mid)", Color) = (0,0.5,1,1)
		_TubeColorC ("Tube Color 3 (Light)", Color) = (0.5,0.8,1,1)
		_TubeEdgeColor ("Tube Edge Color", Color) = (1,0,0.5,1)
		_TubeSpeed ("Tube Speed", Float) = 2
		[Header(Background Colors)] _BGColorA ("BG Color 1", Color) = (0.1,0,0.1,1)
		_BGColorB ("BG Color 2", Color) = (0.2,0,0.3,1)
		_BGColorC ("BG Color 3", Color) = (0.4,0,0.6,1)
		_BGSpeed ("BG Speed", Float) = 0.5
		[Header(Noise Distortion)] _NoiseScale ("Noise Scale", Float) = 8
		_Stretch ("Speed Stretch (Motion Blur)", Range(1, 20)) = 1
		_Distortion ("Warp Strength", Float) = 0.5
		_NoiseContrast ("Noise Contrast", Range(0.5, 3)) = 1.2
		[Header(Center Orb)] _OrbSize ("Orb Size", Range(0, 0.5)) = 0.1
		_OrbSoftness ("Orb Edge Blur", Range(0.001, 0.5)) = 0.05
		_OrbColor ("Orb Color (HDR)", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _Zoom, _MainAlpha, _TubeAngle, _TubeWidth, _TubeEdgeSoftness, _TubeEdgeColorWidth, _TubeSpeed, _BGSpeed;
			float _NoiseScale, _Stretch, _Distortion, _NoiseContrast, _OrbSize, _OrbSoftness;
			float4 _TubeColorA, _TubeColorB, _TubeColorC, _TubeEdgeColor, _BGColorA, _BGColorB, _BGColorC, _OrbColor;

			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			float plasmaNoise (float2 coord) {
				float2 q = coord * _NoiseScale;
				float n = sin(q.x) + sin(q.y) + sin(q.x + q.y) + sin(length(q) * _Distortion);
				return pow(n * 0.125 + 0.5, _NoiseContrast);
			}

			float3 ramp (float n, float3 a, float3 b, float3 c) {
				return lerp(lerp(a, b, min(n * 2.0, 1.0)), c, saturate((n - 0.5) * 2.0));
			}

			fixed4 frag (v2f i) : SV_Target {
				float ang = radians(-_TubeAngle);
				float cs = cos(ang), sn = sin(ang);
				float2 p = (i.uv - 0.5) / _Zoom;
				float across = p.x * cs - p.y * sn;
				float along = (p.y * cs + p.x * sn) / _Stretch;

				float3 tube = ramp(plasmaNoise(float2(across, along + _Time.y * _TubeSpeed)), _TubeColorA.rgb, _TubeColorB.rgb, _TubeColorC.rgb);
				float edgeStart = _TubeWidth - _TubeEdgeSoftness - _TubeEdgeColorWidth * 0.2;
				tube = lerp(tube, _TubeEdgeColor.rgb, smoothstep(edgeStart, _TubeWidth, abs(across)));

				float3 bg = ramp(plasmaNoise(float2(across, along + _Time.y * _BGSpeed)), _BGColorA.rgb, _BGColorB.rgb, _BGColorC.rgb);
				float3 col = lerp(bg, tube, smoothstep(0.0, -_TubeEdgeSoftness, abs(across) - _TubeWidth));

				float orb = smoothstep(0.0, -_OrbSoftness, length(p) - _OrbSize) * _OrbColor.a;
				return float4(lerp(col, _OrbColor.rgb, orb), _MainAlpha);
			}
			ENDCG
		}
	}
}
