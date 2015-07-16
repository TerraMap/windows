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

  }
}
