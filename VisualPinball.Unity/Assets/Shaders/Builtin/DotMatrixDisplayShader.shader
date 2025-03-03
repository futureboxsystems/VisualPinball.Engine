﻿Shader "Pinball/Dot Matrix Display Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		__Dimensions ("Dimensions", Vector) = (128, 32, 0, 0)
		__Padding ("Padding", Range(0.0, 0.8)) = .2
		__Roundness ("Roundness", Range(0.0, 0.5)) = .35
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			#include "../Srp/Display/DotMatrixDisplayShader.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float2 __Dimensions;
			float __Padding;
			float __Roundness;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float2 dotCenter;
				SamplePosition_float(i.uv, __Dimensions, dotCenter);

				float4 pixelColor = tex2D(_MainTex, dotCenter);
				float4 outColor;
				Dot_float(i.uv, __Dimensions, __Padding, __Roundness, pixelColor, outColor);

				return outColor;
			}
			ENDCG
		}
	}
}
