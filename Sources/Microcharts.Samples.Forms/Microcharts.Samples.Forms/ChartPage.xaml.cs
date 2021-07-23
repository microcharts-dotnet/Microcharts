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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            chartView.Chart = ExampleChartItem.Chart;
            if(!chartView.Chart.IsAnimating)
                chartView.Chart.AnimateAsync(true).ConfigureAwait(false);

            if (ExampleChartItem.IsDynamic && (chartView.Chart as LineChart) != null )
            {
                Random r = new Random((int)DateTime.Now.Ticks);
                LineChart lc = (LineChart)chartView.Chart;


#if false
/*
                DelayTimer timer = Timer.Create() as DelayTimer;

                int minTicks = (int)(1000 * TimeSpan.TicksPerMillisecond);
                int maxTicks = (int)(2000 * TimeSpan.TicksPerMillisecond);

                timer.Start(new TimeSpan(r.Next(minTicks, maxTicks)), () =>
                {
                    chartView.Chart = null;
                    foreach (var s in lc.Series)
                    {
                        DateTime label = DateTime.Now;
                        var value = r.Next(0, 100);
                        var entry = new ChartEntry(value) { ValueLabel = value.ToString(), Label = label.ToString("mm:ss") };
                        var entries = s.Entries.ToList();
                        entries.Add(entry);

                        if (entries.Count > 15) entries.RemoveAt(0);

                        s.Entries = entries;
                    }

                    chartView.Chart = lc;
                    return Running;
                });
*/

#else
                int ticks = (int)(1000 * TimeSpan.TicksPerMillisecond);

                foreach ( var s in lc.Series )
                {
                    DelayTimer timer = Timer.Create() as DelayTimer;


                    timer.Start(new TimeSpan(ticks), () =>
                    {
                        chartView.Chart = null;

                        DateTime label = DateTime.Now;
                        var value = r.Next(0, 100);
                        var entry = new ChartEntry(value) { ValueLabel = value.ToString(), Label = label.ToString("mm:ss") };
                        var entries = s.Entries.ToList();
                        entries.Add(entry);

                        s.Entries = entries;
                        chartView.Chart = lc;

                        return Running;
                    });

                    ticks += (int)(1000 * TimeSpan.TicksPerMillisecond);
                }
#endif
            }
        }
    }
}
