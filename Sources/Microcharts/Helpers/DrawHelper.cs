using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace Microcharts
{
    internal static class DrawHelper
    {
        internal static void DrawLabel(SKCanvas canvas, Orientation orientation, bool isTop, SKSize itemSize, SKPoint point, SKColor color, SKRect bounds, string text, float textSize, SKTypeface typeface)
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

                    canvas.DrawText(text, 0, 0, paint);
                }
            }
        }
    }
}
