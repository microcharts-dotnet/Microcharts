// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Samples.Slint
{
    using global::Slint.Android;

    [Activity(Label = "Microcharts on Slint", MainLauncher = true, Exported = true, ConfigurationChanges = SlintActivity.ConfigurationChanges)]
    [MetaData(SlintActivity.LibraryNameKey, Value = SlintActivity.LibraryName)]
    public class MainActivity : SlintActivity
    {
        // Runs on the Slint thread, like every callback of the components.
        protected override void SlintMain()
        {
            using var gallery = Gallery.Create();
            GallerySetup.Connect(gallery);
            gallery.Run();
        }
    }
}
