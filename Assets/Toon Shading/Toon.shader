// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toon"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Thresholds ("Thresholds", Vector) = (0.25, 0.5, 0.75, 1)
		_Colour0 ("Colour0", Color) = (0, 0, 0, 1)
		_Colour1 ("Colour1", Color) = (0.33, 0.33, 0.33, 1)
		_Colour2 ("Colour2", Color) = (0.66, 0.66, 0.66, 1)
		_Colour3 ("Colour3", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"IgnoreProjector" = "True"
		}

		Pass
		{
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _Thresholds;

			float4 _Colour0;
			float4 _Colour1;
			float4 _Colour2;
			float4 _Colour3;

			struct v2f
			{
				float4 position : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			v2f vert (float4 position : POSITION, float2 uv_MainTex : TEXCOORD0)
			{
				v2f o;

				o.position = UnityObjectToClipPos(position);
				o.uv = uv_MainTex;

				return o;
			}

			fixed4 frag(v2f v) : SV_TARGET
			{
				fixed4 col = tex2D(_MainTex, v.uv);

				float lowest = min(min(col.r, col.g), col.b);
				float highest = max(max(col.r, col.g), col.b);

				//	Red sorting
				//if (col.r < _Thresholds.x)
				//	col.r = _Thresholds.x;
				//else if (col.r < _Thresholds.y)
				//	col.r = _Thresholds.y;
				//else if (col.r < _Thresholds.z)
				//	col.r = _Thresholds.z;
				//else
				//	col.r = _Thresholds.w;

				//	Green sorting
				//if (col.g < _Thresholds.x)
				//	col.g = _Thresholds.x;
				//else if (col.g < _Thresholds.y)
				//	col.g = _Thresholds.y;
				//else if (col.g < _Thresholds.z)
				//	col.g = _Thresholds.z;
				//else
				//	col.g = _Thresholds.w;

				//	Blue sorting
				//if (col.b < _Thresholds.x)
				//	col.b = _Thresholds.x;
				//else if (col.b < _Thresholds.y)
				//	col.b = _Thresholds.y;
				//else if (col.b < _Thresholds.z)
				//	col.b = _Thresholds.z;
				//else
				//	col.r = _Thresholds.w;

				//return col;

				float l = (lowest + highest) / 2;
				if (l < _Thresholds.x)
					return _Colour0;
				else if (l < _Thresholds.y)
					return _Colour1;
				else if (l < _Thresholds.z)
					return _Colour2;
				else
					return _Colour3;

				return col;
			}

			ENDCG
		}
	} 

	FallBack "Diffuse"
}

