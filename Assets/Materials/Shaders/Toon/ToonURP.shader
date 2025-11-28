Shader "Custom/ToonURP"
{
    Properties
    {
        _BaseColor      ("Base Color",      Color) = (1, 1, 1, 1)
        _ShadowStrength ("Shadow Strength", Range(0, 1)) = 0.5
        _Steps          ("Toon Steps",      Range(1, 5)) = 2
        _MainTex        ("Texture",         2D)   = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "Queue"      = "Geometry"
            "RenderPipeline" = "UniversalRenderPipeline"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // ---------------------------------------
            // CBUFFER SRP (vide mais requis)
            // ---------------------------------------
            CBUFFER_START(UnityPerMaterial)
            CBUFFER_END

            // ---------------------------------------
            // GPU-instanced material properties
            // ---------------------------------------
            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _ShadowStrength)
                UNITY_DEFINE_INSTANCED_PROP(float, _Steps)
            UNITY_INSTANCING_BUFFER_END(Props)

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float2 uv          : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.normalWS    = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv          = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);

                float4 baseCol   = UNITY_ACCESS_INSTANCED_PROP(Props, _BaseColor);
                float  shadows   = UNITY_ACCESS_INSTANCED_PROP(Props, _ShadowStrength);
                float  steps     = UNITY_ACCESS_INSTANCED_PROP(Props, _Steps);

                float3 normal    = normalize(IN.normalWS);
                float3 lightDir  = normalize(_MainLightPosition.xyz);
                float  NdotL     = saturate(dot(normal, lightDir));

                steps            = max(1, steps);
                float stepValue  = floor(NdotL * steps) / (steps - 1);
                stepValue        = saturate(stepValue);

                float3 lightIntensity = _MainLightColor.rgb;

                float3 shadowColor =
                    baseCol.rgb *
                    lerp(1.0, shadows, 1.0 - stepValue) *
                    lightIntensity;

                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);

                return float4(shadowColor, 1.0) * tex;
            }

            ENDHLSL
        }
    }
}
