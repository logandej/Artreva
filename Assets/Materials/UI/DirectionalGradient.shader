Shader "UI/DirectionalGradient"
{
    Properties
    {
        _MainTex ("Sprite", 2D) = "white" {}
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (0,0,0,1)
        _Direction ("Direction", Vector) = (1,0,0,0)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _ColorA;
            fixed4 _ColorB;
            float4 _Direction;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 localPos : TEXCOORD1;
            };

            float2 minBounds;
            float2 maxBounds;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Transform world to local normalized space
                o.localPos = v.vertex.xy;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 dir = normalize(_Direction.xy);
                
                // Convert local position to normalized [0,1] space based on direction
                float projection = dot(i.localPos, dir);
                float gradient = saturate(projection * 0.5 + 0.5); // Map [-1,1] -> [0,1]

                fixed4 texCol = tex2D(_MainTex, i.uv);
                fixed4 gradCol = lerp(_ColorA, _ColorB, gradient);
                gradCol.a *= texCol.a;

                return gradCol;
            }
            ENDCG
        }
    }
}