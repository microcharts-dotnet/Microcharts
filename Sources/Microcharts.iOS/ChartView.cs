namespace Microcharts.iOS
{
	using UIKit;
	using SkiaSharp.Views.iOS;

	public class ChartView : SKCanvasView
	{
		public ChartView()
		{
			this.BackgroundColor = UIColor.Clear;
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
			if (this.chart != null)
			{
				this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
			}
		}
	}
}
