using System;
using CoreGraphics;
using Microcharts.iOS;
using UIKit;

namespace Microcharts.Samples.iOS
{
        public partial class QuickstartViewController : UIViewController
        {
                public QuickstartViewController()
                {

                }

                protected QuickstartViewController(IntPtr handle) : base(handle)
                {
                        // Note: this .ctor should not contain any initialization logic.
                }

                public override void ViewDidLoad()
                {
                        base.ViewDidLoad();

                        this.View.BackgroundColor = UIColor.LightGray;

                        var charts = Data.CreateQuickstart();

                        // Simple
                        var chartView = new ChartView
                        {
                                Frame = new CGRect(0, 32, this.View.Bounds.Width, 140),
                                AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                                Chart = charts[0],
                        };

                        this.View.Add(chartView);

                        //Additional
                        for (int i = 1; i < charts.Length; i++)
                        {
                                this.AddAdditionnalGraph(charts, i);
                        }
                }

                private void AddAdditionnalGraph(Chart[] charts, int i)
                {
                        this.View.Add(new ChartView
                        {
                                Frame = new CGRect(0, 32 + (i * (140 + 12)), this.View.Bounds.Width, 140),
                                AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                                Chart = charts[i],
                        });
                }
        }
}

