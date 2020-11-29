namespace Microcharts
{
    /// <summary>
    /// Define the value label layout option of <see cref="T:Microcharts.LineSeriesChart"/> charts
    /// </summary>
    public enum ValueLabelOption
    {
        /// <summary>
        /// Display value label on top of the chart
        /// </summary>
        /// <remarks>Only work for 1 serie. With multi-series filled, it's TopOfPoint that will be used</remarks>
        TopOfChart,
        /// <summary>
        /// Display value label on top of point
        /// </summary>
        TopOfPoint,
        /// <summary>
        /// Display value label over the point
        /// </summary>
        OverPoint,
        /// <summary>
        /// Not display the value label
        /// </summary>
        None
    }
}
