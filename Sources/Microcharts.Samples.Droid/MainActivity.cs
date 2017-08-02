using Android.App;
using Android.Widget;
using Android.OS;
using Microcharts.Droid;

namespace Microcharts.Samples.Droid
{
	[Activity(Label = "Microcharts", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var charts = Data.CreateQuickstart();

			FindViewById<ChartView>(Resource.Id.chartView1).Chart = charts[0];
			FindViewById<ChartView>(Resource.Id.chartView2).Chart = charts[1];
			FindViewById<ChartView>(Resource.Id.chartView3).Chart = charts[2];
			FindViewById<ChartView>(Resource.Id.chartView4).Chart = charts[3];

		}
	}
}

