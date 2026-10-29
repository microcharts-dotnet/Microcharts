// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Samples.Slint
{
    internal static class Program
    {
        private static void Main()
        {
            using var gallery = Gallery.Create();
            GallerySetup.Connect(gallery);
            // Slint starts the app here, as Info.plist's scene delegate says, and Run doesn't return.
            gallery.Run();
        }
    }
}
