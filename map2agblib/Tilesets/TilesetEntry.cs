using System;
using System.Linq;
using map2agblib.Imaging;
using System.Runtime.Serialization;

namespace map2agblib.Tilesets
{

    [DataContract]
    public class TilesetEntry
    {

        #region Properties

        [DataMember]
        public BlockBehaviour Behaviour { get; set; }

        /// <summary>
        /// The Tilemap entries
        /// Order: bottom-layer (left-top, right-top, left-bottom, right-bottom) and the same for the top layer
        /// </summary>
        [DataMember]
        public BlockTilemap[] TilemapEntry { get; set; }

        #endregion

        #region Construcotrs

        public TilesetEntry(BlockBehaviour behaviour, BlockTilemap[] tilemap)
        {
            Behaviour = behaviour;
            TilemapEntry = tilemap;
        }

        public TilesetEntry()
        {
            TilemapEntry = Enumerable.Repeat(new BlockTilemap(), 8).ToArray();
        }

        #endregion

    }

}
