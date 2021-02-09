namespace Microcharts
{
    /// <summary>
    /// Define the value label layout option of <see cref="T:Microcharts.AxisBasedChart"/> charts
    /// </summary>
    public enum ValueLabelOption
    {
        /// <summary>
        /// Display value label on top of the chart
        /// </summary>
        /// <remarks>For <see cref="T:Microcharts.LineChart"/> only work for 1 serie. With multi-series filled, it's <seealso cref="TopOfElement"/> that will be used</remarks>
        TopOfChart,
        /// <summary>
        /// Display value label on top of displayed element (e.g Bar or Point)
        /// </summary>
        TopOfElement,
        /// <summary>
        /// Display value label over the displayed element (e.g Bar or Point)
        /// </summary>
        OverElement,
        /// <summary>
        /// Not display the value label
        /// </summary>
        None
    }
}
