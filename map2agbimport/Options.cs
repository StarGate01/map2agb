using CommandLine;
using CommandLine.Text;

namespace map2agbimport
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "The rom file to read the map data from")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "The directory to save the object(s) to")]
        public string OutputFolder { get; set; }

        [Option("maptable", Required = false, DefaultValue = "-1", HelpText = "Explicit Map Table offset")]
        public string MapTable { get; set; }

        [Option("nametable", Required = false, DefaultValue = "-1", HelpText = "Explicit name table offset")]
        public string NameTable { get; set; }

        [Option('t', "tileset", DefaultValue = false, HelpText = "If set, the tilesets of the map will be exported")]
        public bool ExportTileset { get; set; }

        [Option('m', "map", DefaultValue = false, HelpText = "If set, the map data will be exported")]
        public bool ExportMap { get; set; }

        [Option('n', "number", Required = true, HelpText = "The map number which should be exported")]
        public byte MapNumber { get; set; }

        [Option('b', "bank", Required = true, HelpText = "The bank number of the map to export")]
        public byte BankNumber { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
