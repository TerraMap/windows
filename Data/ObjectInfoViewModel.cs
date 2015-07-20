using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TerraMap.Data;

namespace TerraMap.Data
{
	public class ObjectInfoViewModel : INotifyPropertyChanged
	{
		private ItemInfo itemInfo;

		public ItemInfo ItemInfo
		{
			get { return itemInfo; }
			set
			{
				itemInfo = value;
				RaisePropertyChanged();
				RaisePropertyChanged("Type");
				RaisePropertyChanged("ItemName");
				RaisePropertyChanged("ItemId");
			}
		}

		private TileInfo tileInfo;

		public TileInfo TileInfo
		{
			get { return tileInfo; }
			set
			{
				tileInfo = value;
				RaisePropertyChanged();
				RaisePropertyChanged("Type");
				RaisePropertyChanged("ItemName");
				RaisePropertyChanged("ItemId");
			}
		}

		private WallInfo wallInfo;

		public WallInfo WallInfo
		{
			get { return wallInfo; }
			set
			{
				wallInfo = value;
				RaisePropertyChanged();
				RaisePropertyChanged("Type");
				RaisePropertyChanged("ItemName");
				RaisePropertyChanged("ItemId");
			}
		}

		private bool isRedWire;

		public bool IsRedWire
		{
			get { return isRedWire; }
			set
			{
				isRedWire = value;
				RaisePropertyChanged();
			}
		}

		private bool isGreenWire;

		public bool IsGreenWire
		{
			get { return isGreenWire; }
			set
			{
				isGreenWire = value;
				RaisePropertyChanged();
			}
		}

		private bool isBlueWire;

		public bool IsBlueWire
		{
			get { return isBlueWire; }
			set
			{
				isBlueWire = value;
				RaisePropertyChanged();
			}
		}

		private string name;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				RaisePropertyChanged();
			}
		}

		private bool isSelected;

		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				isSelected = value;
				RaisePropertyChanged();
			}
		}

		public virtual string Type
		{
			get
			{
				if (this.tileInfo != null)
					return "Tile";
				else if (this.itemInfo != null)
					return "Item";
				else
					return "Wall";
			}
		}

		public string ItemName
		{
			get
			{
				if (this.tileInfo != null)
					return this.tileInfo.Name;
				else if (this.itemInfo != null)
					return this.itemInfo.Name;
				else
					return this.wallInfo.Name;
			}
		}

		public int ItemId
		{
			get
			{
				if (this.tileInfo != null)
					return this.tileInfo.Id;
				else if (this.itemInfo != null)
					return this.itemInfo.Id;
				else
					return this.wallInfo.Id;
			}
		}

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (this.PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
