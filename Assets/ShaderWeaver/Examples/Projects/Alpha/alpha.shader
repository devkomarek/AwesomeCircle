//ShaderWeaverData{"shaderQueue":3000,"shaderQueueOffset":0,"shaderType":0,"shaderBlend":0,"excludeRoot":false,"version":1.0,"pixelPerUnit":0.0,"spriteRect":{"serializedVersion":"2","x":0.0,"y":0.0,"width":0.0,"height":0.0},"title":"alpha","materialGUID":"d8be69d252e0171489f7a5f9c9997699","paramList":[{"type":0,"name":"p","min":0.0,"max":1.0,"defaultValue":0.0}],"nodes":[{"id":"c3fffbd3_7fb6_4e50_9c31_78a792235fca","name":"ROOT","depth":1,"type":0,"parent":[],"children":["945ecab3_7963_416d_b60a_97c3fe504abc"],"textureExGUID":"","textureGUID":"0dc8f47d0af28114f9d5d0e901e4e657","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":664.0,"y":206.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0,0,0,0,0,0,0,0,0],"inputType":[0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3,0,1,3],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}},{"id":"945ecab3_7963_416d_b60a_97c3fe504abc","name":"alpha1","depth":1,"type":5,"parent":["c3fffbd3_7fb6_4e50_9c31_78a792235fca"],"children":[],"textureExGUID":"","textureGUID":"fa3108da2fe38a748bfce58b4c9b5410","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":422.0,"y":206.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":-1.0,"pop_speed":1.5,"pop_Param":"p","pop_channel":0,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[3,3,3,3,3,3,3,3,3,3,3],"inputType":[1,1,1,1,1,1,1,1,1,1,1],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":1,"strs":["c3fffbd3_7fb6_4e50_9c31_78a792235fca"]}}],"clipValue":0.0}
Shader "Shader Weaver/alpha"{   
	Properties {   
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("_MainTex", 2D) = "white" { }
		_wave_32682 ("_wave_32682", 2D) = "white" { }
		p ("p", Range(0,1)) = 0
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
			sampler2D _wave_32682;   
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
				//============ alpha1 ============   
				float2  uv_alpha1 = i.uv_Other;
				float2 center_alpha1 = float2(0.5,0.5);    
				uv_alpha1 = uv_alpha1-center_alpha1;    
				uv_alpha1 = uv_alpha1+fixed2(0,0);    
				uv_alpha1 = uv_alpha1+fixed2(0,0)*(_TimeAll.y);    
				uv_alpha1 = UV_RotateAround(fixed2(0,0),uv_alpha1,0);    
				uv_alpha1 = uv_alpha1/fixed2(1,1);    
				float2 dir_alpha1 = uv_alpha1/length(uv_alpha1);    
				uv_alpha1 = uv_alpha1-dir_alpha1*fixed2(0,0)*(_TimeAll.y);    
				uv_alpha1 = UV_RotateAround(fixed2(0,0),uv_alpha1,0*(_TimeAll.y));    
				uv_alpha1 = uv_alpha1+center_alpha1;    
				float4 color_alpha1 = tex2D(_wave_32682,uv_alpha1);    
				color_alpha1 = -1 +1.5*(p) + color_alpha1;    
				color_alpha1 = color_alpha1*fixed4(1,1,1,1);    


				//====================================
				//============ ROOT ============   
				float2  uv_ROOT = i.uv_MainTex;
				float4 color_ROOT = tex2D(_MainTex,uv_ROOT);    
				result = lerp(result,float4(color_ROOT.rgb,1),clamp(color_ROOT.a*1,0,1));    
				result = float4(result.rgb,result.a*color_alpha1.r*1*1);    
				result = result*i.color;
				clip(result.a - 0);    
				return result;
			}  
			ENDCG
		}
	}
}
