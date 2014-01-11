using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TerraMap.Data;

namespace TerraMap
{
	public class ObjectInfoTemplateSelector : DataTemplateSelector
	{
		public DataTemplate TileInfoTemplate { get; set; }
		public DataTemplate ObjectInfoTemplate { get; set; }

		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (item is TileInfo)
				return TileInfoTemplate;
			else if (item is ObjectInfo)
				return ObjectInfoTemplate;

			return base.SelectTemplate(item, container);
		}
	}
}
