using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
						globalColors.SkyColor = (Color)ColorConverter.ConvertFromString(color);
						break;
					case "earth":
						globalColors.EarthColor = (Color)ColorConverter.ConvertFromString(color);
						break;
					case "rock":
						globalColors.RockColor = (Color)ColorConverter.ConvertFromString(color);
						break;
					case "hell":
						globalColors.HellColor = (Color)ColorConverter.ConvertFromString(color);
						break;
					case "water":
						globalColors.WaterColor = (Color)ColorConverter.ConvertFromString(color);
						break;
					case "lava":
						globalColors.LavaColor = (Color)ColorConverter.ConvertFromString(color);
						break;
					case "honey":
						globalColors.HoneyColor = (Color)ColorConverter.ConvertFromString(color);
						break;
				}
			}

			return globalColors;
		}
	}
}
