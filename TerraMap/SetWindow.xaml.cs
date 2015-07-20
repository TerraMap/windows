using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
	public partial class SetWindow : OwnedWindow
	{
		SetWindowViewModel viewModel;

		public SetWindow(Window owner = null) : base(owner)
		{
			InitializeComponent();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.viewModel = this.DataContext as SetWindowViewModel;
		}

		private void OnAccept(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void OnCanExecuteNew(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OnExecutedNew(object sender, ExecutedRoutedEventArgs e)
		{
			var newViewModel = new MainWindowViewModel();
			newViewModel.ObjectInfoViewModels = this.viewModel.ObjectInfoViewModels;

			foreach (var item in newViewModel.ObjectInfoViewModels)
				item.IsSelected = false;

			var window = new TileSelectionWindow(this);
			window.DataContext = newViewModel;

			window.ShowDialog();

			foreach (var item in newViewModel.ObjectInfoViewModels.Where(v => v.IsSelected))
			{
				if (item.TileInfo != null)
				{
					if (string.IsNullOrWhiteSpace(item.TileInfo.Name))
					{
						if (this.viewModel.Set.ObjectInfoViewModels.Any(o => o.TileInfo != null && o.TileInfo.Id == item.TileInfo.Id))
							continue;
					}
					else
					{
						if (this.viewModel.Set.ObjectInfoViewModels.Any(o => o.TileInfo != null && o.TileInfo.Name == item.TileInfo.Name))
							continue;
					}

					this.viewModel.Set.ObjectInfoViewModels.Add(item);
					continue;
				}

				if (item.ItemInfo != null)
				{
					if (string.IsNullOrWhiteSpace(item.ItemInfo.Name))
					{
						if (this.viewModel.Set.ObjectInfoViewModels.Any(o => o.ItemInfo != null && o.ItemInfo.Id == item.ItemInfo.Id))
							continue;
					}
					else
					{
						if (this.viewModel.Set.ObjectInfoViewModels.Any(o => o.ItemInfo != null && o.ItemInfo.Name == item.ItemInfo.Name))
							continue;
					}

					this.viewModel.Set.ObjectInfoViewModels.Add(item);
					continue;
				}

				if (string.IsNullOrWhiteSpace(item.WallInfo.Name))
				{
					if (this.viewModel.Set.ObjectInfoViewModels.Any(o => o.WallInfo.Id == item.WallInfo.Id))
						continue;
				}
				else
				{
					if (this.viewModel.Set.ObjectInfoViewModels.Any(o => o.WallInfo.Name == item.WallInfo.Name))
						continue;
				}

				this.viewModel.Set.ObjectInfoViewModels.Add(item);
				continue;
			}
		}
		
		private void OnCanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.viewModel?.SelectedItem != null;
		}

		private void OnExecutedDelete(object sender, ExecutedRoutedEventArgs e)
		{
			var selectedItem = this.viewModel?.SelectedItem;
			if (selectedItem == null)
				return;

			var selectedItems = this.viewModel.Set.ObjectInfoViewModels.Where(v => v.IsSelected).ToList();

			if (selectedItems.Count() == 1)
			{
				var result = MessageBox.Show(this, "Are you sure you want to delete this item?", "Delete Item?", MessageBoxButton.YesNo);

				if (result != MessageBoxResult.Yes)
					return;

				this.viewModel.Set.ObjectInfoViewModels.Remove(selectedItem);
			}
			else
			{
				var result = MessageBox.Show(this, string.Format("Are you sure you want to delete these {0} items?", selectedItems.Count()), "Delete Items?", MessageBoxButton.YesNo);

				if (result != MessageBoxResult.Yes)
					return;
				
				foreach(var item in selectedItems)
					this.viewModel.Set.ObjectInfoViewModels.Remove(item);
			}
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
