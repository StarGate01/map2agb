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

        [DataMember]
        public BlockTilemap TilemapEntry { get; set; }

        #endregion

        #region Construcotrs

        public TilesetEntry(BlockBehaviour behaviour, BlockTilemap tilemap)
        {
            Behaviour = behaviour;
            TilemapEntry = tilemap;
        }

        public TilesetEntry()
        { }

        #endregion

    }

}
