Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color("Main Color", Color) = (.5,.5,.5,1)
        _OutlineColor("Outline Color", Color) = (1,0,0,1)
        _MainTex("Base (RGB)", 2D) = "white" { }
        _Outline("Outline width", Range(0.002, 0.03)) = 0.005
    }

        SubShader
        {
            Tags { "Queue" = "Overlay" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            struct Input
            {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;

            void surf(Input IN, inout SurfaceOutput o)
            {
                // 主体颜色
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }

            SubShader
            {
                Tags { "Queue" = "Overlay" }
                LOD 100

                CGPROGRAM
                #pragma surface surf Lambert

                sampler2D _MainTex;
                fixed4 _Color;
                fixed4 _OutlineColor;
                float _Outline;

                struct Input
                {
                    float2 uv_MainTex;
                };

                void surf(Input IN, inout SurfaceOutput o)
                {
                    // 主体颜色
                    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

                    o.Albedo = c.rgb;
                    o.Alpha = c.a;

                    // 计算描边
                    float4 outColor;
                    fixed4 curColor = c;
                    fixed4 prevColor = tex2D(_MainTex, IN.uv_MainTex + float2(-_Outline, 0));
                    if (length(curColor - prevColor) > 0.1)
                        outColor = _OutlineColor;
                    else
                    {
                        prevColor = tex2D(_MainTex, IN.uv_MainTex + float2(_Outline, 0));
                        if (length(curColor - prevColor) > 0.1)
                            outColor = _OutlineColor;
                        else
                        {
                            prevColor = tex2D(_MainTex, IN.uv_MainTex + float2(0, -_Outline));
                            if (length(curColor - prevColor) > 0.1)
                                outColor = _OutlineColor;
                            else
                            {
                                prevColor = tex2D(_MainTex, IN.uv_MainTex + float2(0, _Outline));
                                if (length(curColor - prevColor) > 0.1)
                                    outColor = _OutlineColor;
                                else
                                    outColor = c;
                            }
                        }
                    }

                    o.Albedo = outColor.rgb;
                    o.Alpha = outColor.a;
                }
                ENDCG
            }
}