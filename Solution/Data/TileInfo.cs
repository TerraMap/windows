using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;

namespace TerraMap.Data
{
	public class TileInfo : ObjectInfo
	{
		public UInt32 ColorValue;
		public string ColorName;
		public Color Color;
		public bool colorSet;
		public bool hasExtra;
		public double light;
		public double lightR, lightG, lightB;
		public bool transparent, solid;
		public bool isStone, isGrass;
		public bool canMerge;
		public Int16 blend;
		public int u, v, minu, maxu, minv, maxv;
		public bool isHilighting;
		public List<TileInfo> variants;

		public byte Id { get; set; }

		public TileInfo()
			: base()
		{
			this.Type = "Block";
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
