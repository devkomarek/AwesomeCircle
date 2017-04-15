﻿Shader "Hero/Laser"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_DisplayTex ("Displacement Texture", 2D) = "white"{}
		_Magnitude ("Magnitude", Range(0, .1)) = 1
		_Distortion ("Distortion", Float) = 0.0
		_LightSaberFactor("LightSaberFactor", Range(0.0,1.0)) = 0.0
		_Color("Color",Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags{"Queue"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha 
		Zwrite off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"



			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float4 col : COLOR;
				float3 normal : NORMAL;
		
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0; 
				float4 col : COLOR;
			};
          		    float _LineWidth;
           		    float _LineScale;
					sampler2D _MainTex;
					float4 _MainTex_ST;
					sampler2D _DisplayTex;
					float _Magnitude;
					float _Distortion;
					float _LightSaberFactor;
					float4 _Color;
			v2f vert (appdata v)
			{
				v2f o;
                o.uv = TRANSFORM_TEX (v.uv, _MainTex);  
                
                // float4 vMVP = mul(UNITY_MATRIX_MVP, v.vertex);
                // float4 otherPos = float4(v.normal.xyz, 1.0);
                // float4 otherMVP = mul(UNITY_MATRIX_MVP, otherPos);
				// float scaledLineWidth = _LineWidth * _LineScale;

                // float2 lineDirProj = scaledLineWidth * normalize((vMVP.xy/vMVP.w) - (otherMVP.xy/otherMVP.w));

				// vMVP.x = vMVP.x + lineDirProj.x * v.texcoord1.x + lineDirProj.y * v.texcoord1.y;
				// vMVP.y = vMVP.y + lineDirProj.y * v.texcoord1.x - lineDirProj.x * v.texcoord1.y;
				// o.vertex = vMVP;
				// o.col = v.col;
                // return o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);	

				o.col = v.col	;		
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
                float2 disp = tex2D(_DisplayTex, i.uv /_Distortion).xy;
				disp -=0.5;
				disp = disp* _Magnitude;

				float4 tex = tex2D(_MainTex, i.uv + disp) * i.col;

				if(tex.a < _LightSaberFactor)
				{
                	return tex;
                }
                else 
                {
                	return tex * _Color; 
                }
			//	float4 col = tex2D(_MainTex, i.uv + disp) * i.col;
				//float4 col = float4(0,1,1,1);
				//return col;
			}
			ENDCG
		}
	}
}
