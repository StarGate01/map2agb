using map2agbimport.Compression;
using map2agbimport.Graphics;
using map2agblib.Imaging;
using map2agblib.Map;
using map2agblib.Map.Event;
using map2agblib.Map.LevelScript;
using map2agblib.Tilesets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbimport
{
    public static class AgbImport
    {

        public const string TS_PRIM_SUF = "tsp";
        public const string TS_SEC_SUF = "tss";

        public const string NPC_D = "npc";
        public const string SIGN_D = "sgn";
        public const string SCRIPT_D = "src";
        public const string LSCRIPT_D = "lscr";

        /// <summary>
        /// Reads an EventHeader from a ROM
        /// </summary>
        /// <param name="reader">BinaryReader object to read from</param>
        /// <param name="eventOffset">Offset of Event Data in ROM</param>
        /// <param name="prefix">Prefix to put in front of entity object symbols</param>
        /// <returns>EventHeader object representing the event data of the ROM</returns>
        private static EventHeader EventsFromReader(BinaryReader reader, uint eventOffset, string prefix)
        {
            EventHeader events = new EventHeader();
            reader.BaseStream.Seek(eventOffset & 0x1FFFFFF, SeekOrigin.Begin);
            byte personCount = reader.ReadByte();
            byte warpCount = reader.ReadByte();
            byte scriptCount = reader.ReadByte();
            byte signCount = reader.ReadByte();

            uint personOffset = reader.ReadUInt32();
            uint warpOffset = reader.ReadUInt32();
            uint scriptOffset = reader.ReadUInt32();
            uint signOffset = reader.ReadUInt32();


            reader.BaseStream.Seek(personOffset & 0x1FFFFFF, SeekOrigin.Begin);
            for (int i = 0; i < personCount; ++i)
            {
                events.Persons.Add(new EventEntityPerson()
                {
                    Id = reader.ReadByte(),
                    Picture = reader.ReadByte(),
                    Field2 = reader.ReadByte(),
                    Field3 = reader.ReadByte(),
                    X = reader.ReadInt16(),
                    Y = reader.ReadInt16(),
                    Height = reader.ReadByte(),
                    Behaviour = reader.ReadByte(),
                    Movement = reader.ReadByte(),
                    FieldB = reader.ReadByte(),
                    IsTrainer = reader.ReadByte(),
                    FieldD = reader.ReadByte(),
                    AlertRadius = reader.ReadUInt16(),
                    InternalScript = reader.ReadUInt32(),
                    Flag = reader.ReadUInt16(),
                    Padding = reader.ReadUInt16()
                });
                events.Persons[i].Script = "0x" + events.Persons[i].InternalScript.ToString("X8");
            }

            reader.BaseStream.Seek(warpOffset & 0x1FFFFFF, SeekOrigin.Begin);
            for (int i = 0; i < warpCount; ++i)
            {
                events.Warps.Add(new EventEntityWarp()
                {
                    X = reader.ReadInt16(),
                    Y = reader.ReadInt16(),
                    Height = reader.ReadByte(),
                    TargetWarp = reader.ReadByte(),
                    TargetMap = reader.ReadByte(),
                    TargetBank = reader.ReadByte()
                });
            }

            reader.BaseStream.Seek(signOffset & 0x1FFFFFF, SeekOrigin.Begin);
            for (int i = 0; i < signCount; ++i)
            {
                events.Signs.Add(new EventEntitySign()
                {
                    X = reader.ReadInt16(),
                    Y = reader.ReadInt16(),
                    Height = reader.ReadByte(),
                    Type = reader.ReadByte(),
                    Unknown = reader.ReadUInt16()
                });
                if (events.Signs[i].Layout == EventEntitySign.SignType.Item)
                {
                    events.Signs[i].ItemId = reader.ReadUInt16();
                    events.Signs[i].HiddenId = reader.ReadByte();
                    byte bField = reader.ReadByte();
                    events.Signs[i].ItemCount = (byte)(bField & 0x3F);
                    events.Signs[i].IsCoin = (bField & 0x40) > 0;
                    events.Signs[i].DetectorDisabled = (bField & 0x80) > 0;
                }
                else
                {
                    /* ITEM ID ??? */
                    events.Signs[i].InternalScript = reader.ReadUInt32();
                    events.Signs[i].Script = "0x" + events.Signs[i].InternalScript.ToString("X8");
                }
            }

            reader.BaseStream.Seek(scriptOffset & 0x1FFFFFF, SeekOrigin.Begin);
            for (int i = 0; i < scriptCount; ++i)
            {
                /* TODO: Check */
                events.ScriptTriggers.Add(new EventEntityTrigger()
                {
                    X = reader.ReadInt16(),
                    Y = reader.ReadInt16(),
                    Height = reader.ReadByte(),
                    Field5 = reader.ReadByte(),
                    Variable = reader.ReadUInt16(),
                    Value = reader.ReadUInt16(),
                    FieldA = reader.ReadByte(),
                    FieldB = reader.ReadByte(),
                    InternalScript = reader.ReadUInt32(),
                });
                events.ScriptTriggers[i].Script = "0x" + events.ScriptTriggers[i].InternalScript.ToString("X8");
            }
            return events;
        }

        /// <summary>
        /// Reads a MapFooter from a ROM
        /// </summary>
        /// <param name="reader">BinaryReader object to read from</param>
        /// <param name="footerOffset">Offset of Footer Data in ROM</param>
        /// <param name="prefix">Prefix to put in front of tileset symbols</param>
        /// <returns>MapFooter object representing a map footer of the ROM</returns>
        private static MapFooter FooterFromReader(BinaryReader reader, uint footerOffset, string prefix)
        {
            reader.BaseStream.Seek(footerOffset & 0x1FFFFFF, SeekOrigin.Begin);
            MapFooter footer = new MapFooter();
            footer.Width = reader.ReadUInt32();
            footer.Height = reader.ReadUInt32();
            uint borderOffset = reader.ReadUInt32();
            uint mapDataOffset = reader.ReadUInt32();
            uint blocksetOneOffset = reader.ReadUInt32();
            uint blocksetTwoOffset = reader.ReadUInt32();
            byte borderWidth = reader.ReadByte();
            byte borderHeight = reader.ReadByte();
            ushort padding = reader.ReadUInt16();

            reader.BaseStream.Seek(borderOffset & 0x1FFFFFF, SeekOrigin.Begin);

            ushort[][] borderBlock = Enumerable.Repeat(new ushort[borderWidth], borderHeight).ToArray();
            for (int y = 0; y < borderHeight; ++y)
                for (int x = 0; x < borderWidth; ++x)
                    borderBlock[y][x] = reader.ReadUInt16();

            ushort[][] mapBlock = Enumerable.Repeat(new ushort[footer.Width], (int)footer.Height).ToArray();
            for (int y = 0; y < footer.Height; ++y)
                for (int x = 0; x < footer.Width; ++x)
                    mapBlock[y][x] = reader.ReadUInt16();

            footer.FirstTilesetID = prefix + TS_PRIM_SUF;
            footer.SecondTilesetID = prefix + TS_SEC_SUF;
            footer.BorderBlock = borderBlock;
            footer.MapBlock = mapBlock;
            footer.BorderWidth = borderWidth;
            footer.BorderHeight = borderHeight;
            footer.FirstTilesetInternal = blocksetOneOffset;
            footer.SecondTilesetInternal = blocksetTwoOffset;

            return footer;
        }

        /// <summary>
        /// Reads Connection Data from a ROM
        /// </summary>
        /// <param name="reader">BinaryReader object to read from</param>
        /// <param name="connectionOffset">Offset of connection data in ROM</param>
        /// <returns>ConnectionHeader object containing all connections of a Map</returns>
        private static ConnectionHeader ConnectionsFromReader(BinaryReader reader, uint connectionOffset)
        {
            ConnectionHeader connections = new ConnectionHeader();
            reader.BaseStream.Seek(connectionOffset & 0x1FFFFFF, SeekOrigin.Begin);

            uint connectionCount = reader.ReadUInt32();
            reader.BaseStream.Seek(reader.ReadUInt32() & 0x1FFFFFF, SeekOrigin.Begin);

            for (int i = 0; i < connectionCount; ++i)
            {
                connections.Connections.Add(new Connection()
                {
                    Direction = (Connection.ConnectionDirection)reader.ReadUInt32(),
                    Displacement = reader.ReadInt32(),
                    Bank = reader.ReadByte(),
                    Map = reader.ReadByte(),
                    FieldA = reader.ReadByte(),
                    FieldB = reader.ReadByte()
                });
            }
            return connections;
        }

        /// <summary>
        /// Reads LevelScript Data from a ROM
        /// </summary>
        /// <param name="reader">BinaryReader object to read from</param>
        /// <param name="levelScriptOffset">Offset of levelscript data in ROM</param>
        /// <param name="prefix">Prefix to put in front of levelscript symbols</param>
        /// <returns>MapScriptHeader object containing the levelscripts of the Map</returns>
        private static MapScriptHeader LevelScriptsFromReader(BinaryReader reader, uint levelScriptOffset, string prefix)
        {
            MapScriptHeader scriptHeader = new MapScriptHeader();
            reader.BaseStream.Seek(levelScriptOffset & 0x1FFFFFF, SeekOrigin.Begin);
            byte scrType;
            int i = 0;
            while ((scrType = reader.ReadByte()) != 0)
            {
                MapScript script = new MapScript((MapScript.MapScriptTypes)scrType);
                uint sIntern = 0;
                switch (script.Layout)
                {
                    case MapScript.MapScriptLayout.Script:

                        //TODO: Save internal stuff
                        sIntern = reader.ReadUInt32();
                        break;
                    case MapScript.MapScriptLayout.ExtendedScript:
                        long pos = reader.BaseStream.Position + 4;
                        reader.BaseStream.Seek(reader.ReadUInt32() & 0x1FFFFFF, SeekOrigin.Begin);
                        script.Variable = reader.ReadUInt16();
                        script.Value = reader.ReadUInt16();
                        sIntern = reader.ReadUInt32();
                        //TODO: Save internal stuff
                        reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                        break;
                    case MapScript.MapScriptLayout.None:
                        throw new Exception("invalid code reached");
                }
                script.Script = "0x" + sIntern.ToString("X8");
                scriptHeader.MapScripts.Add(script);
                i++;
            }
            return scriptHeader;
        }

        /// <summary>
        /// Reads a MapHeader from a ROM
        /// </summary>
        /// <param name="reader">BinaryReader to read data from</param>
        /// <param name="mapTable">Offset of the MapTable in the ROM (MapBankTable)</param>
        /// <param name="bankNumber">Bank number to read</param>
        /// <param name="mapNumber">Map number to read</param>
        /// <returns>MapHeader imported from the stream object</returns>
        public static MapHeader HeaderFromStream(BinaryReader reader, long mapTable, byte bankNumber, byte mapNumber)
        {
            MapHeader header;
            reader.BaseStream.Seek(mapTable + (4 * bankNumber), SeekOrigin.Begin);
            reader.BaseStream.Seek((reader.ReadUInt32() & 0x1FFFFFF) + (4 * mapNumber), SeekOrigin.Begin);
            reader.BaseStream.Seek(reader.ReadUInt32() & 0x1FFFFFF, SeekOrigin.Begin);
            uint mapFooterOffset = reader.ReadUInt32();
            uint eventOffset = reader.ReadUInt32();
            uint levelScriptOffset = reader.ReadUInt32();
            uint connectionOffset = reader.ReadUInt32();

            header = new MapHeader()
            {
                Music = reader.ReadUInt16(),
                Index = reader.ReadUInt16(),
                Name = reader.ReadByte(),
                Flash = reader.ReadByte(),
                Weather = reader.ReadByte(),
                Type = reader.ReadByte(),
                Unknown = reader.ReadUInt16(),
                ShowName = reader.ReadByte(),
                BattleStyle = reader.ReadByte()
            };
            string mapBasePrefix = "m_" + bankNumber.ToString() + "_" + mapNumber.ToString() + "_";
            header.Footer = FooterFromReader(reader, mapFooterOffset, mapBasePrefix);
            header.Events = EventsFromReader(reader, eventOffset, mapBasePrefix);
            header.Connections = ConnectionsFromReader(reader, connectionOffset);
            header.MapScripts = LevelScriptsFromReader(reader, levelScriptOffset, mapBasePrefix);
            return header;
        }

        private static Tileset TilesetFromStream(BinaryReader reader, uint offset, string tilesetDirectory)
        {
            if (!Directory.Exists(tilesetDirectory))
                throw new ArgumentException(string.Format("Directory {0} does not exists", tilesetDirectory), tilesetDirectory);
            
            reader.BaseStream.Seek(offset, SeekOrigin.Begin);
            Tileset output = new Tileset(reader.ReadBoolean(), reader.ReadBoolean());
            output.Field2 = reader.ReadByte();
            output.Field3 = reader.ReadByte();

            uint imageOffset = reader.ReadUInt32() & 0x1FFFFFF;
            uint palOffset = reader.ReadUInt32() & 0x1FFFFFF;
            uint blockOffset = reader.ReadUInt32() & 0x1FFFFFF;
            uint animationOffset = reader.ReadUInt32();
            uint behaviorOffset = reader.ReadUInt32() & 0x1FFFFFF;

            /* NOTE: Check on second tileset palette */
            if (output.Secondary)
                palOffset = palOffset + 0xE0;
            reader.BaseStream.Seek(palOffset, SeekOrigin.Begin);
            ShortColor[][] palette = new ShortColor[6][];
            for (int i = 0; i < 6; ++i)
            {
                palette[i] = new ShortColor[16];
                for (int j = 0; j < 16; ++j)
                {
                    palette[i][j] = new ShortColor(reader.ReadUInt16());
                    
                }
                output.Palettes[i] = new Palette(palette[i]);
            }
            reader.BaseStream.Seek(imageOffset, SeekOrigin.Begin);
            byte[] tileset = reader.ReadLzArray().ToArray();
            TileCollection set = new TileCollection(tileset);
            set.ToImage(16, palette[1]).Save(tilesetDirectory + "/" + offset.ToString("X7") + "_tileset_" +
                (output.Secondary ? "secondary" : "primary") + ".png");

            reader.BaseStream.Seek(blockOffset, SeekOrigin.Begin);
            int maxBlocks = output.Secondary ? Tileset.MAX_SECOND_TILESET_SIZE : Tileset.MAX_FIRST_TILESET_SIZE;
            for (int i = 0; i < maxBlocks; ++i)
            {
                output.Blocks[i] = new TilesetEntry(null,
                    new BlockTilemap[] { new BlockTilemap(reader.ReadUInt16()), new BlockTilemap(reader.ReadUInt16()),
                                         new BlockTilemap(reader.ReadUInt16()), new BlockTilemap(reader.ReadUInt16()),

                                         new BlockTilemap(reader.ReadUInt16()), new BlockTilemap(reader.ReadUInt16()),
                                         new BlockTilemap(reader.ReadUInt16()), new BlockTilemap(reader.ReadUInt16()) });
            }
            output.AnimationInitFunctionInternal = animationOffset;

            reader.BaseStream.Seek(behaviorOffset, SeekOrigin.Begin);
            for (int i = 0; i < maxBlocks; ++i)
            {
                output.Blocks[i].Behaviour = new BlockBehaviour(reader.ReadUInt32());
            }

            return output;
        }

        public static Tuple<Tileset, Tileset> TilesetsFromStream(BinaryReader reader, MapHeader header, string tilesetDirectory)
        {
            return new Tuple<Tileset, Tileset>(TilesetFromStream(reader, header.Footer.FirstTilesetInternal & 0x1FFFFFF, tilesetDirectory), TilesetFromStream(reader, header.Footer.SecondTilesetInternal & 0x1FFFFFF, tilesetDirectory));
        }
    }
}
