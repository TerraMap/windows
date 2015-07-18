using System;
using System.Windows;
using System.Windows.Data;

namespace TerraMap
{
	public class InvertedBooleanToVisibilityConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is bool))
				return value;

			return (bool)value ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is Visibility))
				return value;

			return ((Visibility)value) == Visibility.Collapsed ? true : false;
		}

		#endregion
	}
}
