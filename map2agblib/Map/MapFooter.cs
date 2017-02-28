using map2agblib.Data;
using map2agblib.Tilesets;
using System.Linq;
using System.Runtime.Serialization;

namespace map2agblib.Map
{
    /// <summary>
    /// Represents a MapFooter, an object containing information about the actual MapBlock and used Tilesets
    /// </summary>
    [DataContract]
    public class MapFooter
    {
        #region Properties

        //TODO: Explicit setter to hold consistancy with BorderBlock and MapBlock Arrays
        /// <summary>
        /// The Width of the Map in Tiles (16x16)
        /// </summary>
        [DataMember]
        public uint Width { get; set; }

        /// <summary>
        /// The Height of the Map in Tiles (16x16)
        /// </summary>
        [DataMember]
        public uint Height { get; set; }

        //TODO: Create Block objects
        //TODO: Create explicit setter to hold consitancy with Width and Height or find a better solution
        /// <summary>
        /// The BorderBlock which is displayed when the camera hits the border
        /// </summary>
        [DataMember]
        public ushort[][] BorderBlock { get; set; }

        /// <summary>
        /// The actual MapBlock, contains Graphical tile data as well as the collision Map
        /// </summary>
        [DataMember]
        public ushort[][] MapBlock { get; set; }

        /// <summary>
        /// The Main Tileset for this Map
        /// </summary>
        [DataMember]
        public string FirstTilesetID { get; set; }

        /// <summary>
        /// The Secondary Tileset for this Map
        /// </summary>
        [DataMember]
        public string SecondTilesetID { get; set; }

        //TODO: Create resize methode to resize MapBlock and BorderBlock

        /// <summary>
        /// The Width of the BorderBlock
        /// </summary>
        [DataMember]
        public byte BorderWidth { get; set; }

        /// <summary>
        /// The Height of the BorderBlock
        /// </summary>
        [DataMember]
        public byte BorderHeight { get; set; }

        /// <summary>
        /// Bytes used to assure alignment, probably unused otherwise
        /// </summary>
        [DataMember]
        public ushort Padding { get; set; }
        #endregion

        #region Constructor

        public MapFooter()
        {
            BorderBlock = new ushort[0][];
            MapBlock = new ushort[0][];
        }

        #endregion
    }
}
