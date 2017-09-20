using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkiaSharp;

namespace Microcharts.Samples.Wpf
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var charts = Data.CreateQuickstart();
            this.chart1.Chart = charts[0];
            this.chart2.Chart = charts[1];
            this.chart3.Chart = charts[2];
            this.chart4.Chart = charts[3];
            this.chart5.Chart = charts[4];
            this.chart6.Chart = charts[5];
        }
    }
}
