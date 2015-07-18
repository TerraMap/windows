using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TerraMap.Data;

namespace TerraMap
{
	public class MainWindowViewModel : ViewModelBase
	{
		private StaticData staticData;

		public StaticData StaticData
		{
			get { return staticData; }
			set
			{
				staticData = value;
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

		private ObservableCollection<ObjectInfoSetViewModel> sets;

		public ObservableCollection<ObjectInfoSetViewModel> Sets
		{
			get { return sets; }
			set
			{
				sets = value;
				RaisePropertyChanged();
			}
		}

		private ObjectInfoSetViewModel selectedSet;

		public ObjectInfoSetViewModel SelectedSet
		{
			get { return selectedSet; }
			set
			{
				selectedSet = value;
				RaisePropertyChanged();
			}
		}

		private ObservableCollection<NPC> npcs = new ObservableCollection<NPC>();

		public ObservableCollection<NPC> NPCs
		{
			get { return npcs; }
			set
			{
				npcs = value;
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

		private WorldFileViewModel selectedWorldFile;

		public WorldFileViewModel SelectedWorldFile
		{
			get { return selectedWorldFile; }
			set
			{
				selectedWorldFile = value;
				RaisePropertyChanged();
			}
		}

		private ObservableCollection<WorldFileViewModel> worldFiles = new ObservableCollection<WorldFileViewModel>();

		public ObservableCollection<WorldFileViewModel> WorldFiles
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

		private int totalTileCount;

		public int TotalTileCount
		{
			get { return totalTileCount; }
			set
			{
				totalTileCount = value;
				RaisePropertyChanged();
			}
		}

		private int highlightedTileCount;

		public int HighlightedTileCount
		{
			get { return highlightedTileCount; }
			set
			{
				highlightedTileCount = value;
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

		private WriteableBitmap writeableBitmapMask;

		public WriteableBitmap WriteableBitmapMask
		{
			get { return writeableBitmapMask; }
			set
			{
				writeableBitmapMask = value;
				RaisePropertyChanged();
			}
		}

		private double maskOpacity = 0.75;

		public double MaskOpacity
		{
			get { return maskOpacity; }
			set
			{
				maskOpacity = value;
				RaisePropertyChanged();
			}
		}

		public string Filename { get; set; }

		private Visibility updateVisibility = Visibility.Collapsed;

		public Visibility UpdateVisibility
		{
			get { return updateVisibility; }
			set
			{
				updateVisibility = value;
				RaisePropertyChanged();
			}
		}

		private ReleaseInfo newRelease;

		public ReleaseInfo NewRelease
		{
			get { return newRelease; }
			set
			{
				newRelease = value;
				RaisePropertyChanged();
			}
		}

		private bool isCheckingForUpdate;

		public bool IsCheckingForUpdate
		{
			get { return isCheckingForUpdate; }
			set
			{
				isCheckingForUpdate = value;
				RaisePropertyChanged();
			}
		}

		public string SetsFilename { get; internal set; }

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

			CommandManager.InvalidateRequerySuggested();
		}
	}
}
