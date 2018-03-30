﻿namespace Rollbar.Common
{
    using System;
    using Rollbar.Utils;
    using Xamarin.iOS.Foundation;

    /// <summary>
    /// An abstract base for implementing IReconfigurable types.
    /// </summary>
    /// <typeparam name="T">A type that supports its reconfiguration.</typeparam>
    /// <seealso cref="Rollbar.Common.IReconfigurable{T}" />
    [Preserve]
    public abstract class ReconfigurableBase<T>
        : IReconfigurable<T>
        where T : ReconfigurableBase<T>
    {
        /// <summary>
        /// Reconfigures this object similar to the specified one.
        /// </summary>
        /// <param name="likeMe">The pre-configured instance to be cloned in terms of its configuration/settings.</param>
        /// <returns>
        /// Reconfigured instance.
        /// </returns>
        public virtual T Reconfigure(T likeMe)
        {
            var properties = 
                ReflectionUtil.GetAllPublicInstanceProperties(this.GetType());

            foreach(var property in properties)
            {
                property.SetValue(this, property.GetValue(likeMe));
            }

            OnReconfigured(new EventArgs());

            return (T) this;
        }

        /// <summary>
        /// Occurs when this instance reconfigured.
        /// </summary>
        public event EventHandler Reconfigured;

        /// <summary>
        /// Raises the <see cref="E:Reconfigured" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnReconfigured(EventArgs e)
        {
            EventHandler handler = Reconfigured;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }



    /// <summary>
    /// An abstract base for implementing IReconfigurable (based on a base type) types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TBase">The type of the base.</typeparam>
    /// <seealso cref="Rollbar.Common.IReconfigurable{T}" />
    public abstract class ReconfigurableBase<T, TBase>
        : IReconfigurable<T, TBase>
        where T : ReconfigurableBase<T, TBase>, TBase
    {

        /// <summary>
        /// Reconfigures this object similar to the specified one.
        /// </summary>
        /// <param name="likeMe">The pre-configured instance to be cloned in terms of its configuration/settings.</param>
        /// <returns>
        /// Reconfigured instance.
        /// </returns>
        public virtual T Reconfigure(TBase likeMe)
        {
            var properties =
                ReflectionUtil.GetAllPublicInstanceProperties(likeMe.GetType());

            foreach (var property in properties)
            {
                property.SetValue(this, property.GetValue(likeMe));
            }

            OnReconfigured(new EventArgs());

            return (T)this;
        }

        /// <summary>
        /// Occurs when this instance reconfigured.
        /// </summary>
        public event EventHandler Reconfigured;

        /// <summary>
        /// Raises the <see cref="E:Reconfigured" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnReconfigured(EventArgs e)
        {
            EventHandler handler = Reconfigured;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
