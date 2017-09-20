using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;

namespace Microcharts.Wpf
{
    public class ChartView : SKElement, INotifyPropertyChanged
    {
        private Chart _chart;

        public ChartView()
        {
            this.PaintSurface += OnPaintCanvas;
        }

        public Chart Chart
        {
            get { return _chart; }
            set
            {
                _chart = value;
                OnLabelChanged(this, new DependencyPropertyChangedEventArgs());
                NotifyPropertyChanged(nameof(Chart));
            }
        }


        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as ChartView;
            view.InvalidateVisual();
        }

        private void OnPaintCanvas(object sender, SKPaintSurfaceEventArgs e)
        {
            this.Chart.Draw(e.Surface.Canvas, e.Info.Width, e.Info.Height);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

    }
}
