using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
  public class Chest
  {
    public int X { get; set; }
    public int Y { get; set; }
    public List<Item> Items { get; set; }
    public string Name { get; set; }

    public Chest()
    {
      this.Items = new List<Item>();
    }

    public static Chest Read(BinaryReader reader, World world)
    {
      if (!reader.ReadBoolean())
        return null;

      Chest chest = new Chest
      {
        X = reader.ReadInt32(),
        Y = reader.ReadInt32()
      };

      var maxItems = 20;
      if (world.Version >= 58)
        maxItems = 40;

      for (int i = 0; i < maxItems; i++)
      {
        var stack = reader.ReadByte();
        byte stackHighByte;

        if (world.Version >= 59)
          stackHighByte = reader.ReadByte();

        if (stack < 1)
          continue;

        var item = new Item
        {
          Count = stack
        };

        if (world.Version < 38)
          item.Name = reader.ReadString();
        else
          item.Id = reader.ReadInt32();

        if (world.Version >= 36)
          item.PrefixId = reader.ReadByte();

        if (item.Id != 0 && world.StaticData.ItemInfos.ContainsKey(item.Id))
        {
          var itemInfo = world.StaticData.ItemInfos[item.Id];
          item.Name = itemInfo.Name;
        }

        if (item.PrefixId > 0 && world.StaticData.ItemPrefixes.Count > item.PrefixId)
          item.Name = world.StaticData.ItemPrefixes[item.PrefixId].Name + " " + item.Name;

        chest.Items.Add(item);
      }

      return chest;
    }
  }
}
