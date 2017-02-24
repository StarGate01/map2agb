namespace map2agblib.Tilesets
{
    public class TilesetEntry
    {
        public BlockBehaviour Behaviour { get; set; }
        public BlockTilemap TilemapEntry { get; set; }

        public TilesetEntry(BlockBehaviour behaviour, BlockTilemap tilemap)
        {
            Behaviour = behaviour;
            TilemapEntry = tilemap;
        }

        public TilesetEntry()
        { }
    }
}
