using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Tilesets
{
    public class TilesetEntry
    {
        public BlockBehaviour Behaviour { get; set; }
        public BlockTileMap TilemapEntry { get; set; }

        public TilesetEntry(BlockBehaviour behaviour, BlockTileMap tilemap)
        {
            Behaviour = behaviour;
            TilemapEntry = tilemap;
        }

        public TilesetEntry()
        { }
    }
}
