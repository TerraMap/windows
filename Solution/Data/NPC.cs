using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	public class NPC
	{
		public string Type { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public bool IsHomeless { get; set; }
		public Int32 HomeX { get; set; }
		public Int32 HomeY { get; set; }

		public static NPC Read(BinaryReader reader)
		{
			if (!reader.ReadBoolean())
				return null;

			NPC npc = new NPC();

			npc.Type = reader.ReadString();
			npc.X = reader.ReadSingle();
			npc.Y = reader.ReadSingle();
			npc.IsHomeless = reader.ReadBoolean();
			npc.HomeX = reader.ReadInt32();
			npc.HomeY = reader.ReadInt32();

			return npc;
		}

		public string Name { get; set; }
	}
}
