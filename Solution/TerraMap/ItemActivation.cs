using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TerraMap
{
	public static class ItemActivation
	{
		public delegate void ItemActivateRoutedEventHandler(object sender, ItemActivateEventArgs e);

		public static readonly RoutedEvent ItemActivateEvent = EventManager.RegisterRoutedEvent("ItemActivate", RoutingStrategy.Bubble, typeof(ItemActivateRoutedEventHandler), typeof(ItemActivation));

		public static void AddItemActivateHandler(DependencyObject o, RoutedEventHandler handler)
		{
			((UIElement)o).AddHandler(ItemActivation.ItemActivateEvent, handler);
		}

		public static void RemoveItemActivateHandler(DependencyObject o, RoutedEventHandler handler)
		{
			((UIElement)o).RemoveHandler(ItemActivation.ItemActivateEvent, handler);
		}

		public static ActivationMode GetActivationMode(DependencyObject obj)
		{
			return (ActivationMode)obj.GetValue(ActivationModeProperty);
		}

		public static void SetActivationMode(DependencyObject obj, ActivationMode value)
		{
			obj.SetValue(ActivationModeProperty, value);
		}

		public static readonly DependencyProperty ActivationModeProperty = DependencyProperty.RegisterAttached(
			"ActivationMode", typeof(ActivationMode), typeof(ItemActivation), new FrameworkPropertyMetadata(ActivationMode.None, ItemActivation.HandleActivationModeChanged));

		private static MouseButtonEventHandler ItemsControlMouseDoubleClickHandler = new MouseButtonEventHandler(ItemActivation.HandleItemsControlMouseDoubleClick);
		private static KeyEventHandler ItemsControlKeyDownHandler = new KeyEventHandler(ItemActivation.HandleItemsControlKeyDown);

		private static void HandleActivationModeChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
		{
			var itemsControl = target as ItemsControl;

			// if trying to attach to something else than a ItemsControl, just ignore
			if (target == null)
				return;

			ActivationMode newActivation = (ActivationMode)e.NewValue;

			if ((newActivation & ActivationMode.Mouse) == ActivationMode.Mouse)
			{
				itemsControl.MouseDoubleClick += ItemsControlMouseDoubleClickHandler;
			}
			else
			{
				itemsControl.MouseDoubleClick -= ItemsControlMouseDoubleClickHandler;
			}

			if ((newActivation & ActivationMode.Keyboard) == ActivationMode.Keyboard)
			{
				itemsControl.KeyDown += ItemsControlKeyDownHandler;
			}
			else
			{
				itemsControl.KeyDown -= ItemsControlKeyDownHandler;
			}
		}

		static void HandleItemsControlMouseDoubleClick(object o, MouseButtonEventArgs e)
		{
			var sender = o as ItemsControl;
			DependencyObject originalSender = e.OriginalSource as DependencyObject;

			if (sender == null || originalSender == null)
				return;

			DependencyObject container = ItemsControl.ContainerFromElement(sender as ItemsControl, e.OriginalSource as DependencyObject);

			if (container is ItemsControl)
			{
				DependencyObject nestedContainer = null;
				// see if the container returned is actually a parent (in case of TreeView, the first container we get back is of the root node)
				while ((nestedContainer = ItemsControl.ContainerFromElement(container as ItemsControl, e.OriginalSource as DependencyObject)) != null)
				{
					container = nestedContainer;
				}
			}

			// just in case, check if the double click doesn't come from somewhere else than something in a container
			if (container == null || container == DependencyProperty.UnsetValue)
				return;

			// found a container, now find the item.
			object activatedItem = sender.ItemContainerGenerator.ItemFromContainer(container);

			if (activatedItem != null && activatedItem != DependencyProperty.UnsetValue)
				sender.RaiseEvent(new ItemActivateEventArgs(ItemActivation.ItemActivateEvent, sender, activatedItem, ActivationMode.Mouse));
		}

		static void HandleItemsControlKeyDown(object o, KeyEventArgs e)
		{
			if (e.Key != Key.Enter)
				return;

			ItemsControl sender = o as ItemsControl;
			DependencyObject originalSender = e.OriginalSource as DependencyObject;

			if (sender == null || originalSender == null)
				return;

			DependencyObject container = ItemsControl.ContainerFromElement(sender as ItemsControl, e.OriginalSource as DependencyObject);

			// just in case, check if the double click doesn't come from somewhere else than something in a container
			if (container == null || container == DependencyProperty.UnsetValue)
				return;

			// found a container, now find the item.
			object activatedItem = sender.ItemContainerGenerator.ItemFromContainer(container);

			if (activatedItem != null && activatedItem != DependencyProperty.UnsetValue)
				sender.RaiseEvent(new ItemActivateEventArgs(ItemActivation.ItemActivateEvent, sender, activatedItem, ActivationMode.Mouse));
		}
	}

	public enum ActivationMode
	{
		None,
		Mouse,
		Keyboard,
	}

	public class ItemActivateEventArgs : RoutedEventArgs
	{
		public ActivationMode ActivationMode { get; private set; }
		public object ActivatedItem { get; private set; }

		public ItemActivateEventArgs(RoutedEvent routedEvent, ItemsControl source, object activatedItem, ActivationMode activationMode)
			: base(routedEvent, source)
		{
			this.ActivatedItem = activatedItem;
			this.ActivationMode = activationMode;
		}
	}
}
