#ifndef INCLUDED_HELPERS
#define INCLUDED_HELPERS

float3 GetViewDirectionFromPosition(float3 positionWS) {
    return normalize(GetCameraPositionWS() - positionWS);
}

float3 GetViewToPointDir (float3 positionWS) {
    return normalize(positionWS - GetCameraPositionWS());
}

float4 CalculateShadowCoord(float3 positionWS, float4 positionCS) {
#if SHADOWS_SCREEN
    return ComputeScreenPos(positionCS);
#else
    return TransformWorldToShadowCoord(positionWS);
#endif
}

#endif