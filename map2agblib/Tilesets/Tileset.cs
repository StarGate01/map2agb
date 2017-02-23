using System;
using System.Linq;
using System.Xml.Serialization;
using map2agblib.Imaging;

namespace map2agblib.Tilesets
{
    /// <summary>
    /// Represents a tileset that is referenced by a map
    /// </summary>
    public class Tileset
    {
        const int MAX_SECOND_TILESET_SIZE = 0x180;
        const int MAX_FIRST_TILESET_SIZE = 0x280;
        const int MAX_PALETTES = 6;

        #region Properties

        /// <summary>
        /// Is the tileset lz77 compressed
        /// </summary>
        public bool Compressed { get; set; }

        /// <summary>
        /// Is the tileset primary (Pal 0-5 used) or secondary (Pal 6-11 used)
        /// </summary>
        public bool Secondary { get; set; }

        /// <summary>
        /// Filler
        /// </summary>
        public byte Field2 { get; set; }

        /// <summary>
        /// Filler
        /// </summary>
        public byte Field3 { get; set; }

        /// <summary>
        /// Reference to a compiled (and maybe compressed) .png 4 4bpp tileset graphic
        /// </summary>
        public string Graphic { get; set; }

        /// <summary>
        /// Reference to an array of 6 Palettes (with 16 colors each)
        /// </summary>
        public Palette[] Palettes { get; set; }

        /// <summary>
        /// Reference to a list of blocks (tilemaps as well as behaviors)
        /// </summary>
        public TilesetEntry[] Blocks { get; set; }

        

        /// <summary>
        /// Refers to a label or offset of the animation init function
        /// </summary>
        public string AnimationInitFunction { set; get; }



        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new Tileset with the default Array sizes and given parameters
        /// </summary>
        /// <param name="isSecondary">true: the tileset is classified as secondary and has less blocks</param>
        /// <param name="compressed">true: the tileset will be compiled with the -gzl flag on grit</param>
        public Tileset(bool isSecondary, bool compressed)
        {
            Compressed = compressed;
            Secondary = isSecondary;

            Blocks = Secondary ? new TilesetEntry[MAX_SECOND_TILESET_SIZE] : new TilesetEntry[MAX_FIRST_TILESET_SIZE];
            Palettes = Enumerable.Repeat(new Palette(), MAX_PALETTES).ToArray();

            Graphic = string.Empty;
            AnimationInitFunction = string.Empty;
        }

        public Tileset()
        {

        }
        #endregion

    }
}
