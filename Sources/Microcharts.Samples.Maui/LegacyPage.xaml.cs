using Microcharts.Samples.Maui.Model;

namespace Microcharts.Samples.Maui
{
    public partial class LegacyPage
    {
        public LegacyPage()
        {
            var charts = Data.CreateXamarinLegacySample();
            var items = new List<ChartItem>();
            for (int i = 0; i < charts.Length; i++)
            {
                items.Add(new ChartItem(charts[i].GetType().Name, charts[i], i));
            }
            Items = items;
            InitializeComponent();
        }

        public List<ChartItem> Items { get; }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var border = sender as Border;
            ChartItem chartItem = border.BindingContext as ChartItem;
            Navigation.PushAsync(new ChartConfigurationPage(chartItem.Name));
        }
    }
}
