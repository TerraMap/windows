using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
  public class TileHitTestInfo
  {
    public Tile Tile { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public TileInfo TileInfo { get; set; }
    public string TileUV { get; set; }
    public string Name { get; set; }

    public Chest Chest { get; set; }
    public string ChestName { get; set; }

    public Sign Sign { get; set; }
    public string SignText { get; set; }

    public String Actuator { get; set; }

    public string Liquid { get; set; }

    public WallInfo WallInfo { get; set; }
    public string WallName { get; set; }
    public string WallType { get; set; }

    public string WireColors { get; set; }

    public TileHitTestInfo()
    {
    }

    public TileHitTestInfo(int x, int y)
    {
    }

    public TileHitTestInfo(Tile tile, int x, int y)
    {
      this.Tile = tile;
      this.X = x;
      this.Y = y;
    }
  }
}
