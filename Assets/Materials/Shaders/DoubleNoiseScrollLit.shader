
Shader "Custom/DoubleNoiseScrollLit"
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
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"
    ENDHLSL

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 300

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_fog
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _SHADOWS_SOFT

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
            };

            sampler2D _NoiseA;
            sampler2D _NoiseB;

            float4 _ColorA;
            float4 _ColorB;
            float2 _ScrollDirA;
            float2 _ScrollDirB;
            float _ScrollSpeedA;
            float _ScrollSpeedB;
            float _BlendSharpness;
            float _Smoothness;
            float _Metallic;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float2 uvA = IN.uv + _ScrollDirA * _ScrollSpeedA * _Time.y;
                float2 uvB = IN.uv + _ScrollDirB * _ScrollSpeedB * _Time.y;

                float noiseA = tex2D(_NoiseA, uvA).r;
                float noiseB = tex2D(_NoiseB, uvB).r;

                float blend = smoothstep(0.0, 1.0, (noiseA - noiseB) * _BlendSharpness + 0.5);
                float3 finalColor = lerp(_ColorA.rgb, _ColorB.rgb, blend);

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = finalColor;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.normalTS = float3(0, 0, 1);
                surfaceData.occlusion = 1;
                surfaceData.emission = 0;
                surfaceData.alpha = 1;

                InputData inputData = (InputData)0;
                inputData.positionWS = IN.positionWS;
                inputData.normalWS = normalize(IN.normalWS);
                inputData.viewDirectionWS = normalize(GetWorldSpaceViewDir(IN.positionWS));
                inputData.shadowCoord = TransformWorldToShadowCoord(IN.positionWS);
                inputData.fogCoord = 0;
                inputData.vertexLighting = float3(0, 0, 0);
                inputData.bakedGI = SampleSH(inputData.normalWS);

                return UniversalFragmentPBR(inputData, surfaceData);
            }

            ENDHLSL
        }
    }
}
