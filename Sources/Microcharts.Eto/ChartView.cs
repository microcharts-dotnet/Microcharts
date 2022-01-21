
namespace Microcharts.Eto
{
    using global::Eto.SkiaDraw;
    public class ChartView : SkiaDrawable
    {
        Chart? _chart;
        public ChartView()
        {
        }

        public Chart? Chart
        {
            get => _chart;
            set
            {
                _chart = value;
                this.Invalidate();
            }
        }
        protected override void OnPaint(SKPaintEventArgs e)
        {
            if (_chart != null)
                _chart.Draw(e.Surface.Canvas, Width, Height);
            else
                e.Surface.Canvas.Clear();
        }
    }
}
