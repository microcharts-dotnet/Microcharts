using System;
using System.Collections.Generic;
using SkiaSharp;

namespace Microcharts
{
    public class ExtendedBarChart : AxisBasedChart
    {
        private const float RotatedTextXCorrection = 0.433f;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.ExtendedBarChart"/> class.
        /// </summary>
        public ExtendedBarChart() : base()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or sets the minimum height for a bar
        /// </summary>
        /// <value>The minium height of a bar.</value>
        public float MinBarHeight { get; set; } = DefaultValues.MinBarHeight;

        /// <summary>
        /// Get or sets the corner radius for a bar
        /// </summary>
        /// <value>The corner radius of a bar.</value>
        public float CornerRadius { get; set; } = DefaultValues.CornerRadius;

        public VerticalTextOrientation VerticalValueLabelTextOrientation { get; set; } = VerticalTextOrientation.Left;

        public VerticalTextOrientation VerticalLabelTextOrientation { get; set; } = VerticalTextOrientation.RotatedToRight;

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

            (SKPoint location, SKSize size) = GetBarDrawingProperties(headerWithLegendHeight, itemSize, barSize, origin, barX, barY);

            if (ValueLabelOption == ValueLabelOption.TopOfChart)
            {
                base.DrawValueLabel(canvas, valueLabelSizes, headerWithLegendHeight, itemSize, barSize, entry, barX, barY, itemX, origin);

                return;
            }

            if (valueLabelSizes[entry].Width * 2 > size.Height || ValueLabelOption is ValueLabelOption.TopOfElement)
            {
                var bounds = valueLabelSizes[entry];

                DrawHelper.DrawLabel(canvas,
                    ValueLabelOrientation,
                    VerticalValueLabelTextOrientation is VerticalTextOrientation.Left ? YPositionBehavior.UpToElementMiddle : YPositionBehavior.UpToElementHeight,
                    barSize,
                    new SKPoint(location.X + size.Width/2, origin - size.Height - (VerticalValueLabelTextOrientation is VerticalTextOrientation.Left ? 0 : Margin)),
                    entry.ValueLabelColor.WithAlpha((byte)(255 * AnimationProgress)),
                    valueLabelSizes[entry],
                    entry.ValueLabel,
                    ValueLabelTextSize,
                    Typeface,
                    VerticalValueLabelTextOrientation);

                return;
            }

            if (ValueLabelOption == ValueLabelOption.OverElement)
            {
                var bounds = valueLabelSizes[entry];

                DrawHelper.DrawLabel(canvas,
                    ValueLabelOrientation,
                    VerticalValueLabelTextOrientation == VerticalTextOrientation.Left ? YPositionBehavior.DownToElementMiddle : YPositionBehavior.UpToElementMiddle,
                    barSize,
                    new SKPoint(location.X + size.Width/2, origin - size.Height + bounds.Width),
                    BackgroundColor.WithAlpha((byte)(255 * AnimationProgress)),
                    valueLabelSizes[entry],
                    entry.ValueLabel,
                    ValueLabelTextSize,
                    Typeface,
                    VerticalValueLabelTextOrientation);
            }
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

                canvas.DrawRectWithCornerRadius(rect, paint, topLeft: CornerRadius, topRight: CornerRadius);
            }
        }

        protected override void DrawBarArea(SKCanvas canvas, float headerHeight, SKSize itemSize, SKSize barSize, SKColor color,
            SKColor otherColor, float origin, float value, float barX, float barY)
        {
            /* Nothing to do here */
        }

        protected override void DrawLabels(SKCanvas canvas, float height, float footerWithLegendHeight, float yAxisXShift, SKSize itemSize,
            SKRect[] labelSizes, string[] labels)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                var itemX = Margin + (itemSize.Width / 2) + (i * (itemSize.Width + Margin)) + yAxisXShift;

                string label = labels[i];

                if (!string.IsNullOrEmpty(label))
                {
                    SKRect labelSize = labelSizes[i];
                    var yPositionBehaviour = YPositionBehavior.None;
                    var yAdjustment = 0f;

                    switch (VerticalLabelTextOrientation)
                    {
                        case VerticalTextOrientation.RotatedToRight:
                            itemX += labelSize.Height * RotatedTextXCorrection;
                            break;
                        case VerticalTextOrientation.Right:
                            itemX += Margin;
                            break;
                        case VerticalTextOrientation.Left:
                            yPositionBehaviour = YPositionBehavior.DownToElementMiddle;
                            itemX += labelSize.Height;
                            yAdjustment = labelSize.Width / 2;
                            break;
                    }

                    DrawHelper.DrawLabel(canvas,
                        LabelOrientation,
                        yPositionBehaviour,
                        itemSize,
                        new SKPoint(itemX, height - footerWithLegendHeight + Margin + yAdjustment),
                        LabelColor,
                        labelSize,
                        label,
                        LabelTextSize,
                        Typeface,
                        VerticalLabelTextOrientation);
                }
            }
        }

        protected override SKSize CalculateItemSize(int items, int width, int height, float reservedSpace)
        {
            var w = (width - Margin * 2- ((items + 1) * Margin)) / items;
            var h = height - Margin - reservedSpace;

            return new SKSize(w, h);
        }

        private (SKPoint location, SKSize size) GetBarDrawingProperties(float headerHeight, SKSize itemSize, SKSize barSize, float origin, float barX, float barY)
        {
            var x = Margin + barX - (itemSize.Width / 2);
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

        #endregion
    }
}
