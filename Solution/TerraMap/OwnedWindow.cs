using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace TerraMap
{
	public class OwnedWindow : Window
	{
		public OwnedWindow(Window owner = null)
			: base()
		{
			this.Owner = owner;
			this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

			this.ShowInTaskbar = false;
			WindowControlBox.SetHasMinimizeButton(this, false);
		}
	}
}