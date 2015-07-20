using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TerraMap.Data;

namespace TerraMap.Data
{
  public class ObjectInfoSetViewModel : ObjectInfoViewModel
  {
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

    private string inputGestureText;

    public string InputGestureText
    {
      get { return inputGestureText; }
      set
      {
        inputGestureText = value;
        RaisePropertyChanged();
      }
    }

		private ObjectInfoViewModel selectedItem;

		public ObjectInfoViewModel SelectedItem
		{
			get { return selectedItem; }
			set { selectedItem = value; }
		}

		public override string Type
		{
			get
			{
				return "Set";
			}
		}
	}
}
