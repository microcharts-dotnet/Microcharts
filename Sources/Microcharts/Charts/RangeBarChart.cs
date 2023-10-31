using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    public class RangeBarChart : SimpleChart
    {
        public RangeBarChart()
        {
            LabelOrientation = Orientation.Default;
            ValueLabelOrientation = Orientation.Default;

            YAxisTextPaint = new SKPaint
            {
                Color = SKColors.Black.WithAlpha(0x50),
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

        public IEnumerable<RangeChartEntry> Entries
        {
            get => entries.OfType<RangeChartEntry>();
            set => UpdateEntries(value);
        }

        #region Properties

        private Orientation labelOrientation;
        private Orientation valueLabelOrientation;
        private float valueLabelTextSize = 16;
        private float barWidthToSpaceRatio = .5f;
        private bool valueLabelTextSizeDefaultValue = true;

        public Orientation LabelOrientation
        {
            get => labelOrientation;
            set => labelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        public Orientation ValueLabelOrientation
        {
            get => valueLabelOrientation;
            set => valueLabelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        public float ValueLabelTextSize
        {
            get => valueLabelTextSize;
            set
            {
                Set(ref valueLabelTextSize, value);
                valueLabelTextSizeDefaultValue = false;
            }
        }

        public bool ShowYAxisText { get; set; } = false;

        public bool ShowYAxisLines { get; set; } = false;

        public int YAxisMaxTicks { get; set; } = 5;

        public Position YAxisPosition { get; set; } = Position.Right;

        public SKPaint YAxisTextPaint { get; set; }

        public SKPaint YAxisLinesPaint { get; set; }

        public float BarWidthToSpaceRatio
        {
            get => barWidthToSpaceRatio;
            set => barWidthToSpaceRatio = value < 0 ? 0 : value > 1 ? 1 : value;
        }

        public Func<float, string> YAxisLabelFormatter { get; set; } = EmptyLabelFormatter;

        public override float MinValue
        {
            get
            {
                if (!entries.Any())
                {
                    return 0;
                }

                if (InternalMinValue == null)
                {
                    return Entries.Where( x=>x.StartValue.HasValue).Min(x => x.StartValue.Value);
                }

                return Math.Min(InternalMinValue.Value, Entries.Where( x=>x.StartValue.HasValue).Min(x => x.StartValue.Value));
            }

            set => InternalMinValue = value;
        }

        #endregion

        #region Methods

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (entries is null)
            {
                return;
            }

            bool fixedRange = InternalMaxValue.HasValue || InternalMinValue.HasValue;

            float maxValue = MaxValue;
            float minValue = MinValue;

            width = MeasureHelper.CalculateYAxis(ShowYAxisText,
                ShowYAxisLines,
                entries,
                YAxisMaxTicks,
                YAxisTextPaint,
                YAxisPosition,
                width,
                fixedRange,
                ref maxValue, ref minValue, out float yAxisXShift, out List<float> yAxisIntervalLabels,
                YAxisLabelFormatter);

            float valRange = maxValue - minValue;

            var labels = Entries.Select(x => x.Label)
                .ToArray();

            var entriesCount = Entries.Count();

            var labelSizes = MeasureHelper.MeasureTexts(labels, LabelTextSize);
            var footerHeight = MeasureHelper.CalculateFooterHeaderHeight(Margin, LabelTextSize, labelSizes, LabelOrientation); ;

            var valueLabelSizes = MeasureValueLabels();
            float headerHeight = CalculateHeaderHeight(valueLabelSizes);

            var itemSize = CalculateItemSize(entriesCount, width, height, footerHeight + headerHeight);

            var yCenterPosition = headerHeight + itemSize.Height * maxValue / valRange;

            DrawHelper.DrawYAxis(ShowYAxisText,
                ShowYAxisLines,
                YAxisPosition,
                YAxisTextPaint,
                YAxisLinesPaint,
                Margin,
                AnimationProgress,
                maxValue,
                valRange,
                canvas,
                width,
                yAxisXShift,
                yAxisIntervalLabels,
                headerHeight,
                itemSize,
                yCenterPosition,
                YAxisLabelFormatter);

            for (int chartIndex = 0; chartIndex < entriesCount; chartIndex++)
            {
                RangeChartEntry entry = Entries.ElementAt(chartIndex);

                var itemX = Margin + itemSize.Width / 2 + chartIndex * (itemSize.Width + Margin) + yAxisXShift;

                var label = labels[chartIndex];

                if (!string.IsNullOrEmpty(label))
                {
                    SKRect labelSize = labelSizes[chartIndex];

                    DrawHelper.DrawLabel(canvas,
                        LabelOrientation,
                        YPositionBehavior.None,
                        itemSize,
                        new SKPoint(itemX, height),
                        LabelColor,
                        labelSize,
                        label,
                        LabelTextSize,
                        Typeface);
                }

                if (entry == null || !entry.Value.HasValue || !entry.StartValue.HasValue)
                {
                    continue;
                }

                float barWidth = itemSize.Width * barWidthToSpaceRatio;

                if (entry.StartValue.Value < 0 && entry.Value.Value > 0)
                {
                    float positiveValue = entry.Value.Value;

                    float negativeValue = Math.Abs(entry.StartValue.Value);

                    if (positiveValue > 0)
                    {
                        var fullHeight = positiveValue / valRange * itemSize.Height;

                        float currentHeight = fullHeight * AnimationProgress;

                        var rectLocation = new SKPoint(itemX - barWidth / 2, yCenterPosition - currentHeight);
                        var rectSize = new SKSize(barWidth, currentHeight);

                        var rect = SKRect.Create(rectLocation, rectSize);

                        using (var paint = PaintExtensions.FillPaintWithColor(entry.Color))
                        {
                            canvas.DrawRectWithCornerRadius(rect, paint, barWidth/2, barWidth/2);
                        }
                    }

                    if (negativeValue > 0)
                    {
                        var fullHeight = negativeValue / valRange * itemSize.Height;

                        var currentHeight = fullHeight * AnimationProgress;

                        var rectLocation = new SKPoint(itemX - barWidth / 2, yCenterPosition);
                        var rectSize = new SKSize(barWidth, currentHeight);

                        var rect = SKRect.Create(rectLocation, rectSize);

                        using (var paint = PaintExtensions.FillPaintWithColor(entry.LowerColor))
                        {
                            canvas.DrawRectWithCornerRadius(rect, paint, bottomLeft: barWidth / 2, bottomRight: barWidth / 2);
                        }
                    }
                }
                else
                {
                    var entryRange = entry.Value.Value - entry.StartValue.Value;

                    var fullHeight = entryRange / valRange * itemSize.Height;

                    var currentHeight = fullHeight * AnimationProgress;

                    var yPosition = headerHeight + (maxValue - entry.Value.Value) / valRange * itemSize.Height;

                    var rectLocation = new SKPoint(itemX - barWidth / 2,  yPosition - fullHeight / 2 + currentHeight/2);
                    var rectSize = new SKSize(barWidth, currentHeight);

                    var rect = SKRect.Create(rectLocation, rectSize);

                    var color = entry.Value.Value > 0 ? entry.Color : entry.LowerColor;

                    using (var paint = PaintExtensions.FillPaintWithColor(color))
                    {
                        canvas.DrawRoundRect(rect, barWidth/2, barWidth/2, paint);
                    }
                }
            }
        }
        protected virtual float CalculateHeaderHeight(Dictionary<ChartEntry, SKRect> valueLabelSizes)
        {
            return MeasureHelper.CalculateFooterHeaderHeight(Margin, ValueLabelTextSize, valueLabelSizes.Values.ToArray(), ValueLabelOrientation);
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

        private SKSize CalculateItemSize(int items, int width, int height, float reservedSpace)
        {
            var w = (width - ((items + 1) * Margin)) / items;
            var h = height - Margin - reservedSpace;
            return new SKSize(w, h);
        }

        private static string EmptyLabelFormatter(float value)
        {
            return value.ToString();
        }

        #endregion
    }
}
