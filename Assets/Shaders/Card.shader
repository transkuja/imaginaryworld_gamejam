// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/Card"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
		_DamagedMask("Damaged Mask", 2D) = "black" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Hp("HP", int) = 3
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0
				#pragma multi_compile_instancing
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnitySprites.cginc"
				sampler2D _Maintex;
		sampler2D _DamagedMask;
		int _Hp;
		struct appdata
		{
			float4 vertex : POSITION;
			half2 texcoord : TEXCOORD0;
		};

		struct v2f_card
		{
			float4 pos : SV_POSITION;
			half2 uv : TEXCOORD0;
		};

		v2f_card vert(appdata IN)
		{
			v2f_card OUT;

			OUT.pos = UnityObjectToClipPos(IN.vertex);
			OUT.uv = IN.texcoord;

			return OUT;
		}


		fixed4 frag(v2f_card IN) : SV_Target
		{
			float4 col = tex2D(_MainTex, IN.uv);
			col.rgb = col.rgb - tex2D(_DamagedMask, IN.uv).a;
			
			return col * _Color;
		}

        ENDCG
        }
    }
}
