using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	public class Tile
	{
		public Boolean IsActive { get; set; }

		public Byte Type { get; set; }

		public Int16 TextureU { get; set; }
		public Int16 TextureV { get; set; }

		[PropertyInfo(48)]
		public Boolean IsColorPresent { get; set; }

		[PropertyInfo(48)]
		public Byte ColorValue { get; set; }

		[PropertyInfo(maximumVersion: 25)]
		public Boolean DummyField { get; set; }

		public Boolean IsWallPresent { get; set; }

		public Byte WallType { get; set; }

		[PropertyInfo(48)]
		public Boolean IsWallColorPresent { get; set; }

		[PropertyInfo(48)]
		public byte WallColor { get; set; }

		public bool IsLiquidPresent { get; set; }

		public byte LiquidAmount { get; set; }

		public bool IsLiquidLava { get; set; }

		[PropertyInfo(51)]
		public bool IsLiquidHoney { get; set; }

		[PropertyInfo(33)]
		public bool IsRedWirePresent { get; set; }

		[PropertyInfo(43)]
		public bool IsGreenWirePresent { get; set; }

		[PropertyInfo(43)]
		public bool IsBlueWirePresent { get; set; }

		[PropertyInfo(41)]
		public bool IsHalfTile { get; set; }

		[PropertyInfo(49)]
		public byte Slope { get; set; }

		[PropertyInfo(42)]
		public bool IsActuatorPresent { get; set; }

		[PropertyInfo(42)]
		public bool IsInactive { get; set; }

		[PropertyInfo(25)]
		public Int16 RleLength { get; set; }

		[PropertyInfo(ignore: true)]
		public Color Color { get; set; }

		public static Tile Read(BinaryReader reader, World world)
		{
			var tile = new Tile();

			tile.IsActive = reader.ReadBoolean();
			if (tile.IsActive)
			{
				tile.Type = reader.ReadByte();

				var tileInfo = world.TileInfos[tile.Type];

				if (tileInfo.hasExtra || (world.Version < 72 && tile.Type == 170))
				{
					// torches and platforms didn't have extra in older versions
					if ((world.Version < 28 && tile.Type == 4) || (world.Version < 40 && tile.Type == 19))
					{
						tile.TextureU = -1;
						tile.TextureV = -1;
					}
					else
					{
						tile.TextureU = reader.ReadInt16();
						tile.TextureV = reader.ReadInt16();
						if (tile.Type == 144) //timer
							tile.TextureV = 0;
					}
				}
				else
				{
					tile.TextureU = -1;
					tile.TextureV = -1;
				}

				if (world.Version >= 48)
				{
					tile.IsColorPresent = reader.ReadBoolean();
					if (tile.IsColorPresent)
					{
						tile.ColorValue = reader.ReadByte();
					}
				}
			}

			if (world.Version < 26)
				tile.DummyField = reader.ReadBoolean();

			tile.IsWallPresent = reader.ReadBoolean();
			if (tile.IsWallPresent)
			{
				tile.WallType = reader.ReadByte();

				if (world.Version >= 48)
				{
					tile.IsWallColorPresent = reader.ReadBoolean();
					if (tile.IsWallColorPresent)
						tile.WallColor = reader.ReadByte();
				}
			}

			tile.IsLiquidPresent = reader.ReadBoolean();
			if (tile.IsLiquidPresent)
			{
				tile.LiquidAmount = reader.ReadByte();
				tile.IsLiquidLava = reader.ReadBoolean();
				if (world.Version >= 51)
					tile.IsLiquidHoney = reader.ReadBoolean();
			}

			if (world.Version >= 33)
				tile.IsRedWirePresent = reader.ReadBoolean();

			if (world.Version >= 43)
			{
				tile.IsGreenWirePresent = reader.ReadBoolean();
				tile.IsBlueWirePresent = reader.ReadBoolean();
			}

			if (world.Version >= 41)
				tile.IsHalfTile = reader.ReadBoolean();

			if (world.Version >= 49)
				tile.Slope = reader.ReadByte();

			if (world.Version >= 42)
			{
				tile.IsActuatorPresent = reader.ReadBoolean();
				tile.IsInactive = reader.ReadBoolean();
			}

			if (world.Version >= 25)
				tile.RleLength = reader.ReadInt16();

			return tile;
		}
	}
}
