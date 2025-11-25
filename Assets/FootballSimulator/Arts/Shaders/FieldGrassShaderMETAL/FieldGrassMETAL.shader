Shader "FieldGrassMETAL"
{
    Properties
    {
        _BaseTexture ("Base Texture", 2D) = "white" {}
        _BaseTexturePower ("Base texture power", Range(0, 1)) = 1

        [HDR] _BaseColor("Base color", Color) = (0, 0.5, 0, 1)
        [HDR] _TopColor("Top color", Color) = (0, 1, 0, 1)
        _TotalHeight("Grass height", Float) = 1

        _FieldLinesTexture("Field lines", 2D) = "white" {}
        [HDR] _FieldLinesColor("Field lines color", Color) = (0, 1, 0, 1)
        
        _FieldLinesNoiseTexture ("_FieldLinesNoiseTexture", 2D) = "white" {}
        _FieldLinesNoisePower ("_FieldLinesNoisePower", float) = 1

        _PatternHorizontalTexture ("_PatternHorizontalTexture", 2D) = "white" {}
        _PatternHorizontalPower ("_PatternHorizontalPower", float) = 1

        _PatternVerticalTexture ("_PatternVerticalTexture", 2D) = "white" {}
        _PatternVerticalPower ("_PatternVerticalPower", float) = 1

        _FieldArea ("_FieldArea", 2D) = "white" {}
        
        [HDR] _OutOfFieldColor ("Out of field color", Color) = (0, 1, 0, 1)
        _OutOfFieldPower ("Out of field color power", float) = 1

        _HeightMap ("Height Map", 2D) = "white" {}
        _HeightMapPower ("Height Map Power", Range(0, 1)) = 1
        _HeightWoundTexture ("Height Wound Map", 2D) = "white" {}
        [HDR] _HeightWoundColor ("Height wound color", Color) = (0, 1, 0, 1)

        _ShadowsTexture ("Shadows Texture", 2D) = "white" {}
        _DirtTexture ("Dirt Texture", 2D) = "white" {}
        _StaticDirtTexture ("Static Dirt Texture", 2D) = "white" {}
        [HDR] _ShadowColor ("Shadow color", Color) = (0, 0, 0, 1)
        [HDR] _DirtWoundColor ("Dirt wound color", Color) = (0, 1, 0, 1)
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
                float2 baseuv : TEXCOORD0;
                float2 fieldlinesuv : TEXCOORD1;
                float2 fieldareauv : TEXTCOORD2;
                float2 dirtuv : TEXCOORD3;
                float2 heightwounduv : TEXCOORD4;
                float2 patterhoriuv : TEXCOORD5;
                float2 pattervertuv : TEXCOORD6;
                float2 fieldlinesnoiseuv : TEXCOORD7;
                float2 shadowsuv : TEXCOORD8;
                UNITY_FOG_COORDS(9)
                float4 vertex : SV_POSITION;
            };

            sampler2D _BaseTexture;
            float4 _BaseTexture_ST;
            
            float _BaseTexturePower;
            float4 _BaseColor;
            float4 _TopColor;
            float _TotalHeight;
            
            sampler2D _FieldLinesTexture;
            float4 _FieldLinesTexture_ST;
            float4 _FieldLinesColor;
            
            sampler2D _FieldLinesNoiseTexture;
            float4 _FieldLinesNoiseTexture_ST;
            float _FieldLinesNoisePower;
            
            sampler2D _PatternHorizontalTexture;
            float4 _PatternHorizontalTexture_ST;
            float _PatternHorizontalPower;
            sampler2D _PatternVerticalTexture;
            float4 _PatternVerticalTexture_ST;
            float _PatternVerticalPower;
            
            sampler2D _FieldArea;
            float4 _FieldArea_ST;
            float _OutOfFieldPower;
            float4 _OutOfFieldColor;
            
            sampler2D _HeightMap;
            float4 _HeightMap_ST;
            float _HeightMapPower;
            float4 _HeightWoundColor;
            sampler2D _HeightWoundTexture;
            float4 _HeightWoundTexture_ST;
            
            sampler2D _ShadowsTexture;
            float4 _ShadowsTexture_ST;
            sampler2D _StaticDirtTexture;
            float4 _StaticDirtTexture_ST;
            sampler2D _DirtTexture;
            float4 _DirtTexture_ST;
            float4 _ShadowColor;
            float4 _DirtWoundColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.baseuv = TRANSFORM_TEX(v.uv, _BaseTexture);
                o.fieldlinesuv = TRANSFORM_TEX(v.uv, _FieldLinesTexture);
                o.fieldareauv = TRANSFORM_TEX(v.uv, _FieldArea);
                o.dirtuv = TRANSFORM_TEX(v.uv, _DirtTexture);
                o.heightwounduv = TRANSFORM_TEX(v.uv, _HeightWoundTexture);
                o.patterhoriuv = TRANSFORM_TEX(v.uv, _PatternHorizontalTexture);
                o.pattervertuv = TRANSFORM_TEX(v.uv, _PatternVerticalTexture);
                o.fieldlinesnoiseuv = TRANSFORM_TEX(v.uv, _FieldLinesNoiseTexture);
                o.shadowsuv = TRANSFORM_TEX(v.uv, _ShadowsTexture);
    
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float heightMap = tex2D(_HeightMap, i.baseuv).r;
                heightMap += (1 - heightMap) * (1 - _HeightMapPower);
                half4 heightWoundSample = tex2D(_HeightWoundTexture, i.baseuv);
    
                float4 fieldArea = tex2D(_FieldArea, i.fieldareauv);
                float fieldAreaAlpha = fieldArea.r;
                float dirtMap = (tex2D(_DirtTexture, i.dirtuv).r * 0.5 + tex2D(_StaticDirtTexture, i.dirtuv).r) * 2 +
                    (1 - fieldAreaAlpha) / 2;
    
                heightMap = lerp(heightMap, 2, min(1, dirtMap));

                float height = heightMap;

                half4 baseTextureColor = tex2D(_BaseTexture, i.baseuv);
    
                half4 albedo = lerp(_BaseColor, _TopColor, height * _TotalHeight);
                    
                float3 white = float3(1, 1, 1);

                albedo = lerp(albedo, _HeightWoundColor * heightWoundSample, heightMap * _HeightMapPower);

                albedo = lerp(albedo, baseTextureColor * _BaseColor, _BaseTexturePower);
    
                float4 black = float4(0, 0, 0, 0);
                float4 patternHorizontal = tex2D(_PatternHorizontalTexture, i.patterhoriuv);
                float4 patternVertical = tex2D(_PatternVerticalTexture, i.pattervertuv);
                float patternHorizontalPower = patternHorizontal.r * _PatternHorizontalPower * fieldArea;
                float patternVerticalPower = patternVertical.r * _PatternVerticalPower * fieldArea;

                float pattern_vert = patternVerticalPower;
                float pattern_hori = patternHorizontalPower;

                albedo = albedo * (1 - pattern_hori) + black * pattern_hori;
                albedo = albedo * (1 - pattern_vert) + black * pattern_vert;

                float4 fieldNoise = tex2D(_FieldLinesNoiseTexture, i.fieldlinesnoiseuv);
                float fieldNoisePower = fieldNoise.r * _FieldLinesNoisePower;

                float4 fieldLines = tex2D(_FieldLinesTexture, i.fieldlinesuv);
                float fieldLineTextureAlpha = fieldLines.r;
                float fieldLineAlpha = fieldAreaAlpha * (_FieldLinesColor.a * fieldLineTextureAlpha + fieldNoisePower * fieldLineTextureAlpha);
                albedo = albedo * (1 - fieldLineAlpha) + _FieldLinesColor * fieldLineAlpha;

                albedo = lerp(albedo, albedo * _OutOfFieldColor, (1 - fieldAreaAlpha) * _OutOfFieldPower);

                albedo = lerp(albedo, heightWoundSample * _DirtWoundColor, dirtMap);

                float3 shadowsMap = tex2D(_ShadowsTexture, i.shadowsuv).rgb;
                albedo = lerp(albedo, _ShadowColor, shadowsMap.r);

                UNITY_APPLY_FOG(i.fogCoord, albedo);
    
                return albedo;
            }
            ENDCG
        }
    }
}
