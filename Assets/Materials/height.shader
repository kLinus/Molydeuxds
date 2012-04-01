Shader "Custom/height" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 customColor;
  		};
	      void vert (inout appdata_full v, out Input o) {
	          o.customColor.r = v.vertex.y;
	          o.customColor.g = v.vertex.y;
	          o.customColor.b = v.vertex.y;
	          //o.customColor.rgb = abs(v.normal);
	      }

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb * IN.customColor;
			//o.Albedo *= ;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
