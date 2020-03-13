Shader "Custom/GrassShaderNew"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_QuadSize("GrassQuadSize", float) = 0.5 //currently uniformscales all quad
		_GrasHeight("Grass Height",float) =	0.25
		_GrasWidth("Grass Height",float) = 0.25
		_TesselationStage("Tesselation Amount", Range(0,3)) = 0  
	}

		CGINCLUDE
		#include "UnityCG.cginc"
		#include "AutoLight.cginc"
			//disable backface culling.
			struct geometryOutput
		{
			float4 pos : SV_POSITION; //takes input worldposition for each vert
			float3 norm : NORMAL; //takes normal input data for each vert
			float2 uv : TEXCOORD0; 
			float3 diffuseColor : TEXCOORD1;
			//float3 specularColor : TEXCOORD2;
		};

		half _QuadSize;
		half _GrassHeight;
		half _GrassWidth;
		[maxvertexcount(8)]
			void geo(point float4 IN[1] : SV_POSITION, inout TriangleStream<geometryOutput> triStream)
		{
				
				float pos = IN[0]; 
				
				float3 perpendicularAngle = float3(1, 0, 0);
				float3 normal = cross(perpendicularAngle, IN[0].norm) //to get normal of actuall quad
				float3 v0 = IN[0].pos.xyz; //base vert pos
				float3 v1 = IN[0].pos.xyz + IN[0].norm * _GrassHeight; //top vert pos
				geometryOutput o;

				float3 colo = (IN[0].color)
				//UNITY_MATRIX_MVP is the view projection matrix  to world pos, camera pos and screen pos

				o.pos = mul(UNITY_MATRIX_MVP, v0 + perpendicularAngle * 0.5 * _GrassHeight);
				o.norm = normal;
				o.uv = new float2(1, 0)
				o.diffuseColor = color;
				triStream.Append(o); //gets first vert

				o.pos = mul(UNITY_MATRIX_MVP, v0 - perpendicularAngle * 0.5 * _GrassHeight);
				o.norm = normal;
				o.uv = float2(0, 0)
				o.diffuseColor = color;
				triStream.Append(o); //gets second vert

				o.pos = mul(UNITY_MATRIX_MVP, v1 + perpendicularAngle * 0.5 * _GrassHeight);
				o.norm = normal;
				o.uv = new float2(1, 1)
				o.diffuseColor = color;
				triStream.Append(o); //gets first vert

				o.pos = mul(UNITY_MATRIX_MVP, v1 - perpendicularAngle * 0.5 * _GrassHeight);
				o.norm = normal;
				o.uv = new float2(0, 1)
				o.diffuseColor = color;
			





				




		}


		ENDCG
	

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
       // #pragma surface surf Standard fullforwardshadows
		#pragma vertex vert
		#pragma framgment frag
		#pragma geometry geo
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 4.0

		


		
        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

       /* void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }*/
        ENDCG
    }

    FallBack "Diffuse"
}
