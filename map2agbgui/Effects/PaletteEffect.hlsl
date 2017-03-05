sampler2D implicitInputSampler : register(S0);
sampler2D palette : register(S1);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float index = tex2D(implicitInputSampler, uv).a;
    return tex2D(palette, float2(index, 0));
}