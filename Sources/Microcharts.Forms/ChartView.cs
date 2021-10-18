// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Forms
{
    using Xamarin.Forms;
    using SkiaSharp.Views.Forms;
    using SkiaSharp;
    using System;
    using Xamarin.Forms.Internals;

    public class ChartView : SKCanvasView
    {
        #region Constructors

        public ChartView()
        {
            this.BackgroundColor = Color.Transparent;
            this.PaintSurface += OnPaintCanvas;

            var pinchGesture = new PinchGestureRecognizer();
            pinchGesture.PinchUpdated += OnPinchUpdated;
            GestureRecognizers.Add(pinchGesture);
        }

        public event EventHandler<SKPaintSurfaceEventArgs> ChartPainted;

        #endregion

        #region Static fields

        public static readonly BindableProperty ChartProperty = BindableProperty.Create(nameof(Chart), typeof(Chart), typeof(ChartView), null, propertyChanged: OnChartChanged);

        #endregion

        #region Fields

        private InvalidatedWeakEventHandler<ChartView> handler;

        private Chart chart;

        #endregion

        #region Properties

        public Chart Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        #endregion

        #region Methods

        private static void OnChartChanged(BindableObject d, object oldValue, object value)
        {
            var view = d as ChartView;

            if (view.chart != null)
            {
                view.handler.Dispose();
                view.handler = null;
            }

            view.chart = value as Chart;
            view.InvalidateSurface();

            if (view.chart != null)
            {
                view.handler = view.chart.ObserveInvalidate(view, (v) => v.InvalidateSurface());
            }
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            if (this.chart != null)
            {
                this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
            else
            {
                e.Surface.Canvas.Clear(SKColors.Transparent);
            }

            ChartPainted?.Invoke(sender, e);
        }


        float currentScale = 1;
        float startScale = 1;
        SKPoint offsetPosition = new SKPoint(0, 0);
        SKPoint translation = new SKPoint(0, 0);

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            AxisBasedChart axisChart = this.Chart as AxisBasedChart;
            if (axisChart == null) return;

            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                startScale = currentScale;
            }
            if (e.Status == GestureStatus.Running)
            {
                // Calculate the scale factor to be applied.
                currentScale += (float)((e.Scale - 1) * startScale);
                currentScale = Math.Max(1, currentScale);
                currentScale = Math.Min(3, currentScale);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = X + offsetPosition.X;
                double deltaX = renderedX / CanvasSize.Width;
                double deltaWidth = CanvasSize.Width / (CanvasSize.Width * startScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Y + offsetPosition.Y;
                double deltaY = renderedY / CanvasSize.Height;
                double deltaHeight = CanvasSize.Height / (CanvasSize.Height * startScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = offsetPosition.X - (originX * CanvasSize.Width) * (currentScale - startScale);
                double targetY = offsetPosition.Y - (originY * CanvasSize.Height) * (currentScale - startScale);

                // Apply translation based on the change in origin.
                translation.X = (float)targetX.Clamp(-CanvasSize.Width * (currentScale - 1), 0);
                translation.Y = (float)targetY.Clamp(-CanvasSize.Height * (currentScale - 1), 0);

                Console.WriteLine("{0}, {1}", translation.X, translation.Y);

                axisChart.XForm.Scale = currentScale;
                axisChart.XForm.Offset = translation;
                InvalidateSurface();

            }
            if (e.Status == GestureStatus.Completed)
            {
                // Store the translation delta's of the wrapped user interface element.
                offsetPosition = translation;

                axisChart.XForm.Scale = currentScale;
                axisChart.XForm.Offset = translation;
                InvalidateSurface();
            }
        }

        #endregion
    }
}
