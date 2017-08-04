using System;
namespace Microcharts
{
	using SkiaSharp;
	using System.Linq;

	public class BarChart : PointChart
	{
		public BarChart()
		{
			this.PointSize = 0;
		}

		public byte BarAreaAlpha { get; set; } = 32;

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
						Color = entry.FillColor,
					})
					{
						var x = point.X - itemSize.Width / 2;
						var y = Math.Min(origin, point.Y);
						var height = Math.Max(minBarHeight, Math.Abs(origin - point.Y));
						if(height < minBarHeight)
						{
							height = minBarHeight;
							if(y + height > this.Margin + itemSize.Height)
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
						Color = entry.FillColor.WithAlpha(this.BarAreaAlpha),
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
	}
}
