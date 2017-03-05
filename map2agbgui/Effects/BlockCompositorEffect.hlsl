/*

This shader composes 8 tiles with attributes and 2 tileset textures into a block

A tile has the following layout: [x = hFlip, y = vFlip, z = palIndex, w = tileID]

Notice! Pixels are offset by 0.5, so a pixel in a textute is at [x+0.5, y+0.5], or in texture coordinates: [(1/width)*x + (1/(width*2)), (1/height)*y + (1/(height*2))]

*/

static const float one12th = 1.0 / 12.0;
static const float one16th = 1.0 / 16.0;
static const float one32rd = 1.0 / 32.0;

float4 bottomLayer_TopLeft : register(C0);
float4 bottomLayer_TopRight : register(C1);
float4 bottomLayer_BottomLeft : register(C2);
float4 bottomLayer_BottomRight : register(C3);
float4 topLayer_TopLeft : register(C4);
float4 topLayer_TopRight : register(C5);
float4 topLayer_BottomLeft : register(C6);
float4 topLayer_BottomRight : register(C7);

float secondary : register(C8);

sampler2D implicitInputSampler : register(S0);
sampler2D lowGraphic : register(S1);
sampler2D highGraphic : register(S2);
sampler2D mergedPalette : register(S3);

float lowGraphicWidth : register(C9);
float lowGraphicHeight : register(C10);
float highGraphicWidth : register(C11);
float highGraphicHeight : register(C12);

float4 main(float2 uv : TEXCOORD) : COLOR
{
   /*
     //float x = floor(uv.x * 16.0);
    //float y = floor(uv.y * 16.0);

    //only bottom for now

    //if(uv.x < 0.5)
    //{
    //    if (uv.y < 0.5)
    //    {
    //        //render TopLeft

    //        //calculate pixel
    //        //float offset = (bottomLayer_TopLeft.w * 8.0);
    //        //float offsetX = ((offset % lowGraphicWidth) / lowGraphicWidth) + ((uv.x * 2) / lowGraphicWidth);
    //        //float offsetY = ((floor(offset / lowGraphicWidth)) / lowGraphicHeight) + ((uv.y * 2) / lowGraphicHeight);

    //        //float x = ((5.0 + (uv.x * 2.0)) * 8.0) / 128.0;
    //        //float y = ((5.0 + (uv.y * 2.0)) * 8.0) / 320.0;

    //        float palIndex = tex2D(lowGraphic, uv).a;
    //        if (palIndex > one16th) //opaque?
    //        {
    //            float palCoordY = ((bottomLayer_TopLeft.z / 6.0) + one12th) / 2.0; //calculate which palette to use (palette sampler y offset)
    //            if (secondary) //if secondary, shift palette down
    //            {
    //                palCoordY += 0.5;
    //            }
    //            return tex2D(mergedPalette, float2(palIndex, palCoordY));
    //        }
    //        else //transparent?
    //        {
    //            return float4(0, 0, 0, 0);
    //        }
    //    }
    //    else
    //    {
    //        //render BottomLeft
    //    }
    //}
    //else
    //{
    //    if (uv.y < 0.5)
    //    {
    //        //render TopRight
    //    }
    //    else
    //    {
    //        //render BottomRight
    //    }
    //}

    //return float4(0, 0, 0, 1);



    float palIndex = tex2D(lowGraphic, uv).a;
    if (palIndex > one16th) //opaque?
    {
        float palCoordY = one32rd; //bottomLayer_TopLeft.z; //calculate which palette to use (palette sampler y offset)
        if (secondary) //if secondary, shift palette down
        {
            palCoordY += 0.5;
        }
        return tex2D(mergedPalette, float2(palIndex, palCoordY));
    }
    else //transparent?
    {
        return float4(0, 0, 0, 0);
    }
    */

    //float2(((16.0 + floor(uv.x * 16.0)) / 128.0) + (1.0 / 256.0), ((floor(uv.y * 16.0)) / 320.0) + (1.0 / 640.0))
    
    return tex2D(lowGraphic, uv);

}