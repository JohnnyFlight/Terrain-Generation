// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "heightmap_displacement"
{
	Properties
	{
		_Tess ("Tesselation", Range(1, 32)) = 4
		_MyTexture ("Map", 2D) = "Black" {}
		_Amount ("Extrusion Amount", Range(-1.0,1.0)) = 0
	}
    SubShader
    {
    	Pass
    	{
	    	CGPROGRAM
	    	#pragma vertex vert
	    	#pragma fragment frag

	    	//#pragma surface surf BlinnPhong

	    	#include "UnityCG.cginc"

	    	struct appdata
	    	{
	    		float4 vertex : POSITION;
	    		float2 uv : TEXCOORD0;
	    	};

	    	struct v2f
	    	{
	    		float2 uv : TEXCOORD0;
	    		float4 vertex : SV_POSITION;
	    	};

	    	struct Input
	    	{
	    		float2 uv_MyTexture;
	    	};

	    	sampler2D _MyTexture;
	    	float _Amount;

	    	float _Tess;

	    	float4 tessFixed()
	    	{
	    		return _Tess;
	    	}

	    	v2f vert (appdata v)
	    	{
	    		v2f o;
	    		float4 access = { v.uv, 0, 0 };
	    		float4 blah = tex2Dlod(_MyTexture, access);
	    		blah.a = 0.0;
	    		blah *= 2.0;
	    		blah -= float4(1, 1, 1, 0);

	    		v.vertex += float4(0, blah.g, 0, 0) * _Amount;

	    		o.vertex = UnityObjectToClipPos(v.vertex);
	    		o.uv = v.uv;
	    		return o;
	    	}

	    	fixed4 frag (v2f i) : SV_TARGET
	    	{
	    		fixed4 col = tex2D(_MyTexture, i.uv);
	    		return col;
	    	}

	    	ENDCG
    	}
    }
}