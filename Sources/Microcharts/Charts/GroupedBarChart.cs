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
    /// ![chart](../images/GroupedBar.png)
    ///
    /// A grouped bar chart.
    /// </summary>
    public class GroupedBarChart : SeriesChart, IBarChart
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.GroupedBarChart"/> class.
        /// </summary>
        public GroupedBarChart()
        {
            YAxisTextPaint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.StrokeAndFill,
            };

            YAxisLinesPaint = new SKPaint
            {
                Color = SKColors.Black.WithAlpha(0x50),
                IsAntialias = true,
                Style = SKPaintStyle.Stroke
            };
        }

        #endregion

        #region Properties

        private Orientation labelOrientation;
        private Orientation valueLabelOrientation;
        private float valueLabelTextSize = 16;
        private float serieLabelTextSize = 16;

        /// <summary>
        /// Get or sets the legend option for the chart
        /// </summary>
        /// <value>The legend option</value>
        public GroupedBarLegendOption LegendOption { get; set; } = GroupedBarLegendOption.Bottom;

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

        /// <summary>
        /// Gets or sets the text size of the serie labels.
        /// </summary>
        /// <value>The size of the serie label text.</value>
        public float SerieLabelTextSize
        {
            get => serieLabelTextSize;
            set => Set(ref serieLabelTextSize, value);
        }

        /// <summary>
        /// Show Y Axis Text?
        /// </summary>
        public bool ShowYAxisText { get; set; } = false;

        /// <summary>
        /// Show Y Axis Lines?
        /// </summary>
        public bool ShowYAxisLines { get; set; } = false;

        //TODO : calculate this automatically, based on available area height and text height
        /// <summary>
        /// Y Axis Max Ticks
        /// </summary>
        public int YAxisMaxTicks { get; set; } = 5;

        /// <summary>
        /// Y Axis Position
        /// </summary>
        public Position YAxisPosition { get; set; } = Position.Right;

        /// <summary>
        /// Y Axis Paint
        /// </summary>
        public SKPaint YAxisTextPaint { get; set; }

        /// <summary>
        /// Y Axis Paint
        /// </summary>
        public SKPaint YAxisLinesPaint { get; set; }

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
                width = MeasureHelper.CalculateYAxis(ShowYAxisText, ShowYAxisLines, entries, YAxisMaxTicks, YAxisTextPaint, YAxisPosition, width, out float yAxisXShift, out List<float> yAxisIntervalLabels);
                var firstSerie = Series.FirstOrDefault();
                var labels = firstSerie.Entries.Select(x => x.Label).ToArray();
                int nbItems = labels.Length;

                var groupedEntries = entries.GroupBy(x => x.Label);
                
                int barPerItems = groupedEntries.Max(g => g.Count());

                var seriesNames = Series.Select(s => s.Name).ToArray();
                var seriesSizes = MeasureHelper.MeasureTexts(seriesNames, SerieLabelTextSize);
                float legendHeight = CalculateLegendSize(seriesSizes, SerieLabelTextSize, width);

                var labelSizes = MeasureHelper.MeasureTexts(labels, LabelTextSize);
                var footerHeight = MeasureHelper.CalculateFooterHeaderHeight(Margin, LabelTextSize, labelSizes, LabelOrientation);
                var footerWithLegendHeight = footerHeight + (LegendOption == GroupedBarLegendOption.Bottom ? legendHeight : 0);

                var valueLabelSizes = MeasureValueLabels();
                var headerHeight = MeasureHelper.CalculateFooterHeaderHeight(Margin, LabelTextSize, valueLabelSizes.Values.ToArray(), ValueLabelOrientation);
                var headerWithLegendHeight = headerHeight + (LegendOption == GroupedBarLegendOption.Top ? legendHeight : 0);

                var itemSize = CalculateItemSize(nbItems, width, height, footerHeight + headerHeight + legendHeight);
                var barSize = CalculateBarSize(itemSize, Series.Count());
                var origin = CalculateYOrigin(itemSize.Height, headerWithLegendHeight);
                DrawHelper.DrawYAxis(ShowYAxisText, ShowYAxisLines, YAxisPosition, YAxisTextPaint, YAxisLinesPaint, Margin, AnimationProgress, MaxValue, ValueRange, canvas, width, yAxisXShift, yAxisIntervalLabels, headerHeight, itemSize, origin);

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
                        float barY = headerWithLegendHeight + ((1 - AnimationProgress) * (origin - headerWithLegendHeight) + (((MaxValue - value) / ValueRange) * itemSize.Height) * AnimationProgress);

                        DrawBarArea(canvas, headerWithLegendHeight, itemSize, barSize, serie.Color, value, barX, barY);
                        DrawBar(canvas, headerWithLegendHeight, itemSize, barSize, origin, barX, barY, serie.Color);

                        if (!string.IsNullOrEmpty(entry?.ValueLabel))
                            DrawHelper.DrawLabel(canvas, ValueLabelOrientation, true, barSize, new SKPoint(barX - (itemSize.Width / 2) + (barSize.Width/2), headerWithLegendHeight - Margin), entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)), valueLabelSizes[entry], entry.ValueLabel, ValueLabelTextSize, Typeface);
                    }

                    DrawHelper.DrawLabel(canvas, LabelOrientation, false, itemSize, new SKPoint(itemX + Margin/2, height - footerWithLegendHeight + Margin), LabelColor, labelSize, label, LabelTextSize, Typeface);
                }

                DrawLegend(canvas, seriesSizes, SerieLabelTextSize, legendHeight, height, width);
            }
        }

        private void DrawLegend(SKCanvas canvas, SKRect[] seriesNameSize, float serieLabelTextSize, float legendHeight, float height, float width)
        {
            if (LegendOption == GroupedBarLegendOption.None)
                return;

            float lineHeight = Math.Max(seriesNameSize.Where(b => !b.IsEmpty).Select(b => b.Height).FirstOrDefault(), SerieLabelTextSize);

            float origin = Margin;
            if (LegendOption == GroupedBarLegendOption.Bottom)
                origin += height - legendHeight;

            int nbLine = 1;
            float currentWidthUsed = 0;
            var series = Series.ToArray();
            for (int i = 0; i < series.Length; i++)
            {
                var serie = series[i];
                var serieBound = seriesNameSize[i];
            
                float legentItemWidth = Margin + SerieLabelTextSize + Margin + serieBound.Width;
                if (legentItemWidth > width)
                {
                    if (currentWidthUsed != 0)
                    {
                        nbLine++;
                        currentWidthUsed = 0;
                    }

                    currentWidthUsed = GenerateSerieLegend(canvas, lineHeight, origin, nbLine, currentWidthUsed, serie);
                }
                else if (legentItemWidth + currentWidthUsed > width)
                {
                    nbLine++;
                    currentWidthUsed = 0;
                    currentWidthUsed = GenerateSerieLegend(canvas, lineHeight, origin, nbLine, currentWidthUsed, serie);
                }
                else
                {
                    currentWidthUsed = GenerateSerieLegend(canvas, lineHeight, origin, nbLine, currentWidthUsed, serie);
                }
            }

        }

        private float GenerateSerieLegend(SKCanvas canvas, float lineHeight, float origin, int nbLine, float currentWidthUsed, ChartSerie serie)
        {
            var legendColor = serie.Color.WithAlpha((byte)(serie.Color.Alpha * AnimationProgress));
            var lblColor = LabelColor.WithAlpha((byte)(LabelColor.Alpha * AnimationProgress));
            var yPosition = origin + (nbLine - 1) * (lineHeight + Margin);
            var rect = SKRect.Create(currentWidthUsed + Margin, yPosition, SerieLabelTextSize, SerieLabelTextSize);
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = legendColor
            })
            {
                canvas.DrawRect(rect, paint);
            }

            currentWidthUsed += Margin + SerieLabelTextSize + Margin;
            using (var paint = new SKPaint())
            {
                paint.TextSize = SerieLabelTextSize;
                paint.IsAntialias = true;
                paint.Color = lblColor;
                paint.IsStroke = false;
                paint.Typeface = Typeface;

                var bounds = new SKRect();
                paint.MeasureText(serie.Name, ref bounds);
                //Vertical center align the text to the legend color box
                float textYPosition = rect.Bottom - ((rect.Bottom - rect.Top) / 2) + (bounds.Height / 2);
                canvas.DrawText(serie.Name, currentWidthUsed, textYPosition, paint);
                currentWidthUsed += bounds.Width;
            }

            return currentWidthUsed;
        }

        private float CalculateLegendSize(SKRect[] seriesSizes, float serieLabelTextSize, int width)
        {
            if (LegendOption == GroupedBarLegendOption.None)
                return 0;

            int nbLine = 1;
            float currentWidthUsed = 0;
            foreach(var rect in seriesSizes)
            {
                float legentItemWidth = Margin + serieLabelTextSize + Margin + rect.Width;
                if (legentItemWidth > width)
                {
                    if (currentWidthUsed != 0)
                    {
                        nbLine++;
                    }
                    currentWidthUsed = width;
                }
                else if (legentItemWidth + currentWidthUsed > width)
                {
                    nbLine++;
                    currentWidthUsed = legentItemWidth;
                }
            }

            float height = Math.Max(seriesSizes.Where(b => !b.IsEmpty).Select(b => b.Height).FirstOrDefault(), serieLabelTextSize);

            return nbLine * height + nbLine * Margin;
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

        private  SKSize CalculateItemSize(int items, int width, int height, float reservedSpace)
        {
            var w = (width - ((items + 1) * Margin)) / items;
            var h = height - Margin - reservedSpace;
            return new SKSize(w, h);
        }

        #endregion
    }
}
