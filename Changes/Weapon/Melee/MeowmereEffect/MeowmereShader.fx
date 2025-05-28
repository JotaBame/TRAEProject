sampler uImage0 : register(s0);
texture cellNoiseTexture1;
texture cellNoiseTexture2;
texture cellNoiseTexture3;
texture cellNoiseTexture4;
texture gradientTexture;
float2 coordOffset;
float opacity;
float time;
float rotation;
float scale;
sampler noise1 = sampler_state
{
    Texture = (cellNoiseTexture1);
    AddressU = wrap;
    AddressV = wrap;
};
sampler noise2 = sampler_state
{
    Texture = (cellNoiseTexture2);
    AddressU = wrap;
    AddressV = wrap;
};
sampler noise3 = sampler_state
{
    Texture = (cellNoiseTexture3);
    AddressU = wrap;
    AddressV = wrap;
};
sampler noise4 = sampler_state
{
    Texture = (cellNoiseTexture4);
    AddressU = wrap;
    AddressV = wrap;
};
sampler gradient = sampler_state
{
    Texture = (gradientTexture);
    AddressU = wrap;
    AddressV = wrap;
};
float3 RGBtoHSL(float3 rgb)
{
    float cMax = max(max(rgb.r, rgb.g), rgb.b);
    float cMin = min(min(rgb.r, rgb.g), rgb.b);
    float delta = cMax - cMin;
    float hue = 0.0;
    if (delta > 0.00001)
    {
        if (cMax == rgb.r)
            hue = fmod(((rgb.g - rgb.b) / delta), 6.0);
        else if (cMax == rgb.g)
            hue = ((rgb.b - rgb.r) / delta) + 2.0;
        else
            hue = ((rgb.r - rgb.g) / delta) + 4.0;
        hue /= 6.0;
        if (hue < 0.0)
            hue += 1.0;
    }
    float light = (cMax + cMin) * 0.5;
    float sat = 0.0;
    if (delta > 0.00001)
        sat = delta / (1.0 - abs(2.0 * light - 1.0));
    return float3(hue, sat, light);
}

float HueToRGB(float p, float q, float t)
{
    if (t < 0.0)
        t += 1.0;
    if (t > 1.0)
        t -= 1.0;
    if (t < 1.0 / 6.0)
        return p + (q - p) * 6.0 * t;
    if (t < 1.0 / 2.0)
        return q;
    if (t < 2.0 / 3.0)
        return p + (q - p) * (2.0 / 3.0 - t) * 6.0;
    return p;
}

float3 HSLtoRGB(float3 hsl)
{
    float h = hsl.x;
    float s = hsl.y;
    float l = hsl.z;
    float r, g, b;
    if (s == 0.0)
    {
        r = g = b = l;
    }
    else
    {
        float q = l < 0.5 ? l * (1.0 + s) : l + s - l * s;
        float p = 2.0 * l - q;
        r = HueToRGB(p, q, h + 1.0 / 3.0);
        g = HueToRGB(p, q, h);
        b = HueToRGB(p, q, h - 1.0 / 3.0);
    }
    return float3(r, g, b);
}

float4 HSLLerp(float4 rgba1, float4 rgba2, float t)
{
    float3 hsl1 = RGBtoHSL(rgba1.rgb);
    float3 hsl2 = RGBtoHSL(rgba2.rgb);
    if (abs(hsl1.x - hsl2.x) > 0.5)
    {
        if (hsl1.x > hsl2.x)
            hsl2.x += 1.0;
        else
            hsl1.x += 1.0;
    }
    float3 hsl = lerp(hsl1, hsl2, t);
    hsl.x = fmod(hsl.x, 1.0);
    float alpha = lerp(rgba1.a, rgba2.a, t);
    return float4(HSLtoRGB(hsl), alpha);
}
float xorWrappedBlend(float a, float b)
{
    float ab = a * b;
    return frac((a + b - ab) * (1 - ab));
}
float4 rainbowy(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 rotatedCoords = coords;
    coords -= .5;
    float2 centeredCoords = coords;
    coords *= scale;
    coords /= 2;
    coords += .5;
    float cosAmt = cos(rotation);
    float sinAmt = sin(rotation);
    rotatedCoords.x = (centeredCoords.x * cosAmt) - (centeredCoords.y * sinAmt);
    rotatedCoords.y = (centeredCoords.x * sinAmt) + (centeredCoords.y * cosAmt);
    rotatedCoords += .5;
    float4 currentCol = tex2D(uImage0, rotatedCoords);
    //return float4(rotatedCoords.x, rotatedCoords.y, 0, 1);
    coords += coordOffset;
    float2 noise1UV = coords;
    float2 noise2UV = coords;
    float2 noise3UV = coords;
    float2 noise4UV = coords;
    noise1UV.x += time;
    noise2UV.x -= time;
    noise3UV.y += time;
    noise4UV.y -= time;
    float noise1Val = tex2D(noise1, noise1UV);
    float noise2Val = tex2D(noise2, noise2UV);
    float noise3Val = tex2D(noise3, noise3UV);
    float noise4Val = tex2D(noise4, noise4UV);
    float noiseSquishAmount = 4;
    noise1UV *= noiseSquishAmount;
    noise2UV *= noiseSquishAmount;
    noise3UV *= noiseSquishAmount;
    noise4UV *= noiseSquishAmount;
    
    float noiseValSum = noise1Val + noise2Val + noise3Val + noise4Val;
    noise1Val = tex2D(noise1, noise1UV);
    noise2Val = tex2D(noise2, noise2UV);
    noise3Val = tex2D(noise3, noise3UV);
    noise4Val = tex2D(noise4, noise4UV);
    //float shinynessCheck = atan2(noise2Val + noise1Val, noise3Val + noise4Val);
    float shinynessCheck = xorWrappedBlend(max(noise2Val, noise1Val), max(
    noise3Val, noise4Val));
    if (shinynessCheck > .7)
    {
        return currentCol.aaaa * opacity;
    }
    float offsetValue = frac(noiseValSum); // xorWrappedBlend(xorWrappedBlend(noise3Val, noise4Val), xorWrappedBlend(noise1Val, noise2Val));
    offsetValue *= 0.2f;
    float curA = currentCol.a * opacity;
    float2 sampleCoords = coords;
    sampleCoords.x += offsetValue + time * 3;
    float3 gradientCol = HSLtoRGB(float3(frac(sampleCoords.x), 1, .65f));
    float4 resultCol = float4(gradientCol.r * curA, gradientCol.g * curA, gradientCol.b * curA, curA);
    return resultCol;
}

technique Technique1
{
    pass RainbowyPass
    {
        PixelShader = compile ps_3_0 rainbowy();
    }
}