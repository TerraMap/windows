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
		public UInt32 ColorValue { get; set; }
		public string ColorName { get; set; }
		public Color Color { get; set; }
		public bool IsColorSet { get; set; }
		public bool HasExtra { get; set; }
		public double Light { get; set; }
		public double LightR { get; set; }
		public double LightG { get; set; }
		public double LightB { get; set; }
		public bool IsTransparent { get; set; }
		public bool IsSolid { get; set; }
		public bool IsStone { get; set; }
		public bool IsGrass { get; set; }
		public bool CanMerge { get; set; }
		public Int16 Blend { get; set; }
		public int U { get; set; }
		public int V { get; set; }
		public int MinU { get; set; }
		public int MaxU { get; set; }
		public int MinV { get; set; }
		public int MaxV { get; set; }
		public bool IsHilighting { get; set; }
		public List<TileInfo> Variants { get; set; }

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
