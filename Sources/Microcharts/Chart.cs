namespace Microcharts
{
	using System.Collections.Generic;
	using SkiaSharp;

	public abstract class Chart
	{
		public float Margin { get; set; } = 20;

		public float LabelTextSize { get; set; } = 16;

		public SKColor BackgroundColor { get; set; } = SKColors.White;

		public IEnumerable<Entry> Entries { get; set; }

		public void Draw(SKCanvas canvas, int width, int height)
		{
			canvas.Clear(BackgroundColor);

			this.DrawContent(canvas, width, height);
		}

		public abstract void DrawContent(SKCanvas canvas, int width, int height);
	}
}
