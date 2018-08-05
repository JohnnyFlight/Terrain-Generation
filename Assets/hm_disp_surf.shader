Shader "hm_disp_surf"
{
	Properties
	{
		_Tess ("Tesselation", Range(1, 32)) = 4
		_Texture ("Texture", 2D) = "White" {}
		_DispTexture ("Displacement", 2D) = "gray" {}
		_NormalMap ("NormalMap", 2D) = "bump" {}
		_Amount ("Displacement", float) = 0
		_Color ("Color", color) = (1, 1, 1, 0)
	}

    SubShader
    {
    	Tags { "RenderType" = "Opaque" }
    	LOD 300
    	
    	CGPROGRAM
    	#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap

    	struct appdata
    	{
    		float4 vertex : POSITION;
    		float4 tangent : TANGENT;
    		float3 normal : NORMAL;
    		float2 texcoord : TEXCOORD0;
    	};

    	float _Tess;

    	float4 tessFixed()
    	{
    		return _Tess;
    	}

    	sampler2D _DispTexture;
    	float _Amount;

    	void disp (inout appdata v)
    	{
    		float val = clamp(tex2Dlod(_DispTexture, float4(v.texcoord.xy, 0, 0)).r, 0.0, 1.0);
    		float d = val * _Amount;
    		v.vertex.xyz += v.normal * d;
    	}

    	struct Input
    	{
    		float2 uv_Texture;
    	};

    	sampler2D _Texture;
    	sampler2D _NormalMap;
    	fixed4 _Color;

    	void surf (Input IN, inout SurfaceOutput o)
    	{
    		half4 c = tex2D(_Texture, IN.uv_Texture) * _Color;

    		o.Albedo = c.rgb;
    		o.Specular = 0.9;
    		o.Gloss = 2.0;
    		o.Normal = tex2D(_NormalMap, IN.uv_Texture) * 2.0 - float3(1.0, 1.0, 1.0);
    	}

    	ENDCG
    }

    FallBack "Diffuse"
}