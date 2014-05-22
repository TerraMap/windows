using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TerraMap.Data;

namespace TerraMap
{
	public partial class TileSelectionWindow : OwnedWindow
	{
		CollectionViewSource tileInfoViewSource;

		public TileSelectionWindow()
		{
			InitializeComponent();

			tileInfoViewSource = (CollectionViewSource)this.FindResource("tileInfoViewSource");
		}

		public MainWindowViewModel ViewModel
		{
			get
			{
				return this.DataContext as MainWindowViewModel;
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.searchBox.Focus();
		}

		private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			this.tileInfoViewSource.View.Refresh();
			this.tileInfoViewSource.View.MoveCurrentToFirst();
		}

		private void OnFilter(object sender, FilterEventArgs e)
		{
			e.Accepted = false;

			var tileInfo = e.Item as ObjectInfoViewModel;
			if (tileInfo == null)
				return;

			e.Accepted = tileInfo.Name.ToLower().Contains(this.searchBox.Text.ToLower());
		}

		private void OnAccept(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void OnCancel(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void OnItemActivate(object sender, ItemActivateEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void OnCheckAll(object sender, RoutedEventArgs e)
		{
			this.SetIsSelected(true);
		}

		private void OnUncheckAll(object sender, RoutedEventArgs e)
		{
			this.SetIsSelected(false);
		}

		private void SetIsSelected(bool isSelected)
		{
			if (tileInfoViewSource == null || tileInfoViewSource.View == null)
				return;

			foreach (var item in tileInfoViewSource.View.OfType<ObjectInfoViewModel>())
			{
				item.IsSelected = isSelected;
			}
		}
	}
}
