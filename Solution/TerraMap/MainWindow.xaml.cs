using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using TerraMap.Data;
using WPFExtensions.Controls;

namespace TerraMap
{
	public partial class MainWindow : Window
	{
		MainWindowViewModel viewModel = new MainWindowViewModel();
		private bool ignoreSelectedWorldFileChanges;

		Storyboard indicatorStoryboard;

		int width, height, stride;
		byte[] pixels;
		PixelFormat pixelFormat = PixelFormats.Bgr32;

		public MainWindow()
		{
			InitializeComponent();

			viewModel = (MainWindowViewModel)this.DataContext;
			viewModel.World = null;
			viewModel.IsLoading = false;
			viewModel.Status = null;
			viewModel.Position = null;
			viewModel.TileName = null;
			indicatorStoryboard = (Storyboard)this.FindResource("indicatorStoryboard");
		}

		private async void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.LoadWorldFiles();

			var staticDataFileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

			string staticDataFilename = Path.Combine(staticDataFileInfo.DirectoryName, "tiles.xml");

			var staticData = await StaticData.ReadAsync(staticDataFilename);

			this.viewModel.StaticData = staticData;

			this.viewModel.ObjectInfoViewModels = new ObservableCollection<ObjectInfoViewModel>();

			foreach (var itemInfo in staticData.ItemInfos.Values.Where(i => !string.IsNullOrEmpty(i.Name)))
				this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { ItemInfo = itemInfo, Name = itemInfo.Name });

			foreach (var tileInfo in staticData.TileInfos)
			{
				var existingTileInfo = this.viewModel.ObjectInfoViewModels.FirstOrDefault(v => v.Name == tileInfo.Name);

				if (existingTileInfo == null)
				{
					if (!string.IsNullOrEmpty(tileInfo.Name))
					{
						this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { TileInfo = tileInfo, Name = tileInfo.Name });
					}
				}
				else
				{
					existingTileInfo.TileInfo = tileInfo;
				}

