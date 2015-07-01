using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCustomControlsLibrary
{
	/// <summary>
	/// Custom panel which provides a `snaking` layout
	/// </summary>
	public class SnakePanel : Panel
	{
		/// <summary>
		/// From MSDN : When overridden in a derived class, measures the 
		/// size in layout required for child elements and determines a
		/// size for the FrameworkElement-derived class
		/// </summary>
		protected override Size MeasureOverride(Size constraint)
		{
			var currentRowSize = new Size();
			var panelSize = new Size();

			foreach (UIElement element in this.InternalChildren)
			{
				element.Measure(constraint);
				var desiredSize = element.DesiredSize;

				if (currentRowSize.Width + desiredSize.Width > constraint.Width)
				{
					// place the current element on a new row because space has run out
					panelSize.Width = Math.Max(currentRowSize.Width, panelSize.Width);
					panelSize.Height += currentRowSize.Height;
					currentRowSize = desiredSize;
				}
				else
				{
					// keep adding to the current row
					currentRowSize.Width += desiredSize.Width;

					// make sure the line is as tall as its tallest element
					currentRowSize.Height = Math.Max(desiredSize.Height, currentRowSize.Height);
				}
			}

			// return the size required to fit all elements. add the dimensions of the last row which is not full
			panelSize.Width = Math.Max(currentRowSize.Width, panelSize.Width);
			panelSize.Height += currentRowSize.Height;

			return panelSize;
		}

		/// <summary>
		/// From MSDN : When overridden in a derived class, positions child elements
		/// and determines a size for a FrameworkElement derived class.
		/// </summary>
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			var firstItemIndex = 0;
			var currentRowSize = new Size();
			var accumulatedHeight = 0d;
			var elements = this.InternalChildren;
			var isEvenRow = true;

			for (int i = 0; i < elements.Count; i++)
			{
				var desiredSize = elements[i].DesiredSize;

				if (currentRowSize.Width + desiredSize.Width > arrangeBounds.Width)
				{
					// need to switch to another row
					this.ArrangeRow(accumulatedHeight, currentRowSize.Height, arrangeBounds, isEvenRow, firstItemInRow: firstItemIndex, lastItemInRow: i);

					accumulatedHeight += currentRowSize.Height;
					currentRowSize = desiredSize;
					isEvenRow = !isEvenRow;

					firstItemIndex = i;
				}
				else
				{
					//continue to accumulate a row
					currentRowSize.Width += desiredSize.Width;
					currentRowSize.Height = Math.Max(desiredSize.Height, currentRowSize.Height);
				}
			}

			// there are unarranged items. arrange them
			if (firstItemIndex < elements.Count)
				this.ArrangeRow(accumulatedHeight, currentRowSize.Height, arrangeBounds, isEvenRow, firstItemInRow: firstItemIndex, lastItemInRow: elements.Count);

			return arrangeBounds;
		}

		private void ArrangeRow(double accumulatedHeight, double rowHeight, Size arrangeBounds, bool isEvenRow, int firstItemInRow, int lastItemInRow)
		{
			var tallestChildHeight = 0d;
			var yOffset = 0d;
			var children = this.InternalChildren;
			UIElement child;

			// calculate the row's tallest element. its height will be used later to center all elements in the row
			for (int i = firstItemInRow; i < lastItemInRow; i++)
			{
				child = children[i];
				if (child.DesiredSize.Height > tallestChildHeight)
				{
					tallestChildHeight = child.DesiredSize.Height;
				}
			}

			var x = isEvenRow ? 0 : arrangeBounds.Width;

			// position the elements in row
			for (int i = firstItemInRow; i < lastItemInRow; i++)
			{
				child = children[i];
				// center the element vertically if needed
				if (child.DesiredSize.Height < tallestChildHeight)
				{
					yOffset = ((tallestChildHeight - child.DesiredSize.Height) / 2);
				}

				// if its an odd row, position the elements from right to left
				if (!isEvenRow)
				{
					x -= child.DesiredSize.Width;
				}

				child.Arrange(new Rect(x, accumulatedHeight + yOffset, child.DesiredSize.Width, rowHeight));
				yOffset = 0;

				// if its an even row, position the elements from left to right
				if (isEvenRow)
				{
					x += child.DesiredSize.Width;
				}
			}
		}
	}
}
