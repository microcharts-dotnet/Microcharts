using System.Collections.Generic;
namespace Microcharts
{
	using System;
	using SkiaSharp;
	using System.Linq;

	public class DonutChart : Chart
	{
		public DonutChart()
		{
		}

		public float HoleRadius { get; set; } = 0;

		#region Drawing parts

		public const float PI = (float)Math.PI;

		private const float UprightAngle = PI / 2f;
		private const float TotalAngle = 2f * PI;

		public static SKPoint GetCirclePoint(float r, float angle)
		{
			return new SKPoint(r * (float)Math.Cos(angle), r * (float)Math.Sin(angle));
		}

		public static SKPath CreateSectorPath(float start, float end, float outerRadius, float innerRadius = 0.0f, float margin = 0.0f)
		{
			var path = new SKPath();

			// if the sector has no size, then it has no path
			if (start == end)
			{
				return path;
			}

			// the the sector is a full circle, then do that
			if (end - start == 1.0f)
			{
				path.AddCircle(0, 0, outerRadius, SKPathDirection.Clockwise);
				path.AddCircle(0, 0, innerRadius, SKPathDirection.Clockwise);
				path.FillType = SKPathFillType.EvenOdd;
				return path;
			}

			// calculate the angles
			var startAngle = TotalAngle * start - UprightAngle;
			var endAngle = TotalAngle * end - UprightAngle;
			var large = endAngle - startAngle > PI ? SKPathArcSize.Large : SKPathArcSize.Small;
			var sectorCenterAngle = (endAngle - startAngle) / 2f + startAngle;

			// get the radius bits
			var cectorCenterRadius = (outerRadius - innerRadius) / 2f + innerRadius;

			// calculate the angle for the margins
			var offsetR = outerRadius == 0 ? 0 : ((margin / (TotalAngle * outerRadius)) * TotalAngle);
			var offsetr = innerRadius == 0 ? 0 : ((margin / (TotalAngle * innerRadius)) * TotalAngle);

			// get the points
			var a = GetCirclePoint(outerRadius, startAngle + offsetR);
			var b = GetCirclePoint(outerRadius, endAngle - offsetR);
			var c = GetCirclePoint(innerRadius, endAngle - offsetr);
			var d = GetCirclePoint(innerRadius, startAngle + offsetr);

			// add the points to the path
			path.MoveTo(a);
			path.ArcTo(outerRadius, outerRadius, 0, large, SKPathDirection.Clockwise, b.X, b.Y);
			path.LineTo(c);
			if (innerRadius == 0.0f)
			{
				// take a short cut
				path.LineTo(d);
			}
			else
			{
				path.ArcTo(innerRadius, innerRadius, 0, large, SKPathDirection.CounterClockwise, d.X, d.Y);
			}
			path.Close();

			return path;
		}

		#endregion

		private void DrawCaption(SKCanvas canvas, int width, int height)
		{
			var sumValue = this.Entries.Sum(x => Math.Abs(x.Value));
			var rightValues = new List<Entry>();
			var leftValues = new List<Entry>();

			int i = 0;
			var current = 0.0f;
			while (i < this.Entries.Count() && (current < sumValue / 2))
			{
				var entry = this.Entries.ElementAt(i);
				rightValues.Add(entry);
				current += Math.Abs(entry.Value);
				i++;
			}

			while (i < this.Entries.Count())
			{
				var entry = this.Entries.ElementAt(i);
				leftValues.Add(entry);
				i++;
			}

			leftValues.Reverse();

			this.DrawCaptionElements(canvas, width, height, rightValues, false);
			this.DrawCaptionElements(canvas, width, height, leftValues, true);
		}

		private void DrawCaptionElements(SKCanvas canvas, int width, int height, List<Entry> entries, bool isLeft)
		{
			var margin = 2 * this.Margin;
			var availableHeight = height - 2 * margin;
			var x = isLeft ? this.Margin : (width - this.Margin - this.LabelTextSize);
			var ySpace = (availableHeight - this.LabelTextSize) / ((entries.Count <= 1) ?  1 : entries.Count - 1);

			for (int i = 0; i < entries.Count; i++)
			{
				var entry = entries.ElementAt(i);
				var y = margin + i * ySpace;
				if (entries.Count <= 1) y += (availableHeight - this.LabelTextSize) / 2;

				var hasLabel = !string.IsNullOrEmpty(entry.Label);
				var hasValueLabel = !string.IsNullOrEmpty(entry.ValueLabel);

				if (hasLabel || hasValueLabel)
				{
					var hasOffset = hasLabel && hasValueLabel;
					var captionMargin = LabelTextSize * 0.60f;
					var space = hasOffset ? captionMargin : 0;

					using (var paint = new SKPaint
					{
						Style = SKPaintStyle.Fill,
						Color = entry.FillColor,
					})
					{
						var rect = SKRect.Create(x, y, this.LabelTextSize, this.LabelTextSize);
						canvas.DrawRect(rect, paint);
					}

					y += LabelTextSize / 2;

					if (hasLabel)
					{
						using (var paint = new SKPaint()
						{
							TextSize = this.LabelTextSize,
							IsAntialias = true,
							Color = entry.TextColor,
							IsStroke = false
						})
						{
							var bounds = new SKRect();
							var text = entry.Label;
							paint.MeasureText(text, ref bounds);
							var textX = isLeft ? this.Margin + captionMargin + this.LabelTextSize : width - this.Margin - captionMargin - this.LabelTextSize - bounds.Width;
							var textY = y - (bounds.Top + bounds.Bottom) / 2 - space;

							canvas.DrawText(text, textX, textY, paint);
						}
					}

					if (hasValueLabel)
					{
						using (var paint = new SKPaint()
						{
							TextSize = this.LabelTextSize,
							IsAntialias = true,
							FakeBoldText = true,
							Color = entry.FillColor,
							IsStroke = false
						})
						{
							var bounds = new SKRect();
							var text = entry.ValueLabel;
							paint.MeasureText(text, ref bounds);
							var textX = isLeft ? this.Margin + captionMargin + this.LabelTextSize : width - this.Margin - captionMargin - this.LabelTextSize - bounds.Width;
							var textY = y - (bounds.Top + bounds.Bottom) / 2 + space;
							canvas.DrawText(text, textX, textY, paint);
						}
					}
				}
			}
		}


		public override void DrawContent(SKCanvas canvas, int width, int height)
		{
			this.DrawCaption(canvas, width, height);
			using (new SKAutoCanvasRestore(canvas))
			{
				canvas.Translate(width / 2, height / 2);
				var sumValue = this.Entries.Sum(x => Math.Abs(x.Value));
				var radius = (Math.Min(width, height) / 2) - 2 * Margin;

				var start = 0.0f;
				for (int i = 0; i < this.Entries.Count(); i++)
				{
					var entry = this.Entries.ElementAt(i);
					var end = start + (Math.Abs(entry.Value) / sumValue);

					// Sector
					var path = CreateSectorPath(start, end, radius, radius * this.HoleRadius);
					using (var paint = new SKPaint
					{
						Style = SKPaintStyle.Fill,
						Color = entry.FillColor,
						IsAntialias = true,
					})
					{
						canvas.DrawPath(path, paint);
					}

					start = end;
				}
			}
		}
	}
}