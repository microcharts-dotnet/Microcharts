
namespace Microcharts.Samples.Eto
{
    using SkiaSharp;
    public class SampleCharts
    {
        public ChartEntry[] Entries = new ChartEntry[]
        {
            new ChartEntry(200)
            {
                    Label = "January",
                    ValueLabel = "200",
                    Color = SKColor.Parse("#266489")
            },
            new ChartEntry(400)
            {
                    Label = "February",
                    ValueLabel = "400",
                    Color = SKColor.Parse("#68B9C0")
            },
            new ChartEntry(-100)
            {
                    Label = "March",
                    ValueLabel = "-100",
                    Color = SKColor.Parse("#90D585")
            }
        };

        public Chart[] Charts { get; set; }

        public SampleCharts()
        {
            this.Charts = new Chart[]
            {
                new BarChart() {Entries = this.Entries},
                new PointChart() {Entries = this.Entries},
                new LineChart() {Entries = this.Entries},
                new DonutChart() {Entries = this.Entries},
                new RadialGaugeChart() {Entries = this.Entries},
                new RadarChart() {Entries = this.Entries}
            };
        }
    }
}
