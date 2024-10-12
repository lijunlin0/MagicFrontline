Shader "Custom/TransparentWhiteShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 获取纹理颜色
                fixed4 col = tex2D(_MainTex, i.texcoord);

                // 如果像素不透明（alpha 大于 0），将颜色设为白色
                if (col.a > 0)
                {
                    col.rgb = fixed3(1.0, 1.0, 1.0); // 将有颜色的像素变为白色
                }

                return col;
            }
            ENDCG
        }
    }
}
