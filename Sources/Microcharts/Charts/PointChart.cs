// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using SkiaSharp;
using Topten.RichTextKit;

namespace Microcharts
{
    /// <summary>
    /// ![chart](../images/Point.png)
    ///
    /// Point chart.
    /// </summary>
    public class PointChart : Chart
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.PointChart"/> class.
        /// </summary>
        public PointChart()
        {
            LabelOrientation = Orientation.Default;
            ValueLabelOrientation = Orientation.Default;

            YAxisTextPaint = new SKPaint
            {
                Color = SKColors.Black,
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

        #endregion

        #region Fields

        private Orientation labelOrientation, valueLabelOrientation;

        #endregion

        #region Properties
        /// <summary>
        /// Show Y Axis Text?
        /// </summary>
        public bool ShowYAxisText { get; set; } = false;

        /// <summary>
        /// Show Y Axis Lines?
        /// </summary>
        public bool ShowYAxisLines { get; set; } = false;

        //TODO : calculate this automatically, based on available area height and text height
        /// <summary>
        /// Y Axis Max Ticks
        /// </summary>
        public int YAxisMaxTicks { get; set; } = 5;

        /// <summary>
        /// Y Axis Position
        /// </summary>
        public Position YAxisPosition { get; set; } = Position.Right;

        /// <summary>
        /// Y Axis Paint
        /// </summary>
        public SKPaint YAxisTextPaint { get; set; }

        /// <summary>
        /// Y Axis Paint
        /// </summary>
        public SKPaint YAxisLinesPaint { get; set; }

        /// <summary>
        /// Gets or sets the size of the point.
        /// </summary>
        /// <value>The size of the point.</value>
        public float PointSize { get; set; } = 14;

        /// <summary>
        /// Gets or sets the point mode.
        /// </summary>
        /// <value>The point mode.</value>
        public PointMode PointMode { get; set; } = PointMode.Circle;

        /// <summary>
        /// Gets or sets the point area alpha.
        /// </summary>
        /// <value>The point area alpha.</value>
        public byte PointAreaAlpha { get; set; } = 100;

        /// <summary>
        /// Gets or sets the text orientation of labels.
        /// </summary>
        /// <value>The label orientation.</value>
        public Orientation LabelOrientation
        {
            get => labelOrientation;
            set => labelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        /// <summary>
        /// Gets or sets the text orientation of value labels.
        /// </summary>
        /// <value>The label orientation.</value>
        public Orientation ValueLabelOrientation
        {
            get => valueLabelOrientation;
            set => valueLabelOrientation = (value == Orientation.Default) ? Orientation.Vertical : value;
        }

        private float ValueRange => MaxValue - MinValue;

        #endregion

        #region Methods

        public override void DrawContent(SKCanvas canvas, int width, int height)
        {
            if (Entries != null)
            {
                var yAxisXShift = 0.0f;
                var yAxisIntervalLabels = new List<float>();

                if (ShowYAxisText || ShowYAxisLines)
                {
                    var yAxisWidth = width;

                    var enumerable = Entries.ToList(); // to avoid double enumeration

                    NiceScale.Calculate(enumerable.Min(e => e.Value), enumerable.Max(e => e.Value), YAxisMaxTicks, out var range, out var tickSpacing, out var niceMin, out var niceMax);

                    var ticks = (int)(range / tickSpacing);

                    yAxisIntervalLabels = Enumerable.Range(0, ticks)
                        .Select(i => (float)(niceMax - (i * tickSpacing)))
                        .ToList();

                    var longestYAxisLabel = yAxisIntervalLabels.Aggregate(string.Empty, (max, cur) => max.Length > cur.ToString().Length ? max : cur.ToString());
                    var longestYAxisLabelWidth = MeasureLabel(longestYAxisLabel, YAxisTextPaint).Width;

                    yAxisWidth = (int)(width - longestYAxisLabelWidth);

                    if (YAxisPosition == Position.Left)
                    {
                        yAxisXShift = longestYAxisLabelWidth;
                    }

                    // to reduce chart width
                    width = yAxisWidth;
                }

                var labels = Entries.Select(x => x.Label).ToArray();
                var labelSizes = MeasureLabels(labels);
                var footerHeight = CalculateFooterHeaderHeight(labelSizes, LabelOrientation);

                var valueLabels = Entries.Select(x => x.ValueLabel).ToArray();
                var valueLabelSizes = MeasureLabels(valueLabels);
                var headerHeight = CalculateFooterHeaderHeight(valueLabelSizes, ValueLabelOrientation);

                var itemSize = CalculateItemSize(width, height, footerHeight, headerHeight);
                var origin = CalculateYOrigin(itemSize.Height, headerHeight);
                var points = CalculatePoints(itemSize, origin, headerHeight, yAxisXShift);
                var cnt = 0;

                if (ShowYAxisText || ShowYAxisLines)
                {
                    var intervals = yAxisIntervalLabels
                        .Select(t => new ValueTuple<string, SKPoint>
                        (
                            t.ToString(),
                            new SKPoint(YAxisPosition == Position.Left ? yAxisXShift : width, CalculatePoint(t, cnt++, itemSize, origin, headerHeight).Y)
                        ))
                        .ToList();

                    if (ShowYAxisText)
                    {
                        DrawYAxisText(canvas, intervals);
                    }

                    if (ShowYAxisLines)
                    {
                        var lines = intervals.Select(tup =>
                        {
                            (_, SKPoint pt) = tup;

                            return YAxisPosition == Position.Right ?
                                SKRect.Create(0, pt.Y, width, 0) :
                                SKRect.Create(yAxisXShift, pt.Y, width, 0);
                        });

                        DrawYAxisLines(canvas, lines);
                    }
                }

                DrawAreas(canvas, points, itemSize, origin, headerHeight);
                DrawPoints(canvas, points);
                DrawHeader(canvas, valueLabels, valueLabelSizes, points, itemSize, height, headerHeight);
                DrawFooter(canvas, labels, labelSizes, points, itemSize, height, footerHeight);
            }
        }

        protected float CalculateYOrigin(float itemHeight, float headerHeight)
        {
            if (MaxValue <= 0)
            {
                return headerHeight;
            }

            if (MinValue > 0)
            {
                return headerHeight + itemHeight;
            }

            return headerHeight + ((MaxValue / ValueRange) * itemHeight);
        }

        protected SKSize CalculateItemSize(int width, int height, float footerHeight, float headerHeight)
        {
            var total = Entries.Count();
            var w = (width - ((total + 1) * Margin)) / total;
            var h = height - Margin - footerHeight - headerHeight;
            return new SKSize(w, h);
        }

        protected SKPoint[] CalculatePoints(SKSize itemSize, float origin, float headerHeight, float originX = 0)
        {
            var result = new List<SKPoint>();

            for (int i = 0; i < Entries.Count(); i++)
            {
                var entry = Entries.ElementAt(i);
                var value = entry.Value;

                result.Add(CalculatePoint(value, i, itemSize, origin, headerHeight, originX));
            }

            return result.ToArray();
        }

        protected SKPoint CalculatePoint(float value, int i, SKSize itemSize, float origin, float headerHeight, float originX = 0)
        {
            var x = originX + Margin + (itemSize.Width / 2) + (i * (itemSize.Width + Margin));
            var y = headerHeight + ((1 - AnimationProgress) * (origin - headerHeight) + (((MaxValue - value) / ValueRange) * itemSize.Height) * AnimationProgress);

            return new SKPoint(x, y);
        }

        protected void DrawHeader(SKCanvas canvas, string[] labels, SKRect[] labelSizes, SKPoint[] points, SKSize itemSize, int height, float headerHeight)
        {
            DrawLabels(canvas,
                            labels,
                            points.Select(p => new SKPoint(p.X, headerHeight - Margin)).ToArray(),
                            labelSizes,
                            Entries.Select(x => x.Color.WithAlpha((byte)(255 * AnimationProgress))).ToArray(),
                            ValueLabelOrientation,
                            true,
                            itemSize,
                            height);
        }

        protected virtual void DrawFooter(SKCanvas canvas, string[] labels, SKRect[] labelSizes, SKPoint[] points, SKSize itemSize, int height, float footerHeight)
        {
            DrawLabels(canvas,
                            labels,
                            points.Select(p => new SKPoint(p.X, height - footerHeight + Margin)).ToArray(),
                            labelSizes,
                            Entries.Select(x => LabelColor).ToArray(),
                            LabelOrientation,
                            false,
                            itemSize,
                            height);
        }

        protected virtual void DrawPoints(SKCanvas canvas, SKPoint[] points)
        {
            if (points.Length > 0 && PointMode != PointMode.None)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];
                    canvas.DrawPoint(point, entry.Color, PointSize, PointMode);
                }
            }
        }

