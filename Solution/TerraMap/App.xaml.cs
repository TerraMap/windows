using System.Windows;
using System.Windows.Threading;

namespace TerraMap
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			this.Properties["Args"] = e.Args;
		}

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			ExceptionWindow.ShowDialog(e.Exception);
			e.Handled = true;
		}
	}
}
