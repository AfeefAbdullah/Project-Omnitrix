// Reconstructed from the shipped GLES3 program (data.unity3d): HologramShader plus a depth pre-pass.
Shader "SFHologram/Holograma" {
	Properties {
		_Brightness ("Brightness", Range(0.1, 6)) = 3
		_Alpha ("Alpha", Range(0, 1)) = 1
		_Direction ("Direction", Vector) = (0,1,0,0)
		_MainTex ("MainTexture", 2D) = "white" {}
		_MainColor ("MainColor", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (1,1,1,1)
		_RimPower ("Rim Power", Range(0.1, 10)) = 5
		_ScanTiling ("Scan Tiling", Range(0.01, 50)) = 0.05
		_ScanSpeed ("Scan Speed", Range(-2, 2)) = 1
		_GlowTiling ("Glow Tiling", Range(0.01, 1)) = 0.05
		_GlowSpeed ("Glow Speed", Range(-10, 10)) = 1
		_GlitchSpeed ("Glitch Speed", Range(0, 50)) = 1
		_GlitchIntensity ("Glitch Intensity", Float) = 0
		_FlickerTex ("Flicker Control Texture", 2D) = "white" {}
		_FlickerSpeed ("Flicker Speed", Range(0.01, 100)) = 1
		_Fold ("__fld", Float) = 1
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 100
		Pass {
			ColorMask 0

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment fragDepth
			#pragma shader_feature _GLITCH_ON
			#include "SFHologram.cginc"
			ENDCG
		}
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma shader_feature _SCAN_ON
			#pragma shader_feature _GLOW_ON
			#pragma shader_feature _GLITCH_ON
			#include "SFHologram.cginc"
			ENDCG
		}
	}
}
