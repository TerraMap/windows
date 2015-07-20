using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerraMap.Data
{
	public class GlobalColors
	{
		public Color SkyColor { get; set; }
		public Color EarthColor { get; set; }
		public Color RockColor { get; set; }
		public Color HellColor { get; set; }
		public Color LavaColor { get; set; }
		public Color WaterColor { get; set; }
		public Color HoneyColor { get; set; }

		public static GlobalColors Read(XmlDocument xml)
		{
			var globalColors = new GlobalColors();

			var globalNodes = xml.GetElementsByTagName("global");

			for (int i = 0; i < globalNodes.Count; i++)
			{
				string id = globalNodes[i].Attributes["id"].Value;
				string color = globalNodes[i].Attributes["color"].Value;
				switch (id)
				{
					case "sky":
						globalColors.SkyColor = ColorTranslator.FromHtml(color);
						break;
					case "earth":
						globalColors.EarthColor = ColorTranslator.FromHtml(color);
						break;
					case "rock":
						globalColors.RockColor = ColorTranslator.FromHtml(color);
						break;
					case "hell":
						globalColors.HellColor = ColorTranslator.FromHtml(color);
						break;
					case "water":
						globalColors.WaterColor = ColorTranslator.FromHtml(color);
						break;
					case "lava":
						globalColors.LavaColor = ColorTranslator.FromHtml(color);
						break;
					case "honey":
						globalColors.HoneyColor = ColorTranslator.FromHtml(color);
						break;
				}
			}

			return globalColors;
		}
	}
}
