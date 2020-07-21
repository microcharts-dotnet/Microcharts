// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Microcharts.Samples.iOS
{
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView chart1 { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (chart1 != null) {
                chart1.Dispose ();
                chart1 = null;
            }
        }
    }
}