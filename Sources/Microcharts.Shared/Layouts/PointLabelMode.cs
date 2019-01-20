// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts
{
	public enum PointLabelMode
	{
        /// <summary>
        /// All labels below the chart are horizontal. Label can be cut if there is not enough horizontal space for the entire text.
        /// </summary>
		Horizontal,

        /// <summary>
        /// All labels below the chart are vertical.
        /// </summary>
		Vertical,

        /// <summary>
        /// All labels below the chart are horizontal if there is enough horizontal space for the entire text. Otherwise - labels are vertical.
        /// </summary>
		PreferHorizontal,
	}
}
