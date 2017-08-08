namespace Microcharts.iOS
{
	using Xamarin.Forms;
	using SkiaSharp.Views.Forms;

	public class ChartView : SKCanvasView
	{
		public ChartView()
		{
			this.BackgroundColor = Color.Transparent;
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
					this.InvalidateSurface();
				}
			}
		}

		private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
		{
			if (this.chart != null)
			{
				this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
			}
		}
	}
}
