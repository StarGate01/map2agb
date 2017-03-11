using map2agblib.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

        public unsafe Bitmap ToImage(int tileWidth, ShortColor[] pal)
        {
            int width = tileWidth * 8;
            int height = (Tiles.Length / (tileWidth)) * 8;

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format4bppIndexed);
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            ColorPalette bmpPal = bmp.Palette;

            for (int i = 0; i < 16; ++i)
            {
                bmpPal.Entries[i] = Color.FromArgb(pal[i].Red * 8, pal[i].Green * 8, pal[i].Blue * 8);
            }

            bmp.Palette = bmpPal;
            for (int i = 0; i < height; ++i)
            {

                byte* currentByte = (byte*)(bmpData.Scan0 + i * bmpData.Stride);
                for (int j = 0; j < bmpData.Width / 2; ++j)
                {
                    /* unused locals, remove once finished */
                    /* TODO: find error, remove unused locals */
                    byte b = Tiles[(i / 8) * tileWidth + (j / 4)].Indices[(4 * (i % 8)) + (j % 4)];
                    byte n1 = (byte)(b & 0xF);
                    byte n2 = (byte)((b & 0xF0) >> 4);
                    b = (byte)((n2) | (byte)((n1) << 4));
                    *(currentByte++) = b;
                }
            }

            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
