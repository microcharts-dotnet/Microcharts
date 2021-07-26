using System;

namespace Microcharts.Samples.Maui.Model
{
    public class ChartItem
    {
        public ChartItem(string name, Chart chart, int index)
        {
            Name = name;
            Chart = chart;
            ChartFactory = () => Data.CreateXamarinSample()[index];
            HasSeries = chart is SeriesChart;
        }

        public string Name { get; private set; }
        public Chart Chart { get; }
        public Func<Chart> ChartFactory { get; }
        public bool HasSeries { get; }
    }
}
