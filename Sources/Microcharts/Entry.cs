// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts
{
    using SkiaSharp;
    using System;

    /// <summary>
    /// A data entry for a chart.
    /// </summary>
    [Obsolete("This class is obsolete as of 0.8.4-pre, please use ChartEntry class instead to avoid conflicts with Xamarin.Forms.Entry.")]
    public class Entry
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.Entry"/> class.
        /// </summary>
        /// <param name="value">The entry value.</param>
        public Entry(float value)
        {
            this.Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public float Value { get; }

        /// <summary>
        /// Gets or sets the caption label.
        /// </summary>
        /// <value>The label.</value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the label associated to the value.
        /// </summary>
        /// <value>The value label.</value>
        public string ValueLabel { get; set; }

        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>The color of the fill.</value>
        public SKColor Color { get; set; } = SKColors.Black;

        /// <summary>
        /// Gets or sets the color of the text (for the caption label).
        /// </summary>
        /// <value>The color of the text.</value>
        public SKColor TextColor { get; set; } = SKColors.Gray;

        #endregion
    }
}
