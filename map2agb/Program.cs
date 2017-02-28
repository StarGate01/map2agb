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
                List<Tuple<MapHeader, string>> maps = (List<Tuple<MapHeader, string>>)inputFiles.Select(path => {
                    //Map to a tuple of MapHeader, FileName (is used as symbol name)
                    return new Tuple<MapHeader, string>(mapHeaderBuilder.ImportFromFile(path), Path.GetFileNameWithoutExtension(path));
                    }
                ).ToList();
                


                Console.ReadKey();
                return 0;
            }
            catch(Exception ex)
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
