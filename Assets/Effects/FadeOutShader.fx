sampler uImage0 : register(s0);
float4 uSourceRect;
float2 uImageSize0;
float beginPoint;
float speed;

float4 main(float4 samplerColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);

    float frameY = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;    
    color *= 1 - saturate(frameY - beginPoint) * speed;

    return color * samplerColor;
}

technique Technique1
{
	pass pass1
	{
		PixelShader = compile ps_2_0 main();
	}
}