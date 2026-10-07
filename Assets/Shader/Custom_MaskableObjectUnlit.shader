// Reconstructed from the shipped GLES3 program (data.unity3d): hidden where Custom/MaskStencil wrote 10.
Shader "Custom/MaskableObjectUnlit" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_BaseMap ("Base Map", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		Pass {
			Stencil {
				Ref 10
				Comp NotEqual
			}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _Color;

			struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; float4 color : COLOR; };
			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; fixed4 color : COLOR; };

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color * _Color;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				return fixed4(i.color.rgb, tex2D(_MainTex, i.uv).a * i.color.a);
			}
			ENDCG
		}
	}
}
