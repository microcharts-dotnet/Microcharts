// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Samples.Slint
{
    using System.Linq;
    using global::Slint;
    using Microcharts.Slint;

    /// <summary>
    /// The charts of <see cref="Data.CreateQuickstart"/>, each in a ChartView of the gallery.
    /// </summary>
    public static class GallerySetup
    {
        public static void Connect(Gallery gallery)
        {
            SlintTimer.Install();
            var charts = Data.CreateQuickstart();
            var items = new ListModel<ChartItem>(charts.Select(chart => new ChartItem { Title = chart.GetType().Name }));
            gallery.Charts = items;

            var renderers = charts.Select((chart, index) =>
                new ChartRenderer(gallery.Window, image => items[index] = items[index] with { Chart = image }) { Chart = chart }).ToArray();

            gallery.OnChartResized((index, width, height) => renderers[index].Resize(width, height));
            gallery.OnReplay(() =>
            {
                foreach (var chart in charts)
                {
                    _ = chart.AnimateAsync(entrance: true);
                }
            });
        }
    }
}
