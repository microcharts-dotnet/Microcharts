
namespace Microcharts.Samples.Eto
{
    using global::Eto.Forms;
    using global::Eto.Drawing;
    using Microcharts;
    public partial class MainForm : Form
    {
        Chart[] charts = new SampleCharts().Charts;
        public MainForm()
        {
            InitializeComponent();

            var layout1 = new TableLayout() { Spacing = Size.Empty + 4 };
            layout1.Rows.Add(new TableRow(cell(0), cell(1), cell(2)) { ScaleHeight = true });
            layout1.Rows.Add(new TableRow(cell(3), cell(4), cell(5)) { ScaleHeight = true });

            var entries = new ChartEntry[]
                {
                    new ChartEntry(200) { Label = "January", ValueLabel = "200", Color = SkiaSharp.SKColors.CornflowerBlue },
                    new ChartEntry(400) { Label = "February", ValueLabel = "400", Color = SkiaSharp.SKColors.ForestGreen },
                    new ChartEntry(-100) { Label = "March", ValueLabel = "-100", Color = SkiaSharp.SKColors.MediumVioletRed }
                };
            this.Content = new Microcharts.Eto.ChartView() { Chart = new RadarChart { Entries = entries, AnimationProgress = 100 } };

            Content = layout1;
        }

        private TableCell cell(int idx)
        {
            var view = new Microcharts.Eto.ChartView { Chart = charts[idx] };

            view.Chart.AnimationProgress = 100;

            return new TableCell(view, true);
        }
    }
}
