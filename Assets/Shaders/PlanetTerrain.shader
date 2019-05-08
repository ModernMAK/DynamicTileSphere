Shader "Custom/PlanetTerrain" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Terrain Texture Array", 2DArray) = "white" {}
		//_GridTex("Grid Texture", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Standard fullforwardshadows vertex:vert
#pragma target 3.5


	half _Glossiness;
	half _Metallic;
	fixed4 _Color;
	//sampler2D _GridTex;
	UNITY_DECLARE_TEX2DARRAY(_MainTex);

	struct Input {

		float4 color : COLOR;
		//float3 worldPos : SV_PO;
		float2 uv_MainTex : TEXCOORD0;
		//float2 uv_GridTex : TEXCOORD1; 
		float4 terrain : TEXCOORD2;
	};

	void vert(inout appdata_full v, out Input data) {
		data.color = v.color;
		//data.worldPos = v.vertex;
		//UNITY_INITIALIZE_OUTPUT(Input, data);
		data.uv_MainTex = float2(v.texcoord.xy);
		//data.uv_GridTex = float2(v.texcoord.zw);
		data.terrain = float4(v.texcoord2.xyz,0);
	}

	float4 GetTerrainColor(Input IN, int index) {
		float3 uvw = float3(IN.uv_MainTex.xy /** 0.02*/, IN.terrain[index]);
		float4 c = UNITY_SAMPLE_TEX2DARRAY(_MainTex, uvw);
		return c * IN.color[index];
	}

	void surf(Input IN, inout SurfaceOutputStandard o) {
		fixed4 cM =
			GetTerrainColor(IN, 0) +
			GetTerrainColor(IN, 1) +
			GetTerrainColor(IN, 2);

		/*fixed4 cG =
			tex2D(_GridTex, IN.uv_MainTex);
		*/



		o.Albedo = cM.rgb;// /*cG.rgb;// */ ((cG.rgb * cG.a) + (cM.rgb * (1 - cG.a))) * _Color;
		o.Metallic = _Metallic;
		o.Smoothness = _Glossiness;
		o.Alpha = cM.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}