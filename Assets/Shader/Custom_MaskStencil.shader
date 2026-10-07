// Reconstructed from the shipped GLES3 program (data.unity3d): writes stencil 10, no color.
Shader "Custom/MaskStencil" {
	Properties {
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_BaseMap ("Base Map", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent-1" "RenderType"="Transparent" }
		Pass {
			Stencil {
				Ref 10
				Comp Always
				Pass Replace
			}
			Blend Zero One
			ZWrite Off
		}
	}
}
