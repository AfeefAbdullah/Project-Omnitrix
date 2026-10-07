// Shared by SFHologram/HologramShader and SFHologram/Holograma (their shipped GLES3 programs are identical).
#include "UnityCG.cginc"

sampler2D _MainTex, _FlickerTex;
float4 _MainTex_ST, _Direction, _MainColor, _RimColor;
float _RimPower, _Brightness, _Alpha, _ScanTiling, _ScanSpeed, _GlowTiling, _GlowSpeed;
float _GlitchSpeed, _GlitchIntensity, _FlickerSpeed;

struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float4 worldPos : TEXCOORD1;
	float3 viewDir : TEXCOORD2;
	float3 normal : NORMAL;
};

v2f vert (appdata_base v) {
	#ifdef _GLITCH_ON
	// Rare horizontal jolt: only while sin(t) peaks, and only on alternating bands of y.
	v.vertex.x += _GlitchIntensity * step(0.99, sin(_Time.y * _GlitchSpeed * 0.5)) * step(0.5, sin(_Time.y * 2.0 + v.vertex.y));
	#endif
	v2f o;
	o.worldPos = mul(unity_ObjectToWorld, v.vertex);
	o.pos = UnityWorldToClipPos(o.worldPos.xyz);
	o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
	o.viewDir = normalize(_WorldSpaceCameraPos - o.worldPos.xyz);
	o.normal = UnityObjectToWorldNormal(v.normal);
	return o;
}

fixed4 frag (v2f i) : SV_Target {
	float proj = (dot(i.worldPos, normalize(float4(_Direction.xyz, 1.0))) + 1.0) * 0.5;
	float rimBase = 1.0 - saturate(dot(i.viewDir, i.normal));
	float shape = rimBase;
	float3 glowRgb = 0;

	#ifdef _SCAN_ON
	shape += step(frac(proj * _ScanTiling + _Time.w * _ScanSpeed), 0.5) * 0.65;
	#endif
	#ifdef _GLOW_ON
	float glow = frac(proj * _GlowTiling - _Time.x * _GlowSpeed);
	shape += glow;
	glowRgb = glow * 0.35 * _MainColor.rgb;
	#endif

	fixed4 tex = tex2D(_MainTex, i.uv);
	float3 rgb = (tex.rgb * _MainColor.rgb + glowRgb + _RimColor.rgb * pow(rimBase, _RimPower)) * _Brightness;
	float flicker = tex2D(_FlickerTex, _Time.xy * _FlickerSpeed).r;
	return fixed4(rgb, tex.a * _Alpha * shape * flicker);
}

fixed4 fragDepth (v2f i) : SV_Target { return 0; }
