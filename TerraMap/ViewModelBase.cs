using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TerraMap
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (this.PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
