using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace TerraMap.Data
{
	public class ItemInfo : ObjectInfo
	{
		public int Id { get; set; }

		public static Regex regex = new Regex(@"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", RegexOptions.Compiled);

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

				//if (id >= 1966)
				//{
				//	name = regex.Replace(name, " $1");
				//	Debug.WriteLine(string.Format("<item num=\"{0}\" name=\"{1}\" />", id, name));
				//}

				var itemInfo = new ItemInfo() { Id = id, Name = name };

				itemInfos[id] = itemInfo;
			}

			return itemInfos;
		}
	}
}
