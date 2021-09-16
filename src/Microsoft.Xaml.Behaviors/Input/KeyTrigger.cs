// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Xaml.Behaviors.Input
{
    public enum KeyTriggerFiredOn
    {
        KeyDown,
        KeyUp
    }

    /// <summary>
    /// A Trigger that is triggered by a keyboard event.  If the target Key and Modifiers are detected, it fires.
    /// </summary>
    public class KeyTrigger : EventTriggerBase<UIElement>
    {
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register(nameof(Key), typeof(Key), typeof(KeyTrigger));

        public static readonly DependencyProperty ModifiersProperty =
            DependencyProperty.Register(nameof(Modifiers), typeof(ModifierKeys), typeof(KeyTrigger));

        public static readonly DependencyProperty ActiveOnFocusProperty =
            DependencyProperty.Register(nameof(ActiveOnFocus), typeof(bool), typeof(KeyTrigger));

        public static readonly DependencyProperty FiredOnProperty =
            DependencyProperty.Register(nameof(FiredOn), typeof(KeyTriggerFiredOn), typeof(KeyTrigger));

        private UIElement targetElement;

        /// <summary>
        /// The key that must be pressed for the trigger to fire.
        /// </summary>
        public Key Key
        {
            get { return (Key)this.GetValue(KeyProperty); }
            set { this.SetValue(KeyProperty, value); }
        }

        /// <summary>
        /// The modifiers that must be active for the trigger to fire (the default is no modifiers pressed).
        /// </summary>
        public ModifierKeys Modifiers
        {
            get { return (ModifierKeys)this.GetValue(ModifiersProperty); }
            set { this.SetValue(ModifiersProperty, value); }
        }

        /// <summary>
        /// If true, the Trigger only listens to its trigger Source object, which means that element must have focus for the trigger to fire.
        /// If false, the Trigger listens at the root, so any unhandled KeyDown/Up messages will be caught.
        /// </summary>
        public bool ActiveOnFocus
        {
            get { return (bool)this.GetValue(ActiveOnFocusProperty); }
            set { this.SetValue(ActiveOnFocusProperty, value); }
        }

        /// <summary>
        /// Determines whether or not to listen to the KeyDown or KeyUp event.
        /// </summary>
        public KeyTriggerFiredOn FiredOn
        {
            get { return (KeyTriggerFiredOn)this.GetValue(FiredOnProperty); }
            set { this.SetValue(FiredOnProperty, value); }
        }

        protected override string GetEventName()
        {
            return "Loaded";
        }

        private void OnKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == this.Key &&
                Keyboard.Modifiers == GetActualModifiers(e.Key, this.Modifiers))
            {
                this.InvokeActions(e);
            }
        }

        private static ModifierKeys GetActualModifiers(Key key, ModifierKeys modifiers)
        {
            if (key == Key.LeftCtrl || key == Key.RightCtrl)
            {
                modifiers |= ModifierKeys.Control;
            } else if (key == Key.LeftAlt || key == Key.RightAlt || key == Key.System)
            {
                modifiers |= ModifierKeys.Alt;
            } else if (key == Key.LeftShift || key == Key.RightShift)
            {
                modifiers |= ModifierKeys.Shift;
            }

            return modifiers;
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            // Listen to keyboard events.
            this.targetElement = this.ActiveOnFocus ? this.Source : GetRoot(this.Source);

            if (this.FiredOn == KeyTriggerFiredOn.KeyDown)
            {
                this.targetElement.KeyDown += this.OnKeyPress;
            } else
            {
                this.targetElement.KeyUp += this.OnKeyPress;
            }
        }

        protected override void OnDetaching()
        {
            if (this.targetElement != null)
            {
                if (this.FiredOn == KeyTriggerFiredOn.KeyDown)
                {
                    this.targetElement.KeyDown -= this.OnKeyPress;
                } else
                {
                    this.targetElement.KeyUp -= this.OnKeyPress;
                }
            }

            base.OnDetaching();
        }

        private static UIElement GetRoot(DependencyObject current)
        {
            UIElement last = null;

            while (current != null)
            {
                last = current as UIElement;
                current = VisualTreeHelper.GetParent(current);
            }

            return last;
        }
    }
}
