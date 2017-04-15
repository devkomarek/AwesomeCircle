//ShaderWeaverData{"shaderQueue":3000,"shaderQueueOffset":0,"shaderType":0,"shaderBlend":0,"excludeRoot":false,"version":1.0,"pixelPerUnit":0.0,"spriteRect":{"serializedVersion":"2","x":0.0,"y":0.0,"width":0.0,"height":0.0},"title":"tutorial3","materialGUID":"7d1bcd0f0843151429e557f44e57f6ab","paramList":[],"nodes":[{"id":"9ca8233c_bfed_415d_953c_8716cf4e16b3","name":"ROOT","depth":1,"type":0,"parent":[],"children":["a66c567c_6e01_4829_99e9_1443b1f88771"],"textureExGUID":"","textureGUID":"197595468a5aecd40bea9e6e3e1d0768","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":537.5,"y":353.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0],"inputType":[0,1,3,0,1,3,0,1,3],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}},{"id":"98c4a241_0b89_4356_b241_d130f7617cdd","name":"color1","depth":1,"type":3,"parent":["a66c567c_6e01_4829_99e9_1443b1f88771"],"children":[],"textureExGUID":"","textureGUID":"a9c9beca4efbcdc44ba70da88e0100c4","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":205.0,"y":347.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0],"inputType":[1,3,0,1,3,0,1,3,0],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}},{"id":"a66c567c_6e01_4829_99e9_1443b1f88771","name":"mask0","depth":1,"type":1,"parent":["9ca8233c_bfed_415d_953c_8716cf4e16b3"],"children":["98c4a241_0b89_4356_b241_d130f7617cdd"],"textureExGUID":"","textureGUID":"3c26eded4fa30fe47ad16e69e58e067d","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":371.0,"y":348.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,1,3,0,1,3,0,1,3],"inputType":[0,1,3,0,1,3,0,1,3],"dirty":false,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}}],"clipValue":0.0}
Shader "Shader Weaver/tutorial3"{   
	Properties {   
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("_MainTex", 2D) = "white" { }
		_ButtonBlue_9816 ("_ButtonBlue_9816", 2D) = "white" { }
		_tutorial3mask0_9896 ("_tutorial3mask0_9896", 2D) = "white" { }
	}   
	SubShader {   
		Tags {
			"Queue"="Transparent"
		}
		pass {   
		Blend SrcAlpha  OneMinusSrcAlpha   
			CGPROGRAM  
			#pragma vertex vert   
			#pragma fragment frag   
			#include "UnityCG.cginc"   
			fixed4 _Color;
			float4 _TimeEditor;
			float4 _MainTex_ST;
			sampler2D _MainTex;   
			sampler2D _ButtonBlue_9816;   
			sampler2D _tutorial3mask0_9896;   
			struct appdata_t {
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};
			struct v2f {   
				float4  vertex : SV_POSITION;   
				fixed4 color    : COLOR;
				float2  uv_MainTex : TEXCOORD1;
				float2  uv_Other : TEXCOORD2;
			};   
			float2 UV_RotateAround(float2 center,float2 uv,float rad)
			{
				float2 fuv = uv - center;
				float2x2 ma;
				ma[0] = float2(cos(rad),sin(rad));
				ma[1] = float2(-sin(rad),cos(rad));
				fuv = mul(ma,fuv)+center;
				return fuv;
			}
			v2f vert (appdata_t IN) {
				v2f OUT;   
				OUT.vertex = mul(UNITY_MATRIX_MVP,IN.vertex);   
				OUT.color = IN.color * _Color;
				OUT.uv_MainTex = TRANSFORM_TEX(IN.texcoord,_MainTex);  
				OUT.uv_Other = TRANSFORM_TEX(IN.texcoord,_MainTex);  
				return OUT;   
			}   
			float4 frag (v2f i) : COLOR {
				float4 _TimeAll = _Time + _TimeEditor;
				float4 _TimeMod = _TimeAll%1;
				float4 color_tutorial3mask0_9896 = tex2D(_tutorial3mask0_9896,i.uv_Other);    
				float4 result = float4(0,0,0,0);


				//====================================
				//============ color1 ============   
				float2  uv_color1 = i.uv_Other;
				float2 center_color1 = float2(0.5,0.5);    
				uv_color1 = uv_color1-center_color1;    
				uv_color1 = uv_color1+fixed2(0,0);    
				uv_color1 = uv_color1+fixed2(0,0)*(_TimeAll.y);    
				uv_color1 = UV_RotateAround(fixed2(0,0),uv_color1,0);    
				uv_color1 = uv_color1/fixed2(1,1);    
				float2 dir_color1 = uv_color1/length(uv_color1);    
				uv_color1 = uv_color1-dir_color1*fixed2(0,0)*(_TimeAll.y);    
				uv_color1 = UV_RotateAround(fixed2(0,0),uv_color1,0*(_TimeAll.y));    
				uv_color1 = uv_color1+center_color1;    
				float4 color_color1 = tex2D(_ButtonBlue_9816,uv_color1);    
				color_color1 = 0 +0*(1) + color_color1;    
				color_color1 = color_color1*fixed4(1,1,1,1);    


				//====================================
				//============ mask0 ============   


				//====================================
				//============ ROOT ============   
				float2  uv_ROOT = i.uv_MainTex;
				float4 color_ROOT = tex2D(_MainTex,uv_ROOT);    
				result = lerp(result,float4(color_ROOT.rgb,1),clamp(color_ROOT.a*1,0,1));    
				result = lerp(result,float4(color_color1.rgb,1),clamp(color_color1.a*1*(1)*color_tutorial3mask0_9896.r,0,1));    
				result = result*i.color;
				clip(result.a - 0);    
				return result;
			}  
			ENDCG
		}
	}
}
