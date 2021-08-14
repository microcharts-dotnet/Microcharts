using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace Microcharts.Avalonia
{
    public class ChartView : Control
    {
        public ChartView()
        {
            this.GetObservable(ChartProperty).Subscribe(_ => InvalidateVisual());
            drawOperation = new ChartDrawOperation(this);
        }

        readonly ChartDrawOperation drawOperation;
        Chart? chart;

        public Chart? Chart
        {
            get => chart;
            set => SetAndRaise(ChartProperty, ref chart, value);
        }

        public static readonly DirectProperty<ChartView, Chart?> ChartProperty =
            AvaloniaProperty.RegisterDirect<ChartView, Chart?>(
                nameof(Chart),
                c => c.Chart,
                (c, v) => c.Chart = v);

        public override void Render(DrawingContext context)
        {
            context.Custom(drawOperation);
        }
    }
}
