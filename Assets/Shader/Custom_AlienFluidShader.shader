// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/AlienFluidShader" {
	Properties {
		_MainTex ("Texture (Sprite/Base)", 2D) = "white" {}
		_Color ("Tint Color", Color) = (1,1,1,1)
		[Header(Colores Ben 10)] _MainColor ("Color Manchas", Color) = (0,1,0,1)
		_BackgroundColor ("Color Fondo", Color) = (0,0.05,0,1)
		[Header(Configuracion)] _GridDensity ("Densidad", Float) = 5
		_CellSize ("Tamano Celula", Range(0, 1.2)) = 0.5
		_Softness ("Suavidad", Range(0.001, 0.5)) = 0.05
		[Header(Movimiento)] _Speed ("Velocidad", Float) = 1
		_Distortion ("Distorsion", Float) = 0.5
		[Header(Opciones 3D)] [Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 2
		[Enum(Off,0,On,1)] _ZWrite ("ZWrite", Float) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite [_ZWrite]
			Cull [_Cull]

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _MainTex_ST;
			fixed4 _Color, _MainColor, _BackgroundColor;
			float _GridDensity, _Speed, _Distortion;
			half _CellSize, _Softness;

			struct appdata { float4 vertex : POSITION; float4 color : COLOR; float2 uv : TEXCOORD0; };
			struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };

			v2f vert (appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color * _Color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			float2 hash2 (float2 p) {
				return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
			}

			fixed4 frag (v2f i) : SV_Target {
				float t = _Time.y * _Speed;
				float2 p = i.uv * _GridDensity;
				p.x += sin(p.y * 2.0 + t * 0.5) * _Distortion;
				p.y += cos(p.x * 2.0 + t * 0.3) * _Distortion;

				float2 ip = floor(p), fp = frac(p);
				float minDistSq = 1.0;
				for (int y = -1; y <= 1; y++)
				for (int x = -1; x <= 1; x++) {
					float2 n = float2(x, y);
					float2 d = sin(hash2(ip + n) * 6.2831 + t) * 0.5 + n - fp + 0.5;
					minDistSq = min(minDistSq, dot(d, d));
				}
				float cell = 1.0 - sqrt(minDistSq);
				half edge = 1.0 - _CellSize;
				half m = smoothstep(edge - _Softness, edge + _Softness, cell);
				return lerp(_BackgroundColor, _MainColor, m) * i.color;
			}
			ENDCG
		}
	}
}
