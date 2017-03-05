using map2agblib.Common;
using map2agblib.Map;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using map2agblib.Map.Event;
using map2agblib.Map.LevelScript;

namespace map2agbimport
{
    class Program
    {

        static void Main(string[] args)
        {
            Options opt = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, opt))
            {
                long mapTable = opt.MapTable.ToLong(CultureInfo.InvariantCulture);
                long nameTable = opt.NameTable.ToLong(CultureInfo.InvariantCulture);
                bool error = false;
                FileStream fs = null;
                try
                {
                    fs = new FileStream(opt.InputFile, FileMode.Open, FileAccess.Read);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        fs = null;
                        /*TODO: Start Reading the object model */
                        if (opt.ExportMap || opt.ExportTileset)
                        {
                            if (mapTable == -1)
                            {
                                Console.Error.WriteLine("error: cannot import map without --maptable option specified");
                                return;
                            }
                            br.BaseStream.Seek(mapTable + (4 * opt.BankNumber), SeekOrigin.Begin);
                            br.BaseStream.Seek((br.ReadUInt32() & 0x1FFFFFF) + (4 * opt.MapNumber), SeekOrigin.Begin);
                            br.BaseStream.Seek(br.ReadUInt32() & 0x1FFFFFF, SeekOrigin.Begin);
                            uint mapFooterOffset = br.ReadUInt32();
                            uint eventOffset = br.ReadUInt32();
                            uint levelScriptOffset = br.ReadUInt32();
                            uint connectionOffset = br.ReadUInt32();

                            MapHeader header = new MapHeader()
                            {
                                Music = br.ReadUInt16(),
                                Index = br.ReadUInt16(),
                                Name = br.ReadByte(),
                                Flash = br.ReadByte(),
                                Weather = br.ReadByte(),
                                Type = br.ReadByte(),
                                Unknown = br.ReadUInt16(),
                                ShowName = br.ReadByte(),
                                BattleStyle = br.ReadByte()
                            };

                            br.BaseStream.Seek(mapFooterOffset & 0x1FFFFFF, SeekOrigin.Begin);
                            MapFooter footer = new MapFooter();
                            footer.Width = br.ReadUInt32();
                            footer.Height = br.ReadUInt32();
                            uint borderOffset = br.ReadUInt32();
                            uint mapDataOffset = br.ReadUInt32();
                            uint blocksetOneOffset = br.ReadUInt32();
                            uint blocksetTwoOffset = br.ReadUInt32();
                            byte borderWidth = br.ReadByte();
                            byte borderHeight = br.ReadByte();
                            ushort padding = br.ReadUInt16();

                            br.BaseStream.Seek(borderOffset & 0x1FFFFFF, SeekOrigin.Begin);

                            ushort[][] borderBlock = Enumerable.Repeat(new ushort[borderWidth], borderHeight).ToArray();
                            for (int y = 0; y < borderHeight; ++y)
                                for (int x = 0; x < borderWidth; ++x)
                                    borderBlock[y][x] = br.ReadUInt16();

                            ushort[][] mapBlock = Enumerable.Repeat(new ushort[footer.Width], (int)footer.Height).ToArray();
                            for (int y = 0; y < footer.Height; ++y)
                                for (int x = 0; x < footer.Width; ++x)
                                    mapBlock[y][x] = br.ReadUInt16();

                            footer.FirstTilesetID = "tsp_m_" + opt.BankNumber.ToString() + "_" + opt.MapNumber.ToString();
                            footer.SecondTilesetID = "tss_m_" + opt.BankNumber.ToString() + "_" + opt.MapNumber.ToString();
                            footer.BorderBlock = borderBlock;
                            footer.MapBlock = mapBlock;
                            footer.BorderWidth = borderWidth;
                            footer.BorderHeight = borderHeight;

                            header.Footer = footer;
                            EventHeader events = new EventHeader();
                            br.BaseStream.Seek(eventOffset & 0x1FFFFFF, SeekOrigin.Begin);
                            byte personCount = br.ReadByte();
                            byte warpCount = br.ReadByte();
                            byte scriptCount = br.ReadByte();
                            byte signCount = br.ReadByte();

                            uint personOffset = br.ReadUInt32();
                            uint warpOffset = br.ReadUInt32();
                            uint scriptOffset = br.ReadUInt32();
                            uint signOffset = br.ReadUInt32();


                            br.BaseStream.Seek(personOffset & 0x1FFFFFF, SeekOrigin.Begin);
                            for (int i = 0; i < personCount; ++i)
                            {
                                events.Persons.Add(new EventEntityPerson()
                                {
                                    Id = br.ReadByte(),
                                    Picture = br.ReadByte(),
                                    Field2 = br.ReadByte(),
                                    Field3 = br.ReadByte(),
                                    X = br.ReadInt16(),
                                    Y = br.ReadInt16(),
                                    Height = br.ReadByte(),
                                    Behaviour = br.ReadByte(),
                                    Movement = br.ReadByte(),
                                    FieldB = br.ReadByte(),
                                    IsTrainer = br.ReadByte(),
                                    FieldD = br.ReadByte(),
                                    AlertRadius = br.ReadUInt16(),
                                    Script = "npc_m_" + opt.BankNumber.ToString() + "_" + opt.MapNumber.ToString() + "_" + i.ToString(),
                                    InternalScript = br.ReadUInt32(),
                                    Flag = br.ReadUInt16(),
                                    Padding = br.ReadUInt16()
                                });
                            }

                            br.BaseStream.Seek(warpOffset & 0x1FFFFFF, SeekOrigin.Begin);
                            for (int i = 0; i < warpCount; ++i)
                            {
                                events.Warps.Add(new EventEntityWarp()
                                {
                                    X = br.ReadInt16(),
                                    Y = br.ReadInt16(),
                                    Height = br.ReadByte(),
                                    TargetWarp = br.ReadByte(),
                                    TargetMap = br.ReadByte(),
                                    TargetBank = br.ReadByte()
                                });
                            }

                            br.BaseStream.Seek(signOffset & 0x1FFFFFF, SeekOrigin.Begin);
                            for (int i = 0; i < signCount; ++i)
                            {
                                events.Signs.Add(new EventEntitySign()
                                {
                                    X = br.ReadInt16(),
                                    Y = br.ReadInt16(),
                                    Height = br.ReadByte(),
                                    Type = br.ReadByte(),
                                    Unknown = br.ReadUInt16()
                                });
                                if (events.Signs[i].Layout == EventEntitySign.SignType.Item)
                                {
                                    events.Signs[i].ItemId = br.ReadUInt16();
                                    events.Signs[i].HiddenId = br.ReadByte();
                                    byte bField = br.ReadByte();
                                    events.Signs[i].ItemCount = (byte)(bField & 0x3F);
                                    events.Signs[i].IsCoin = (bField & 0x40) > 0;
                                    events.Signs[i].DetectorDisabled = (bField & 0x80) > 0;
                                }
                                else
                                {
                                    /* ITEM ID ??? */
                                    events.Signs[i].Script = "sgn_m_" + opt.BankNumber.ToString() + "_" + opt.MapNumber.ToString();
                                    events.Signs[i].InternalScript = br.ReadUInt32();
                                }
                            }

                            br.BaseStream.Seek(scriptOffset & 0x1FFFFFF, SeekOrigin.Begin);
                            for (int i = 0; i < scriptCount; ++i)
                            {
                                /* TODO: Check */
                                events.ScriptTriggers.Add(new EventEntityTrigger()
                                {
                                    X = br.ReadInt16(),
                                    Y = br.ReadInt16(),
                                    Height = br.ReadByte(),
                                    Field5 = br.ReadByte(),
                                    Variable = br.ReadUInt16(),
                                    Value = br.ReadUInt16(),
                                    FieldA = br.ReadByte(),
                                    FieldB = br.ReadByte(),
                                    InternalScript = br.ReadUInt32(),
                                    Script = "scr_m_" + opt.BankNumber.ToString() + "_" + opt.MapNumber.ToString()
                                });
                            }


                            header.Events = events;

                            ConnectionHeader connections = new ConnectionHeader();
                            br.BaseStream.Seek(connectionOffset & 0x1FFFFFF, SeekOrigin.Begin);

                            uint connectionCount = br.ReadUInt32();
                            br.BaseStream.Seek(br.ReadUInt32() & 0x1FFFFFF, SeekOrigin.Begin);

                            for (int i = 0; i < connectionCount; ++i)
                            {
                                connections.Connections.Add(new Connection()
                                {
                                    Direction = (Connection.ConnectionDirection)br.ReadUInt32(),
                                    Displacement = br.ReadInt32(),
                                    Bank = br.ReadByte(),
                                    Map = br.ReadByte(),
                                    FieldA = br.ReadByte(),
                                    FieldB = br.ReadByte()
                                });
                            }
                            header.Connections = connections;

                            MapScriptHeader scriptHeader = new MapScriptHeader();
                            br.BaseStream.Seek(levelScriptOffset & 0x1FFFFFF, SeekOrigin.Begin);
                            byte scrType;
                            while ((scrType = br.ReadByte()) != 0)
                            {
                                MapScript script = new MapScript((MapScript.MapScriptTypes)scrType);
                                switch (script.Layout)
                                {
                                    case MapScript.MapScriptLayout.Script:

                                        br.ReadUInt32();
                                        break;
                                    case MapScript.MapScriptLayout.ExtendedScript:
                                        long pos = br.BaseStream.Position + 4;
                                        br.BaseStream.Seek(br.ReadUInt32() & 0x1FFFFFF, SeekOrigin.Begin);
                                        script.Variable = br.ReadUInt16();
                                        script.Value = br.ReadUInt16();
                                        br.BaseStream.Seek(pos, SeekOrigin.Begin);
                                        break;
                                    case MapScript.MapScriptLayout.None:
                                        throw new Exception("invalid code reached");
                                }
                                script.Script = "lscr_m_" + opt.BankNumber.ToString() + "_" + opt.MapNumber.ToString();
                                scriptHeader.MapScripts.Add(script);
                            }
                            header.MapScripts = scriptHeader;

                            if (opt.ExportMap)
                            {
                                header.ExportToFile(header, "test.hxml");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("error: {0}", ex.Message);
                    error = true;
                }
                finally
                {
                    if (fs != null)
                        fs.Dispose();
                }
                if (error)
                    return;

                /* do stuff */
            }
            Console.ReadKey();
            /* don't do stuff */
        }
    }
}
