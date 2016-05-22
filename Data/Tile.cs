using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TerraMap.Data
{
  public class Tile
  {
    public Boolean IsActive { get; set; }
    public ushort Type { get; set; }
    public Int16 TextureU { get; set; }
    public Int16 TextureV { get; set; }
    public Boolean IsColorPresent { get; set; }
    public Byte ColorValue { get; set; }
    public Boolean DummyField { get; set; }
    public Boolean IsWallPresent { get; set; }
    public Byte WallType { get; set; }
    public Boolean IsWallColorPresent { get; set; }
    public byte WallColor { get; set; }
    public bool IsLiquidPresent { get; set; }
    public byte LiquidAmount { get; set; }
    public bool IsLiquidLava { get; set; }
    public bool IsLiquidHoney { get; set; }
    public bool IsRedWirePresent { get; set; }
    public bool IsGreenWirePresent { get; set; }
    public bool IsBlueWirePresent { get; set; }
    public bool IsYellowWirePresent { get; set; }
    public bool IsHalfTile { get; set; }
    public byte Slope { get; set; }
    public bool IsActuatorPresent { get; set; }
    public bool IsInactive { get; set; }
    public Int16 RleLength { get; set; }
    public Color Color { get; set; }

    public Tile()
    {
    }

    public Tile(Tile tile)
    {
      this.IsActive = tile.IsActive;
      this.Type = tile.Type;
      this.TextureU = tile.TextureU;
      this.TextureV = tile.TextureV;
      this.IsColorPresent = tile.IsColorPresent;
      this.ColorValue = tile.ColorValue;
      this.DummyField = tile.DummyField;
      this.IsWallPresent = tile.IsWallPresent;
      this.WallType = tile.WallType;
      this.IsWallColorPresent = tile.IsWallColorPresent;
      this.WallColor = tile.WallColor;
      this.IsLiquidPresent = tile.IsLiquidPresent;
      this.LiquidAmount = tile.LiquidAmount;
      this.IsLiquidLava = tile.IsLiquidLava;
      this.IsLiquidHoney = tile.IsLiquidHoney;
      this.IsRedWirePresent = tile.IsRedWirePresent;
      this.IsGreenWirePresent = tile.IsGreenWirePresent;
      this.IsBlueWirePresent = tile.IsBlueWirePresent;
      this.IsYellowWirePresent = tile.IsYellowWirePresent;
      this.IsHalfTile = tile.IsHalfTile;
      this.Slope = tile.Slope;
      this.IsActuatorPresent = tile.IsActuatorPresent;
      this.IsInactive = tile.IsInactive;
      this.RleLength = tile.RleLength;
      this.Color = tile.Color;
    }

    public static Tile Read(BinaryReader reader, World world)
    {
      var tile = new Tile();

      tile.IsActive = reader.ReadBoolean();
      if (tile.IsActive)
      {
        tile.Type = reader.ReadByte();

        var tileInfo = world.StaticData.TileInfos[tile.Type];

        if (tileInfo.HasExtra || (world.Version < 72 && tile.Type == 170))
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
