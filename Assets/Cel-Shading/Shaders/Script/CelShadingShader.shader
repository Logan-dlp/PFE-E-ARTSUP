Shader "logandlp/CustomShaders/CelShading"
{
    Properties
    {
        _mainColor("Main Color", Color) = (1, 1, 1, 1)
        _mainTexture("Texture", 2D) = "white" {}
        _cutoffTresholds("Cutoff Tresholds", Vector) = (-0.05, 0.05, 0.0, 0.0)
        _ambientLightStrength("Ambient Light Strength", Range(0.0, 1.0)) = 0.005
    }
    
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalRenderPipeline"
        }

        Cull Off
        ZWrite On
        Pass
        {
            Name "CelShading"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _FORWARD_PLUS
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _mainColor;
                float2 _cutoffTresholds;
                float _ambientLightStrength;
            CBUFFER_END

            TEXTURE2D(_mainTexture);
            SAMPLER(sampler_mainTexture);
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float3 worldPos     : TEXCOORD2;
                float3 normalWS     : TEXCOORD1;
                float2 uv           : TEXCOORD0;
            };            
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                    OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                    OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                    OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                    OUT.uv = IN.uv;
                
                return OUT;
            }

            float3 MyLightingFunction(float3 normalWS, Light light)
            {
                float3 color = dot(light.direction, normalWS);
                color = lerp(_ambientLightStrength, 1.0f, smoothstep(_cutoffTresholds.x, _cutoffTresholds.y, color));
                color *= light.color;
                color *= light.distanceAttenuation;

                return color;
            }

            float3 MyLightLoop(float3 color, InputData inputData)
            {
                float3 lighting = 0;
                
                Light mainLight = GetMainLight();
                lighting += MyLightingFunction(inputData.normalWS, mainLight);
                
                #if defined(_ADDITIONAL_LIGHTS)
                
                #if USE_FORWARD_PLUS
                UNITY_LOOP for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
                {
                    Light additionalLight = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1,1,1,1));
                    lighting += MyLightingFunction(inputData.normalWS, additionalLight);
                }
                #endif
                
                uint pixelLightCount = GetAdditionalLightsCount();
                LIGHT_LOOP_BEGIN(pixelLightCount)
                    Light additionalLight = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1,1,1,1));
                    lighting += MyLightingFunction(inputData.normalWS, additionalLight);
                LIGHT_LOOP_END
                
                #endif
                
                return color * lighting;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                InputData inputData = (InputData)0;
                inputData.positionWS = IN.worldPos;
                inputData.normalWS = IN.normalWS;
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.worldPos);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionHCS);

                half4 textureColor = SAMPLE_TEXTURE2D(_mainTexture, sampler_mainTexture, IN.uv);
                float3 baseColor = textureColor.rgb * _mainColor.rgb;
                
                float3 lighting = MyLightLoop(baseColor, inputData);
                
                return half4(lighting, 1.0f);
            }
            
            ENDHLSL
        }
    }
}