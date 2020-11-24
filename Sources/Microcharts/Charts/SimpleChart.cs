using System.Collections.Generic;

namespace Microcharts
{
    /// <summary>
    /// Base class of simple chart
    /// </summary>
    public abstract class SimpleChart : Chart
    {
        /// <summary>
        /// Gets or Sets Entries
        /// </summary>
        /// <value>IEnumerable of <seealso cref="T:Microcharts.ChartEntry"/></value>
        public IEnumerable<ChartEntry> Entries
        {
            get => entries;
            set => UpdateEntries(value);
        }
    }
}
