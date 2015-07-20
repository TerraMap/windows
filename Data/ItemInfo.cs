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

		public static Dictionary<int, ItemInfo> Read(XmlDocument xml)
		{
			var itemNodes = xml.GetElementsByTagName("item");

      return Read(itemNodes);
    }

    public static Dictionary<int, ItemInfo> Read(XmlNodeList itemNodes)
    {
			var itemInfos = new Dictionary<int, ItemInfo>();

			for (int i = 0; i < itemNodes.Count; i++)
			{
				var itemNode = itemNodes[i];
        
        int id = 0;

        if (itemNode.Attributes["num"] != null)
          id = Convert.ToInt32(itemNode.Attributes["num"].Value);

				string name = itemNode.Attributes["name"].Value;

				var itemInfo = new ItemInfo() { Id = id, Name = name };

				itemInfos[id] = itemInfo;
			}

			return itemInfos;
		}

    public static List<ItemInfo> ReadList(XmlNodeList itemNodes)
    {
      var itemInfos = new List<ItemInfo>();

      for (int i = 0; i < itemNodes.Count; i++)
      {
        var itemNode = itemNodes[i];

        int id = 0;

        if (itemNode.Attributes["num"] != null)
          id = Convert.ToInt32(itemNode.Attributes["num"].Value);

        string name = itemNode.Attributes["name"].Value;

        var itemInfo = new ItemInfo() { Id = id, Name = name };

        itemInfos.Add(itemInfo);
      }

      return itemInfos;
    }
	}
}
