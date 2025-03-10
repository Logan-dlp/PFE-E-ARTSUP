#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
struct SurfaceVariables {

	float3 normal;

};

float3 ClaculateCelShading(Light l, SurfaceVariables s) {
	float diffuse = saturate(dot(s.normal, l.direction));

	float3 h = SafeNormalize(l.direction + s.view);
	float specular = saturate(dot(s.normal, h));
	specular = pow(specular, s.shininess);
	specular *= diffuse * s.smoothness;

	float rim = 1 - dot(s.view, s.normal);
	rim *= pow(diffuse, s.rimTreshold);

	return l.color * (diffuse + max(specular, rim));
	}
#endif
	

void LightingCelShaded_float(float Smoothness, float RimTresholdf, float3 position, loat3 Normal, float3 view, out float3 Color) {
#if defined(SHADERGRAPH_PREVIEW)

#else
	// Initilize and populate SurfaceVariables s
	SurfaceVariables s;
	s.normal = normalize(Normal);
	s.view = SafeNormalize(view);
	s.smoothness = Smoothness;
	s.shininess = exp2(10 * Smoothness + 1);
	s.rimTreshold = RimTreshold;
    
#if SHADOW_SCREEN
	float4 clipPos = TransformWorldToHClip (position);
	float4 shadowCoord = ComputeScreenPos(clipPos);
#else
	float4 shadowCoord = TransformWorldToShadowCoord(position);
#endif

    Light light = GetMainLight();
	Color = CalculateCelShading(light, s);
#endif
	}

#endif