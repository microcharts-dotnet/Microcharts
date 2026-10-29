// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Slint
{
    using System;
    using global::Slint;
    using SkiaSharp;

    /// <summary>
    /// Draws a chart into images for a Slint <c>Image</c> element, at the element's size in physical pixels.
    /// </summary>
    /// <remarks>
    /// Pass the element's size to <see cref="Resize"/> whenever it changes, and show each image that the renderer
    /// hands to <c>show</c> in the element. The chart's sizes, such as <see cref="Chart.LabelTextSize"/>, are in
    /// logical pixels, like Slint's lengths. Use it on the Slint thread, after <see cref="SlintTimer.Install"/>.
    /// </remarks>
    public sealed class ChartRenderer : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a renderer for an element in <paramref name="window"/>.
        /// </summary>
        /// <param name="window">The window of the element, for its scale factor.</param>
        /// <param name="show">Shows an image in the element, such as by setting the property that its source binds to.</param>
        public ChartRenderer(Window window, Action<Image> show)
        {
            this.window = window ?? throw new ArgumentNullException(nameof(window));
            this.show = show ?? throw new ArgumentNullException(nameof(show));
        }

        #endregion

        #region Fields

        private readonly Window window;

        private readonly Action<Image> show;

        private InvalidatedWeakEventHandler<ChartRenderer> handler;

        private Chart chart;

        private float width, height;

        private SKBitmap bitmap;

        private bool renderScheduled;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the chart to draw.
        /// </summary>
        public Chart Chart
        {
            get => chart;
            set
            {
                handler?.Dispose();
                handler = null;
                chart = value;
                if (chart != null)
                {
                    handler = chart.ObserveInvalidate(this, r => r.Render());
                }

                Render();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the chart at the element's new size, in logical pixels.
        /// </summary>
        /// <remarks>
        /// It draws once the callback that reports the size returns, since the element may still be in creation,
        /// such as in a repeater, and once for a change of both the width and the height.
        /// </remarks>
        public void Resize(float width, float height)
        {
            this.width = width;
            this.height = height;
            if (!renderScheduled)
            {
                renderScheduled = true;
                Timer.SingleShot(TimeSpan.Zero, () =>
                {
                    renderScheduled = false;
                    Render();
                });
            }
        }

        /// <summary>
        /// Draws the chart again, such as after the window's scale factor changed.
        /// </summary>
        public void Render()
        {
            var scale = window.ScaleFactor;
            var pixelWidth = (int)Math.Round(width * scale);
            var pixelHeight = (int)Math.Round(height * scale);
            if (pixelWidth <= 0 || pixelHeight <= 0)
            {
                return;
            }

            if (bitmap == null || bitmap.Width != pixelWidth || bitmap.Height != pixelHeight)
            {
                bitmap?.Dispose();
                bitmap = new SKBitmap(new SKImageInfo(pixelWidth, pixelHeight, SKColorType.Rgba8888, SKAlphaType.Premul));
            }

            using (var canvas = new SKCanvas(bitmap))
            {
                if (chart != null)
                {
                    canvas.Scale(scale);
                    chart.Draw(canvas, (int)Math.Round(pixelWidth / scale), (int)Math.Round(pixelHeight / scale));
                }
                else
                {
                    canvas.Clear(SKColors.Transparent);
                }
            }

            show(Image.FromPremultipliedRgba8(pixelWidth, pixelHeight, bitmap.GetPixelSpan()));
        }

        /// <summary>
        /// Stops following the chart's changes.
        /// </summary>
        public void Dispose()
        {
            handler?.Dispose();
            handler = null;
            bitmap?.Dispose();
            bitmap = null;
        }

        #endregion
    }
}
