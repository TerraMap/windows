using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TerraMap.Data
{
	public class World : INotifyPropertyChanged
	{
		public World()
		{
		}

		#region Dynamically read properties

		public Int32 Version { get; set; }
		public String Name { get; set; }
		public Int32 Id { get; set; }
		public Rectangle Bounds { get; set; }
		public Int32 WorldHeightinTiles { get; set; }
		public Int32 WorldWidthinTiles { get; set; }

		[PropertyInfo(63)]
		public Byte MoonType { get; set; }

		[PropertyInfo(44, 3)]
		public Int32[] TreeTypeXCoordinates { get; set; }	// x3

		[PropertyInfo(44, 4)]
		public Int32[] TreeStyles { get; set; }	// x4

		[PropertyInfo(60, 3)]
		public Int32[] CaveBackXCoordinates { get; set; }	// x3

		[PropertyInfo(60, 4)]
		public Int32[] CaveBackStyles { get; set; }	// x4

		[PropertyInfo(60)]
		public Int32 IceBackStyle { get; set; }
		[PropertyInfo(61)]
		public Int32 JungleBackStyle { get; set; }
		[PropertyInfo(61)]
		public Int32 HellBackStyle { get; set; }

		public Int32 SpawnX { get; set; }
		public Int32 SpawnY { get; set; }
		public Double WorldSurfaceY { get; set; }
		public Double RockLayerY { get; set; }
		public Double GameTime { get; set; }
		public Boolean IsDay { get; set; }
		public Int32 MoonPhase { get; set; }
		public Boolean BloodMoon { get; set; }

		[PropertyInfo(70)]
		public Boolean Eclipse { get; set; }

		public Int32 DungeonX { get; set; }
		public Int32 DungeonY { get; set; }

		[PropertyInfo(56)]
		public Boolean CrimsonWorld { get; set; }

		public Boolean KilledEyeofCthulu { get; set; }
		public Boolean KilledEaterofWorlds { get; set; }
		public Boolean KilledSkeletron { get; set; }

		[PropertyInfo(66)]
		public Boolean KilledQueenBee { get; set; }
		[PropertyInfo(44)]
		public Boolean KilledTheDestroyer { get; set; }
		[PropertyInfo(44)]
		public Boolean KilledTheTwins { get; set; }
		[PropertyInfo(44)]
		public Boolean KilledSkeletronPrime { get; set; }
		[PropertyInfo(44)]
		public Boolean KilledAnyHardmodeBoss { get; set; }
		[PropertyInfo(64)]
		public Boolean KilledPlantera { get; set; }
		[PropertyInfo(64)]
		public Boolean KilledGolem { get; set; }
		[PropertyInfo(29)]
		public Boolean SavedGoblinTinkerer { get; set; }
		[PropertyInfo(29)]
		public Boolean SavedWizard { get; set; }
		[PropertyInfo(34)]
		public Boolean SavedMechanic { get; set; }
		[PropertyInfo(29)]
		public Boolean DefeatedGoblinInvasion { get; set; }
		[PropertyInfo(32)]
		public Boolean KilledClown { get; set; }
		[PropertyInfo(37)]
		public Boolean DefeatedFrostLegion { get; set; }
		[PropertyInfo(56)]
		public Boolean DefeatedPirates { get; set; }

		public Boolean BrokeaShadowOrb { get; set; }
		public Boolean MeteorSpawned { get; set; }
		public Byte ShadowOrbsbrokenmod3 { get; set; }

		[PropertyInfo(23)]
		public Int32 AltarsSmashed { get; set; }
		[PropertyInfo(23)]
		public Boolean HardMode { get; set; }

		public Int32 GoblinInvasionDelay { get; set; }
		public Int32 GoblinInvasionSize { get; set; }
		public Int32 GoblinInvasionType { get; set; }
		public Double GoblinInvasionX { get; set; }

		[PropertyInfo(53)]
		public Boolean IsRaining { get; set; }
		[PropertyInfo(53)]
		public Int32 RainTime { get; set; }
		[PropertyInfo(53)]
		public Single MaxRain { get; set; }
		[PropertyInfo(54)]
		public Int32 Tier1OreID { get; set; }
		[PropertyInfo(54)]
		public Int32 Tier2OreID { get; set; }
		[PropertyInfo(54)]
		public Int32 Tier3OreID { get; set; }
		[PropertyInfo(55)]
		public Byte TreeStyle { get; set; }
		[PropertyInfo(55)]
		public Byte CorruptionStyle { get; set; }
		[PropertyInfo(55)]
		public Byte JungleStyle { get; set; }
		[PropertyInfo(60)]
		public Byte SnowStyle { get; set; }
		[PropertyInfo(60)]
		public Byte HallowStyle { get; set; }
		[PropertyInfo(60)]
		public Byte CrimsonStyle { get; set; }
		[PropertyInfo(60)]
		public Byte DesertStyle { get; set; }
		[PropertyInfo(60)]
		public Byte OceanStyle { get; set; }
		[PropertyInfo(60)]
		public Int32 CloudBackground { get; set; }
		[PropertyInfo(62)]
		public Int16 NumberofClouds { get; set; }
		[PropertyInfo(62)]
		public Single WindSpeed { get; set; }

		#endregion

		#region Other properties

		private string status;

		[PropertyInfo(ignore: true)]
		public string Status
		{
			get { return status; }
			set
			{
				status = value;
				RaisePropertyChanged();
			}
		}

		[PropertyInfo(ignore: true)]
		public Tile[,] Tiles { get; set; }

		[PropertyInfo(ignore: true)]
		public List<Chest> Chests { get; set; }

		private StaticData staticData;

		[PropertyInfo(ignore: true)]
		public StaticData StaticData
		{
			get { return staticData; }
			set
			{
				staticData = value;
				RaisePropertyChanged();
			}
		}

		private ConcurrentQueue<Int32Rect> updatedRectangles = new ConcurrentQueue<Int32Rect>();

		[PropertyInfo(ignore: true)]
		public ConcurrentQueue<Int32Rect> UpdatedRectangles
		{
			get { return updatedRectangles; }
			set
			{
				updatedRectangles = value;
				RaisePropertyChanged();
			}
		}

		private int progressValue;

		[PropertyInfo(ignore: true)]
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

		[PropertyInfo(ignore: true)]
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

		[PropertyInfo(ignore: true)]
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

		[PropertyInfo(ignore: true)]
		public int HighlightedTileCount
		{
			get { return highlightedTileCount; }
			set
			{
				highlightedTileCount = value;
				RaisePropertyChanged();
			}
		}

		[PropertyInfo(ignore: true)]
		public List<Sign> Signs { get; set; }

		[PropertyInfo(ignore: true)]
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

		[PropertyInfo(ignore: true)]
		public bool Success { get; set; }

		[PropertyInfo(ignore: true)]
		public string VerificationWorldName { get; set; }

		[PropertyInfo(ignore: true)]
		public int VerificationWorldId { get; set; }

		#endregion

		public Task ReadAsync(string filename)
		{
			return Task.Run(() =>
			{
				this.Read(filename);
			});
		}

		public void Read(string filename)
		{
			this.ProgressMaximum = 0;
			this.ProgressValue = 0;

			//this.OnProgressChanged(new ProgressEventArgs(tilesProcessed, totalTileCount));

			using (Stream stream = File.OpenRead(filename))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					this.Status = "Reading header";
					this.ReadHeader(reader);

					this.Status = "Reading tiles";
					this.ReadTiles(reader);

					this.Status = "Reading chests";
					this.ReadChests(reader);

					this.Status = "Reading signs";
					this.ReadSigns(reader);

					this.Status = "Reading NPCs";
					this.ReadNPCs(reader);

					//this.Status = "Reading Verification";
					//this.ReadVerification(reader);
				}
			}

			this.Status = string.Format("Finished reading '{0}'", filename);
		}

		private void ReadVerification(BinaryReader reader)
		{
			if (this.Version >= 7)
			{
				this.Success = reader.ReadBoolean();
				this.VerificationWorldName = reader.ReadString();
				this.VerificationWorldId = reader.ReadInt32();
			}
		}

		public Task ReadHeaderAsync(string filename)
		{
			return Task.Run(() =>
			{
				using (Stream stream = File.OpenRead(filename))
				{
					using (BinaryReader reader = new BinaryReader(stream))
					{
						this.ReadHeader(reader);
					}
				}
			});
		}

		public void ReadHeader(BinaryReader reader)
		{
			var properties = typeof(World).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var property in properties)
			{
				var minimumVersion = 0;
				var count = 1;

				var propertyInfoAttribute = property.GetCustomAttribute<PropertyInfoAttribute>();
				if (propertyInfoAttribute != null)
				{
					if (propertyInfoAttribute.Ignore)
						continue;

					minimumVersion = propertyInfoAttribute.MinimumVersion;
					count = propertyInfoAttribute.Count;
				}

				if (minimumVersion > this.Version)
					continue;

				var dataType = property.PropertyType;

				if (dataType == typeof(Boolean))
					property.SetValue(this, reader.ReadBoolean());
				else if (dataType == typeof(Byte))
					property.SetValue(this, reader.ReadByte());
				else if (dataType == typeof(Int16))
					property.SetValue(this, reader.ReadInt16());
				else if (dataType == typeof(Int32))
					property.SetValue(this, reader.ReadInt32());
				else if (dataType == typeof(String))
					property.SetValue(this, reader.ReadString());
				else if (dataType == typeof(Single))
					property.SetValue(this, reader.ReadSingle());
				else if (dataType == typeof(Double))
					property.SetValue(this, reader.ReadDouble());
				else if (dataType == typeof(Rectangle))
					property.SetValue(this, ReadRectangle(reader));
				else if (dataType == typeof(Int32[]))
				{
					Int32[] array = new Int32[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = reader.ReadInt32();
					}

					property.SetValue(this, array);
				}
			}
		}

		public void ReadTiles(BinaryReader reader)
		{
			this.Tiles = new Tile[this.WorldWidthinTiles, this.WorldHeightinTiles];

			this.totalTileCount = this.WorldHeightinTiles * this.WorldWidthinTiles;

			var tilesProcessed = 0;

			this.ProgressMaximum = this.totalTileCount;
			this.ProgressValue = tilesProcessed;

			//this.OnProgressChanged(new ProgressEventArgs(tilesProcessed, totalTileCount));

			for (int x = 0; x < this.WorldWidthinTiles; x++)
			{
				for (int y = 0; y < this.WorldHeightinTiles; y++)
				{
					var tile = Tile.Read(reader, this);

					Color color = Color.Transparent;

					var tileInfo = this.staticData.TileInfos[tile.Type, tile.TextureU, tile.TextureV];

					if (tile.IsActive)
					{
						if (tileInfo.IsColorSet)
							color = tileInfo.Color;
						else
						{
							color = tileInfo.Color = ColorTranslator.FromHtml(tileInfo.ColorName);

							tileInfo.IsColorSet = true;
						}
					}
					else if (tile.IsLiquidPresent)
					{
						if (tile.IsLiquidLava)
							color = this.staticData.GlobalColors.LavaColor;
						else if (tile.IsLiquidHoney)
							color = this.staticData.GlobalColors.HoneyColor;
						else
							color = this.staticData.GlobalColors.WaterColor;
					}
					else if (tile.IsWallPresent)
					{
						color = this.staticData.WallInfos[tile.WallType].Color;
					}
					else if (y < this.WorldSurfaceY)
					{
						color = this.staticData.GlobalColors.SkyColor;
					}
					else // if (y < this.RockLayerY)
					{
						color = this.staticData.GlobalColors.EarthColor;
					}
					//else
					//{
					//	// fade betwen rock color and hell color
					//}

					tile.Color = color;

					this.Tiles[x, y] = tile;

					for (int i = 0; i < tile.RleLength; i++)
					{
						y++;
						this.Tiles[x, y] = tile;
					}
				}

				tilesProcessed += this.WorldHeightinTiles;

				this.ProgressMaximum = this.totalTileCount;
				this.ProgressValue = tilesProcessed;

				//this.OnProgressChanged(new ProgressEventArgs(tilesProcessed, totalTileCount));
			}
		}

		private void ReadChests(BinaryReader reader)
		{
			this.Chests = new List<Chest>();

			var totalCount = 1000;

			var processedCount = 0;

			this.ProgressMaximum = totalCount;
			this.ProgressValue = processedCount;

			for (int i = 0; i < totalCount; i++)
			{
				var chest = Chest.Read(reader, this);

				if (chest == null)
					continue;

				this.Chests.Add(chest);

				this.ProgressValue++;
			}
		}

		private void ReadSigns(BinaryReader reader)
		{
			this.Signs = new List<Sign>();

			var totalCount = 1000;

			var processedCount = 0;

			this.ProgressMaximum = totalCount;
			this.ProgressValue = processedCount;

			for (int i = 0; i < totalCount; i++)
			{
				var sign = Sign.Read(reader);

				if (sign == null)
					continue;

				this.Signs.Add(sign);

				this.ProgressValue++;
			}
		}

		private void ReadNPCs(BinaryReader reader)
		{
			this.NPCs = new ObservableCollection<NPC>();

			NPC npc = null;

			while ((npc = NPC.Read(reader)) != null)
			{
				this.NPCs.Add(npc);
			}

			this.ReadNpcName(reader, "Merchant");
			this.ReadNpcName(reader, "Nurse");
			this.ReadNpcName(reader, "Arms Dealer");
			this.ReadNpcName(reader, "Dryad");
			this.ReadNpcName(reader, "Guide");
			this.ReadNpcName(reader, "Clothier");
			this.ReadNpcName(reader, "Demolitionist");
			this.ReadNpcName(reader, "Goblin Tinkerer");
			this.ReadNpcName(reader, "Wizard");

			if (this.Version >= 34)
				this.ReadNpcName(reader, "Mechanic");

			if (this.Version >= 65 && this.NPCs.Count > 0)
			{
				this.ReadNpcName(reader, "Truffle");
				this.ReadNpcName(reader, "Steampunker");
				this.ReadNpcName(reader, "Dye Trader");
				this.ReadNpcName(reader, "Party Girl");
				this.ReadNpcName(reader, "Cyborg");
				this.ReadNpcName(reader, "Painter");
				this.ReadNpcName(reader, "Witch Doctor");
				this.ReadNpcName(reader, "Pirate");
			}
		}

		private void ReadNpcName(BinaryReader reader, string type)
		{
			var name = reader.ReadString();
			var npc = this.NPCs.FirstOrDefault(n => n.Type == type);
			if (npc != null)
				npc.Name = name;
		}

		private static Rectangle ReadRectangle(BinaryReader reader)
		{
			var left = reader.ReadInt32();
			var right = reader.ReadInt32();
			var top = reader.ReadInt32();
			var bottom = reader.ReadInt32();

			return new Rectangle(left, top, right - left, bottom - top);
		}

		//public async Task WriteBitmapAsync(WriteableBitmap bitmap, byte tileType = byte.MinValue)
		//{
		//	bitmap.Lock();

		//	// Get a pointer to the back buffer. 
		//	int pBackBuffer = (int)bitmap.BackBuffer;

		//	await Task.Run(() =>
		//	{
		//		unsafe
		//		{
		//			for (int y = 0; y < this.WorldHeightinTiles; y++)
		//			{
		//				for (int x = 0; x < this.WorldWidthinTiles; x++)
		//				{
		//					var tile = this.Tiles[x, y];

		//					// compute the pixel's color
		//					int color_data = tile.Color.R << 16;
		//					color_data |= tile.Color.G << 8;
		//					color_data |= tile.Color.B << 0;

		//					if (tileType != byte.MinValue && tileType != tile.Type)
		//					{
		//						color_data = AlphaBlend(0, color_data, 0.3);
		//					}

		//					*((int*)pBackBuffer) = color_data;

		//					pBackBuffer += 4;
		//				}

		//				this.ProgressValue += this.WorldWidthinTiles;
		//			}
		//		}
		//	});

		//	bitmap.AddDirtyRect(new Int32Rect(0, 0, this.WorldWidthinTiles, this.WorldHeightinTiles));

		//	bitmap.Unlock();
		//}

		//public async Task WriteBitmapAsync(WriteableBitmap bitmap, byte tileType = byte.MinValue)
		//{
		//	bitmap.Lock();

		//	// Get a pointer to the back buffer. 
		//	int pBackBuffer = (int)bitmap.BackBuffer;

		//	await Task.Run(() =>
		//	{
		//		unsafe
		//		{
		//			Parallel.For(0, this.WorldWidthinTiles, (y) =>
		//			//for (int y = 0; y < this.WorldHeightinTiles; y++)
		//			{
		//				var pBackBufferCopy = pBackBuffer;

		//				for (int x = 0; x < this.WorldWidthinTiles; x++)
		//				{
		//					var tile = this.Tiles[x, y];

		//					// compute the pixel's color
		//					int color_data = tile.Color.R << 16;
		//					color_data |= tile.Color.G << 8;
		//					color_data |= tile.Color.B << 0;

		//					if (tileType != byte.MinValue && tileType != tile.Type)
		//					{
		//						color_data = AlphaBlend(0, color_data, 0.3);
		//					}

		//					*((int*)pBackBuffer) = color_data;

		//					pBackBuffer += 4;
		//				}

		//				this.ProgressValue += this.WorldWidthinTiles;
		//			}
		//			);
		//		}
		//	});

		//	bitmap.AddDirtyRect(new Int32Rect(0, 0, this.WorldWidthinTiles, this.WorldHeightinTiles));

		//	bitmap.Unlock();
		//}

		public async Task WritePixelDataAsync(byte[] pixelData, int rawStride, ObjectInfoViewModel objectTypeToHighlight = null)
		{
			await Task.Run(() =>
			{
				this.WritePixelData(pixelData, rawStride, objectTypeToHighlight);
			});
		}

		public void WritePixelData(byte[] buffer, int rawStride, ObjectInfoViewModel objectTypeToHighlight = null)
		{
			int totalTileCount = this.WorldWidthinTiles * this.WorldHeightinTiles;
			int tilesProcessed = 0;

			this.ProgressMaximum = totalTileCount;
			this.ProgressValue = tilesProcessed;
			this.HighlightedTileCount = 0;

			//this.OnProgressChanged(new ProgressEventArgs(tilesProcessed, totalTileCount));

			Parallel.For(0, this.WorldHeightinTiles, (y) =>
			//for (int y = 0; y < this.WorldHeightinTiles; y++)
			{
				for (int x = 0; x < this.WorldWidthinTiles; x++)
				{
					var tile = this.Tiles[x, y];

					// compute the pixel's color

					Color color = tile.Color;

					var tileMatches = IsTileMatch(objectTypeToHighlight, x, y, tile);

					if (tileMatches)
					{
						if (objectTypeToHighlight != null)
							this.HighlightedTileCount++;
					}
					else
					{
						color = Blend(Color.Black, color, 0.8);
					}

					var pixelIndex = y * this.WorldWidthinTiles * 4 + (x * 4);

					buffer[pixelIndex++] = color.B;
					buffer[pixelIndex++] = color.G;
					buffer[pixelIndex++] = color.R;
					buffer[pixelIndex++] = color.A;

					tilesProcessed++;
				}

				this.updatedRectangles.Enqueue(new Int32Rect(0, y, this.WorldWidthinTiles, 1));

				this.ProgressMaximum = totalTileCount;
				this.ProgressValue = tilesProcessed;

				//this.OnProgressChanged(new ProgressEventArgs(tilesProcessed, totalTileCount));
			}
			);
		}

		public bool IsTileMatch(ObjectInfoViewModel objectTypeToHighlight, int x, int y, Tile tile, TileHitTestInfo currentTile = null)
		{
			if (objectTypeToHighlight == null)
				return true;

			if (objectTypeToHighlight.TileInfo != null)
			{
				var tileInfo = this.StaticData.TileInfos[tile.Type, tile.TextureU, tile.TextureV];

				if (tileInfo == objectTypeToHighlight.TileInfo)
					return true;
			}

			if (objectTypeToHighlight.ItemInfo != null)
			{
				var tileInfo = this.StaticData.TileInfos[tile.Type];

				var variantTileInfo = this.StaticData.TileInfos[tile.Type, tile.TextureU, tile.TextureV];

				if (tileInfo.Name != "Chest")
					return false;

				var chest = this.Chests.FirstOrDefault(c => (c.X == x || c.X + 1 == x) && (c.Y == y || c.Y + 1 == y));
				if (chest == null)
					return false;

				if (currentTile != null)
				{
					var currentChest = this.Chests.FirstOrDefault(c => (c.X == currentTile.X || c.X + 1 == currentTile.X) && (c.Y == currentTile.Y || c.Y + 1 == currentTile.Y));
					if (chest == currentChest)
						return false;
				}

				foreach (var item in chest.Items)
				{
					if (item.Id == objectTypeToHighlight.ItemInfo.Id)
					{
						return true;
					}
				}
			}

			return false;
		}

		private Color Blend(Color color, Color backColor, double amount)
		{
			byte r = (byte)((color.R * amount) + backColor.R * (1 - amount));
			byte g = (byte)((color.G * amount) + backColor.G * (1 - amount));
			byte b = (byte)((color.B * amount) + backColor.B * (1 - amount));
			return Color.FromArgb(r, g, b);
		}

		private int AlphaBlend(int from, int to, double alpha)
		{
			int fr = (from >> 16) & 0xff;
			int fg = (from >> 8) & 0xff;
			int fb = from & 0xff;
			int tr = (to >> 16) & 0xff;
			int tg = (to >> 8) & 0xff;
			int tb = to & 0xff;
			fr = (int)(tr * alpha + fr * (1 - alpha));
			fg = (int)(tg * alpha + fg * (1 - alpha));
			fb = (int)(tb * alpha + fb * (1 - alpha));
			return (fr << 16) | (fg << 8) | fb;
		}

		public string GetTileName(int x, int y)
		{
			string name = "Nothing";

			if (x >= 0 && x < this.WorldWidthinTiles &&
				y >= 0 && y < this.WorldHeightinTiles)
			{
				var tile = this.Tiles[x, y];

				if (tile.IsActive)
				{
					name = this.staticData.TileInfos[tile.Type, tile.TextureU, tile.TextureV].Name;
				}
				else if (tile.IsLiquidPresent)
				{
					if (tile.IsLiquidLava)
						name = "Lava";
					else if (tile.IsLiquidHoney)
						name = "Honey";
					else
						name = "Water";
				}
				else if (tile.IsWallPresent)
				{
					name = this.staticData.WallInfos[tile.WallType].Name;
				}
			}
			else
			{
				name = null;
			}

			return name;
		}

		public override string ToString()
		{
			var properties = typeof(World).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			StringBuilder stringBuilder = new StringBuilder();

			foreach (var property in properties)
			{
				stringBuilder.AppendFormat("{0}: {1}", property.Name, property.GetValue(this));
				stringBuilder.AppendLine();
			}

			return stringBuilder.ToString();
		}

		//private void OnProgressChanged(ProgressEventArgs args)
		//{
		//	if (this.ProgressChanged != null)
		//		this.ProgressChanged(this, args);
		//}

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (this.PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
