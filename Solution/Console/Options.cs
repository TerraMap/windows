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

		[Option('t', "tileId", MutuallyExclusiveSet = "highlight", Required = false, HelpText = "ID (number) of a tile to highlight.")]
		public int? TileId { get; set; }

		[Option('m', "itemId", MutuallyExclusiveSet = "highlight", Required = false, HelpText = "ID (number) of an item to highlight.")]
		public int? ItemId { get; set; }

		[Option('n', "name", MutuallyExclusiveSet = "highlight", Required = false, HelpText = "Name of a tile and/or item to highlight.")]
		public string Name { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
		}
	}
}
