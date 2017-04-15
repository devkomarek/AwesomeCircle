//ShaderWeaverData{"shaderQueue":3000,"shaderQueueOffset":0,"shaderType":0,"shaderBlend":0,"excludeRoot":false,"version":1.0,"pixelPerUnit":0.0,"spriteRect":{"serializedVersion":"2","x":0.0,"y":0.0,"width":0.0,"height":0.0},"title":"tutorial2","materialGUID":"9106c310df69c724a81adb52569f6966","paramList":[{"type":0,"name":"p","min":40.0,"max":80.0,"defaultValue":0.0}],"nodes":[{"id":"c829c052_d209_47cc_a2bb_9ef3749e1c70","name":"ROOT","depth":1,"type":0,"parent":[],"children":["4afda766_105a_40f8_b9b6_6ad18618c93d"],"textureExGUID":"","textureGUID":"a9c9beca4efbcdc44ba70da88e0100c4","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":551.2667846679688,"y":340.877197265625,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0,0,0,0,0,0],"inputType":[0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}},{"id":"4afda766_105a_40f8_b9b6_6ad18618c93d","name":"color1","depth":-1,"type":3,"parent":["c829c052_d209_47cc_a2bb_9ef3749e1c70"],"children":[],"textureExGUID":"","textureGUID":"e9d3d239a897e32439d7062da97aca3e","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":295.0,"y":324.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":-1.6272674798965455,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":-3.531650883195625e-12,"y":7.084756710076933e-21},"r_speed":0.0,"s_speed":{"x":0.17627274990081788,"y":0.17627274990081788},"t_Param":"p","r_Param":"_TimeAll.y","s_Param":"p","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0,0,0],"inputType":[1,3,0,1,3,0,1,3,0,1,3,0,1,3,0],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}}],"clipValue":0.0}
Shader "Shader Weaver/tutorial2"{   
	Properties {   
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("_MainTex", 2D) = "white" { }
		_Diamond_9770 ("_Diamond_9770", 2D) = "white" { }
		p ("p", Range(40,80)) = 0
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
			sampler2D _Diamond_9770;   
			float p; 
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
				float4 result = float4(0,0,0,0);


				//====================================
				//============ color1 ============   
				float2  uv_color1 = i.uv_Other;
				float2 center_color1 = float2(0.5,0.5);    
				uv_color1 = uv_color1-center_color1;    
				uv_color1 = uv_color1+fixed2(0,0);    
				uv_color1 = uv_color1+fixed2(3.531651E-12,-7.084757E-21)*(p);    
				uv_color1 = UV_RotateAround(fixed2(0,0),uv_color1,-1.627267);    
				uv_color1 = uv_color1/fixed2(1,1);    
				float2 dir_color1 = uv_color1/length(uv_color1);    
				uv_color1 = uv_color1-dir_color1*fixed2(0.1762727,0.1762727)*(p);    
				uv_color1 = UV_RotateAround(fixed2(0,0),uv_color1,0*(_TimeAll.y));    
				uv_color1 = uv_color1+center_color1;    
				float4 color_color1 = tex2D(_Diamond_9770,uv_color1);    
				color_color1 = 0 +0*(1) + color_color1;    
				color_color1 = color_color1*fixed4(1,1,1,1);    


				//====================================
				//============ ROOT ============   
				float2  uv_ROOT = i.uv_MainTex;
				float4 color_ROOT = tex2D(_MainTex,uv_ROOT);    
				result = lerp(result,float4(color_color1.rgb,1),clamp(color_color1.a*1*(1),0,1));    
				result = lerp(result,float4(color_ROOT.rgb,1),clamp(color_ROOT.a*1,0,1));    
				result = result*i.color;
				clip(result.a - 0);    
				return result;
			}  
			ENDCG
		}
	}
}
