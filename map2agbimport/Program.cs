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
                        MapHeader header = null;
                        if (opt.ExportMap)
                        {
                            header = AgbImport.HeaderFromStream(br, mapTable, opt.BankNumber, opt.MapNumber);
                            
                            //TODO serialize
                        }
                        if (opt.ExportTileset)
                        {
                            header = header ?? AgbImport.HeaderFromStream(br, mapTable, opt.BankNumber, opt.MapNumber);
                            AgbImport.TilesetsFromStream(br, header);
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
                if (error)
                    return;
            }
            Console.ReadKey();
        }
    }
}
