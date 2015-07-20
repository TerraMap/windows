using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerraMap.Data
{
	public class ProgressEventArgs : EventArgs
	{
		public int Value { get; private set; }
		public int Maximum { get; private set; }

		public ProgressEventArgs(int value, int maximum)
		{
			this.Value = value;
			this.Maximum = maximum;
		}
	}
}
