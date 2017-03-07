/*

This shader applies a palette texture on a texture with indexed colors.

Palette sampler texture looks like this:
|0|1|2| ... |14|15|
|0|1|2| ... |14|15|
...
|0|1|2| ... |14|15|

15 colums and 6 rows.
Rows = palettes, index corresponds to C0 (paletteIndex), 
Colums = colors of palette

The processed texture must have the color index as value of the alpha channel, in a rage from 0.0 to 1.0 (Colors 0x00 - 0xFF) 

Notice! Pixels are offset by 0.5, so a pixel in a textute is at [x+0.5, y+0.5], or in texture coordinates: [(1/width)*x + (1/(width*2)), (1/height)*y + (1/(height*2))]

*/

//static const float one12th = 1.0 / 12.0;
//static const float one16th = 1.0 / 16.0;

float paletteIndex : register(C0);
float overlayPaletteIndex : register(C1);
float hFlip : register(C2);
float vFlip : register(C3);
float overlayHFlip : register(C4);
float overlayVFlip : register(C5);

sampler2D implicitInputSampler : register(S0);
sampler2D overlay : register(S1);
sampler2D palette : register(S2);
sampler2D overlayPalette : register(S3);

float4 main(float2 uv : TEXCOORD) : COLOR
{
    return tex2D(implicitInputSampler, uv);

}