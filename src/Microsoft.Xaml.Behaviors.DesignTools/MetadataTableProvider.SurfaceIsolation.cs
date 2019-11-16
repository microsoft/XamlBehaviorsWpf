// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;

namespace Microsoft.Xaml.Behaviors.DesignTools
{
    partial class MetadataTableProvider
    {
        private void AddAttributes(string typeIdentifier, params Attribute[] attributes)
        {
            _attributeTableBuilder.AddCustomAttributes(typeIdentifier, attributes);
        }

        private void AddAttributes(string typeIdentifier, string propertyName, params Attribute[] attributes)
        {
            _attributeTableBuilder.AddCustomAttributes(typeIdentifier, propertyName, attributes);
        }

        /// <summary>
        /// This class contains the type names required by the new Extensibility APIs.
        /// </summary>
        private static class Targets
        {
            internal const string EventTrigger = "Microsoft.Xaml.Behaviors.EventTrigger";
            internal const string EventTriggerBase = "Microsoft.Xaml.Behaviors.EventTriggerBase";
            internal const string TriggerBase = "Microsoft.Xaml.Behaviors.TriggerBase";
            internal const string TriggerAction = "Microsoft.Xaml.Behaviors.TriggerAction";
            internal const string TargetedTriggerAction = "Microsoft.Xaml.Behaviors.TargetedTriggerAction";
            internal const string ChangePropertyAction = "Microsoft.Xaml.Behaviors.Core.ChangePropertyAction";
            internal const string InvokeCommandAction = "Microsoft.Xaml.Behaviors.InvokeCommandAction";
            internal const string LaunchUriOrFileAction = "Microsoft.Xaml.Behaviors.Core.LaunchUriOrFileAction";
            internal const string MouseDragElementBehavior = "Microsoft.Xaml.Behaviors.Layout.MouseDragElementBehavior";
            internal const string DataStateBehavior = "Microsoft.Xaml.Behaviors.Core.DataStateBehavior";
            internal const string FluidMoveBehavior = "Microsoft.Xaml.Behaviors.Layout.FluidMoveBehavior";
            internal const string FluidMoveBehaviorBase = "Microsoft.Xaml.Behaviors.Layout.FluidMoveBehaviorBase";
            internal const string StoryboardAction = "Microsoft.Xaml.Behaviors.Media.StoryboardAction";
            internal const string ControlStoryboardAction = "Microsoft.Xaml.Behaviors.Media.ControlStoryboardAction";
            internal const string GoToStateAction = "Microsoft.Xaml.Behaviors.Core.GoToStateAction";
            internal const string TranslateZoomRotateBehavior = "Microsoft.Xaml.Behaviors.Input.TranslateZoomRotateBehavior";
            internal const string PlaySoundAction = "Microsoft.Xaml.Behaviors.Media.PlaySoundAction";
            internal const string CallMethodAction = "Microsoft.Xaml.Behaviors.Core.CallMethodAction";
        }
    }
}
