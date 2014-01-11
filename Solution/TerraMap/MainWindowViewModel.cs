using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TerraMap.Data;

namespace TerraMap
{
	public class MainWindowViewModel : INotifyPropertyChanged
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

		private ObservableCollection<ObjectInfoViewModel> objectInfoViewModels;

		public ObservableCollection<ObjectInfoViewModel> ObjectInfoViewModels
		{
			get { return objectInfoViewModels; }
			set
			{
				objectInfoViewModels = value;
				RaisePropertyChanged();
			}
		}

		private ObjectInfoViewModel selectedObjectInfoViewModel;

		public ObjectInfoViewModel SelectedObjectInfoViewModel
		{
			get { return selectedObjectInfoViewModel; }
			set
			{
				selectedObjectInfoViewModel = value;
				RaisePropertyChanged();
			}
		}
		
		private TileHitTestInfo currentTileHitTestInfo;

		public TileHitTestInfo CurrentTileHitTestInfo
		{
			get { return currentTileHitTestInfo; }
			set
			{
				currentTileHitTestInfo = value;
				RaisePropertyChanged();
			}
		}

		private string[] currentChestItemNames;

		public string[] CurrentChestItemNames
		{
			get { return currentChestItemNames; }
			set
			{
				currentChestItemNames = value;
				RaisePropertyChanged();
			}
		}

		private FileInfo selectedWorldFile;

		public FileInfo SelectedWorldFile
		{
			get { return selectedWorldFile; }
			set
			{
				selectedWorldFile = value;
				RaisePropertyChanged();
			}
		}

		private ObservableCollection<FileInfo> worldFiles = new ObservableCollection<FileInfo>();

		public ObservableCollection<FileInfo> WorldFiles
		{
			get { return worldFiles; }
			set
			{
				worldFiles = value;
				RaisePropertyChanged();
			}
		}

		private string tileName;

		public string TileName
		{
			get { return tileName; }
			set
			{
				tileName = value;
				RaisePropertyChanged();
			}
		}

		private string position;

		public string Position
		{
			get { return position; }
			set
			{
				position = value;
				RaisePropertyChanged();
			}
		}

		private int progressValue;

		public int ProgressValue
		{
			get { return progressValue; }
			set
			{
				progressValue = value;
				RaisePropertyChanged();
			}
		}

		private int progressMaximum;

		public int ProgressMaximum
		{
			get { return progressMaximum; }
			set
			{
				progressMaximum = value;
				RaisePropertyChanged();
			}
		}

		private string status;

		public string Status
		{
			get { return status; }
			set
			{
				status = value;
				RaisePropertyChanged();
			}
		}

		private bool isLoading;

		public bool IsLoading
		{
			get { return isLoading; }
			set
			{
				isLoading = value;
				RaisePropertyChanged();
			}
		}

		private bool isLoaded;

		public bool IsLoaded
		{
			get { return isLoaded; }
			set
			{
				isLoaded = value;
				RaisePropertyChanged();
			}
		}

		private bool isProgressIndeterminate;

		public bool IsProgressIndeterminate
		{
			get { return isProgressIndeterminate; }
			set
			{
				isProgressIndeterminate = value;
				RaisePropertyChanged();
			}
		}

		private World world;

		public World World
		{
			get { return world; }
			set
			{
				world = value;
				RaisePropertyChanged();
			}
		}

		private bool isHighlighting;

		public bool IsHighlighting
		{
			get { return isHighlighting; }
			set
			{
				isHighlighting = value;
				RaisePropertyChanged();
			}
		}

		private WriteableBitmap writeableBitmap;

		public WriteableBitmap WriteableBitmap
		{
			get { return writeableBitmap; }
			set
			{
				writeableBitmap = value;
				RaisePropertyChanged();
			}
		}

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (this.PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public string Filename { get; set; }

		public void BeginLoading(string status, bool isProgressIndeterminate = false)
		{
			this.Status = status;
			this.IsProgressIndeterminate = isProgressIndeterminate;
			this.IsLoaded = false;
			this.IsLoading = true;
		}

		public void EndLoading()
		{
			this.IsLoaded = true;
			this.IsLoading = false;
		}
	}
}
