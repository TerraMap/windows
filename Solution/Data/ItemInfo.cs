using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	public class ItemInfo : ObjectInfo
	{
		public int Id { get; set; }

		public ItemInfo()
			: base()
		{
			this.Type = "Item";
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
