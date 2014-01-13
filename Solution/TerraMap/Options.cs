using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap
{
	public class Options
	{
		[Option('i', "input", Required = true, HelpText = "Input wld file to be processed.")]
		public string InputFile { get; set; }

		[Option('o', "output", Required = true, HelpText = "Output png file to be created/overwritten.")]
		public string OutputFile { get; set; }

		[Option('n', "nogui", DefaultValue = false, HelpText = "Do not display any user interface.")]
		public bool NoGui { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
