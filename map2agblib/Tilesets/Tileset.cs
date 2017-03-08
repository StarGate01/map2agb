using System;
using System.Linq;
using map2agblib.Imaging;
using System.Runtime.Serialization;
using map2agblib.Data;
using System.Drawing;

namespace map2agblib.Tilesets
{
    /// <summary>
    /// Represents a tileset that is referenced by a map
    /// </summary>
    [DataContract]
    public class Tileset : XMLImportExport<Tileset>
    {

        public const int MAX_SECOND_TILESET_SIZE = 0x180;
        public const int MAX_FIRST_TILESET_SIZE = 0x280;
        public const int MAX_PALETTES = 6;

        #region Properties

        /// <summary>
        /// Is the tileset lz77 compressed
        /// </summary>
        [DataMember]
        public bool Compressed { get; set; }

        /// <summary>
        /// Is the tileset primary (Pal 0-5 used) or secondary (Pal 6-11 used)
        /// </summary>
        [DataMember]
        public bool Secondary { get; set; }

        /// <summary>
        /// Filler
        /// </summary>
        [DataMember]
        public byte Field2 { get; set; }

        /// <summary>
        /// Filler
        /// </summary>
        [DataMember]
        public byte Field3 { get; set; }

        /// <summary>
        /// Reference to a compiled (and maybe compressed) .png 4 4bpp tileset graphic
        /// </summary>
        [DataMember]
        public string Graphic { get; set; }

        /// <summary>
        /// Reference to an array of 6 Palettes (with 16 colors each)
        /// </summary>
        [DataMember]
        public Palette[] Palettes { get; set; }

        /// <summary>
        /// Reference to a list of blocks (tilemaps as well as behaviors)
        /// </summary>
        [DataMember]
        public TilesetEntry[] Blocks { get; set; }

        /// <summary>
        /// Refers to a label or offset of the animation init function
        /// </summary>
        [DataMember]
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
            for (int i = 0; i < Blocks.Length; i++) Blocks[i] = new TilesetEntry();
            Palettes = new Palette[MAX_PALETTES];
            for (int i = 0; i < Palettes.Length; i++) Palettes[i] = new Palette();

            Graphic = string.Empty;
            AnimationInitFunction = string.Empty;
        }

        public Tileset() : this(false, false)
        {
         
        }

        #endregion

    }
}
