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


        float zoomCurScale = 1;
        float zoomStartScale = 1;

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
                float eScale = (float)e.Scale;
                SKPoint eScaleOrigin = new SKPoint((float)e.ScaleOrigin.X, (float)e.ScaleOrigin.Y);
                SKPoint canvasSize = new SKPoint(CanvasSize.Width, CanvasSize.Height);

                // e.Scale is the delta to be applied for the current frame
                // Calculate the scale factor to be applied.
                zoomCurScale += (eScale - 1) * zoomStartScale;
                zoomCurScale = Math.Max(1, zoomCurScale);
                zoomCurScale = Math.Min(5, zoomCurScale);

                SKPoint zoomPanDelta = new SKPoint((eScaleOrigin.X - zoomStartOrigin.X) * CanvasSize.Width, (eScaleOrigin.Y - zoomStartOrigin.Y) * CanvasSize.Height);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the X pixel coordinate.
                float renderedX = (float)X + zoomStartOffset.X;
                float deltaX = renderedX / CanvasSize.Width;
                float deltaWidth = CanvasSize.Width / (CanvasSize.Width * zoomStartScale);
                float originX = (eScaleOrigin.X - deltaX) * deltaWidth;

                //Console.WriteLine("ScaleOrigin: {0}, {1}", e.ScaleOrigin.X, e.ScaleOrigin.Y);

                // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                // so get the Y pixel coordinate.
                float renderedY = (float)Y + zoomStartOffset.Y;
                float deltaY = renderedY / CanvasSize.Height;
                float deltaHeight = CanvasSize.Height / (CanvasSize.Height * zoomStartScale);
                float originY = (eScaleOrigin.Y - deltaY) * deltaHeight;

                // Calculate the transformed element pixel coordinates.
                zoomTranslation.X = zoomStartOffset.X - (originX * CanvasSize.Width) * (zoomCurScale - zoomStartScale);
                zoomTranslation.Y = zoomStartOffset.Y - (originY * CanvasSize.Height) * (zoomCurScale - zoomStartScale);

                // Calculate final translation with pan, and clamp the whole thing
                SKPoint final = zoomTranslation + zoomPanDelta;
                final.X = Math.Min(Math.Max(final.X, -CanvasSize.Width * (zoomCurScale - 1)), 0);
                final.Y = Math.Min(Math.Max(final.Y, -CanvasSize.Height * (zoomCurScale - 1)), 0);
                
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
