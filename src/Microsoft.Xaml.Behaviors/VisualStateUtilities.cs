// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Microsoft.Xaml.Behaviors
{
    /// <summary>
    /// This class provides various platform agnostic standard operations for working with VisualStateManager.
    /// </summary>
    public static class VisualStateUtilities
    {
        /// <summary>
        /// Transitions the control between two states.
        /// </summary>
        /// <param name="element">The element to transition between states.</param>
        /// <param name="stateName">The state to transition to.</param>
        /// <param name="useTransitions">True to use a System.Windows.VisualTransition to transition between states; otherwise, false.</param>
        /// <returns>True if the control successfully transitioned to the new state; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException">Control is null.</exception>
        /// <exception cref="System.ArgumentNullException">StateName is null.</exception>
        public static bool GoToState(FrameworkElement element, string stateName, bool useTransitions)
        {
            if (string.IsNullOrEmpty(stateName))
            {
                return false;
            }

            if (!(element is Control targetControl))
            {
                return VisualStateManager.GoToElementState(element, stateName, useTransitions);
            }

            targetControl.ApplyTemplate();
            return VisualStateManager.GoToState(targetControl, stateName, useTransitions);
        }

        /// <summary>
        /// Gets the value of the VisualStateManager.VisualStateGroups attached property.
        /// </summary>
        /// <param name="targetObject">The element from which to get the VisualStateManager.VisualStateGroups.</param>
        /// <returns></returns>
        public static IList GetVisualStateGroups(FrameworkElement targetObject)
        {
            IList visualStateGroups = new List<VisualStateGroup>();

            if (targetObject == null)
            {
                return visualStateGroups;
            }

            visualStateGroups = VisualStateManager.GetVisualStateGroups(targetObject);

            if (visualStateGroups != null && visualStateGroups.Count == 0)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(targetObject);
                if (childrenCount > 0)
                {
                    FrameworkElement childElement = VisualTreeHelper.GetChild(targetObject, 0) as FrameworkElement;
                    visualStateGroups =
                        VisualStateManager.GetVisualStateGroups(childElement ?? throw new InvalidOperationException());
                }
            }

            // WPF puts UserControl content in a template, so it won't be the direct visual child. However,
            // the Content element is where the VSGs are expected to be located, so check there.
            if (visualStateGroups == null || visualStateGroups.Count != 0)
            {
                return visualStateGroups;
            }

            UserControl userControl = targetObject as UserControl;
            if (userControl?.Content is FrameworkElement contentElement)
            {
                visualStateGroups = VisualStateManager.GetVisualStateGroups(contentElement);
            }

            return visualStateGroups;
        }

        /// <summary>
        /// Find the nearest parent which contains visual states.
        /// </summary>
        /// <param name="contextElement">The element from which to find the nearest stateful control.</param>
        /// <param name="resolvedControl">The nearest stateful control if True; else null.</param>
        /// <returns>True if a parent contains visual states; else False.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Stateful")]
        public static bool TryFindNearestStatefulControl(FrameworkElement contextElement,
            out FrameworkElement resolvedControl)
        {
            FrameworkElement frameworkElement = contextElement;

            if (frameworkElement == null)
            {
                // TODO: should we throw an exception here? Tracked as spec issue 82282.
                resolvedControl = null;
                return false;
            }

            // Try to find an element which is the immediate child of a UserControl, ControlTemplate or other such "boundary" element
            FrameworkElement parent = frameworkElement.Parent as FrameworkElement;
            bool succesfullyResolved = true;

            // bubble up looking for a place to stop
            while (!HasVisualStateGroupsDefined(frameworkElement) && ShouldContinueTreeWalk(parent))
            {
                frameworkElement = parent;
                parent = parent?.Parent as FrameworkElement;
            }

            if (HasVisualStateGroupsDefined(frameworkElement))
            {
                if (frameworkElement?.TemplatedParent is Control control)
                {
                    // We didn't need to walk the tree to get this because TemplatedParent is set for all elements in the
                    // template.  However, it maintains consistency in our error checking to do it this way.
                    frameworkElement = control;
                } else if (parent is UserControl)
                {
                    // if our parent is a UserControl, then use that
                    frameworkElement = parent;
                }
            } else
            {
                succesfullyResolved = false;
            }

            resolvedControl = frameworkElement;
            return succesfullyResolved;
        }

        private static bool HasVisualStateGroupsDefined(FrameworkElement frameworkElement)
        {
            if (frameworkElement is null)
            {
                return false;
            }

            IList groups = VisualStateManager.GetVisualStateGroups(frameworkElement);
            return groups != null && groups.Count != 0;
        }

        internal static FrameworkElement FindNearestStatefulControl(FrameworkElement contextElement)
        {
            TryFindNearestStatefulControl(contextElement, out FrameworkElement resolvedControl);
            return resolvedControl;
        }

        private static bool ShouldContinueTreeWalk(FrameworkElement element)
        {
            switch (element)
            {
                case null:
                // stop if parent is a UserControl
                case UserControl _:
                    // stop if we can't go any further
                    return false;
                default:
                {
                    if (element.Parent == null)
                    {
                        // stop if parent's parent is null AND parent isn't the template root of a ControlTemplate or DataTemplate
                        FrameworkElement templatedParent = FindTemplatedParent(element);
                        if (templatedParent == null ||
                            (!(templatedParent is Control) && !(templatedParent is ContentPresenter)))
                        {
                            return false;
                        }
                    }

                    break;
                }
            }

            return true;
        }

        private static FrameworkElement FindTemplatedParent(FrameworkElement parent)
        {
            return parent.TemplatedParent as FrameworkElement;
        }
    }
}
