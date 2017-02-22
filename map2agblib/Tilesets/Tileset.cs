using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Tilesets
{
    /// <summary>
    /// Represents a tileset that is referenced by a map
    /// </summary>
    public class Tileset
    {
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
        public String Graphic { get; set; }

        /// <summary>
        /// Reference to an array of 6 Palettes (with 16 colors each)
        /// </summary>
        public Color.Palette[] P { get; set; }

        /// <summary>
        /// Reference to a list of blocks (tilemaps as well as behaviors)
        /// </summary>
        public List<Tuple<BlockTileMap, BlockBehaviour>> Blocks { private set; get; }

        /// <summary>
        /// Refers to a label or offset of the animation init function
        /// </summary>
        public String AnimationInitFunction { set; get; }



        #endregion

        #region Constructor
        public Tileset()
        {
            //todo
        }
        #endregion

    }
}
