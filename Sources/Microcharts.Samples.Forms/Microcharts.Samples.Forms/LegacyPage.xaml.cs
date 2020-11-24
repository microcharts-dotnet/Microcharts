using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts.Samples.Forms.Model;
using Xamarin.Forms;

namespace Microcharts.Samples.Forms
{
    public partial class LegacyPage : ContentPage
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public List<ChartItem> Items { get; }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Frame frame = (sender as Frame);
            ChartItem chartItem = frame.BindingContext as ChartItem;
            Navigation.PushAsync(new ChartConfigurationPage(chartItem.Name));
        }
    }
}
