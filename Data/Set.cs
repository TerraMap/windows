using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerraMap.Data
{
	public class Set
	{
		public string Name { get; set; }
		public TileInfos TileInfos { get; set; }
		public List<WallInfo> WallInfos { get; set; }
		public List<ItemInfo> ItemInfos { get; set; }

		public static Task<List<Set>> ReadAsync(string filename)
		{
			return Task.Factory.StartNew(() =>
			{
				return Read(filename);
			});
		}

		public static List<Set> Read(string filename)
		{
			List<Set> sets = new List<Set>();

			var xmlDocument = new XmlDocument();

			using (var stream = File.OpenRead(filename))
			{
				xmlDocument.Load(stream);
			}

			var nodes = xmlDocument.GetElementsByTagName("set");

			for (int i = 0; i < nodes.Count; i++)
			{
				var node = nodes[i];

				var set = new Set();

				set.Name = node.Attributes["name"].Value;

				set.TileInfos = new TileInfos(node.SelectNodes("tile"));
				set.WallInfos = WallInfo.Read(node.SelectNodes("wall"));
				set.ItemInfos = ItemInfo.ReadList(node.SelectNodes("item"));

				sets.Add(set);
			}

			return sets;
		}

		public static void Save(IEnumerable<ObjectInfoSetViewModel> sets, string filename)
		{
			var xmlDocument = new XmlDocument();

			var setsNode = xmlDocument.CreateElement("sets");
			xmlDocument.AppendChild(setsNode);

			foreach (var set in sets)
			{
				var setNode = xmlDocument.CreateElement("set");
				var attribute = xmlDocument.CreateAttribute("name");
				attribute.Value = set.Name;
				setNode.Attributes.Append(attribute);

				foreach (var item in set.ObjectInfoViewModels)
				{
					var itemNode = xmlDocument.CreateElement(item.Type.ToLower());
					attribute = xmlDocument.CreateAttribute("name");
					attribute.Value = item.ItemName;
					itemNode.Attributes.Append(attribute);
					setNode.AppendChild(itemNode);
				}

				setsNode.AppendChild(setNode);
			}

			xmlDocument.Save(filename);
		}
	}
}
