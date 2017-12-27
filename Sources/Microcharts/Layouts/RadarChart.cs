// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts
{
    using System;
    using System.Linq;
    using SkiaSharp;

    /// <summary>
    /// ![chart](../images/Radar.png)
    /// 
    /// A radar chart.
    /// </summary>
    public class RadarChart : Chart
    {
        #region Constants

        private const float Epsilon = 0.01f;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the size of the line.
        /// </summary>
        /// <value>The size of the line.</value>
        public float LineSize { get; set; } = 3;

        /// <summary>
        /// Gets or sets the color of the border line.
        /// </summary>
        /// <value>The color of the border line.</value>
        public SKColor BorderLineColor { get; set; } = SKColors.LightGray.WithAlpha(110);

        /// <summary>
        /// Gets or sets the size of the border line.
        /// </summary>
        /// <value>The size of the border line.</value>
        public float BorderLineSize { get; set; } = 2;

        /// <summary>
        /// Gets or sets the point mode.
        /// </summary>
        /// <value>The point mode.</value>
        public PointMode PointMode { get; set; } = PointMode.Circle;

        /// <summary>
        /// Gets or sets the size of the points.
        /// </summary>
        /// <value>The size of the point.</value>
        public float PointSize { get; set; } = 14;

        private float AbsoluteMinimum => this.Entries.Select(x => x.Value).Concat(new[] { this.MaxValue, this.MinValue, this.InternalMinValue ?? 0 }).Min(x => Math.Abs(x));

        private float AbsoluteMaximum => this.Entries.Select(x => x.Value).Concat(new[] { this.MaxValue, this.MinValue, this.InternalMinValue ?? 0 }).Max(x => Math.Abs(x));

        private float ValueRange => this.AbsoluteMaximum - this.AbsoluteMinimum;

        #endregion

        #region Methods

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var total = this.Entries?.Count() ?? 0;

            if (total > 0)
            {
                var captionHeight = this.Entries.Max(x =>
                {
                    var result = 0.0f;

                    var hasLabel = !string.IsNullOrEmpty(x.Label);
                    var hasValueLabel = !string.IsNullOrEmpty(x.ValueLabel);
                    if (hasLabel || hasValueLabel)
                    {
                        var hasOffset = hasLabel && hasValueLabel;
                        var captionMargin = this.LabelTextSize * 0.60f;
                        var space = hasOffset ? captionMargin : 0;

                        if (hasLabel)
                        {
                            result += this.LabelTextSize;
                        }

                        if (hasValueLabel)
                        {
                            result += this.LabelTextSize;
                        }
                    }

                    return result;
                });

                var center = new SKPoint(width / 2, height / 2);
                var radius = ((Math.Min(width, height) - (2 * Margin)) / 2) - captionHeight;
                var rangeAngle = (float)((Math.PI * 2) / total);
                var startAngle = (float)Math.PI;

                var nextEntry = this.Entries.First();
                var nextAngle = startAngle;
                var nextPoint = this.GetPoint(nextEntry.Value * this.AnimationProgress, center, nextAngle, radius);

                this.DrawBorder(canvas, center, radius);

                using (var clip = new SKPath())
                {
                    clip.AddCircle(center.X, center.Y, radius);

                    for (int i = 0; i < total; i++)
                    {
                        var angle = nextAngle;
                        var entry = nextEntry;
                        var point = nextPoint;

                        var nextIndex = (i + 1) % total;
                        nextAngle = startAngle + (rangeAngle * nextIndex);
                        nextEntry = this.Entries.ElementAt(nextIndex);
                        nextPoint = this.GetPoint(nextEntry.Value * this.AnimationProgress, center, nextAngle, radius);

                        canvas.Save();
                        canvas.ClipPath(clip);

                        // Border center bars
                        using (var paint = new SKPaint()
                        {
                            Style = SKPaintStyle.Stroke,
                            StrokeWidth = this.BorderLineSize,
                            Color = this.BorderLineColor,
                            IsAntialias = true,
                        })
                        {
                            var borderPoint = this.GetPoint(this.MaxValue, center, angle, radius);
                            canvas.DrawLine(point.X, point.Y, borderPoint.X, borderPoint.Y, paint);
                        }

                        // Values points and lines
                        using (var paint = new SKPaint()
                        {
                            Style = SKPaintStyle.Stroke,
                            StrokeWidth = this.BorderLineSize,
                            Color = entry.Color.WithAlpha((byte)(entry.Color.Alpha * 0.75f * this.AnimationProgress)),
                            PathEffect = SKPathEffect.CreateDash(new[] { this.BorderLineSize, this.BorderLineSize * 2 }, 0),
                            IsAntialias = true,
                        })
                        {
                            var amount = Math.Abs(entry.Value - this.AbsoluteMinimum) / this.ValueRange;
                            canvas.DrawCircle(center.X, center.Y, radius * amount, paint);
                        }

                        canvas.DrawGradientLine(center, entry.Color.WithAlpha(0), point, entry.Color.WithAlpha((byte)(entry.Color.Alpha * 0.75f)), this.LineSize);
                        canvas.DrawGradientLine(point, entry.Color, nextPoint, nextEntry.Color, this.LineSize);
                        canvas.DrawPoint(point, entry.Color, this.PointSize, this.PointMode);

                        canvas.Restore();

                        // Labels
                        var labelPoint = new SKPoint(0, radius + this.LabelTextSize + (this.PointSize / 2));
                        var rotation = SKMatrix.MakeRotation(angle);
                        labelPoint = center + rotation.MapPoint(labelPoint);
                        var alignment = SKTextAlign.Left;

                        if ((Math.Abs(angle - (startAngle + Math.PI)) < Epsilon) || (Math.Abs(angle - Math.PI) < Epsilon))
                        {
                            alignment = SKTextAlign.Center;
                        }
                        else if (angle > (float)(startAngle + Math.PI))
                        {
                            alignment = SKTextAlign.Right;
                        }

                        canvas.DrawCaptionLabels(entry.Label, entry.TextColor, entry.ValueLabel, entry.Color.WithAlpha((byte)(255 * this.AnimationProgress)), this.LabelTextSize, labelPoint, alignment, base.Typeface);
                    }
                }
            }
        }

        /// <summary>
        /// Finds point cordinates of an entry.
        /// </summary>
        /// <returns>The point.</returns>
        /// <param name="value">The value.</param>
        /// <param name="center">The center.</param>
        /// <param name="angle">The entry angle.</param>
        /// <param name="radius">The radius.</param>
        private SKPoint GetPoint(float value, SKPoint center, float angle, float radius)
        {
            var amount = Math.Abs(value - this.AbsoluteMinimum) / this.ValueRange;
            var point = new SKPoint(0, radius * amount);
            var rotation = SKMatrix.MakeRotation(angle);
            return center + rotation.MapPoint(point);
        }

        private void DrawBorder(SKCanvas canvas, SKPoint center, float radius)
        {
            using (var paint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = this.BorderLineSize,
                Color = this.BorderLineColor,
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(center.X, center.Y, radius, paint);
            }
        }

        #endregion
    }
}