using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	public class TileHitTestInfo
	{
		public Tile Tile { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }

		public TileHitTestInfo(Tile tile, int x, int y)
		{
			this.Tile = tile;
			this.X = x;
			this.Y = y;
		}
	}
}