        protected virtual void DrawAreas(SKCanvas canvas, SKPoint[] points, SKSize itemSize, float origin, float headerHeight)
        {
            if (points.Length > 0 && PointAreaAlpha > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];
                    var y = Math.Min(origin, point.Y);

                    using (var shader = SKShader.CreateLinearGradient(new SKPoint(0, origin), new SKPoint(0, point.Y), new[] { entry.Color.WithAlpha(PointAreaAlpha), entry.Color.WithAlpha((byte)(PointAreaAlpha / 3)) }, null, SKShaderTileMode.Clamp))
                    using (var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = entry.Color.WithAlpha(PointAreaAlpha),
                    })
                    {
                        paint.Shader = shader;
                        var height = Math.Max(2, Math.Abs(origin - point.Y));
                        canvas.DrawRect(SKRect.Create(point.X - (PointSize / 2), y, PointSize, height), paint);
                    }
                }
            }
        }

        /// <summary>
        /// draws the labels
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="texts"></param>
        /// <param name="points"></param>
        /// <param name="sizes"></param>
        /// <param name="colors"></param>
        /// <param name="orientation"></param>
        /// <param name="isTop"></param>
        /// <param name="itemSize"></param>
        /// <param name="height"></param>
        protected virtual void DrawLabels(SKCanvas canvas, string[] texts, SKPoint[] points, SKRect[] sizes, SKColor[] colors, Orientation orientation, bool isTop, SKSize itemSize, float height)
        {
            if (points.Length > 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    var entry = Entries.ElementAt(i);
                    var point = points[i];

                    if (!string.IsNullOrEmpty(entry.ValueLabel))
                    {
                        using (new SKAutoCanvasRestore(canvas))
                        {
                            using (var paint = new SKPaint())
                            {
                                paint.TextSize = LabelTextSize;
                                paint.IsAntialias = true;
                                paint.Color = colors[i];
                                paint.IsStroke = false;
                                paint.Typeface = Typeface;
                                var bounds = sizes[i];
                                var text = texts[i];

                                if (orientation == Orientation.Vertical)
                                {
                                    var y = point.Y;

                                    if (isTop)
                                    {
                                        y -= bounds.Width;
                                    }

                                    canvas.RotateDegrees(90);
                                    canvas.Translate(y, -point.X + (bounds.Height / 2));
                                }
                                else
                                {
                                    if (bounds.Width > itemSize.Width)
                                    {
                                        text = text.Substring(0, Math.Min(3, text.Length));
                                        paint.MeasureText(text, ref bounds);
                                    }

                                    if (bounds.Width > itemSize.Width)
                                    {
                                        text = text.Substring(0, Math.Min(1, text.Length));
                                        paint.MeasureText(text, ref bounds);
                                    }

                                    var y = point.Y;

                                    if (isTop)
                                    {
                                        y -= bounds.Height;
                                    }

                                    canvas.Translate(point.X - (bounds.Width / 2), y);
                                }

                                var rs = new RichString()
                                    .TextDirection(TextDirection)
                                    .Add(text, fontSize: LabelTextSize);

                                rs.Paint(canvas, new TextPaintOptions
                                {
                                    IsAntialias = true,
                                    LcdRenderText = true
                                });
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Shows a Y axis
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="yAxisWidth"></param>
        /// <param name="intervals"></param>
        protected virtual void DrawYAxisText(SKCanvas canvas, IEnumerable<(string Label, SKPoint Point)> intervals)
        {
            var pt = YAxisTextPaint.Clone();
            pt.TextAlign = YAxisPosition == Position.Left ? SKTextAlign.Right : SKTextAlign.Left;

            foreach (var @int in intervals)
                canvas.DrawTextCenteredVertically(@int.Label, pt, @int.Point.X, @int.Point.Y);
        }

        /// <summary>
        /// Draws interval lines
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="intervals"></param>
        protected virtual void DrawYAxisLines(SKCanvas canvas, IEnumerable<SKRect> intervals)
        {
            foreach (var @int in intervals)
            {
                canvas.DrawLine(Margin / 2 + @int.Left, @int.Top, @int.Right - Margin / 2, @int.Bottom, YAxisLinesPaint);
            }
        }

        /// <summary>
        /// Calculates the height of the footer.
        /// </summary>
        /// <returns>The footer height.</returns>
        /// <param name="valueLabelSizes">Value label sizes.</param>
        /// <param name="orientation">orientation of content</param>
        protected float CalculateFooterHeaderHeight(SKRect[] valueLabelSizes, Orientation orientation)
        {
            var result = Margin;

            if (Entries.Any(e => !string.IsNullOrEmpty(e.Label)))
            {
                if(orientation == Orientation.Vertical)
                {
                    var maxValueWidth = valueLabelSizes.Max(x => x.Width);
                    if (maxValueWidth > 0)
                    {
                        result += maxValueWidth + Margin;
                    }
                }
                else
                {
                    result += LabelTextSize + Margin;
                }
            }

            return result;
        }

        /// <summary>
        /// Measures the value labels.
        /// </summary>
        /// <returns>The value labels.</returns>
        protected SKRect[] MeasureLabels(string[] labels, SKPaint paint = null)
        {
            if (paint == null)
            {
                paint = new SKPaint
                {
                    TextSize = LabelTextSize
                };
            }

            return labels.Select(text =>
            {
                if (string.IsNullOrEmpty(text))
                {
                    return SKRect.Empty;
                }

                var bounds = new SKRect();
                paint.MeasureText(text, ref bounds);
                return bounds;
            }).ToArray();
        }

        /// <summary>
        /// Measures the value label.
        /// </summary>
        /// <returns>The value label.</returns>
        protected SKRect MeasureLabel(string label, SKPaint paint = null) => MeasureLabels(new[] { label }, paint).First();
        #endregion
    }
}
