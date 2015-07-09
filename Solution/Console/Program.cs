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

				var world = new World();

				world.StaticData = StaticData.Read(staticDataFilename);

				Console.WriteLine("Reading world...");

				world.Read(options.InputFile);

				Console.WriteLine("Read {0} tiles", world.TotalTileCount);

				Console.WriteLine("Writing image data...");

				var pixelFormat = PixelFormats.Bgr32;

				var width = world.WorldWidthinTiles;
				var height = world.WorldHeightinTiles;
				var stride = (width * pixelFormat.BitsPerPixel + 7) / 8;
				var pixels = new byte[stride * height];

				var writeableBitmap = new WriteableBitmap(width, height, 96, 96, pixelFormat, null);

				ObjectInfoViewModel objectTypeToHighlight = null;

				if (options.TileId != null)
				{
					objectTypeToHighlight = new ObjectInfoViewModel();
					objectTypeToHighlight.TileInfo = world.StaticData.TileInfos[options.TileId.Value];
				}
				else if (options.ItemId != null)
				{
					objectTypeToHighlight = new ObjectInfoViewModel();
					objectTypeToHighlight.ItemInfo = world.StaticData.ItemInfos[options.TileId.Value];
				}
				else if (!string.IsNullOrEmpty(options.Name))
				{
					string name = options.Name.ToLower();

					objectTypeToHighlight = new ObjectInfoViewModel();
					objectTypeToHighlight.TileInfo = world.StaticData.TileInfos[name];
					objectTypeToHighlight.ItemInfo = world.StaticData.ItemInfos.Values.FirstOrDefault(t => t.Name.ToLower() == name);
				}

        ObjectInfoViewModel[] objectTypesToHighlight = null;
        if (objectTypeToHighlight != null)
          objectTypesToHighlight = new ObjectInfoViewModel[] { objectTypeToHighlight };

        world.WritePixelData(pixels, stride, objectTypesToHighlight);

				Int32Rect rect;

				while (world.UpdatedRectangles.TryDequeue(out rect))
				{
					var offset = rect.Y * width * 4;

					writeableBitmap.WritePixels(rect, pixels, stride, offset);
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
	}
}
