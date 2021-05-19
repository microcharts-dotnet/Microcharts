// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using SkiaSharp;

namespace Microcharts
{
    internal static class CanvasExtensions
    {
        public static void DrawCaptionLabels(this SKCanvas canvas, string label, SKColor labelColor, string value, SKColor valueColor, float textSize, SKPoint point, SKTextAlign horizontalAlignment, SKTypeface typeface, out SKRect totalBounds)
        {
            var hasLabel = !string.IsNullOrEmpty(label);
            var hasValueLabel = !string.IsNullOrEmpty(value);

            totalBounds = new SKRect();

            if (hasLabel || hasValueLabel)
            {
                var hasOffset = hasLabel && hasValueLabel;
                var captionMargin = textSize * 0.60f;
                var space = hasOffset ? captionMargin : 0;

                if (hasLabel)
                {
                    using (var paint = new SKPaint
                    {
                        TextSize = textSize,
                        IsAntialias = true,
                        Color = labelColor,
                        IsStroke = false,
                        TextAlign = horizontalAlignment,
                        Typeface = typeface
                    })
                    {
                        var bounds = new SKRect();
                        var text = label;
                        paint.MeasureText(text, ref bounds);

                        var y = point.Y - ((bounds.Top + bounds.Bottom) / 2) - space;

                        canvas.DrawText(text, point.X, y, paint);

                        var labelBounds = GetAbsolutePositionRect(point.X, y, bounds, horizontalAlignment);
                        totalBounds = labelBounds.Standardized;
                    }
                }

                if (hasValueLabel)
                {
                    using (var paint = new SKPaint()
                    {
                        TextSize = textSize,
                        IsAntialias = true,
                        FakeBoldText = true,
                        Color = valueColor,
                        IsStroke = false,
                        TextAlign = horizontalAlignment,
                        Typeface = typeface
                    })
                    {
                        var bounds = new SKRect();
                        var text = value;
                        paint.MeasureText(text, ref bounds);

                        var y = point.Y - ((bounds.Top + bounds.Bottom) / 2) + space;

                        canvas.DrawText(text, point.X, y, paint);

                        var valueBounds = GetAbsolutePositionRect(point.X, y, bounds, horizontalAlignment);

                        if (totalBounds.IsEmpty)
                        {
                            totalBounds = valueBounds.Standardized;
                        }
                        else
                        {
                            totalBounds.Union(valueBounds.Standardized);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws the given point.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="point">The point.</param>
        /// <param name="color">The fill color.</param>
        /// <param name="size">The point size.</param>
        /// <param name="mode">The point mode.</param>
        public static void DrawPoint(this SKCanvas canvas, SKPoint point, SKColor color, float size, PointMode mode)
        {
            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                IsAntialias = true,
                Color = color,
            })
            {
                switch (mode)
                {
                    case PointMode.Square:
                        canvas.DrawRect(SKRect.Create(point.X - (size / 2), point.Y - (size / 2), size, size), paint);
                        break;

                    case PointMode.Circle:
                        paint.IsAntialias = true;
                        canvas.DrawCircle(point.X, point.Y, size / 2, paint);
                        break;
                }
            }
        }

        /// <summary>
        /// Draws a line with a gradient stroke.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="startPoint">The starting point.</param>
        /// <param name="startColor">The starting color.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="endColor">The end color.</param>
        /// <param name="size">The stroke size.</param>
        public static void DrawGradientLine(this SKCanvas canvas, SKPoint startPoint, SKColor startColor, SKPoint endPoint, SKColor endColor, float size)
        {
            using (var shader = SKShader.CreateLinearGradient(startPoint, endPoint, new[] { startColor, endColor }, null, SKShaderTileMode.Clamp))
            {
                using (var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = size,
                    Shader = shader,
                    IsAntialias = true,
                })
                {
                    canvas.DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, paint);
                }
            }
        }

        /// <summary>
        /// Draws text vertically aligned
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="text">The text to display</param>
        /// <param name="paint">The paint to use for text and calculations</param>
        /// <param name="point">The baseLine point where to vertically draw</param>
        /// <remarks>https://stackoverflow.com/questions/27631736/meaning-of-top-ascent-baseline-descent-bottom-and-leading-in-androids-font</remarks>
        public static void DrawTextCenteredVertically(this SKCanvas canvas, string text, SKPaint paint, SKPoint point)
        {
            canvas.DrawTextCenteredVertically(text, paint, point.X, point.Y);
        }

        /// <summary>
        /// Draws text vertically aligned
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="text">The text to display</param>
        /// <param name="paint">The paint to use for text and calculations</param>
        /// <param name="x">The baseLine point x where to vertically draw</param>
        /// <param name="y">The baseLine point y where to vertically draw</param>
        /// <remarks>https://stackoverflow.com/questions/27631736/meaning-of-top-ascent-baseline-descent-bottom-and-leading-in-androids-font</remarks>
        public static void DrawTextCenteredVertically(this SKCanvas canvas, string text, SKPaint paint, float x, float y)
        {
            var textY = y + (((-paint.FontMetrics.Ascent + paint.FontMetrics.Descent) / 2) - paint.FontMetrics.Descent);
            canvas.DrawText(text, x, textY, paint);
        }

        /// <summary>
        /// Gets the absolute bounds of a given rectangle, aligned at a given position.
        /// </summary>
        /// <param name="x">The absolute x position.</param>
        /// <param name="y">The absolute y position.</param>
        /// <param name="bounds">The bounds of the rectangle.</param>
        /// <param name="horizontalAlignment">The alignment of the rectangle, relative to x/y.</param>
        /// <returns></returns>
        private static SKRect GetAbsolutePositionRect(float x, float y, SKRect bounds, SKTextAlign horizontalAlignment)
        {
            var captionBounds = new SKRect
            {
                Left = x + bounds.Left,
                Top = y + bounds.Top
            };

            switch (horizontalAlignment)
            {
                case SKTextAlign.Left:
                    captionBounds.Right = captionBounds.Left + bounds.Width;
                    break;
                case SKTextAlign.Center:
                    captionBounds.Right = captionBounds.Left + bounds.Width / 2;
                    break;
                case SKTextAlign.Right:
                    captionBounds.Right = captionBounds.Left - bounds.Width;
                    break;
            }

            captionBounds.Bottom = captionBounds.Top + bounds.Height;

            return captionBounds;
        }
    }
}
