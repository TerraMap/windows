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

		[OptionList('t', "tileId", Required = false, HelpText = "ID numbers of tiles to highlight, separated by semicolons (-t \"1;2;3\").", Separator = ';')]
		public IEnumerable<string> TileIds { get; set; }

		[OptionList('m', "itemId", Required = false, HelpText = "ID numbers of items to highlight, separated by semicolons (-m \"1;2;3\").", Separator = ';')]
		public IEnumerable<string> ItemIds { get; set; }

		[OptionList('n', "name", Required = false, HelpText = "Tile and/or item names to highlight, separated by semicolons (-n \"Ebonstone;Ebonsand;Corrupted Vine\").", Separator = ';')]
		public IEnumerable<string> Names { get; set; }
		
		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			var help = HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
			help.AddPostOptionsLine("Examples:\n");
			help.AddPostOptionsLine("terramapcmd -i \"C:\\Users\\Jason\\Documents\\My Games\\Terraria\\Worlds\\World1.wld\" -o \"C:\\Users\\Jason\\Documents\\My Games\\Terraria\\Worlds\\World1.png\"\n");
			help.AddPostOptionsLine("terramapcmd -i \"C:\\Users\\Jason\\Documents\\My Games\\Terraria\\Worlds\\World1.wld\" -o \"C:\\Users\\Jason\\Documents\\My Games\\Terraria\\Worlds\\World1.png\" -n \"Ebonstone;Ebonsand;Corrupted Vine\"\n");
			help.AddPostOptionsLine("terramapcmd -i \"C:\\Users\\Jason\\Documents\\My Games\\Terraria\\Worlds\\World1.wld\" -o \"C:\\Users\\Jason\\Documents\\My Games\\Terraria\\Worlds\\World1.png\" -t \"23;25;32\"\n");
			help.AddPostOptionsLine("terramapcmd -i \"C:\\Users\\Jason\\Documents\\My Games\\Terraria\\Worlds\\World1.wld\" -o \"C:\\Users\\Jason\\Documents\\My Games\\Terraria\\Worlds\\World1.png\" -m \"23;25;32\"\n");

			return help;
		}
	}
}
