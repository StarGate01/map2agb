using map2agblib.Common;
using map2agblib.Map;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using map2agblib.Map.Event;
using map2agblib.Map.LevelScript;
using map2agblib.Tilesets;

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
                    if (!Directory.Exists(opt.OutputFolder))
                        Directory.CreateDirectory(opt.OutputFolder);
                    fs = new FileStream(opt.InputFile, FileMode.Open, FileAccess.Read);
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        fs = null;
                        MapHeader header = null;
                        if (opt.ExportMap)
                        {
                            header = AgbImport.HeaderFromStream(br, mapTable, opt.BankNumber, opt.MapNumber);
                            header.ExportToFile(header, opt.OutputFolder + "/" + opt.BankNumber.ToString("D2") + "_" + opt.MapNumber.ToString("D2") + ".mapheader");
                            //TODO serialize
                        }
                        if (opt.ExportTileset)
                        {
                            header = header ?? AgbImport.HeaderFromStream(br, mapTable, opt.BankNumber, opt.MapNumber);
                            if (!Directory.Exists(opt.OutputFolder + "/tileset"))
                                Directory.CreateDirectory(opt.OutputFolder + "/tileset");
                            Tuple<Tileset, Tileset> sets = AgbImport.TilesetsFromStream(br, header, opt.OutputFolder + "/tileset");
                            sets.Item1.ExportToFile(sets.Item1, opt.OutputFolder + "/tileset/tileset_" + header.Footer.FirstTilesetInternal.ToString("X8") + ".tileset");
                            sets.Item2.ExportToFile(sets.Item2, opt.OutputFolder + "/tileset/tileset_" + header.Footer.SecondTilesetInternal.ToString("X8") + ".tileset");
                            //TODO import tileset
                            //TODO serialize
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
            }
            //Console.ReadKey();
        }
    }
}
