using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace TerraMap
{
	public partial class ExceptionWindow : OwnedWindow
	{
		public ExceptionWindow(Exception exception, Window owner = null)
			: base(owner)
		{
			try
			{
				InitializeComponent();

				this.DataContext = ExceptionViewModel.Create(exception);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		private Exception exception;

		public Exception Exception
		{
			get { return exception; }
			set
			{
				exception = value;

				ExceptionViewModel viewModel = ExceptionViewModel.Create(exception);

				this.DataContext = viewModel;
			}
		}

		public static void ShowDialog(Exception exception, Window owner = null)
		{
			try
			{
				Debug.WriteLine(exception);

				var window = new ExceptionWindow(exception, owner);

				window.ShowDialog();
			}
			catch (Exception)
			{
				if (owner != null)
					System.Windows.MessageBox.Show(owner, exception.ToString());
				else
					System.Windows.MessageBox.Show(exception.ToString());
			}
		}

		private void OnCloseCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.IsLoaded;
		}

		private void OnCloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				this.DialogResult = false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				switch (e.Key)
				{
					case Key.Escape:
					case Key.Enter:
						this.DialogResult = false;
						e.Handled = true;
						break;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				switch (e.Key)
				{
					case Key.Escape:
					case Key.Enter:
						this.DialogResult = false;
						e.Handled = true;
						break;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
	}
}

