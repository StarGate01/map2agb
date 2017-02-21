using System;
using map2agblib.Tilesets;

namespace map2agblib.Map
{
    /// <summary>
    /// Represents a MapFooter, an object containing information about the actual MapBlock and used Tilesets
    /// </summary>
    public class MapFooter
    {
        #region Properties

        /// <summary>
        /// The Width of the Map in Tiles (16x16)
        /// </summary>
        public uint Width { get; private set; }

        /// <summary>
        /// The Height of the Map in Tiles (16x16)
        /// </summary>
        public uint Height { get; private set; }

        //TODO: Create Block objects

        /// <summary>
        /// The BorderBlock which is displayed when the camera hits the border
        /// </summary>
        public ushort[][] BorderBlock { get; private set; }

        /// <summary>
        /// The actual MapBlock, contains Graphical tile data as well as the collision Map
        /// </summary>
        public ushort[][] MapBlock { get; private set; }

        /// <summary>
        /// The Main Tileset for this Map
        /// </summary>
        public Tileset FirstTileset { get; set; }

        /// <summary>
        /// The Secondary Tileset for this Map
        /// </summary>
        public Tileset SecondTileset { get; set; }

        //TODO: Create resize methode to resize MapBlock and BorderBlock

        /// <summary>
        /// The Width of the BorderBlock
        /// </summary>
        public byte BorderWidth { get; private set; }

        /// <summary>
        /// The Height of the BorderBlock
        /// </summary>
        public byte Borderheight { get; private set; }

        /// <summary>
        /// Bytes used to assure alignment, probably unused otherwise
        /// </summary>
        public ushort Padding { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new MapFooter object //TODO: Implement
        /// </summary>
        public MapFooter()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
