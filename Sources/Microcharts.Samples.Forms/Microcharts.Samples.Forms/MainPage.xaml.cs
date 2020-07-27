using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Microcharts.Samples.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var charts = Data.CreateXamarinSample();
            chart1.Chart = charts[0];
            chart2.Chart = charts[1];
            chart3.Chart = charts[2];
            chart4.Chart = charts[3];
            chart5.Chart = charts[4];
            chart6.Chart = charts[5];
        }
    }
}
