using map2agblib.Imaging;
using map2agblib.Tilesets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agb
{
    /// <summary>
    /// Static class to compile tileset instances
    /// </summary>
    class TilesetCompiler
    {
        /// <summary>
        /// Method to retrieve an assembly string by an instanciated Tileset object
        /// </summary>
        /// <param name="t"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        public static string TilesetToString(Tileset tileset, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();
            
            b.Append("@ Tileset Assembly created by map2agb at ");
            b.Append(DateTime.Now.ToString());
            b.Append(Environment.NewLine);

            // Append the Tileset struct
            b.Append(MapTilesetToString(tileset, baseSymbol));
            b.Append(Environment.NewLine);

            return b.ToString();
        }

        /// <summary>
        /// Retrieves an assembly string by an Tileset
        /// </summary>
        /// <param name="t"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static string MapTilesetToString(Tileset tileset, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();

            b.Append("@ Section: Tileset Header"); b.Append(Environment.NewLine);
            b.Append(".align 4"); b.Append(Environment.NewLine);
            b.Append(".global "); b.Append(baseSymbol); b.Append(Environment.NewLine);
            b.Append(baseSymbol); b.Append(":"); b.Append(Environment.NewLine);

            // Add all members

            b.Append(".byte "); b.Append(Convert.ToInt32(tileset.Compressed).ToString()); b.Append(Environment.NewLine);
            b.Append(".byte "); b.Append(Convert.ToInt32(tileset.Secondary).ToString()); b.Append(Environment.NewLine);
            b.Append(".byte "); b.Append(tileset.Field2.ToString()); b.Append(Environment.NewLine);
            b.Append(".byte "); b.Append(tileset.Field2.ToString()); b.Append(Environment.NewLine);

            b.Append(".word "); b.Append(tileset.Graphic == null || tileset.Graphic.Length == 0 ? baseSymbol : tileset.Graphic); b.Append("Tiles"); b.Append(Environment.NewLine);
            b.Append(".word "); b.Append(baseSymbol); b.Append("_palettes"); b.Append(tileset.Secondary ? " - 0xE0" : ""); b.Append(Environment.NewLine);
            b.Append(".word "); b.Append(baseSymbol); b.Append("_blocks"); b.Append(Environment.NewLine);
            b.Append(".word "); b.Append(tileset.AnimationInitFunction); b.Append(Environment.NewLine);
            b.Append(".word "); b.Append(baseSymbol); b.Append("_block_behaviours"); b.Append(Environment.NewLine);
            b.Append(Environment.NewLine);

            b.Append(PalettesToString(tileset.Palettes, baseSymbol));
            b.Append(BlocksToString(tileset.Blocks, baseSymbol));
            b.Append(BehavioursToString(tileset.Blocks, baseSymbol));

            return b.ToString();
        }

        /// <summary>
        /// Retrieves an assembly string of a palette Array (transforming all palettes into uncompressed ushorts)
        /// </summary>
        /// <param name="tileset"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static string PalettesToString(Palette[] palettes, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();

            b.Append("@ Section: Tileset Palettes (6 x 16 Colors)"); b.Append(Environment.NewLine);
            b.Append(".align 4"); b.Append(Environment.NewLine);
            b.Append(".global "); b.Append(baseSymbol); b.Append("_palettes"); b.Append(Environment.NewLine);
            b.Append(baseSymbol); b.Append("_palettes:"); b.Append(Environment.NewLine);

            foreach(Palette palette in palettes)
            {
                // Append all palettes
                b.Append("@ // Palette start"); b.Append(Environment.NewLine);
                foreach(ShortColor color in palette.Colors)
                {
                    b.Append("\t.hword "); b.Append(color.ToUShort().ToString()); b.Append(Environment.NewLine);
                }
            }
            b.Append(Environment.NewLine);

            return b.ToString();
        }

        /// <summary>
        /// Retrieves an assembly string of the tilemap data of tileset blocks
        /// </summary>
        /// <param name="tilesetEntries"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static string BlocksToString(TilesetEntry[] tilesetEntries, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();

            b.Append("@ Section: TilesetBlocks"); b.Append(Environment.NewLine);
            b.Append(".align 4"); b.Append(Environment.NewLine);
            b.Append(".global "); b.Append(baseSymbol); b.Append("_blocks"); b.Append(Environment.NewLine);
            b.Append(baseSymbol); b.Append("_blocks:"); b.Append(Environment.NewLine);

            foreach(TilesetEntry tilesetEntry in tilesetEntries)
            {
                b.Append("\t.hword ");
                b.Append(String.Join(", ", tilesetEntry.TilemapEntry.Select(elem => elem.ToUShort().ToString())));
                b.Append(Environment.NewLine);
            }

            b.Append(Environment.NewLine);

            return b.ToString();
        }

        /// <summary>
        /// Retrieves an assembly string of the behaviour data of tileset blocks
        /// </summary>
        /// <param name="tilesetEntries"></param>
        /// <param name="baseSymbol"></param>
        /// <returns></returns>
        private static string BehavioursToString(TilesetEntry[] tilesetEntries, string baseSymbol)
        {
            StringBuilder b = new StringBuilder();

            b.Append("@ Section: TilesetBehaviours"); b.Append(Environment.NewLine);
            b.Append(".align 4"); b.Append(Environment.NewLine);
            b.Append(".global "); b.Append(baseSymbol); b.Append("_block_behaviours"); b.Append(Environment.NewLine);
            b.Append(baseSymbol); b.Append("_block_behaviours:"); b.Append(Environment.NewLine);

            foreach(TilesetEntry tilesetEntry in tilesetEntries)
            {
                b.Append("\t.word "); b.Append(tilesetEntry.Behaviour.ToUint32().ToString()); b.Append(Environment.NewLine);
            }

            return b.ToString();
        }
    }

    
}
