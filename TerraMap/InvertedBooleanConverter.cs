using System;
using System.Windows.Data;

namespace TerraMap
{
	public class InvertedBooleanConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is bool))
				return value;

			return !(bool)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (!(value is bool))
				return value;

			return !(bool)value;
		}

		#endregion
	}
}
