namespace Microcharts.iOS
{
	using SkiaSharp.Views.iOS;

	public class ChartView : SKCanvasView
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
				if(this.chart != value)
				{
					this.chart = value;
					this.SetNeedsDisplayInRect(this.Bounds);
				}
			}
		}

		private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
		{
			this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
		}
	}
}
