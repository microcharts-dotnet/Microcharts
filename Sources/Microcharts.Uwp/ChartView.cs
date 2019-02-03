namespace Microcharts.Uwp
{
    using SkiaSharp.Views.UWP;
    using Windows.UI.Xaml;

    public class ChartView : SKXamlCanvas
    {
        public ChartView()
        {
            this.PaintSurface += OnPaintCanvas;
        }

        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart), typeof(ChartView), typeof(Chart), new PropertyMetadata(null, new PropertyChangedCallback(OnLabelChanged)));

        public Chart Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }


        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as ChartView;
            view.Invalidate();
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            this.Chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }
    }
}
