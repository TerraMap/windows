using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Xml;

namespace TerraMap.Data
{
	public struct NpcInfo
	{
		public int Id;
		public string Name;

		public static NpcInfoList Read(XmlDocument xml)
		{
			var nodeList = xml.GetElementsByTagName("Npc");

			return Read(nodeList);
		}

		public static NpcInfoList Read(XmlNodeList nodeList)
		{
			var list = new NpcInfoList();

			for (int i = 0; i < nodeList.Count; i++)
			{
				var node = nodeList[i];

				int id = 0;

				if (node.Attributes["Id"] != null)
					id = Convert.ToInt32(node.Attributes["Id"].Value);

        var info = new NpcInfo
        {
          Id = id,
          Name = node.Attributes["Name"].Value
        };

        list.Add(info);
			}

			return list;
		}
  }

  public class NpcInfoList : List<NpcInfo>
  {
    public string FindType(int id)
    {
      return this.FirstOrDefault(n => n.Id == id).Name;
    }
  }
}
