Shader "Custom/GrayscaleToColorDecal"
{
    Properties
    {
        _GrayscaleTex("Grayscale Texture", 2D) = "white" {}
        _Color1("Color 1", Color) = (1, 0, 0, 1) // rouge
        _Color2("Color 2", Color) = (0, 1, 0, 1) // vert
        _Color3("Color 3", Color) = (0, 0, 1, 1) // bleu
        _AlphaCutoff("Alpha Cutoff", Float) = 0.1
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "Decal" = "Projector" }

        Pass
        {
            Name "Decal"
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma target 3.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"

            TEXTURE2D(_GrayscaleTex);
            SAMPLER(sampler_GrayscaleTex);

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float _AlphaCutoff;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float gray = SAMPLE_TEXTURE2D(_GrayscaleTex, sampler_GrayscaleTex, IN.uv).r;

                float4 col;
                if (gray < 0.33)
                    col = _Color1;
                else if (gray < 0.66)
                    col = _Color2;
                else
                    col = _Color3;

                // Optional alpha discard
                if (col.a < _AlphaCutoff)
                    discard;

                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}