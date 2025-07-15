Shader "Custom/Outline/InvertedHull"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (1,1,0,1)
        _OutlineWidth("Outline Width", Float) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Overlay" }
        Cull Front
        ZWrite Off
        ZTest LEqual

        Pass
        {
            Name "OutlineInvertedHull"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _OutlineColor;
            float _OutlineWidth;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                positionWS += normalWS * _OutlineWidth;

                OUT.positionCS = TransformWorldToHClip(positionWS);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
    Fallback Off
}
