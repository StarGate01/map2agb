using CommandLine;
using CommandLine.Text;

namespace map2agbimport
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "The rom file to read the map data from")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "The directory to save the project to")]
        public string OutputFolder { get; set; }

        [Option("maptable", Required = false, DefaultValue = "-1", HelpText = "Explicit Map Table offset")]
        public string MapTable { get; set; }

        [Option("nametable", Required = false, DefaultValue = "-1", HelpText = "Explicit name table offset")]
        public string NameTable { get; set; }

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
