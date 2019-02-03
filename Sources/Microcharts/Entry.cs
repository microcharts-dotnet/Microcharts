namespace Microcharts
{
	using SkiaSharp;

	public class Entry
	{
		public Entry(float value)
		{
			this.Value = value;
		}

		public float Value { get; }

		public string Label { get; set; }

		public string ValueLabel { get; set; }

		public SKColor FillColor { get; set; } = SKColors.Black;

		public SKColor TextColor { get; set; } = SKColors.Gray;
	}
}
