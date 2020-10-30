using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microcharts
{
    public abstract class SeriesChart : Chart
    {
        protected IEnumerable<ChartSerie> series;

        public IEnumerable<ChartSerie> Series
        {
            get => series;
            set => UpdateSeries(value);
        }

        protected virtual void UpdateSeries(IEnumerable<ChartSerie> value)
        {
            Set(ref series, value);
            UpdateEntries(series.SelectMany(s => s.Entries).ToList());
        }
    }
}
