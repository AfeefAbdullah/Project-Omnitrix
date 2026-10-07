// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/TransformationStripes" {
	Properties {
		[Header(Colores)] _VoidColor ("Color Vacio (Fondo Global)", Color) = (0,0,0,1)
		_StreamColor ("Color Liquido (Fondo Franja)", Color) = (0,0.2,0,1)
		_CellColor ("Color Celulas", Color) = (0.2,1,0.4,1)
		[Header(Configuracion de Franjas)] _StreamCount ("Cantidad de Corrientes", Float) = 3
		_StreamWidth ("Ancho de Corriente", Range(0.1, 1)) = 0.8
		_StreamEdgeSoftness ("Suavizado Borde Corriente", Range(0, 0.5)) = 0.1
		[Header(Distorsion)] _StreamDistortion ("Intensidad Distorsion", Range(0, 0.5)) = 0.1
		_DistortionSpeed ("Velocidad Ondulacion", Float) = 1
		[Header(Comportamiento Celulas)] _CellDensity ("Densidad (Escala)", Float) = 5
		_CellSharpness ("Nitidez Celula", Range(0.001, 0.5)) = 0.02
		_CellSizeControl ("Tamano Base Celulas", Range(0, 1)) = 0.4
		_CellShrinkFalloff ("Radio de Encogimiento", Range(0.1, 1)) = 0.6
		[Header(Velocidades)] _FlowSpeed ("Velocidad de Flujo", Float) = 1.5
		_MorphSpeed ("Velocidad de Morfosis", Float) = 2
	}
	SubShader {
		Tags { "Queue"="Background" "RenderType"="Opaque" "PreviewType"="Plane" }
		LOD 100
		Pass {
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _VoidColor, _StreamColor, _CellColor;
			float _StreamCount, _StreamWidth, _StreamEdgeSoftness, _StreamDistortion, _DistortionSpeed;
			float _CellDensity, _CellSharpness, _CellSizeControl, _CellShrinkFalloff, _FlowSpeed, _MorphSpeed;

			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy;
				return o;
			}

			float2 hash2 (float2 p) {
				return frac(sin(float2(dot(p, float2(127.1, 311.7)), dot(p, float2(269.5, 183.3)))) * 43758.5453);
			}

			fixed4 frag (v2f i) : SV_Target {
				float x = i.uv.x + sin(i.uv.y * 3.0 + _Time.y * _DistortionSpeed) * _StreamDistortion;
				float s = x * _StreamCount;
				float streamId = floor(s);
				float f = frac(s);

				float halfWidth = _StreamWidth * 0.5;
				float d = abs(f - 0.5) - halfWidth;
				float stream = smoothstep(0.0, -_StreamEdgeSoftness, d);
				float shrink = smoothstep(0.0, -halfWidth * _CellShrinkFalloff, d);

				// Animated Voronoi cells, offset per stream so neighbouring streams differ.
				float2 p = float2(f * _CellDensity, i.uv.y * _StreamCount * _CellDensity + _Time.y * _FlowSpeed);
				float2 ip = floor(p), fp = frac(p);
				float morph = _Time.y * _MorphSpeed;
				float minDist = 1.0;
				for (int y = -1; y <= 1; y++)
				for (int xo = -1; xo <= 1; xo++) {
					float2 n = float2(xo, y);
					float2 pt = sin(hash2(ip + n + float2(streamId, 0)) * 6.2831 + morph) * 0.5 + n;
					minDist = min(minDist, length(pt - fp + 0.5));
				}
				float cells = smoothstep(0.0, _CellSharpness, (1.0 - minDist) * shrink - _CellSizeControl) * stream;

				fixed4 bg = lerp(_VoidColor, _StreamColor, stream);
				return lerp(bg, _CellColor, cells);
			}
			ENDCG
		}
	}
}
