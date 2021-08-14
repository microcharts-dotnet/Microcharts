// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/Radar.png)
    ///
    /// A radar chart.
    /// </summary>
    public class RadarChart : SimpleChart
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

        private float AbsoluteMinimum => Entries.Where( x=>x.Value.HasValue).Select(x => x.Value.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Min(x => Math.Abs(x));

        private float AbsoluteMaximum => Entries.Where(x => x.Value.HasValue).Select(x => x.Value.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Max(x => Math.Abs(x));

        /// <inheritdoc />
        protected override float ValueRange => AbsoluteMaximum - AbsoluteMinimum;

        #endregion

        #region Methods

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            var total = Entries?.Count() ?? 0;

            if (total > 0)
            {
                var captionHeight = Entries.Max(x =>
                {
                    var result = 0.0f;

                    var hasLabel = !string.IsNullOrEmpty(x.Label);
                    var hasValueLabel = !string.IsNullOrEmpty(x.ValueLabel);

                    if (hasLabel || hasValueLabel)
                    {
                        var hasOffset = hasLabel && hasValueLabel;
                        var captionMargin = LabelTextSize * 0.60f;
                        var space = hasOffset ? captionMargin : 0;

                        if (hasLabel)
                        {
                            result += LabelTextSize;
                        }

                        if (hasValueLabel)
                        {
                            result += LabelTextSize;
                        }
                    }

                    return result;
                });

                var center = new SKPoint(width / 2, height / 2);
                var radius = ((Math.Min(width, height) - (2 * Margin)) / 2) - captionHeight;
                var rangeAngle = (float)((Math.PI * 2) / total);
                var startAngle = (float)Math.PI;

                DrawBorder(canvas, center, radius);

                using (var clip = new SKPath())
                {
                    clip.AddCircle(center.X, center.Y, radius);

                    
                    for (int i = 0; i < total; i++)
                    {
                        var angle = startAngle + (rangeAngle * i);
                        var entry = Entries.ElementAt(i);


                        int nextIndex = (i + 1) % total;
                        var nextAngle = startAngle + (rangeAngle * nextIndex);
                        var nextEntry = Entries.ElementAt(nextIndex);
                        while( !nextEntry.Value.HasValue)
                        {
                            nextIndex = (nextIndex + 1) % total;
                            nextAngle = startAngle + (rangeAngle * nextIndex);
                            nextEntry = Entries.ElementAt(nextIndex);
                        }

                        canvas.Save();
                        if (entry.Value.HasValue)
                        {
                            var point = GetPoint(entry.Value.Value * AnimationProgress, center, angle, radius);
                            var nextPoint = GetPoint(nextEntry.Value.Value * AnimationProgress, center, nextAngle, radius);

                            canvas.ClipPath(clip);

                            // Border center bars
                            using (var paint = new SKPaint()
                            {
                                Style = SKPaintStyle.Stroke,
                                StrokeWidth = BorderLineSize,
                                Color = BorderLineColor,
                                IsAntialias = true,
                            })
                            {
                                var borderPoint = GetPoint(MaxValue, center, angle, radius);
                                canvas.DrawLine(point.X, point.Y, borderPoint.X, borderPoint.Y, paint);
                            }

                            // Values points and lines
                            using (var paint = new SKPaint()
                            {
                                Style = SKPaintStyle.Stroke,
                                StrokeWidth = BorderLineSize,
                                Color = entry.Color.WithAlpha((byte)(entry.Color.Alpha * 0.75f * AnimationProgress)),
                                PathEffect = SKPathEffect.CreateDash(new[] { BorderLineSize, BorderLineSize * 2 }, 0),
                                IsAntialias = true,
                            })
                            {
                                var amount = Math.Abs(entry.Value.Value - AbsoluteMinimum) / ValueRange;
                                canvas.DrawCircle(center.X, center.Y, radius * amount, paint);
                            }

                            canvas.DrawGradientLine(center, entry.Color.WithAlpha(0), point, entry.Color.WithAlpha((byte)(entry.Color.Alpha * 0.75f)), LineSize);
                            canvas.DrawGradientLine(point, entry.Color, nextPoint, nextEntry.Color, LineSize);
                            canvas.DrawPoint(point, entry.Color, PointSize, PointMode);
                        }
                        canvas.Restore();

                        // Labels
                        var labelPoint = new SKPoint(0, radius + LabelTextSize + (PointSize / 2));
                        var rotation = SKMatrix.CreateRotation(angle);
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

                        canvas.DrawCaptionLabels(entry.Label, entry.TextColor, entry.ValueLabel, entry.Color.WithAlpha((byte)(255 * AnimationProgress)), LabelTextSize, labelPoint, alignment, base.Typeface, out var _);
                    }
                }
            }
        }

        /// <summary>
        /// Finds point coordinates of an entry.
        /// </summary>
        /// <returns>The point.</returns>
        /// <param name="value">The value.</param>
        /// <param name="center">The center.</param>
        /// <param name="angle">The entry angle.</param>
        /// <param name="radius">The radius.</param>
        private SKPoint GetPoint(float value, SKPoint center, float angle, float radius)
        {
            var amount = Math.Abs(value - AbsoluteMinimum) / ValueRange;
            var point = new SKPoint(0, radius * amount);
            var rotation = SKMatrix.CreateRotation(angle);
            return center + rotation.MapPoint(point);
        }

        private void DrawBorder(SKCanvas canvas, SKPoint center, float radius)
        {
            using (var paint = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = BorderLineSize,
                Color = BorderLineColor,
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(center.X, center.Y, radius, paint);
            }
        }

        #endregion
    }
}
