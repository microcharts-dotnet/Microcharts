using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using Microcharts.Droid;

namespace Microcharts.Samples.Droid
{
    [Activity(Label = "Microcharts", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var charts = Data.UnicodeTest();

            FindViewById<ChartView>(Resource.Id.chartView1).Chart = charts[0];
            FindViewById<ChartView>(Resource.Id.chartView2).Chart = charts[1];
            FindViewById<ChartView>(Resource.Id.chartView3).Chart = charts[2];
            FindViewById<ChartView>(Resource.Id.chartView4).Chart = charts[3];
        }
    }
}

