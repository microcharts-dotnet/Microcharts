using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;

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

        public void Render(ImmediateDrawingContext context)
        {
            if (context.TryGetFeature<ISkiaSharpApiLeaseFeature>() is { } leaseFeature)
            {
                using var lease = leaseFeature.Lease();
                var canvas = lease.SkCanvas;
                canvas.Save();
                Parent.Chart?.Draw(canvas, (int)Bounds.Width, (int)Bounds.Height);
                canvas.Restore();
            }
        }

        public Rect Bounds => Parent.Bounds;
    }
}
