using System;
using System.Linq;
using CoreGraphics;
using Microcharts.iOS;
using UIKit;

namespace Microcharts.Samples.iOS
{
    public partial class MainViewController : UIViewController
    {
        public MainViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var chartView = new ChartView
            {
                Frame = new CGRect(0, 0, chart1.Bounds.Width, 172),
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                Chart = Data.CreateQuickstart().ElementAt(2)
            };

            chart1.AddSubview(chartView);
        }
    }
}

