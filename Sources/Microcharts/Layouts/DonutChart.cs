// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microcharts.Layouts;
    using SkiaSharp;

    /// <summary>
    /// ![chart](../images/Donut.png)
    /// 
    /// A donut chart.
    /// </summary>
    public class DonutChart : Chart
    {
        #region Properties

        /// <summary>
        /// Gets or sets the radius of the hole in the center of the chart.
        /// </summary>
        /// <value>The hole radius.</value>
        public float HoleRadius { get; set; } = 0.5f;

        /// <summary>
        /// Gets or sets a value whether the caption elements should all reside on the right side.
        /// </summary>
        public LabelMode LabelMode { get; set; } = LabelMode.LeftAndRight;

        /// <summary>
        /// Gets or sets whether the graph should be drawn in the center or automatically fill the space.
        /// </summary>
        public GraphPosition GraphPosition { get; set; } = GraphPosition.AutoFill;

        #endregion

        #region Methods

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (this.Entries != null)
            {
                this.DrawCaption(canvas, width, height);
                using (new SKAutoCanvasRestore(canvas))
                {
                    if (this.DrawDebugRectangles)
                    {
                        using (var paint = new SKPaint
                        {
                            Color = SKColors.Red,
                            IsStroke = true,
                        })
                        {
                            canvas.DrawRect(this.DrawableChartArea, paint);
                        }
                    }

                    canvas.Translate(this.DrawableChartArea.Left + this.DrawableChartArea.Width / 2, height / 2);
                    var sumValue = this.Entries.Sum(x => Math.Abs(x.Value));
                    var radius = (Math.Min(this.DrawableChartArea.Width, this.DrawableChartArea.Height) - (2 * Margin)) / 2;

                    var start = 0.0f;
                    for (int i = 0; i < this.Entries.Count(); i++)
                    {
                        var entry = this.Entries.ElementAt(i);
                        var end = start + ((Math.Abs(entry.Value) / sumValue) * this.AnimationProgress);

                        // Sector
                        var path = RadialHelpers.CreateSectorPath(start, end, radius, radius * this.HoleRadius);
                        using (var paint = new SKPaint
                        {
                            Style = SKPaintStyle.Fill,
                            Color = entry.Color,
                            IsAntialias = true,
                        })
                        {
                            canvas.DrawPath(path, paint);
                        }

                        start = end;
                    }
                }
            }
        }

        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var isGraphCentered =
                GraphPosition == GraphPosition.Center;

            switch (this.LabelMode)
            {
                case LabelMode.None:
                    return;

                case LabelMode.RightOnly:
                    this.DrawCaptionElements(canvas, width, height, this.Entries.ToList(), false, isGraphCentered);
                    return;

                case LabelMode.LeftAndRight:
                    this.DrawCaptionLeftAndRight(canvas, width, height, isGraphCentered);
                    return;
            }
        }

        private void DrawCaptionLeftAndRight(SKCanvas canvas, int width, int height, bool isGraphCentered)
        {
            var sumValue = this.Entries.Sum(x => Math.Abs(x.Value));
            var rightValues = new List<Entry>();
            var leftValues = new List<Entry>();

            int i = 0;
            var current = 0.0f;
            while (i < this.Entries.Count() && (current < sumValue / 2))
            {
                var entry = this.Entries.ElementAt(i);
                rightValues.Add(entry);
                current += Math.Abs(entry.Value);
                i++;
            }

            while (i < this.Entries.Count())
            {
                var entry = this.Entries.ElementAt(i);
                leftValues.Add(entry);
                i++;
            }

            leftValues.Reverse();

            this.DrawCaptionElements(canvas, width, height, rightValues, false, isGraphCentered);
            this.DrawCaptionElements(canvas, width, height, leftValues, true, isGraphCentered);
        }
        #endregion
    }
}