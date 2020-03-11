Shader "Custom/WaterShader"
{
	Properties
	{
		//[Header("Main Texture Settings")]
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Roughness("Roughness", Range(0,1)) = 0.5
		_NormalMap("Normal", 2D) = "bump" {}
		_NormalIntensity("NormalIntensity", float) = 1.0
		_EmissionMap("Emission",2D) = "Black"{}
		_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_Metallic("Metallic", Range(0,1)) = 0.0
		_HeightFactor("Height Factor",float ) = 1.0

		[Space(30)]


		//[Header("Secondary Maps Settings")]
		_SecondaryAlbedo("Albedo (RGBA)",2D) = "gray" {}
		//_SecondaryColor("Secondary Color", Color) = (1,1,1,1)
		_SecondaryNormal("Normal",2D) = "bump" {}
		_SecondaryNormalIntensity("Normal Intensity", float) = 0.0


		[Space(30)]

			_TextureWeighting("TextureWeighting", Range(0,1)) = 0.0

		[Space(30)]

			//[Header("Wave Setttings ")]
			_Color2("Color2", Color) = (1, 1, 1, 1)
				// color of the edge effect
			_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
				// width of the edge effect
			_DepthFactor("Depth Factor", float) = 1.0
			_Height("Height", float) = 0.0 //Extra height value
			_WaveAmp("Wave amp", float) = 0.0
			_WaveSpeed("Wave Speed",float) = 0.0
			_NoiseTex("Noise texture",2D) = "white" {}
	}
		SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _EmissionMap;
		sampler2D _NormalMap;
		sampler2D _NoiseTex;
		sampler2D _SecondaryNormal;
		sampler2D _SecondaryAlbedo;


		half _Roughness;
		half _Metallic;

		fixed4 _Color;
		fixed4 _EmissionColor;

		float _TextureWeighting;
		float _NormalIntensity;
		float _SecondaryNormalIntensity;
		float _SecondaryColor;
		float _Height;
		float _WaveSpeed;
		float _WaveAmp;

	struct Input
	{
		float2 uv_MainTex;
		float2 uv_NormalMap;
		float2 uv_RoughnessMap;
		float2 uv_EmissionMap;
		float2 uv_SecondaryAlbedo;
		float2 uv_SecondaryNormal;
	};

	UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
	UNITY_INSTANCING_BUFFER_END(Props)


		//vert displacement
		void vert(inout appdata_full v)
		{

		// apply wave animation
		float noiseSample = tex2Dlod(_NoiseTex, float4(v.texcoord.xy, 0, 0));
		v.vertex.y = sin(_Time * _WaveSpeed * noiseSample) * _WaveAmp + _Height;
		v.vertex.x += cos(_Time * _WaveSpeed * noiseSample) * _WaveAmp;
}


inline fixed3 combineNormalMaps(fixed3 base, fixed3 detail) {
	base += fixed3(0, 0, 1);
	detail *= fixed3(-1, -1, 1);
	return base * dot(base, detail) / base.z - detail;
}

void surf(Input IN, inout SurfaceOutputStandard o)
{
	//albedo combination using a weigthed value.

	fixed4 baseAlbedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed4 detailAlbedo = tex2D(_SecondaryAlbedo, IN.uv_SecondaryAlbedo);

	float baseFactor;
	float detailFactor;
	if (_TextureWeighting != 0)
	{
		baseFactor = _TextureWeighting;
		detailFactor = 1 - _TextureWeighting;
	}
	else
	{
		baseFactor = 1;
		detailFactor = 1;
	}
	baseAlbedo *= baseFactor;
	detailAlbedo *= detailFactor;
	fixed4 CombinedAlbedo = normalize(baseAlbedo + detailAlbedo);




	o.Albedo = CombinedAlbedo;





	o.Alpha = CombinedAlbedo.a;
	o.Metallic = _Metallic;
	o.Smoothness = _Roughness;
	o.Emission = tex2D(_EmissionMap, IN.uv_EmissionMap).rgb * _EmissionColor.rgb;;




	//normal map combination using the dot product of the two maps
	fixed3 baseNormal = UnpackScaleNormal(tex2D(_NormalMap, IN.uv_NormalMap), _NormalIntensity);
	fixed3 detailNormal = UnpackScaleNormal(tex2D(_SecondaryNormal, IN.uv_SecondaryNormal), _SecondaryNormalIntensity);
	fixed3 combinedNormal = combineNormalMaps(baseNormal, detailNormal);

	o.Normal = combinedNormal;
}
ENDCG
	Pass
{
Blend SrcAlpha OneMinusSrcAlpha
ZWrite off
CGPROGRAM
// required to use ComputeScreenPos()
#include "UnityCG.cginc"


#pragma vertex vert
#pragma fragment frag
#pragma glsl
#pragma target 3.0

 // Unity built-in - NOT required in Properties
 sampler2D _CameraDepthTexture;


struct vertexInput
 {
   float4 vertex : POSITION;
   float4 texCoord : TEXCOORD1;
 };

struct vertexOutput
 {
	float4 pos : SV_POSITION;
	float4 texCoord : TEXCOORD0;
	float4 screenPos : TEXCOORD1;
 };

float _Height;
float _WaveSpeed;
float _WaveAmp;
sampler2D _NoiseTex;
float _HeightFactor;
vertexOutput vert(vertexInput input)
  {
	vertexOutput output;

	// convert to camera clip space
	output.pos = UnityObjectToClipPos(input.vertex);

	// apply wave animation



	// convert obj-space position to camera clip space
	output.pos = UnityObjectToClipPos(input.vertex);
	float noiseSample = tex2Dlod(_NoiseTex, float4(input.texCoord.xy, 0, 0));
	output.pos.y += sin(_Time * _WaveSpeed * noiseSample) * _WaveAmp * -_HeightFactor;
	output.pos.x += cos(_Time * _WaveSpeed * noiseSample) * _WaveAmp  ;

	// compute depth (screenPos is a float4)
	output.screenPos = ComputeScreenPos(output.pos);
	//output.pos.y += _Height;
	output.texCoord = input.texCoord;

	return output;
  }
sampler2D _MainTex;
fixed4 _Color2;
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
  float4 col = _Color2 + (foamLine * _EdgeColor);

  if (foamLine < .03)
  {
	  col.a = 0;
  }
  // else return _MainTex.albedo;
	return col;

  }

	  ENDCG
  }
	}



		FallBack "Diffuse"
}
