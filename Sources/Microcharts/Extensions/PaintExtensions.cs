using SkiaSharp;

namespace Microcharts
{
    public static class PaintExtensions
    {
        public static SKPaint FillPaintWithColor(SKColor color)
        {
            return new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = color
            };
        }
    }
}
