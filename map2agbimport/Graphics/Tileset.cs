using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbimport.Graphics
{
    public class Tileset
    {
        public Tile[] Tiles { get; set; }

        public Tileset(byte[] rawBytes)
        {
            if (rawBytes.Length % 32 != 0)
                throw new ArgumentException("must have a multiple of 32 members", "rawBytes");

            Tiles = new Tile[rawBytes.Length / 32];

            for (int i = 0; i < Tiles.Length; ++i)
            {
                Tiles[i] = new Tile(rawBytes, i);
            }
        }

        public Bitmap ToBitmap()
        {
            return null;
        }
    }
}
