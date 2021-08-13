using System;
using System.Collections.Generic;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/BarSeries.png)
    ///
    /// A grouped bar chart.
    /// </summary>
    public class PointChart : AxisBasedChart
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.PointSeriesChart"/> class.
        /// </summary>
        public PointChart() : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the size of the point.
        /// </summary>
        /// <value>The size of the point.</value>
        public float PointSize { get; set; } = 14;

        /// <summary>
        /// Gets or sets the point mode.
        /// </summary>
        /// <value>The point mode.</value>
        public PointMode PointMode { get; set; } = PointMode.Circle;

        /// <summary>
        /// Gets or sets the point area alpha.
        /// </summary>
        /// <value>The point area alpha.</value>
        public byte PointAreaAlpha { get; set; } = 100;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void DrawValueLabel(SKCanvas canvas, Dictionary<ChartEntry, SKRect> valueLabelSizes, float headerWithLegendHeight, SKSize itemSize, SKSize barSize, ChartEntry entry, float barX, float barY, float itemX, float origin)
        {
            string label = entry?.ValueLabel;
            if (string.IsNullOrEmpty(label))
                return;

            var drawedPoint = new SKPoint(barX - (itemSize.Width / 2) + (barSize.Width / 2), barY);
            if (ValueLabelOption == ValueLabelOption.TopOfChart)
                base.DrawValueLabel(canvas, valueLabelSizes, headerWithLegendHeight, itemSize, barSize, entry, barX, barY, itemX, origin);
            else if (ValueLabelOption == ValueLabelOption.TopOfElement)
                DrawHelper.DrawLabel(canvas, ValueLabelOrientation, ValueLabelOrientation == Orientation.Vertical ? YPositionBehavior.UpToElementHeight : YPositionBehavior.None, barSize, new SKPoint(drawedPoint.X, drawedPoint.Y - (PointSize / 2) - (Margin / 2)), entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSizes[entry], label, ValueLabelTextSize, Typeface);
            else if (ValueLabelOption == ValueLabelOption.OverElement)
                DrawHelper.DrawLabel(canvas, ValueLabelOrientation, ValueLabelOrientation == Orientation.Vertical ? YPositionBehavior.UpToElementMiddle : YPositionBehavior.DownToElementMiddle, barSize, new SKPoint(drawedPoint.X, drawedPoint.Y), entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSizes[entry], label, ValueLabelTextSize, Typeface);
        }

        /// <inheritdoc />
        protected override void DrawBar(ChartSerie serie, SKCanvas canvas, float headerHeight, float itemX, SKSize itemSize, SKSize barSize, float origin, float barX, float barY, SKColor color)
        {
            if (PointMode != PointMode.None)
            {
                var point = new SKPoint(barX - (itemSize.Width / 2) + (barSize.Width / 2), barY);
                canvas.DrawPoint(point, color, PointSize, PointMode);
            }
        }

        /// <inheritdoc />
        protected override void DrawBarArea(SKCanvas canvas, float headerHeight, SKSize itemSize, SKSize barSize, SKColor color, float origin, float value, float barX, float barY)
        {
            if (PointAreaAlpha > 0)
            {
                var y = Math.Min(origin, barY);

                using (var shader = SKShader.CreateLinearGradient(new SKPoint(0, origin), new SKPoint(0, barY), new[] { color.WithAlpha(PointAreaAlpha), color.WithAlpha((byte)(PointAreaAlpha / 3)) }, null, SKShaderTileMode.Clamp))
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = color.WithAlpha(PointAreaAlpha),
                })
                {
                    paint.Shader = shader;
                    var height = Math.Max(2, Math.Abs(origin - barY));
                    canvas.DrawRect(SKRect.Create(barX - (itemSize.Width / 2) + (barSize.Width / 2) - (PointSize / 2), y, PointSize, height), paint);
                }
            }
        }

        #endregion
    }
}
