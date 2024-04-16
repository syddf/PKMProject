Shader "Unlit/FlowTex"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            
            float rand(float2 co){
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453)*5.0;
            }
            float rand2(float2 co){
                return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453)*50.0;
            }
            float4 thunder( float2 fragCoord, float iTime)
            {
                float3 baseColor = float3(0.7f, 0.7f, 0.7f);
                float4 fragColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
                float2 uv = fragCoord;
                uv.x += sin(iTime);
                
                float bg = (cos(uv.x*3.14159*2.0) + sin((uv.y)*3.14159)) * 0.15;
                
                float2 p = uv*2.0 - 1.0;
                p *= 15.0;
                
                float2 sfunc = float2(p.x, p.y + 5.0*sin(uv.x*10.0-iTime*20.0 )+2.0*sin(uv.x*25.0+iTime*40.0));

                
                sfunc.y = p.y + rand(float2(iTime, 0.0))*sin(uv.x*rand2(float2(iTime, 1.0)))
                    +rand(float2(iTime, 2.0))*sin(uv.x*rand2(float2(iTime, 2.0)));
                sfunc.y +=p.y + rand(float2(iTime, 1.0))*sin(uv.x*rand2(float2(iTime, 3.0)))
                    +rand(float2(iTime, 4.0))*sin(uv.x*rand2(float2(iTime, 4.0)));
                sfunc.y += p.y + rand(float2(iTime, 2.0))*sin(uv.x*rand(float2(iTime, 5.0)))
                    +rand(float2(iTime, 6.0))*sin(uv.x*rand2(float2(iTime, 6.0)));
                sfunc.y += p.y + rand(float2(iTime, 3.0))*sin(uv.x*rand(float2(iTime, 7.0)))
                    +rand(float2(iTime, 8.0))*sin(uv.x*rand2(float2(iTime, 8.0)));
                sfunc.y /= 0.1; // Thickness fix
                float3 c = float3(abs(sfunc.y), abs(sfunc.y), abs(sfunc.y));
                c = pow(c, float3(-0.7, -0.7, -0.7));
                c *= baseColor;
                fragColor = float4(c,1.0);
                
                sfunc.y = p.y + rand(float2(iTime, 0.1))*sin(uv.x*rand2(float2(iTime, 1.1)))
                    +rand(float2(iTime, 2.1))*sin(uv.x*rand2(float2(iTime, 2.1)));
                sfunc.y +=p.y + rand(float2(iTime, 1.1))*sin(uv.x*rand2(float2(iTime, 3.1)))
                    +rand(float2(iTime, 4.1))*sin(uv.x*rand2(float2(iTime, 4.1)));
                sfunc.y += p.y + rand(float2(iTime, 2.1))*sin(uv.x*rand(float2(iTime, 5.1)))
                    +rand(float2(iTime, 6.1))*sin(uv.x*rand2(float2(iTime, 6.1)));
                sfunc.y += p.y + rand(float2(iTime, 3.1))*sin(uv.x*rand(float2(iTime, 7.1)))
                    +rand(float2(iTime, 8.1))*sin(uv.x*rand2(float2(iTime, 8.1)));
                sfunc.y /= 0.5; // Thickness fix
                c = float3(abs(sfunc.y), abs(sfunc.y),abs(sfunc.y));
                c = pow(c, float3(-0.7, -0.7, -0.7));
                c *= baseColor;
                fragColor += float4(c,1.0);
                
                sfunc.y = p.y + rand(float2(iTime, 0.0))*sin(uv.x*rand2(float2(iTime, 1.4)))
                    +rand(float2(iTime, 2.4))*sin(uv.x*rand2(float2(iTime, 2.0)));
                sfunc.y +=p.y + rand(float2(iTime, 1.0))*sin(uv.x*rand2(float2(iTime, 3.4)))
                    +rand(float2(iTime, 4.4))*sin(uv.x*rand2(float2(iTime, 4.0)));
                sfunc.y += p.y + rand(float2(iTime, 2.0))*sin(uv.x*rand(float2(iTime, 5.4)))
                    +rand(float2(iTime, 6.4))*sin(uv.x*rand2(float2(iTime, 6.0)));
                sfunc.y += p.y + rand(float2(iTime, 3.0))*sin(uv.x*rand(float2(iTime, 7.4)))
                    +rand(float2(iTime, 8.4))*sin(uv.x*rand2(float2(iTime, 8.0)));
                sfunc.y /= 1.0; // Thickness fix
                c = float3(abs(sfunc.y), abs(sfunc.y), abs(sfunc.y));
                c = pow(c, float3(-0.7, -0.7, -0.7));
                c *= baseColor;
                fragColor += float4(c,1.0);
                
                sfunc.y = p.y + rand(float2(iTime, 0.2))*sin(uv.x*rand2(float2(iTime, 1.2)))
                    +rand(float2(iTime, 2.2))*sin(uv.x*rand2(float2(iTime, 2.2)));
                sfunc.y +=p.y + rand(float2(iTime, 1.2))*sin(uv.x*rand2(float2(iTime, 3.2)))
                    +rand(float2(iTime, 4.2))*sin(uv.x*rand2(float2(iTime, 4.2)));
                sfunc.y += p.y + rand(float2(iTime, 2.2))*sin(uv.x*rand(float2(iTime, 5.2)))
                    +rand(float2(iTime, 6.2))*sin(uv.x*rand2(float2(iTime, 6.2)));
                sfunc.y += p.y + rand(float2(iTime, 3.2))*sin(uv.x*rand(float2(iTime, 7.2)))
                    +rand(float2(iTime, 8.2))*sin(uv.x*rand2(float2(iTime, 8.2)));
                sfunc.y /= 10.0; // Thickness fix
                c = float3(abs(sfunc.y), abs(sfunc.y), abs(sfunc.y));
                c = pow(c, float3(-0.7, -0.7, -0.7));
                c *= baseColor;
                fragColor += float4(c, 1.0);


                return fragColor;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x += 1.0f * _Time.y;
                // sample the texture
                fixed4 col = tex2D(_MainTex, uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col + thunder(uv, _Time.y);
            }
            ENDCG
        }
    }
}
