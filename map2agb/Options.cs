﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace map2agb
{
    /// <summary>
    /// Class to parse command line options
    /// </summary>
    class Options
    {
        [Option('m', "mode", Required = true,
            HelpText = "Compiling mode: map compiles .map files, tileset compiles .ts files and meta compiles .meta files"
            )]
        public string Mode { get; set; }

        [Option('o', "output", Required = true,
            HelpText = "Specifies the output file"
            )]
        public string OutputPath { get; set; }

        [ValueList(typeof(List<string>))]
        public List<string> InputFiles { get; set; }

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
