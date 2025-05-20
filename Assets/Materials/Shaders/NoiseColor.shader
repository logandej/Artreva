Shader "Custom/NoiseColor"
{
    Properties
    {
        _MainTex("Noise Texture", 2D) = "white" {}
        _Color1("Color 1", Color) = (1, 0, 0, 1)
        _Color2("Color 2", Color) = (0, 1, 0, 1)
        _Color3("Color 3", Color) = (0, 0, 1, 1)
        _NoiseScale("Noise Scale", Float) = 5
        _BlendSpeed("Blend Speed", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            Name "StylizedPass"
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
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float _NoiseScale;
            float _BlendSpeed;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 noiseUV = IN.uv * _NoiseScale + _Time.y * _BlendSpeed;
                float noiseVal = tex2D(_MainTex, noiseUV).r;

                float3 blended = lerp(_Color1.rgb, _Color2.rgb, noiseVal);
                blended = lerp(blended, _Color3.rgb, sin(noiseVal * 3.14159));

                return float4(blended, 1);
            }
            ENDHLSL
        }
    }
}
