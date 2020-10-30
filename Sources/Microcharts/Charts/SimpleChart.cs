using System;
using System.Collections.Generic;
using System.Text;

namespace Microcharts
{
    public abstract class SimpleChart : Chart
    {
        public IEnumerable<ChartEntry> Entries
        {
            get => entries;
            set => UpdateEntries(value);
        }
    }
}
