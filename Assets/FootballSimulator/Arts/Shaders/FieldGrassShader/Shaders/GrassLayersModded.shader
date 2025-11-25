
Shader "Grass/GrassLayersModded" {
    Properties{
        _BaseTexture ("Base Texture", 2D) = "white" {}
        _BaseTexturePower ("Base texture power", Range(0, 1)) = 1

        [HDR] _BaseColor("Base color", Color) = (0, 0.5, 0, 1)
        [HDR] _TopColor("Top color", Color) = (0, 1, 0, 1)
        _TotalHeight("Grass height", Float) = 1
        _DetailNoiseTexture("Grainy noise", 2D) = "white" {}
        _DetailDepthScale("Grainy depth scale", Range(0, 1)) = 1
        _SmoothNoiseTexture("Smooth noise", 2D) = "white" {}
        _SmoothDepthScale("Smooth depth scale", Range(0, 1)) = 1

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

    SubShader {
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}

        Pass {

            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}
            Cull Back

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma require geometry

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fog

            #pragma vertex Vertex
            #pragma geometry Geometry
            #pragma fragment Fragment

            #include "GrassLayers.hlsl"    

            ENDHLSL
        }

        Pass {

            Name "ShadowCaster"
            Tags {"LightMode" = "ShadowCaster"}

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma require geometry

            #pragma multi_compile_shadowcaster

            #pragma vertex Vertex
            #pragma geometry Geometry
            #pragma fragment Fragment

            #define SHADOW_CASTER_PASS

            #include "GrassLayers.hlsl"

            ENDHLSL
        }
    }
}
