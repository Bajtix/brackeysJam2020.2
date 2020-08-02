Shader "Hidden/Saturator"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        Saturation ("Saturation Amount",Float) = 1
            [Space]
        Saturated ("Saturated +",Float) = 1
        Desaturated ("Saturated -",Float) = 0
    } 
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float Saturation;
            float Saturated;
            float Desaturated;

            float3 Grayscale(float3 inputColor)
            {
                float gray = (inputColor.r + inputColor.g + inputColor.b) / 3;
                return float3(gray, gray, gray);
            }
            float map(float value, float min1, float max1, float min2, float max2) {
                return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
            }
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                float gradientPos = 1 - i.uv.y;

                if (gradientPos < Saturation) {
                    gradientPos = Saturated;
                }
                else {
                    gradientPos = Desaturated;
                }
                
                col.rgb = lerp(Grayscale(col.rgb),col.rgb,gradientPos);
                return col;
            }


            
            ENDCG
        }
    }
}
