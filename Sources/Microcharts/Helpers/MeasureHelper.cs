using System.Collections.Generic;
using System.Linq;
using SkiaSharp;

namespace Microcharts
{
    internal static class MeasureHelper
    {
        /// <summary>
        /// Measures the text values.
        /// </summary>
        /// <returns>The texts bounds.</returns>
        internal static SKRect[] MeasureTexts(string[] texts, float textSize)
        {
            using (var paint = new SKPaint())
            {
                paint.TextSize = textSize;
                return MeasureTexts(texts, paint);
            }
        }

        /// <summary>
        /// Measures the text values.
        /// </summary>
        /// <returns>The texts bounds.</returns>
        internal static SKRect[] MeasureTexts(string[] texts, SKPaint paint)
        {
            return texts.Select(text =>
            {
                if (string.IsNullOrEmpty(text))
                {
                    return SKRect.Empty;
                }

                var bounds = new SKRect();
                paint.MeasureText(text, ref bounds);
                return bounds;
            }).ToArray();
        }

        /// <summary>
        /// Calculates the height of the header or footer.
        /// </summary>
        /// <returns>The header or footer height.</returns>
        /// <param name="margin">the global margin of chart</param>
        /// <param name="textSize">the text size</param>
        /// <param name="textSizes">text sizes</param>
        /// <param name="orientation">orientation of content</param>
        internal static float CalculateFooterHeaderHeight(float margin, float textSize, SKRect[] textSizes, Orientation orientation)
        {
            var result = margin;
            if (textSizes.Any(l => !l.IsEmpty))
            {
                if (orientation == Orientation.Vertical)
                {
                    var maxValueWidth = textSizes.Max(x => x.Width);
                    if (maxValueWidth > 0)
                    {
                        result += maxValueWidth + margin;
                    }
                }
                else
                {
                    result += textSize + margin;
                }
            }

            return result;
        }

        internal static int CalculateYAxis(bool showYAxisText, bool showYAxisLines, IEnumerable<ChartEntry> entries, int yAxisMaxTicks, SKPaint yAxisTextPaint, Position yAxisPosition, int width, bool fixedRange, ref float maxValue, ref float minValue, out float yAxisXShift, out List<float> yAxisIntervalLabels)
        {
            yAxisXShift = 0.0f;
            yAxisIntervalLabels = new List<float>();
            if (showYAxisText || showYAxisLines)
            {
                var yAxisWidth = width;
                double range, niceMin, niceMax, tickSpacing;
                int ticks;

                if (!fixedRange)
                {
                    //var enumerable = entries.ToList(); // to avoid double enumeration
                    if (minValue == maxValue)
                    {
                        if (minValue >= 0)
                            maxValue += 100;
                        else
                            maxValue = 0;
                    }

                    NiceScale.Calculate(minValue, maxValue, yAxisMaxTicks, out range, out tickSpacing, out niceMin, out niceMax);
                    ticks = (int)(range / tickSpacing);
                }
                else
                {
                    niceMin = minValue;
                    niceMax = maxValue;
                    range = niceMax - niceMin;
                    tickSpacing = range / (yAxisMaxTicks-1);
                    ticks = yAxisMaxTicks;
                }

                yAxisIntervalLabels = Enumerable.Range(0, ticks)
                    .Select(i => (float)(niceMax - (i * tickSpacing)))
                    .ToList();

                var longestYAxisLabel = yAxisIntervalLabels.Aggregate(string.Empty, (max, cur) => max.Length > cur.ToString().Length ? max : cur.ToString());
                var longestYAxisLabelWidth = MeasureHelper.MeasureTexts(new string[] { longestYAxisLabel }, yAxisTextPaint).Select(b => b.Width).FirstOrDefault();
                yAxisWidth = (int)(width - longestYAxisLabelWidth);
                if (yAxisPosition == Position.Left)
                {
                    yAxisXShift = longestYAxisLabelWidth;
                }

                // to reduce chart width
                width = yAxisWidth;
                maxValue = (float)niceMax;
                minValue = (float)niceMin;
            }

            return width;
        }

        internal static SKPoint CalculatePoint(float margin, float animationProgress, float maxValue, float valueRange, float value, int i, SKSize itemSize, float origin, float headerHeight, float originX = 0)
        {
            var x = originX + margin + (itemSize.Width / 2) + (i * (itemSize.Width + margin));
            var y = headerHeight + ((1 - animationProgress) * (origin - headerHeight) + (((maxValue - value) / valueRange) * itemSize.Height) * animationProgress);

            return new SKPoint(x, y);
        }
    }
}
