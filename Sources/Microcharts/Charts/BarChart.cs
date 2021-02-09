// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/BarSeries.png)
    ///
    /// A grouped bar chart.
    /// </summary>
    public class BarChart : AxisBasedChart
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.BarSeriesChart"/> class.
        /// </summary>
        public BarChart() : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bar background area alpha.
        /// </summary>
        /// <value>The bar area alpha.</value>
        public byte BarAreaAlpha { get; set; } = DefaultValues.BarAreaAlpha;

        /// <summary>
        /// Get or sets the minimum height for a bar
        /// </summary>
        /// <value>The minium height of a bar.</value>
        public float MinBarHeight { get; set; } = DefaultValues.MinBarHeight;

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected override float CalculateHeaderHeight(Dictionary<ChartEntry, SKRect> valueLabelSizes)
        {
            if (ValueLabelOption == ValueLabelOption.None || ValueLabelOption == ValueLabelOption.OverElement)
                return Margin;

            return base.CalculateHeaderHeight(valueLabelSizes);
        }

        /// <inheritdoc/>
        protected override void DrawValueLabel(SKCanvas canvas, Dictionary<ChartEntry, SKRect> valueLabelSizes, float headerWithLegendHeight, SKSize itemSize, SKSize barSize, ChartEntry entry, float barX, float barY, float itemX, float origin)
        {
            if (string.IsNullOrEmpty(entry?.ValueLabel))
                return;

            (SKPoint location, SKSize size) = GetBarDrawingProperties(headerWithLegendHeight, itemSize, barSize, 0, barX, barY);
            if(ValueLabelOption == ValueLabelOption.TopOfChart)
                base.DrawValueLabel(canvas, valueLabelSizes, headerWithLegendHeight, itemSize, barSize, entry, barX, barY, itemX, origin);
            else if(ValueLabelOption == ValueLabelOption.TopOfElement)
                DrawHelper.DrawLabel(canvas, ValueLabelOrientation, ValueLabelOrientation == Orientation.Vertical ? YPositionBehavior.UpToElementHeight : YPositionBehavior.None, barSize, new SKPoint(location.X + size.Width / 2, barY - Margin), entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSizes[entry], entry.ValueLabel, ValueLabelTextSize, Typeface);
            else if(ValueLabelOption == ValueLabelOption.OverElement)
                DrawHelper.DrawLabel(canvas, ValueLabelOrientation, ValueLabelOrientation == Orientation.Vertical ? YPositionBehavior.UpToElementMiddle : YPositionBehavior.DownToElementMiddle, barSize, new SKPoint(location.X + size.Width / 2, barY + (origin - barY) / 2), entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSizes[entry], entry.ValueLabel, ValueLabelTextSize, Typeface);
        }

        /// <inheritdoc />
        protected override void DrawBar(ChartSerie serie, SKCanvas canvas, float headerHeight, float itemX, SKSize itemSize, SKSize barSize, float origin, float barX, float barY, SKColor color)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = color,
            })
            {
                (SKPoint location, SKSize size) = GetBarDrawingProperties(headerHeight, itemSize, barSize, origin, barX, barY);
                var rect = SKRect.Create(location, size);
                canvas.DrawRect(rect, paint);
            }
        }

        private (SKPoint location, SKSize size) GetBarDrawingProperties(float headerHeight, SKSize itemSize, SKSize barSize, float origin, float barX, float barY)
        {
            var x = barX - (itemSize.Width / 2);
            var y = Math.Min(origin, barY);
            var height = Math.Max(MinBarHeight, Math.Abs(origin - barY));
            if (height < MinBarHeight)
            {
                height = MinBarHeight;
                if (y + height > Margin + itemSize.Height)
                {
                    y = headerHeight + itemSize.Height - height;
                }
            }

            return (new SKPoint(x, y), new SKSize(barSize.Width, height));
        }

        /// <inheritdoc />
        protected override void DrawBarArea(SKCanvas canvas, float headerHeight, SKSize itemSize, SKSize barSize, SKColor color, float origin, float value, float barX, float barY)
        {
            if (BarAreaAlpha > 0)
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Fill,
                    Color = color.WithAlpha((byte)(this.BarAreaAlpha * this.AnimationProgress)),
                })
                {
                    var max = value > 0 ? headerHeight : headerHeight + itemSize.Height;
                    var height = Math.Abs(max - barY);
                    var y = Math.Min(max, barY);
                    canvas.DrawRect(SKRect.Create(barX - (itemSize.Width / 2), y, barSize.Width, height), paint);
                }
            }
        }

        #endregion
    }
}
