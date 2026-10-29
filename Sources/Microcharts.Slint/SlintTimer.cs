// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts.Slint
{
    using System;
    using global::Slint;
    using ITimer = Microcharts.Abstracts.ITimer;

    /// <summary>
    /// Steps chart animations with Slint's timers, on the Slint thread, where the charts may update the UI.
    /// </summary>
    public sealed class SlintTimer : ITimer
    {
        /// <summary>
        /// Makes every chart animate with a <see cref="SlintTimer"/>.
        /// Call it on the Slint thread before creating the charts, since a chart starts its animation when it gets its entries.
        /// </summary>
        public static void Install() => Microcharts.Timer.Create = () => new SlintTimer();

        /// <inheritdoc />
        public void Start(TimeSpan interval, Func<bool> step)
        {
            if (!step())
            {
                return;
            }

            var timer = new Timer();
            timer.Start(TimerMode.Repeated, interval, () =>
            {
                if (!step())
                {
                    timer.Dispose();
                }
            });
        }
    }
}
