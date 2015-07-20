using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerraMap.Data
{
	public class ItemPrefix
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public static List<ItemPrefix> Read(XmlDocument xmlDocument)
		{
			var prefixNodeList = xmlDocument.GetElementsByTagName("prefix");

			var prefixes = new List<ItemPrefix>();

			for (int i = 0; i < prefixNodeList.Count; i++)
			{
				var prefixNode = prefixNodeList[i];

				int id = Convert.ToInt32(prefixNode.Attributes["num"].Value);
				string name = prefixNode.Attributes["name"].Value;

				var itemPrefix = new ItemPrefix() { Id = id, Name = name };

				prefixes.Add(itemPrefix);
			}

			return prefixes;
		}
	}
}
