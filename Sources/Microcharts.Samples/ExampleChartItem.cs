namespace Microcharts.Samples
{
    public class ExampleChartItem
    {
        public Chart Chart { get; set; }
        public string ChartType { get => Chart?.GetType()?.Name ?? "Unknown"; }
        public string ExampleName { get; set; }
        public string ExampleDescription { get; set; }
    }
}
