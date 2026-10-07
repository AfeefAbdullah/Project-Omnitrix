// Reconstructed from the shipped GLES3 program (data.unity3d).
Shader "Custom/XRayFlash" {
	Properties {
		_MainTint ("Tinte del Flash (Azul)", Color) = (0,0.6,1,1)
		_RimColor ("Color del Borde", Color) = (1,1,1,1)
		_RimPower ("Potencia Fresnel (Borde)", Range(0.5, 8)) = 3
		_Brightness ("Intensidad del Brillo", Range(1, 10)) = 2
		_Frequency ("Frecuencia (Velocidad)", Range(0.1, 20)) = 1
		_Duration ("Duracion del Pulso (Ancho)", Range(0.01, 0.99)) = 0.3
		_AttackSoftness ("Suavidad de Ataque (Fade)", Range(0.01, 1)) = 0.2
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		GrabPass { "_BackgroundTexture" }
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _BackgroundTexture;
			half4 _MainTint, _RimColor;
			float _RimPower, _Brightness, _Frequency, _Duration, _AttackSoftness;

			struct v2f {
				float4 pos : SV_POSITION;
				float4 grabPos : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				float3 normal : NORMAL;
			};

			v2f vert (appdata_base v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.grabPos = ComputeGrabScreenPos(o.pos);
				o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target {
				float pulse = smoothstep(1.0 - _Duration, 1.0 - _Duration + _AttackSoftness, sin(_Time.y * _Frequency) * 0.5 + 0.5);
				float3 rim = pow(1.0 - saturate(dot(i.viewDir, i.normal)), _RimPower) * _RimColor.rgb;
				float3 bg = tex2Dproj(_BackgroundTexture, i.grabPos).rgb;
				float3 xray = dot(bg, float3(0.3, 0.59, 0.11)) * _MainTint.rgb * _Brightness + rim;
				return fixed4(lerp(bg, xray, pulse), 1.0);
			}
			ENDCG
		}
	}
}
