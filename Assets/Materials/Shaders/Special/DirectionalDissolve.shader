Shader "Custom/DirectionalDissolve"
{
    Properties
    {
        _MainTex ("Main Texture (RGBA)", 2D) = "white" {}
        _MainColor ("Main Color Tint", Color) = (1, 1, 1, 1)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _FillAmount ("Fill Amount", Range(0, 1)) = 0
        _Direction ("Fill Direction", Vector) = (0, 1, 0, 0)
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.2
        _EdgeSmooth ("Edge Smoothness", Range(0.001, 0.2)) = 0.05
        _GlowColor ("Glow Color", Color) = (1, 0.5, 0, 1)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 2
        _ProjectionScale ("Projection Scale", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Name "DissolvePass"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 localPos : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainColor;
                float _FillAmount;
                float4 _Direction;
                float _NoiseStrength;
                float _EdgeSmooth;
                float4 _GlowColor;
                float _GlowIntensity;
                float _ProjectionScale;
            CBUFFER_END

            TEXTURE2D(_MainTex);        SAMPLER(sampler_MainTex);
            TEXTURE2D(_NoiseTex);       SAMPLER(sampler_NoiseTex);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.localPos = IN.positionOS.xyz;
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float projection = dot(IN.localPos, normalize(_Direction.xyz)) / _ProjectionScale;
                float fillThreshold = lerp(-1, 1, _FillAmount);
                float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, IN.uv).r;
                float offset = noise * _NoiseStrength;
                float dissolve = fillThreshold - projection + offset;
                float edgeAlpha = smoothstep(0.0, _EdgeSmooth, dissolve);

                clip(edgeAlpha - 0.01);

                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _MainColor;
                float finalAlpha = texColor.a * edgeAlpha;
                float glowAmount = (1.0 - edgeAlpha);
                float3 glow = glowAmount * _GlowColor.rgb * _GlowIntensity;

                return float4(texColor.rgb + glow, finalAlpha);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
