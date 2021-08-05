using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Microcharts.Avalonia
{
    public class ChartDrawOperation : ICustomDrawOperation
    {
        public ChartView Parent { get; }

        public ChartDrawOperation(ChartView parent)
        {
            Parent = parent;
        }

        public void Dispose() { }
        public bool HitTest(Point p) => Parent.Bounds.Contains(p);
        public bool Equals(ICustomDrawOperation other) => this == other;

        public void Render(IDrawingContextImpl context)
        {
            if (context is ISkiaDrawingContextImpl skia)
            {
                Parent.Chart?.Draw(skia.SkCanvas, (int)Parent.Bounds.Width, (int)Parent.Bounds.Height);
            }
        }

        public Rect Bounds => Parent.Bounds;
    }
}
