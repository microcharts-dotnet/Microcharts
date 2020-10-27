using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Microcharts.Samples.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : ContentPage
    {
        private ChartItem _chartItem;

        public ChartPage(ChartItem chartItem)
        {
            InitializeComponent();
            Title = chartItem.Name;
            _chartItem = chartItem;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            chartView.Chart = _chartItem.ChartFactory();
        }

        protected override void OnDisappearing()
        {
            chartView.Chart.AnimateAsync(false).ConfigureAwait(false);
            base.OnDisappearing();
        }
    }
}
