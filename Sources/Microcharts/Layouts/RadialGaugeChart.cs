// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts
{
    using System;
    using System.Linq;
    using SkiaSharp;

    /// <summary>
    /// ![chart](../images/RadialGauge.png)
    /// 
    /// Radial gauge chart.
    /// </summary>
    public class RadialGaugeChart : Chart
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

        private float AbsoluteMinimum => this.Entries?.Select(x => x.Value).Concat(new[] { this.MaxValue, this.MinValue, this.InternalMinValue ?? 0 }).Min(x => Math.Abs(x)) ?? 0;

        private float AbsoluteMaximum => this.Entries?.Select(x => x.Value).Concat(new[] { this.MaxValue, this.MinValue, this.InternalMinValue ?? 0 }).Max(x => Math.Abs(x)) ?? 0;

        private float ValueRange => this.AbsoluteMaximum - this.AbsoluteMinimum;

        #endregion

        #region Methods

        public void DrawGaugeArea(SKCanvas canvas, Entry entry, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                Color = entry.Color.WithAlpha(this.LineAreaAlpha),
                IsAntialias = true,
            })
            {
                canvas.DrawCircle(cx, cy, radius, paint);
            }
        }

        public void DrawGauge(SKCanvas canvas, Entry entry, float radius, int cx, int cy, float strokeWidth)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = strokeWidth,
                StrokeCap = SKStrokeCap.Round,
                Color = entry.Color,
                IsAntialias = true,
            })
            {
                using (SKPath path = new SKPath())
                {
                    var sweepAngle = this.AnimationProgress * 360 * (Math.Abs(entry.Value) - this.AbsoluteMinimum) / this.ValueRange;
                    path.AddArc(SKRect.Create(cx - radius, cy - radius, 2 * radius, 2 * radius), this.StartAngle, sweepAngle);
                    canvas.DrawPath(path, paint);
                }
            }
        }

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (this.Entries != null)
            {
                this.DrawCaption(canvas, width, height);

                var sumValue = this.Entries.Sum(x => Math.Abs(x.Value));
                var radius = (Math.Min(width, height) - (2 * Margin)) / 2;
                var cx = width / 2;
                var cy = height / 2;
                var lineWidth = (this.LineSize < 0) ? (radius / ((this.Entries.Count() + 1) * 2)) : this.LineSize;
                var radiusSpace = lineWidth * 2;

                for (int i = 0; i < this.Entries.Count(); i++)
                {
                    var entry = this.Entries.ElementAt(i);
                    var entryRadius = (i + 1) * radiusSpace;
                    this.DrawGaugeArea(canvas, entry, entryRadius, cx, cy, lineWidth);
                    this.DrawGauge(canvas, entry, entryRadius, cx, cy, lineWidth);
                } 
            }
        }

        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var rightValues = this.Entries.Take(Entries.Count() / 2).ToList();
            var leftValues = this.Entries.Skip(rightValues.Count()).ToList();

            leftValues.Reverse();

            this.DrawCaptionElements(canvas, width, height, rightValues, false, false);
            this.DrawCaptionElements(canvas, width, height, leftValues, true, false);
        }

        #endregion
    }
}