using System;
using System.Collections.Generic;
using System.Text;

namespace Microcharts
{
    interface IBarChart
    {

        /// <summary>
        /// Gets or sets the bar background area alpha.
        /// </summary>
        /// <value>The bar area alpha.</value>
        byte BarAreaAlpha { get; set; }

        /// <summary>
        /// Get or sets the minimum height for a bar
        /// </summary>
        /// <value>The minium height of a bar.</value>
        float MinBarHeight { get; set; }

        /// <summary>
        /// Gets or sets the text orientation of labels.
        /// </summary>
        /// <value>The label orientation.</value>
        Orientation LabelOrientation { get; set; }

        /// <summary>
        /// Gets or sets the text orientation of value labels.
        /// </summary>
        /// <value>The label orientation.</value>
        Orientation ValueLabelOrientation { get; set; }
    }
}
