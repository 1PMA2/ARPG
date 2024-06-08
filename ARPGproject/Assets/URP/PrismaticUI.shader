Shader "Unlit/Prismatic"
{
    Properties
    {
        _Strength("Strength", Float) = 1
        _Frequency("Frequency", Float) = 1
        _Rotate("Rotate", Float) = 0.5
        _CycleTime("CycleTime", Float) = 3
        _wavespeed("wavespeed", Float) = 1
        [NoScaleOffset]_Tex("Tex", 2D) = "white" {}
        _alpha("alpha", Float) = 0.5
        _power("power", Float) = 0.8
        _UnscaledTime("UnscaledTime", Float) = 0
        [HideInInspector]_QueueOffset("_QueueOffset", Float) = 0
        [HideInInspector]_QueueControl("_QueueControl", Float) = -1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
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
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
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
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
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
             float3 interp3 : INTERP3;
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
            output.interp3.xyz =  input.viewDirectionWS;
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
            output.viewDirectionWS = input.interp3.xyz;
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
        float _Strength;
        float _Frequency;
        float _Rotate;
        float _CycleTime;
        float _wavespeed;
        float4 _Tex_TexelSize;
        float _alpha;
        float _power;
        float _UnscaledTime;
        CBUFFER_END
        
        // Object and Global properties
        TEXTURE2D(_Tex);
        SAMPLER(sampler_Tex);
        
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
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
        {
            // RGB to HSV
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float E = 1e-10;
            float V = (D == 0) ? Q.x : (Q.x + E);
            float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
        
            float hue = hsv.x + Offset;
            hsv.x = (hue < 0)
                    ? hue + 1
                    : (hue > 1)
                        ? hue - 1
                        : hue;
        
            // HSV to RGB
            float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
            Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Modulo_float(float A, float B, out float Out)
        {
            Out = fmod(A, B);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void TriangleWave_float(float In, out float Out)
        {
            Out = 2.0 * abs( 2 * (In - floor(0.5 + In)) ) - 1.0;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
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
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 Color_1a92fef6b8fa42bd97d2bcc6da64d839 = IsGammaSpace() ? LinearToSRGB(float4(1, 0, 0, 0)) : float4(1, 0, 0, 0);
            float4 _UV_e328c965a8114d75a81a25bc753e2055_Out_0 = IN.uv0;
            float _Split_a10bc08a4d924af8a37eb130f5c032d7_R_1 = _UV_e328c965a8114d75a81a25bc753e2055_Out_0[0];
            float _Split_a10bc08a4d924af8a37eb130f5c032d7_G_2 = _UV_e328c965a8114d75a81a25bc753e2055_Out_0[1];
            float _Split_a10bc08a4d924af8a37eb130f5c032d7_B_3 = _UV_e328c965a8114d75a81a25bc753e2055_Out_0[2];
            float _Split_a10bc08a4d924af8a37eb130f5c032d7_A_4 = _UV_e328c965a8114d75a81a25bc753e2055_Out_0[3];
            float _Property_a58b2c51348e419f9059421f06756713_Out_0 = _Frequency;
            float _Multiply_e04816ac47064c8da6541039582a6bac_Out_2;
            Unity_Multiply_float_float(_Split_a10bc08a4d924af8a37eb130f5c032d7_R_1, _Property_a58b2c51348e419f9059421f06756713_Out_0, _Multiply_e04816ac47064c8da6541039582a6bac_Out_2);
            float _Sine_00ef0460b3d14e439203d5216cfaed78_Out_1;
            Unity_Sine_float(_Multiply_e04816ac47064c8da6541039582a6bac_Out_2, _Sine_00ef0460b3d14e439203d5216cfaed78_Out_1);
            float3 _Hue_49577b74efd34cbabe9a8c6f95054403_Out_2;
            Unity_Hue_Normalized_float((Color_1a92fef6b8fa42bd97d2bcc6da64d839.xyz), _Sine_00ef0460b3d14e439203d5216cfaed78_Out_1, _Hue_49577b74efd34cbabe9a8c6f95054403_Out_2);
            float _Property_f0e2372cb920480dba7c25b1140f26e2_Out_0 = _Rotate;
            float2 _Rotate_b1748c2d60e84974b75df5bb194a05f8_Out_3;
            Unity_Rotate_Radians_float(IN.uv0.xy, float2 (0.5, 0.5), _Property_f0e2372cb920480dba7c25b1140f26e2_Out_0, _Rotate_b1748c2d60e84974b75df5bb194a05f8_Out_3);
            float _Split_c7f86b574fd6493e8373025425d44e6f_R_1 = _Rotate_b1748c2d60e84974b75df5bb194a05f8_Out_3[0];
            float _Split_c7f86b574fd6493e8373025425d44e6f_G_2 = _Rotate_b1748c2d60e84974b75df5bb194a05f8_Out_3[1];
            float _Split_c7f86b574fd6493e8373025425d44e6f_B_3 = 0;
            float _Split_c7f86b574fd6493e8373025425d44e6f_A_4 = 0;
            float _Subtract_7256d70390694f588bba7d074925548f_Out_2;
            Unity_Subtract_float(_Split_c7f86b574fd6493e8373025425d44e6f_R_1, 1.2, _Subtract_7256d70390694f588bba7d074925548f_Out_2);
            float _Property_5d6d818318f14114ba605df21009fed6_Out_0 = _UnscaledTime;
            float _Property_dd94e6ecea024170b84e73a59e58a185_Out_0 = _CycleTime;
            float _Modulo_3936eba955a7482282c7351804399c5e_Out_2;
            Unity_Modulo_float(_Property_5d6d818318f14114ba605df21009fed6_Out_0, _Property_dd94e6ecea024170b84e73a59e58a185_Out_0, _Modulo_3936eba955a7482282c7351804399c5e_Out_2);
            float _Property_71f8b8f90cec4ba1aaf659e06fe8f235_Out_0 = _wavespeed;
            float _Multiply_f041f83d64b1475694ba2efc8f4e7a57_Out_2;
            Unity_Multiply_float_float(_Modulo_3936eba955a7482282c7351804399c5e_Out_2, _Property_71f8b8f90cec4ba1aaf659e06fe8f235_Out_0, _Multiply_f041f83d64b1475694ba2efc8f4e7a57_Out_2);
            float _Add_d487fcc600004b16a3a23f4db5a8f462_Out_2;
            Unity_Add_float(_Subtract_7256d70390694f588bba7d074925548f_Out_2, _Multiply_f041f83d64b1475694ba2efc8f4e7a57_Out_2, _Add_d487fcc600004b16a3a23f4db5a8f462_Out_2);
            float _Property_7b18469ca7ff4776ab6bf177418d7dce_Out_0 = _power;
            float _Multiply_c4bd5033f4f1479385a7fd38748de3c8_Out_2;
            Unity_Multiply_float_float(_Add_d487fcc600004b16a3a23f4db5a8f462_Out_2, _Property_7b18469ca7ff4776ab6bf177418d7dce_Out_0, _Multiply_c4bd5033f4f1479385a7fd38748de3c8_Out_2);
            float _Saturate_c64eb0216f1f459d9d0bcf84b8d2dbf2_Out_1;
            Unity_Saturate_float(_Multiply_c4bd5033f4f1479385a7fd38748de3c8_Out_2, _Saturate_c64eb0216f1f459d9d0bcf84b8d2dbf2_Out_1);
            float _TriangleWave_6ecf9adc48554e6d9ed2c378d0ab6711_Out_1;
            TriangleWave_float(_Saturate_c64eb0216f1f459d9d0bcf84b8d2dbf2_Out_1, _TriangleWave_6ecf9adc48554e6d9ed2c378d0ab6711_Out_1);
            float3 _Multiply_04cb7c12646f43e88ad98dcaaffdabdf_Out_2;
            Unity_Multiply_float3_float3(_Hue_49577b74efd34cbabe9a8c6f95054403_Out_2, (_TriangleWave_6ecf9adc48554e6d9ed2c378d0ab6711_Out_1.xxx), _Multiply_04cb7c12646f43e88ad98dcaaffdabdf_Out_2);
            float _Property_002aeebed8ec4677b27b30c8e69293fd_Out_0 = _Strength;
            float3 _Multiply_19c286fcf62545748c187ffc5d548630_Out_2;
            Unity_Multiply_float3_float3(_Multiply_04cb7c12646f43e88ad98dcaaffdabdf_Out_2, (_Property_002aeebed8ec4677b27b30c8e69293fd_Out_0.xxx), _Multiply_19c286fcf62545748c187ffc5d548630_Out_2);
            float _Property_fd70ac17c42d41869a03c1b612f001cd_Out_0 = _alpha;
            surface.BaseColor = _Multiply_19c286fcf62545748c187ffc5d548630_Out_2;
            surface.Alpha = _Property_fd70ac17c42d41869a03c1b612f001cd_Out_0;
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
        Cull Back
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
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TEXCOORD0
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
             float3 interp3 : INTERP3;
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
            output.interp3.xyz =  input.viewDirectionWS;
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
            output.viewDirectionWS = input.interp3.xyz;
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
        float _Strength;
        float _Frequency;
        float _Rotate;
        float _CycleTime;
        float _wavespeed;
        float4 _Tex_TexelSize;
        float _alpha;
        float _power;
        float _UnscaledTime;
        CBUFFER_END
        
        // Object and Global properties
        TEXTURE2D(_Tex);
        SAMPLER(sampler_Tex);
        
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
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
        {
            // RGB to HSV
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float E = 1e-10;
            float V = (D == 0) ? Q.x : (Q.x + E);
            float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
        
            float hue = hsv.x + Offset;
            hsv.x = (hue < 0)
                    ? hue + 1
                    : (hue > 1)
                        ? hue - 1
                        : hue;
        
            // HSV to RGB
            float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
            Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
        }
        
        void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
        {
            //rotation matrix
            UV -= Center;
            float s = sin(Rotation);
            float c = cos(Rotation);
        
            //center rotation matrix
            float2x2 rMatrix = float2x2(c, -s, s, c);
            rMatrix *= 0.5;
            rMatrix += 0.5;
            rMatrix = rMatrix*2 - 1;
        
            //multiply the UVs by the rotation matrix
            UV.xy = mul(UV.xy, rMatrix);
            UV += Center;
        
            Out = UV;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Modulo_float(float A, float B, out float Out)
        {
            Out = fmod(A, B);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        void TriangleWave_float(float In, out float Out)
        {
            Out = 2.0 * abs( 2 * (In - floor(0.5 + In)) ) - 1.0;
        }
        
        void Unity_Multiply_float3_float3(float3 A, float3 B, out float3 Out)
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
            float3 BaseColor;
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 Color_1a92fef6b8fa42bd97d2bcc6da64d839 = IsGammaSpace() ? LinearToSRGB(float4(1, 0, 0, 0)) : float4(1, 0, 0, 0);
            float4 _UV_e328c965a8114d75a81a25bc753e2055_Out_0 = IN.uv0;
            float _Split_a10bc08a4d924af8a37eb130f5c032d7_R_1 = _UV_e328c965a8114d75a81a25bc753e2055_Out_0[0];
            float _Split_a10bc08a4d924af8a37eb130f5c032d7_G_2 = _UV_e328c965a8114d75a81a25bc753e2055_Out_0[1];
            float _Split_a10bc08a4d924af8a37eb130f5c032d7_B_3 = _UV_e328c965a8114d75a81a25bc753e2055_Out_0[2];
            float _Split_a10bc08a4d924af8a37eb130f5c032d7_A_4 = _UV_e328c965a8114d75a81a25bc753e2055_Out_0[3];
            float _Property_a58b2c51348e419f9059421f06756713_Out_0 = _Frequency;
            float _Multiply_e04816ac47064c8da6541039582a6bac_Out_2;
            Unity_Multiply_float_float(_Split_a10bc08a4d924af8a37eb130f5c032d7_R_1, _Property_a58b2c51348e419f9059421f06756713_Out_0, _Multiply_e04816ac47064c8da6541039582a6bac_Out_2);
            float _Sine_00ef0460b3d14e439203d5216cfaed78_Out_1;
            Unity_Sine_float(_Multiply_e04816ac47064c8da6541039582a6bac_Out_2, _Sine_00ef0460b3d14e439203d5216cfaed78_Out_1);
            float3 _Hue_49577b74efd34cbabe9a8c6f95054403_Out_2;
            Unity_Hue_Normalized_float((Color_1a92fef6b8fa42bd97d2bcc6da64d839.xyz), _Sine_00ef0460b3d14e439203d5216cfaed78_Out_1, _Hue_49577b74efd34cbabe9a8c6f95054403_Out_2);
            float _Property_f0e2372cb920480dba7c25b1140f26e2_Out_0 = _Rotate;
            float2 _Rotate_b1748c2d60e84974b75df5bb194a05f8_Out_3;
            Unity_Rotate_Radians_float(IN.uv0.xy, float2 (0.5, 0.5), _Property_f0e2372cb920480dba7c25b1140f26e2_Out_0, _Rotate_b1748c2d60e84974b75df5bb194a05f8_Out_3);
            float _Split_c7f86b574fd6493e8373025425d44e6f_R_1 = _Rotate_b1748c2d60e84974b75df5bb194a05f8_Out_3[0];
            float _Split_c7f86b574fd6493e8373025425d44e6f_G_2 = _Rotate_b1748c2d60e84974b75df5bb194a05f8_Out_3[1];
            float _Split_c7f86b574fd6493e8373025425d44e6f_B_3 = 0;
            float _Split_c7f86b574fd6493e8373025425d44e6f_A_4 = 0;
            float _Subtract_7256d70390694f588bba7d074925548f_Out_2;
            Unity_Subtract_float(_Split_c7f86b574fd6493e8373025425d44e6f_R_1, 1.2, _Subtract_7256d70390694f588bba7d074925548f_Out_2);
            float _Property_5d6d818318f14114ba605df21009fed6_Out_0 = _UnscaledTime;
            float _Property_dd94e6ecea024170b84e73a59e58a185_Out_0 = _CycleTime;
            float _Modulo_3936eba955a7482282c7351804399c5e_Out_2;
            Unity_Modulo_float(_Property_5d6d818318f14114ba605df21009fed6_Out_0, _Property_dd94e6ecea024170b84e73a59e58a185_Out_0, _Modulo_3936eba955a7482282c7351804399c5e_Out_2);
            float _Property_71f8b8f90cec4ba1aaf659e06fe8f235_Out_0 = _wavespeed;
            float _Multiply_f041f83d64b1475694ba2efc8f4e7a57_Out_2;
            Unity_Multiply_float_float(_Modulo_3936eba955a7482282c7351804399c5e_Out_2, _Property_71f8b8f90cec4ba1aaf659e06fe8f235_Out_0, _Multiply_f041f83d64b1475694ba2efc8f4e7a57_Out_2);
            float _Add_d487fcc600004b16a3a23f4db5a8f462_Out_2;
            Unity_Add_float(_Subtract_7256d70390694f588bba7d074925548f_Out_2, _Multiply_f041f83d64b1475694ba2efc8f4e7a57_Out_2, _Add_d487fcc600004b16a3a23f4db5a8f462_Out_2);
            float _Property_7b18469ca7ff4776ab6bf177418d7dce_Out_0 = _power;
            float _Multiply_c4bd5033f4f1479385a7fd38748de3c8_Out_2;
            Unity_Multiply_float_float(_Add_d487fcc600004b16a3a23f4db5a8f462_Out_2, _Property_7b18469ca7ff4776ab6bf177418d7dce_Out_0, _Multiply_c4bd5033f4f1479385a7fd38748de3c8_Out_2);
            float _Saturate_c64eb0216f1f459d9d0bcf84b8d2dbf2_Out_1;
            Unity_Saturate_float(_Multiply_c4bd5033f4f1479385a7fd38748de3c8_Out_2, _Saturate_c64eb0216f1f459d9d0bcf84b8d2dbf2_Out_1);
            float _TriangleWave_6ecf9adc48554e6d9ed2c378d0ab6711_Out_1;
            TriangleWave_float(_Saturate_c64eb0216f1f459d9d0bcf84b8d2dbf2_Out_1, _TriangleWave_6ecf9adc48554e6d9ed2c378d0ab6711_Out_1);
            float3 _Multiply_04cb7c12646f43e88ad98dcaaffdabdf_Out_2;
            Unity_Multiply_float3_float3(_Hue_49577b74efd34cbabe9a8c6f95054403_Out_2, (_TriangleWave_6ecf9adc48554e6d9ed2c378d0ab6711_Out_1.xxx), _Multiply_04cb7c12646f43e88ad98dcaaffdabdf_Out_2);
            float _Property_002aeebed8ec4677b27b30c8e69293fd_Out_0 = _Strength;
            float3 _Multiply_19c286fcf62545748c187ffc5d548630_Out_2;
            Unity_Multiply_float3_float3(_Multiply_04cb7c12646f43e88ad98dcaaffdabdf_Out_2, (_Property_002aeebed8ec4677b27b30c8e69293fd_Out_0.xxx), _Multiply_19c286fcf62545748c187ffc5d548630_Out_2);
            float _Property_fd70ac17c42d41869a03c1b612f001cd_Out_0 = _alpha;
            surface.BaseColor = _Multiply_19c286fcf62545748c187ffc5d548630_Out_2;
            surface.Alpha = _Property_fd70ac17c42d41869a03c1b612f001cd_Out_0;
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
    }
    CustomEditorForRenderPipeline "UnityEditor.ShaderGraphUnlitGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}