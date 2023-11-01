// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Microcharts.Samples.iOS
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		UIKit.UIScrollView _scrollView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (_scrollView != null) {
				_scrollView.Dispose ();
				_scrollView = null;
			}

		}
	}
}
