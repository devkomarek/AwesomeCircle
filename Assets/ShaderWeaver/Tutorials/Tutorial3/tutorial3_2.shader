//ShaderWeaverData{"shaderQueue":3000,"shaderQueueOffset":0,"shaderType":0,"shaderBlend":0,"excludeRoot":false,"version":1.0,"pixelPerUnit":0.0,"spriteRect":{"serializedVersion":"2","x":0.0,"y":0.0,"width":0.0,"height":0.0},"title":"tutorial3_2","materialGUID":"f69d009c8004b8a42885b35024cbad65","paramList":[],"nodes":[{"id":"f4a0350f_c076_482f_996c_96461b9a6b34","name":"ROOT","depth":1,"type":0,"parent":[],"children":["ca34a71a_5032_4887_ae47_65c26f02eca1"],"textureExGUID":"","textureGUID":"a12d6965ef8f47048814fddddb17aa33","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":486.5,"y":194.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0,0,0,0,0],"inputType":[0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}},{"id":"118cabcc_9260_4a72_a139_93cf38e4da6a","name":"color2","depth":1,"type":3,"parent":["ca34a71a_5032_4887_ae47_65c26f02eca1"],"children":[],"textureExGUID":"","textureGUID":"f0c751e3e6b99b549bfc463f96fc132f","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":137.0,"y":190.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":-1.1920928955078126e-7},"r_angle":1.3715214729309083,"s_scale":{"x":1.0,"y":0.9726893901824951},"t_speed":{"x":0.1503906399011612,"y":-2.0459426153252026e-8},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0,0,0,0,0],"inputType":[1,3,1,3,1,3,1,3,0,1,3,0,1,3,0,1,3,0],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}},{"id":"ca34a71a_5032_4887_ae47_65c26f02eca1","name":"mask3","depth":1,"type":1,"parent":["f4a0350f_c076_482f_996c_96461b9a6b34"],"children":["118cabcc_9260_4a72_a139_93cf38e4da6a"],"textureExGUID":"","textureGUID":"542f8ed60cc19e445be466b80cd415ce","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":313.0,"y":191.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,1,3,0,1,3,0,1,3,0,1,3],"inputType":[0,1,3,0,1,3,0,1,3,0,1,3],"dirty":false,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":1,"strs":["f4a0350f_c076_482f_996c_96461b9a6b34","118cabcc_9260_4a72_a139_93cf38e4da6a"]}}],"clipValue":0.0}
Shader "Shader Weaver/tutorial3_2"{   
	Properties {   
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("_MainTex", 2D) = "white" { }
		_ramp_9662 ("_ramp_9662", 2D) = "white" { }
		_tutorial32mask0_9724 ("_tutorial32mask0_9724", 2D) = "white" { }
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
			sampler2D _ramp_9662;   
			sampler2D _tutorial32mask0_9724;   
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
				float4 color_tutorial32mask0_9724 = tex2D(_tutorial32mask0_9724,i.uv_Other);    
				float4 result = float4(0,0,0,0);


				//====================================
				//============ color2 ============   
				float2  uv_color2 = i.uv_Other;
				float2 center_color2 = float2(0.5,0.5);    
				uv_color2 = uv_color2-center_color2;    
				uv_color2 = uv_color2+fixed2(0,1.192093E-07);    
				uv_color2 = uv_color2+fixed2(-0.1503906,2.045943E-08)*(_TimeAll.y);    
				uv_color2 = UV_RotateAround(fixed2(0,0),uv_color2,1.371521);    
				uv_color2 = uv_color2/fixed2(1,0.9726894);    
				float2 dir_color2 = uv_color2/length(uv_color2);    
				uv_color2 = uv_color2-dir_color2*fixed2(0,0)*(_TimeAll.y);    
				uv_color2 = UV_RotateAround(fixed2(0,0),uv_color2,0*(_TimeAll.y));    
				uv_color2 = uv_color2+center_color2;    
				float4 color_color2 = tex2D(_ramp_9662,uv_color2);    
				color_color2 = 0 +0*(1) + color_color2;    
				color_color2 = color_color2*fixed4(1,1,1,1);    


				//====================================
				//============ mask3 ============   


				//====================================
				//============ ROOT ============   
				float2  uv_ROOT = i.uv_MainTex;
				float4 color_ROOT = tex2D(_MainTex,uv_ROOT);    
				result = lerp(result,float4(color_ROOT.rgb,1),clamp(color_ROOT.a*1,0,1));    
				result = lerp(result,float4(color_color2.rgb,1),clamp(color_color2.a*1*(1)*color_tutorial32mask0_9724.r,0,1));    
				result = result*i.color;
				clip(result.a - 0);    
				return result;
			}  
			ENDCG
		}
	}
}
