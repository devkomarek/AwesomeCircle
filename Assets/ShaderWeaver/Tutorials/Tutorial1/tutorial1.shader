//ShaderWeaverData{"shaderQueue":3000,"shaderQueueOffset":0,"shaderType":0,"shaderBlend":0,"excludeRoot":false,"version":1.0,"pixelPerUnit":0.0,"spriteRect":{"serializedVersion":"2","x":0.0,"y":0.0,"width":0.0,"height":0.0},"title":"tutorial1","materialGUID":"823ab34a5eaa78f4a833e5b83cd9459c","paramList":[],"nodes":[{"id":"029e2f1c_06e5_46c7_9263_93484268039d","name":"ROOT","depth":1,"type":0,"parent":[],"children":[],"textureExGUID":"","textureGUID":"a9c9beca4efbcdc44ba70da88e0100c4","spriteGUID":"","spriteName":"","rect":{"serializedVersion":"2","x":417.0,"y":141.0,"width":100.0,"height":130.0},"effectData":{"t_startMove":{"x":0.0,"y":0.0},"r_angle":0.0,"s_scale":{"x":1.0,"y":1.0},"t_speed":{"x":0.0,"y":0.0},"r_speed":0.0,"s_speed":{"x":0.0,"y":0.0},"t_Param":"_TimeAll.y","r_Param":"_TimeAll.y","s_Param":"_TimeAll.y","pop_startValue":0.0,"pop_speed":0.0,"pop_Param":"1","pop_channel":3,"useLoop":false,"loopX":0,"gapX":0.0,"loopY":0,"gapY":0.0},"effectDataColor":{"color":{"r":1.0,"g":1.0,"b":1.0,"a":1.0},"op":0,"param":"1"},"effectDataUV":{"op":0,"param":"1","amountR":{"x":0.0,"y":0.0},"amountG":{"x":0.0,"y":0.0},"amountB":{"x":0.0,"y":0.0},"amountA":{"x":0.0,"y":0.0}},"maskChannel":0,"outputType":[0,0,0,0],"inputType":[0,1,3,0,1,3,0,1,3,0,1,3],"dirty":true,"remap":{"x":0.0,"y":0.05000000074505806},"layerMask":{"mask":0,"strs":[]}}],"clipValue":0.0}
Shader "Shader Weaver/tutorial1"{   
	Properties {   
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("_MainTex", 2D) = "white" { }
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
				//============ ROOT ============   
				float2  uv_ROOT = i.uv_MainTex;
				float4 color_ROOT = tex2D(_MainTex,uv_ROOT);    
				result = lerp(result,float4(color_ROOT.rgb,1),clamp(color_ROOT.a*1,0,1));    
				result = result*i.color;
				clip(result.a - 0);    
				return result;
			}  
			ENDCG
		}
	}
}
