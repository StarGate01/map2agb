using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.Data;
using map2agblib.Map;
using map2agblib.Tilesets;

namespace map2agbgui
{
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
                    new LazyReference<MapHeader>(new MapHeader() { Name = 0, Footer = new MapFooter() { FirstTilesetID = "TSE0" } }),
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
                { "TSE0",  new LazyReference<Tileset>(new Tileset() { Graphic = @"C:\Users\Christoph\Desktop\Tileset0.bmp" }) },
                { "TSE245157", new LazyReference<Tileset>(new Tileset() { Graphic = @"C:\Users\Christoph\Desktop\Tileset245157.bmp" }) }
            };
            return romData;
        }

    }
}
