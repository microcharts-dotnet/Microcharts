using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using Microcharts.Droid;

namespace Microcharts.Samples.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
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
            FindViewById<ChartView>(Resource.Id.chartView5).Chart = charts[4];
            FindViewById<ChartView>(Resource.Id.chartView6).Chart = charts[5];
            FindViewById<ChartView>(Resource.Id.chartView7).Chart = charts[6];
            FindViewById<ChartView>(Resource.Id.chartView8).Chart = charts[7];
        }
    }
}
