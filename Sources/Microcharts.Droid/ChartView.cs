namespace Microcharts.Droid
{
	using Android.Content;
	using SkiaSharp.Views.Android;
	using Android.Util;
	using System;
	using Android.Runtime;

	public class ChartView : SKCanvasView
	{
		#region Constructors

		public ChartView(Context context) : base(context) 
		{
			this.PaintSurface += OnPaintCanvas;
		}

		public ChartView(Context context, IAttributeSet attributes) : base(context, attributes)
		{
			this.PaintSurface += OnPaintCanvas;
		}

		public ChartView(Context context, IAttributeSet attributes, int defStyleAtt) : base(context, attributes, defStyleAtt)
		{
			this.PaintSurface += OnPaintCanvas;
		}

		public ChartView(IntPtr ptr, JniHandleOwnership jni) : base(ptr, jni)
		{
			this.PaintSurface += OnPaintCanvas;
		}

		#endregion

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
			if(this.chart != null)
			{
				this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
			}
		}
	}
}
