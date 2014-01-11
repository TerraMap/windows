using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	public class PropertyInfoAttribute : Attribute
	{
		public int MinimumVersion { get; set; }
		public int MaximumVersion { get; set; }
		public int Count { get; set; }
		public bool Ignore { get; set; }

		public PropertyInfoAttribute(int minimumVersion = 0, int count = 1, int maximumVersion = int.MaxValue, bool ignore = false)
		{
			this.MinimumVersion = minimumVersion;
			this.Count = count;
			this.MaximumVersion = maximumVersion;
			this.Ignore = ignore;
		}
	}
}
