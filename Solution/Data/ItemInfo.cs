using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerraMap.Data
{
	public class ItemInfo : ObjectInfo
	{
		public int Id { get; set; }

		public ItemInfo()
			: base()
		{
			this.Type = "Item";
		}

		public override string ToString()
		{
			return this.Name;
		}

		public static Dictionary<int, ItemInfo> ReadItems(XmlDocument xml)
		{
			var itemInfos = new Dictionary<int, ItemInfo>();

			XmlNodeList itemNodes = xml.GetElementsByTagName("item");

			for (int i = 0; i < itemNodes.Count; i++)
			{
				var itemNode = itemNodes[i];

				int id = Convert.ToInt32(itemNode.Attributes["num"].Value);
				string name = itemNode.Attributes["name"].Value;

				var itemInfo = new ItemInfo() { Id = id, Name = name };

				itemInfos[id] = itemInfo;
			}

			return itemInfos;
		}
	}
}
