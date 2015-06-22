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
				for (int i = 0; i < 10; i++)
				{
					yield return "Long long long item " + i;
				}

				yield return "A very looooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooong item";

				for (int i = 10; i < 30; i++)
				{
					yield return "Very long long long long item " + i;
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
