using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;

namespace TerraMap.Data
{
	public class TileInfos : IEnumerable<TileInfo>
	{
		public Color skyColor, earthColor, rockColor, hellColor, lavaColor, waterColor, honeyColor;

		private TileInfo[] tileInfos;
		public WallInfo[] WallInfos { get; private set; }
		public Dictionary<int, ItemInfo> ItemInfos { get; private set; }
		public string[] Prefixes { get; private set; }

		public TileInfos(XmlNodeList nodes)
		{
			tileInfos = new TileInfo[nodes.Count];
			for (int i = 0; i < nodes.Count; i++)
			{
				int id = Convert.ToInt32(nodes[i].Attributes["num"].Value);
				var tileInfo = new TileInfo() { Id = (byte)id };
				loadInfo(tileInfo, nodes[i]);
				tileInfos[id] = tileInfo;
			}
		}

		public TileInfo this[int id] //no variantions
		{
			get { return tileInfos[id]; }
		}

		public TileInfo this[int id, Int16 u, Int16 v]
		{
			get { return find(tileInfos[id], u, v); }
		}

		public ArrayList Items()
		{
			ArrayList items = new ArrayList();
			for (int i = 0; i < tileInfos.Length; i++)
				items.Add(tileInfos[i]);
			return items;
		}

		private TileInfo find(TileInfo info, Int16 u, Int16 v)
		{
			foreach (TileInfo vars in info.variants)
			{
				// must match *all* restrictions... and we take the first match we find.
				if ((vars.u < 0 || vars.u == u) &&
					 (vars.v < 0 || vars.v == v) &&
					 (vars.minu < 0 || vars.minu <= u) &&
					 (vars.minv < 0 || vars.minv <= v) &&
					 (vars.maxu < 0 || vars.maxu > u) &&
					 (vars.maxv < 0 || vars.maxv > v))
					return find(vars, u, v); //check for sub-variants
			}
			// if we get here, there are no variants that match
			return info;
		}

		private double parseDouble(string value)
		{
			return Double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
		}

		private Int16 parseInt(string value)
		{
			return Int16.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
		}

		private UInt32 parseColor(string color)
		{
			UInt32 c = 0;
			for (int j = 0; j < color.Length; j++)
			{
				c <<= 4;
				if (color[j] >= '0' && color[j] <= '9')
					c |= (byte)(color[j] - '0');
				else if (color[j] >= 'A' && color[j] <= 'F')
					c |= (byte)(10 + color[j] - 'A');
				else if (color[j] >= 'a' && color[j] <= 'f')
					c |= (byte)(10 + color[j] - 'a');
			}
			return c;
		}

		private void loadInfo(TileInfo info, XmlNode node)
		{
			info.Name = node.Attributes["name"].Value;
			info.ColorName = node.Attributes["color"].Value;
			info.ColorValue = parseColor(info.ColorName);
			info.hasExtra = node.Attributes["hasExtra"] != null;
			info.light = (node.Attributes["light"] == null) ? 0.0 : parseDouble(node.Attributes["light"].Value);
			info.lightR = (node.Attributes["lightr"] == null) ? 0.0 : parseDouble(node.Attributes["lightr"].Value);
			info.lightG = (node.Attributes["lightg"] == null) ? 0.0 : parseDouble(node.Attributes["lightg"].Value);
			info.lightB = (node.Attributes["lightb"] == null) ? 0.0 : parseDouble(node.Attributes["lightb"].Value);
			info.transparent = node.Attributes["letLight"] != null;
			info.solid = node.Attributes["solid"] != null;
			info.isStone = node.Attributes["isStone"] != null;
			info.isGrass = node.Attributes["isGrass"] != null;
			info.canMerge = node.Attributes["merge"] != null;
			if (node.Attributes["blend"] != null)
				info.blend = parseInt(node.Attributes["blend"].Value);
			else
				info.blend = -1;
			info.variants = new List<TileInfo>();
			if (node.HasChildNodes)
				for (int i = 0; i < node.ChildNodes.Count; i++)
					info.variants.Add(newVariant(info, node.ChildNodes[i]));
		}

