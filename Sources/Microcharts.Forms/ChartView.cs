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
                AxisBasedChart axisChart = view.chart as AxisBasedChart;
                if( axisChart != null && axisChart.EnableZoom )
                {
                    //FIXME: how to handle disable zoom after already enabled
                    var pinchGesture = new PinchGestureRecognizer();
                    pinchGesture.PinchUpdated += view.OnPinchUpdated;
                    view.GestureRecognizers.Add(pinchGesture);
                }

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


        double zoomCurScale = 1;
        double zoomStartScale = 1;

        SKPoint zoomStartOrigin = new SKPoint(0, 0);
        SKPoint zoomStartOffset = new SKPoint(0, 0);
        SKPoint zoomTranslation = new SKPoint(0, 0);

        void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            AxisBasedChart axisChart = this.Chart as AxisBasedChart;
            if (axisChart == null) return;

            if (e.Status == GestureStatus.Started)
            {
                // Store the current scale factor applied to the wrapped user interface element,
                // and zero the components for the center point of the translate transform.
                zoomStartScale = zoomCurScale = axisChart.XForm.Scale;
                zoomStartOrigin = new SKPoint( (float)e.ScaleOrigin.X, (float)e.ScaleOrigin.Y);
                zoomStartOffset = axisChart.XForm.Offset;
            }

            if (e.Status == GestureStatus.Running)
            {
                // e.Scale is the delta to be applied for the current frame
                // Calculate the scale factor to be applied.
                zoomCurScale += (e.Scale - 1) * zoomStartScale;
                zoomCurScale = Math.Max(1, zoomCurScale);
                zoomCurScale = Math.Min(5, zoomCurScale);


                SKPoint zoomPanDelta = new SKPoint((float)((e.ScaleOrigin.X - zoomStartOrigin.X) * CanvasSize.Width), (float)((e.ScaleOrigin.Y - zoomStartOrigin.Y) * CanvasSize.Height));

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                double renderedX = X + zoomStartOffset.X;
                double deltaX = renderedX / CanvasSize.Width;
                double deltaWidth = CanvasSize.Width / (CanvasSize.Width * zoomStartScale);
                double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                //Console.WriteLine("ScaleOrigin: {0}, {1}", e.ScaleOrigin.X, e.ScaleOrigin.Y);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                double renderedY = Y + zoomStartOffset.Y;
                double deltaY = renderedY / CanvasSize.Height;
                double deltaHeight = CanvasSize.Height / (CanvasSize.Height * zoomStartScale);
                double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                double targetX = zoomStartOffset.X - (originX * CanvasSize.Width) * (zoomCurScale - zoomStartScale);
                double targetY = zoomStartOffset.Y - (originY * CanvasSize.Height) * (zoomCurScale - zoomStartScale);

                // Apply translation based on the change in origin.
                zoomTranslation.X = (float)targetX;
                zoomTranslation.Y = (float)targetY;


                SKPoint final = zoomTranslation + zoomPanDelta;
                final.X = Math.Min(Math.Max(final.X, -CanvasSize.Width * (float)(zoomCurScale - 1)), 0);
                final.Y = Math.Min(Math.Max(final.Y, -CanvasSize.Height * (float)(zoomCurScale - 1)), 0);
                

                axisChart.XForm.Scale = (float)zoomCurScale;
                axisChart.XForm.Offset = final;
                InvalidateSurface();

            }

            if (e.Status == GestureStatus.Completed)
            {
                InvalidateSurface();
            }
        }

        #endregion
    }
}
