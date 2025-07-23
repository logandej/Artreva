Shader "Custom/AnimatedCrowdShader"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
        _Color3 ("Color 3", Color) = (0, 0, 1, 1)
        _Color4 ("Color 4", Color) = (1, 1, 0, 1)
        _Color5 ("Color 5", Color) = (1, 0, 1, 1)
        _Speed ("Wiggle Speed", Float) = 2.0
        _Amplitude ("Wiggle Amplitude", Float) = 0.02
        _UpperBodyStartY ("Upper Body Y Threshold", Float) = 0.9
        _FadeZone ("Fade zone size", Float) = 0.2
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 color      : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float, _RandomSeed)
                UNITY_DEFINE_INSTANCED_PROP(int, _ColorIndex)
            UNITY_INSTANCING_BUFFER_END(Props)

            float4 _Color1, _Color2, _Color3, _Color4, _Color5;
            float _UpperBodyStartY;
            float _Speed, _Amplitude;
            float _FadeZone;

            float4 GetRandomColor(int index)
            {
                if (index == 0) return _Color1;
                if (index == 1) return _Color2;
                if (index == 2) return _Color3;
                if (index == 3) return _Color4;
                return _Color5;
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

                float rand = UNITY_ACCESS_INSTANCED_PROP(Props, _RandomSeed);
                float time = _Time.y * _Speed + rand;

                // Poids d'influence en fonction de la hauteur (Y locale)
                float fadeStart = _UpperBodyStartY;
                float fadeEnd = _UpperBodyStartY + _FadeZone;
                float t = saturate((IN.positionOS.z - fadeStart) / _FadeZone);

                float3 wiggleOffset = float3(0, sin(time) * (_Amplitude/2) * t , sin(time) * _Amplitude * t);
                float3 finalPos = IN.positionOS.xyz + wiggleOffset;

                OUT.positionCS = TransformObjectToHClip(finalPos);
                OUT.color = GetRandomColor((int)UNITY_ACCESS_INSTANCED_PROP(Props, _ColorIndex));
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return IN.color;
            }
            ENDHLSL
        }
    }
}