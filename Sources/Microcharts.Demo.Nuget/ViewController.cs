using System;

using UIKit;
using Microcharts.iOS;
using SkiaSharp;

namespace Microcharts.Demo.Nuget
{
        public partial class ViewController : UIViewController
        {
                protected ViewController(IntPtr handle) : base(handle)
                {
                        // Note: this .ctor should not contain any initialization logic.
                }

                public override void ViewDidLoad()
                {
                        base.ViewDidLoad();

                        var entries = new []
                        {
                                new Entry(600)
                                {
                                        Color = SKColor.Parse("#FF0000"),
                                        ValueLabel = "600",
                                        Label="January",
                                },
                                new Entry(400)
                                {
                                        Color = SKColor.Parse("#0000FF"),
                                        ValueLabel = "400",
                                        Label="Février",
                                },
                        };

                        var chart = new RadialGaugeChart()
                        {
                                MaxValue = 1000,
                                Entries = entries,
                        };

                        var chartView = new ChartView()
                        {
                                Chart = chart,
                                Frame = new CoreGraphics.CGRect(0, 0, this.View.Bounds.Width, 150),
                                AutoresizingMask = UIViewAutoresizing.FlexibleDimensions,
                        };

                        this.View.AddSubview(chartView);




                        // Perform any additional setup after loading the view, typically from a nib.
                }

                public override void DidReceiveMemoryWarning()
                {
                        base.DidReceiveMemoryWarning();
                        // Release any cached data, images, etc that aren't in use.
                }
        }
}
