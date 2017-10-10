namespace Microcharts.Uwp
{
    using SkiaSharp.Views.UWP;
    using Windows.UI.Xaml;

    public class ChartView : SKXamlCanvas
    {
        #region Constructors

        public ChartView()
        {
            this.PaintSurface += OnPaintCanvas;
        }

        #endregion

        #region Static fields

        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart), typeof(ChartView), typeof(Chart), new PropertyMetadata(null, new PropertyChangedCallback(OnChartChanged)));

        #endregion

        #region Fields

        private InvalidatedWeakEventHandler<ChartView> handler;

        private Chart chart;

        #endregion

        #region Properties

        public Chart Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        #endregion

        #region Methods

        private static void OnChartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as ChartView;

            if (view.chart != null)
            {
                handler.Dispose();
                view.handler = null;
            }

            view.chart = value;
            view.Invalidate();

            if (view.chart != null)
            {
                view.handler = this.chart.ObserveInvalidate(view, (v) => v.Invalidate());
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            this.Chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }

        #endregion
    }
}
