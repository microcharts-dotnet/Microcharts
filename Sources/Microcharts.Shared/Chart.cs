namespace Microcharts
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using SkiaSharp;

	public abstract class Chart
	{
		#region Fields

		protected float? minValue, maxValue;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the global margin.
		/// </summary>
		/// <value>The margin.</value>
		public float Margin { get; set; } = 20;

		/// <summary>
		/// Gets or sets the text size of the labels.
		/// </summary>
		/// <value>The size of the label text.</value>
		public float LabelTextSize { get; set; } = 16;

		/// <summary>
		/// Gets or sets the color of the chart background.
		/// </summary>
		/// <value>The color of the background.</value>
		public SKColor BackgroundColor { get; set; } = SKColors.White;

		/// <summary>
		/// Gets or sets the data entries.
		/// </summary>
		/// <value>The entries.</value>
		public IEnumerable<Entry> Entries { get; set; }

		/// <summary>
		/// Gets or sets the minimum value from entries. If not defined, it will be the minimum between zero and the 
		/// minimal entry value.
		/// </summary>
		/// <value>The minimum value.</value>
		public float MinValue
		{
			get
			{
				if (!this.Entries.Any()) return 0;
				if (this.minValue == null) return Math.Min(0, this.Entries.Min(x => x.Value));
				return Math.Min(this.minValue.Value, this.Entries.Min(x => x.Value));
			}
			set => this.minValue = value;
		}

		/// <summary>
		/// Gets or sets the maximum value from entries. If not defined, it will be the maximum between zero and the 
		/// maximum entry value.
		/// </summary>
		/// <value>The minimum value.</value>
		public float MaxValue
		{
			get
			{
				if (!this.Entries.Any()) return 0;
				if (this.maxValue == null) return Math.Max(0, this.Entries.Max(x => x.Value));
				return Math.Max(this.maxValue.Value, this.Entries.Max(x => x.Value));
			}
			set => this.maxValue = value;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Draw the  graph onto the specified canvas.
		/// </summary>
		/// <returns>The draw.</returns>
		/// <param name="canvas">Canvas.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public void Draw(SKCanvas canvas, int width, int height)
		{
			canvas.Clear(BackgroundColor);

			this.DrawContent(canvas, width, height);
		}

		/// <summary>
		/// Draws caption elements on the right or left side of the chart.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="entries">Entries.</param>
		/// <param name="isLeft">If set to <c>true</c> is left.</param>
		protected void DrawCaptionElements(SKCanvas canvas, int width, int height, List<Entry> entries, bool isLeft)
		{
			var margin = 2 * this.Margin;
			var availableHeight = height - 2 * margin;
			var x = isLeft ? this.Margin : (width - this.Margin - this.LabelTextSize);
			var ySpace = (availableHeight - this.LabelTextSize) / ((entries.Count <= 1) ? 1 : entries.Count - 1);

			for (int i = 0; i < entries.Count; i++)
			{
				var entry = entries.ElementAt(i);
				var y = margin + i * ySpace;
				if (entries.Count <= 1) y += (availableHeight - this.LabelTextSize) / 2;

				var hasLabel = !string.IsNullOrEmpty(entry.Label);
				var hasValueLabel = !string.IsNullOrEmpty(entry.ValueLabel);

				if (hasLabel || hasValueLabel)
				{
					var hasOffset = hasLabel && hasValueLabel;
					var captionMargin = LabelTextSize * 0.60f;
					var space = hasOffset ? captionMargin : 0;

					using (var paint = new SKPaint
					{
						Style = SKPaintStyle.Fill,
						Color = entry.Color,
					})
					{
						var rect = SKRect.Create(x, y, this.LabelTextSize, this.LabelTextSize);
						canvas.DrawRect(rect, paint);
					}

					y += LabelTextSize / 2;

					if (hasLabel)
					{
						using (var paint = new SKPaint()
						{
							TextSize = this.LabelTextSize,
							IsAntialias = true,
							Color = entry.TextColor,
							IsStroke = false
						})
						{
							var bounds = new SKRect();
							var text = entry.Label;
							paint.MeasureText(text, ref bounds);
							var textX = isLeft ? this.Margin + captionMargin + this.LabelTextSize : width - this.Margin - captionMargin - this.LabelTextSize - bounds.Width;
							var textY = y - (bounds.Top + bounds.Bottom) / 2 - space;

							canvas.DrawText(text, textX, textY, paint);
						}
					}

					if (hasValueLabel)
					{
						using (var paint = new SKPaint()
						{
							TextSize = this.LabelTextSize,
							IsAntialias = true,
							FakeBoldText = true,
							Color = entry.Color,
							IsStroke = false
						})
						{
							var bounds = new SKRect();
							var text = entry.ValueLabel;
							paint.MeasureText(text, ref bounds);
							var textX = isLeft ? this.Margin + captionMargin + this.LabelTextSize : width - this.Margin - captionMargin - this.LabelTextSize - bounds.Width;
							var textY = y - (bounds.Top + bounds.Bottom) / 2 + space;
							canvas.DrawText(text, textX, textY, paint);
						}
					}
				}
			}
		}

		/// <summary>
		/// Draws the chart content.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public abstract void DrawContent(SKCanvas canvas, int width, int height);

		#endregion
	}
}
