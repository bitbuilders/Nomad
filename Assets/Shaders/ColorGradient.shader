Shader "Custom/ColorGradient" {
	Properties{
		_MainTex("Sprite Texture", 2D) = "white" {}
		_ColorTopLeft("Top Left Color", Color) = (1,1,1,1)
		_ColorTopRight("Top Right Color", Color) = (1,1,1,1)
		_ColorBot("Bot Color", Color) = (1,1,1,1)

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
		ZWrite Off
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

			fixed4 frag(v2f i) : COLOR {
				//fixed4 c = lerp(_ColorBot, _ColorMid, i.texcoord.y / _Middle) * step(i.texcoord.y, _Middle);
				//c += lerp(_ColorMid, _ColorTop, (i.texcoord.y - _Middle) / (1 - _Middle)) * step(_Middle, i.texcoord.y);
				//fixed4 c = lerp(_ColorBot, _ColorTopLeft, i.texcoord.y / 0.5f) * step(i.texcoord.y, 0.5f);
				//c += lerp(_ColorTopLeft, _ColorTopRight, (i.texcoord.y - 0.5f) / (1 - 0.5f)) * step(0.5f, i.texcoord.y);
				fixed4 c = lerp(_ColorTopLeft, _ColorTopRight, pow(i.texcoord.x, 1));
				c = lerp(c, _ColorBot, pow(clamp(1.0f - i.texcoord.y, 0.0f, 1.0f), 1));
				c.a = 1;
				return c;
			}
			ENDCG
			}
		}
}