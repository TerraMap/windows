using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TerraMap.Data;

namespace TerraMap
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        var options = new Options();
        if (!CommandLine.Parser.Default.ParseArguments(args, options))
          return;

        var start = DateTime.Now;

        var staticDataFileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

        string staticDataFilename = Path.Combine(staticDataFileInfo.DirectoryName, "tiles.xml");

        Console.WriteLine("Reading static data...");

        var world = new World
        {
          StaticData = StaticData.Read(staticDataFilename)
        };

        var objectTypesToHighlight = GetObjectTypesToHighlight(world, options);

        Console.WriteLine("Reading world...");

        world.Read(options.InputFile);

        Console.WriteLine("Read {0} tiles", world.TotalTileCount);

        Console.WriteLine("Writing image data...");

        var pixelFormat = PixelFormats.Bgr32;

        var width = world.WorldWidthinTiles;
        var height = world.WorldHeightinTiles;
        var stride = (width * pixelFormat.BitsPerPixel + 7) / 8;
        var pixels = new byte[stride * height];
        var maskPixels = new byte[stride * height];

        var writeableBitmap = new WriteableBitmap(width, height, 96, 96, pixelFormat, null);

        world.WritePixelData(pixels, stride);

        Int32Rect rect;

        while (world.UpdatedRectangles.TryDequeue(out rect))
        {
          var offset = rect.Y * width * 4;

          writeableBitmap.WritePixels(rect, pixels, stride, offset);
        }

        if (objectTypesToHighlight.Count > 0)
        {
          Console.WriteLine("Writing selected item mask data...");

          world.WritePixelData(maskPixels, stride, objectTypesToHighlight);

          var maskWriteableBitmap = new WriteableBitmap(width, height, 96, 96, pixelFormat, null);

          while (world.UpdatedRectangles.TryDequeue(out rect))
          {
            var offset = rect.Y * width * 4;

            maskWriteableBitmap.WritePixels(rect, maskPixels, stride, offset);
          }

          var destRect = new Rect(0, 0, width, height);
          var point = new Point(0, 0);

          byte alpha = (byte)(255 * 0.75);

          maskWriteableBitmap.ForEach((x, y, color) =>
          {
            if (color == Colors.Black)
              return Color.FromArgb(alpha, color.R, color.G, color.B);
            else
              return color;
          });

          writeableBitmap.Blit(destRect, maskWriteableBitmap, destRect, WriteableBitmapExtensions.BlendMode.Alpha);
        }

        Console.WriteLine("Writing image file...");

        using (var stream = new FileStream(options.OutputFile, FileMode.Create))
        {
          var encoder = new PngBitmapEncoder();
          encoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
          encoder.Save(stream);
          stream.Close();
        }

        var elapsed = DateTime.Now - start;

        world.Status = "Successfully wrote image in " + elapsed;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex);
        Console.Error.WriteLine(ex);
      }
    }

    private static List<ObjectInfoViewModel> GetObjectTypesToHighlight(World world, Options options)
    {
      var objectTypesToHighlight = new List<ObjectInfoViewModel>();

      if (options.TileIds != null)
      {
        foreach (var tileIdString in options.TileIds)
        {
          if (!int.TryParse(tileIdString, out int tileId))
          {
            Console.WriteLine("Invalid tile ID specified on command line: " + tileIdString);
            continue;
          }

          if (tileId < 0 || tileId >= world.StaticData.TileInfos.Count)
          {
            Console.WriteLine("Invalid tile ID specified on command line: " + tileId);
            continue;
          }

          objectTypesToHighlight.Add(
            new ObjectInfoViewModel()
            {
              TileInfo = world.StaticData.TileInfos[tileId]
            });
        }
      }

      if (options.ItemIds != null)
      {
        foreach (var itemIdString in options.ItemIds)
        {
          if (!int.TryParse(itemIdString, out int itemId))
          {
            Console.WriteLine("Invalid item ID specified on command line: " + itemIdString);
            continue;
          }

          if (itemId < 0 || itemId >= world.StaticData.ItemInfos.Count)
          {
            Console.WriteLine("Invalid item ID specified on command line: " + itemId);
            continue;
          }

          objectTypesToHighlight.Add(
            new ObjectInfoViewModel()
            {
              ItemInfo = world.StaticData.ItemInfos[itemId]
            });
        }
      }

      if (options.Names != null)
      {
        foreach (var name in options.Names)
        {
          string lowerName = name.ToLower();

          var tileInfo = world.StaticData.TileInfos[name];
          var itemInfo = world.StaticData.ItemInfos.Values.FirstOrDefault(t => t.Name.ToLower() == name);

          if (tileInfo == null && itemInfo == null)
          {
            Console.WriteLine("Invalid tile and/or item name specified on command line: " + name);
            continue;
          }

          objectTypesToHighlight.Add(new ObjectInfoViewModel()
          {
            TileInfo = tileInfo,
            ItemInfo = itemInfo,
          });
        }
      }

      return objectTypesToHighlight;
    }
  }
}
