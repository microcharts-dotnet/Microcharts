// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Microcharts.Samples.iOS
{
        [Register ("ViewController")]
        partial class ViewController
        {
                [Outlet]
                UIKit.UISlider areaAlphaSlider { get; set; }

                [Outlet]
                UIKit.UISwitch hasFewValuesSwitch { get; set; }

                [Outlet]
                UIKit.UISwitch hasLabelsSwitch { get; set; }

                [Outlet]
                UIKit.UISwitch hasNegativeValuesSwitch { get; set; }

                [Outlet]
                UIKit.UISwitch hasPositiveValuesSwitch { get; set; }

                [Outlet]
                UIKit.UISwitch hasSignleColorSwitch { get; set; }

                [Outlet]
                UIKit.UISwitch hasSplinesSwitch { get; set; }

                [Outlet]
                UIKit.UISwitch hasValueLabelsSwitch { get; set; }

                [Outlet]
                UIKit.UISlider holeSlider { get; set; }

                [Outlet]
                UIKit.UISlider lineSizeSlider { get; set; }

                [Outlet]
                UIKit.UISlider pointSizeSlider { get; set; }

                [Outlet]
                UIKit.UIStackView stackView { get; set; }

                [Outlet]
                UIKit.UISlider valuesSlider { get; set; }
                
                void ReleaseDesignerOutlets ()
                {
                        if (hasSplinesSwitch != null) {
                                hasSplinesSwitch.Dispose ();
                                hasSplinesSwitch = null;
                        }

                        if (areaAlphaSlider != null) {
                                areaAlphaSlider.Dispose ();
                                areaAlphaSlider = null;
                        }

                        if (hasFewValuesSwitch != null) {
                                hasFewValuesSwitch.Dispose ();
                                hasFewValuesSwitch = null;
                        }

                        if (hasLabelsSwitch != null) {
                                hasLabelsSwitch.Dispose ();
                                hasLabelsSwitch = null;
                        }

                        if (hasNegativeValuesSwitch != null) {
                                hasNegativeValuesSwitch.Dispose ();
                                hasNegativeValuesSwitch = null;
                        }

                        if (hasPositiveValuesSwitch != null) {
                                hasPositiveValuesSwitch.Dispose ();
                                hasPositiveValuesSwitch = null;
                        }

                        if (hasSignleColorSwitch != null) {
                                hasSignleColorSwitch.Dispose ();
                                hasSignleColorSwitch = null;
                        }

                        if (hasValueLabelsSwitch != null) {
                                hasValueLabelsSwitch.Dispose ();
                                hasValueLabelsSwitch = null;
                        }

                        if (holeSlider != null) {
                                holeSlider.Dispose ();
                                holeSlider = null;
                        }

                        if (lineSizeSlider != null) {
                                lineSizeSlider.Dispose ();
                                lineSizeSlider = null;
                        }

                        if (pointSizeSlider != null) {
                                pointSizeSlider.Dispose ();
                                pointSizeSlider = null;
                        }

                        if (stackView != null) {
                                stackView.Dispose ();
                                stackView = null;
                        }

                        if (valuesSlider != null) {
                                valuesSlider.Dispose ();
                                valuesSlider = null;
                        }
                }
        }
}
