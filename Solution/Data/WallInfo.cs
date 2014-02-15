using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace TerraMap.Data
{
	public struct WallInfo
	{
		public string Name;
		public UInt32 ColorValue;
		public Int16 Blend;
		public Color Color;
		public string ColorName;

		public static List<WallInfo> Read(XmlDocument xml)
		{
			XmlNodeList wallNodeList = xml.GetElementsByTagName("wall");

			var wallInfoList = new List<WallInfo>();

			for (int i = 0; i < wallNodeList.Count; i++)
			{
				var wallNode = wallNodeList[i];

				int id = Convert.ToInt32(wallNode.Attributes["num"].Value);

				var wallInfo = new WallInfo();

				wallInfo.Name = wallNode.Attributes["name"].Value;

				if (wallNode.Attributes["color"] != null)
				{
					wallInfo.ColorName = wallNode.Attributes["color"].Value;
					wallInfo.ColorValue = TileInfos.ParseColor(wallInfo.ColorName);
					wallInfo.Color = ColorTranslator.FromHtml(wallInfo.ColorName);
				}
				else
				{
					wallInfo.Color = Map.GetWallColor((ushort)id);
				}

				if (wallNode.Attributes["blend"] != null)
					wallInfo.Blend = Convert.ToInt16(wallNodeList[i].Attributes["blend"].Value);
				else
					wallInfo.Blend = (Int16)id;

				wallInfoList.Add(wallInfo);
			}

			return wallInfoList;
		}
	}
}
