using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCustomControlsLibrary
{
	// TODO: do we need this control?
	public class SnakeControl : ItemsControl
	{
		static SnakeControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SnakeControl), new FrameworkPropertyMetadata(typeof(SnakeControl)));
		}
	}
}