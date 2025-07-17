
Shader "Custom/WindOscillation"
{
  Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Float) = 0.05
        _Frequency ("Frequency", Float) = 2.0
        _Direction ("Wind Direction", Vector) = (1,0,0,0)
        _Color1 ("Color A", Color) = (1,0,0,1)
        _Color2 ("Color B", Color) = (0,0,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Amplitude;
            float _Frequency;
            float4 _Direction;
            fixed4 _Color1;
            fixed4 _Color2;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float offset = sin(_Time.y * _Frequency + v.vertex.y * 5.0) * _Amplitude;
                v.vertex.xyz += _Direction.xyz * offset;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = saturate(i.uv.y);
                fixed4 gradientColor = lerp(_Color1, _Color2, t);
                fixed4 col = tex2D(_MainTex, i.uv) * gradientColor;
                return col;
            }
            ENDCG
        }
    }
}
