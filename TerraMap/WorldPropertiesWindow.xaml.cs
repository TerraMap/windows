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
	public partial class WorldPropertiesWindow : OwnedWindow
	{
		CollectionViewSource collectionViewSource;

		public WorldPropertiesWindow()
		{
			InitializeComponent();

			collectionViewSource = (CollectionViewSource)this.FindResource("collectionViewSource");
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.searchBox.Focus();
		}

		private void OnFilter(object sender, FilterEventArgs e)
		{
			e.Accepted = false;

			var item = e.Item as WorldProperty;
			if (item == null)
				return;

			e.Accepted = item.Name.ToLower().Contains(this.searchBox.Text.ToLower());
		}

		private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			this.collectionViewSource.View.Refresh();
			this.collectionViewSource.View.MoveCurrentToFirst();
		}

		private void OnSearchBoxClear(object sender, RoutedEventArgs e)
		{
			this.searchBox.Text = "";
		}

		private void OnCanExecuteClose(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OnExecutedClose(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}
	}
}
