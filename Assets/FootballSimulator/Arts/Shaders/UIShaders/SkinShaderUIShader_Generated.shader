Shader "Converted/SkinShaderUIShader"
{
    Properties
    {
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        _SkinColor("SkinColor", Color) = (0, 0, 0, 0)
        Color_026b99ebbce54410be4181fcb809c401("MaskColor", Color) = (0.5754717, 0.5754717, 0.5754717, 0)
        Vector1_2e7c5e477ea440939df922612e0f2975("MaskRange", Float) = 0
        Vector1_065a56b3fa214828a10f39060c7dd219("MaskFuzziness", Float) = 1
        [NoScaleOffset]Texture2D_31397dbc8e1e4d79af76d0caf80bd60f("AdditionalColorMaskTexture", 2D) = "white" {}
        Color_e889be29e5c44a1c837dbba019b7de95("AdditionalColorMaskColor", Color) = (0, 1, 0.1451273, 1)
        Color_c585ed4a09b147d78a9ece6bfec5d9d1("AdditionalColor", Color) = (1, 0.8702337, 0.759434, 0)
        Vector1_c70ef2a839634731b17db8077bdaafaa("AdditionalColorRange", Float) = 0.1
        Vector1_5425f7f138b946a4aa273900af576cbf("AdditionalColorFuzziness", Float) = 1
        Vector1_7d87c1cfd8404d848478b68ea56092ff("BlendToAdditional", Float) = 1
        Vector1_a662623d6531424896586fc4fbe013f8("AdditionalTextureKeepsOriginal", Float) = 1
        [NoScaleOffset]Texture2D_6fa0461c58f544d084e28ca6adc7620b("FinalMaskTexture", 2D) = "white" {}
        Color_1ddcfbd9c83849eda1a48f00eb961192("FinalColorMask", Color) = (0, 0, 0, 0)
        _FinalMaskColor("FinalMaskColor", Color) = (0, 0, 0, 0)
        Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec("FinalMaskFuzziness", Float) = 0
        Vector1_10e6d5f136e94e468796501a8a8e780c("FinalMaskRange", Float) = 0
        Vector1_4ca23baddccc43b197801e5c1833619e("FinalMaskBlend", Float) = 1
        Vector1_ec2dd9b446cb4361ae423a8ff2571c95("Ageing", Range(0, 1)) = 1
        [NoScaleOffset]Texture2D_af137a3fb91848e7b36e6124083820e6("NoiseTexture", 2D) = "white" {}
        Vector1_a84c0be762ea439ca9a7d92ab697247a("NoiseSize", Float) = 0
        Vector2_afc7dad7cc254e12b82eaa3aab957337("NoiseOffset", Vector) = (0, 0, 0, 0)
        [NoScaleOffset]Texture2D_dd8808d63a26435c9287d39f5269af03("BackLightTexture", 2D) = "white" {}
        Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8("BackLightPower", Float) = 0
        Color_aa879283620e447ca38efda0099cf5fe("MultiplyColor", Color) = (1, 0, 0, 1)
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
        
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_ColorMask_float(float3 In, float3 MaskColor, float Range, out float Out, float Fuzziness)
        {
            float Distance = distance(MaskColor, In);
            Out = saturate(1 - (Distance - Range) / max(Fuzziness, 1e-5));
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Blend_Overlay_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
        {
            float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
            float4 result2 = 2.0 * Base * Blend;
            float4 zeroOrOne = step(Base, 0.5);
            Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
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
            float4 Color_53d032a3f1e543249326cca931a92532 = IsGammaSpace() ? float4(0.3773585, 0.3773585, 0.3773585, 0) : float4(SRGBToLinear(float3(0.3773585, 0.3773585, 0.3773585)), 0);
            float _Float_043d58b4b22f4ccc9a7e91f1d952f3f3_Out_0 = 15;
            float _GradientNoise_652092172498416f864fd6cb778d627f_Out_2;
            Unity_GradientNoise_float(IN.uv0.xy, _Float_043d58b4b22f4ccc9a7e91f1d952f3f3_Out_0, _GradientNoise_652092172498416f864fd6cb778d627f_Out_2);
            float _Float_4ac9ee155bdb4e40b2825549bebd6cd8_Out_0 = 0.1;
            float _Multiply_acf8b925a8fd4b27bdb74c47b06181a7_Out_2;
            Unity_Multiply_float_float(_GradientNoise_652092172498416f864fd6cb778d627f_Out_2, _Float_4ac9ee155bdb4e40b2825549bebd6cd8_Out_0, _Multiply_acf8b925a8fd4b27bdb74c47b06181a7_Out_2);
            float4 _Multiply_9679795391a64f15b71b9021246075a2_Out_2;
            Unity_Multiply_float4_float4(Color_53d032a3f1e543249326cca931a92532, (_Multiply_acf8b925a8fd4b27bdb74c47b06181a7_Out_2.xxxx), _Multiply_9679795391a64f15b71b9021246075a2_Out_2);
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float4 _Property_226be0815a374c75b11f0347c7b51ecd_Out_0 = Color_026b99ebbce54410be4181fcb809c401;
            float _Property_ab5b138477f54c98ac4e1955112a83cf_Out_0 = Vector1_2e7c5e477ea440939df922612e0f2975;
            float _Property_c80f7fed453c4c52b3f9a9978bc76d58_Out_0 = Vector1_065a56b3fa214828a10f39060c7dd219;
            float _ColorMask_d4d0285b043b4534b28a7c0ebb2b04a9_Out_3;
            Unity_ColorMask_float((_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.xyz), (_Property_226be0815a374c75b11f0347c7b51ecd_Out_0.xyz), _Property_ab5b138477f54c98ac4e1955112a83cf_Out_0, _ColorMask_d4d0285b043b4534b28a7c0ebb2b04a9_Out_3, _Property_c80f7fed453c4c52b3f9a9978bc76d58_Out_0);
            float4 _Property_c89768b6e0fb408a81b8788681a81d5e_Out_0 = _SkinColor;
            float4 _Multiply_629cdddc444e448f87b5221982a4c1af_Out_2;
            Unity_Multiply_float4_float4(_Property_c89768b6e0fb408a81b8788681a81d5e_Out_0, float4(1, 1, 1, 1), _Multiply_629cdddc444e448f87b5221982a4c1af_Out_2);
            float4 _Multiply_3c311cc6ab4941e989c6b55cb91de541_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_d4d0285b043b4534b28a7c0ebb2b04a9_Out_3.xxxx), _Multiply_629cdddc444e448f87b5221982a4c1af_Out_2, _Multiply_3c311cc6ab4941e989c6b55cb91de541_Out_2);
            float4 _Add_dae6192ecd694ce3947fe0f369af5735_Out_2;
            Unity_Add_float4(_Multiply_9679795391a64f15b71b9021246075a2_Out_2, _Multiply_3c311cc6ab4941e989c6b55cb91de541_Out_2, _Add_dae6192ecd694ce3947fe0f369af5735_Out_2);
            float _Property_2d6142e8c02342a5be7a6b0e49d7c233_Out_0 = Vector1_a662623d6531424896586fc4fbe013f8;
            UnityTexture2D _Property_3d932aa14b154d35b84f5703d4459cef_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
            float4 _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_3d932aa14b154d35b84f5703d4459cef_Out_0.tex, _Property_3d932aa14b154d35b84f5703d4459cef_Out_0.samplerstate, _Property_3d932aa14b154d35b84f5703d4459cef_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_R_4 = _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.r;
            float _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_G_5 = _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.g;
            float _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_B_6 = _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.b;
            float _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_A_7 = _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.a;
            float _Multiply_42228054e759431b8236b8124abae500_Out_2;
            Unity_Multiply_float_float(_Property_2d6142e8c02342a5be7a6b0e49d7c233_Out_0, _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_A_7, _Multiply_42228054e759431b8236b8124abae500_Out_2);
            float4 _Lerp_eced9f93f77249d0aecff9d10aef621a_Out_3;
            Unity_Lerp_float4(_Add_dae6192ecd694ce3947fe0f369af5735_Out_2, _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0, (_Multiply_42228054e759431b8236b8124abae500_Out_2.xxxx), _Lerp_eced9f93f77249d0aecff9d10aef621a_Out_3);
            float4 _Property_0d72b76cdd6a431eaf5735993b76d6b2_Out_0 = Color_e889be29e5c44a1c837dbba019b7de95;
            float _Property_7364598478394759a8e59ff68d32c926_Out_0 = Vector1_c70ef2a839634731b17db8077bdaafaa;
            float _Property_c00612e16b734b8aa24f17cb280381f2_Out_0 = Vector1_5425f7f138b946a4aa273900af576cbf;
            float _ColorMask_772328f80fbb4701838de5ac81d156fa_Out_3;
            Unity_ColorMask_float((_SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.xyz), (_Property_0d72b76cdd6a431eaf5735993b76d6b2_Out_0.xyz), _Property_7364598478394759a8e59ff68d32c926_Out_0, _ColorMask_772328f80fbb4701838de5ac81d156fa_Out_3, _Property_c00612e16b734b8aa24f17cb280381f2_Out_0);
            float4 _Property_218325e8e2684408a2ae86c73fb58630_Out_0 = Color_c585ed4a09b147d78a9ece6bfec5d9d1;
            float4 _Multiply_b7021add5e57456996787e19482da2b9_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_772328f80fbb4701838de5ac81d156fa_Out_3.xxxx), _Property_218325e8e2684408a2ae86c73fb58630_Out_0, _Multiply_b7021add5e57456996787e19482da2b9_Out_2);
            float _Property_5b236935bb2d4e6c8657b77c2981d922_Out_0 = Vector1_7d87c1cfd8404d848478b68ea56092ff;
            float _Multiply_a6975fc778524c8ca9aa8f4d9d39effd_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_A_7, _Property_5b236935bb2d4e6c8657b77c2981d922_Out_0, _Multiply_a6975fc778524c8ca9aa8f4d9d39effd_Out_2);
            float4 _Blend_35b088f11a144677a6eced017df56f7d_Out_2;
            Unity_Blend_Overlay_float4(_Lerp_eced9f93f77249d0aecff9d10aef621a_Out_3, _Multiply_b7021add5e57456996787e19482da2b9_Out_2, _Blend_35b088f11a144677a6eced017df56f7d_Out_2, _Multiply_a6975fc778524c8ca9aa8f4d9d39effd_Out_2);
            UnityTexture2D _Property_a466bc4d1a554219bfaaeecad85e9603_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
            float4 _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a466bc4d1a554219bfaaeecad85e9603_Out_0.tex, _Property_a466bc4d1a554219bfaaeecad85e9603_Out_0.samplerstate, _Property_a466bc4d1a554219bfaaeecad85e9603_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_R_4 = _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.r;
            float _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_G_5 = _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.g;
            float _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_B_6 = _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.b;
            float _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_A_7 = _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.a;
            float4 _Property_c5941bcf3d914e79b6bc6b74baeae8be_Out_0 = Color_1ddcfbd9c83849eda1a48f00eb961192;
            float _Property_aa04d04e87df42b1a5254284e1055231_Out_0 = Vector1_10e6d5f136e94e468796501a8a8e780c;
            float _Property_9e9f05512eb3440bb2a9f1855edf84f9_Out_0 = Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
            float _ColorMask_29392ec0a3eb4c0b99c805877747e3bf_Out_3;
            Unity_ColorMask_float((_SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.xyz), (_Property_c5941bcf3d914e79b6bc6b74baeae8be_Out_0.xyz), _Property_aa04d04e87df42b1a5254284e1055231_Out_0, _ColorMask_29392ec0a3eb4c0b99c805877747e3bf_Out_3, _Property_9e9f05512eb3440bb2a9f1855edf84f9_Out_0);
            float4 _Property_fd576c2708f04b9788e85d4ef81f2b10_Out_0 = _FinalMaskColor;
            float4 _Multiply_c87b43ceb7aa4144a8966d3c607a41b5_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_29392ec0a3eb4c0b99c805877747e3bf_Out_3.xxxx), _Property_fd576c2708f04b9788e85d4ef81f2b10_Out_0, _Multiply_c87b43ceb7aa4144a8966d3c607a41b5_Out_2);
            float _Property_b1f48e39a3ca41a089e3da2e1a4daec9_Out_0 = Vector1_4ca23baddccc43b197801e5c1833619e;
            float _Multiply_b8295db02136421b97489723e5dd2c49_Out_2;
            Unity_Multiply_float_float(_ColorMask_29392ec0a3eb4c0b99c805877747e3bf_Out_3, _Property_b1f48e39a3ca41a089e3da2e1a4daec9_Out_0, _Multiply_b8295db02136421b97489723e5dd2c49_Out_2);
            float4 _Blend_e3e1314c0b554a3083d2570c19a38d96_Out_2;
            Unity_Blend_Overlay_float4(_Blend_35b088f11a144677a6eced017df56f7d_Out_2, _Multiply_c87b43ceb7aa4144a8966d3c607a41b5_Out_2, _Blend_e3e1314c0b554a3083d2570c19a38d96_Out_2, _Multiply_b8295db02136421b97489723e5dd2c49_Out_2);
            UnityTexture2D _Property_184edcb90cae4bba9c4355a2ae615f42_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_af137a3fb91848e7b36e6124083820e6);
            float _Property_4034a29fe92b4ae6b9173b65e317fc75_Out_0 = Vector1_a84c0be762ea439ca9a7d92ab697247a;
            float2 _Property_a7aba2e9069e48ed8d17add70d5d017e_Out_0 = Vector2_afc7dad7cc254e12b82eaa3aab957337;
            float2 _TilingAndOffset_255b6da43c7546189e4a3209d8142268_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_4034a29fe92b4ae6b9173b65e317fc75_Out_0.xx), _Property_a7aba2e9069e48ed8d17add70d5d017e_Out_0, _TilingAndOffset_255b6da43c7546189e4a3209d8142268_Out_3);
            float4 _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0 = SAMPLE_TEXTURE2D(_Property_184edcb90cae4bba9c4355a2ae615f42_Out_0.tex, _Property_184edcb90cae4bba9c4355a2ae615f42_Out_0.samplerstate, _Property_184edcb90cae4bba9c4355a2ae615f42_Out_0.GetTransformedUV(_TilingAndOffset_255b6da43c7546189e4a3209d8142268_Out_3));
            float _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_R_4 = _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0.r;
            float _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_G_5 = _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0.g;
            float _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_B_6 = _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0.b;
            float _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_A_7 = _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0.a;
            float4 Color_d0e16079f9224b579a1edfcfba1efcf6 = IsGammaSpace() ? float4(0, 0, 0, 1) : float4(SRGBToLinear(float3(0, 0, 0)), 1);
            float4 _Multiply_e909bfd81d30469a9317f821586c11ba_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0, Color_d0e16079f9224b579a1edfcfba1efcf6, _Multiply_e909bfd81d30469a9317f821586c11ba_Out_2);
            float _Property_0f3e4a3a5c214965bf134fa3cc5978bb_Out_0 = Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
            float _Multiply_ab68a1f3bf5d4d5e8c887b9726f4491d_Out_2;
            Unity_Multiply_float_float(_Property_0f3e4a3a5c214965bf134fa3cc5978bb_Out_0, 0.2, _Multiply_ab68a1f3bf5d4d5e8c887b9726f4491d_Out_2);
            float _Multiply_a45608a8071a4701a9c5187b9851efa1_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_A_7, _Multiply_ab68a1f3bf5d4d5e8c887b9726f4491d_Out_2, _Multiply_a45608a8071a4701a9c5187b9851efa1_Out_2);
            float4 _Blend_3dd9d1ee7f824dfe9e0db84d3c9129ea_Out_2;
            Unity_Blend_Overlay_float4(_Blend_e3e1314c0b554a3083d2570c19a38d96_Out_2, _Multiply_e909bfd81d30469a9317f821586c11ba_Out_2, _Blend_3dd9d1ee7f824dfe9e0db84d3c9129ea_Out_2, _Multiply_a45608a8071a4701a9c5187b9851efa1_Out_2);
            float4 Color_68548613a985401e809e71396d5fb566 = IsGammaSpace() ? float4(1, 0.8963315, 0.8160377, 1) : float4(SRGBToLinear(float3(1, 0.8963315, 0.8160377)), 1);
            UnityTexture2D _Property_d6b5dc2b037c49de89b48bce9213fcbb_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_dd8808d63a26435c9287d39f5269af03);
            float4 _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d6b5dc2b037c49de89b48bce9213fcbb_Out_0.tex, _Property_d6b5dc2b037c49de89b48bce9213fcbb_Out_0.samplerstate, _Property_d6b5dc2b037c49de89b48bce9213fcbb_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_R_4 = _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0.r;
            float _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_G_5 = _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0.g;
            float _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_B_6 = _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0.b;
            float _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_A_7 = _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0.a;
            float _Property_8682c7da0e7f4bd48925947b78d3fe1d_Out_0 = Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
            float4 _Multiply_1eb74e1318344ab58c3c5a7e4dadcc21_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0, (_Property_8682c7da0e7f4bd48925947b78d3fe1d_Out_0.xxxx), _Multiply_1eb74e1318344ab58c3c5a7e4dadcc21_Out_2);
            float4 _Blend_b3d9c866e0ad40bcb8ce0c132feb4861_Out_2;
            Unity_Blend_Overlay_float4(_Blend_3dd9d1ee7f824dfe9e0db84d3c9129ea_Out_2, Color_68548613a985401e809e71396d5fb566, _Blend_b3d9c866e0ad40bcb8ce0c132feb4861_Out_2, (_Multiply_1eb74e1318344ab58c3c5a7e4dadcc21_Out_2).x);
            float4 _Property_8fbe5cf4ce78472787b923d15e1906f0_Out_0 = Color_aa879283620e447ca38efda0099cf5fe;
            float4 _Multiply_c493307440ab400f82760145286ce413_Out_2;
            Unity_Multiply_float4_float4(_Blend_b3d9c866e0ad40bcb8ce0c132feb4861_Out_2, _Property_8fbe5cf4ce78472787b923d15e1906f0_Out_0, _Multiply_c493307440ab400f82760145286ce413_Out_2);
            UnityTexture2D _Property_ae960e8b441549edb9ae418a18f36cda_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
            float4 _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ae960e8b441549edb9ae418a18f36cda_Out_0.tex, _Property_ae960e8b441549edb9ae418a18f36cda_Out_0.samplerstate, _Property_ae960e8b441549edb9ae418a18f36cda_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_R_4 = _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0.r;
            float _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_G_5 = _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0.g;
            float _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_B_6 = _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0.b;
            float _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_A_7 = _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0.a;
            float _Multiply_4c323886d0f94438ac86242f788191a9_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_B_6, _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_A_7, _Multiply_4c323886d0f94438ac86242f788191a9_Out_2);
            float4 _Lerp_377499de17fc42ac95f79512695062a3_Out_3;
            Unity_Lerp_float4(_Blend_b3d9c866e0ad40bcb8ce0c132feb4861_Out_2, _Multiply_c493307440ab400f82760145286ce413_Out_2, (_Multiply_4c323886d0f94438ac86242f788191a9_Out_2.xxxx), _Lerp_377499de17fc42ac95f79512695062a3_Out_3);
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.BaseColor = (_Lerp_377499de17fc42ac95f79512695062a3_Out_3.xyz);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
        
        
        float2 Unity_GradientNoise_Dir_float(float2 p)
        {
            // Permutation and hashing used in webgl-nosie goo.gl/pX7HtC
            p = p % 289;
            // need full precision, otherwise half overflows when p > 1
            float x = float(34 * p.x + 1) * p.x % 289 + p.y;
            x = (34 * x + 1) * x % 289;
            x = frac(x / 41) * 2 - 1;
            return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
        }
        
        void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
        {
            float2 p = UV * Scale;
            float2 ip = floor(p);
            float2 fp = frac(p);
            float d00 = dot(Unity_GradientNoise_Dir_float(ip), fp);
            float d01 = dot(Unity_GradientNoise_Dir_float(ip + float2(0, 1)), fp - float2(0, 1));
            float d10 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 0)), fp - float2(1, 0));
            float d11 = dot(Unity_GradientNoise_Dir_float(ip + float2(1, 1)), fp - float2(1, 1));
            fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
            Out = lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x) + 0.5;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_ColorMask_float(float3 In, float3 MaskColor, float Range, out float Out, float Fuzziness)
        {
            float Distance = distance(MaskColor, In);
            Out = saturate(1 - (Distance - Range) / max(Fuzziness, 1e-5));
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Blend_Overlay_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
        {
            float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
            float4 result2 = 2.0 * Base * Blend;
            float4 zeroOrOne = step(Base, 0.5);
            Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
            Out = lerp(Base, Out, Opacity);
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
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
            float4 Color_53d032a3f1e543249326cca931a92532 = IsGammaSpace() ? float4(0.3773585, 0.3773585, 0.3773585, 0) : float4(SRGBToLinear(float3(0.3773585, 0.3773585, 0.3773585)), 0);
            float _Float_043d58b4b22f4ccc9a7e91f1d952f3f3_Out_0 = 15;
            float _GradientNoise_652092172498416f864fd6cb778d627f_Out_2;
            Unity_GradientNoise_float(IN.uv0.xy, _Float_043d58b4b22f4ccc9a7e91f1d952f3f3_Out_0, _GradientNoise_652092172498416f864fd6cb778d627f_Out_2);
            float _Float_4ac9ee155bdb4e40b2825549bebd6cd8_Out_0 = 0.1;
            float _Multiply_acf8b925a8fd4b27bdb74c47b06181a7_Out_2;
            Unity_Multiply_float_float(_GradientNoise_652092172498416f864fd6cb778d627f_Out_2, _Float_4ac9ee155bdb4e40b2825549bebd6cd8_Out_0, _Multiply_acf8b925a8fd4b27bdb74c47b06181a7_Out_2);
            float4 _Multiply_9679795391a64f15b71b9021246075a2_Out_2;
            Unity_Multiply_float4_float4(Color_53d032a3f1e543249326cca931a92532, (_Multiply_acf8b925a8fd4b27bdb74c47b06181a7_Out_2.xxxx), _Multiply_9679795391a64f15b71b9021246075a2_Out_2);
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float4 _Property_226be0815a374c75b11f0347c7b51ecd_Out_0 = Color_026b99ebbce54410be4181fcb809c401;
            float _Property_ab5b138477f54c98ac4e1955112a83cf_Out_0 = Vector1_2e7c5e477ea440939df922612e0f2975;
            float _Property_c80f7fed453c4c52b3f9a9978bc76d58_Out_0 = Vector1_065a56b3fa214828a10f39060c7dd219;
            float _ColorMask_d4d0285b043b4534b28a7c0ebb2b04a9_Out_3;
            Unity_ColorMask_float((_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.xyz), (_Property_226be0815a374c75b11f0347c7b51ecd_Out_0.xyz), _Property_ab5b138477f54c98ac4e1955112a83cf_Out_0, _ColorMask_d4d0285b043b4534b28a7c0ebb2b04a9_Out_3, _Property_c80f7fed453c4c52b3f9a9978bc76d58_Out_0);
            float4 _Property_c89768b6e0fb408a81b8788681a81d5e_Out_0 = _SkinColor;
            float4 _Multiply_629cdddc444e448f87b5221982a4c1af_Out_2;
            Unity_Multiply_float4_float4(_Property_c89768b6e0fb408a81b8788681a81d5e_Out_0, float4(1, 1, 1, 1), _Multiply_629cdddc444e448f87b5221982a4c1af_Out_2);
            float4 _Multiply_3c311cc6ab4941e989c6b55cb91de541_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_d4d0285b043b4534b28a7c0ebb2b04a9_Out_3.xxxx), _Multiply_629cdddc444e448f87b5221982a4c1af_Out_2, _Multiply_3c311cc6ab4941e989c6b55cb91de541_Out_2);
            float4 _Add_dae6192ecd694ce3947fe0f369af5735_Out_2;
            Unity_Add_float4(_Multiply_9679795391a64f15b71b9021246075a2_Out_2, _Multiply_3c311cc6ab4941e989c6b55cb91de541_Out_2, _Add_dae6192ecd694ce3947fe0f369af5735_Out_2);
            float _Property_2d6142e8c02342a5be7a6b0e49d7c233_Out_0 = Vector1_a662623d6531424896586fc4fbe013f8;
            UnityTexture2D _Property_3d932aa14b154d35b84f5703d4459cef_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
            float4 _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0 = SAMPLE_TEXTURE2D(_Property_3d932aa14b154d35b84f5703d4459cef_Out_0.tex, _Property_3d932aa14b154d35b84f5703d4459cef_Out_0.samplerstate, _Property_3d932aa14b154d35b84f5703d4459cef_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_R_4 = _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.r;
            float _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_G_5 = _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.g;
            float _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_B_6 = _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.b;
            float _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_A_7 = _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.a;
            float _Multiply_42228054e759431b8236b8124abae500_Out_2;
            Unity_Multiply_float_float(_Property_2d6142e8c02342a5be7a6b0e49d7c233_Out_0, _SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_A_7, _Multiply_42228054e759431b8236b8124abae500_Out_2);
            float4 _Lerp_eced9f93f77249d0aecff9d10aef621a_Out_3;
            Unity_Lerp_float4(_Add_dae6192ecd694ce3947fe0f369af5735_Out_2, _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0, (_Multiply_42228054e759431b8236b8124abae500_Out_2.xxxx), _Lerp_eced9f93f77249d0aecff9d10aef621a_Out_3);
            float4 _Property_0d72b76cdd6a431eaf5735993b76d6b2_Out_0 = Color_e889be29e5c44a1c837dbba019b7de95;
            float _Property_7364598478394759a8e59ff68d32c926_Out_0 = Vector1_c70ef2a839634731b17db8077bdaafaa;
            float _Property_c00612e16b734b8aa24f17cb280381f2_Out_0 = Vector1_5425f7f138b946a4aa273900af576cbf;
            float _ColorMask_772328f80fbb4701838de5ac81d156fa_Out_3;
            Unity_ColorMask_float((_SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_RGBA_0.xyz), (_Property_0d72b76cdd6a431eaf5735993b76d6b2_Out_0.xyz), _Property_7364598478394759a8e59ff68d32c926_Out_0, _ColorMask_772328f80fbb4701838de5ac81d156fa_Out_3, _Property_c00612e16b734b8aa24f17cb280381f2_Out_0);
            float4 _Property_218325e8e2684408a2ae86c73fb58630_Out_0 = Color_c585ed4a09b147d78a9ece6bfec5d9d1;
            float4 _Multiply_b7021add5e57456996787e19482da2b9_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_772328f80fbb4701838de5ac81d156fa_Out_3.xxxx), _Property_218325e8e2684408a2ae86c73fb58630_Out_0, _Multiply_b7021add5e57456996787e19482da2b9_Out_2);
            float _Property_5b236935bb2d4e6c8657b77c2981d922_Out_0 = Vector1_7d87c1cfd8404d848478b68ea56092ff;
            float _Multiply_a6975fc778524c8ca9aa8f4d9d39effd_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_84524dd4f01347eb8a4541ab3f60e08a_A_7, _Property_5b236935bb2d4e6c8657b77c2981d922_Out_0, _Multiply_a6975fc778524c8ca9aa8f4d9d39effd_Out_2);
            float4 _Blend_35b088f11a144677a6eced017df56f7d_Out_2;
            Unity_Blend_Overlay_float4(_Lerp_eced9f93f77249d0aecff9d10aef621a_Out_3, _Multiply_b7021add5e57456996787e19482da2b9_Out_2, _Blend_35b088f11a144677a6eced017df56f7d_Out_2, _Multiply_a6975fc778524c8ca9aa8f4d9d39effd_Out_2);
            UnityTexture2D _Property_a466bc4d1a554219bfaaeecad85e9603_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
            float4 _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a466bc4d1a554219bfaaeecad85e9603_Out_0.tex, _Property_a466bc4d1a554219bfaaeecad85e9603_Out_0.samplerstate, _Property_a466bc4d1a554219bfaaeecad85e9603_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_R_4 = _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.r;
            float _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_G_5 = _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.g;
            float _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_B_6 = _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.b;
            float _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_A_7 = _SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.a;
            float4 _Property_c5941bcf3d914e79b6bc6b74baeae8be_Out_0 = Color_1ddcfbd9c83849eda1a48f00eb961192;
            float _Property_aa04d04e87df42b1a5254284e1055231_Out_0 = Vector1_10e6d5f136e94e468796501a8a8e780c;
            float _Property_9e9f05512eb3440bb2a9f1855edf84f9_Out_0 = Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
            float _ColorMask_29392ec0a3eb4c0b99c805877747e3bf_Out_3;
            Unity_ColorMask_float((_SampleTexture2D_fef9fa9fff1b4720a1be2becf0d59d1f_RGBA_0.xyz), (_Property_c5941bcf3d914e79b6bc6b74baeae8be_Out_0.xyz), _Property_aa04d04e87df42b1a5254284e1055231_Out_0, _ColorMask_29392ec0a3eb4c0b99c805877747e3bf_Out_3, _Property_9e9f05512eb3440bb2a9f1855edf84f9_Out_0);
            float4 _Property_fd576c2708f04b9788e85d4ef81f2b10_Out_0 = _FinalMaskColor;
            float4 _Multiply_c87b43ceb7aa4144a8966d3c607a41b5_Out_2;
            Unity_Multiply_float4_float4((_ColorMask_29392ec0a3eb4c0b99c805877747e3bf_Out_3.xxxx), _Property_fd576c2708f04b9788e85d4ef81f2b10_Out_0, _Multiply_c87b43ceb7aa4144a8966d3c607a41b5_Out_2);
            float _Property_b1f48e39a3ca41a089e3da2e1a4daec9_Out_0 = Vector1_4ca23baddccc43b197801e5c1833619e;
            float _Multiply_b8295db02136421b97489723e5dd2c49_Out_2;
            Unity_Multiply_float_float(_ColorMask_29392ec0a3eb4c0b99c805877747e3bf_Out_3, _Property_b1f48e39a3ca41a089e3da2e1a4daec9_Out_0, _Multiply_b8295db02136421b97489723e5dd2c49_Out_2);
            float4 _Blend_e3e1314c0b554a3083d2570c19a38d96_Out_2;
            Unity_Blend_Overlay_float4(_Blend_35b088f11a144677a6eced017df56f7d_Out_2, _Multiply_c87b43ceb7aa4144a8966d3c607a41b5_Out_2, _Blend_e3e1314c0b554a3083d2570c19a38d96_Out_2, _Multiply_b8295db02136421b97489723e5dd2c49_Out_2);
            UnityTexture2D _Property_184edcb90cae4bba9c4355a2ae615f42_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_af137a3fb91848e7b36e6124083820e6);
            float _Property_4034a29fe92b4ae6b9173b65e317fc75_Out_0 = Vector1_a84c0be762ea439ca9a7d92ab697247a;
            float2 _Property_a7aba2e9069e48ed8d17add70d5d017e_Out_0 = Vector2_afc7dad7cc254e12b82eaa3aab957337;
            float2 _TilingAndOffset_255b6da43c7546189e4a3209d8142268_Out_3;
            Unity_TilingAndOffset_float(IN.uv0.xy, (_Property_4034a29fe92b4ae6b9173b65e317fc75_Out_0.xx), _Property_a7aba2e9069e48ed8d17add70d5d017e_Out_0, _TilingAndOffset_255b6da43c7546189e4a3209d8142268_Out_3);
            float4 _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0 = SAMPLE_TEXTURE2D(_Property_184edcb90cae4bba9c4355a2ae615f42_Out_0.tex, _Property_184edcb90cae4bba9c4355a2ae615f42_Out_0.samplerstate, _Property_184edcb90cae4bba9c4355a2ae615f42_Out_0.GetTransformedUV(_TilingAndOffset_255b6da43c7546189e4a3209d8142268_Out_3));
            float _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_R_4 = _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0.r;
            float _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_G_5 = _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0.g;
            float _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_B_6 = _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0.b;
            float _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_A_7 = _SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0.a;
            float4 Color_d0e16079f9224b579a1edfcfba1efcf6 = IsGammaSpace() ? float4(0, 0, 0, 1) : float4(SRGBToLinear(float3(0, 0, 0)), 1);
            float4 _Multiply_e909bfd81d30469a9317f821586c11ba_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_RGBA_0, Color_d0e16079f9224b579a1edfcfba1efcf6, _Multiply_e909bfd81d30469a9317f821586c11ba_Out_2);
            float _Property_0f3e4a3a5c214965bf134fa3cc5978bb_Out_0 = Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
            float _Multiply_ab68a1f3bf5d4d5e8c887b9726f4491d_Out_2;
            Unity_Multiply_float_float(_Property_0f3e4a3a5c214965bf134fa3cc5978bb_Out_0, 0.2, _Multiply_ab68a1f3bf5d4d5e8c887b9726f4491d_Out_2);
            float _Multiply_a45608a8071a4701a9c5187b9851efa1_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_5ea45be25c0b4181b3f8bd3efddc9db3_A_7, _Multiply_ab68a1f3bf5d4d5e8c887b9726f4491d_Out_2, _Multiply_a45608a8071a4701a9c5187b9851efa1_Out_2);
            float4 _Blend_3dd9d1ee7f824dfe9e0db84d3c9129ea_Out_2;
            Unity_Blend_Overlay_float4(_Blend_e3e1314c0b554a3083d2570c19a38d96_Out_2, _Multiply_e909bfd81d30469a9317f821586c11ba_Out_2, _Blend_3dd9d1ee7f824dfe9e0db84d3c9129ea_Out_2, _Multiply_a45608a8071a4701a9c5187b9851efa1_Out_2);
            float4 Color_68548613a985401e809e71396d5fb566 = IsGammaSpace() ? float4(1, 0.8963315, 0.8160377, 1) : float4(SRGBToLinear(float3(1, 0.8963315, 0.8160377)), 1);
            UnityTexture2D _Property_d6b5dc2b037c49de89b48bce9213fcbb_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_dd8808d63a26435c9287d39f5269af03);
            float4 _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d6b5dc2b037c49de89b48bce9213fcbb_Out_0.tex, _Property_d6b5dc2b037c49de89b48bce9213fcbb_Out_0.samplerstate, _Property_d6b5dc2b037c49de89b48bce9213fcbb_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_R_4 = _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0.r;
            float _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_G_5 = _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0.g;
            float _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_B_6 = _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0.b;
            float _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_A_7 = _SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0.a;
            float _Property_8682c7da0e7f4bd48925947b78d3fe1d_Out_0 = Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
            float4 _Multiply_1eb74e1318344ab58c3c5a7e4dadcc21_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_034aa97c8bd6498a8a89a2a12e3c2094_RGBA_0, (_Property_8682c7da0e7f4bd48925947b78d3fe1d_Out_0.xxxx), _Multiply_1eb74e1318344ab58c3c5a7e4dadcc21_Out_2);
            float4 _Blend_b3d9c866e0ad40bcb8ce0c132feb4861_Out_2;
            Unity_Blend_Overlay_float4(_Blend_3dd9d1ee7f824dfe9e0db84d3c9129ea_Out_2, Color_68548613a985401e809e71396d5fb566, _Blend_b3d9c866e0ad40bcb8ce0c132feb4861_Out_2, (_Multiply_1eb74e1318344ab58c3c5a7e4dadcc21_Out_2).x);
            float4 _Property_8fbe5cf4ce78472787b923d15e1906f0_Out_0 = Color_aa879283620e447ca38efda0099cf5fe;
            float4 _Multiply_c493307440ab400f82760145286ce413_Out_2;
            Unity_Multiply_float4_float4(_Blend_b3d9c866e0ad40bcb8ce0c132feb4861_Out_2, _Property_8fbe5cf4ce78472787b923d15e1906f0_Out_0, _Multiply_c493307440ab400f82760145286ce413_Out_2);
            UnityTexture2D _Property_ae960e8b441549edb9ae418a18f36cda_Out_0 = UnityBuildTexture2DStructNoScale(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
            float4 _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ae960e8b441549edb9ae418a18f36cda_Out_0.tex, _Property_ae960e8b441549edb9ae418a18f36cda_Out_0.samplerstate, _Property_ae960e8b441549edb9ae418a18f36cda_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_R_4 = _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0.r;
            float _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_G_5 = _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0.g;
            float _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_B_6 = _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0.b;
            float _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_A_7 = _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_RGBA_0.a;
            float _Multiply_4c323886d0f94438ac86242f788191a9_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_B_6, _SampleTexture2D_6139831fbd4a41b6b4ae4443d17a2361_A_7, _Multiply_4c323886d0f94438ac86242f788191a9_Out_2);
            float4 _Lerp_377499de17fc42ac95f79512695062a3_Out_3;
            Unity_Lerp_float4(_Blend_b3d9c866e0ad40bcb8ce0c132feb4861_Out_2, _Multiply_c493307440ab400f82760145286ce413_Out_2, (_Multiply_4c323886d0f94438ac86242f788191a9_Out_2.xxxx), _Lerp_377499de17fc42ac95f79512695062a3_Out_3);
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.BaseColor = (_Lerp_377499de17fc42ac95f79512695062a3_Out_3.xyz);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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
        float4 _MainTex_TexelSize;
        float4 _SkinColor;
        float4 Color_026b99ebbce54410be4181fcb809c401;
        float Vector1_2e7c5e477ea440939df922612e0f2975;
        float Vector1_065a56b3fa214828a10f39060c7dd219;
        float4 Texture2D_31397dbc8e1e4d79af76d0caf80bd60f_TexelSize;
        float4 Color_e889be29e5c44a1c837dbba019b7de95;
        float4 Color_c585ed4a09b147d78a9ece6bfec5d9d1;
        float Vector1_c70ef2a839634731b17db8077bdaafaa;
        float Vector1_5425f7f138b946a4aa273900af576cbf;
        float Vector1_7d87c1cfd8404d848478b68ea56092ff;
        float Vector1_a662623d6531424896586fc4fbe013f8;
        float4 Texture2D_6fa0461c58f544d084e28ca6adc7620b_TexelSize;
        float4 Color_1ddcfbd9c83849eda1a48f00eb961192;
        float4 _FinalMaskColor;
        float Vector1_a3abf1e5cc5e496aa0f5054c3cc855ec;
        float Vector1_10e6d5f136e94e468796501a8a8e780c;
        float Vector1_4ca23baddccc43b197801e5c1833619e;
        float Vector1_ec2dd9b446cb4361ae423a8ff2571c95;
        float4 Texture2D_af137a3fb91848e7b36e6124083820e6_TexelSize;
        float Vector1_a84c0be762ea439ca9a7d92ab697247a;
        float2 Vector2_afc7dad7cc254e12b82eaa3aab957337;
        float4 Texture2D_dd8808d63a26435c9287d39f5269af03_TexelSize;
        float Vector1_22cf4e38e53247e09f4b85d9d4c9d0e8;
        float4 Color_aa879283620e447ca38efda0099cf5fe;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(Texture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        SAMPLER(samplerTexture2D_31397dbc8e1e4d79af76d0caf80bd60f);
        TEXTURE2D(Texture2D_6fa0461c58f544d084e28ca6adc7620b);
        SAMPLER(samplerTexture2D_6fa0461c58f544d084e28ca6adc7620b);
        TEXTURE2D(Texture2D_af137a3fb91848e7b36e6124083820e6);
        SAMPLER(samplerTexture2D_af137a3fb91848e7b36e6124083820e6);
        TEXTURE2D(Texture2D_dd8808d63a26435c9287d39f5269af03);
        SAMPLER(samplerTexture2D_dd8808d63a26435c9287d39f5269af03);
        
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
            UnityTexture2D _Property_5098d2de5720488a9e22db968a380bd1_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5098d2de5720488a9e22db968a380bd1_Out_0.tex, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.samplerstate, _Property_5098d2de5720488a9e22db968a380bd1_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_R_4 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.r;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_G_5 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.g;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_B_6 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.b;
            float _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7 = _SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_RGBA_0.a;
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_R_1 = IN.VertexColor[0];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_G_2 = IN.VertexColor[1];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_B_3 = IN.VertexColor[2];
            float _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4 = IN.VertexColor[3];
            float _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
            Unity_Multiply_float_float(_SampleTexture2D_6fff61d9669346eeafc10a1d0e25e15d_A_7, _Split_b37dc136d2794b1ebb4462f48ebfae2e_A_4, _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2);
            surface.Alpha = _Multiply_192cbcfbb0e14ae28b25fc6e6ae3aee2_Out_2;
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