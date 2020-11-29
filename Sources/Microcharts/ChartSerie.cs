using System.Collections.Generic;
using SkiaSharp;

namespace Microcharts
{
    /// <summary>
    /// A serie of data entries for chart
    /// </summary>
    public class ChartSerie
    {
        /// <summary>
        /// Gets or sets the name of the serie
        /// </summary>
        /// <value>Name of the serie</value>
        public string Name { get; set; } = "Default";

        /// <summary>
        /// Gets or sets the color of the fill
        /// </summary>
        /// <value>The color of the fill.</value>
        public SKColor? Color { get; set; } = SKColors.Black;

        /// <summary>
        /// Gets or sets the entries value for the serie 
        /// </summary>
        public IEnumerable<ChartEntry> Entries { get; set; }
    }
}
