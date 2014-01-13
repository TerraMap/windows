using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TerraMap
{
	public partial class App : Application
	{
		[DllImport("Kernel32.dll")]
		public static extern bool AttachConsole(int processId);

		[DllImport("kernel32.dll")]
		public static extern Boolean FreeConsole();

		public Options Options { get; private set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			this.Options = new Options();

			var commandLineParsed = CommandLine.Parser.Default.ParseArguments(e.Args, this.Options);

			if (AttachConsole(-1))
			{
				Console.WriteLine("Attach debugger now and press any key...");
				Console.ReadKey();

				Console.WriteLine("");

				try
				{
					if (this.Options.NoGui)
					{
						this.RenderInConsole();
					}
				}
				finally
				{
					Console.WriteLine("Press a key to quit");

					FreeConsole();

					this.Shutdown();
				}
			}
			else
			{
				new MainWindow().Show();
			}
		}

		private void RenderInConsole()
		{
		}

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString());
			e.Handled = true;
		}
	}
}