		private TileInfo newVariant(TileInfo parent, XmlNode node)
		{
			TileInfo info = new TileInfo();
			info.Id = parent.Id;
			info.Name = (node.Attributes["name"] == null) ? parent.Name : node.Attributes["name"].Value;
			info.ColorValue = (node.Attributes["color"] == null) ? parent.ColorValue : parseColor(node.Attributes["color"].Value);
			info.ColorName = (node.Attributes["color"] == null) ? parent.ColorName : node.Attributes["color"].Value;
			info.transparent = (node.Attributes["letLight"] == null) ? parent.transparent : true;
			info.solid = (node.Attributes["solid"] == null) ? parent.solid : true;
			info.light = (node.Attributes["light"] == null) ? parent.light : parseDouble(node.Attributes["light"].Value);
			info.lightR = (node.Attributes["lightr"] == null) ? parent.lightR : parseDouble(node.Attributes["lightr"].Value);
			info.lightG = (node.Attributes["lightg"] == null) ? parent.lightG : parseDouble(node.Attributes["lightg"].Value);
			info.lightB = (node.Attributes["lightb"] == null) ? parent.lightB : parseDouble(node.Attributes["lightb"].Value);
			info.u = (node.Attributes["u"] == null) ? -1 : parseInt(node.Attributes["u"].Value);
			info.v = (node.Attributes["v"] == null) ? -1 : parseInt(node.Attributes["v"].Value);
			info.minu = (node.Attributes["minu"] == null) ? -1 : parseInt(node.Attributes["minu"].Value);
			info.maxu = (node.Attributes["maxu"] == null) ? -1 : parseInt(node.Attributes["maxu"].Value);
			info.minv = (node.Attributes["minv"] == null) ? -1 : parseInt(node.Attributes["minv"].Value);
			info.maxv = (node.Attributes["maxv"] == null) ? -1 : parseInt(node.Attributes["maxv"].Value);
			info.variants = new List<TileInfo>();
			if (node.HasChildNodes)
				for (int i = 0; i < node.ChildNodes.Count; i++)
					info.variants.Add(newVariant(info, node.ChildNodes[i]));
			return info;
		}

		public static TileInfos Read(string path)
		{
			XmlDocument xml = new XmlDocument();

			using (var stream = File.OpenRead(path))
			{
				xml.Load(stream);
			}

			var tileInfos = new TileInfos(xml.GetElementsByTagName("tile"));

			tileInfos.ReadWalls(xml);
			tileInfos.ReadGlobals(xml);
			tileInfos.ReadPrefixes(xml);
			tileInfos.ReadItems(xml);

			return tileInfos;
		}

		private void ReadWalls(XmlDocument xml)
		{
			XmlNodeList wallList = xml.GetElementsByTagName("wall");
			WallInfos = new WallInfo[wallList.Count + 1];
			for (int i = 0; i < wallList.Count; i++)
			{
				var wall = wallList[i];

				int id = Convert.ToInt32(wall.Attributes["num"].Value);

				var wallInfo = WallInfos[id];
				wallInfo.Name = wall.Attributes["name"].Value;

				string colorName = wall.Attributes["color"].Value;
				wallInfo.ColorValue = parseColor(colorName);
				wallInfo.Color = ColorTranslator.FromHtml(colorName);

				if (wall.Attributes["blend"] != null)
					wallInfo.Blend = Convert.ToInt16(wallList[i].Attributes["blend"].Value);
				else
					wallInfo.Blend = (Int16)id;

				WallInfos[id] = wallInfo;
			}
		}

		private void ReadGlobals(XmlDocument xml)
		{
			XmlNodeList globalList = xml.GetElementsByTagName("global");
			for (int i = 0; i < globalList.Count; i++)
			{
				string id = globalList[i].Attributes["id"].Value;
				string color = globalList[i].Attributes["color"].Value;
				switch (id)
				{
					case "sky":
						skyColor = ColorTranslator.FromHtml(color);
						break;
					case "earth":
						earthColor = ColorTranslator.FromHtml(color);
						break;
					case "rock":
						rockColor = ColorTranslator.FromHtml(color);
						break;
					case "hell":
						hellColor = ColorTranslator.FromHtml(color);
						break;
					case "water":
						waterColor = ColorTranslator.FromHtml(color);
						break;
					case "lava":
						lavaColor = ColorTranslator.FromHtml(color);
						break;
					case "honey":
						honeyColor = ColorTranslator.FromHtml(color);
						break;
				}
			}
		}

		private void ReadPrefixes(XmlDocument xml)
		{
			XmlNodeList prefixList = xml.GetElementsByTagName("prefix");
			Prefixes = new string[prefixList.Count + 1];
			for (int i = 0; i < prefixList.Count; i++)
			{
				int id = Convert.ToInt32(prefixList[i].Attributes["num"].Value);
				Prefixes[id] = prefixList[i].Attributes["name"].Value;
			}
		}

		private void ReadItems(XmlDocument xml)
		{
			this.ItemInfos = new Dictionary<int, ItemInfo>();

			XmlNodeList itemList = xml.GetElementsByTagName("item");

			for (int i = 0; i < itemList.Count; i++)
			{
				var element = itemList[i];

				int id = Convert.ToInt32(element.Attributes["num"].Value);
				string name = element.Attributes["name"].Value;

				var itemInfo = new ItemInfo() { Id = id, Name = name };

				ItemInfos[id] = itemInfo;
			}
		}

		public IEnumerator<TileInfo> GetEnumerator()
		{
			return this.tileInfos.ToList().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.tileInfos.GetEnumerator();
		}
	};

}