				foreach (var variant in tileInfo.Variants)
				{
					var existingVariantViewModel = this.viewModel.ObjectInfoViewModels.FirstOrDefault(v => v.Name == variant.Name);

					if (existingVariantViewModel == null)
					{
						if (!string.IsNullOrEmpty(variant.Name))
						{
							this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { TileInfo = variant, Name = variant.Name });
						}
					}
					else
					{
						existingVariantViewModel.TileInfo = variant;
					}
				}
			}

			this.viewModel.SelectedObjectInfoViewModel = this.viewModel.ObjectInfoViewModels.FirstOrDefault(v => v.Name == "Sand");

			var args = App.Current.Properties["Args"] as string[];
			if (args != null && args.Length > 0)
			{
				string filename = args[0];

				var uri = new Uri(filename);
				filename = uri.LocalPath;

				await this.Open(filename);
			}
		}

		private void LoadWorldFiles()
		{
			try
			{
				ignoreSelectedWorldFileChanges = true;

				FileInfo worldFile = this.viewModel.SelectedWorldFile;

				this.viewModel.WorldFiles.Clear();

				var path = this.GetWorldsPath();

				foreach (var filename in Directory.GetFiles(path, "*.wld"))
				{
					this.viewModel.WorldFiles.Add(new FileInfo(filename));
				}

				if (worldFile != null)
				{
					worldFile = this.viewModel.WorldFiles.FirstOrDefault(f => f.FullName == worldFile.FullName);

					this.viewModel.SelectedWorldFile = worldFile;
				}
			}
			finally
			{
				ignoreSelectedWorldFileChanges = false;
			}
		}

		private void OnWorldProgressChanged(object sender, ProgressEventArgs e)
		{
			//if (!this.Dispatcher.CheckAccess())
			//{
			//	this.Dispatcher.Invoke(new Action<object, ProgressEventArgs>(this.OnWorldProgressChanged), sender, e);
			//	return;
			//}

			viewModel.ProgressMaximum = e.Maximum;
			viewModel.ProgressValue = e.Value;
		}

		private void OnDispatcherTimerTick(object sender, EventArgs e)
		{
			this.Update();
		}

		private string GetWorldsPath()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			path = Path.Combine(path, "My Games");
			path = Path.Combine(path, "Terraria");
			path = Path.Combine(path, "Worlds");
			return path;
		}

		private async Task Open()
		{
			string path = this.GetWorldsPath();

			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "World Files (*.wld)|*.wld|World Backup Files (*.bak)|*.*.bak|All Files (*.*)|*.*";

			if (Directory.Exists(path))
				dialog.InitialDirectory = path;

			var result = dialog.ShowDialog() ?? false;
			if (!result)
				return;

			await this.Open(dialog.FileName);
		}

		private async Task Open(FileInfo fileInfo)
		{
			this.viewModel.SelectedWorldFile = fileInfo;

			await this.Open(fileInfo.FullName);
		}

		private async Task Open(string filename)
		{
			var start = DateTime.Now;

			this.viewModel.TileName = null;

			this.Title = "World Info - " + filename;

			this.viewModel.Filename = filename;

			var world = new World();

			world.StaticData = this.viewModel.StaticData;

			this.viewModel.World = world;

			this.viewModel.BeginLoading("Reading world...");
			try
			{
				var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Background, this.OnDispatcherTimerTick, this.Dispatcher);
				timer.Start();

				await world.ReadAsync(filename);

				this.viewModel.TotalTileCount = world.TotalTileCount;

				this.Grid.Width = world.WorldWidthinTiles;
				this.Grid.Height = world.WorldHeightinTiles;

				this.Canvas.Width = world.WorldWidthinTiles;
				this.Canvas.Height = world.WorldHeightinTiles;

				world.Status = "Drawing map";

				width = world.WorldWidthinTiles;
				height = world.WorldHeightinTiles;
				stride = (width * pixelFormat.BitsPerPixel + 7) / 8;
				pixels = new byte[stride * height];

				viewModel.WriteableBitmap = new WriteableBitmap(width, height, 96, 96, pixelFormat, null);

				if (this.viewModel.IsHighlighting)
					await world.WritePixelDataAsync(pixels, stride, this.viewModel.SelectedObjectInfoViewModel);
				else
					await world.WritePixelDataAsync(pixels, stride);

				timer.Stop();

				this.Update();

				world.Status = "";

				var elapsed = DateTime.Now - start;

				world.Status = "Loaded in " + elapsed;
			}
			finally
			{
				this.viewModel.EndLoading();
			}
		}

		private async Task Refresh()
		{
			await this.Open(this.viewModel.Filename);
		}

		private async Task UpdateHighlight()
		{
			var start = DateTime.Now;

			if (this.viewModel.IsLoading)
				return;

			var world = this.viewModel.World;
			if (world == null)
				return;

			this.viewModel.IsLoading = true;

			world.Status = "Updating map";

			var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(20), DispatcherPriority.Background, this.OnDispatcherTimerTick, this.Dispatcher);
			timer.Start();

			if (this.viewModel.IsHighlighting)
				await world.WritePixelDataAsync(pixels, stride, this.viewModel.SelectedObjectInfoViewModel);
			else
				await world.WritePixelDataAsync(pixels, stride);

			timer.Stop();

			this.Update();

			world.Status = "";

			this.viewModel.IsLoaded = true;
			this.viewModel.IsLoading = false;

			var elapsed = DateTime.Now - start;

			if (this.viewModel.IsHighlighting)
				world.Status = string.Format("Highlighted {0:N0} out of {1:N0} blocks in {2:N1} seconds", this.viewModel.HighlightedTileCount, this.viewModel.TotalTileCount, elapsed.TotalSeconds);
			else
				world.Status = string.Format("Updated {0:N0} blocks in {1:N1} seconds", this.viewModel.TotalTileCount, elapsed.TotalSeconds);
		}

		private void Update()
		{
			this.viewModel.Status = viewModel.World.Status;
			this.viewModel.ProgressMaximum = viewModel.World.ProgressMaximum;
			this.viewModel.ProgressValue = viewModel.World.ProgressValue;
			this.viewModel.HighlightedTileCount = viewModel.World.HighlightedTileCount;

			Int32Rect rect;

			while (viewModel.World.UpdatedRectangles.TryDequeue(out rect))
			{
				var offset = rect.Y * width * 4;

				viewModel.WriteableBitmap.WritePixels(rect, pixels, stride, offset);
			}
		}

		private void UpdateAll()
		{
			var rect = new Int32Rect(0, 0, this.width, this.height);
			viewModel.WriteableBitmap.WritePixels(rect, pixels, stride, 0);
		}

		private void Save()
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Filter = "PNG Images (*.png)|*.png";
			var result = dialog.ShowDialog() ?? false;
			if (!result)
				return;

			this.Save(dialog.FileName);
		}

		private void Save(string filename)
		{
			using (var stream = new FileStream(filename, FileMode.Create))
			{
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(this.viewModel.WriteableBitmap));
				encoder.Save(stream);
				stream.Close();
			}
		}

		private void ClearCurrentTileHit()
		{
			this.viewModel.CurrentTileHitTestInfo = null;

			this.Indicator.Visibility = Visibility.Collapsed;
		}

		private async Task FindTile(SearchDirection direction)
		{
			this.viewModel.BeginLoading("Finding tile...", true);

			TileHitTestInfo tileHitTestInfo = null;

			if (direction == SearchDirection.Forwards)
			{
				tileHitTestInfo = await this.FindNextTileAsync(this.viewModel.SelectedObjectInfoViewModel, this.viewModel.CurrentTileHitTestInfo);
				if (tileHitTestInfo == null && this.viewModel.CurrentTileHitTestInfo != null)
					tileHitTestInfo = await this.FindNextTileAsync(this.viewModel.SelectedObjectInfoViewModel);
			}
			else
			{
				tileHitTestInfo = await this.FindPreviousTileAsync(this.viewModel.SelectedObjectInfoViewModel, this.viewModel.CurrentTileHitTestInfo);
				if (tileHitTestInfo == null && this.viewModel.CurrentTileHitTestInfo != null)
					tileHitTestInfo = await this.FindPreviousTileAsync(this.viewModel.SelectedObjectInfoViewModel);
			}

			this.viewModel.EndLoading();

			this.viewModel.CurrentTileHitTestInfo = tileHitTestInfo;

			if (tileHitTestInfo == null)
			{
				this.Indicator.Visibility = Visibility.Collapsed;
				return;
			}

			this.Indicator.Visibility = Visibility.Visible;
			Canvas.SetLeft(this.Indicator, tileHitTestInfo.X - 1);
			Canvas.SetTop(this.Indicator, tileHitTestInfo.Y - 1);

			indicatorStoryboard.Begin();

			if (this.zoomControl.Zoom < 1)
				this.zoomControl.Zoom = 1;

			this.zoomControl.Mode = ZoomControlModes.Custom;

			this.NavigateTo(tileHitTestInfo.X, tileHitTestInfo.Y);
		}

		private void NavigateToSpawn()
		{
			this.NavigateTo(this.viewModel.World.SpawnX, this.viewModel.World.SpawnY);
		}

		private void NavigateToDungeon()
		{
			this.NavigateTo(this.viewModel.World.DungeonX, this.viewModel.World.DungeonY);
		}

		private void NavigateToNpc(NPC npc)
		{
			this.NavigateTo(npc.X / 16, npc.Y / 16);
		}

		private void NavigateTo(int x, int y)
		{
			this.zoomControl.TranslateX = (-x + this.zoomControl.ActualWidth / 2) * this.zoomControl.Zoom;
			this.zoomControl.TranslateY = (-y + this.zoomControl.ActualHeight / 2) * this.zoomControl.Zoom;
		}

		private void NavigateTo(float x, float y)
		{
			this.zoomControl.TranslateX = (-x + this.zoomControl.ActualWidth / 2) * this.zoomControl.Zoom;
			this.zoomControl.TranslateY = (-y + this.zoomControl.ActualHeight / 2) * this.zoomControl.Zoom;
		}

		private Task<TileHitTestInfo> FindPreviousTileAsync(ObjectInfoViewModel targetObjectType, TileHitTestInfo currentTileHitTestInfo = null)
		{
			return Task.Run<TileHitTestInfo>(() =>
			{
				return this.FindPreviousTile(targetObjectType, currentTileHitTestInfo);
			});
		}

		private TileHitTestInfo FindPreviousTile(ObjectInfoViewModel targetObjectType, TileHitTestInfo currentTileHitTestInfo = null)
		{
			Tile matchingTile = null;
			int tileX = 0;
			int tileY = 0;

			bool foundStartTile = false;

			int startX = this.viewModel.World.WorldWidthinTiles - 1;
			int startY = this.viewModel.World.WorldHeightinTiles - 1;

			if (currentTileHitTestInfo != null)
			{
				startX = currentTileHitTestInfo.X;
				startY = currentTileHitTestInfo.Y;
			}

			for (int x = startX; x > -1; x--)
			{
				for (int y = startY; y > -1; y--)
				{
					var tile = this.viewModel.World.Tiles[x, y];

					if (currentTileHitTestInfo != null && !foundStartTile)
					{
						if (tile == currentTileHitTestInfo.Tile)
							foundStartTile = true;

						continue;
					}

					if (currentTileHitTestInfo != null && tile == currentTileHitTestInfo.Tile)
						continue;

					if (this.viewModel.World.IsTileMatch(targetObjectType, x, y, tile, currentTileHitTestInfo))
					{
						tileX = x;
						tileY = y;
						matchingTile = tile;
						break;
					}
				}

				startY = this.viewModel.World.WorldHeightinTiles - 1;

				if (matchingTile != null)
					break;
			}

			if (matchingTile == null)
				return null;

			return new TileHitTestInfo(matchingTile, tileX, tileY);
		}

		private Task<TileHitTestInfo> FindNextTileAsync(ObjectInfoViewModel targetObjectType, TileHitTestInfo currentTileHitTestInfo = null)
		{
			return Task.Run<TileHitTestInfo>(() =>
			{
				return this.FindNextTile(targetObjectType, currentTileHitTestInfo);
			});
		}

		private TileHitTestInfo FindNextTile(ObjectInfoViewModel targetObjectType, TileHitTestInfo currentTileHitTestInfo = null)
		{
			Tile matchingTile = null;
			int tileX = 0;
			int tileY = 0;

			bool foundStartTile = false;

			int startX = 0;
			int startY = 0;

			if (currentTileHitTestInfo != null)
			{
				startX = currentTileHitTestInfo.X;
				startY = currentTileHitTestInfo.Y;
			}

			for (int x = startX; x < this.viewModel.World.WorldWidthinTiles; x++)
			{
				for (int y = startY; y < this.viewModel.World.WorldHeightinTiles; y++)
				{
					var tile = this.viewModel.World.Tiles[x, y];

					if (currentTileHitTestInfo != null && !foundStartTile)
					{
						if (tile == currentTileHitTestInfo.Tile)
							foundStartTile = true;

						continue;
					}

					if (currentTileHitTestInfo != null && tile == currentTileHitTestInfo.Tile)
						continue;

					if (this.viewModel.World.IsTileMatch(targetObjectType, x, y, tile, currentTileHitTestInfo))
					{
						tileX = x;
						tileY = y;
						matchingTile = tile;
						break;
					}
				}

				startY = 0;

				if (matchingTile != null)
					break;
			}

			if (matchingTile == null)
				return null;

			return new TileHitTestInfo(matchingTile, tileX, tileY);
		}

		private void ChooseTileInfo()
		{
			var viewModel = new MainWindowViewModel();
			viewModel.ObjectInfoViewModels = this.viewModel.ObjectInfoViewModels;
			viewModel.SelectedObjectInfoViewModel = this.viewModel.SelectedObjectInfoViewModel;

			var window = new TileSelectionWindow();
			window.Owner = this;
			window.DataContext = viewModel;

			var result = window.ShowDialog() ?? false;
			if (!result)
				return;

			this.viewModel.SelectedObjectInfoViewModel = viewModel.SelectedObjectInfoViewModel;
		}

		#region Event Handlers

		private async void OnSelectedTileInfoChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.viewModel.IsLoading)
				return;

			if (this.viewModel.IsHighlighting)
				await this.UpdateHighlight();
		}

		private void OnZoomControlMouseMove(object sender, MouseEventArgs e)
		{
			if (this.viewModel.IsLoading || !this.viewModel.IsLoaded)
				return;

			var world = this.viewModel.World;
			if (world == null)
				return;

			var position = e.GetPosition(this.Canvas);

			var x = (int)position.X;
			var y = (int)position.Y;

			string name = world.GetTileName(x, y);

			this.viewModel.TileName = name;

			if (string.IsNullOrEmpty(name))
				this.viewModel.Position = null;
			else
				this.viewModel.Position = string.Format("{0}, {1}", x, y);
		}

		private void OnZoomControlMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (this.viewModel.IsLoading || !this.viewModel.IsLoaded)
				return;

			var world = this.viewModel.World;
			if (world == null)
				return;

			var position = e.GetPosition(this.Canvas);

			var x = (int)position.X;
			var y = (int)position.Y;

			if (x < 0 || x >= this.viewModel.World.WorldWidthinTiles ||
				y < 0 || y >= this.viewModel.World.WorldHeightinTiles)
				return;

			//var tile = this.viewModel.World.Tiles[x, y];

			//if (!tile.IsActive)
			//	return;

			//var tileInfo = this.viewModel.World.TileInfoList[tile.Type];

			//if (tileInfo.Name != "Chest")
			//	return;

			//List<string> itemNames = new List<string>() { tileInfo.Name };

			var chest = this.viewModel.World.Chests.FirstOrDefault(c => (c.X == x || c.X + 1 == x) && (c.Y == y || c.Y + 1 == y));
			if (chest != null)
			{
				var itemNames = chest.Items.Select(i =>
					{
						if (i.Count > 1)
							return string.Concat(i.Name, " (", i.Count, ")");
						else
							return i.Name;
					}).ToArray();

				this.viewModel.CurrentChestItemNames = itemNames;

				this.popup.IsOpen = true;
			}

			var sign = this.viewModel.World.Signs.FirstOrDefault(c => (c.X == x || c.X + 1 == x) && (c.Y == y || c.Y + 1 == y));
			if (sign != null)
			{
				this.viewModel.CurrentChestItemNames = new string[] { sign.Text };
				this.popup.IsOpen = true;
			}
		}

		private async void OnOpenWorldFile(object sender, RoutedEventArgs e)
		{
			if (this.viewModel.IsLoading)
				return;

			var element = sender as FrameworkElement;
			if (element == null)
				return;

			var fileInfo = element.DataContext as FileInfo;
			if (fileInfo == null)
				return;

			await this.Open(fileInfo);
		}

		private async void OnSelectedWorldFileChanged(object sender, SelectionChangedEventArgs e)
		{
			if (ignoreSelectedWorldFileChanges || this.viewModel.IsLoading)
				return;

			await this.Open(this.viewModel.SelectedWorldFile);
		}

		private void OnWorldsSubmenuOpened(object sender, RoutedEventArgs e)
		{
			this.LoadWorldFiles();
		}

		private void OnWorldsDropDownOpened(object sender, EventArgs e)
		{
			this.LoadWorldFiles();
		}

		private async void OnIsHighlightingChanged(object sender, RoutedEventArgs e)
		{
			await this.UpdateHighlight();
		}

		private void OnNavigateToNpc(object sender, RoutedEventArgs e)
		{
			if (this.viewModel.IsLoading)
				return;

			var element = sender as FrameworkElement;
			if (element == null)
				return;

			var npc = element.DataContext as NPC;
			if (npc == null)
				return;

			this.NavigateToNpc(npc);
		}

		#endregion

		#region Command Event Handlers

		private void OnOpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (!this.viewModel.IsLoading)
				e.CanExecute = true;
		}

		private async void OnOpenExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			await this.Open();
		}


		private void OnSaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded)
				e.CanExecute = true;
		}

		private void OnSaveExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.Save();
		}


		private void OnRefreshCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded)
				e.CanExecute = true;
		}

		private async void OnRefreshExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			await this.Refresh();
		}


		private void OnZoomToOriginalCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded)
				e.CanExecute = true;
		}

		private void OnZoomToOriginalExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.zoomControl.Zoom = 1;
		}


		private void OnZoomToFitCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded)
				e.CanExecute = true;
		}

		private void OnZoomToFitExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.zoomControl.ZoomToFill();
		}


		private void OnDecreaseZoomCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded)
				e.CanExecute = true;
		}

		private void OnDecreaseZoomExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.zoomControl.ZoomOut();
		}


		private void OnIncreaseZoomCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded)
				e.CanExecute = true;
		}

		private void OnIncreaseZoomExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.zoomControl.ZoomIn();
		}


		private void OnFindCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			//if (this.viewModel.IsLoaded)
			e.CanExecute = true;
		}

		private void OnFindExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.ChooseTileInfo();
		}


		private void OnPreviousPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded && this.viewModel.SelectedObjectInfoViewModel != null)
				e.CanExecute = true;
		}

		private async void OnPreviousPageExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			await this.FindTile(SearchDirection.Backwards);
		}


		private void OnBrowseStopCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded && this.viewModel.CurrentTileHitTestInfo != null)
				e.CanExecute = true;
		}

		private void OnBrowseStopExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.ClearCurrentTileHit();
		}


		private void OnNextPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded && this.viewModel.SelectedObjectInfoViewModel != null)
				e.CanExecute = true;
		}

		private async void OnNextPageExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			await this.FindTile(SearchDirection.Forwards);
		}

		private void OnNavigateToSpawnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded)
				e.CanExecute = true;
		}

		private void OnNavigateToSpawnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.NavigateToSpawn();
		}

		private void OnNavigateToDungeonCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (this.viewModel.IsLoaded)
				e.CanExecute = true;
		}

		private void OnNavigateToDungeonExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.NavigateToDungeon();
		}

		private void OnCloseCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}

		#endregion
	}
}
