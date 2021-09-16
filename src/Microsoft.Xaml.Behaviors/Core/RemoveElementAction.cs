// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Xaml.Behaviors.Core
{
    /// <summary>
    /// An action that will remove the targeted element from the tree when invoked.
    /// </summary>
    /// <remarks>
    /// This action may fail. The action understands how to remove elements from common parents but not from custom collections or direct manipulation
    /// of the visual tree.
    /// </remarks>
    public class RemoveElementAction : TargetedTriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject == null || this.Target == null)
            {
                return;
            }

            DependencyObject parent = this.Target.Parent;

            switch (parent)
            {
                case Panel panel:
                    panel.Children.Remove(this.Target);
                    return;
                case ContentControl contentControl:
                {
                    if (ReferenceEquals(contentControl.Content, this.Target))
                    {
                        contentControl.Content = null;
                    }

                    return;
                }
                case ItemsControl itemsControl:
                    itemsControl.Items.Remove(this.Target);
                    return;
                case Page page:
                {
                    if (ReferenceEquals(page.Content, this.Target))
                    {
                        page.Content = null;
                    }

                    return;
                }
                case Decorator decorator:
                {
                    if (decorator.Child == this.Target)
                    {
                        decorator.Child = null;
                    }

                    return;
                }
            }

            if (parent != null)
            {
                throw new InvalidOperationException(ExceptionStringTable.UnsupportedRemoveTargetExceptionMessage);
            }
        }
    }
}
