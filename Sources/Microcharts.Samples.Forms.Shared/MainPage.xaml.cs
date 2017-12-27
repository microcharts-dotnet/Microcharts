using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microcharts.Samples.Forms
{
    public partial class MainPage : Xamarin.Forms.ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public static IEnumerable<Entry> GenerateEntries()
        {
            var values = Enumerable.Range(0, 12).Select(x => random.Next(-1000, 1000)).ToArray();
            return new Entry[]
            {
                new Entry(values[0]) { ValueLabel = values[0].ToString(), Label = "January", Color = SKColor.Parse("#266489") },
                new Entry(values[1]) { ValueLabel = values[1].ToString(),Label = "February", Color = SKColor.Parse("#68B9C0") },
                new Entry(values[2]) { ValueLabel = values[2].ToString(),Label = "March", Color = SKColor.Parse("#90D585") },
                new Entry(values[3]) { ValueLabel = values[3].ToString(),Label = "April", Color = SKColor.Parse("#F3C151")},
                new Entry(values[4]) { ValueLabel = values[4].ToString(),Label = "May", Color = SKColor.Parse("#F37F64")},
                new Entry(values[5]) { ValueLabel = values[5].ToString(),Label = "June", Color = SKColor.Parse("#424856") },
                new Entry(values[6]) { ValueLabel = values[6].ToString(),Label = "July", Color = SKColor.Parse("#8F97A4")},
                new Entry(values[7]) { ValueLabel = values[7].ToString(),Label = "August", Color = SKColor.Parse("#DAC096") },
                new Entry(values[8]) { ValueLabel = values[8].ToString(),Label = "September", Color = SKColor.Parse("#76846E") },
                new Entry(values[9]) { ValueLabel = values[9].ToString(),Label = "October", Color = SKColor.Parse("#A65B69") },
                new Entry(values[10]) { ValueLabel = values[10].ToString(),Label = "November", Color = SKColor.Parse("#DABFAF") },
                new Entry(values[11]) { ValueLabel = values[11].ToString(),Label = "December", Color = SKColor.Parse("#97A69D") },
            };
        }

        private static Random random = new Random();

        private int chartType = 0;

        private Type[] ChartTypes =
        {
            typeof(BarChart),
            typeof(PointChart),
            typeof(LineChart),
            typeof(DonutChart),
            typeof(PieChart),
            typeof(RadarChart),
            typeof(RadialGaugeChart),
        };

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ChangeChart(null, null);
        }

        private void GenerateData(object sender, EventArgs e)
        {
            if (this.chart.Chart != null)
            {
                this.chart.Chart.Entries = GenerateEntries();
            }
        }

        private void ChangeChart(object sender, EventArgs e)
        {
            chartType = (chartType + 1) % ChartTypes.Length;
            var type = this.ChartTypes[chartType];
            this.chart.Chart = Activator.CreateInstance(type) as Chart;
            this.chart.Chart.MinValue = -1000;
            this.chart.Chart.MaxValue = 1000;
            GenerateData(null, null);
        }

        private void ChangeFont(object sender, EventArgs e)
        {

            chartType = (chartType + 1) % ChartTypes.Length;
            var type = this.ChartTypes[chartType];
            this.chart.Chart = Activator.CreateInstance(type) as Chart;
            this.chart.Chart.MinValue = -1000;
            this.chart.Chart.MaxValue = 1000;

            Random r = new Random();
            int rInt = r.Next(0, 3);
            var fontName = string.Empty;
            switch (rInt)
            {
                case 0:
                    fontName = "Avenir";
                    break;
                case 1:
                    fontName = "Noteworthy";
                    break;
                case 2:
                    fontName = "Helvetica";
                    break;
                default:
                    fontName = string.Empty; // test if empty
                    break;
            }
            this.chart.Chart.Typeface = SKTypeface.FromFamilyName(fontName);
            GenerateData(null, null);
        }
    }
}
