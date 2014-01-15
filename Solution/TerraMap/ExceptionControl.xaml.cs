using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TerraMap
{
	public partial class ExceptionControl : UserControl
	{
		public ExceptionControl()
		{
			InitializeComponent();
		}

		public Visibility CancelButtonVisibility
		{
			get { return (Visibility)GetValue(CancelButtonVisibilityProperty); }
			set { SetValue(CancelButtonVisibilityProperty, value); }
		}

		public static readonly DependencyProperty CancelButtonVisibilityProperty =
			 DependencyProperty.Register("CancelButtonVisibility", typeof(Visibility), typeof(ExceptionControl), new UIPropertyMetadata(Visibility.Collapsed));

		private void OnCopyCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this.DataContext != null;
		}

		private void OnCopyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				if (this.DataContext != null)
					Clipboard.SetText(this.DataContext.ToString());
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				MessageBox.Show("Unable to set clipboard text.");
			}
		}
	}
}
