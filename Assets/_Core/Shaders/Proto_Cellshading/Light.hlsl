float DistanceAttenuation(float distanceSqr, float2 distanceAndSpotAttenuation)
{
float distance = sqrt(distanceSqr);
float range = rsqrt(distanceAndSpotAttenuation.x);
float distance01 = saturate(1 - (distance / range));
float lightAtten = pow(distance01, 2);
return lightAtten;
}