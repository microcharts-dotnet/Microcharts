using System;
using SkiaSharp;

namespace Microcharts
{
    public class RangeChartEntry : ChartEntry
    {
        public float? StartValue { get; set; }

        public SKColor LowerColor { get; set; } = SKColors.Black;

        public RangeChartEntry(float? startValue, float? endValue) : base(endValue)
        {
            StartValue = startValue;
        }
    }
}
