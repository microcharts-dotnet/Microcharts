using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Microcharts.Avalonia
{
    public class ChartView : Control
    {
        public ChartView()
        {
            drawOperation = new ChartDrawOperation(this);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == ChartProperty)
            {
                InvalidateVisual();
            }
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
