using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	public class ObjectInfo
	{
		public string Name { get; set; }
		public string Type { get; set; }

		public override string ToString()
		{
			return this.Name;
		}
	}
}
