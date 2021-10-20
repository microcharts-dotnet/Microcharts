// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/RadialGauge.png)
    ///
    /// Radial gauge chart.
    /// </summary>
    public class RadialGaugeChart : SimpleChart
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

        private float AbsoluteMinimum => Entries?.Where(x=>x.Value.HasValue).Select(x => x.Value.Value).Concat(new[] { MaxValue, MinValue, InternalMinValue ?? 0 }).Min(x => Math.Abs(x)) ?? 0;

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
                Color = entry.Color.WithAlpha(LineAreaAlpha),
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(cx, cy, radius, paint);
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
                    var sweepAngle = AnimationProgress * 360 * (Math.Abs(value) - AbsoluteMinimum) / ValueRange;
                    path.AddArc(SKRect.Create(cx - radius, cy - radius, 2 * radius, 2 * radius), StartAngle, sweepAngle);
                    canvas.DrawPath(path, paint);
                }
            }
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Entries != null)
            {
                var sumValue = Entries.Where( x=>x.Value.HasValue).Sum(x => Math.Abs(x.Value.Value));
                var radius = (Math.Min(width, height) - (2 * Margin)) / 2;
                var cx = width / 2;
                var cy = height / 2;
                var lineWidth = (LineSize < 0) ? (radius / ((Entries.Count() + 1) * 2)) : LineSize;
                var radiusSpace = lineWidth * 2;

                for (int i = 0; i < Entries.Count(); i++)
                {
                    var entry = Entries.ElementAt(i);

                    //Skip the ring if it has a null value
                    if (!entry.Value.HasValue) continue;

                    var entryRadius = (i + 1) * radiusSpace;
                    DrawGaugeArea(canvas, entry, entryRadius, cx, cy, lineWidth);
                    DrawGauge(canvas, entry.Color, entry.Value.Value, entryRadius, cx, cy, lineWidth);
                }

                //Make sure captions draw on top of chart
                DrawCaption(canvas, width, height); 
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
