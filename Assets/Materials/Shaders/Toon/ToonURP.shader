Shader "Custom/ToonURP"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ShadowStrength ("Shadow Strength", Range(0,1)) = 0.5
        _Steps ("Toon Steps", Range(1,5)) = 2
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _ShadowStrength;
                float _Steps;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normal = normalize(IN.normalWS);
                float3 lightDir = normalize(_MainLightPosition.xyz);
                float NdotL = saturate(dot(normal, lightDir));

                // Toon steps
                float steps = max(1, _Steps);
                float stepValue = floor(NdotL * steps) / (steps - 1);
                stepValue = saturate(stepValue);

                // Shadow modulation
                float3 lightIntensity = _MainLightColor.rgb; // couleur + intensité de la lumière principale
                float3 shadowColor = _BaseColor.rgb * lerp(1.0, _ShadowStrength, 1.0 - stepValue) * lightIntensity;

                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                return float4(shadowColor, 1.0) * tex;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}