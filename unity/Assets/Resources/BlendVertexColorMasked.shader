// tk2d/BlendVertexColor + stencil test: draws only where no tk2d/Depth Mask quad marked the pixel.
// Used by ScrollMaskFix for the tavern's scrollable shop content (Slayin64 2026-09-19).
Shader "tk2d/BlendVertexColorMasked" {
Properties { _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {} }
SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	ZWrite Off Lighting Off Cull Off Fog { Mode Off } Blend SrcAlpha OneMinusSrcAlpha
	Stencil { Ref 1 Comp NotEqual Pass Keep }
	LOD 110
	Pass {
		CGPROGRAM
		#pragma vertex vert_vct
		#pragma fragment frag_mult
		#pragma fragmentoption ARB_precision_hint_fastest
		#include "UnityCG.cginc"
		sampler2D _MainTex; float4 _MainTex_ST;
		struct vin_vct { float4 vertex : POSITION; float4 color : COLOR; float2 texcoord : TEXCOORD0; };
		struct v2f_vct { float4 vertex : SV_POSITION; fixed4 color : COLOR; float2 texcoord : TEXCOORD0; };
		v2f_vct vert_vct(vin_vct v) { v2f_vct o; o.vertex = UnityObjectToClipPos(v.vertex); o.color = v.color; o.texcoord = v.texcoord; return o; }
		fixed4 frag_mult(v2f_vct i) : SV_Target { fixed4 col = tex2D(_MainTex, i.texcoord) * i.color; return col; }
		ENDCG
	}
}
}
