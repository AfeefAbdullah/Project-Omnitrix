// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/Efectos/FluidCellsCamera" {
	Properties {
		[Header(Configuracion General)] _BackgroundColor ("Color de Fondo", Color) = (0.05,0.05,0.08,1)
		[Header(Camara)] _CamZoom ("Zoom de Camara", Float) = 1
		[Header(Control de Forma)] [IntRange] _CellDensity ("Densidad (Costo Rendimiento)", Range(1, 30)) = 15
		_GlobalSpeed ("Velocidad Global", Float) = 1
		_Fusion ("Fusion (Viscosidad)", Range(0.1, 1.5)) = 0.6
		_BaseRadius ("Radio Base", Range(0.1, 0.8)) = 0.3
		_EdgeSoftness ("Suavizado Borde Exterior", Range(0.01, 0.1)) = 0.02
		[Header(Detalle Interno de Celula)] _MembraneWidth ("Ancho Membrana", Range(0, 0.3)) = 0.12
		_MembraneBlur ("Difuminado Membrana", Range(0.01, 0.2)) = 0.08
		_MembraneDarkness ("Oscuridad Membrana", Range(0, 1)) = 0.8
		_NucleusSize ("Tamano Nucleo", Range(0, 0.6)) = 0.2
		_NucleusBlur ("Difuminado Nucleo", Range(0.01, 0.5)) = 0.2
		_NucleusTint ("Tinte Nucleo (HDR)", Color) = (1.2,1.2,1.5,1)
		[Header(Capa 1 Fondo)] _Layer1Color ("Color Capa Fondo", Color) = (0.1,0.3,0.4,0.4)
		_Layer1Data ("Escala (X), Velocidad (Y)", Vector) = (10,0.4,0,0)
		[Header(Capa 2 Medio)] _Layer2Color ("Color Capa Media", Color) = (0.2,0.6,0.8,0.6)
		_Layer2Data ("Escala (X), Velocidad (Y)", Vector) = (8,0.7,0,0)
		[Header(Capa 3 Frente)] _Layer3Color ("Color Capa Frente", Color) = (0.4,0.8,1,0.9)
		_Layer3Data ("Escala (X), Velocidad (Y)", Vector) = (5,1,0,0)
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

			half4 _BackgroundColor, _NucleusTint, _Layer1Color, _Layer2Color, _Layer3Color;
			float4 _Layer1Data, _Layer2Data, _Layer3Data;
			float _CamZoom, _GlobalSpeed, _Fusion, _BaseRadius, _EdgeSoftness;
			float _MembraneWidth, _MembraneBlur, _MembraneDarkness, _NucleusSize, _NucleusBlur;
			int _CellDensity;

			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			// Signed distance to a blob of orbiting circles, merged with a polynomial smooth-min.
			float cellField (float2 p, float seed, float4 data, float time) {
				float d = 100.0;
				for (int i = 0; i < 30; i++) {
					if (i >= _CellDensity) break;
					float id = i + seed;
					float2 s = float2(id, id * 0.73);
					float h1 = dot(s, float2(127.1, 311.7)), h2 = dot(s, float2(269.5, 183.3));
					float4 r = frac(sin(float4(h2, h1, h1, h2)) * 43758.5453);

					float2 ang = r.xz * float2(12.0, 11.0) + time * data.y;
					float2 amp = r.yw * 0.5 + 0.6;
					float2 center = (r.zw - 0.5) * data.x + float2(sin(ang.x), cos(ang.y)) * amp;
					float radius = sin(i * 2.0 + time * data.y * 1.5) * 0.15 + _BaseRadius;
					float dd = length(p * data.x - center) - radius;

					float h = saturate(0.5 + 0.5 * (dd - d) / _Fusion);
					d = lerp(dd, d, h) - _Fusion * h * (1.0 - h);
				}
				return d;
			}

			half4 shadeLayer (float d, half4 color) {
				float alpha = 1.0 - smoothstep(0.0, _EdgeSoftness, d);
				float membrane = 1.0 - smoothstep(-_MembraneWidth - _MembraneBlur, -_MembraneBlur, d);
				float nucleus = 1.0 - smoothstep(-_NucleusSize - _NucleusBlur, -_NucleusSize, d);
				float3 rgb = color.rgb * (1.0 - membrane * _MembraneDarkness);
				rgb = lerp(rgb, _NucleusTint.rgb, nucleus * color.a * 0.8);
				return alpha > 0.001 ? half4(rgb, alpha * color.a) : 0;
			}

			half4 frag (v2f i) : SV_Target {
				float2 p = (i.uv - 0.5) * _CamZoom;
				float t = _Time.y * _GlobalSpeed;
				half4 l1 = shadeLayer(cellField(p, 0.0, _Layer1Data, t), _Layer1Color);
				half4 l2 = shadeLayer(cellField(p, 33.0, _Layer2Data, t), _Layer2Color);
				half4 l3 = shadeLayer(cellField(p, 77.0, _Layer3Data, t), _Layer3Color);
				half4 col = lerp(_BackgroundColor, l1, l1.a);
				col = lerp(col, l2, l2.a);
				return lerp(col, l3, l3.a);
			}
			ENDCG
		}
	}
}
