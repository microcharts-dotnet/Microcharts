namespace Microcharts
{
	using SkiaSharp;
	using System.Linq;

	public class LineChart : PointChart
	{
		public LineChart()
		{
			this.PointSize = 10;
		}

		public float LineSize { get; set; } = 3;

		public LineMode LineMode { get; set; } = LineMode.Spline;

		public byte LineAreaAlpha { get; set; } = 32;

		private SKShader CreateGradient(SKPoint[] points, byte alpha = 255)
		{
			var startX = points.First().X;
			var endX = points.Last().X;
			var rangeX = endX - startX;

			return SKShader.CreateLinearGradient(
				new SKPoint(startX, 0),
				new SKPoint(endX, 0),
				this.Entries.Select(x => x.FillColor.WithAlpha(alpha)).ToArray(),
				null,
				SKShaderTileMode.Clamp);
		}

		private (SKPoint point, SKPoint control, SKPoint nextPoint, SKPoint nextControl) CalculateCubicInfo(SKPoint[] points, int i, SKSize itemSize)
		{
			var point = points[i];
			var nextPoint = points[i + 1];
			var controlOffset = new SKPoint(itemSize.Width * 0.8f, 0);
			var currentControl = point + controlOffset;
			var nextControl = nextPoint - controlOffset;
			return (point, currentControl, nextPoint, nextControl);
		}

		protected void DrawLine(SKCanvas canvas, SKPoint[] points, SKSize itemSize)
		{
			if (points.Length > 1 && this.LineMode != LineMode.None)
			{
				using (var paint = new SKPaint
				{
					Style = SKPaintStyle.Stroke,
					Color = SKColors.White,
					StrokeWidth = this.LineSize,
					IsAntialias = true,
				})
				{
					using (var shader = CreateGradient(points))
					{
						paint.Shader = shader;

						var path = new SKPath();

						path.MoveTo(points.First());

						var last = (this.LineMode == LineMode.Spline) ? points.Length - 1 : points.Length;
						for (int i = 0; i < last; i++)
						{
							if(this.LineMode == LineMode.Spline)
							{
								var entry = this.Entries.ElementAt(i);
								var nextEntry = this.Entries.ElementAt(i + 1);
								var cubicInfo = this.CalculateCubicInfo(points, i, itemSize);
								path.CubicTo(cubicInfo.control, cubicInfo.nextControl, cubicInfo.nextPoint);
							}
							else if(this.LineMode == LineMode.Straight)
							{
								path.LineTo(points[i]);
							}

						}

						canvas.DrawPath(path, paint);
					}
				}
			}
		}

		protected void DrawArea(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float origin)
		{
			if (this.LineAreaAlpha > 0 && points.Length > 1)
			{
				using (var paint = new SKPaint
				{
					Style = SKPaintStyle.Fill,
					Color = SKColors.White,
					IsAntialias = true,
				})
				{
					using (var shader = CreateGradient(points, this.LineAreaAlpha))
					{
						paint.Shader = shader;

						var path = new SKPath();

						path.MoveTo(points.First().X, origin);
						path.LineTo(points.First());

						var last = (this.LineMode == LineMode.Spline) ? points.Length - 1 : points.Length;
						for (int i = 0; i < last; i++)
						{
							if (this.LineMode == LineMode.Spline)
							{
								var entry = this.Entries.ElementAt(i);
								var nextEntry = this.Entries.ElementAt(i + 1);
								var cubicInfo = this.CalculateCubicInfo(points, i, itemSize);
								path.CubicTo(cubicInfo.control, cubicInfo.nextControl, cubicInfo.nextPoint);
							}
							else if (this.LineMode == LineMode.Straight)
							{
								path.LineTo(points[i]);
							}
						}

						path.LineTo(points.Last().X, origin);

						path.Close();

						canvas.DrawPath(path, paint);
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

			this.DrawArea(canvas, points, itemSize, origin);
			this.DrawLine(canvas, points, itemSize);
			this.DrawPoints(canvas, points);
			this.DrawFooter(canvas, points, itemSize, height, footerHeight);
			this.DrawValueLabel(canvas, points, itemSize, height,valueLabelSizes);
		}
	}
}
