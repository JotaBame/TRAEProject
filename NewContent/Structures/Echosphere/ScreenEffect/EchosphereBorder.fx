sampler uImage0 : register(s0);
float uTime;
float2 uScreenResolution;
texture uImage1;
float uIntensity;
float4 uColor;
float4 uSecondaryColor;
float uProgress;

sampler perlin = sampler_state
{
    Texture = (uImage1);
    AddressU = wrap;
    AddressV = wrap;
    Filter = point;
};
float2 rotate(float2 vecToRotate, float amountRadians, float2 center)
{
    float2 translated = vecToRotate - center;
    float cosA = cos(amountRadians);
    float sinA = sin(amountRadians);
    float2 rotated = float2(
        translated.x * cosA - translated.y * sinA,
        translated.x * sinA + translated.y * cosA
    );
    return rotated + center;
}

float2 scale(float2 vecToScale, float scaleAmount, float2 center)
{
    float2 translated = vecToScale - center;
    float2 scaled = translated * scaleAmount;
    return scaled + center;
}

float invLerp(float a, float b, float value)
{
    return clamp((value - a) / max(b - a, 0.0001), 0.0, 1.0);
}

float4 BorderVFX(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 uv = coords;
    float2 screenshotUv = uv;
    float2 center = float2(0.5, 0.5);
    float aspect = (uScreenResolution.y / uScreenResolution.x);

    float rotateAmount = length(uv - center);
    float distToCenter = rotateAmount;
    float rotateAmountMult = invLerp(0.25, 2.0, distToCenter);
    rotateAmountMult *= 3.0;
    rotateAmountMult /= distToCenter;
    rotateAmount = sin(rotateAmount * 30.0 + uTime * 0.2) * rotateAmountMult;
    
    uv = rotate(uv, rotateAmount, center);
    uv = scale(uv, 0.1, center);
    
    float2 origUv = uv;
    float2 distUv = uv;
    distUv.x /= aspect;
    
    uv.x += uTime * 0.02;

    float noiseDistortAmount = 0.3;
    float noise = tex2D(perlin, distUv).x * noiseDistortAmount - (noiseDistortAmount * 0.5);
    uv.xy += noise;

    noise = tex2D(perlin, uv).x * noiseDistortAmount - (noiseDistortAmount * 0.5);
    uv = origUv;
    uv.xy -= noise;

    noise = tex2D(perlin, uv).x * noiseDistortAmount;
    uv = origUv;
    uv.xy += noise;

    float4 purpleRGBA = uColor;
    
    
    //uProgress is only set to 1 on getfixedboi worlds, otherwise it is set to 0
    //displace it so it's nigh unplayable
    screenshotUv.xy += noise * uIntensity * uProgress;
    
    float4 fragColor = tex2D(uImage0, screenshotUv);
    //workaround for no custom params
    float swirlFadeStart = uSecondaryColor.g;
    float swirlFadeEnd = uSecondaryColor.b;
    float tintStrength = uSecondaryColor.r;
    //using swirlFadeEnd as the second parameter causes a compiler error
    float strengthMult = invLerp(swirlFadeStart, swirlFadeEnd, distToCenter);
    noise *= strengthMult;
    noise *= uIntensity;
    fragColor += purpleRGBA * noise;
    fragColor = lerp(fragColor, purpleRGBA, tintStrength * uIntensity); 
    return fragColor;
}

technique Technique1
{
    pass BorderPass
    {
        PixelShader = compile ps_3_0 BorderVFX();
    }
}