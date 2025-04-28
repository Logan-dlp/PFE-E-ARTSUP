#ifndef CUSTOM_CEL_SHADING_H
#define CUSTOM_CEL_SHADING_H

#pragma multi_compile _ _ADDITIONAL_LIGHTS
#pragma multi_compile _ _FORWARD_PLUS

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

float3 MyLightingFunction(float3 normalWS, Light light, float ambientLightStrength, float2 cutoffTresholds)
{
    float3 color = dot(light.direction, normalWS);
    color = lerp(ambientLightStrength, 1.0f, smoothstep(cutoffTresholds.x, cutoffTresholds.y, color));
    color *= light.color;
    color *= light.distanceAttenuation;

    return color;
}

float3 MyLightLoop(InputData inputData, float ambientLightStrength, float2 cutoffTresholds)
{
    float3 lighting = 0;
                
    Light mainLight = GetMainLight();
    lighting += MyLightingFunction(inputData.normalWS, mainLight, ambientLightStrength, cutoffTresholds);
                
    #if defined(_ADDITIONAL_LIGHTS)
                
    #if USE_FORWARD_PLUS
    UNITY_LOOP for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
    {
        Light additionalLight = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1,1,1,1));
        lighting += MyLightingFunction(inputData.normalWS, additionalLight, ambientLightStrength, cutoffTresholds);
    }
    #endif
                
    uint pixelLightCount = GetAdditionalLightsCount();
    LIGHT_LOOP_BEGIN(pixelLightCount)
        Light additionalLight = GetAdditionalLight(lightIndex, inputData.positionWS, half4(1,1,1,1));
    lighting += MyLightingFunction(inputData.normalWS, additionalLight, ambientLightStrength, cutoffTresholds);
    LIGHT_LOOP_END
                
    #endif
                
    return lighting;
}

void CelShading_float(float3 positionWS, float3 normalWS, float3 positionOS, float ambientLightStrength, float2 cutoffTresholds, out float3 lightColor)
{
    InputData inputData = (InputData)0;
    inputData.positionWS = positionWS;
    inputData.normalWS = normalWS;
    inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(positionWS);
    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(positionOS);

    float3 lighting = MyLightLoop(inputData, ambientLightStrength, cutoffTresholds);

    lightColor = half4(lighting, 1.0f);
}

#endif // CUSTOM_CEL_SHADING_H