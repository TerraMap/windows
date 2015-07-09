using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
			MapHelper.Initialize();
		}

		#region Dynamically read properties

		public Int32 Version { get; set; }
		public String Name { get; set; }
		public Int32 Id { get; set; }
		public Rectangle Bounds { get; set; }
		public Int32 WorldHeightinTiles { get; set; }
		public Int32 WorldWidthinTiles { get; set; }

		[PropertyInfo(112)]
		public Boolean ExpertMode { get; set; }

		[PropertyInfo(141)]
		public Int64 CreationTime { get; set; }

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

		[PropertyInfo(118)]
		public Boolean KilledSlimeKing { get; set; }

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

		[PropertyInfo(118)]
		public Double SlimeRainTime { get; set; }

		[PropertyInfo(113)]
		public Byte SundialCooldown { get; set; }

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

		private ObservableCollection<NPC> npcs = new ObservableCollection<NPC>();

		[PropertyInfo(ignore: true)]
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

		private ObservableCollection<WorldProperty> properties = new ObservableCollection<WorldProperty>();

		[PropertyInfo(ignore: true)]
		public ObservableCollection<WorldProperty> Properties
		{
			get { return properties; }
			set
			{
				properties = value;
				RaisePropertyChanged();
			}
		}

		private bool savedAngler;

		[PropertyInfo(ignore: true)]
		public bool SavedAngler
		{
			get { return savedAngler; }
			set
			{
				savedAngler = value;
				RaisePropertyChanged();
			}
		}

		private int anglerQuest;

		[PropertyInfo(ignore: true)]
		public int AnglerQuest
		{
			get { return anglerQuest; }
			set
			{
				anglerQuest = value;
				RaisePropertyChanged();
			}
		}

		[PropertyInfo(ignore: true)]
		public uint Revision { get; set; }

		[PropertyInfo(ignore: true)]
		public bool IsFavorite { get; set; }

		[PropertyInfo(ignore: true)]
		public bool SavedStylist { get; set; }

		[PropertyInfo(ignore: true)]
		public bool SavedTaxCollector { get; set; }

		[PropertyInfo(ignore: true)]
		public int InvasionSizeStart { get; set; }

		[PropertyInfo(ignore: true)]
		public int TempCultistDelay { get; set; }

		[PropertyInfo(ignore: true)]
		public bool FastForwardTime { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedFishron { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedMartians { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedAncientCultist { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedMoonlord { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedHalloweenKing { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedHalloweenTree { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedChristmasIceQueen { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedChristmasSantank { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedChristmasTree { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedTowerSolar { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedTowerVortex { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedTowerNebula { get; set; }

		[PropertyInfo(ignore: true)]
		public bool DownedTowerStardust { get; set; }

		[PropertyInfo(ignore: true)]
		public bool TowerActiveSolar { get; set; }

		[PropertyInfo(ignore: true)]
		public bool TowerActiveVortex { get; set; }

		[PropertyInfo(ignore: true)]
		public bool TowerActiveNebula { get; set; }

		[PropertyInfo(ignore: true)]
		public bool TowerActiveStardust { get; set; }

		[PropertyInfo(ignore: true)]
		public bool LunarApocalypseIsUp { get; set; }

		#endregion

		public static string GetWorldName(string worldFileName)
		{
			if (worldFileName == null)
			{
				return string.Empty;
			}
			try
			{
				using (FileStream fileStream = new FileStream(worldFileName, FileMode.Open))
				{
					using (BinaryReader binaryReader = new BinaryReader(fileStream))
					{
						int version = binaryReader.ReadInt32();
						if (version > 0 && version <= 93)
						{
							string text;
							string result;
							if (version <= 87)
							{
								text = binaryReader.ReadString();
								binaryReader.Close();
								result = text;
								return result;
							}
							binaryReader.ReadInt16();
							fileStream.Position = (long)binaryReader.ReadInt32();
							text = binaryReader.ReadString();
							binaryReader.Close();
							result = text;
							return result;
						}
					}
				}
			}
			catch
			{
			}
			string[] array = worldFileName.Split(new char[]
			{
				Path.DirectorySeparatorChar
			});
			string text2 = array[array.Length - 1];
			return text2.Substring(0, text2.Length - 4);
		}

		public Task ReadAsync(string filename)
		{
			return Task.Factory.StartNew(() =>
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
					this.Version = reader.ReadInt32();

					reader.BaseStream.Position = 0L;

					if (Version <= 87)
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
					}
					else
					{
						this.ReadWorldVersion2(reader);
					}

					//this.Status = "Reading Verification";
					//this.ReadVerification(reader);
				}
			}

			this.Status = string.Format("Finished reading '{0}'", filename);
		}

		private void ReadWorldVersion2(BinaryReader reader)
		{
			bool[] importance;
			int[] positions;

			this.LoadFileFormatHeader(reader, out importance, out positions);

			if (reader.BaseStream.Position != (long)positions[0])
				throw new Exception(string.Format("World file header is not where it's expected to be. Expected: {0} Actual: {1}", positions[0], reader.BaseStream.Position));
			this.ReadHeader(reader, skipVersion: true);

			if (reader.BaseStream.Position != (long)positions[1])
				throw new Exception(string.Format("World file tiles list start is not where it's expected to be. Expected: {0} Actual: {1}", positions[1], reader.BaseStream.Position));
			this.ReadTilesVersion2(reader, importance);

			if (reader.BaseStream.Position != (long)positions[2])
				throw new Exception(string.Format("World file chests list start is not where it's expected to be. Expected: {0} Actual: {1}", positions[2], reader.BaseStream.Position));
			this.ReadChestsVersion2(reader);

			this.Signs = new List<Sign>();

			if (reader.BaseStream.Position != (long)positions[3])
				throw new Exception(string.Format("World file signs list start is not where it's expected to be. Expected: {0} Actual: {1}", positions[3], reader.BaseStream.Position));
			this.ReadSignsVersion2(reader);

			if (reader.BaseStream.Position != (long)positions[4])
				throw new Exception(string.Format("World file NPCs list start is not where it's expected to be. Expected: {0} Actual: {1}", positions[4], reader.BaseStream.Position));
			this.ReadNPCsVersion2(reader);

			if (reader.BaseStream.Position != (long)positions[5])
				throw new Exception(string.Format("World file verification footer is not where it's expected to be. Expected: {0} Actual: {1}", positions[4], reader.BaseStream.Position));
			this.ReadVerification(reader);
		}

		private void LoadFileFormatHeader(BinaryReader reader, out bool[] importance, out int[] positions)
		{
			this.Version = reader.ReadInt32();

			if (this.Version >= 135)
			{
				// read file metadata
				ulong num = reader.ReadUInt64();

				if ((num & 72057594037927935uL) != 27981915666277746uL)
				{
					throw new FileFormatException("Expected Re-Logic file format.");
				}

				this.Revision = reader.ReadUInt32();
				ulong num2 = reader.ReadUInt64();
				this.IsFavorite = ((num2 & 1uL) == 1uL);
			}

			var positionsLength = reader.ReadInt16();
			positions = new int[(int)positionsLength];
			for (int i = 0; i < (int)positionsLength; i++)
			{
				positions[i] = reader.ReadInt32();
			}

			var importanceLength = reader.ReadInt16();
			importance = new bool[(int)importanceLength];
			byte b = 0;
			byte b2 = 128;
			for (int i = 0; i < (int)importanceLength; i++)
			{
				if (b2 == 128)
				{
					b = reader.ReadByte();
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
				if ((b & b2) == b2)
				{
					importance[i] = true;
				}
			}
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
			return Task.Factory.StartNew(() =>
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

		public void ReadHeader(BinaryReader reader, bool skipVersion = false)
		{
			var properties = typeof(World).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var property in properties)
			{
				if (property.Name == "Version" && skipVersion)
				{
					this.Properties.Add(new WorldProperty() { Name = "Version", Value = this.Version });
					continue;
				}

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

				object value = null;

				if (dataType == typeof(Boolean))
					property.SetValue(this, reader.ReadBoolean(), null);
				else if (dataType == typeof(Byte))
					property.SetValue(this, reader.ReadByte(), null);
				else if (dataType == typeof(Int16))
					property.SetValue(this, reader.ReadInt16(), null);
				else if (dataType == typeof(Int32))
					property.SetValue(this, reader.ReadInt32(), null);
				else if (dataType == typeof(Int64))
					property.SetValue(this, reader.ReadInt64(), null);
				else if (dataType == typeof(String))
					property.SetValue(this, reader.ReadString(), null);
				else if (dataType == typeof(Single))
					property.SetValue(this, reader.ReadSingle(), null);
				else if (dataType == typeof(Double))
					property.SetValue(this, reader.ReadDouble(), null);
				else if (dataType == typeof(Rectangle))
					property.SetValue(this, ReadRectangle(reader), null);
				else if (dataType == typeof(Int32[]))
				{
					Int32[] array = new Int32[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = reader.ReadInt32();
					}

					if (count > 0)
						value = string.Join(", ", array);

					property.SetValue(this, array, null);
				}

				if (value == null)
					value = property.GetValue(this, null);

				this.Properties.Add(new WorldProperty() { Name = property.Name, Value = value });
			}

			if (Version < 95)
				return;

			List<string> anglerWhoFinishedToday = new List<string>();

			for (int i = reader.ReadInt32(); i > 0; i--)
			{
				anglerWhoFinishedToday.Add(reader.ReadString());
			}

			if (Version < 99)
				return;

			this.SavedAngler = reader.ReadBoolean();

			if (Version < 101)
				return;

			this.AnglerQuest = reader.ReadInt32();
			
			if (Version < 104)
				return;

			this.SavedStylist = reader.ReadBoolean();
			
			if (Version >= 129)
			{
				this.SavedTaxCollector = reader.ReadBoolean();
			}

			if (Version < 107)
			{
			}
			else
			{
				this.InvasionSizeStart = reader.ReadInt32();
			}

			if (Version < 108)
			{
				this.TempCultistDelay = 86400;
			}
			else
			{
				this.TempCultistDelay = reader.ReadInt32();
			}

			if (Version < 109)
				return;
			
			int num2 = (int)reader.ReadInt16();
			for (int j = 0; j < num2; j++)
			{
				if (j < 540)
				{
					//this.NpcKillCount[j] = reader.ReadInt32();
					reader.ReadInt32();
				}
				else
				{
					reader.ReadInt32();
				}
			}

			if (Version < 128)
				return;
			
			this.FastForwardTime = reader.ReadBoolean();
			
			if (Version < 131)
				return;
			
			this.DownedFishron = reader.ReadBoolean();
			this.DownedMartians = reader.ReadBoolean();
			this.DownedAncientCultist = reader.ReadBoolean();
			this.DownedMoonlord = reader.ReadBoolean();
			this.DownedHalloweenKing = reader.ReadBoolean();
			this.DownedHalloweenTree = reader.ReadBoolean();
			this.DownedChristmasIceQueen = reader.ReadBoolean();
			this.DownedChristmasSantank = reader.ReadBoolean();
			this.DownedChristmasTree = reader.ReadBoolean();
			
			if (Version < 140)
				return;

			this.DownedTowerSolar = reader.ReadBoolean();
			this.DownedTowerVortex = reader.ReadBoolean();
			this.DownedTowerNebula = reader.ReadBoolean();
			this.DownedTowerStardust = reader.ReadBoolean();
			this.TowerActiveSolar = reader.ReadBoolean();
			this.TowerActiveVortex = reader.ReadBoolean();
			this.TowerActiveNebula = reader.ReadBoolean();
			this.TowerActiveStardust = reader.ReadBoolean();
			this.LunarApocalypseIsUp = reader.ReadBoolean();
		}

		private void ReadTilesVersion2(BinaryReader reader, bool[] importance)
		{
			this.Tiles = new Tile[this.WorldWidthinTiles, this.WorldHeightinTiles];

			this.totalTileCount = this.WorldHeightinTiles * this.WorldWidthinTiles;

			var tilesProcessed = 0;

			this.ProgressMaximum = this.totalTileCount;
			this.ProgressValue = tilesProcessed;

			for (int x = 0; x < this.WorldWidthinTiles; x++)
			{
				for (int y = 0; y < this.WorldHeightinTiles; y++)
				{
					//if (x == 100 && y == 440)
					//	Debug.Write("");

					int num2 = -1;
					byte b2;
					byte b = b2 = 0;
					Tile tile = new Tile();
					byte b3 = reader.ReadByte();
					if ((b3 & 1) == 1)
					{
						b2 = reader.ReadByte();
						if ((b2 & 1) == 1)
						{
							b = reader.ReadByte();
						}
					}
					byte b4;
					if ((b3 & 2) == 2)
					{
						tile.IsActive = true;
						if ((b3 & 32) == 32)
						{
							b4 = reader.ReadByte();
							num2 = (int)reader.ReadByte();
							num2 = (num2 << 8 | (int)b4);
						}
						else
						{
							num2 = (int)reader.ReadByte();
						}

						tile.Type = (ushort)num2;

						//if (tile.Type > 254 && tile.Type != 280)
						//	Debug.Write("");

						if (importance[num2])
						{
							tile.TextureU = reader.ReadInt16();
							tile.TextureV = reader.ReadInt16();
							if (tile.Type == 144)
							{
								tile.TextureV = 0;
							}
							if (tile.Type == 26)
							{
								Debug.WriteLine("");
							}
						}
						else
						{
							tile.TextureU = -1;
							tile.TextureV = -1;

							if (tile.Type == 105)
							{
								Debug.WriteLine("");
							}
						}
						if ((b & 8) == 8)
						{
							tile.ColorValue = reader.ReadByte();
						}
					}
					if ((b3 & 4) == 4)
					{
						tile.WallType = reader.ReadByte();
						tile.IsWallPresent = true;
						if ((b & 16) == 16)
						{
							tile.WallColor = reader.ReadByte();
							tile.IsWallColorPresent = true;
						}
					}
					b4 = (byte)((b3 & 24) >> 3);
					if (b4 != 0)
					{
						tile.IsLiquidPresent = true;
						tile.LiquidAmount = reader.ReadByte();
						if (b4 > 1)
						{
							if (b4 == 2)
							{
								tile.IsLiquidLava = true;
							}
							else
							{
								tile.IsLiquidHoney = true;
							}
						}
					}
					if (b2 > 1)
					{
						if ((b2 & 2) == 2)
						{
							tile.IsRedWirePresent = true;
						}
						if ((b2 & 4) == 4)
						{
							tile.IsGreenWirePresent = true;
						}
						if ((b2 & 8) == 8)
						{
							tile.IsBlueWirePresent = true;
						}
						b4 = (byte)((b2 & 112) >> 4);
						//if (b4 != 0 && this.SolidTiles[(int)tile.Type])
						//{
						//	if (b4 == 1)
						//	{
						//		tile.IsHalfTile = true;
						//	}
						//	else
						//	{
						//		tile.Slope = b4;
						//	}
						//}
					}
					if (b > 0)
					{
						if ((b & 2) == 2)
						{
							tile.IsActuatorPresent = true;
						}
						if ((b & 4) == 4)
						{
							tile.IsActive = false;
						}
					}
					b4 = (byte)((b3 & 192) >> 6);
					int k;
					if (b4 == 0)
					{
						k = 0;
					}
					else
					{
						if (b4 == 1)
						{
							k = (int)reader.ReadByte();
						}
						else
						{
							k = (int)reader.ReadInt16();
						}
					}
					//if (num2 != -1)
					//{
					//	if ((double)y <= this.WorldSurfaceY)
					//	{
					//		if ((double)(y + k) <= this.worldSurface)
					//		{
					//			WorldGen.tileCounts[num2] += (k + 1) * 5;
					//		}
					//		else
					//		{
					//			int num3 = (int)(Main.worldSurface - (double)y + 1.0);
					//			int num4 = k + 1 - num3;
					//			WorldGen.tileCounts[num2] += num3 * 5 + num4;
					//		}
					//	}
					//	else
					//	{
					//		WorldGen.tileCounts[num2] += k + 1;
					//	}
					//}

					tile.Color = GetTileColor(y, tile);

					this.Tiles[x, y] = tile;
					while (k > 0)
					{
						y++;
						this.Tiles[x, y] = tile;
						k--;
					}
				}

				tilesProcessed += this.WorldHeightinTiles;

				this.ProgressMaximum = this.totalTileCount;
				this.ProgressValue = tilesProcessed;
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

					tile.Color = GetTileColor(y, tile);

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

		private Color GetTileColor(int y, Tile tile)
		{
			Color color = Color.Transparent;

			var tileInfo = this.staticData.TileInfos[tile.Type, tile.TextureU, tile.TextureV];

			if (tile.IsActive)
			{
				if (string.IsNullOrEmpty(tileInfo.ColorName))
				{
					color = MapHelper.GetTileColor(tile.Type, tile.TextureU, tile.TextureV);
				}
				else if (tileInfo.IsColorSet)
				{
					color = tileInfo.Color;
				}
				else
				{
					color = tileInfo.Color = ColorTranslator.FromHtml(tileInfo.ColorName);

					tileInfo.IsColorSet = true;
				}
			}
			else if (tile.IsLiquidPresent)
			{
				if (tile.IsLiquidLava)
					color = this.staticData.GlobalColors.LavaColor; // Terraria.MapHelper.GetLiquidColor(1);
				else if (tile.IsLiquidHoney)
					color = this.staticData.GlobalColors.HoneyColor; // Terraria.MapHelper.GetLiquidColor(2);
				else
					color = this.staticData.GlobalColors.WaterColor; // Terraria.MapHelper.GetLiquidColor(0);
			}
			else if (tile.IsWallPresent)
			{
				WallInfo? wallInfo = null;

				if (tile.WallType - 1 > -1 && tile.WallType - 1 < this.staticData.WallInfos.Count)
					wallInfo = this.staticData.WallInfos[tile.WallType - 1];

				if (wallInfo == null || string.IsNullOrEmpty(wallInfo.Value.ColorName))
					color = MapHelper.GetWallColor(tile.WallType);
				else
					color = wallInfo.Value.Color;
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
			return color;
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

		private void ReadChestsVersion2(BinaryReader reader)
		{
			this.Chests = new List<Chest>();

			int num = (int)reader.ReadInt16();
			int num2 = (int)reader.ReadInt16();
			int num3;
			int num4;

			int maxItems = 40;

			if (num2 < maxItems)
			{
				num3 = num2;
				num4 = 0;
			}
			else
			{
				num3 = maxItems;
				num4 = num2 - maxItems;
			}
			int i;
			for (i = 0; i < num; i++)
			{
				Chest chest = new Chest();
				chest.X = reader.ReadInt32();
				chest.Y = reader.ReadInt32();
				chest.Name = reader.ReadString();
				for (int j = 0; j < num3; j++)
				{
					short num5 = reader.ReadInt16();
					Item item = new Item();
					if (num5 > 0)
					{
						item.Id = reader.ReadInt32();
						item.Count = (int)num5;
						item.PrefixId = reader.ReadByte();
						chest.Items.Add(item);

						if (item.Id != 0 && this.StaticData.ItemInfos.ContainsKey(item.Id))
						{
							var itemInfo = this.StaticData.ItemInfos[item.Id];
							item.Name = itemInfo.Name;
						}

						if (item.PrefixId > 0 && this.StaticData.ItemPrefixes.Count > item.PrefixId)
							item.Name = this.StaticData.ItemPrefixes[item.PrefixId].Name + " " + item.Name;
					}
				}
				for (int j = 0; j < num4; j++)
				{
					short num5 = reader.ReadInt16();
					if (num5 > 0)
					{
						reader.ReadInt32();
						reader.ReadByte();
					}
				}
				this.Chests.Add(chest);
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

		private void ReadSignsVersion2(BinaryReader reader)
		{
			this.Signs = new List<Sign>();

			short num = reader.ReadInt16();
			int i;
			for (i = 0; i < (int)num; i++)
			{
				string text = reader.ReadString();
				int x = reader.ReadInt32();
				int y = reader.ReadInt32();
				Tile tile = this.Tiles[x, y];
				if (tile.IsActive && (tile.Type == 55 || tile.Type == 85))
				{
					Sign sign = new Sign();
					sign.Text = text;
					sign.X = x;
					sign.Y = y;
					this.Signs.Add(sign);
				}
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

		private void ReadNPCsVersion2(BinaryReader reader)
		{
			this.NPCs = new ObservableCollection<NPC>();

			int num = 0;
			bool flag = reader.ReadBoolean();
			while (flag)
			{
				NPC nPC = new NPC();
				nPC.Type = reader.ReadString();
				nPC.Name = reader.ReadString();
				nPC.X = reader.ReadSingle();
				nPC.Y = reader.ReadSingle();
				nPC.IsHomeless = reader.ReadBoolean();
				nPC.HomeX = reader.ReadInt32();
				nPC.HomeY = reader.ReadInt32();
				num++;
				flag = reader.ReadBoolean();
				this.NPCs.Add(nPC);
			}

			if (Version < 140)
				return;
			flag = reader.ReadBoolean();
			while (flag)
			{
				NPC npc = new NPC();
				npc.Type = reader.ReadString();
				npc.X = reader.ReadSingle();
				npc.Y = reader.ReadSingle();
				num++;
				flag = reader.ReadBoolean();
			}
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

		public async Task WritePixelDataAsync(byte[] pixelData, int rawStride, IEnumerable<ObjectInfoViewModel> objectTypesToHighlight = null)
		{
			await Task.Factory.StartNew(() =>
			{
				this.WritePixelData(pixelData, rawStride, objectTypesToHighlight);
			});
		}

		public void WritePixelData(byte[] buffer, int rawStride, IEnumerable<ObjectInfoViewModel> objectTypesToHighlight = null)
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
					//if (x == 47 && y == 715)
					//	Debug.Write("");

					var tile = this.Tiles[x, y];

					Color color = tile.Color;

					if (objectTypesToHighlight != null)
					{
						var tileMatches = IsTileMatch(objectTypesToHighlight, x, y, tile);

						if (tileMatches)
						{
							this.HighlightedTileCount++;
						}
						else
						{
							color = Color.Black;
						}
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
			}
			);
		}

		public bool IsTileMatch(IEnumerable<ObjectInfoViewModel> objectTypesToHighlight, int x, int y, Tile tile, TileHitTestInfo currentTile = null)
		{
			if (objectTypesToHighlight == null)
				return true;

			foreach (ObjectInfoViewModel objectTypeToHighlight in objectTypesToHighlight)
			{
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
						continue;

					var chest = this.Chests.FirstOrDefault(c => (c.X == x || c.X + 1 == x) && (c.Y == y || c.Y + 1 == y));
					if (chest == null)
						continue;

					if (currentTile != null)
					{
						var currentChest = this.Chests.FirstOrDefault(c => (c.X == currentTile.X || c.X + 1 == currentTile.X) && (c.Y == currentTile.Y || c.Y + 1 == currentTile.Y));
						if (chest == currentChest)
							continue;
					}

					foreach (var item in chest.Items)
					{
						if (item.Id == objectTypeToHighlight.ItemInfo.Id)
						{
							return true;
						}
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
					var tileInfo = this.staticData.TileInfos[tile.Type, tile.TextureU, tile.TextureV];

					name = tileInfo.Name;

					if (tileInfo.Id == 21)
					{
						var chest = this.Chests.FirstOrDefault(c => (c.X == x || c.X + 1 == x) && (c.Y == y || c.Y + 1 == y));

						if (chest != null && !string.IsNullOrEmpty(chest.Name))
							name += ": \"" + chest.Name + "\"";
					}
					else if (tileInfo.Name == "Sign")
					{
						var sign = this.Signs.FirstOrDefault(c => (c.X == x || c.X + 1 == x) && (c.Y == y || c.Y + 1 == y));

						if (sign != null && !string.IsNullOrEmpty(sign.Text))
							name += ": \"" + sign.Text + "\"";
					}
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
					name = this.staticData.WallInfos[tile.WallType - 1].Name;
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
				stringBuilder.AppendFormat("{0}: {1}", property.Name, property.GetValue(this, null));
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
