using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TerraMap
{
  public static class Commands
  {
    static Commands()
    {
      ZoomToFit = new RoutedUICommand();
      ZoomToOriginal = new RoutedUICommand();
      NavigateToSpawn = new RoutedUICommand();
      NavigateToDungeon = new RoutedUICommand();
      HighlightSet = new RoutedUICommand();
    }

    public static RoutedUICommand ZoomToFit { get; private set; }
    public static RoutedUICommand ZoomToOriginal { get; private set; }

    public static RoutedUICommand NavigateToSpawn { get; private set; }
    public static RoutedUICommand NavigateToDungeon { get; private set; }

    public static RoutedUICommand HighlightSet { get; private set; }
  }
}
