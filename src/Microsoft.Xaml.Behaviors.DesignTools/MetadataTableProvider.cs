// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using Microsoft.VisualStudio.DesignTools.Extensibility.Metadata;
using Microsoft.VisualStudio.DesignTools.Extensibility.PropertyEditing;
using Microsoft.Xaml.Behaviors.DesignTools.Properties;
using System;
using System.ComponentModel;

[assembly: ProvideMetadata(typeof(Microsoft.Xaml.Behaviors.DesignTools.MetadataTableProvider))]
namespace Microsoft.Xaml.Behaviors.DesignTools
{
    internal class MetadataTableProvider : IProvideAttributeTable
    {
        private AttributeTableBuilder _attributeTableBuilder;

        // Accessed by the designer to register any design-time metadata.
        public AttributeTable AttributeTable
        {
            get
            {
                if (_attributeTableBuilder == null)
                {
                    _attributeTableBuilder = new AttributeTableBuilder();
                }

                #region EventTrigger
                AddAttributes("Microsoft.Xaml.Behaviors.EventTrigger",
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior));

                AddAttributes("Microsoft.Xaml.Behaviors.EventTrigger", "EventName",
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior_EventName),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion EventTrigger

                #region EventTriggerBase
                AddAttributes("Microsoft.Xaml.Behaviors.EventTriggerBase",
                    new DefaultBindingPropertyAttribute("SourceObject"));

                AddAttributes("Microsoft.Xaml.Behaviors.EventTriggerBase", "SourceObject",
                    new PropertyOrderAttribute(PropertyOrder.CreateBefore(PropertyOrder.Early)),
                    new DescriptionAttribute(Resources.Description_EventTriggerBase_SourceObject));

                AddAttributes("Microsoft.Xaml.Behaviors.EventTriggerBase", "SourceName",
                    new PropertyOrderAttribute(PropertyOrder.CreateBefore(PropertyOrder.Early)),
                    new DescriptionAttribute(Resources.Description_EventTriggerBase_SourceName));
                #endregion EventTriggerBase

                #region TriggerBase
                AddAttributes("Microsoft.Xaml.Behaviors.TriggerBase", "Actions",
                    new BrowsableAttribute(false));
                #endregion TriggerBase

                #region TriggerAction
                AddAttributes("Microsoft.Xaml.Behaviors.TriggerAction", "IsEnabled",
                    new DescriptionAttribute(Resources.Description_TriggerAction_IsEnabled),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));
                #endregion TriggerAction

                #region TargetedTriggerAction
                AddAttributes("Microsoft.Xaml.Behaviors.TargetedTriggerAction",
                    new DefaultBindingPropertyAttribute("TargetObject"));

