// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using Microsoft.Xaml.Behaviors.Core;
using Microsoft.Xaml.Behaviors.Input;
using Microsoft.Xaml.Behaviors.Layout;
using Microsoft.Xaml.Behaviors.Media;

namespace Microsoft.Xaml.Behaviors.DesignTools
{
    partial class MetadataTableProvider
    {
        private void AddAttribute(Type type, Attribute attribute)
        {
            _attributeTableBuilder.AddCustomAttributes(type, attribute);
        }

        private void AddAttributes(Type type, params Attribute[] attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                AddAttribute(type, attribute);
            }
        }

        private void AddAttribute(Type type, string propertyName, Attribute attribute)
        {
            _attributeTableBuilder.AddCustomAttributes(type, propertyName, attribute);
        }

        private void AddAttributes(Type type, string propertyName, params Attribute[] attributes)
        {
            foreach (Attribute attribute in attributes)
            {
                AddAttribute(type, propertyName, attribute);
            }
        }

        /// <summary>
        /// This class contains the types used by the older Extensibility APIs.
        /// </summary>
        private static class Targets
        {
            internal static readonly Type EventTrigger = typeof(EventTrigger);
            internal static readonly Type EventTriggerBase = typeof(EventTriggerBase);
            internal static readonly Type TriggerBase = typeof(TriggerBase);
            internal static readonly Type TriggerAction = typeof(TriggerAction);
            internal static readonly Type TargetedTriggerAction = typeof(TargetedTriggerAction);
            internal static readonly Type ChangePropertyAction = typeof(ChangePropertyAction);
            internal static readonly Type InvokeCommandAction = typeof(InvokeCommandAction);
            internal static readonly Type LaunchUriOrFileAction = typeof(LaunchUriOrFileAction);
            internal static readonly Type MouseDragElementBehavior = typeof(MouseDragElementBehavior);
            internal static readonly Type DataStateBehavior = typeof(DataStateBehavior);
            internal static readonly Type FluidMoveBehavior = typeof(FluidMoveBehavior);
            internal static readonly Type FluidMoveBehaviorBase = typeof(FluidMoveBehaviorBase);
            internal static readonly Type StoryboardAction = typeof(StoryboardAction);
            internal static readonly Type ControlStoryboardAction = typeof(ControlStoryboardAction);
            internal static readonly Type GoToStateAction = typeof(GoToStateAction);
            internal static readonly Type TranslateZoomRotateBehavior = typeof(TranslateZoomRotateBehavior);
            internal static readonly Type PlaySoundAction = typeof(PlaySoundAction);
            internal static readonly Type CallMethodAction = typeof(CallMethodAction);
        }
    }
}
