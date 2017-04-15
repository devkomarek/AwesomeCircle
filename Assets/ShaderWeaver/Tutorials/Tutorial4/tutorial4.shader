//ShaderWeaverData{"shaderQueue":3000,"shaderQueueOffset":0,"shaderType":0,"shaderBlend":0,"excludeRoot":false,"version":1.0,"pixelPerUnit":0.0,"spriteRect":{"serializedVersion":"2","x":0.0,"y":0.0,"width":0.0,"height":0.0},"title":"tutorial4","materialGUID":"2da2ff4dbe53e2c4aa1f704a8f86798d","paramList":[{"type":0,"name":"Speed","min":0.0,"max":1.0,"defaultValue":0.0},{"type":0,"name":"Amount","min":0.0,"max":1.0,"defaultValue":0.0}],"nodes":[{"id":"1a629c36_4a11_45a9_a461_2061c3c67316","name":"ROOT","depth":1,"type":0,"parent":[],"children":["e2af9859_af7e_47dd_9a31_0c514ffa5bce"],"textureExGUID":"","textureGUID":"6b50937b678fd4144877b602d623578a","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":539.5,"y":353.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0,0,0,0,0],"inputType":[0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}},{"id":"e2af9859_af7e_47dd_9a31_0c514ffa5bce","name":"uv1","depth":1,"type":4,"parent":["1a629c36_4a11_45a9_a461_2061c3c67316"],"children":[],"textureExGUID":"","textureGUID":"fa3108da2fe38a748bfce58b4c9b5410","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":254.0,"y":352.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.263671875},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":1,"param":"_TimeAll.y","amountR":{"x":0.0,"y":0.263671875},"amountG":{"x":0.15625,"y":0.107421875},"amountB":{"x":-0.189453125,"y":0.119140625},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[1,1,1,1,1,1,1],"inputType":[1,1,1,1,1,1,1],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}}],"clipValue":0.0}
Shader "Shader Weaver/tutorial4"{   
	Properties {   
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("_MainTex", 2D) = "white" { }
		_wave_9818 ("_wave_9818", 2D) = "white" { }
		Speed ("Speed", Range(0,1)) = 0
		Amount ("Amount", Range(0,1)) = 0
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
			sampler2D _wave_9818;   
			float Speed; 
			float Amount; 
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
				//============ uv1 ============   
				float2  uv_uv1 = i.uv_Other;
				float2 center_uv1 = float2(0.5,0.5);    
				uv_uv1 = uv_uv1-center_uv1;    
				uv_uv1 = uv_uv1+fixed2(0,0);    
				uv_uv1 = uv_uv1+fixed2(0,-0.2636719)*(_TimeAll.y);    
				uv_uv1 = UV_RotateAround(fixed2(0,0),uv_uv1,0);    
				uv_uv1 = uv_uv1/fixed2(1,1);    
				float2 dir_uv1 = uv_uv1/length(uv_uv1);    
				uv_uv1 = uv_uv1-dir_uv1*fixed2(0,0)*(_TimeAll.y);    
				uv_uv1 = UV_RotateAround(fixed2(0,0),uv_uv1,0*(_TimeAll.y));    
				uv_uv1 = uv_uv1+center_uv1;    
				float4 color_uv1 = tex2D(_wave_9818,uv_uv1);    
				color_uv1 = 0 +0*(1) + color_uv1;    
				uv_uv1 = -(color_uv1.r*fixed2(0,0.2636719) + color_uv1.g*fixed2(0.15625,0.1074219) + color_uv1.b*fixed2(-0.1894531,0.1191406) +  color_uv1.a*fixed2(0,0));    


				//====================================
				//============ ROOT ============   
				float2  uv_ROOT = i.uv_MainTex;
				float4 color_ROOT = tex2D(_MainTex,uv_ROOT);    
				float4 color_ROOT_uv_uv1 = tex2D(_MainTex,i.uv_MainTex + uv_uv1);    
				color_ROOT = lerp(color_ROOT,color_ROOT_uv_uv1,clamp(1*(_TimeAll.y),0,1));    
				result = lerp(result,float4(color_ROOT.rgb,1),clamp(color_ROOT.a*1,0,1));    
				result = result*i.color;
				clip(result.a - 0);    
				return result;
			}  
			ENDCG
		}
	}
}
