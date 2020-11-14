using System.Collections.Generic;
using SkiaSharp;

namespace Microcharts
{
    public class ChartSerie
    {
        public ChartSerie()
        {
        }

        public string Name { get; set; } = "Default";

        public SKColor? Color { get; set; } = SKColors.Black;

        public IEnumerable<ChartEntry> Entries { get; set; }
    }
}
