// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/Bar.png)
    ///
    /// A bar chart.
    /// </summary>
    public class GroupedBarChart : BarChart
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

        private IEnumerable<ChartSerie> series;

        public IEnumerable<ChartSerie> Series
        {
            get => series;
            set => UpdateSeries(value);
        }

        public float MinBarHeight { get; set; } = 4;

        #endregion

        #region Methods

        private void UpdateSeries(IEnumerable<ChartSerie> value)
        {
            Set(ref series, value);
            Entries = series.SelectMany(s => s.Entries).ToList();
        }

        /// <summary>
        /// Draws the content of the chart onto the specified canvas.
        /// </summary>
        /// <param name="canvas">The output canvas.</param>
        /// <param name="width">The width of the chart.</param>
        /// <param name="height">The height of the chart.</param>
        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Series != null && Entries != null)
            {
                var firstSerie = Series.FirstOrDefault();

                var labels = firstSerie.Entries.Select(x => x.Label).ToArray();
                int nbItems = labels.Length;


                var groupedEntries = Entries.GroupBy(x => x.Label);
                
                int barPerItems = groupedEntries.Max(g => g.Count());

                var labelSizes = MeasureLabels(labels);
                var footerHeight = CalculateFooterHeaderHeight(labelSizes, LabelOrientation);

                var valueLabelSizes = MeasureValueLabels();
                var headerHeight = CalculateFooterHeaderHeight(valueLabelSizes.Values.ToArray(), ValueLabelOrientation);


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
                        float marge = Margin / 2;// y > 0 ? Margin / 2 : 0;
                        float totalBarMarge = y * Margin / 2;  //y > 0 ? (y - 1) * Margin / 2 : 0;
                        float barX = itemX + marge + y * barSize.Width + totalBarMarge;
                        float barY = headerHeight + ((1 - AnimationProgress) * (origin - headerHeight) + (((MaxValue - value) / ValueRange) * itemSize.Height) * AnimationProgress);

                        DrawBarArea(canvas, headerHeight, itemSize, barSize, serie.Color, value, barX, barY);
                        DrawBar(canvas, headerHeight, itemSize, barSize, origin, barX, barY, serie.Color);

                        if (!string.IsNullOrEmpty(entry?.ValueLabel))
                            DrawLabel(canvas, ValueLabelOrientation, true, barSize, new SKPoint(barX - (itemSize.Width / 2) + (barSize.Width/2), headerHeight - Margin), entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSizes[entry], entry.ValueLabel);
                    }

                    DrawLabel(canvas, LabelOrientation, false, itemSize, new SKPoint(itemX - ((nbSeries -1) * (Margin / 2)), height - footerHeight + Margin), LabelColor, labelSize, label);
                }
            }
        }

        private Dictionary<ChartEntry, SKRect> MeasureValueLabels()
        {
            var dict = new Dictionary<ChartEntry, SKRect>();
            using (var paint = new SKPaint())
            {
                paint.TextSize = LabelTextSize;
                foreach (var e in Entries)
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
            if (PointAreaAlpha > 0)
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
            var w = itemSize.Width / barPerItems - ((barPerItems) * Margin/2);
            return new SKSize(w, itemSize.Height);
        }

        private  SKSize CalculateItemSize(int items, int width, int height, float footerHeight, float headerHeight)
        {
            var w = (width - ((items + 1) * Margin)) / items;
            var h = height - Margin - footerHeight - headerHeight;
            return new SKSize(w, h);
        }

        /// <summary>
        /// Draws the value bars.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="points">The points.</param>
        /// <param name="itemSize">The item size.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="headerHeight">The Header height.</param>
        protected void DrawBars(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float origin, float headerHeight)
        {
            const float MinBarHeight = 4;
            if (points.Length > 0)
            {
                for (int i = 0; i < Entries.Count(); i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color,
                    })
                    {
                        var x = point.X - (itemSize.Width / 2);
                        var y = Math.Min(origin, point.Y);
                        var height = Math.Max(MinBarHeight, Math.Abs(origin - point.Y));
                        if (height < MinBarHeight)
                        {
                            height = MinBarHeight;
                            if (y + height > Margin + itemSize.Height)
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
        /// <param name="canvas">The output canvas.</param>
        /// <param name="points">The entry points.</param>
        /// <param name="itemSize">The item size.</param>
        /// <param name="headerHeight">The header height.</param>
        protected void DrawBarAreas(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float headerHeight)
        {
            if (points.Length > 0 && PointAreaAlpha > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];

                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color.WithAlpha((byte)(this.BarAreaAlpha * this.AnimationProgress)),
                    })
                    {
                        var max = entry.Value > 0 ? headerHeight : headerHeight + itemSize.Height;
                        var height = Math.Abs(max - point.Y);
                        var y = Math.Min(max, point.Y);
                        canvas.DrawRect(SKRect.Create(point.X - (itemSize.Width / 2), y, itemSize.Width, height), paint);
                    }
                }
            }
        }

        #endregion
    }
}
