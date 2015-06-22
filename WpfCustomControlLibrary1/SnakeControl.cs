using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCustomControlsLibrary
{
	public class SnakeControl : ItemsControl
	{
		static SnakeControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(SnakeControl), new FrameworkPropertyMetadata(typeof(SnakeControl)));
		}
	}
}