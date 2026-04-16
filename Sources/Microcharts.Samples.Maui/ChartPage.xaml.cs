namespace Microcharts.Samples.Maui
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage
    {
        public ChartPage(ExampleChartItem chartItem)
        {
            ExampleChartItem = chartItem;
            InitializeComponent();
            Title = ExampleChartItem.ChartType;
        }

        public ExampleChartItem ExampleChartItem { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            chartView.Chart = ExampleChartItem.Chart;
            if(!chartView.Chart.IsAnimating)
                chartView.Chart.AnimateAsync(true).ConfigureAwait(false);
        }
    }
}
