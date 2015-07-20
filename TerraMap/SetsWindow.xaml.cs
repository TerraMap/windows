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
	public partial class SetsWindow : OwnedWindow
	{
		MainWindowViewModel viewModel;

		public SetsWindow(Window owner = null) : base(owner)
		{
			InitializeComponent();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.viewModel = this.DataContext as MainWindowViewModel;
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
			var item = new ObjectInfoSetViewModel()
			{
				Name = "New (Set)",
				ObjectInfoViewModels = new ObservableCollection<ObjectInfoViewModel>(),
				IsSelected = true
			};

			this.viewModel.Sets.Add(item);
			this.viewModel.SelectedSet = item;

			this.EditSet();
		}

		private void OnCanExecuteProperties(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.viewModel?.SelectedSet != null;
		}

		private void OnExecutedProperties(object sender, ExecutedRoutedEventArgs e)
		{
			this.EditSet();
		}

		private void EditSet()
		{
			var set = this.viewModel?.SelectedSet;
			if (set == null)
				return;

			var setWindow = new SetWindow(this);
			var setViewModel = new SetWindowViewModel()
			{
				Set = set,
			};

			foreach (var item in this.viewModel.ObjectInfoViewModels)
			{
				item.IsSelected = false;
			}

			setViewModel.ObjectInfoViewModels = this.viewModel.ObjectInfoViewModels;

			setWindow.DataContext = setViewModel;

			setWindow.ShowDialog();
		}

		private void SaveSets()
		{
			throw new NotImplementedException();
		}

		private void OnCanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.viewModel?.SelectedSet != null;
		}

		private void OnExecutedDelete(object sender, ExecutedRoutedEventArgs e)
		{
			var item = this.viewModel?.SelectedSet;
			if (item == null)
				return;

			var result = MessageBox.Show(this, "Are you sure you want to delete '" + item.Name + "'?", "Delete Set?", MessageBoxButton.YesNo);

			if (result != MessageBoxResult.Yes)
				return;

			this.viewModel.Sets.Remove(item);
		}

		private void OnCanExecuteClose(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OnExecutedClose(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}

		private void OnCanExecuteDown(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.viewModel?.SelectedSet != null && this.viewModel.Sets.Count > 1 && this.viewModel.Sets.IndexOf(this.viewModel.SelectedSet) < this.viewModel.Sets.Count - 1;
		}

		private void OnExecutedDown(object sender, ExecutedRoutedEventArgs e)
		{
			var set = this.viewModel?.SelectedSet;
			if (set == null)
				return;

			if (this.viewModel.Sets.Count < 2)
				return;

			var index = this.viewModel.Sets.IndexOf(set);

      if (index > this.viewModel.Sets.Count - 1)
				return;

			this.viewModel.Sets.Remove(set);

			this.viewModel.Sets.Insert(index + 1, set);

			set.IsSelected = true;
		}

		private void OnCanExecuteUp(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.viewModel?.SelectedSet != null && this.viewModel.Sets.Count > 1 && this.viewModel.Sets.IndexOf(this.viewModel.SelectedSet) > 0;
		}

		private void OnExecutedUp(object sender, ExecutedRoutedEventArgs e)
		{
			var set = this.viewModel?.SelectedSet;
			if (set == null)
				return;

			if (this.viewModel.Sets.Count < 2)
				return;

			var index = this.viewModel.Sets.IndexOf(set);

			if (index == 0)
				return;

			this.viewModel.Sets.Remove(set);

			this.viewModel.Sets.Insert(index - 1, set);

			set.IsSelected = true;
		}

		private void OnItemMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount < 2)
				return;

			this.EditSet();
		}
	}
}
