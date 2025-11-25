Shader "Converted/TeamLogoUIShader"
{
    Properties
    {
        [NoScaleOffset]_MaskTexture("MaskTexture", 2D) = "white" {}
        _FirstColor("FirstColor", Color) = (0, 0, 0, 0)
        Vector1_20ec970ab7554c17bd5ed89e9bda7ec1("ColorRange", Float) = 0
        Vector1_015703dd126b46c1bbf10073d763c78e("ColorFuzziness", Float) = 0
        _SecondColor("SecondColor", Color) = (0, 0, 0, 0)
        [ToggleUI]Boolean_9adaed0c6de84900aab724c15134aad9("ColorizeEffect", Float) = 1
        [NoScaleOffset]Texture2D_e1fa2c08b38046549cb9a8682d5c7f15("ColorizeEffectAlphaTexture", 2D) = "white" {}
        Vector1_0f3cb6e470c245b1b027cee0824b2e1e("ColorizeSpeed", Float) = 1
        Vector2_4ca45a0a63824ce79e034b016ff60cee("ColorizeScale", Vector) = (1, 1, 0, 0)
        Vector1_10e12b7709ea4ef7980e6b2beffbd6fe("ColorizeMinAlpha", Float) = 0
        [NoScaleOffset]_MainTex("MainTex (unused)", 2D) = "white" {}
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
_StencilComp ("Stencil Comparison", Float) = 8.000000         
_Stencil ("Stencil ID", Float) = 0.000000                     
_StencilOp ("Stencil Operation", Float) = 0.000000            
_StencilWriteMask ("Stencil Write Mask", Float) = 255.000000  
_StencilReadMask ("Stencil Read Mask", Float) = 255.000000    
_ColorMask ("Color Mask", Float) = 15.000000                  

        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
ZTest [unity_GUIZTestMode]
        ZWrite Off

Stencil {                      
Ref [_Stencil]                 
ReadMask [_StencilReadMask]    
WriteMask [_StencilWriteMask]  
Comp [_StencilComp]            
Pass [_StencilOp]              
}                              


        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
             float4 color;
             float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
             float3 interp4 : INTERP4;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            output.interp4.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_ColorMask_float(float3 In, float3 MaskColor, float Range, out float Out, float Fuzziness)
        {
            float Distance = distance(MaskColor, In);
            Out = saturate(1 - (Distance - Range) / max(Fuzziness, 1e-5));
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_440b1e7a1f8c4827be8947d3d4027b38_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0 = SAMPLE_TEXTURE2D(_Property_440b1e7a1f8c4827be8947d3d4027b38_Out_0.tex, _Property_440b1e7a1f8c4827be8947d3d4027b38_Out_0.samplerstate, _Property_440b1e7a1f8c4827be8947d3d4027b38_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_c7f3d923e5714684b585ae452327aada_R_4 = _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.r;
            float _SampleTexture2D_c7f3d923e5714684b585ae452327aada_G_5 = _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.g;
            float _SampleTexture2D_c7f3d923e5714684b585ae452327aada_B_6 = _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.b;
            float _SampleTexture2D_c7f3d923e5714684b585ae452327aada_A_7 = _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.a;
            float4 _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGBA_4;
            float3 _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGB_5;
            float2 _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RG_6;
            Unity_Combine_float(_SampleTexture2D_c7f3d923e5714684b585ae452327aada_R_4, _SampleTexture2D_c7f3d923e5714684b585ae452327aada_G_5, _SampleTexture2D_c7f3d923e5714684b585ae452327aada_B_6, 0, _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGBA_4, _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGB_5, _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RG_6);
            float _Property_201be6ac2d7e4f32933b2b9e4594e27c_Out_0 = Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
            float _Property_1c9228ddd3164bebb4e09a93a979d09f_Out_0 = Vector1_015703dd126b46c1bbf10073d763c78e;
            float _ColorMask_0216fb3bfe8b4db5b18fcba37762366f_Out_3;
            Unity_ColorMask_float(_Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGB_5, IsGammaSpace() ? float3(1, 0, 0) : SRGBToLinear(float3(1, 0, 0)), _Property_201be6ac2d7e4f32933b2b9e4594e27c_Out_0, _ColorMask_0216fb3bfe8b4db5b18fcba37762366f_Out_3, _Property_1c9228ddd3164bebb4e09a93a979d09f_Out_0);
            float4 _Property_1eebfd3faede4caa9491171cceb2c713_Out_0 = _FirstColor;
            float4 _Multiply_230a389ca52d4d83b03743dbbf24c425_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_0216fb3bfe8b4db5b18fcba37762366f_Out_3.xxxx), _Property_1eebfd3faede4caa9491171cceb2c713_Out_0, _Multiply_230a389ca52d4d83b03743dbbf24c425_Out_2);
            float _Property_47d40eca9e364770b00760452a2c5782_Out_0 = Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
            float _Property_796ec4ca311b479fbac33d11855f7268_Out_0 = Vector1_015703dd126b46c1bbf10073d763c78e;
            float _ColorMask_9357c0eac8e34ad3b92ae44d8179d1f4_Out_3;
            Unity_ColorMask_float((_SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.xyz), IsGammaSpace() ? float3(0, 0, 1) : SRGBToLinear(float3(0, 0, 1)), _Property_47d40eca9e364770b00760452a2c5782_Out_0, _ColorMask_9357c0eac8e34ad3b92ae44d8179d1f4_Out_3, _Property_796ec4ca311b479fbac33d11855f7268_Out_0);
            float4 _Property_c4b990dff59e419ba5606a2332f47114_Out_0 = _SecondColor;
            float4 _Multiply_588211649a0c47b6ba7deba6c658392b_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_9357c0eac8e34ad3b92ae44d8179d1f4_Out_3.xxxx), _Property_c4b990dff59e419ba5606a2332f47114_Out_0, _Multiply_588211649a0c47b6ba7deba6c658392b_Out_2);
            float4 _Add_a565a405238940c78f538912332ad434_Out_2;
            Unity_Add_float4(_Multiply_230a389ca52d4d83b03743dbbf24c425_Out_2, _Multiply_588211649a0c47b6ba7deba6c658392b_Out_2, _Add_a565a405238940c78f538912332ad434_Out_2);
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.BaseColor = (_Add_a565a405238940c78f538912332ad434_Out_2.xyz);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormalsOnly"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
             float4 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
             float4 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 4.5
        #pragma exclude_renderers gles gles3 glcore
        #pragma multi_compile_instancing
        #pragma multi_compile _ DOTS_INSTANCING_ON
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalUnlitSubTarget"
        }
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                // LightMode: <None>
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma shader_feature _ _SAMPLE_GI
        #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_VIEWDIRECTION_WS
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define _FOG_FRAGMENT 1
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 texCoord0;
             float4 color;
             float3 viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
             float3 interp4 : INTERP4;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            output.interp4.xyz =  input.viewDirectionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_ColorMask_float(float3 In, float3 MaskColor, float Range, out float Out, float Fuzziness)
        {
            float Distance = distance(MaskColor, In);
            Out = saturate(1 - (Distance - Range) / max(Fuzziness, 1e-5));
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_440b1e7a1f8c4827be8947d3d4027b38_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0 = SAMPLE_TEXTURE2D(_Property_440b1e7a1f8c4827be8947d3d4027b38_Out_0.tex, _Property_440b1e7a1f8c4827be8947d3d4027b38_Out_0.samplerstate, _Property_440b1e7a1f8c4827be8947d3d4027b38_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_c7f3d923e5714684b585ae452327aada_R_4 = _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.r;
            float _SampleTexture2D_c7f3d923e5714684b585ae452327aada_G_5 = _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.g;
            float _SampleTexture2D_c7f3d923e5714684b585ae452327aada_B_6 = _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.b;
            float _SampleTexture2D_c7f3d923e5714684b585ae452327aada_A_7 = _SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.a;
            float4 _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGBA_4;
            float3 _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGB_5;
            float2 _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RG_6;
            Unity_Combine_float(_SampleTexture2D_c7f3d923e5714684b585ae452327aada_R_4, _SampleTexture2D_c7f3d923e5714684b585ae452327aada_G_5, _SampleTexture2D_c7f3d923e5714684b585ae452327aada_B_6, 0, _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGBA_4, _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGB_5, _Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RG_6);
            float _Property_201be6ac2d7e4f32933b2b9e4594e27c_Out_0 = Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
            float _Property_1c9228ddd3164bebb4e09a93a979d09f_Out_0 = Vector1_015703dd126b46c1bbf10073d763c78e;
            float _ColorMask_0216fb3bfe8b4db5b18fcba37762366f_Out_3;
            Unity_ColorMask_float(_Combine_45bc1e9b3c0c41fc91b76630070b8ef0_RGB_5, IsGammaSpace() ? float3(1, 0, 0) : SRGBToLinear(float3(1, 0, 0)), _Property_201be6ac2d7e4f32933b2b9e4594e27c_Out_0, _ColorMask_0216fb3bfe8b4db5b18fcba37762366f_Out_3, _Property_1c9228ddd3164bebb4e09a93a979d09f_Out_0);
            float4 _Property_1eebfd3faede4caa9491171cceb2c713_Out_0 = _FirstColor;
            float4 _Multiply_230a389ca52d4d83b03743dbbf24c425_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_0216fb3bfe8b4db5b18fcba37762366f_Out_3.xxxx), _Property_1eebfd3faede4caa9491171cceb2c713_Out_0, _Multiply_230a389ca52d4d83b03743dbbf24c425_Out_2);
            float _Property_47d40eca9e364770b00760452a2c5782_Out_0 = Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
            float _Property_796ec4ca311b479fbac33d11855f7268_Out_0 = Vector1_015703dd126b46c1bbf10073d763c78e;
            float _ColorMask_9357c0eac8e34ad3b92ae44d8179d1f4_Out_3;
            Unity_ColorMask_float((_SampleTexture2D_c7f3d923e5714684b585ae452327aada_RGBA_0.xyz), IsGammaSpace() ? float3(0, 0, 1) : SRGBToLinear(float3(0, 0, 1)), _Property_47d40eca9e364770b00760452a2c5782_Out_0, _ColorMask_9357c0eac8e34ad3b92ae44d8179d1f4_Out_3, _Property_796ec4ca311b479fbac33d11855f7268_Out_0);
            float4 _Property_c4b990dff59e419ba5606a2332f47114_Out_0 = _SecondColor;
            float4 _Multiply_588211649a0c47b6ba7deba6c658392b_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_9357c0eac8e34ad3b92ae44d8179d1f4_Out_3.xxxx), _Property_c4b990dff59e419ba5606a2332f47114_Out_0, _Multiply_588211649a0c47b6ba7deba6c658392b_Out_2);
            float4 _Add_a565a405238940c78f538912332ad434_Out_2;
            Unity_Add_float4(_Multiply_230a389ca52d4d83b03743dbbf24c425_Out_2, _Multiply_588211649a0c47b6ba7deba6c658392b_Out_2, _Add_a565a405238940c78f538912332ad434_Out_2);
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.BaseColor = (_Add_a565a405238940c78f538912332ad434_Out_2.xyz);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormalsOnly"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_TEXCOORD1
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.tangentWS;
            output.interp2.xyzw =  input.texCoord0;
            output.interp3.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.tangentWS = input.interp1.xyzw;
            output.texCoord0 = input.interp2.xyzw;
            output.color = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
             float4 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        #define ALPHA_CLIP_THRESHOLD 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
             float4 interp1 : INTERP1;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthNormals"
            Tags
            {
                "LightMode" = "DepthNormalsOnly"
            }
        
        // Render State
        Cull Off
        ZTest LEqual
        ZWrite On
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma only_renderers gles gles3 glcore d3d11
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma instancing_options renderinglayer
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
        #define _SURFACE_TYPE_TRANSPARENT 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float4 VertexColor;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float4 interp2 : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.color = input.interp2.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MaskTexture_TexelSize;
        float4 _FirstColor;
        float Vector1_20ec970ab7554c17bd5ed89e9bda7ec1;
        float Vector1_015703dd126b46c1bbf10073d763c78e;
        float4 _SecondColor;
        float Boolean_9adaed0c6de84900aab724c15134aad9;
        float4 Texture2D_e1fa2c08b38046549cb9a8682d5c7f15_TexelSize;
        float Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
        float2 Vector2_4ca45a0a63824ce79e034b016ff60cee;
        float Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
        float4 _MainTex_TexelSize;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MaskTexture);
        SAMPLER(sampler_MaskTexture);
        TEXTURE2D(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        SAMPLER(samplerTexture2D_e1fa2c08b38046549cb9a8682d5c7f15);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_R_1 = IN.VertexColor[0];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_G_2 = IN.VertexColor[1];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_B_3 = IN.VertexColor[2];
            float _Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4 = IN.VertexColor[3];
            float _Split_d81f613dcfd5420ea467138172604e41_R_1 = IN.VertexColor[0];
            float _Split_d81f613dcfd5420ea467138172604e41_G_2 = IN.VertexColor[1];
            float _Split_d81f613dcfd5420ea467138172604e41_B_3 = IN.VertexColor[2];
            float _Split_d81f613dcfd5420ea467138172604e41_A_4 = IN.VertexColor[3];
            UnityTexture2D _Property_7941c080810e4a0db57adc50ed04da5c_Out_0 = UnityBuildTexture2DStructNoScale(_MaskTexture);
            float4 _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7941c080810e4a0db57adc50ed04da5c_Out_0.tex, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.samplerstate, _Property_7941c080810e4a0db57adc50ed04da5c_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_R_4 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.r;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_G_5 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.g;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_B_6 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.b;
            float _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7 = _SampleTexture2D_2921c099de0846849f78f614722fc56f_RGBA_0.a;
            float _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2;
            Unity_Multiply_float_float(_Split_d81f613dcfd5420ea467138172604e41_A_4, _SampleTexture2D_2921c099de0846849f78f614722fc56f_A_7, _Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2);
            float _Property_578a410c7d3245438c28dd16056f5a74_Out_0 = Boolean_9adaed0c6de84900aab724c15134aad9;
            UnityTexture2D _Property_7f494883abae4238a5f5dc35bcca091e_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_e1fa2c08b38046549cb9a8682d5c7f15);
            float4 _UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0 = IN.uv0;
            float2 _Property_dc8cd787037f452986237448f60aae4a_Out_0 = Vector2_4ca45a0a63824ce79e034b016ff60cee;
            float _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0 = Vector1_0f3cb6e470c245b1b027cee0824b2e1e;
            float _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2;
            Unity_Multiply_float_float(IN.TimeParameters.x, _Property_fe0e57a20e5549459fcc468bd384fdae_Out_0, _Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2);
            float2 _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3;
            Unity_TilingAndOffset_float((_UV_20917d0edb25401d8c42f6d01dcf8f49_Out_0.xy), _Property_dc8cd787037f452986237448f60aae4a_Out_0, (_Multiply_b2bef4e503cc46479287b941f9fab2e6_Out_2.xx), _TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3);
            float4 _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0 = SAMPLE_TEXTURE2D(_Property_7f494883abae4238a5f5dc35bcca091e_Out_0.tex, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.samplerstate, _Property_7f494883abae4238a5f5dc35bcca091e_Out_0.GetTransformedUV(_TilingAndOffset_3260cb99e1324b8f9263e89dce625bb6_Out_3));
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_R_4 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.r;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_G_5 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.g;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_B_6 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.b;
            float _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7 = _SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_RGBA_0.a;
            float _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0 = Vector1_10e12b7709ea4ef7980e6b2beffbd6fe;
            float _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2;
            Unity_Add_float(_SampleTexture2D_87209cfd331845689e4d53ce5f1a5336_A_7, _Property_fb1e2ed5625b4a7689ebd2b275b8ef1c_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2);
            float _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3;
            Unity_Branch_float(_Property_578a410c7d3245438c28dd16056f5a74_Out_0, _Add_f5bf41c184a7431f8a8bbdeae81894ac_Out_2, 1, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3);
            float _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2;
            Unity_Multiply_float_float(_Multiply_15aadf756a0d4e8fa84f463e37b3d373_Out_2, _Branch_2fa4c465a05349b2b144a262a1ce69ba_Out_3, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2);
            float _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            Unity_Multiply_float_float(_Split_a58e9cdd0b624c70ac7784413aeb50eb_A_4, _Multiply_894e22c1ea9f4864952996ea031fd0cb_Out_2, _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2);
            surface.Alpha = _Multiply_6793f5f6d68748908a7f7a5f7d55de57_Out_2;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
            // FragInputs from VFX come from two places: Interpolator or CBuffer.
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.VertexColor = input.color;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}