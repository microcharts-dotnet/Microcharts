// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/HalfRadialGauge.png)
    ///
    /// Radial gauge chart.
    /// </summary>
    public class HalfRadialGaugeChart : SimpleChart
    {
        #region Properties

        /// <summary>
        /// Gets or sets the size of each gauge. If negative, then its will be calculated from the available space.
        /// </summary>
        /// <value>The size of the line.</value>
        public float LineSize { get; set; } = -1;

        /// <summary>
        /// Gets or sets the gauge background area alpha.
        /// </summary>
        /// <value>The line area alpha.</value>
        public byte LineAreaAlpha { get; set; } = 52;

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>The start angle.</value>
        public float StartAngle { get; set; } = -90;

        private float AbsoluteMinimum => Entries?.Where(x => x.Value.HasValue).Select(x => x.Value.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Min(x => Math.Abs(x)) ?? 0;

        private float AbsoluteMaximum => Entries?.Where(x => x.Value.HasValue).Select(x => x.Value.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Max(x => Math.Abs(x)) ?? 0;

        /// <inheritdoc />
        protected override float ValueRange => AbsoluteMaximum - AbsoluteMinimum;

        #endregion

        #region Methods

        public void DrawGaugeArea(SKCanvas canvas, ChartEntry entry, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                StrokeCap = SKStrokeCap.Round,
                Color = entry.Color.WithAlpha(LineAreaAlpha),
                IsAntialias = true,
            })
            {
                using (SKPath path = new SKPath())
                {
                    path.AddArc(SKRect.Create(cx - radius * 2, cy - radius * 2, 4 * radius, 4 * radius), 180, 180);
                    canvas.DrawPath(path, paint);
                }
            }
        }

        public void DrawGauge(SKCanvas canvas, SKColor color, float value, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                StrokeCap = SKStrokeCap.Round,
                Color = color,
                IsAntialias = true,
            })
            {
                using (SKPath path = new SKPath())
                {
                    var sweepAngle =  AnimationProgress * 180 * (Math.Abs(value) - AbsoluteMinimum) / ValueRange;
                    path.AddArc(SKRect.Create(cx - radius * 2, cy - radius * 2, 4 * radius, 4 * radius), 180, sweepAngle);
                    canvas.DrawPath(path, paint);
                }
            }
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Entries != null)
            {
                DrawCaption(canvas, width, height);

                var sumValue = Entries.Where(x => x.Value.HasValue).Sum(x => Math.Abs(x.Value.Value));
                var radius = (Math.Min(width, height) - (2 * Margin)) / 2;
                if (width / 2 < height)
                    radius = (Math.Min(width, height) - (2 * Margin)) / 4;
                var cx = width / 2;
                var cy = height / 2 + (int)radius - (int)Margin;
                var lineWidth = (LineSize < 0) ? (radius / (Entries.Count() + 1)) : LineSize;
                var radiusSpace = lineWidth;

                for (int i = 0; i < Entries.Count(); i++)
                {
                    var entry = Entries.ElementAt(i);

                    //Skip the ring if it has a null value
                    if (!entry.Value.HasValue) continue;

                    var entryRadius = (i + 1) * radiusSpace;
                    if (entries.Count() == 1)
                        entryRadius = radius - radiusSpace / 2;
                    DrawGaugeArea(canvas, entry, entryRadius, cx, cy, lineWidth);
                    DrawGauge(canvas, entry.Color, entry.Value.Value, entryRadius, cx, cy, lineWidth);
                }
            }
        }

        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var rightValues = Entries.Take(Entries.Count() / 2).ToList();
            var leftValues = Entries.Skip(rightValues.Count()).ToList();

            leftValues.Reverse();

            DrawCaptionElements(canvas, width, height, rightValues, false, false);
            DrawCaptionElements(canvas, width, height, leftValues, true, false);
        }

        #endregion
    }
}
