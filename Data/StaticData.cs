using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TerraMap.Data
{
	public class StaticData : INotifyPropertyChanged
	{
		private TileInfos tileInfos;

		public TileInfos TileInfos
		{
			get { return tileInfos; }
			set
			{
				tileInfos = value;
				RaisePropertyChanged();
			}
		}

		private List<WallInfo> wallInfos;

		public List<WallInfo> WallInfos
		{
			get { return wallInfos; }
			set
			{
				wallInfos = value;
				RaisePropertyChanged();
			}
		}

		private GlobalColors globalColors;

		public GlobalColors GlobalColors
		{
			get { return globalColors; }
			set
			{
				globalColors = value;
				RaisePropertyChanged();
			}
		}

		private List<ItemPrefix> itemPrefixes;

		public List<ItemPrefix> ItemPrefixes
		{
			get { return itemPrefixes; }
			set
			{
				itemPrefixes = value;
				RaisePropertyChanged();
			}
		}

		private Dictionary<int, ItemInfo> itemInfos;

		public Dictionary<int, ItemInfo> ItemInfos
		{
			get { return itemInfos; }
			set
			{
				itemInfos = value;
				RaisePropertyChanged();
			}
		}

		public static Task<StaticData> ReadAsync(string filename)
		{
			return Task.Factory.StartNew(() =>
			{
				return Read(filename);
			});
		}

		public static StaticData Read(string filename)
		{
			var staticData = new StaticData();

			var xmlDocument = new XmlDocument();

			using (var stream = File.OpenRead(filename))
			{
				xmlDocument.Load(stream);
			}

			staticData.TileInfos = TileInfos.Read(xmlDocument);
			staticData.WallInfos = WallInfo.Read(xmlDocument);
			staticData.GlobalColors = GlobalColors.Read(xmlDocument);
			staticData.ItemPrefixes = ItemPrefix.Read(xmlDocument);
			staticData.ItemInfos = ItemInfo.Read(xmlDocument);

			return staticData;
		}

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (this.PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
