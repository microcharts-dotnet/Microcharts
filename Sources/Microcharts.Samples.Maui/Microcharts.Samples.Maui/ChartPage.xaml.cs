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
    public partial class ChartPage : ContentPage
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
