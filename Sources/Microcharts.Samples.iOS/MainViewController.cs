using System;
using CoreGraphics;
using Microcharts.iOS;
using UIKit;

namespace Microcharts.Samples.iOS
{
    public partial class MainViewController : UIViewController
    {
        private const int ChartHeight = 250;
        private const int ChartSpacing = 64;

        public MainViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var charts = Data.CreateQuickstart();

            for(int i = 0; i < charts.Length; i++)
            {
                var chartView = new ChartView()
                {
                    Chart = charts[i],
                    Frame = new CGRect(0,i * (ChartHeight + ChartSpacing) + ChartSpacing, View.Frame.Size.Width, ChartHeight),
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
                };

                _scrollView.AddSubview(chartView);
            }

            _scrollView.ContentSize = new CGSize(View.Frame.Size.Width, charts.Length * (ChartHeight + ChartSpacing) + ChartSpacing);
        }
    }
}

