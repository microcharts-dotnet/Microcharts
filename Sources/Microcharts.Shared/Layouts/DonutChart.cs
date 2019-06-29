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
    public class DonutChart : Chart
    {
        #region Properties

        /// <summary>
        /// Gets or sets the radius of the hole in the center of the chart.
        /// </summary>
        /// <value>The hole radius.</value>
        public float HoleRadius { get; set; } = 0;

        /// <summary>
        /// Gets or sets the condition distribute tags of the chart.
        /// </summary>
        /// <value>The condition for distribute tags.</value>
        public bool IsDistributedTags { get; set; } = false;

        #endregion

        #region Methods

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            DrawCaption(canvas, width, height);
            using (new SKAutoCanvasRestore(canvas))
            {
                canvas.Translate(width / 2.0f, height / 2.0f);
                var sumValue = Entries.Sum(x => Math.Abs(x.Value));
                var radius = (Math.Min(width, height) - (2 * Margin)) / 2;

                var start = 0.0f;
                for (int i = 0; i < Entries.Count(); i++)
                {
                    var entry = Entries.ElementAt(i);
                    var end = start + (Math.Abs(entry.Value) / sumValue);

                    // Sector
                    var path = RadialHelpers.CreateSectorPath(start, end, radius, radius * HoleRadius);
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color,
                        IsAntialias = true
                    })
                    {
                        canvas.DrawPath(path, paint);
                    }

                    start = end;
                }
            }
        }

        private void DrawCaption(SKCanvas canvas, int width, int height)
        {
            var sumValue = Entries.Sum(x => Math.Abs(x.Value));
            var rightValues = new List<Entry>();
            var leftValues = new List<Entry>();

            int i = 0;
            var current = 0.0f;
            if (IsDistributedTags)
            {
                while (i < Entries.Count() && i < Entries.Count() / 2)
                {
                    var entry = Entries.ElementAt(i);
                    rightValues.Add(entry);
                    current += Math.Abs(entry.Value);
                    i++;
                }
            }
            else
            {
                while (i < Entries.Count() && current < sumValue / 2)
                {
                    var entry = Entries.ElementAt(i);
                    rightValues.Add(entry);
                    current += Math.Abs(entry.Value);
                    i++;
                }
            }

            while (i < Entries.Count())
            {
                var entry = Entries.ElementAt(i);
                leftValues.Add(entry);
                i++;
            }

            leftValues.Reverse();

            DrawCaptionElements(canvas, width, height, rightValues, false);
            DrawCaptionElements(canvas, width, height, leftValues, true);
        }

        #endregion
    }
}