using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.Data;
using map2agblib.Map;
using map2agblib.Tilesets;
using map2agblib.Imaging;
using map2agblib.Imaging.IO;

namespace map2agbgui.Models
{

#if DEBUG

    public class MockData
    {

        public static RomData MockRomData()
        {
            RomData romData = new RomData();
            romData.NameTable[0] = "TESTMAP";
            romData.NameTable[1] = "JOJOJO";
            romData.NameTable[2] = "ALLESKLAR";
            romData.NameTable[5] = "OKGUT";
            romData.Banks = new List<List<LazyReference<MapHeader>>>()
            {
                new List<LazyReference<MapHeader>>()
                {
                    new LazyReference<MapHeader>(new MapHeader() { Name = 0, Footer = new MapFooter() { FirstTilesetID = "TSE0", SecondTilesetID = "TSE245157" } }),
                    new LazyReference<MapHeader>(new MapHeader() { Name = 2 }),
                    null,
                    new LazyReference<MapHeader>(new MapHeader() { Name = 1 }),
                },
                new List<LazyReference<MapHeader>>()
                {
                    new LazyReference<MapHeader>(new MapHeader() { Name = 5 }),
                    new LazyReference<MapHeader>(new MapHeader() { Name = 2 }),
                    new LazyReference<MapHeader>(new MapHeader() { Name = 0 }),
                    null
                },
                null
            };
            romData.Tilesets = new Dictionary<string, LazyReference<Tileset>>
            {
                { "TSE0",  new LazyReference<Tileset>(new Tileset() {
                    Graphic = @"C:\Users\Christoph\Desktop\Tileset12_I.bmp",
                    Secondary = false }) },
                { "TSE1",  new LazyReference<Tileset>(new Tileset() {
                    Secondary = false }) },
                { "TSE245157", new LazyReference<Tileset>(new Tileset() {
                    Graphic = @"C:\Users\Christoph\Desktop\Tileset245157_I.bmp",
                    Secondary = true }) }
            };
            try
            {
                romData.Tilesets["TSE0"].Data.Palettes[0] = JASCPAL.Import(@"C:\Users\Christoph\Desktop\Tileset12_I.pal");
                romData.Tilesets["TSE0"].Data.Blocks = new TilesetEntry[] { new TilesetEntry() };

                romData.Tilesets["TSE0"].Data.Blocks[0].TilemapEntry[0] = new BlockTilemap() { TileId = 5 };
                romData.Tilesets["TSE0"].Data.Blocks[0].TilemapEntry[1] = new BlockTilemap() { TileId = 6 };
                romData.Tilesets["TSE0"].Data.Blocks[0].TilemapEntry[2] = new BlockTilemap() { TileId = 21 };
                romData.Tilesets["TSE0"].Data.Blocks[0].TilemapEntry[3] = new BlockTilemap() { TileId = 22 };
                romData.Tilesets["TSE0"].Data.Blocks[0].TilemapEntry[4] = new BlockTilemap() { TileId = 98 };
                romData.Tilesets["TSE0"].Data.Blocks[0].TilemapEntry[5] = new BlockTilemap() { TileId = 99 };
                romData.Tilesets["TSE0"].Data.Blocks[0].TilemapEntry[6] = new BlockTilemap() { TileId = 114 };
                romData.Tilesets["TSE0"].Data.Blocks[0].TilemapEntry[7] = new BlockTilemap() { TileId = 115 };

                romData.Tilesets["TSE0"].Data.Palettes[1] = JASCPAL.Import(@"C:\Users\Christoph\Desktop\deko-iv.pal");
                romData.Tilesets["TSE1"].Data.Palettes[0] = JASCPAL.Import(@"C:\Users\Christoph\Desktop\deko-iv.pal");
                romData.Tilesets["TSE245157"].Data.Palettes[0] = JASCPAL.Import(@"C:\Users\Christoph\Desktop\deko-iv.pal");
            }
            catch (Exception) { }
          
            return romData;
        }

    }

#endif

}
