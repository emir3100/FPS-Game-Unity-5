/*

Shader based on @Dickies Normal Injection Decal

*/

Shader "Triplebrick/Decal"
{
	Properties
	{
		_OpacityClip("Clip", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "bump" {}
	}
	SubShader
	{
		Name "DEFERRED"
		Tags { 
			"LightMode" = "Deferred" 
			"Queue" = "Geometry+10"
			}
		LOD 100

		Pass
		{
		Offset -1, -1
		zwrite off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;

				  float4 tSpace0 : TEXCOORD1;
				  float4 tSpace1 : TEXCOORD2;
				  float4 tSpace2 : TEXCOORD3;

				float4 vertex : SV_POSITION;
			};

			sampler2D _NormalTex;
			float4 _NormalTex_ST;
			sampler2D _OpacityClip;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _NormalTex);

				// copied all below out of a compiled standard surface shader
				// had tried just transforming to world space on my own 
				// but I figured the unity devs probably know better
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0 = float4(worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x);
				o.tSpace1 = float4(worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y);
				o.tSpace2 = float4(worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z);

				return o;
			}

			// the outGBuffer2 is the render target being 'injected' into
			// could easily change it to 0 if you wanted to change the base color or w/e
			void frag (v2f i, out half4 outGBuffer2 : SV_Target2)
			{

				half3 norm = UnpackNormal(tex2D(_NormalTex, i.uv));

				clip(tex2D(_OpacityClip,i.uv).a - 0.5);

				// again from surface shader output
				// but converts from tanget space to world
				fixed3 worldN;
				worldN.x = dot(i.tSpace0.xyz, norm);
				worldN.y = dot(i.tSpace1.xyz, norm);
				worldN.z = dot(i.tSpace2.xyz, norm);
				worldN = worldN * 0.5f + 0.5f;
				
				outGBuffer2 = float4( worldN, 0);
			}
			ENDCG
		}
	}
}