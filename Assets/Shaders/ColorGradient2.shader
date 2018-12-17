Shader "Custom/ColorGradient2" {
	Properties{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite on
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

			Pass {
			CGPROGRAM
			#pragma vertex vert  
			#pragma fragment frag
			#include "UnityCG.cginc"

			fixed4 _ColorTopLeft;
			fixed4 _ColorTopRight;
			fixed4 _ColorBot;

			struct v2f {
				float4 pos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
			};

			v2f vert(appdata_full v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : COLOR{
				fixed4 red =		fixed4(1.0f, 0.0f, 0.0f, 0.0f);
				fixed4 purple =		fixed4(1.0f, 0.0f, 1.0f, 0.0f);
				fixed4 blue =		fixed4(0.0f, 0.0f, 1.0f, 0.0f);
				fixed4 turqoise =	fixed4(0.0f, 1.0f, 1.0f, 0.0f);
				fixed4 green =		fixed4(0.0f, 1.0f, 0.0f, 0.0f);
				fixed4 yellow =		fixed4(1.0f, 1.0f, 0.0f, 0.0f);
				float sections = 1.0f / 6.0f;
				fixed4 c =	lerp(red, purple,		(i.texcoord.x / sections * 1) / (1.0f)) * step(i.texcoord.x, sections);
				c +=		lerp(purple, blue,		(i.texcoord.x - sections * 1) / (sections)) * step(sections * 1, i.texcoord.x) * step(i.texcoord.x, sections * 2);
				c +=		lerp(blue, turqoise,	(i.texcoord.x - sections * 2) / (sections)) * step(sections * 2, i.texcoord.x) * step(i.texcoord.x, sections * 3);
				c +=		lerp(turqoise, green,	(i.texcoord.x - sections * 3) / (sections)) * step(sections * 3, i.texcoord.x) * step(i.texcoord.x, sections * 4);
				c +=		lerp(green, yellow,		(i.texcoord.x - sections * 4) / (sections)) * step(sections * 4, i.texcoord.x) * step(i.texcoord.x, sections * 5);
				c +=		lerp(yellow, red,		(i.texcoord.x - sections * 5) / (sections)) * step(sections * 5, i.texcoord.x) * step(i.texcoord.x, sections * 6);
				c.a = 1.0f;
				return c;
			}
			ENDCG
			}
		}
}