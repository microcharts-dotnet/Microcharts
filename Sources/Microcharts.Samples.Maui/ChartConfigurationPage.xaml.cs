using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Microcharts.Samples.Maui
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartConfigurationPage : ContentPage
    {
        public ChartConfigurationPage(string chartType)
        {
            Items = Data.CreateXamarinExampleChartItem(chartType).ToList();
            InitializeComponent();
            Title = chartType;
        }

        public List<ExampleChartItem> Items { get; }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Frame frame = (sender as Frame);
            ExampleChartItem exChartItem = frame.BindingContext as ExampleChartItem;
            Navigation.PushAsync(new ChartPage(exChartItem));
        }
    }
}
