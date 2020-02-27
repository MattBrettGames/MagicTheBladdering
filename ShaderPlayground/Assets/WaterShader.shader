Shader "Custom/Water"
{
	Properties
	{
		
	_MainTex("Albedo Transparency",2D) = "white" {}
		// color of the water
	_Color("Color", Color) = (1, 1, 1, 1)
		// color of the edge effect
	_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		// width of the edge effect
	_DepthFactor("Depth Factor", float) = 1.0
	}

		SubShader		
	{
		Blend SrcAlpha OneMinusSrcAlpha
		Tags{"Queue" = "Transparent" "Render Type" = "Transparent" }



	/*	CGPROGRAM
	//	#include "UnityCG.cginc"
		#pragma surface surf Lambert

		struct SurfaceOutput 
	{
		fixed3 Albedo;  // diffuse color
		fixed3 Normal;  // tangent space normal, if written
		fixed3 Emission;
		half Specular;  // specular power in 0..1 range
		fixed Gloss;    // specular intensity
		fixed Alpha;    // alpha for transparencies
	};

	sampler2D _MainTex;
	struct Input
	{
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
		o.Albedo = c.rgb;
	}

		ENDCG*/
	


	Pass
	{

	CGPROGRAM
	// required to use ComputeScreenPos()
	#include "UnityCG.cginc"
	
	
	#pragma vertex vert
	#pragma fragment frag

	 // Unity built-in - NOT required in Properties
	 sampler2D _CameraDepthTexture;

	
	struct vertexInput
	 {
	   float4 vertex : POSITION;
	 };

	struct vertexOutput
	 {
	   float4 pos : SV_POSITION;
	   float4 screenPos : TEXCOORD1;
	 };

	vertexOutput vert(vertexInput input)
	  {
		vertexOutput output;

		// convert obj-space position to camera clip space
		output.pos = UnityObjectToClipPos(input.vertex);

		// compute depth (screenPos is a float4)
		output.screenPos = ComputeScreenPos(output.pos);

		return output;
	  }

	fixed4 _Color;
	fixed4 _EdgeColor;
	float _DepthFactor;

	
	float4 frag(vertexOutput input) : COLOR
	{

	  float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
	  float depth = LinearEyeDepth(depthSample).r;

	  // apply the DepthFactor to be able to tune at what depth values
	  // the foam line actually starts
	  float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));

	  // multiply the edge color by the foam factor to get the edge,
	  // then add that to the color of the water
	  float4 col = _Color + (foamLine * _EdgeColor);
	
	  if (foamLine < .01)
	  {
		  col.a = 0;
	  }
	  return col;

	}

		ENDCG
	  } 

		

	
	} Fallback  "Diffuse"}