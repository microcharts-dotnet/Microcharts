using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;

namespace Microcharts
{
    internal enum YPositionBehavior
    {
        None,
        UpToElementHeight,
        UpToElementMiddle,
        DownToElementMiddle
    }

    internal static class DrawHelper
    {
        
        internal static void DrawLabel(SKCanvas canvas, Orientation orientation, YPositionBehavior yPositionBehavior, SKSize itemSize, SKPoint point, SKColor color, SKRect bounds, string text, float textSize, SKTypeface typeface)
        {
            using (new SKAutoCanvasRestore(canvas))
            {
                using (var paint = new SKPaint())
                {
                    paint.TextSize = textSize;
                    paint.IsAntialias = true;
                    paint.Color = color;
                    paint.IsStroke = false;
                    paint.Typeface = typeface;

                    if (orientation == Orientation.Vertical)
                    {
                        var y = point.Y;

                        switch (yPositionBehavior)
                        {
                            case YPositionBehavior.UpToElementHeight:
                                y -= bounds.Width;
                                break;
                            case YPositionBehavior.UpToElementMiddle:
                                y -= bounds.Width / 2;
                                break;
                            case YPositionBehavior.DownToElementMiddle:
                                y += bounds.Width / 2;
                                break;
                            case YPositionBehavior.None:
                            default:
                                break;
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

                        switch (yPositionBehavior)
                        {
                            case YPositionBehavior.UpToElementHeight:
                                y -= bounds.Height;
                                break;
                            case YPositionBehavior.UpToElementMiddle:
                                y -= bounds.Height / 2;
                                break;
                            case YPositionBehavior.DownToElementMiddle:
                                y += bounds.Height / 2;
                                break;
                            case YPositionBehavior.None:
                            default:
                                break;
                        }

                        canvas.Translate(point.X - (bounds.Width / 2), y);
                    }

                    canvas.DrawText(text, 0, 0, paint);
                }
            }
        }

        internal static void DrawYAxis(bool showYAxisText, bool showYAxisLines, Position yAxisPosition, SKPaint yAxisTextPaint, SKPaint yAxisLinesPaint, float margin, float animationProgress, float maxValue, float valueRange, SKCanvas canvas, int width, float yAxisXShift, List<float> yAxisIntervalLabels, float headerHeight, SKSize itemSize, float origin)
        {
            if (showYAxisText || showYAxisLines)
            {
                int cnt = 0;
                var intervals = yAxisIntervalLabels
                    .Select(t => new ValueTuple<string, SKPoint>
                    (
                        t.ToString(),
                        new SKPoint(yAxisPosition == Position.Left ? yAxisXShift : width, MeasureHelper.CalculatePoint(margin, animationProgress, maxValue, valueRange, t, cnt++, itemSize, origin, headerHeight).Y)
                    ))
                    .ToList();

                if (showYAxisText)
                {
                    DrawYAxisText(yAxisTextPaint, yAxisPosition, canvas, intervals);
                }

                if (showYAxisLines)
                {
                    var lines = intervals.Select(tup =>
                    {
                        (_, SKPoint pt) = tup;

                        return yAxisPosition == Position.Right ?
                            SKRect.Create(0, pt.Y, width, 0) :
                            SKRect.Create(yAxisXShift, pt.Y, width, 0);
                    });

                    DrawYAxisLines(margin, yAxisLinesPaint, canvas, lines);
                }
            }
        }

        /// <summary>
        /// Shows a Y axis
        /// </summary>
        /// <param name="yAxisTextPaint"></param>
        /// <param name="yAxisPosition"></param>
        /// <param name="canvas"></param>
        /// <param name="intervals"></param>
        private static void DrawYAxisText(SKPaint yAxisTextPaint, Position yAxisPosition, SKCanvas canvas, IEnumerable<(string Label, SKPoint Point)> intervals)
        {
            var pt = yAxisTextPaint.Clone();
            pt.TextAlign = yAxisPosition == Position.Left ? SKTextAlign.Right : SKTextAlign.Left;

            foreach (var @int in intervals)
                canvas.DrawTextCenteredVertically(@int.Label, pt, @int.Point.X, @int.Point.Y);
        }

        /// <summary>
        /// Draws interval lines
        /// </summary>
        /// <param name="Margin"></param>
        /// <param name="yAxisLinesPaint"></param>
        /// <param name="canvas"></param>
        /// <param name="intervals"></param>
        private static void DrawYAxisLines(float Margin, SKPaint yAxisLinesPaint, SKCanvas canvas, IEnumerable<SKRect> intervals)
        {
            foreach (var @int in intervals)
            {
                canvas.DrawLine(Margin / 2 + @int.Left, @int.Top, @int.Right - Margin / 2, @int.Bottom, yAxisLinesPaint);
            }
        }
    }
}
