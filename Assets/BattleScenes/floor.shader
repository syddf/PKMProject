Shader "Custom/ToonGroundExam"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _GlowLevel("Glow Level", Range(0.0, 1.0)) = 0.2
        _GlowAttenuation("Glow Attenuation", Range(0.0, 10.0)) = 6.0
        _Zoom("Zoom", Range(1.0, 80.0)) = 8.0
        _TileRadius("Tile Radius", Range(0.0, 1.0)) = 0.45
        _FrameLevel("Frame Level", Range(0.0, 1.0)) = 0.25
        _LitTileThreshold("Lit Tile Threshold", Range(0.0, 1.0)) = 0.75
        _UnlitTileLevel("Unlit Tile Level", Range(0.0, 1.0)) = 0.15

        _PokeBallTex ("PokeBall", 2D) = "black" {}

        _FloorH ("Floor H", Range(0,1)) = 0.0
        _FloorS ("Floor S", Range(0,1)) = 0.0
        _FloorV ("Floor V", Range(0,1)) = 0.0
	}
		SubShader
		{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Tags
			{
				"LightMode" = "UniversalForward"
			}
			Zwrite off
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
 
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ Anti_Aliasing_ON
 
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
 
			struct appdata
			{
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
 
			};
 
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};
 
        sampler2D _MainTex;
        sampler2D _PokeBallTex;
            half _GlowLevel;
            half _GlowAttenuation;
            half _Zoom;
            half _TileRadius;
            half _FrameLevel;
            half _LitTileThreshold;
            half _UnlitTileLevel;
            half _FloorH;
            half _FloorS;
            half _FloorV;

            half _Glossiness;
        half _Metallic;
        half4 _Color;

#define hash21(p) frac(sin(dot(p, half2(12.9898, 78.233))) * 43758.5453)

            // from https://www.shadertoy.com/view/Xt2BDc
            #define hash31(p) frac(sin(dot(p, half3(17, 1527, 113))) * 43758.5453123)

            #define blend(dest, source) dest = lerp(dest, half4(source.rgb, 1.0), source.a)
            #define add(dest, source) dest += source * source.a

            // from https://www.shadertoy.com/view/MsS3Wc
            half3 hsv2rgb(in half3 c)
            {
                half3 rgb = clamp(abs(fmod(c.x * 6.0 + half3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
                return c.z * lerp(half3(1.0, 1.0, 1.0), rgb, c.y);
            }

            // from https://iquilezles.org/articles/distfunctions2d
            half sdBox(in half2 p, in half2 b)
            {
                half2 d = abs(p) - b;
                return length(max(d, half2(0.0, 0.0))) + min(max(d.x, d.y), 0.0);
            }

            void get_tile_colour(in half2 tile_id, out half3 tile_hsv)
            {
                const half lit_tile_threshold = _LitTileThreshold;
                const half unlit_tile_level = _UnlitTileLevel;
                // hue
                tile_hsv.x = _FloorH;
                tile_hsv.y = _FloorS;
                tile_hsv.z = _FloorV;
            }

            #define ADD_GLOW(tile) \
            get_tile_colour(tile_id + tile, tile_hsv); \
            add(fragColor, half4(hsv2rgb(tile_hsv), pow(1.0 - sdBox(tile - tile_coord, half2(tile_radius, tile_radius)), glow_attenuation) * glow_level * tile_hsv.z));

 
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.uv = v.uv;
 
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float4 SHADOW_COORDS = TransformWorldToShadowCoord(i.worldPos);
 
				Light mainLight = GetMainLight(SHADOW_COORDS);
				half shadow = MainLightRealtimeShadow(SHADOW_COORDS);

                 const half glow_level = _GlowLevel;
                const half glow_attenuation = _GlowAttenuation;
                const half zoom = _Zoom;
                const half tile_radius = _TileRadius;
                const half frame_level = _FrameLevel;
                const half lit_tile_threshold = _LitTileThreshold;
                const half unlit_tile_level = _UnlitTileLevel;
                // normalise the coordinates
                half offset = 0.01 / zoom * (1.0 - tile_radius);
                half2 scaleUV[4] = {
                    half2(0, -offset),
                    half2(0, offset),
                    half2(-offset, 0),
                    half2(offset, 0)
                };
                half4 res = half4(0, 0, 0, 1);
                for(int ind = 0; ind < 4; ind++)
                {
                    half2 realUV = i.uv + scaleUV[ind];
                    half2 R = half2(1, 1), U = (((2.0 * realUV) - R) / min(R.x, R.y)) * zoom, FU = frac(U);
                    // unique ID for the tile
                    half2 tile_id = floor(U);
                    // local tile coords [-0.5, 0.5]
                    half2 tile_coord = FU - 0.5;
                    // distance from edge of light
                    half tile_dist = sdBox(tile_coord, half2(tile_radius, tile_radius));

                    // render the frame
                    half4 fragColor = half4(frame_level, frame_level, frame_level, 1.0);

                    // get tile's colour
                    half3 tile_hsv;
                    get_tile_colour(tile_id, tile_hsv);

                    // calculate a vignette to apply to the saturation (from https://www.shadertoy.com/view/lsKSWR)
                    half2 vignette_coord = FU * (1.0 - FU.yx);
                    half vignette = sqrt(vignette_coord.x * vignette_coord.y * 5.0);

                    // render the tile
                    half3 light_colour = hsv2rgb(half3(tile_hsv.x, 1.0 - (tile_hsv.z * vignette), max(tile_hsv.z, unlit_tile_level)));
                    blend(fragColor, half4(light_colour, step(tile_dist, 0.0)));
                    // render the tile's own glow on the frame
                    add(fragColor, half4(hsv2rgb(tile_hsv), pow(1.0 - max(tile_dist, 0.0), glow_attenuation) * glow_level * tile_hsv.z));

                    // get vector to the three nearest neighbours
                    half2 neighbours = half2((tile_coord.x < 0.0) ? -1.0 : 1.0, (tile_coord.y < 0.0) ? -1.0 : 1.0);
                    // render the neighbours' glows
                    ADD_GLOW(neighbours);
                    ADD_GLOW(half2(neighbours.x, 0.0));
                    ADD_GLOW(half2(0.0, neighbours.y));
                    res += fragColor;
                }
            half2 PokeBallUV = 2.0 * i.uv - 1.0;
            /*
            float angle = _Time.y;
            float2x2 rotationMatrix = float2x2(
                cos(angle), -sin(angle),
                sin(angle), cos(angle)
            );

            // 应用旋转矩阵
            PokeBallUV = mul(rotationMatrix, PokeBallUV);
            */
            PokeBallUV = (PokeBallUV * 2.0 + 1.0) * 0.5; 
            half4 pokeCol = tex2D (_PokeBallTex, PokeBallUV);
            half4 c = shadow * res / 4.0 + pokeCol * 0.2f;;

				return float4(c.rgb, 1);
			}
			ENDHLSL
		}

	}
}