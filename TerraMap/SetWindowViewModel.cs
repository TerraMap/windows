using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TerraMap.Data;

namespace TerraMap
{
	public class SetWindowViewModel : ViewModelBase
	{
		private ObjectInfoSetViewModel set;

		public ObjectInfoSetViewModel Set
		{
			get { return set; }
			set { set = value; }
		}

		private ObjectInfoViewModel selectedItem;

		public ObjectInfoViewModel SelectedItem
		{
			get { return selectedItem; }
			set { selectedItem = value; }
		}

		private ObservableCollection<ObjectInfoViewModel> objectInfoViewModels = new ObservableCollection<ObjectInfoViewModel>();

		public ObservableCollection<ObjectInfoViewModel> ObjectInfoViewModels
		{
			get { return objectInfoViewModels; }
			set
			{
				objectInfoViewModels = value;
				RaisePropertyChanged();
			}
		}
	}
}
