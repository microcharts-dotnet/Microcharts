// Copyright (c) Aloïs DENIEL. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Microcharts
{
    using System;

    /// <summary>
    /// A lightweight proxy instance that will subscribe to a given event with a weak reference to the subscribed target.
    /// If the subscriber is garbage collected, then only this WeakEventHandler will remain subscribed and keeped
    /// in memory instead of the actual subscriber.
    /// This could be considered as an acceptable solution in most cases.
    /// </summary>
    public class InvalidatedWeakEventHandler<TTarget> : IDisposable where TTarget : class
    {
        #region Fields

        private readonly WeakReference<Chart> sourceReference;

        private readonly WeakReference<TTarget> targetReference;

        private readonly Action<TTarget> targetMethod;

        private bool isSubscribed;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="targetMethod">The target method.</param>
        public InvalidatedWeakEventHandler(Chart source, TTarget target, Action<TTarget> targetMethod)
        {
            this.sourceReference = new WeakReference<Chart>(source);
            this.targetReference = new WeakReference<TTarget>(target);
            this.targetMethod = targetMethod;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Microcharts.InvalidateWeakEventHandler`1"/> is alive.
        /// </summary>
        /// <value><c>true</c> if is alive; otherwise, <c>false</c>.</value>
        public bool IsAlive => this.sourceReference.TryGetTarget(out Chart s) && this.targetReference.TryGetTarget(out TTarget t);

        #endregion

        #region Methods

        /// <summary>
        /// Subsribe this handler to the source.
        /// </summary>
        public void Subsribe()
        {
            if (!this.isSubscribed && this.sourceReference.TryGetTarget(out Chart source))
            {
                source.Invalidated += this.OnEvent;
                this.isSubscribed = true;
            }
        }

        /// <summary>
        /// Unsubscribe this handler from the source.
        /// </summary>
        public void Unsubscribe()
        {
            if (this.isSubscribed)
            {
                if (this.sourceReference.TryGetTarget(out Chart source))
                {
                    source.Invalidated -= this.OnEvent;
                }

                this.isSubscribed = false;
            }
        }

        /// <summary>
        /// Releases all resource used by the <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the
        /// <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/>. The <see cref="Dispose"/> method leaves the
        /// <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/> in an unusable state. After calling
        /// <see cref="Dispose"/>, you must release all references to the
        /// <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/> so the garbage collector can reclaim the memory
        /// that the <see cref="T:Microcharts.InvalidatedWeakEventHandler`1"/> was occupying.</remarks>
        public void Dispose() => this.Unsubscribe();

        private void OnEvent(object sender, EventArgs args)
        {
            if (this.targetReference.TryGetTarget(out TTarget t))
            {
                this.targetMethod(t);
            }
            else
            {
                this.Unsubscribe();
            }
        }

        #endregion
    }
}
