// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Microsoft.Xaml.Behaviors
{
    /// <summary>
    /// A trigger that listens for a specified event on its source and fires when that event is fired.
    /// </summary>
    public class EventTrigger : EventTriggerBase<object>
    {
        /// <summary>
        ///     DependencyProperty for <see cref="EventName" />
        /// </summary>
        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register(nameof(EventName),
            typeof(string),
            typeof(EventTrigger),
            new FrameworkPropertyMetadata(
                "Loaded",
                OnEventNameChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTrigger"/> class.
        /// </summary>
        public EventTrigger()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTrigger"/> class.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        public EventTrigger(string eventName)
        {
            this.EventName = eventName;
        }

        /// <summary>
        /// Gets or sets the name of the event to listen for. This is a dependency property.
        /// </summary>
        /// <value>The name of the event.</value>
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public string EventName
        {
            get { return (string)this.GetValue(EventNameProperty); }
            set { this.SetValue(EventNameProperty, value); }
        }

        /// <summary>
        ///     Accessor method for <see cref="EventName" />
        /// </summary>
        /// <returns>The value of <see cref="EventName" /></returns>
        protected override string GetEventName()
        {
            return this.EventName;
        }

        private static void OnEventNameChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            ((EventTrigger)sender).OnEventNameChanged((string)args.OldValue, (string)args.NewValue);
        }
    }
}
