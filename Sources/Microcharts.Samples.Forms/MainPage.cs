using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Microcharts.Samples.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
                InitializeComponent();
        }
        
        public static readonly IEnumerable<Microcharts.Entry> GenerateEntries() => new Microcharts.Entry[]
       {
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "January", Color = SKColor.Parse("#266489") },
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "February", Color = SKColor.Parse("#68B9C0") },
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "March", Color = SKColor.Parse("#90D585") },
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "April", Color = SKColor.Parse("#F3C151")},
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "May", Color = SKColor.Parse("#F37F64")},
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "June", Color = SKColor.Parse("#424856") },
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "July", Color = SKColor.Parse("#8F97A4")},
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "August", Color = SKColor.Parse("#DAC096") },
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "September", Color = SKColor.Parse("#76846E") },
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "October", Color = SKColor.Parse("#A65B69") },
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "November", Color = SKColor.Parse("#DABFAF") },
            new Microcharts.Entry(random.Next(-1000, 1000)) { Label = "December", Color = SKColor.Parse("#97A69D") },
        };

        private static Random random = new Random();

        private static float[] GenerateData()
        {
            return Enumerable.Range(0, 12).Select(x => (float)random.Next(-1000, 1000)).ToArray();
        }

        private float[] data = GenerateData();

        private int chartType = 0;

        private Type[] ChartTypes =
        {
            typeof(PointChart),
            typeof(RadialGaugeChart),
        };

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ChangeChart(null, null);
        }

        private void GenerateData(object sender, System.EventArgs e)
        {
            if(this.chart.Chart != null)
            {
                this.chart.Chart.Entries = GenerateEntries();
            }
        }

        private void ChangeChart(object sender, System.EventArgs e)
        {
            chartType = (chartType + 1) % ChartTypes.Length;
            var type = this.ChartTypes[chartType];
            this.chart.Chart = Activator.CreateInstance(type) as Chart;
            GenerateData(null, null);
        }
    }
}
