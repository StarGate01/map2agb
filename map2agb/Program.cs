using map2agblib.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib.Data;

namespace map2agb
{

    class Program
    {
       
        /// <summary>
        /// Parsing method for map input
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static int ParseMap(Options opts)
        {
            String outPath = opts.OutputPath;
            List<string> inputFiles = opts.InputFiles;
            MapHeader mapHeaderBuilder = new MapHeader();

            try {

                //Append all maps into this single StringBuilder's String
                StringBuilder b = new StringBuilder();
                foreach (String inputFile in inputFiles)
                {
                    MapHeader mapHeader;
                    // First try to import
                    try {
                        mapHeader = mapHeaderBuilder.ImportFromFile(inputFile);
                    }
                    catch (IOException ioex)
                    {
                        Console.Error.WriteLine("IOError for file '" + inputFile + "'");
                        throw ioex;
                    }catch(Exception ex)
                    {
                        Console.Error.WriteLine("Error at parsing map input file '" + inputFile + "'");
                        throw ex;
                    }
                    
                    try
                    {
                        // Try to convert
                        String baseSymbol = Path.GetFileNameWithoutExtension(inputFile);
                        b.Append(MapCompiler.MapToString(mapHeader, baseSymbol));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error at converting map '" + inputFile + "':");
                        throw ex;
                    }
                    
                }
                try
                {
                    // Export all files
                    File.WriteAllText(outPath, b.ToString());

                }catch(Exception ex)
                {
                    Console.Error.WriteLine("Error at exporting file '" + outPath + "':");
                    throw ex;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }
        }

        /// <summary>
        /// Parsing method for tileset input
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static int ParseTileset(Options opts)
        {
            return 0;
        }

        /// <summary>
        /// Parsing method for meta input
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public static int ParseMeta(Options opts)
        {
            return 0;
        }


        
        static int Main(string[] args)
        {
            var opts = new Options();
            if(CommandLine.Parser.Default.ParseArguments(args, opts))
            {

                switch (opts.Mode.ToLower())
                {
                    case "map":
                        //Compile a map
                        return ParseMap(opts);
                    case "tileset":
                    case "tiles":
                        //Compile a tileset
                        return ParseTileset(opts);
                    case "meta":
                        //Parse a metadata table
                        return ParseMeta(opts);
                    default:
                        Console.Error.WriteLine("Unkown mode "+opts.Mode);
                        return 1;
                }
            }
            return 1;
        }

    }

}
