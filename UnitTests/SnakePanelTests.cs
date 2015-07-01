using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfCustomControlsLibrary;

namespace UnitTests
{
	[TestClass]
	public class SnakePanelTests
	{
		private SnakeControl snakeControl;

		[TestInitialize]
		public void Init()
		{
			this.snakeControl = new SnakeControl();
		}

		[TestMethod]
		public void MeasureOverride_WithoutItems_DesiredSizeShouldBeEmptyRect()
		{
			// TODO: test measure override with no items

			Assert.IsTrue(true);
		}
		
		[TestMethod]
		public void MeasureOverride_WithItems_DesiredSizeShouldNotBeEmptyRect()
		{
			this.SetItems(10);

			// TODO: test measure override with items

			Assert.IsTrue(true);
		}

		// TODO: write more tests to cover the arrange override

		private void SetItems(int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.snakeControl.Items.Add("test string item " + i);
			}
		}
	}
}
