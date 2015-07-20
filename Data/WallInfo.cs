using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace TerraMap.Data
{
	public struct WallInfo
  {
    public int Id;
		public string Name;
		public UInt32 ColorValue;
		public Int16 Blend;
		public Color Color;
		public string ColorName;

		public static List<WallInfo> Read(XmlDocument xml)
		{
			var wallNodeList = xml.GetElementsByTagName("wall");

      return Read(wallNodeList);
    }

    public static List<WallInfo> Read(XmlNodeList wallNodeList)
    {
			var wallInfoList = new List<WallInfo>();

			for (int i = 0; i < wallNodeList.Count; i++)
			{
				var wallNode = wallNodeList[i];

        int id = 0;

        if (wallNode.Attributes["num"] != null)
          id = Convert.ToInt32(wallNode.Attributes["num"].Value);

				var wallInfo = new WallInfo();

        wallInfo.Id = id;
				wallInfo.Name = wallNode.Attributes["name"].Value;

				if (wallNode.Attributes["color"] != null)
				{
					wallInfo.ColorName = wallNode.Attributes["color"].Value;
					wallInfo.ColorValue = TileInfos.ParseColor(wallInfo.ColorName);
					wallInfo.Color = ColorTranslator.FromHtml(wallInfo.ColorName);
				}
				else
				{
					wallInfo.Color = MapHelper.GetWallColor((ushort)id);
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
