// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/Bar.png)
    ///
    /// A bar chart.
    /// </summary>
    public class GroupedBarChart : SeriesChart, IBarChart
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.BarChart"/> class.
        /// </summary>
        public GroupedBarChart()
        {
        }

        #endregion

        #region Properties

        private Orientation labelOrientation;
        private Orientation valueLabelOrientation;
        private float valueLabelTextSize = 16;

        /// <inheritdoc cref="IBarChart"/>
        public byte BarAreaAlpha { get; set; } = DefaultValues.BarAreaAlpha;

        /// <inheritdoc cref="IBarChart"/>
        public float MinBarHeight { get; set; } = DefaultValues.MinBarHeight;

        /// <inheritdoc cref="IBarChart"/>
        public Orientation LabelOrientation
        {
            get => labelOrientation;
            set => labelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        /// <inheritdoc cref="IBarChart"/>
        public Orientation ValueLabelOrientation
        {
            get => valueLabelOrientation;
            set => valueLabelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        /// <summary>
        /// Gets or sets the text size of the value labels.
        /// </summary>
        /// <value>The size of the value label text.</value>
        public float ValueLabelTextSize
        {
            get => valueLabelTextSize;
            set => Set(ref valueLabelTextSize, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the content of the chart onto the specified canvas.
        /// </summary>
        /// <param name="canvas">The output canvas.</param>
        /// <param name="width">The width of the chart.</param>
        /// <param name="height">The height of the chart.</param>
        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Series != null && entries != null)
            {
                var firstSerie = Series.FirstOrDefault();

                var labels = firstSerie.Entries.Select(x => x.Label).ToArray();
                int nbItems = labels.Length;

                var groupedEntries = entries.GroupBy(x => x.Label);
                
                int barPerItems = groupedEntries.Max(g => g.Count());

                var labelSizes = MeasureHelper.MeasureTexts(labels, LabelTextSize);
                var footerHeight = MeasureHelper.CalculateFooterHeaderHeight(Margin, LabelTextSize, labelSizes, LabelOrientation);

                var valueLabelSizes = MeasureValueLabels();
                var headerHeight = MeasureHelper.CalculateFooterHeaderHeight(Margin, LabelTextSize, valueLabelSizes.Values.ToArray(), ValueLabelOrientation);

                var itemSize = CalculateItemSize(nbItems, width, height, footerHeight, headerHeight);
                var barSize = CalculateBarSize(itemSize, Series.Count());
                var origin = CalculateYOrigin(itemSize.Height, headerHeight);

                int nbSeries = series.Count();
                for (int i = 0; i < labels.Length; i++)
                {
                    string label = labels[i];
                    SKRect labelSize = labelSizes[i];

                    var itemX = Margin + (itemSize.Width / 2) + (i * (itemSize.Width + Margin));

                    for (int y = 0; y < nbSeries; y++)
                    {
                        ChartSerie serie = Series.ElementAt(y);
                        ChartEntry entry = serie.Entries.FirstOrDefault(e => e.Label == label);
                        float value = entry?.Value ?? 0;
                        float marge = Margin / 2;
                        float totalBarMarge = y * Margin / 2; 
                        float barX = itemX + marge + y * barSize.Width + totalBarMarge;
                        float barY = headerHeight + ((1 - AnimationProgress) * (origin - headerHeight) + (((MaxValue - value) / ValueRange) * itemSize.Height) * AnimationProgress);

                        DrawBarArea(canvas, headerHeight, itemSize, barSize, serie.Color, value, barX, barY);
                        DrawBar(canvas, headerHeight, itemSize, barSize, origin, barX, barY, serie.Color);

                        if (!string.IsNullOrEmpty(entry?.ValueLabel))
                            DrawHelper.DrawLabel(canvas, ValueLabelOrientation, true, barSize, new SKPoint(barX - (itemSize.Width / 2) + (barSize.Width/2), headerHeight - Margin), entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSizes[entry], entry.ValueLabel, ValueLabelTextSize, Typeface);
                    }

                    DrawHelper.DrawLabel(canvas, LabelOrientation, false, itemSize, new SKPoint(itemX + Margin/2, height - footerHeight + Margin), LabelColor, labelSize, label, LabelTextSize, Typeface);
                }
            }
        }

        private float CalculateYOrigin(float itemHeight, float headerHeight)
        {
            if (MaxValue <= 0)
            {
                return headerHeight;
            }

            if (MinValue > 0)
            {
                return headerHeight + itemHeight;
            }

            return headerHeight + ((MaxValue / ValueRange) * itemHeight);
        }

        private Dictionary<ChartEntry, SKRect> MeasureValueLabels()
        {
            var dict = new Dictionary<ChartEntry, SKRect>();
            using (var paint = new SKPaint())
            {
                paint.TextSize = ValueLabelTextSize;
                foreach (var e in entries)
                {
                    SKRect bounds;
                    if (string.IsNullOrEmpty(e.ValueLabel))
                    {
                        bounds = SKRect.Empty;
                    }
                    else
                    {
                        bounds = new SKRect();
                        paint.MeasureText(e.ValueLabel, ref bounds);
                    }

                    dict.Add(e, bounds);
                }
            }

            return dict;
        }

        private void DrawBar(SKCanvas canvas, float headerHeight, SKSize itemSize, SKSize barSize, float origin, float barX, float barY, SKColor color)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = color,
            })
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

                var rect = SKRect.Create(x, y, barSize.Width, height);
                canvas.DrawRect(rect, paint);
            }
        }

        private void DrawBarArea(SKCanvas canvas, float headerHeight, SKSize itemSize, SKSize barSize, SKColor color, float value, float barX, float barY)
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

        private SKSize CalculateBarSize(SKSize itemSize, int barPerItems)
        {
            var w = (itemSize.Width - (barPerItems * Margin / 2)) / barPerItems;
            return new SKSize(w, itemSize.Height);
        }

        private  SKSize CalculateItemSize(int items, int width, int height, float footerHeight, float headerHeight)
        {
            var w = (width - ((items + 1) * Margin)) / items;
            var h = height - Margin - footerHeight - headerHeight;
            return new SKSize(w, h);
        }

        #endregion
    }
}
