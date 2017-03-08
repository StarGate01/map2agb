using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbimport.Graphics
{
    public class TileCollection
    {
        public Tile[] Tiles { get; set; }

        public TileCollection(byte[] rawBytes)
        {
            if (rawBytes.Length % 32 != 0)
                throw new ArgumentException("must have a multiple of 32 members", "rawBytes");

            Tiles = new Tile[rawBytes.Length / 32];

            for (int i = 0; i < Tiles.Length; ++i)
            {
                Tiles[i] = new Tile(rawBytes, i);
            }
        }

        public unsafe Bitmap ToImage(int tileWidth)
        {
            int tileHeight = (Tiles.Length / (tileWidth));
            int width = tileWidth * 8;
            int height = tileHeight * 8;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format4bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format4bppIndexed);
            unsafe
            {
                for (int i = 0; i < height; ++i)
                {
                    byte* currentByte = (byte*)(bmpData.Scan0 + i * bmpData.Stride);
                    for (int j = 0; j < width / 2; ++j)
                    {
                        /* unused locals, remove once finished */
                        /* TODO: find error, remove unused locals */
                        int tileW = j / 4;
                        int tileH = i / 8;
                        int tileN = tileH * tileWidth + tileW;
                        int intX = j % 4;
                        int intY = i % 8;
                        int intN = (4 * intY) + intX;
                        *(currentByte++) = Tiles[tileN].Indices[intN];
                    }
                }
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
