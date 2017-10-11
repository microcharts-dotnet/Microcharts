// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts
{
    using System;

    public static class Timer
    {
        public static Func<ITimer> Create { get; set; } = () => new DelayTimer();

    }
}
