// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Samples.Slint
{
    using System;

    internal static class Program
    {
        // Windows needs a single-threaded apartment for the window.
        [STAThread]
        private static void Main()
        {
            using var gallery = Gallery.Create();
            GallerySetup.Connect(gallery);
            gallery.Run();
        }
    }
}
