// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.ComponentModel;

#if SurfaceIsolation
using Microsoft.VisualStudio.DesignTools.Extensibility.Metadata;
using Microsoft.VisualStudio.DesignTools.Extensibility.PropertyEditing;
using Editors = Microsoft.VisualStudio.DesignTools.Extensibility.PropertyEditing.Editors;
#else
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using Editors = Microsoft.Windows.Design.PropertyEditing.Editors;
#endif

[assembly: ProvideMetadata(typeof(Microsoft.Xaml.Behaviors.DesignTools.MetadataTableProvider))]
namespace Microsoft.Xaml.Behaviors.DesignTools
{
    internal partial class MetadataTableProvider : IProvideAttributeTable
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
                AddAttributes(Targets.EventTrigger,
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior));

                AddAttributes(Targets.EventTrigger, "EventName",
                    new DescriptionAttribute(Resources.Description_EventTriggerBehavior_EventName),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.EventPickerPropertyValueEditor)));
                #endregion EventTrigger

                #region EventTriggerBase
                AddAttributes(Targets.EventTriggerBase, new DefaultBindingPropertyAttribute("SourceObject"));

                AddAttributes(Targets.EventTriggerBase, "SourceObject",
                    new PropertyOrderAttribute(PropertyOrder.CreateBefore(PropertyOrder.Early)),
                    new DescriptionAttribute(Resources.Description_EventTriggerBase_SourceObject),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.ElementBindingPickerPropertyValueEditor)));

                AddAttributes(Targets.EventTriggerBase, "SourceName",
                    new PropertyOrderAttribute(PropertyOrder.CreateBefore(PropertyOrder.Early)),
                    new DescriptionAttribute(Resources.Description_EventTriggerBase_SourceName),
                    // This is mapped to BehaviorElementPickerPropertyValueEditor in legacy.
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.PropertyPickerPropertyValueEditor)));
                #endregion EventTriggerBase
                
                #region TriggerBase
                AddAttributes(Targets.TriggerBase, "Actions", new BrowsableAttribute(false));
                #endregion TriggerBase

                #region TriggerAction
                AddAttributes(Targets.TriggerAction, "IsEnabled",
                    new DescriptionAttribute(Resources.Description_TriggerAction_IsEnabled),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));
                #endregion TriggerAction

                #region TargetedTriggerAction
                AddAttributes(Targets.TargetedTriggerAction, new DefaultBindingPropertyAttribute("TargetObject"));

                AddAttributes(Targets.TargetedTriggerAction, "TargetObject",
                    new PropertyOrderAttribute(PropertyOrder.CreateBefore(PropertyOrder.Early)),
                    new DescriptionAttribute(Resources.Description_TargetedTriggerAction_TargetObject),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.ElementBindingPickerPropertyValueEditor)));

                AddAttributes(Targets.TargetedTriggerAction, "TargetName",
                    new PropertyOrderAttribute(PropertyOrder.CreateBefore(PropertyOrder.Early)),
                    new DescriptionAttribute(Resources.Description_TargetedTriggerAction_TargetName),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    // This is mapped to BehaviorElementPickerPropertyValueEditor in legacy.
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.PropertyPickerPropertyValueEditor)));
                #endregion TargetedTriggerAction

                #region ChangePropertyAction
                AddAttributes(Targets.ChangePropertyAction, new DescriptionAttribute(Resources.Description_ChangePropertyAction));

                AddAttributes(Targets.ChangePropertyAction, "PropertyName",
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_PropertyName),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.PropertyPickerPropertyValueEditor)));

                AddAttributes(Targets.ChangePropertyAction, "Duration",
                    new CategoryAttribute(Resources.Category_Animation_Properties),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_Duration));
                
                AddAttributes(Targets.ChangePropertyAction, "Increment",
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_Increment));

                AddAttributes(Targets.ChangePropertyAction, "Value", 
                    new DescriptionAttribute(Resources.Description_ChangePropertyAction_Value), 
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion ChangePropertyAction

                #region InvokeCommandAction
                AddAttributes(Targets.InvokeCommandAction,
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction),
                    new DefaultBindingPropertyAttribute("Command"));

                AddAttributes(Targets.InvokeCommandAction, "Command",
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_Command),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.PropertyBindingPickerPropertyValueEditor)));

                AddAttributes(Targets.InvokeCommandAction, "CommandParameter",
                    new TypeConverterAttribute(typeof(StringConverter)),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_Command),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes(Targets.InvokeCommandAction, "CommandName",
                    new DescriptionAttribute(Resources.Description_InvokeCommandAction_CommandName),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));
                #endregion InvokeCommandAction

                #region InvokeCommandAction
                AddAttributes(Targets.LaunchUriOrFileAction,
                    new DescriptionAttribute(Resources.Description_LaunchURLOrFileAction));
                
                AddAttributes(Targets.LaunchUriOrFileAction, "Path",
                    new DescriptionAttribute(Resources.Description_LaunchURLOrFileAction_Path),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion InvokeCommandAction

                #region MouseDragElementBehavior
                AddAttributes(Targets.MouseDragElementBehavior,
                    new DescriptionAttribute(Resources.Description_MouseDragElementBehavior));

                AddAttributes(Targets.MouseDragElementBehavior, "X",
                    new CanBeEmptyAttribute(true),
                    new DescriptionAttribute(Resources.Description_MouseDragElementBehavior_X),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes(Targets.MouseDragElementBehavior, "Y",
                    new CanBeEmptyAttribute(true),
                    new DescriptionAttribute(Resources.Description_MouseDragElementBehavior_Y),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced));

                AddAttributes(Targets.MouseDragElementBehavior, "ConstrainToParentBounds",
                    new DescriptionAttribute(Resources.Description_MouseDragElementBehavior_ConstrainToParentBounds),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion MouseDragElementBehavior

                #region DataStateBehavior
                PropertyOrder order = PropertyOrder.Default;
                AddAttributes(Targets.DataStateBehavior,
                    new DescriptionAttribute(Resources.Description_DataStateBehavior),
                    new DefaultBindingPropertyAttribute("Binding"));
                
                AddAttributes(Targets.DataStateBehavior, "Binding",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_DataStateBehavior_Binding),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.PropertyBindingPickerPropertyValueEditor)));

                AddAttributes(Targets.DataStateBehavior, "Value",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_DataStateBehavior_Value),
                    new TypeConverterAttribute(typeof(StringConverter)),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.DataStateBehavior, "TrueState",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_DataStateBehavior_TrueState),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.DataStateBehavior, "FalseState",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_DataStateBehavior_FalseState),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion DataStateBehavior

                #region FluidMoveBehavior
                AddAttributes(Targets.FluidMoveBehavior,
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior));

                AddAttributes(Targets.FluidMoveBehavior, "Duration",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_Duration),
                    new CategoryAttribute(Resources.Category_Animation_Properties));

                AddAttributes(Targets.FluidMoveBehavior, "InitialTag",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_InitialTag),
                    new CategoryAttribute(Resources.Category_Tag_Properties));

                AddAttributes(Targets.FluidMoveBehavior, "InitialTagPath",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_InitialTagPath),
                    new CategoryAttribute(Resources.Category_Tag_Properties));

                AddAttributes(Targets.FluidMoveBehavior, "FloatAbove",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_FloatAbove),
                    new CategoryAttribute(Resources.Category_Animation_Properties));
                
                AddAttributes(Targets.FluidMoveBehavior, "EaseX",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_EaseX),
                    new CategoryAttribute(Resources.Category_Animation_Properties));

                AddAttributes(Targets.FluidMoveBehavior, "EaseY",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_EaseY),
                    new CategoryAttribute(Resources.Category_Animation_Properties));
                #endregion FluidMoveBehavior

                #region FluidMoveBehaviorBase
                AddAttributes(Targets.FluidMoveBehaviorBase, "AppliesTo",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_AppliesTo),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.FluidMoveBehaviorBase, "IsActive",
                    new DescriptionAttribute(Resources.Description_FluidMoveBehavior_IsActive),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
                    new CategoryAttribute(Resources.Category_Common_Properties));

                AddAttributes(Targets.FluidMoveBehaviorBase, "Tag",
                    new DescriptionAttribute(Resources.Description_FluidMoveSetTagBehavior_Tag),
                    new CategoryAttribute(Resources.Category_Tag_Properties));

                AddAttributes(Targets.FluidMoveBehaviorBase, "TagPath",
                    new DescriptionAttribute(Resources.Description_FluidMoveSetTagBehavior_TagPath),
                    new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
                    new CategoryAttribute(Resources.Category_Tag_Properties));
                #endregion FluidMoveBehaviorBase

                #region StoryboardAction
                AddAttributes(Targets.StoryboardAction, "Storyboard",
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction_Storyboard),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new TypeConverterAttribute(typeof(TypeConverter)),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.StoryboardPickerPropertyValueEditor)));
                #endregion StoryboardAction

                #region ControlStoryboardAction
                AddAttributes(Targets.ControlStoryboardAction, 
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction));

                AddAttributes(Targets.ControlStoryboardAction, "ControlStoryboardOption",
                    new DescriptionAttribute(Resources.Description_ControlStoryboardAction_ControlStoryboardOption),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion ControlStoryboardAction
                
                #region GotoStateAction
                AddAttributes(Targets.GoToStateAction,
                    new DescriptionAttribute(Resources.Description_GoToStateAction));

                AddAttributes(Targets.GoToStateAction, "StateName",
                    new DescriptionAttribute(Resources.Description_GoToStateAction_StateName),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.StatePickerPropertyValueEditor)));

                AddAttributes(Targets.GoToStateAction, "UseTransitions",
                    new DescriptionAttribute(Resources.Description_GoToStateAction_UseTransitions),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion GotoStateAction

                #region TranslateZoomRotateBehavior
                AddAttributes(Targets.TranslateZoomRotateBehavior,
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior));

                AddAttributes(Targets.TranslateZoomRotateBehavior, "RotationalFriction",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_RotationalFriction),
                    new NumberIncrementsAttribute(0.001, 0.01, 0.1),
                    new NumberRangesAttribute(0, 0, 1, 1, false),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes(Targets.TranslateZoomRotateBehavior, "TranslateFriction",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_TranslateFriction),
                    new NumberIncrementsAttribute(0.001, 0.01, 0.1),
                    new NumberRangesAttribute(0, 0, 1, 1, false),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes(Targets.TranslateZoomRotateBehavior, "MinimumScale",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_MinimumScale),
                    new NumberIncrementsAttribute(0.01, 0.1, 1.0),
                    new NumberRangesAttribute(0, 0, null, null, false),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));
                
                AddAttributes(Targets.TranslateZoomRotateBehavior, "MaximumScale",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_MaximumScale),
                    new NumberIncrementsAttribute(0.01, 0.1, 1.0),
                    new NumberRangesAttribute(0, 0, null, null, false),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes(Targets.TranslateZoomRotateBehavior, "SupportedGestures",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_SupportedGestures),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));

                AddAttributes(Targets.TranslateZoomRotateBehavior, "ConstrainToParentBounds",
                    new DescriptionAttribute(Resources.Description_TranslateZoomRotateBehavior_ConstrainToParentBounds),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)));
                #endregion TranslateZoomRotateBehavior

                #region PlaySoundAction
                AddAttributes(Targets.PlaySoundAction,
                    new DescriptionAttribute(Resources.Description_PlaySoundAction));

                AddAttributes(Targets.PlaySoundAction, "Source",
                    new DescriptionAttribute(Resources.Description_PlaySoundAction_Source),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.UriPropertyValueEditor)));

                AddAttributes(Targets.PlaySoundAction, "Volume",
                    new NumberRangesAttribute(0, 0, 1, 1, false),
                    new NumberIncrementsAttribute(0.001, 0.01, 0.1),
                    new DescriptionAttribute(Resources.Description_PlaySoundAction_Volume),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion PlaySoundAction

                #region CallMethodAction
                AddAttributes(Targets.CallMethodAction,
                    new DescriptionAttribute(Resources.Description_CallMethodAction),
                    new DefaultBindingPropertyAttribute("TargetObject"));

                AddAttributes(Targets.CallMethodAction, "TargetObject",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_CallMethodAction_TargetObject),
                    new CategoryAttribute(Resources.Category_Common_Properties),
                    PropertyValueEditor.CreateEditorAttribute(typeof(Editors.ElementBindingPickerPropertyValueEditor)));

                AddAttributes(Targets.CallMethodAction, "MethodName",
                    new PropertyOrderAttribute(order = PropertyOrder.CreateAfter(order)),
                    new DescriptionAttribute(Resources.Description_CallMethodAction_MethodName),
                    new CategoryAttribute(Resources.Category_Common_Properties));
                #endregion CallMethodAction

                return _attributeTableBuilder.CreateTable();
            }
        }
    }
}
