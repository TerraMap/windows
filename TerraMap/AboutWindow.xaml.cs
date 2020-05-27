using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TerraMap
{
  public partial class AboutWindow : OwnedWindow
  {
    public AboutWindow()
    {
      InitializeComponent();
    }

    private void OnCloseClicked(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void OnHyperLinkClicked(object sender, RoutedEventArgs e)
    {
      if (!(sender is Hyperlink hyperlink))
        return;

      Process.Start(hyperlink.NavigateUri.ToString());
    }
  }
}
