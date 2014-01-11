using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	public class Sign
	{
		public string Text { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public static Sign Read(BinaryReader reader)
		{
			if (!reader.ReadBoolean())
				return null;

			Sign sign = new Sign();

			sign.Text = reader.ReadString();
			sign.X = reader.ReadInt32();
			sign.Y = reader.ReadInt32();

			return sign;
		}
	}
}
