Shader "Lpk/LightModel/ToonLightBase"
{
    Properties
    {
        _BaseMap            ("Texture", 2D)                       = "white" {}
        _BaseColor          ("Color", Color)                      = (0.5,0.5,0.5,1)
        
        [Space]
        _ShadowStep         ("ShadowStep", Range(0, 1))           = 0.5
        _ShadowStepSmooth   ("ShadowStepSmooth", Range(0, 1))     = 0.04
        
        [Space] 
        _SpecularStep       ("SpecularStep", Range(0, 1))         = 0.6
        _SpecularStepSmooth ("SpecularStepSmooth", Range(0, 1))   = 0.05
        [HDR]_SpecularColor ("SpecularColor", Color)              = (1,1,1,1)
        
        [Space]
        _RimStep            ("RimStep", Range(0, 1))              = 0.65
        _RimStepSmooth      ("RimStepSmooth",Range(0,1))          = 0.4
        _RimColor           ("RimColor", Color)                   = (1,1,1,1)
        
        [Space]   
        _OutlineWidth      ("OutlineWidth", Range(0.0, 1.0))      = 0.15
        _OutlineColor      ("OutlineColor", Color)                = (0.0, 0.0, 0.0, 1)
        
        // New props
        [Space]
        _PaletteTex        ("Palette Texture", 2D)                = "white" {}
        _BaseColorIndex    ("Base Color Index", Range(0,9))       = 0
        _SpecularColorIndex("Specular Color Index", Range(0,9))   = 0
        _RimColorIndex     ("Rim Color Index", Range(0,9))        = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        Pass
        {
            Name "UniversalForward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
             
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            TEXTURE2D(_PaletteTex); SAMPLER(sampler_PaletteTex); // Palette texture
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _ShadowStep;
                float _ShadowStepSmooth;
                float _SpecularStep;
                float _SpecularStepSmooth;
                float4 _SpecularColor;
                float _RimStepSmooth;
                float _RimStep;
                float4 _RimColor;
                float _BaseColorIndex;
                float _SpecularColorIndex;
                float _RimColorIndex;
            CBUFFER_END

            struct Attributes
            {     
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            }; 

            struct Varyings
            {
                float2 uv            : TEXCOORD0;
                float4 normalWS      : TEXCOORD1;
                float4 tangentWS     : TEXCOORD2;
                float4 bitangentWS   : TEXCOORD3;
                float3 viewDirWS     : TEXCOORD4;
                float4 shadowCoord   : TEXCOORD5;
                float4 fogCoord      : TEXCOORD6;
                float3 positionWS    : TEXCOORD7;
                float4 positionCS    : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                float3 viewDirWS = GetCameraPositionWS() - vertexInput.positionWS;

                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.uv = input.uv;
                output.normalWS = float4(normalInput.normalWS, viewDirWS.x);
                output.tangentWS = float4(normalInput.tangentWS, viewDirWS.y);
                output.bitangentWS = float4(normalInput.bitangentWS, viewDirWS.z);
                output.viewDirWS = viewDirWS;
                output.fogCoord = ComputeFogFactor(output.positionCS.z);
                output.shadowCoord = TransformWorldToShadowCoord(vertexInput.positionWS);
                return output;
            }

            float4 GetPaletteColor(float index)
            {
                float col = fmod(index, 5.0); 
                float row = floor(index / 5.0); 
                
                float blockSize = 70.0 / 1024.0;
                
                float uv_x = (col * blockSize) + (blockSize * 0.5);
                float uv_y = 1.0 - ((row * blockSize) + (blockSize * 0.5)); 
                
                return SAMPLE_TEXTURE2D(_PaletteTex, sampler_PaletteTex, float2(uv_x, uv_y));
            }
            
            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float2 uv = input.uv;
                float3 N = normalize(input.normalWS.xyz);
                float3 V = normalize(input.viewDirWS.xyz);
                float3 L = normalize(_MainLightPosition.xyz);
                float3 H = normalize(V + L);
                
                float NV = dot(N, V);
                float NH = dot(N, H);
                float NL = dot(N, L);
                NL = NL * 0.5 + 0.5;

                float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv);
                
                float4 baseColor = GetPaletteColor(_BaseColorIndex) * _BaseColor;
                float4 specularColor = GetPaletteColor(_SpecularColorIndex) * _SpecularColor;
                float4 rimColor = GetPaletteColor(_RimColorIndex) * _RimColor;

                float specularNH = smoothstep((1 - _SpecularStep * 0.05) - _SpecularStepSmooth * 0.05, (1 - _SpecularStep * 0.05) + _SpecularStepSmooth * 0.05, NH);
                float shadowNL = smoothstep(_ShadowStep - _ShadowStepSmooth, _ShadowStep + _ShadowStepSmooth, NL);

                input.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                float shadow = MainLightRealtimeShadow(input.shadowCoord);

                float rim = smoothstep((1 - _RimStep) - _RimStepSmooth * 0.5, (1 - _RimStep) + _RimStepSmooth * 0.5, 0.5 - NV);

                // Add new colors
                float3 diffuse = _MainLightColor.rgb * baseMap.rgb * baseColor.rgb * shadowNL * shadow;
                float3 specular = specularColor.rgb * shadow * shadowNL * specularNH;
                float3 ambient = rim * rimColor.rgb + SampleSH(N) * baseColor.rgb * baseMap.rgb;

                float3 finalColor = diffuse + ambient + specular;
                finalColor = MixFog(finalColor, input.fogCoord);
                return float4(finalColor, 1.0);
            }
            ENDHLSL
        }
        Pass
        {
            Name "Outline"
            Cull Front
            Tags
            {
                "LightMode" = "SRPDefaultUnlit"
            }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 pos      : SV_POSITION;
                float4 fogCoord : TEXCOORD0;
            };
            
            float _OutlineWidth;
            float4 _OutlineColor;
            
            v2f vert(appdata v)
            {
                v2f o;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.pos = TransformObjectToHClip(float4(v.vertex.xyz + v.normal * _OutlineWidth * 0.1 ,1));
                o.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 finalColor = MixFog(_OutlineColor, i.fogCoord);
                return float4(finalColor,1.0);
            }
            ENDHLSL
        }
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}