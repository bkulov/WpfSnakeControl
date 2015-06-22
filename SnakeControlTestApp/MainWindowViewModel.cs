using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SnakeControlTestApp
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public IEnumerable<string> Items
		{
			get
			{
				for (int i = 0; i < 20; i++)
				{
					yield return "Item " + i;
				}
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			var handler = this.PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
