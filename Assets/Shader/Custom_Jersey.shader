Shader "Custom/Jersey" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white" {}
		_MaskTex ("Mask Texture", 2D) = "black" {}
		_Color1 ("Tshirt Color 1 (R)", Vector) = (1,1,1,1)
		_Color2 ("Tshirt Color 2 (Y)", Vector) = (1,1,1,1)
		_Color3 ("Shorts Color (G)", Vector) = (1,1,1,1)
		_Color4 ("Socks Color (B)", Vector) = (1,1,1,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Mobile/VertexLit"
}