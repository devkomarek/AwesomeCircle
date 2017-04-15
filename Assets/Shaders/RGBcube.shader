Shader "Cg shader for RGB cube" { 
   SubShader { 
      Pass { 
         CGPROGRAM 
 
         #pragma vertex vert // vert function is the vertex shader 
         #pragma fragment frag // frag function is the fragment shader
		 #include "UnityCG.cginc"
 
         // for multiple vertex output parameters an output structure 
         // is defined:

		 struct vertexInput{
			 float4 vertexPos : POSITION; // -0.5 - 0.5
			 float4 texcoord : TEXCOORD0;//  0 - 1
			 float3 normal : NORMAL;  // -1 - 1
			 float4 tangent : TANGENT;
		 };
		 
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : TEXCOORD0;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output; 
 
            output.pos =  mul(UNITY_MATRIX_MVP, input.vertexPos);
			//output.col = float4(input.texcoord.x,0.0,0.0,1.0);
			//output.col = float4(0.0,input.texcoord.y,0.0,1.0);
            //output.col = input.texcoord;
			//output.col = input.vertexPos + float4(0.5,0.5,0.5,1.0);
			output.col = float4(input.normal,1.0) + float4(1.0,0.0,0.0,1.0);
			//output.col = float4((input.normal+float3(1.0,1.0,1.0))/2.0,1.0);
			//output.col = input.texcoord - float4(1.5, 2.3, 1.1, 0.0);
			// output.col = input.texcoord.zzzz;
			// output.col = input.texcoord / tan(1.0);
			//output.col = dot(input.normal, input.tangent.xyz) * input.texcoord;
            return output;
         }
 
         float4 frag(vertexOutput i) : COLOR
         {
            return i.col; 
         }
 
         ENDCG  
      }
   }
}