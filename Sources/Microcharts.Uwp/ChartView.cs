<<<<<<< HEAD
﻿// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Uwp
{
    using SkiaSharp;
=======
﻿namespace Microcharts.Uwp
{
>>>>>>> dba36a4f5d24d0492b9e0f31cb03b61d460013e9
    using SkiaSharp.Views.UWP;
    using Windows.UI.Xaml;

    public class ChartView : SKXamlCanvas
    {
<<<<<<< HEAD
        #region Constructors

=======
>>>>>>> dba36a4f5d24d0492b9e0f31cb03b61d460013e9
        public ChartView()
        {
            this.PaintSurface += OnPaintCanvas;
        }

<<<<<<< HEAD
        #endregion

        #region Static fields

        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart), typeof(ChartView), typeof(Chart), new PropertyMetadata(null, new PropertyChangedCallback(OnChartChanged)));

        #endregion

        #region Fields

        private InvalidatedWeakEventHandler<ChartView> handler;

        private Chart chart;

        #endregion

        #region Properties
=======
        public static readonly DependencyProperty ChartProperty = DependencyProperty.Register(nameof(Chart), typeof(ChartView), typeof(Chart), new PropertyMetadata(null, new PropertyChangedCallback(OnLabelChanged)));
>>>>>>> dba36a4f5d24d0492b9e0f31cb03b61d460013e9

        public Chart Chart
        {
            get { return (Chart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

<<<<<<< HEAD
        #endregion

        #region Methods

        private static void OnChartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as ChartView;

            if (view.chart != null)
            {
                view.handler.Dispose();
                view.handler = null;
            }

            view.chart = e.NewValue as Chart;
            view.Invalidate();

            if (view.chart != null)
            {
                view.handler = view.chart.ObserveInvalidate(view, (v) => v.Invalidate());
            }
=======

        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as ChartView;
            view.Invalidate();
>>>>>>> dba36a4f5d24d0492b9e0f31cb03b61d460013e9
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
<<<<<<< HEAD
            if (this.chart != null)
            {
                this.chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
            }
            else
            {
                e.Surface.Canvas.Clear(SKColors.Transparent);
            }
        }

        #endregion
=======
            this.Chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }
>>>>>>> dba36a4f5d24d0492b9e0f31cb03b61d460013e9
    }
}
