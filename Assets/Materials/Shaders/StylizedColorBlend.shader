Shader "Custom/StylizedColorBlend"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
        _Color3 ("Color 3", Color) = (0, 0, 1, 1)
        _Speed ("Cycle Speed", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "ColorPass"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float _Speed;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target
            {
                float t = frac(_Time.y * _Speed);

                // On fait une boucle colorée 1 -> 2 -> 3 -> 1
                float3 col;
                if (t < 0.33)
                {
                    float localT = t / 0.33;
                    col = lerp(_Color1.rgb, _Color2.rgb, localT);
                }
                else if (t < 0.66)
                {
                    float localT = (t - 0.33) / 0.33;
                    col = lerp(_Color2.rgb, _Color3.rgb, localT);
                }
                else
                {
                    float localT = (t - 0.66) / 0.34;
                    col = lerp(_Color3.rgb, _Color1.rgb, localT);
                }

                return float4(col, 1);
            }
            ENDHLSL
        }
    }
}

