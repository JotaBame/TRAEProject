sampler uImage0 : register(s0);
texture cellNoiseTexture1;
texture cellNoiseTexture2;
texture cellNoiseTexture3;
texture cellNoiseTexture4;
texture gradientTexture;
float opacity;
float time;
float rotation;
float scale;
sampler noise1 = sampler_state
{
    Texture = (cellNoiseTexture1);
    AddressU = wrap;
    AddressV = wrap;
    Filter = point;
};
sampler noise2 = sampler_state
{
    Texture = (cellNoiseTexture2);
    AddressU = wrap;
    AddressV = wrap;
    Filter = point;
};
sampler noise3 = sampler_state
{
    Texture = (cellNoiseTexture3);
    AddressU = wrap;
    AddressV = wrap;
    Filter = point;
};
sampler noise4 = sampler_state
{
    Texture = (cellNoiseTexture4);
    AddressU = wrap;
    AddressV = wrap;
    Filter = point;
};
sampler gradient = sampler_state
{
    Texture = (gradientTexture);
    AddressU = wrap;
    AddressV = wrap;
    Filter = point;
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
float4 blurredSample(float kernelSpacing, int kernelHalfSize, float std, sampler textureSampler, float2 coords)
{
    const float sqrtTau = 2.50662827463;
    float result = 0;
    for (int i = -kernelHalfSize; i <= kernelHalfSize; i++)
    {
        for (int j = -kernelHalfSize; j <= kernelHalfSize; j++)
        {
            float2 offset = float2(i, j) * kernelSpacing;
            result += (1f / (-std * sqrtTau)) * exp(-tex2D(textureSampler, coords) / (2 * std * std));

        }
    }
    return result;
}
float4 squares(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{

    
    float finalVal = blurredSample(0.02f, 4, 1, noise1, coords);
    return float4(finalVal, finalVal, finalVal, 1);

}

technique Technique1
{
    pass RainbowyPass
    {
        PixelShader = compile ps_3_0 squares();
    }
}