Shader "Custom/PoolWaves"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveAmplitude ("Wave Amplitude", Float) = 0.1
        _WaveFrequency ("Wave Frequency", Float) = 1.0
        _WaveSpeed ("Wave Speed", Float) = 1.0
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha  // Устанавливает альфа-прозрачность
        ZWrite Off                      // Отключает запись в Z-буфер для корректной сортировки прозрачных объектов

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WaveAmplitude;
            float _WaveFrequency;
            float _WaveSpeed;

            v2f vert (appdata v)
            {
                v2f o;

                // Apply wave effect
                float wave = sin(v.vertex.x * _WaveFrequency + _Time * _WaveSpeed) * _WaveAmplitude;
                v.vertex.y += wave;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}
