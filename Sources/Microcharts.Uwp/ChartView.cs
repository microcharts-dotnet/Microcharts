namespace Microcharts.Uwp
{
    using SkiaSharp.Views.UWP;

    public class ChartView : SKXamlCanvas
    {
        public ChartView()
        {
            this.PaintSurface += OnPaintCanvas;
        }

        private Chart chart;

        public Chart Chart
        {
            get => this.chart;
            set
            {
                if (this.chart != value)
                {
                    this.chart = value;
                    this.Invalidate();
                }
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }
    }
}
