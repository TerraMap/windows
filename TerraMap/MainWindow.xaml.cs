using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
    #region Variables

    CollectionViewSource tileInfoViewSource;

    MainWindowViewModel viewModel = new MainWindowViewModel();
    private bool ignoreSelectedWorldFileChanges;
    private bool ignoreSelectedMapFileChanges;

    Storyboard indicatorStoryboard;
    Storyboard highlightStoryboard;

    int width, height, stride;
    byte[] pixels;
    PixelFormat pixelFormat = PixelFormats.Bgra32;

    byte[] maskPixels;
    byte[] fogPixels;

    bool isChoosingBlocks = false;

    bool allSpoilers = false;

    #endregion

    #region Constructors

    public MainWindow()
    {
      InitializeComponent();

      viewModel = (MainWindowViewModel)this.DataContext;
      viewModel.World = null;
      viewModel.IsLoading = false;
      viewModel.IsLoaded = false;
      viewModel.Status = null;
      viewModel.Position = null;
      viewModel.TileName = null;

      indicatorStoryboard = (Storyboard)this.FindResource("indicatorStoryboard");
      highlightStoryboard = (Storyboard)this.FindResource("highlightStoryboard");

      tileInfoViewSource = (CollectionViewSource)this.FindResource("tileInfoViewSource");
    }

    #endregion

    private void HandleException(Exception ex)
    {
      ExceptionWindow.ShowDialog(ex, this);
    }

    private void LoadWorldFiles()
    {
      try
      {
        ignoreSelectedWorldFileChanges = true;

        var currentWorldFile = this.viewModel.SelectedWorldFile;

        this.viewModel.WorldFiles.Clear();

        var path = this.GetWorldsPath();
        if (Directory.Exists(path))
        {
          foreach (var filename in Directory.GetFiles(path, "*.wld"))
          {
            string name = World.GetWorldName(filename);
            this.viewModel.WorldFiles.Add(new WorldFileViewModel() { FileInfo = new FileInfo(filename), Name = name });
          }
        }

        var modPath = this.GetModdedWorldsPath();
        if (Directory.Exists(modPath))
        {
          foreach (var filename in Directory.GetFiles(modPath, "*.wld"))
          {
            string name = World.GetWorldName(filename);
            this.viewModel.WorldFiles.Add(new WorldFileViewModel() { FileInfo = new FileInfo(filename), Name = String.Concat(name, " (MOD)") });
          }
        }

        var cloudPaths = GetCloudPaths();
        if (Directory.Exists(path))
        {
          foreach (var cloudPath in cloudPaths)
          {
            foreach (var filename in Directory.GetFiles(cloudPath, "*.wld"))
            {
              string name = World.GetWorldName(filename);
              this.viewModel.WorldFiles.Add(new WorldFileViewModel() { FileInfo = new FileInfo(filename), Name = name, Cloud = true });
            }
          }
        }

        this.viewModel.WorldFiles = new ObservableCollection<WorldFileViewModel>(this.viewModel.WorldFiles.OrderBy(w => w.Name));

        if (currentWorldFile != null)
        {
          currentWorldFile = this.viewModel.WorldFiles.FirstOrDefault(f => f.FileInfo.FullName == currentWorldFile.FileInfo.FullName);

          this.viewModel.SelectedWorldFile = currentWorldFile;
        }
      }
      finally
      {
        ignoreSelectedWorldFileChanges = false;
      }
    }

    private IEnumerable<string> GetCloudPaths()
    {
      List<string> cloudPaths = new List<string>();

      string userdataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam");

      try
      {
        using (var HKLM = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
        {
          using (var steamKey = HKLM.OpenSubKey("SOFTWARE\\Valve\\Steam"))
          {
            userdataPath = (string)steamKey.GetValue("InstallPath", userdataPath);
          }
        }
      }
      catch (Exception)
      {
      }

      try
      {
        userdataPath = Path.Combine(userdataPath, "userdata");

        if (Directory.Exists(userdataPath))
        {
          foreach (var userDir in Directory.GetDirectories(userdataPath))
          {
            // Each user could have a Terraria directory
            var cloudPath = Path.Combine(userDir, "105600");
            cloudPath = Path.Combine(cloudPath, "remote");
            cloudPath = Path.Combine(cloudPath, "worlds");

            if (!Directory.Exists(cloudPath))
              continue;

            cloudPaths.Add(cloudPath);
          }
        }
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }

      return cloudPaths;
    }

    private void LoadMapFiles()
    {
      try
      {
        ignoreSelectedMapFileChanges = true;

        var currentMapFile = this.viewModel.SelectedMapFile;

        this.viewModel.MapFiles.Clear();

        this.viewModel.MapFiles.Add(new MapFileViewModel() { FileInfo = null, Name = "(No Spoilers)" });

        if (this.viewModel.World == null || this.viewModel.SelectedWorldFile == null)
        {
          this.viewModel.MapFiles.Add(new MapFileViewModel() { FileInfo = null, Name = "(All Spoilers)" });

          this.viewModel.SelectedMapFile = this.viewModel.MapFiles.FirstOrDefault();

          return;
        }

        foreach (var mapFile in this.viewModel.World.GetPlayerMapFiles())
        {
          this.viewModel.MapFiles.Add(mapFile);
        }

        this.viewModel.MapFiles.Add(new MapFileViewModel() { FileInfo = null, Name = "(All Spoilers)" });

        if (currentMapFile != null)
        {
          currentMapFile = this.viewModel.MapFiles.FirstOrDefault(f => f.Name == currentMapFile.Name);

          this.viewModel.SelectedMapFile = currentMapFile;
        }

        if (this.viewModel.SelectedMapFile == null)
          this.viewModel.SelectedMapFile = this.viewModel.MapFiles.First();
      }
      finally
      {
        ignoreSelectedMapFileChanges = false;
      }
    }

    private string GetWorldsPath()
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

      path = Path.Combine(path, "My Games");
      path = Path.Combine(path, "Terraria");
      path = Path.Combine(path, "Worlds");
      return path;
    }

    private string GetModdedWorldsPath()
    {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

      path = Path.Combine(path, "My Games");
      path = Path.Combine(path, "Terraria");
      path = Path.Combine(path, "ModLoader");
      path = Path.Combine(path, "Worlds");
      return path;
    }

    private async Task Open()
    {
      string path = this.GetWorldsPath();

      OpenFileDialog dialog = new OpenFileDialog
      {
        Filter = "World Files (*.wld)|*.wld|World Backup Files (*.bak)|*.*.bak|All Files (*.*)|*.*"
      };

      if (Directory.Exists(path))
        dialog.InitialDirectory = path;

      var result = dialog.ShowDialog() ?? false;
      if (!result)
        return;

      await this.Open(dialog.FileName);
    }

    private async Task Open(FileInfo fileInfo)
    {
      await this.Open(fileInfo.FullName);
    }

    private async Task Open(string filename)
    {
      var start = DateTime.Now;

      this.viewModel.TileName = null;

      this.viewModel.SelectedWorldFile = this.viewModel.WorldFiles.FirstOrDefault(w => w.FileInfo.FullName == filename);

      this.Title = "World Info - " + filename;

      this.viewModel.Filename = filename;

      var world = new World
      {
        StaticData = this.viewModel.StaticData
      };

      this.viewModel.World = world;

      this.viewModel.NPCs.Clear();

      this.viewModel.InstructionsVisibility = Visibility.Collapsed;

      this.viewModel.ProgressValue = 0;
      this.viewModel.BeginLoading("Reading world...");
      try
      {
        var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Background, this.OnDispatcherTimerTick, this.Dispatcher);
        timer.Start();

        await world.ReadAsync(filename);

        this.Title = "TerraMap - " + world.Name;

        this.viewModel.TotalTileCount = world.TotalTileCount;

        this.Grid.Width = world.WorldWidthinTiles;
        this.Grid.Height = world.WorldHeightinTiles;

        this.Canvas.Width = world.WorldWidthinTiles;
        this.Canvas.Height = world.WorldHeightinTiles;

        this.LoadMapFiles();

        this.LoadMapFile();

        this.viewModel.IsLoaded = true;

        world.Status = "Drawing map";

        width = world.WorldWidthinTiles;
        height = world.WorldHeightinTiles;
        stride = (width * pixelFormat.BitsPerPixel + 7) / 8;
        pixels = new byte[stride * height];
        maskPixels = new byte[stride * height];
        fogPixels = new byte[stride * height];

        viewModel.WriteableBitmap = new WriteableBitmap(width, height, 96, 96, pixelFormat, null);
        viewModel.WriteableBitmapMask = new WriteableBitmap(width, height, 96, 96, pixelFormat, null);
        viewModel.WriteableBitmapFog = new WriteableBitmap(width, height, 96, 96, pixelFormat, null);

        allSpoilers = this.viewModel.SelectedMapFile.Name == "(All Spoilers)";

        await world.WritePixelDataAsync(fogPixels, stride, fog: true, allSpoilers: allSpoilers);

        await world.WritePixelDataAsync(pixels, stride);

        var selectedObjectInfoViewModels = this.viewModel.ObjectInfoViewModels.Where(v => v.IsSelected).ToArray();

        if (this.viewModel.IsHighlighting)
          await world.WritePixelDataAsync(maskPixels, stride, selectedObjectInfoViewModels);
        else
          await world.WritePixelDataAsync(maskPixels, stride);

        foreach (var npc in world.NPCs.OrderBy(n => n.Type))
          viewModel.NPCs.Add(npc);

        timer.Stop();

        this.Update();

        world.Status = "";

        if (this.viewModel.SelectedMapFile == null || this.viewModel.SelectedMapFile.Name == "(No Spoilers)")
        {
          this.viewModel.Instructions = "Please select a player or (All Spoilers) from the Players list.";
          this.viewModel.InstructionsVisibility = Visibility.Visible;
        }

        var elapsed = DateTime.Now - start;

        if (this.viewModel.IsHighlighting)
          world.Status = string.Format(
            "Loaded and highlighted {0:N0} out of {1:N0} blocks ({2:P0}) in {3:N1} seconds",
            this.viewModel.HighlightedTileCount,
            this.viewModel.TotalTileCount,
            (float)this.viewModel.HighlightedTileCount / (float)this.viewModel.TotalTileCount,
            elapsed.TotalSeconds);
        else
          world.Status = world.Status = string.Format("Loaded {0:N0} blocks in {1:N1} seconds", this.viewModel.TotalTileCount, elapsed.TotalSeconds);
      }
      finally
      {
        this.viewModel.EndLoading();
      }
    }

    private async Task OpenPlayerMapFile()
    {
      OpenFileDialog dialog = new OpenFileDialog
      {
        Filter = "Player Map Files (*.map)|*.map|All Files (*.*)|*.*"
      };

      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games\\Terraria\\Players");

      if (Directory.Exists(path))
        dialog.InitialDirectory = path;

      var result = dialog.ShowDialog() ?? false;
      if (!result)
        return;

      await this.OpenPlayerMapFile(dialog.FileName);
    }

    private async Task OpenPlayerMapFile(string filename)
    {
      await this.OpenPlayerMapFile(new FileInfo(filename));
    }

    private async Task OpenPlayerMapFile(FileInfo fileInfo)
    {
      this.viewModel.SelectedMapFile = new MapFileViewModel() { FileInfo = fileInfo, Name = fileInfo.Name };
      this.LoadMapFile();
      await this.UpdateFog();
    }

    private void LoadMapFile()
    {
      MapHelper.ResetTileLight();

      if (this.viewModel.SelectedMapFile == null || this.viewModel.SelectedMapFile.Name == "(No Spoilers)")
      {
        this.viewModel.Instructions = "Please select a player or (All Spoilers) from the Players list.";
        this.viewModel.InstructionsVisibility = Visibility.Visible;
      }
      else
      {
        this.viewModel.InstructionsVisibility = Visibility.Collapsed;
      }

      if (this.viewModel.SelectedMapFile.FileInfo != null)
      {
        allSpoilers = this.viewModel.SelectedMapFile.Name == "(All Spoilers)";

        try
        {
          using (var stream = new FileStream(this.viewModel.SelectedMapFile.FileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
            using (var reader = new BinaryReader(stream))
            {
              MapHelper.LoadMapVersion2(reader, this.viewModel.World);
            }
          }
        }
        catch (Exception) { }
      }
    }

    private async Task Refresh()
    {
      await this.Open(this.viewModel.Filename);
    }

    private void ShowProperties()
    {
      new WorldPropertiesWindow() { DataContext = this.viewModel.World, Owner = this }.Show();
    }

    private async Task UpdateHighlight()
    {
      if (this.viewModel.IsLoading || this.isChoosingBlocks)
        return;

      var world = this.viewModel.World;
      if (world == null)
        return;

      //highlightStoryboard.Stop();

      var start = DateTime.Now;

      this.viewModel.BeginLoading("Updating map");
      this.viewModel.ProgressValue = 0;

      var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(20), DispatcherPriority.Background, this.OnDispatcherTimerTick, this.Dispatcher);
      timer.Start();

      var selectedObjectInfoViewModels = this.viewModel.ObjectInfoViewModels.Where(v => v.IsSelected).ToArray();

      if (this.viewModel.IsHighlighting)
        await world.WritePixelDataAsync(maskPixels, stride, selectedObjectInfoViewModels);
      else
        await world.WritePixelDataAsync(maskPixels, stride);

      timer.Stop();

      this.Update();

      world.Status = "";

      var elapsed = DateTime.Now - start;

      if (this.viewModel.IsHighlighting)
        world.Status = string.Format(
          "Highlighted {0:N0} out of {1:N0} blocks ({2:P0}) in {3:N1} seconds",
          this.viewModel.HighlightedTileCount,
          this.viewModel.TotalTileCount,
          (float)this.viewModel.HighlightedTileCount / (float)this.viewModel.TotalTileCount,
          elapsed.TotalSeconds);
      else
        world.Status = string.Format("Updated {0:N0} blocks in {1:N1} seconds", this.viewModel.TotalTileCount, elapsed.TotalSeconds);

      //if (this.viewModel.IsHighlighting)
      //	highlightStoryboard.Begin();

      this.viewModel.EndLoading();
    }

    private async Task UpdateFog()
    {
      if (this.viewModel.IsLoading || this.isChoosingBlocks)
        return;

      var world = this.viewModel.World;
      if (world == null)
        return;

      var start = DateTime.Now;

      this.viewModel.BeginLoading("Updating map");
      this.viewModel.ProgressValue = 0;

      var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(20), DispatcherPriority.Background, this.OnDispatcherTimerTick, this.Dispatcher);
      timer.Start();

      allSpoilers = this.viewModel.SelectedMapFile.Name == "(All Spoilers)";

      await world.WritePixelDataAsync(fogPixels, stride, fog: true, allSpoilers: allSpoilers);

      timer.Stop();

      this.Update();

      world.Status = "";

      var elapsed = DateTime.Now - start;

      world.Status = string.Format("Updated fog for {0:N0} blocks in {1:N1} seconds", this.viewModel.TotalTileCount, elapsed.TotalSeconds);

      this.viewModel.EndLoading();
    }

    private void Update()
    {
      this.viewModel.Status = viewModel.World.Status;
      this.viewModel.ProgressMaximum = viewModel.World.ProgressMaximum;
      this.viewModel.ProgressValue = viewModel.World.ProgressValue;
      this.viewModel.HighlightedTileCount = viewModel.World.HighlightedTileCount;

      while (viewModel.World.UpdatedRectangles.TryDequeue(out Int32Rect rect))
      {
        var offset = rect.Y * width * 4;

        viewModel.WriteableBitmapFog.WritePixels(rect, fogPixels, stride, offset);
        viewModel.WriteableBitmap.WritePixels(rect, pixels, stride, offset);
        viewModel.WriteableBitmapMask.WritePixels(rect, maskPixels, stride, offset);
      }
    }

    private void UpdateAll()
    {
      var rect = new Int32Rect(0, 0, this.width, this.height);
      viewModel.WriteableBitmap.WritePixels(rect, pixels, stride, 0);
    }

    private void Save()
    {
      SaveFileDialog dialog = new SaveFileDialog
      {
        Filter = "PNG Images (*.png)|*.png",
        Title = this.viewModel.World.Name
      };
      var result = dialog.ShowDialog() ?? false;
      if (!result)
        return;

      this.Save(dialog.FileName);
    }

    private void Save(string filename)
    {
      FileInfo fileInfo = new FileInfo(filename);
      if (fileInfo.Exists && fileInfo.Extension == ".wld")
      {
        MessageBox.Show(this, "I'm sorry, Dave. I'm afraid I can't do that.\r\n\r\nYou don't want me overwriting your .wld file.  Please make sure to specify a filename that ends with png.  :)", "TerraMap");
        return;
      }

      var bitmap = this.viewModel.WriteableBitmap;
      var mask = this.viewModel.WriteableBitmapMask;
      var fog = this.viewModel.WriteableBitmapFog;

      var output = BitmapFactory.New(bitmap.PixelWidth, bitmap.PixelHeight);

      var rect = new Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
      var point = new Point(0, 0);

      output.Blit(rect, bitmap, rect);

      byte alpha = (byte)(255 * this.viewModel.MaskOpacity);

      mask.ForEach((x, y, color) =>
      {
        if (color == Colors.Black)
          return Color.FromArgb(alpha, color.R, color.G, color.B);
        else
          return color;
      });

      output.Blit(rect, mask, rect, WriteableBitmapExtensions.BlendMode.Alpha);

      output.Blit(rect, fog, rect, WriteableBitmapExtensions.BlendMode.Alpha);

      using (var stream = new FileStream(filename, FileMode.Create))
      {
        var encoder = new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(output));
        encoder.Save(stream);
        stream.Close();
      }
    }

    private async Task ExportHighlightedTilePositions()
    {
      SaveFileDialog dialog = new SaveFileDialog
      {
        Filter = "Tab-Delimited Text Files (*.txt)|*.txt|Comma Separated Values Files (*.csv)|*.csv"
      };
      var result = dialog.ShowDialog() ?? false;
      if (!result)
        return;

      await this.ExportHighlightedTilePositions(dialog.FileName);
    }

    private async Task ExportHighlightedTilePositions(string filename)
    {
      FileInfo fileInfo = new FileInfo(filename);
      if (fileInfo.Exists && fileInfo.Extension == ".wld")
      {
        MessageBox.Show(this, "I'm sorry, Dave. I'm afraid I can't do that.\r\n\r\nYou don't want me overwriting your .wld file.  Please make sure to specify a filename that ends with txt or csv.  :)", "TerraMap");
        return;
      }

      var start = DateTime.Now;

      var selectedObjectInfoViewModels = this.viewModel.ObjectInfoViewModels.Where(v => v.IsSelected).ToArray();

      allSpoilers = this.viewModel.SelectedMapFile.Name == "(All Spoilers)";

      await this.viewModel.World.WriteHighlightedTilePositionsAsync(filename, selectedObjectInfoViewModels, true, allSpoilers);

      var elapsed = DateTime.Now - start;

      this.viewModel.World.Status = string.Format(
        "Exported {0:N0} out of {1:N0} blocks ({2:P0}) in {3:N1} seconds",
        this.viewModel.HighlightedTileCount,
        this.viewModel.TotalTileCount,
        (float)this.viewModel.HighlightedTileCount / (float)this.viewModel.TotalTileCount,
        elapsed.TotalSeconds);
    }

    private void ClearCurrentTileHit()
    {
      this.viewModel.CurrentTileHitTestInfo = null;

      this.Indicator.Visibility = Visibility.Collapsed;
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

    private async Task FindTile(SearchDirection direction)
    {
      this.viewModel.BeginLoading("Finding tile...");

      TileHitTestInfo tileHitTestInfo = null;

      var selectedObjectInfoViewModels = this.viewModel.ObjectInfoViewModels.Where(v => v.IsSelected).ToArray();

      if (direction == SearchDirection.Forwards)
      {
        tileHitTestInfo = await this.FindNextTileAsync(selectedObjectInfoViewModels, start: this.viewModel.CurrentTileHitTestInfo);
        if (tileHitTestInfo == null && this.viewModel.CurrentTileHitTestInfo != null)
          tileHitTestInfo = await this.FindNextTileAsync(selectedObjectInfoViewModels, end: this.viewModel.CurrentTileHitTestInfo);
      }
      else
      {
        tileHitTestInfo = await this.FindPreviousTileAsync(selectedObjectInfoViewModels, start: this.viewModel.CurrentTileHitTestInfo);
        if (tileHitTestInfo == null && this.viewModel.CurrentTileHitTestInfo != null)
          tileHitTestInfo = await this.FindPreviousTileAsync(selectedObjectInfoViewModels, end: this.viewModel.CurrentTileHitTestInfo);
      }

      this.viewModel.CurrentTileHitTestInfo = tileHitTestInfo;

      this.viewModel.EndLoading();

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

    private Task<TileHitTestInfo> FindPreviousTileAsync(ObjectInfoViewModel[] targetObjectTypes, TileHitTestInfo start = null, TileHitTestInfo end = null)
    {
      return Task.Factory.StartNew<TileHitTestInfo>(() =>
      {
        return this.FindPreviousTile(targetObjectTypes, start, end);
      });
    }

    private TileHitTestInfo FindPreviousTile(ObjectInfoViewModel[] targetObjectTypes, TileHitTestInfo start = null, TileHitTestInfo end = null)
    {
      Tile matchingTile = null;
      int tileX = 0;
      int tileY = 0;

      bool foundStartTile = false;

      int startX = this.viewModel.World.WorldWidthinTiles - 1;
      int startY = this.viewModel.World.WorldHeightinTiles - 1;

      if (start != null)
      {
        startX = start.X;
        startY = start.Y;
      }

      this.viewModel.ProgressMaximum = this.viewModel.TotalTileCount;
      this.viewModel.ProgressValue = this.viewModel.World.WorldHeightinTiles * (startX - 1) + startY;

      for (int x = startX; x > -1; x--)
      {
        for (int y = startY; y > -1; y--)
        {
          if (end != null && end.X == x && end.Y == y)
            return null;

          var tile = this.viewModel.World.Tiles[x, y];

          if (start != null && !foundStartTile)
          {
            if (tile == start.Tile)
              foundStartTile = true;

            continue;
          }

          if (start != null && tile == start.Tile)
            continue;

          if (this.viewModel.World.IsTileMatch(targetObjectTypes, x, y, tile, start))
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

        this.viewModel.ProgressValue -= this.viewModel.World.WorldHeightinTiles;
      }

      if (matchingTile == null)
        return null;

      return new TileHitTestInfo(matchingTile, tileX, tileY);
    }

    private Task<TileHitTestInfo> FindNextTileAsync(ObjectInfoViewModel[] targetObjectTypes, TileHitTestInfo start = null, TileHitTestInfo end = null)
    {
      return Task.Factory.StartNew<TileHitTestInfo>(() =>
      {
        return this.FindNextTile(targetObjectTypes, start, end);
      });
    }

    private TileHitTestInfo FindNextTile(ObjectInfoViewModel[] targetObjectTypes, TileHitTestInfo start = null, TileHitTestInfo end = null)
    {
      Tile matchingTile = null;
      int tileX = 0;
      int tileY = 0;

      bool foundStartTile = false;

      int startX = 0;
      int startY = 0;

      if (start != null)
      {
        startX = start.X;
        startY = start.Y;
      }

      this.viewModel.ProgressMaximum = this.viewModel.TotalTileCount;
      this.viewModel.ProgressValue = this.viewModel.World.WorldHeightinTiles * (startX - 1) + startY;

      for (int x = startX; x < this.viewModel.World.WorldWidthinTiles; x++)
      {
        for (int y = startY; y < this.viewModel.World.WorldHeightinTiles; y++)
        {
          if (end != null && end.X == x && end.Y == y)
            return null;

          var tile = this.viewModel.World.Tiles[x, y];

          if (start != null && !foundStartTile)
          {
            if (tile == start.Tile)
              foundStartTile = true;

            continue;
          }

          if (start != null && tile == start.Tile)
            continue;

          if (this.viewModel.World.IsTileMatch(targetObjectTypes, x, y, tile, start))
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

        this.viewModel.ProgressValue += this.viewModel.World.WorldHeightinTiles;

        this.viewModel.World.Status = string.Format("Checking tile {0:N0} of {1:N0} ({2:P0})...", this.viewModel.ProgressValue, this.viewModel.ProgressMaximum, (float)this.viewModel.ProgressValue / (float)this.viewModel.ProgressMaximum);
      }

      if (matchingTile == null)
        return null;

      return new TileHitTestInfo(matchingTile, tileX, tileY);
    }

    private async Task ChooseTileInfo()
    {
      try
      {
        this.isChoosingBlocks = true;

        var newViewModel = new MainWindowViewModel
        {
          ObjectInfoViewModels = this.viewModel.ObjectInfoViewModels
        };

        var window = new TileSelectionWindow(this)
        {
          DataContext = newViewModel
        };

        var result = window.ShowDialog() ?? false;
        if (!result)
        {
          this.isChoosingBlocks = false;

          return;
        }

        this.isChoosingBlocks = false;

        if (this.viewModel.IsHighlighting)
          await this.UpdateHighlight();
      }
      finally
      {
        this.isChoosingBlocks = false;
      }
    }

    private void CheckForUpdates(bool isUserInitiated = false)
    {
      try
      {
        this.viewModel.IsCheckingForUpdate = true;
        this.viewModel.UpdateVisibility = Visibility.Collapsed;
        this.viewModel.NewRelease = null;

        var updatesUrl = @"https://api.github.com/repos/terramap/windows/releases/latest";
        var updatesUri = new Uri(updatesUrl);

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var webClient = new WebClient();
        webClient.Headers.Add("user-agent", "Mozilla/5.0");
        webClient.OpenReadCompleted += OnCheckForUpdatesComplete;
        webClient.OpenReadAsync(updatesUri, isUserInitiated);
      }
      catch (Exception ex)
      {
        if (isUserInitiated)
        {
          ExceptionWindow.ShowDialog(new Exception("There was a problem checking for updates", ex));
        }
        else
        {
          Debug.WriteLine(ex);
        }
      }
    }

    private void SetIsSelected(bool isSelected)
    {
      if (tileInfoViewSource == null || tileInfoViewSource.View == null)
        return;

      foreach (var item in tileInfoViewSource.View.OfType<ObjectInfoViewModel>())
      {
        item.IsSelected = isSelected;
      }
    }

    private async Task HighlightSet(ObjectInfoSetViewModel objectInfoSetViewModel)
    {
      this.SetIsSelected(false);

      objectInfoSetViewModel.IsSelected = true;

      this.CheckObjectInfoSet(objectInfoSetViewModel);

      this.viewModel.IsHighlighting = true;

      await this.UpdateHighlight();
    }

    #region Event Handlers

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.CheckForUpdates();

      this.LoadWorldFiles();
      this.LoadMapFiles();

      var staticDataFileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

      string staticDataFilename = Path.Combine(staticDataFileInfo.DirectoryName, "tiles.xml");

      var staticData = await StaticData.ReadAsync(staticDataFilename);

      this.viewModel.StaticData = staticData;

      this.viewModel.ObjectInfoViewModels = new ObservableCollection<ObjectInfoViewModel>();

      // load item viewmodels
      foreach (var itemInfo in staticData.ItemInfos.Values.Where(i => !string.IsNullOrEmpty(i.Name)))
        this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { ItemInfo = itemInfo, Name = itemInfo.Name });

      // load tile viewmodels
      foreach (var tileInfo in staticData.TileInfos)
      {
        if (!string.IsNullOrEmpty(tileInfo.Name))
        {
          this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { TileInfo = tileInfo, Name = tileInfo.Name });
        }

        foreach (var variant in tileInfo.Variants)
        {
          string name = variant.Name;

          var variantViewModel = new ObjectInfoViewModel() { TileInfo = variant, Name = name };

          var existingVariantViewModel = this.viewModel.ObjectInfoViewModels.FirstOrDefault(v => v.Name == variant.Name && v.Type == "Tile");

          if (existingVariantViewModel != null)
          {
            if (name != tileInfo.Name)
              variantViewModel.ParentName = tileInfo.Name;
          }

          if (!string.IsNullOrEmpty(name))
          {
            this.viewModel.ObjectInfoViewModels.Add(variantViewModel);
          }
        }
      }

      this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { Name = "Red Wire", IsRedWire = true });
      this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { Name = "Green Wire", IsGreenWire = true });
      this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { Name = "Blue Wire", IsBlueWire = true });
      this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { Name = "Yellow Wire", IsYellowWire = true });

      // load wall viewmodels
      foreach (var wallInfo in staticData.WallInfos)
      {
        var existingObjectInfo = this.viewModel.ObjectInfoViewModels.FirstOrDefault(v => v.Name == wallInfo.Name && v.Type == "Wall");

        if (existingObjectInfo == null)
        {
          if (!string.IsNullOrEmpty(wallInfo.Name))
          {
            this.viewModel.ObjectInfoViewModels.Add(new ObjectInfoViewModel() { WallInfo = wallInfo, Name = wallInfo.Name });
          }
        }
        else
        {
          existingObjectInfo.WallInfo = wallInfo;
        }
      }

      // load tile sets
      string tileSetsFilename = Path.Combine(staticDataFileInfo.DirectoryName, "sets.xml");

      viewModel.SetsFilename = tileSetsFilename;

      // if the user has not created their own customized sets.xml file, load the default one we ship with
      if (!System.IO.File.Exists(tileSetsFilename))
        tileSetsFilename = Path.Combine(staticDataFileInfo.DirectoryName, "sets-default.xml");

      viewModel.Sets = new ObservableCollection<ObjectInfoSetViewModel>();

      var sets = await Set.ReadAsync(tileSetsFilename);

      int number = 1;

      foreach (var set in sets)
      {
        var setViewModel = new ObjectInfoSetViewModel() { Name = set.Name };

        foreach (var tileInfo in set.TileInfos)
        {
          if (string.IsNullOrWhiteSpace(tileInfo.Name))
          {
            foreach (var objectInfo in this.viewModel.ObjectInfoViewModels.Where(o => o.TileInfo != null && o.TileInfo.Id == tileInfo.Id))
              setViewModel.ObjectInfoViewModels.Add(objectInfo);
          }
          else
          {
            foreach (var objectInfo in this.viewModel.ObjectInfoViewModels.Where(o => o.TileInfo != null && o.TileInfo.Name == tileInfo.Name))
              setViewModel.ObjectInfoViewModels.Add(objectInfo);
          }
        }

        foreach (var wallInfo in set.WallInfos)
        {
          if (string.IsNullOrWhiteSpace(wallInfo.Name))
          {
            foreach (var objectInfo in this.viewModel.ObjectInfoViewModels.Where(o => o.WallInfo.Id == wallInfo.Id))
              setViewModel.ObjectInfoViewModels.Add(objectInfo);
          }
          else
          {
            foreach (var objectInfo in this.viewModel.ObjectInfoViewModels.Where(o => o.WallInfo.Name == wallInfo.Name))
              setViewModel.ObjectInfoViewModels.Add(objectInfo);
          }
        }

        foreach (var itemInfo in set.ItemInfos)
        {
          if (string.IsNullOrWhiteSpace(itemInfo.Name))
          {
            foreach (var objectInfo in this.viewModel.ObjectInfoViewModels.Where(o => o.ItemInfo != null && o.ItemInfo.Id == itemInfo.Id))
              setViewModel.ObjectInfoViewModels.Add(objectInfo);
          }
          else
          {
            foreach (var objectInfo in this.viewModel.ObjectInfoViewModels.Where(o => o.ItemInfo != null && o.ItemInfo.Name == itemInfo.Name))
              setViewModel.ObjectInfoViewModels.Add(objectInfo);
          }
        }

        this.viewModel.ObjectInfoViewModels.Add(setViewModel);

        if (number < 10)
          setViewModel.InputGestureText = "Ctrl+" + number;

        this.viewModel.Sets.Add(setViewModel);

        number++;
      }

      if (App.Current.Properties["Args"] is string[] args && args.Length > 0)
      {
        string filename = args[0];

        var uri = new Uri(filename);
        filename = uri.LocalPath;

        await this.Open(filename);
      }
    }

    private void OnDispatcherTimerTick(object sender, EventArgs e)
    {
      this.Update();
    }

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

      if (x < 0 || x >= this.viewModel.World.WorldWidthinTiles ||
          y < 0 || y >= this.viewModel.World.WorldHeightinTiles)
        return;

      string name = world.GetTileName(x, y);
      this.viewModel.HoverInfo = world.GetTileHitTestInfo(x, y);

      if (!allSpoilers && !MapHelper.IsTileLit(x, y))
        name = "(No spoilers)";

      this.viewModel.TileName = name;

      this.viewModel.Position = world.GetPosition(x);
      this.viewModel.Depth = world.GetDepth(y);

      this.viewModel.Coordinates = string.Format("{0}, {1}", x, y);
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

      if (!allSpoilers && !MapHelper.IsTileLit(x, y))
        return;

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

      var tile = this.viewModel.World.Tiles[x, y];

      var tileHitTestInfo = new TileHitTestInfo(tile, x, y);

      this.viewModel.CurrentTileHitTestInfo = tileHitTestInfo;

      this.Indicator.Visibility = Visibility.Visible;
      Canvas.SetLeft(this.Indicator, tileHitTestInfo.X - 1);
      Canvas.SetTop(this.Indicator, tileHitTestInfo.Y - 1);
    }

    private async void OnOpenWorldFile(object sender, RoutedEventArgs e)
    {
      if (this.viewModel.IsLoading)
        return;

      if (!(sender is FrameworkElement element))
        return;

      if (!(element.DataContext is WorldFileViewModel worldFile))
        return;

      await this.Open(worldFile.FileInfo);
    }

    private void OnOpenMapFile(object sender, RoutedEventArgs e)
    {
      if (this.viewModel.IsLoading)
        return;

      if (!(sender is FrameworkElement element))
        return;

      if (!(element.DataContext is MapFileViewModel mapFile))
        return;

      this.viewModel.SelectedMapFile = mapFile;
    }

    private async void OnSelectedWorldFileChanged(object sender, SelectionChangedEventArgs e)
    {
      if (ignoreSelectedWorldFileChanges || this.viewModel.IsLoading || this.viewModel.SelectedWorldFile == null)
        return;

      await this.Open(this.viewModel.SelectedWorldFile.FileInfo);
    }

    private async void OnSelectedMapFileChanged(object sender, SelectionChangedEventArgs e)
    {
      if (ignoreSelectedMapFileChanges || this.viewModel.IsLoading || this.viewModel.SelectedMapFile == null)
        return;

      this.LoadMapFile();

      await this.UpdateFog();
    }

    private void OnWorldsSubmenuOpened(object sender, RoutedEventArgs e)
    {
      this.LoadWorldFiles();
    }

    private void OnWorldsDropDownOpened(object sender, EventArgs e)
    {
      this.LoadWorldFiles();
    }

    private void OnMapsDropDownOpened(object sender, EventArgs e)
    {
      this.LoadMapFiles();
    }

    private async void OnToggleIsHighlighting(object sender, RoutedEventArgs e)
    {
      this.viewModel.IsHighlighting = !this.viewModel.IsHighlighting;

      await this.UpdateHighlight();
    }

    private async void OnToggleInvertHighlight(object sender, RoutedEventArgs e)
    {
      if (this.viewModel.World != null)
        this.viewModel.World.InvertHighlight = !this.viewModel.World.InvertHighlight;

      await this.UpdateHighlight();
    }

    private async void OnIsHighlightingChanged(object sender, RoutedEventArgs e)
    {
      await this.UpdateHighlight();
    }

    private void OnNavigateToNpc(object sender, RoutedEventArgs e)
    {
      if (this.viewModel.IsLoading)
        return;

      if (!(sender is FrameworkElement element))
        return;

      if (!(element.DataContext is NPC npc))
        return;

      this.NavigateToNpc(npc);
    }

    private void OnCheckForUpdates(object sender, RoutedEventArgs e)
    {
      this.CheckForUpdates(true);
    }

    private void OnShowAboutWindow(object sender, RoutedEventArgs e)
    {
      new AboutWindow() { Owner = this }.ShowDialog();
    }

    private void OnUpdateClicked(object sender, RoutedEventArgs e)
    {
      if (this.viewModel.NewRelease == null)
        return;

      Process.Start(this.viewModel.NewRelease.Url);
    }

    private void OnCheckForUpdatesComplete(object sender, OpenReadCompletedEventArgs e)
    {
      var isUserInitiated = false;

      try
      {
        if (e.UserState is bool)
          isUserInitiated = (bool)e.UserState;

        if (e.Cancelled)
          return;

        if (e.Error != null)
        {
          if (isUserInitiated)
          {
            ExceptionWindow.ShowDialog(new Exception("There was a problem checking for updates", e.Error));
          }
          else
          {
            Debug.WriteLine(e.Error);
          }

          return;
        }

        var releases = new List<ReleaseInfo>();

        using (var stream = e.Result)
        {
          releases = ReleaseInfo.FromJsonStream(stream);
        }

        var currentVersion = Assembly.GetEntryAssembly().GetName().Version;

        if (releases == null || releases.Count < 1)
        {
          if (isUserInitiated)
            MessageBox.Show("No updates found.", "TerraMap Updates", MessageBoxButton.OK, MessageBoxImage.None);

          return;
        }

        var newRelease = ReleaseInfo.GetLatest(releases, ReleaseStatus.Alpha);

        if (newRelease.Version > currentVersion)
        {
          this.viewModel.UpdateVisibility = Visibility.Visible;
          this.viewModel.NewRelease = newRelease;
        }
        else if (isUserInitiated)
        {
          MessageBox.Show(string.Format("Version {0}\r\n\r\nTerraMap is up to date.", currentVersion), "TerraMap Updates", MessageBoxButton.OK, MessageBoxImage.None);
        }
      }
      catch (Exception ex)
      {
        if (isUserInitiated)
        {
          ExceptionWindow.ShowDialog(new Exception("There was a problem checking for updates", ex));
        }
        else
        {
          Debug.WriteLine(ex);
        }
      }
      finally
      {
        this.viewModel.IsCheckingForUpdate = false;
      }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
      this.tileInfoViewSource.View.Refresh();
      this.tileInfoViewSource.View.MoveCurrentToFirst();
    }

    private void OnFilter(object sender, FilterEventArgs e)
    {
      e.Accepted = false;

      if (!(e.Item is ObjectInfoViewModel tileInfo))
        return;

      var searchText = this.searchBox.Text.ToLower();

      if (tileInfo.Name.ToLower().Contains(searchText))
      {
        e.Accepted = true;
        return;
      }

      if (tileInfo.Type.ToLower().Contains(searchText))
      {
        e.Accepted = true;
        return;
      }

      if (!string.IsNullOrWhiteSpace(tileInfo.ParentName) && tileInfo.ParentName.ToLower().Contains(searchText))
      {
        e.Accepted = true;
        return;
      }
    }

    private void OnCheckAll(object sender, RoutedEventArgs e)
    {
      this.SetIsSelected(true);
    }

    private void OnUncheckAll(object sender, RoutedEventArgs e)
    {
      this.SetIsSelected(false);
    }

    private void OnItemChecked(object sender, RoutedEventArgs e)
    {
      if (!(sender is CheckBox checkBox))
        return;

      if (!(checkBox.DataContext is ObjectInfoSetViewModel objectInfoSetViewModel))
        return;

      this.CheckObjectInfoSet(objectInfoSetViewModel);
    }

    private void CheckObjectInfoSet(ObjectInfoSetViewModel objectInfoSetViewModel)
    {
      if (objectInfoSetViewModel == null)
        return;

      foreach (var objectInfoViewModel in objectInfoSetViewModel.ObjectInfoViewModels)
      {
        objectInfoViewModel.IsSelected = objectInfoSetViewModel.IsSelected;
      }
    }

    private void OnBlocksPopupOpened(object sender, EventArgs e)
    {
      if (this.searchBox.Focusable)
        this.searchBox.Focus();
    }

    private async void OnApplyBlockSelection(object sender, RoutedEventArgs e)
    {
      if (this.viewModel.IsHighlighting)
        await this.UpdateHighlight();
    }

    private async void OnSetClicked(object sender, EventArgs e)
    {
      if (!(sender is MenuItem menuItem))
        return;

      if (!(menuItem.DataContext is ObjectInfoSetViewModel objectInfoSetViewModel))
        return;

      await HighlightSet(objectInfoSetViewModel);
    }

    private void OnEditSets(object sender, RoutedEventArgs e)
    {
      var window = new SetsWindow
      {
        Owner = this,
        DataContext = this.viewModel
      };
      window.ShowDialog();

      Set.Save(this.viewModel.Sets, this.viewModel.SetsFilename);

      this.ReorderSets();
    }

    private void ReorderSets()
    {
      int number = 1;

      foreach (var set in this.viewModel.Sets)
      {
        if (number < 10)
          set.InputGestureText = "Ctrl+" + number;
        number++;
      }
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


    private void OnOpenPlayerMapFileCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      if (!this.viewModel.IsLoading && this.viewModel.World != null)
        e.CanExecute = true;
    }

    private async void OnOpenPlayerMapFileExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      await this.OpenPlayerMapFile();
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

    private void OnExportHighlightedTilePositionsCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      if (this.viewModel.IsLoaded && this.viewModel.IsHighlighting)
        e.CanExecute = true;
    }

    private async void OnExportHighlightedTilePositionsExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      try
      {
        await this.ExportHighlightedTilePositions();
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
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


    private void OnPropertiesCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      if (this.viewModel.IsLoaded)
        e.CanExecute = true;
    }

    private void OnPropertiesExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      this.ShowProperties();
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

    private async void OnFindExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      await this.ChooseTileInfo();
    }


    private void OnPreviousPageCanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      if (this.viewModel.IsLoaded)
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
      if (this.viewModel.IsLoaded)
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

    private void OnSearchBoxClear(object sender, RoutedEventArgs e)
    {
      this.searchBox.Text = "";
    }

    private void OnCanExecuteHighlightSet(object sender, CanExecuteRoutedEventArgs e)
    {
      if (!this.viewModel.IsLoaded)
        return;

      string parameter = e.Parameter as string;
      if (string.IsNullOrWhiteSpace(parameter))
        return;


      if (!int.TryParse(parameter, out int number))
        return;

      e.CanExecute = number > 0 && this.viewModel.Sets.Count >= number;
    }

    private async void OnExecutedHighlightSet(object sender, ExecutedRoutedEventArgs e)
    {
      if (!this.viewModel.IsLoaded)
        return;

      string parameter = e.Parameter as string;
      if (string.IsNullOrWhiteSpace(parameter))
        return;

      if (!int.TryParse(parameter, out int number))
        return;

      if (number < 1 || this.viewModel.Sets.Count < number)
        return;

      var set = this.viewModel.Sets[number - 1];

      await this.HighlightSet(set);
    }

    #endregion
  }
}
