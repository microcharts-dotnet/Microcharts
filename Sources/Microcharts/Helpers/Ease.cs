// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts
{
    internal static class Ease
    {
        public static float Out(float t) => t * t * t;


        public static float In(float t) => (--t) * t * t + 1;
    }
}
