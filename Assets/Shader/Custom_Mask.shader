// Reconstructed from the shipped GLES3 program (data.unity3d): depth-only mask, no color.
Shader "Custom/Mask" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_BaseMap ("Base Map", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent+1" }
		Pass {
			Blend Zero One
		}
	}
}
