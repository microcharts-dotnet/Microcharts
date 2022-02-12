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

        public ChartPage(ExampleChartItem chartItem)
        {
            ExampleChartItem = chartItem;
            InitializeComponent();
            Title = ExampleChartItem.ChartType;
            
        }

        private bool Running = true;
        public ExampleChartItem ExampleChartItem { get; }

        protected override void OnDisappearing()
        {
            Running = false;
            base.OnDisappearing();
        }
        bool IsDrawing = false;
        protected void GenerateDynamicData()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            LineChart lc = (LineChart)chartView.Chart;

            int ticks = (int)(250 * TimeSpan.TicksPerMillisecond);

            var series = lc.Series;
            
            int rMax = (int)(lc.MinValue + (lc.MaxValue - lc.MinValue) * 0.66f);
            int rMin = (int)(lc.MinValue + (lc.MaxValue - lc.MinValue) * 0.33f);
            foreach (var s in series)
            {
                int count = s.Entries.Count();
                DelayTimer timer = Timer.Create() as DelayTimer;
                timer.Start(new TimeSpan(ticks), () =>
                {
                    Device.InvokeOnMainThreadAsync(() =>
                    {
                        var label = DateTime.Now.ToString("mm:ss");

                        int idx = 0;
                        foreach (var curSeries in series)
                        {
                            var entries = curSeries.Entries.ToList();

                            if (s == curSeries)
                            {
                                var entry = Data.GenerateTimeSeriesEntry(r, idx, 1);
                                entries.AddRange(entry);

                                if (entries.Count() > count * 1.5) entries.RemoveAt(0);
                            }
                            else
                            {
                                var entry = new ChartEntry(null) { ValueLabel = null, Label = label };
                                entries.Add(entry);
                                if (entries.Count() > count * 1.5) entries.RemoveAt(0);
                            }
                            
                            curSeries.Entries = entries;
                            idx++;
                        }

                        if (!lc.IsAnimating)
                        {
                            lc.IsAnimated = false;
                            lc.Series = series;
                            if (!IsDrawing)
                            {
                                IsDrawing = true;
                                chartView.InvalidateSurface();
                            }
                        }
                    }).ContinueWith(t => {
                        if (t.IsFaulted) Console.WriteLine(t.Exception);
                    });
                    return Running;
                });
                ticks += (int)(250 * TimeSpan.TicksPerMillisecond);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            chartView.Chart = ExampleChartItem.Chart;
            chartView.ChartPainted += (sender, args) =>
            {
                IsDrawing = false;
            };

            if (!chartView.Chart.IsAnimating)
                chartView.Chart.AnimateAsync(true).ConfigureAwait(false);

            if (ExampleChartItem.IsDynamic && (chartView.Chart as LineChart) != null )
            {
                GenerateDynamicData();
            }
        }
    }
}
