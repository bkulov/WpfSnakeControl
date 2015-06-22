using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfCustomControlsLibrary
{
	internal class SnakePanel : Panel
	{
		/// <summary>
		/// From MSDN : When overridden in a derived class, measures the 
		/// size in layout required for child elements and determines a
		/// size for the FrameworkElement-derived class
		/// </summary>
		protected override Size MeasureOverride(Size constraint)
		{
			Size currentRowSize = new Size();
			Size panelSize = new Size();

			foreach (UIElement element in this.InternalChildren)
			{
				element.Measure(constraint);
				Size desiredSize = element.DesiredSize;

				if (currentRowSize.Width + desiredSize.Width > constraint.Width)
				{
					// place the current element on a new row because space has run out
					panelSize.Width = Math.Max(currentRowSize.Width, panelSize.Width);
					panelSize.Height += currentRowSize.Height;
					currentRowSize = desiredSize;

					// if the current element is larger than the constraint, put it on separate row
					if (desiredSize.Width > constraint.Width)
					{
						panelSize.Width = Math.Max(desiredSize.Width, panelSize.Width);
						panelSize.Height += desiredSize.Height;
						currentRowSize = new Size();
					}
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
			int firstItemIndex = 0;
			Size currentRowSize = new Size();
			double accumulatedHeight = 0;
			UIElementCollection elements = this.InternalChildren;

			for (int i = 0; i < elements.Count; i++)
			{
				Size desiredSize = elements[i].DesiredSize;

				// need to switch to another row
				if (currentRowSize.Width + desiredSize.Width > arrangeBounds.Width)
				{
					this.ArrangeRow(accumulatedHeight, currentRowSize.Height, firstItemInLine: firstItemIndex, lastItemInLine: i, arrangeBounds: arrangeBounds);

					accumulatedHeight += currentRowSize.Height;
					currentRowSize = desiredSize;

					// TODO: bvk - check if this is needed. probably the if above will handle the case correctly. Same applies to the MeasureOverride
					// if the current element is larger than the constraint, put it on separate row
					if (desiredSize.Width > arrangeBounds.Width)
					{
						this.ArrangeRow(accumulatedHeight, desiredSize.Width, firstItemInLine: i, lastItemInLine: ++i, arrangeBounds: arrangeBounds);
						accumulatedHeight += desiredSize.Height;
						currentRowSize = new Size();
					}

					firstItemIndex = i;
				}
				else
				{
					//continue to accumulate a row
					currentRowSize.Width += desiredSize.Width;
					currentRowSize.Height = Math.Max(desiredSize.Height, currentRowSize.Height);
				}
			}

			// there are an unarranged items. arrange them
			if (firstItemIndex < elements.Count)
				this.ArrangeRow(accumulatedHeight, currentRowSize.Width, firstItemInLine: firstItemIndex, lastItemInLine: elements.Count, arrangeBounds: arrangeBounds);

			return arrangeBounds;
		}

		private void ArrangeRow(double accumulatedHeight, double rowHeight, int firstItemInLine, int lastItemInLine, Size arrangeBounds)
		{
			double x = 0;
			double totalChildWidth = 0;
			double tallestChildHeight = 0;
			double yOffset = 0;

			UIElementCollection children = this.InternalChildren;
			UIElement child;

			for (int i = firstItemInLine; i < lastItemInLine; i++)
			{
				child = children[i];
				totalChildWidth += child.DesiredSize.Width;
				if (child.DesiredSize.Height > tallestChildHeight)
				{
					tallestChildHeight = child.DesiredSize.Height;
				}
			}

			// TODO: check this outs
			//work out x start offset within a given column
			x = ((arrangeBounds.Width - totalChildWidth) / 2);

			for (int i = firstItemInLine; i < lastItemInLine; i++)
			{
				child = children[i];
				if (child.DesiredSize.Height < tallestChildHeight)
				{
					yOffset = ((tallestChildHeight - child.DesiredSize.Height) / 2);
				}

				child.Arrange(new Rect(x, accumulatedHeight + yOffset, child.DesiredSize.Width, rowHeight));
				x += child.DesiredSize.Width;
				yOffset = 0;
			}
		}
	}
}
