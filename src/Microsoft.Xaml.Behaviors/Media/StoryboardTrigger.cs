// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Microsoft.Xaml.Behaviors.Media
{
    /// <summary>
    /// An abstract class that provides the ability to target a Storyboard.
    /// </summary>
    /// <remarks>
    /// For Trigger authors, this class provides a standard way to target a Storyboard. Design tools may choose to provide a
    /// special editing experience for classes that inherit from this trigger, thereby improving the designer experience.
    /// </remarks>
    public abstract class StoryboardTrigger : TriggerBase<DependencyObject>
    {
        public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register(nameof(Storyboard),
            typeof(Storyboard), typeof(StoryboardTrigger),
            new FrameworkPropertyMetadata(OnStoryboardChanged));

        /// <summary>
        /// The targeted Storyboard. This is a dependency property.
        /// </summary>
        public Storyboard Storyboard
        {
            get { return (Storyboard)this.GetValue(StoryboardProperty); }
            set { this.SetValue(StoryboardProperty, value); }
        }

        private static void OnStoryboardChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is StoryboardTrigger storyboardTrigger)
            {
                storyboardTrigger.OnStoryboardChanged(args);
            }
        }

        /// <summary>
        /// This method is called when the Storyboard property is changed.
        /// </summary>
        protected virtual void OnStoryboardChanged(DependencyPropertyChangedEventArgs args)
        {
        }
    }

    /// <summary>
    /// A trigger that listens for the completion of a Storyboard.
    /// </summary>
    public class StoryboardCompletedTrigger : StoryboardTrigger
    {
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (this.Storyboard != null)
            {
                this.Storyboard.Completed -= this.Storyboard_Completed;
            }
        }

        protected override void OnStoryboardChanged(DependencyPropertyChangedEventArgs args)
        {
            Storyboard oldStoryboard = args.OldValue as Storyboard;
            Storyboard newStoryboard = args.NewValue as Storyboard;

            if (oldStoryboard == newStoryboard)
            {
                return;
            }

            if (oldStoryboard != null)
            {
                oldStoryboard.Completed -= this.Storyboard_Completed;
            }

            if (newStoryboard != null)
            {
                newStoryboard.Completed += this.Storyboard_Completed;
            }
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.InvokeActions(e);
        }
    }
}
