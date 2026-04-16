using Microcharts.Droid;

namespace Microcharts.Samples.Android
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            var charts = Data.CreateQuickstart();

            FindViewById<ChartView>(Resource.Id.chartView1).Chart = charts[0];
            FindViewById<ChartView>(Resource.Id.chartView2).Chart = charts[1];
            FindViewById<ChartView>(Resource.Id.chartView3).Chart = charts[2];
            FindViewById<ChartView>(Resource.Id.chartView4).Chart = charts[3];
        }
    }
}