                AddAttributes("Microsoft.Xaml.Behaviors.TargetedTriggerAction", "TargetObject",
                    new PropertyOrderAttribute(PropertyOrder.CreateBefore(PropertyOrder.Early)),
                    new DescriptionAttribute(Resources.Description_TargetedTriggerAction_TargetObject),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.TargetedTriggerAction", "TargetName",
                    new PropertyOrderAttribute(PropertyOrder.CreateBefore(PropertyOrder.Early)),
                    new DescriptionAttribute(Resources.Description_TargetedTriggerAction_TargetName),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion TargetedTriggerAction

                #region ChangePropertyAction
                AddAttributes("Microsoft.Xaml.Behaviors.Core.ChangePropertyAction", 
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.ChangePropertyAction", "PropertyName",
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_PropertyName));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.ChangePropertyAction", "Duration",
                    new CategoryAttribute(Resources.Category_Animation_Properties),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_Duration));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.ChangePropertyAction", "Increment",
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_Increment));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.ChangePropertyAction", "Value", 
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_Value), 
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion ChangePropertyAction

                #region InvokeCommandAction
                AddAttributes("Microsoft.Xaml.Behaviors.InvokeCommandAction",
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction),
                    new DefaultBindingPropertyAttribute("Command"));

                AddAttributes("Microsoft.Xaml.Behaviors.InvokeCommandAction", "Command",
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_Command),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.InvokeCommandAction", "CommandParameter",
                    new TypeConverterAttribute(typeof(StringConverter)),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_Command),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes("Microsoft.Xaml.Behaviors.InvokeCommandAction", "CommandName",
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_CommandName),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));
                #endregion InvokeCommandAction

                #region InvokeCommandAction
                AddAttributes("Microsoft.Xaml.Behaviors.Core.LaunchUriOrFileAction",
                    new DescriptionAttribute(Resources.Description_LaunchURLOrFileAction));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.LaunchUriOrFileAction", "Path",
                    new DescriptionAttribute(Resources.Description_LaunchURLOrFileAction_Path),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion InvokeCommandAction

                #region MouseDragElementBehavior
                AddAttributes("Microsoft.Xaml.Behaviors.Layout.MouseDragElementBehavior",
                    new DescriptionAttribute(Resources.Description_MouseDragElementBehavior));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.MouseDragElementBehavior", "X",
                    new CanBeEmptyAttribute(true),
                    new DescriptionAttribute(Resources.Description_MouseDragElementBehavior_X),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.MouseDragElementBehavior", "Y",
                    new CanBeEmptyAttribute(true),
                    new DescriptionAttribute(Resources.Description_MouseDragElementBehavior_Y),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.MouseDragElementBehavior", "ConstrainToParentBounds",
                    new DescriptionAttribute(Resources.Description_MouseDragElementBehavior_ConstrainToParentBounds),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion MouseDragElementBehavior

                #region DataStateBehavior
                PropertyOrder order = PropertyOrder.Default;
                AddAttributes("Microsoft.Xaml.Behaviors.Core.DataStateBehavior",
                    new DescriptionAttribute(Resources.Description_DataStateBehavior),
                    new DefaultBindingPropertyAttribute("Binding"));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.DataStateBehavior", "Binding",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_DataStateBehavior_Binding),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.DataStateBehavior", "Value",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_DataStateBehavior_Value),
                    new TypeConverterAttribute(typeof(StringConverter)),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.DataStateBehavior", "TrueState",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_DataStateBehavior_TrueState),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.DataStateBehavior", "FalseState",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_DataStateBehavior_FalseState),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion DataStateBehavior

                #region FluidMoveBehavior
                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehavior",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehavior", "Duration",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_Duration),
                    new CategoryAttribute(Resources.Category_Animation_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehavior", "InitialTag",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_InitialTag),
                    new CategoryAttribute(Resources.Category_Tag_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehavior", "InitialTagPath",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_InitialTagPath),
                    new CategoryAttribute(Resources.Category_Tag_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehavior", "FloatAbove",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_FloatAbove),
                    new CategoryAttribute(Resources.Category_Animation_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehavior", "EaseX",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_EaseX),
                    new CategoryAttribute(Resources.Category_Animation_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehavior", "EaseY",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_EaseY),
                    new CategoryAttribute(Resources.Category_Animation_Properties));
                #endregion FluidMoveBehavior

                #region FluidMoveBehaviorBase
                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehaviorBase", "AppliesTo",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_AppliesTo),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehaviorBase", "IsActive",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_IsActive),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehaviorBase", "Tag",
                    new DescriptionAttribute(Resources.Description_FluidMoveSetTagBehavior_Tag),
                    new CategoryAttribute(Resources.Category_Tag_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Layout.FluidMoveBehaviorBase", "TagPath",
                    new DescriptionAttribute(Resources.Description_FluidMoveSetTagBehavior_TagPath),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
                    new CategoryAttribute(Resources.Category_Tag_Properties));
                #endregion FluidMoveBehaviorBase

                #region StoryboardAction
                AddAttributes("Microsoft.Xaml.Behaviors.Media.StoryboardAction", "Storyboard",
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction_Storyboard),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new TypeConverterAttribute(typeof(TypeConverter)));
                #endregion StoryboardAction

                #region ControlStoryboardAction
                AddAttributes("Microsoft.Xaml.Behaviors.Media.ControlStoryboardAction", 
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction));

                AddAttributes("Microsoft.Xaml.Behaviors.Media.ControlStoryboardAction", "ControlStoryboardOption",
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction_ControlStoryboardOption),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion ControlStoryboardAction

                #region GotoStateAction
                AddAttributes("Microsoft.Xaml.Behaviors.Core.GotoStateAction",
                    new DescriptionAttribute(Resources.Description_GoToStateAction));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.GotoStateAction", "StateName",
                    new DescriptionAttribute(Resources.Description_GoToStateAction_StateName),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.GotoStateAction", "UseTransitions",
                    new DescriptionAttribute(Resources.Description_GoToStateAction_UseTransitions),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion GotoStateAction

                #region TranslateZoomRotateBehavior
                AddAttributes("Microsoft.Xaml.Behaviors.Input.TranslateZoomRotateBehavior",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior));

                AddAttributes("Microsoft.Xaml.Behaviors.Input.TranslateZoomRotateBehavior", "RotationalFriction",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_RotationalFriction),
                    new NumberIncrementsAttribute(0.001, 0.01, 0.1),
                    new NumberRangesAttribute(0, 0, 1, 1, false),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes("Microsoft.Xaml.Behaviors.Input.TranslateZoomRotateBehavior", "TranslateFriction",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_TranslateFriction),
                    new NumberIncrementsAttribute(0.001, 0.01, 0.1),
                    new NumberRangesAttribute(0, 0, 1, 1, false),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes("Microsoft.Xaml.Behaviors.Input.TranslateZoomRotateBehavior", "MinimumScale",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_MinimumScale),
                    new NumberIncrementsAttribute(0.01, 0.1, 1.0),
                    new NumberRangesAttribute(0, 0, null, null, false),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes("Microsoft.Xaml.Behaviors.Input.TranslateZoomRotateBehavior", "MaximumScale",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_MaximumScale),
                    new NumberIncrementsAttribute(0.01, 0.1, 1.0),
                    new NumberRangesAttribute(0, 0, null, null, false),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes("Microsoft.Xaml.Behaviors.Input.TranslateZoomRotateBehavior", "SupportedGestures",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_SupportedGestures),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes("Microsoft.Xaml.Behaviors.Input.TranslateZoomRotateBehavior", "ConstrainToParentBounds",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_ConstrainToParentBounds),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));
                #endregion TranslateZoomRotateBehavior

                #region PlaySoundAction
                AddAttributes("Microsoft.Xaml.Behaviors.Media.PlaySoundAction",
                    new DescriptionAttribute(Resources.Description_PlaySoundAction));

                AddAttributes("Microsoft.Xaml.Behaviors.Media.PlaySoundAction", "Source",
                    new DescriptionAttribute(Resources.Description_PlaySoundAction_Source),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Media.PlaySoundAction", "Volume",
                    new NumberRangesAttribute(0, 0, 1, 1, false),
                    new NumberIncrementsAttribute(0.001, 0.01, 0.1),
                    new DescriptionAttribute(Resources.Description_PlaySoundAction_Volume),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion PlaySoundAction

                #region CallMethodAction
                AddAttributes("Microsoft.Xaml.Behaviors.Core.CallMethodAction",
                    new DescriptionAttribute(Resources.Description_CallMethodAction),
                    new DefaultBindingPropertyAttribute("TargetObject"));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.CallMethodAction", "TargetObject",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_CallMethodAction_TargetObject),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes("Microsoft.Xaml.Behaviors.Core.CallMethodAction", "MethodName",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_CallMethodAction_MethodName),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion CallMethodAction

                return _attributeTableBuilder.CreateTable();
            }
        }

        private void AddAttributes(string typeIdentifier, params Attribute[] attributes)
        {
            _attributeTableBuilder.AddCustomAttributes(typeIdentifier, attributes);
        }

        private void AddAttributes(string typeIdentifier, string propertyName, params Attribute[] attributes)
        {
            _attributeTableBuilder.AddCustomAttributes(typeIdentifier, propertyName, attributes);
        }
    }
}
