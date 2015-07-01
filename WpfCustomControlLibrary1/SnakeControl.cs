using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCustomControlsLibrary
{
	/// <summary>
	/// An ItemsControl derived class which provides a `snaking` layout, list binding and scrolling.
	/// </summary>
	public class SnakeControl : ItemsControl
	{
		static SnakeControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SnakeControl), new FrameworkPropertyMetadata(typeof(SnakeControl)));
		}
	}
}