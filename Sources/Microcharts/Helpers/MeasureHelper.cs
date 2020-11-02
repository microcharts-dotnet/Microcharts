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
        }

        /// <summary>
        /// Calculates the height of the header or footer.
        /// </summary>
        /// <returns>The header or footer height.</returns>
        /// <param name="margin">the global margin of chart</param>
        /// <param name="textSize">the text size</param>
        /// <param name="textSizes">text sizes</param>
        /// <param name="orientation">orientation of content</param>
        internal static float CalculateFooterHeaderHeight(float margin, float textSize, SKRect[] textSizes, Orientation orientation, float additionnalValue=0)
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
    }
}
