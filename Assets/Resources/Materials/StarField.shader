Shader "Unlit/StarField"
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

            #define NUM_LAYERS 8.
            #define TAU 6.28318
            #define PI 3.141592
            #define Velocity .25 //modified value to increse or decrease speed, negative value travel backwards
            #define StarGlow 0.025
            #define StarSize 0.1
            #define CanvasView 60.
            
            
            float Star(float2 uv, float flare){
                float d = length(uv);
                  float m = sin(StarGlow*1.2)/d;  
                float rays = max(0., .5-abs(uv.x*uv.y*1000.)); 
                m += (rays*flare)*2.;
                m *= smoothstep(1., .1, d);
                return m;
            }
            
            float Hash21(float2 p){
                p = frac(p*float2(123.34, 456.21));
                p += dot(p, p+45.32);
                return frac(p.x*p.y);
            }
            
            
            float3 StarLayer(float2 uv){
                float3 col = float3(0,0,0);
                float2 gv = frac(uv);
                float2 id = floor(uv);
                for(int y=-1;y<=1;y++){
                    for(int x=-1; x<=1; x++){
                        float2 offs = float2(x,y);
                        float n = Hash21(id+offs);
                        float size = frac(n);
                        float star = Star(gv-offs-float2(n, frac(n*34.))+.5, smoothstep(.1,.9,size)*.46);
                        float3 color = sin(float3(.2,.3,.9)*frac(n*2345.2)*TAU)*.25+.75;
                        color = color*float3(.9,.59,.9+size);
                        star *= sin(_Time.x*.6+n*TAU)*.5+.5;
                        col += star*size*color;
                    }
                }
                return col;
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
                float2 uv = float2(i.uv.x - 0.5f, i.uv.y - 0.5f);
                uv.y = uv.y * 1080.0 / 1920.0;
                float2 M = float2(0,0);
                M -= float2(M.x+sin(_Time.x*0.22), M.y-cos(_Time.x*0.22));
                float t = _Time.x*Velocity; 
                float3 col = float3(0,0,0);  
                for(float i=0.; i<1.; i+=1./NUM_LAYERS){
                    float depth = frac(i+t);
                    float scale = lerp(CanvasView, .5, depth);
                    float fade = depth*smoothstep(1.,.9,depth);
                    col += StarLayer(uv*scale+i*453.2-_Time.x*.05+M)*fade;}   
                float4 fragColor = float4(col,1.0);
                return fragColor;
            }
            ENDCG
        }
    }
}
