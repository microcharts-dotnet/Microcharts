using System;

namespace Microcharts
{
    /// <summary>
    /// to calculate axis numbers
    /// https://stackoverflow.com/questions/8506881/nice-label-algorithm-for-charts-with-minimum-ticks?noredirect=1&lq=1#answer-28284941
    /// </summary>
    public static class NiceScale
    {
        /// <summary>
        /// Calculate axis numbers
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="maxTicks"></param>
        /// <param name="range"></param>
        /// <param name="tickSpacing"></param>
        /// <param name="niceMin"></param>
        /// <param name="niceMax"></param>
        public static void Calculate(double min, double max, int maxTicks, out double range, out double tickSpacing, out double niceMin, out double niceMax)
        {
            range = NiceNum(max - min, false);
            tickSpacing = NiceNum(range / (maxTicks - 1), true);
            niceMin = Math.Floor(min / tickSpacing) * tickSpacing;
            niceMax = Math.Ceiling(max / tickSpacing) * tickSpacing;
        }

        private static double NiceNum(double range, bool round)
        {
            var pow = Math.Pow(10, Math.Floor(Math.Log10(range)));
            var fraction = range / pow;
            double niceFraction;

            if (round)
            {
                if (fraction < 1.5)
                {
                    niceFraction = 1;
                }
                else if (fraction < 3)
                {
                    niceFraction = 2;
                }
                else if (fraction < 7)
                {
                    niceFraction = 5;
                }
                else
                {
                    niceFraction = 10;
                }
            }
            else
            {
                if (fraction <= 1)
                {
                    niceFraction = 1;
                }
                else if (fraction <= 2)
                {
                    niceFraction = 2;
                }
                else if (fraction <= 5)
                {
                    niceFraction = 5;
                }
                else
                {
                    niceFraction = 10;
                }
            }

            return niceFraction * pow;
        }
    }
}
