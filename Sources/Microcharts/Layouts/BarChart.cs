namespace Microcharts
{
	using System;
	using SkiaSharp;
	using System.Linq;

	public class BarChart : PointChart
	{
		#region Constructors

		public BarChart()
		{
			this.PointSize = 0;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the bar background area alpha.
		/// </summary>
		/// <value>The bar area alpha.</value>
		public byte BarAreaAlpha { get; set; } = 32;

		#endregion

		#region Methods

		/// <summary>
		/// Draws the value bars.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="points">Points.</param>
		/// <param name="itemSize">Item size.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="headerHeight">Header height.</param>
		protected void DrawBars(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float origin, float headerHeight)
		{
			const float minBarHeight = 4;
			if (points.Length > 0)
			{
				for (int i = 0; i < this.Entries.Count(); i++)
				{
					var entry = this.Entries.ElementAt(i);
					var point = points[i];

					using (var paint = new SKPaint
					{
						Style = SKPaintStyle.Fill,
						Color = entry.Color,
					})
					{
						var x = point.X - itemSize.Width / 2;
						var y = Math.Min(origin, point.Y);
						var height = Math.Max(minBarHeight, Math.Abs(origin - point.Y));
						if (height < minBarHeight)
						{
							height = minBarHeight;
							if (y + height > this.Margin + itemSize.Height)
							{
								y = headerHeight + itemSize.Height - height;
							}
						}

						var rect = SKRect.Create(x, y, itemSize.Width, height);
						canvas.DrawRect(rect, paint);
					}
				}
			}
		}

		/// <summary>
		/// Draws the bar background areas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="points">Points.</param>
		/// <param name="itemSize">Item size.</param>
		/// <param name="headerHeight">Header height.</param>
		protected void DrawBarAreas(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float headerHeight)
		{
			if (points.Length > 0 && this.PointAreaAlpha > 0)
			{
				for (int i = 0; i < points.Length; i++)
				{
					var entry = this.Entries.ElementAt(i);
					var point = points[i];

					using (var paint = new SKPaint
					{
						Style = SKPaintStyle.Fill,
						Color = entry.Color.WithAlpha(this.BarAreaAlpha),
					})
					{
						var max = entry.Value > 0 ? headerHeight : headerHeight + itemSize.Height;
						var height = Math.Abs(max - point.Y);
						var y = Math.Min(max, point.Y);
						canvas.DrawRect(SKRect.Create(point.X - itemSize.Width / 2, y, itemSize.Width, height), paint);
					}
				}
			}
		}

		public override void DrawContent(SKCanvas canvas, int width, int height)
		{
			var valueLabelSizes = MeasureValueLabels();
			var footerHeight = CalculateFooterHeight(valueLabelSizes);
			var headerHeight = CalculateHeaderHeight(valueLabelSizes);
			var itemSize = CalculateItemSize(width, height, footerHeight, headerHeight);
			var origin = CalculateYOrigin(itemSize.Height, headerHeight);
			var points = this.CalculatePoints(itemSize, origin, headerHeight);

			this.DrawBarAreas(canvas, points, itemSize, headerHeight);
			this.DrawBars(canvas, points, itemSize, origin, headerHeight);
			this.DrawPoints(canvas, points);
			this.DrawFooter(canvas, points, itemSize, height, footerHeight);
			this.DrawValueLabel(canvas, points, itemSize, height, valueLabelSizes);
		}

  		#endregion
	}
}
