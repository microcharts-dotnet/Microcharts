using System;

using AppKit;
using Microcharts.macOS;
using System.Linq;

namespace Microcharts.Samples.macOS
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var chartView = new ChartView()
            {
                Chart = Data.CreateQuickstart().ElementAt(1),
                Frame = this.View.Bounds,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable
            };

            this.View.AddSubview(chartView);
        }
    }
}
