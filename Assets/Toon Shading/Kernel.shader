// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Kernel"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_Row0 ("Row 0", Vector) = (0, 0, 0, 0)
		_Row1 ("Row 1", Vector) = (0, 0, 0, 0)
		_Row2 ("Row 2", Vector) = (0, 0, 0, 0)
		_Row3 ("Row 3", Vector) = (0, 0, 0, 0)
		_Num ("Numerator", float) = 1
		_Den ("Denomenator", float) = 1
		_Threshold ("Threshold", range(0, 1)) = 0.01
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

			struct vin
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			v2f vert (vin u)
			{
				v2f o;

				o.uv = u.uv;
				o.pos = UnityObjectToClipPos(u.pos);

				return o;
			}

			//	I can access this from a script
			Vector _Row0;
			Vector _Row1;
			Vector _Row2;
			Vector _Row3;

			float _Num;
			float _Den;

			float _Threshold;
			float _Alpha;

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			fixed4 frag (v2f v) : SV_TARGET
			{
				//	Get texture coordinate in pixels
				int2 uvpx = _MainTex_TexelSize.zw * v.uv;
				v.uv = uvpx / _MainTex_TexelSize.zw;

				//	Sample the texture
				fixed4 col = tex2D(_MainTex, v.uv);

				//	Get all adjacent pixels
				fixed4 accumulated = fixed4(0, 0, 0, 1);

				//	Turn this into a loop later?
				int2 pospx = uvpx;

				pospx.x -= 1;
				pospx.y -= 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row0.x;

				pospx.x += 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row0.y;

				pospx.x += 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row0.z;

				pospx.x -= 2;
				pospx.y += 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row1.x;

				pospx.x += 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row1.y;

				pospx.x += 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row1.z;

				pospx.x -= 2;
				pospx.y += 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row2.x;

				pospx.x += 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row2.y;

				pospx.x += 1;
				accumulated.xyz += tex2D(_MainTex, pospx / _MainTex_TexelSize.zw).xyz * _Row2.z;

				accumulated.xyz *= _Num / _Den;

				//	Transparent if less than threshold
				if (accumulated.x < _Threshold)
				{
					accumulated.a = 0;
				}
				//	Solid black otherwise
				else
				{
					accumulated.xyz = float3(0, 0, 0);
				}

				//	Return pixel
				return accumulated;
			}

			ENDCG
		}
	} 

	FallBack "Diffuse"
}

