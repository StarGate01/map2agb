using System;
using System.Linq;
using System.Globalization;
using map2agblib.Common;
using System.IO;
using map2agblib.Map;

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
                            header.Events = null;
                            header.Connections = null;
                            header.MapScripts = null;
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
