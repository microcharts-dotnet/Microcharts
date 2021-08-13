// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/Donut.png)
    ///
    /// A donut chart.
    /// </summary>
    public class DonutChart : SimpleChart
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
            if (Entries != null)
            {
                DrawCaption(canvas, width, height);
                using (new SKAutoCanvasRestore(canvas))
                {
                    if (DrawDebugRectangles)
                    {
                        using (var paint = new SKPaint
                        {
                            Color = SKColors.Red,
                            IsStroke = true,
                        })
                        {
                            canvas.DrawRect(DrawableChartArea, paint);
                        }
                    }

                    canvas.Translate(DrawableChartArea.Left + DrawableChartArea.Width / 2, height / 2);

                    var sumValue = Entries.Where( x => x.Value.HasValue ).Sum(x => Math.Abs(x.Value.Value));
                    var radius = (Math.Min(DrawableChartArea.Width, DrawableChartArea.Height) - (2 * Margin)) / 2;
                    var start = 0.0f;

                    for (int i = 0; i < Entries.Count(); i++)
                    {
                        var entry = Entries.ElementAt(i);
                        if (!entry.Value.HasValue) continue;

                        var end = start + ((Math.Abs(entry.Value.Value) / sumValue) * AnimationProgress);

                        // Sector
                        var path = RadialHelpers.CreateSectorPath(start, end, radius, radius * HoleRadius);
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
            var isGraphCentered = GraphPosition == GraphPosition.Center;
            var sumValue = Entries.Where( x => x.Value.HasValue ).Sum(x => Math.Abs(x.Value.Value));

            switch (LabelMode)
            {
                case LabelMode.None:
                    return;

                case LabelMode.RightOnly:
                    DrawCaptionElements(canvas, width, height, Entries.ToList(), false, isGraphCentered);
                    return;

                case LabelMode.LeftAndRight:
                    DrawCaptionLeftAndRight(canvas, width, height, isGraphCentered);
                    return;
            }
        }

        private void DrawCaptionLeftAndRight(SKCanvas canvas, int width, int height, bool isGraphCentered)
        {
            var sumValue = Entries.Where(x => x.Value.HasValue).Sum(x => Math.Abs(x.Value.Value));
            var rightValues = new List<ChartEntry>();
            var leftValues = new List<ChartEntry>();
            int i = 0;
            var current = 0.0f;

            while (i < Entries.Count() && (current < sumValue / 2))
            {
                var entry = Entries.ElementAt(i);
                if (!entry.Value.HasValue) continue;

                rightValues.Add(entry);
                current += Math.Abs(entry.Value.Value);
                i++;
            }

            while (i < Entries.Count())
            {
                var entry = Entries.ElementAt(i);
                if (!entry.Value.HasValue) continue;

                leftValues.Add(entry);
                i++;
            }

            leftValues.Reverse();

            DrawCaptionElements(canvas, width, height, rightValues, false, isGraphCentered);
            DrawCaptionElements(canvas, width, height, leftValues, true, isGraphCentered);
        }

        #endregion
    }
}
