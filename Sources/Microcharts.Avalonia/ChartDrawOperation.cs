using System;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Microcharts.Avalonia
{
    public class ChartDrawOperation : ICustomDrawOperation
    {
        readonly Func<Chart?> chartGetter;
        readonly Func<Rect> boundsGetter;

        public ChartDrawOperation(Func<Chart?> chartGetter, Func<Rect> boundsGetter)
        {
            this.chartGetter = chartGetter;
            this.boundsGetter = boundsGetter;
        }

        public void Dispose() { }
        public bool HitTest(Point p) => boundsGetter().Contains(p);
        public bool Equals(ICustomDrawOperation other) => this == other;

        public void Render(IDrawingContextImpl context)
        {
            var bounds = boundsGetter();
            var chart = chartGetter();
            var width = (int)bounds.Width;
            var height = (int)bounds.Height;

            var bitmap = new SKBitmap(width, height, false);
            var canvas = new SKCanvas(bitmap);
            canvas.Save();

            chart?.Draw(canvas, width, height);

            if (context is ISkiaDrawingContextImpl skia)
            {
                skia.SkCanvas.DrawBitmap(bitmap, (int)bounds.X, (int)bounds.Y);
            }
        }

        public Rect Bounds => boundsGetter();
    }
}
