using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SkiaSharp;

namespace Microcharts.Samples.Avalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
#if DEBUG
            this.AttachDevTools();
#endif
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        static ChartEntry[] CreateTestEntries() =>
            new ChartEntry[]
            {
                new(200)
                {
                    Label = "January",
                    ValueLabel = "200",
                    Color = SKColor.Parse("#266489")
                },
                new(400)
                {
                    Label = "February",
                    ValueLabel = "400",
                    Color = SKColor.Parse("#68B9C0")
                },
                new(-100)
                {
                    Label = "March",
                    ValueLabel = "-100",
                    Color = SKColor.Parse("#90D585")
                }
            };

        public Chart[] Charts { get; set; } = {
            new BarChart { Entries = CreateTestEntries() },
            new PointChart { Entries = CreateTestEntries() },
            new LineChart { Entries = CreateTestEntries() },
            new DonutChart { Entries = CreateTestEntries() },
            new RadialGaugeChart { Entries = CreateTestEntries() },
            new RadarChart { Entries = CreateTestEntries() }
        };
    }
}
