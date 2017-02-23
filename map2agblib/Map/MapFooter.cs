using System;
using map2agblib.Tilesets;
using System.Xml.Serialization;
using System.Linq;

namespace map2agblib.Map
{
    /// <summary>
    /// Represents a MapFooter, an object containing information about the actual MapBlock and used Tilesets
    /// </summary>
    public class MapFooter
    {
        #region Properties

        //TODO: Explicit setter to hold consistancy with BorderBlock and MapBlock Arrays
        /// <summary>
        /// The Width of the Map in Tiles (16x16)
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// The Height of the Map in Tiles (16x16)
        /// </summary>
        public uint Height { get; set; }

        //TODO: Create Block objects
        //TODO: Create explicit setter to hold consitancy with Width and Height or find a better solution
        /// <summary>
        /// The BorderBlock which is displayed when the camera hits the border
        /// </summary>
        public ushort[][] BorderBlock { get; set; }

        /// <summary>
        /// The actual MapBlock, contains Graphical tile data as well as the collision Map
        /// </summary>
        public ushort[][] MapBlock { get; set; }

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
        public byte BorderWidth { get; set; }

        /// <summary>
        /// The Height of the BorderBlock
        /// </summary>
        public byte BorderHeight { get; set; }

        /// <summary>
        /// Bytes used to assure alignment, probably unused otherwise
        /// </summary>
        public ushort Padding { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new MapFooter object //TODO: Tilesets
        /// </summary>
        public MapFooter(uint width, uint height, byte borderWidth, byte borderHeight)
        {
            Width = width;
            Height = height;
            BorderBlock = Enumerable.Repeat(new ushort[BorderHeight], BorderWidth).ToArray();
            MapBlock = Enumerable.Repeat(new ushort[Height], (int)Width).ToArray();
        }

        /// <summary>
        /// Creates a new empty MapFooter object
        /// </summary>
        public MapFooter()
        {

        }
        #endregion
    }
}
