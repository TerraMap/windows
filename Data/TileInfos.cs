using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;

namespace TerraMap.Data
{
	public class TileInfos : List<TileInfo>
	{
		public TileInfos(XmlNodeList nodes)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
        int id = 0;
        
        if(nodes[i].Attributes["num"] != null)
          id = Convert.ToInt32(nodes[i].Attributes["num"].Value);

				var tileInfo = new TileInfo() { Id = (byte)id };
				LoadInfo(tileInfo, nodes[i]);
				this.Add(tileInfo);
			}
		}

		public TileInfo this[int id, Int16 u, Int16 v]
		{
			get { return Find(this[id], u, v); }
		}

		private TileInfo Find(TileInfo tileInfo, Int16 u, Int16 v)
		{
			foreach (TileInfo variant in tileInfo.Variants)
			{
				// must match *all* restrictions... and we take the first match we find.
				if ((variant.U < 0 || variant.U == u) &&
					 (variant.V < 0 || variant.V == v) &&
					 (variant.MinU < 0 || variant.MinU <= u) &&
					 (variant.MinV < 0 || variant.MinV <= v) &&
					 (variant.MaxU < 0 || variant.MaxU > u) &&
					 (variant.MaxV < 0 || variant.MaxV > v))
					return Find(variant, u, v); //check for sub-variants
			}

			// if we get here, there are no variants that match
			return tileInfo;
		}

		public TileInfo this[string name]
		{
			get { return FirstOrDefault(this, name); }
		}

		private TileInfo FirstOrDefault(List<TileInfo> list, string name)
		{
			var nameLower = name.ToLower();

			foreach (var tileInfo in list)
			{
				if (tileInfo.Name.ToLower() == nameLower)
					return tileInfo;

				var variant = this.FirstOrDefault(tileInfo.Variants, nameLower);
				if (variant != null)
					return variant;
			}

			return null;
		}

		public static double ParseDouble(string value)
		{
			return Double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
		}

		public static Int16 ParseInt(string value)
		{
			return Int16.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
		}

		public static UInt32 ParseColor(string color)
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

		private void LoadInfo(TileInfo info, XmlNode node)
		{
      if(node.Attributes["name"] != null)
			  info.Name = node.Attributes["name"].Value;

			if (node.Attributes["color"] != null)
			{
				info.ColorName = node.Attributes["color"].Value;
				info.ColorValue = ParseColor(info.ColorName);
			}
			info.HasExtra = node.Attributes["hasExtra"] != null;
			info.Light = (node.Attributes["light"] == null) ? 0.0 : ParseDouble(node.Attributes["light"].Value);
			info.LightR = (node.Attributes["lightr"] == null) ? 0.0 : ParseDouble(node.Attributes["lightr"].Value);
			info.LightG = (node.Attributes["lightg"] == null) ? 0.0 : ParseDouble(node.Attributes["lightg"].Value);
			info.LightB = (node.Attributes["lightb"] == null) ? 0.0 : ParseDouble(node.Attributes["lightb"].Value);
			info.IsTransparent = node.Attributes["letLight"] != null;
			info.IsSolid = node.Attributes["solid"] != null;
			info.IsStone = node.Attributes["isStone"] != null;
			info.IsGrass = node.Attributes["isGrass"] != null;
			info.CanMerge = node.Attributes["merge"] != null;
			if (node.Attributes["blend"] != null)
				info.Blend = ParseInt(node.Attributes["blend"].Value);
			else
				info.Blend = -1;
			info.Variants = new List<TileInfo>();
			if (node.HasChildNodes)
				for (int i = 0; i < node.ChildNodes.Count; i++)
					info.Variants.Add(NewVariant(info, node.ChildNodes[i]));
		}

		private TileInfo NewVariant(TileInfo parent, XmlNode node)
		{
			TileInfo info = new TileInfo();
			info.Id = parent.Id;
			info.Name = (node.Attributes["name"] == null) ? parent.Name : node.Attributes["name"].Value;
			info.ColorValue = (node.Attributes["color"] == null) ? parent.ColorValue : ParseColor(node.Attributes["color"].Value);
			info.ColorName = (node.Attributes["color"] == null) ? parent.ColorName : node.Attributes["color"].Value;
			info.IsTransparent = (node.Attributes["letLight"] == null) ? parent.IsTransparent : true;
			info.IsSolid = (node.Attributes["solid"] == null) ? parent.IsSolid : true;
			info.Light = (node.Attributes["light"] == null) ? parent.Light : ParseDouble(node.Attributes["light"].Value);
			info.LightR = (node.Attributes["lightr"] == null) ? parent.LightR : ParseDouble(node.Attributes["lightr"].Value);
			info.LightG = (node.Attributes["lightg"] == null) ? parent.LightG : ParseDouble(node.Attributes["lightg"].Value);
			info.LightB = (node.Attributes["lightb"] == null) ? parent.LightB : ParseDouble(node.Attributes["lightb"].Value);
			info.U = (node.Attributes["u"] == null) ? -1 : ParseInt(node.Attributes["u"].Value);
			info.V = (node.Attributes["v"] == null) ? -1 : ParseInt(node.Attributes["v"].Value);
			info.MinU = (node.Attributes["minu"] == null) ? -1 : ParseInt(node.Attributes["minu"].Value);
			info.MaxU = (node.Attributes["maxu"] == null) ? -1 : ParseInt(node.Attributes["maxu"].Value);
			info.MinV = (node.Attributes["minv"] == null) ? -1 : ParseInt(node.Attributes["minv"].Value);
			info.MaxV = (node.Attributes["maxv"] == null) ? -1 : ParseInt(node.Attributes["maxv"].Value);
			info.Variants = new List<TileInfo>();
			if (node.HasChildNodes)
				for (int i = 0; i < node.ChildNodes.Count; i++)
					info.Variants.Add(NewVariant(info, node.ChildNodes[i]));
			return info;
		}

		public static TileInfos Read(XmlDocument xmlDocument)
		{
			var tileInfos = new TileInfos(xmlDocument.GetElementsByTagName("tile"));

			return tileInfos;
		}
	};
}
