Shader "Custom/DoubleNoiseBlend"
{
 Properties
    {
        _NoiseA("Noise Texture A", 2D) = "white" {}
        _NoiseB("Noise Texture B", 2D) = "black" {}

        _ScrollDirA("Scroll Direction A", Vector) = (1, 0, 0, 0)
        _ScrollDirB("Scroll Direction B", Vector) = (-1, 0, 0, 0)

        _ScrollSpeedA("Scroll Speed A", Float) = 0.5
        _ScrollSpeedB("Scroll Speed B", Float) = 0.5

        _ColorA("Color A", Color) = (1, 0, 0, 1)
        _ColorB("Color B", Color) = (0, 0, 1, 1)

        _BlendSharpness("Blend Sharpness", Range(0.01, 10)) = 2.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "Forward"
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

            sampler2D _NoiseA;
            sampler2D _NoiseB;
            float4 _NoiseA_ST;
            float4 _NoiseB_ST;

            float2 _ScrollDirA;
            float2 _ScrollDirB;
            float _ScrollSpeedA;
            float _ScrollSpeedB;

            float4 _ColorA;
            float4 _ColorB;
            float _BlendSharpness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uvA = IN.uv + _ScrollDirA * _ScrollSpeedA * _Time.y;
                float2 uvB = IN.uv + _ScrollDirB * _ScrollSpeedB * _Time.y;

                float noiseA = tex2D(_NoiseA, uvA).r;
                float noiseB = tex2D(_NoiseB, uvB).r;

                // Mélange non-linéaire des deux bruitages
                float blendVal = smoothstep(0.0, 1.0, (noiseA - noiseB) * _BlendSharpness + 0.5);
                float3 finalColor = lerp(_ColorA.rgb, _ColorB.rgb, blendVal);

                return float4(finalColor, 1.0);
            }

            ENDHLSL
        }
    }
}
