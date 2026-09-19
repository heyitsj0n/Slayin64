// tk2d Depth Mask -> STENCIL mask (Slayin64 2026-09-19). The original depth-buffer mask hid EVERYTHING
// behind it inside its rectangle: in the tavern the two scroll masks sit in front of the title bar, the
// description labels and the background, so on Unity 2022 they rendered as solid black bars. Now the mask
// only marks its rectangle in the stencil buffer (no colour, no depth); ScrollMaskFix gives the scroll
// content a "tk2d/BlendVertexColorMasked" material that refuses to draw where the stencil is set, so only
// the scrolling items are clipped and everything else shows through.
Shader "tk2d/Depth Mask" {
SubShader {
	Tags {"Queue"="Geometry-10"}
	Lighting Off
	ZTest Always
	ZWrite Off
	ColorMask 0
	Stencil { Ref 1 Comp Always Pass Replace }
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		struct v2f { float4 pos : SV_POSITION; };
		v2f vert(float4 vertex : POSITION) { v2f o; o.pos = UnityObjectToClipPos(vertex); return o; }
		fixed4 frag(v2f i) : SV_Target { return fixed4(0,0,0,0); }
		ENDCG
	}
}
}
