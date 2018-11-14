// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Behaviors;
    using Microsoft.Xaml.Behaviors.Core;

    public class VisualStateManagerStub : VisualStateManager
    {
        public string LastStateName
        {
            get;
            private set;
        }

        public bool? LastUseTransitions
        {
            get;
            private set;
        }

        protected override bool GoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
        {
            this.LastStateName = stateName;
            this.LastUseTransitions = useTransitions;
            return base.GoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions);
        }
    }

    public static class VisualStateHelper
    {
        public const string DefaultTrueStateName = "TrueState";
        public const string DefaultFalseStateName = "FalseState";
        public const string ArbitraryThirdStateName = "ThirdState";

        public static T CreateObjectWithStates<T>() where T : DependencyObject, new()
        {
            T obj = new T();
            ObservableCollection<VisualStateGroup> visualStateGroups = (ObservableCollection<VisualStateGroup>)obj.GetValue(VisualStateManager.VisualStateGroupsProperty);
            VisualState trueState = CreateVisualState(DefaultTrueStateName);
            VisualState falseState = CreateVisualState(DefaultFalseStateName);
            VisualState thirdState = CreateVisualState(ArbitraryThirdStateName);
            VisualStateGroup visualStateGroup = CreateVisualStateGroup(trueState, falseState, thirdState);

            visualStateGroups.Add(visualStateGroup);
            return obj;
        }

        public static VisualState CreateVisualState(string stateName)
        {
            return new VisualState()
            {
                Name = stateName,
            };
        }

        public static VisualStateManagerStub CreateVSMStub()
        {
            return new VisualStateManagerStub();
        }

        public static VisualStateGroup CreateVisualStateGroup(params VisualState[] states)
        {
            VisualStateGroup group = new VisualStateGroup();
            foreach (VisualState state in states)
            {
                group.States.Add(state);
            }
            return group;
        }

        public static VisualStateManagerStub AttachCustomVSM(FrameworkElement element)
        {
            VisualStateManagerStub vsmStub = CreateVSMStub();
            element.SetValue(VisualStateManager.CustomVisualStateManagerProperty, vsmStub);
            return vsmStub;
        }
    }

    [TestClass]
    public sealed class DataStateBehaviorTest
    {
        #region Setup/teardown

        [TestInitialize]
        public void Setup()
        {
            Interaction.ShouldRunInDesignMode = true;
        }

        [TestCleanup]
        public void Teardown()
        {
            Interaction.ShouldRunInDesignMode = false;
        }

        #endregion

        #region Helper methods

        private static void AttachBehavior(Behavior behavior, DependencyObject dependencyObject)
        {
            BehaviorCollection behaviors = Interaction.GetBehaviors(dependencyObject);
            behaviors.Add(behavior);
        }

        #endregion

        #region Factory methods

        private const string ConditionMatchValue = "Matched";

        private DataStateBehavior CreateEmptyDataStateBehavior()
        {
            return new DataStateBehavior();
        }

        private DataStateBehavior CreateDefaultDataStateBehavior()
        {
            DataStateBehavior dataStateBehavior = new DataStateBehavior()
            {
                TrueState = VisualStateHelper.DefaultTrueStateName,
                FalseState = VisualStateHelper.DefaultFalseStateName,
            };
            return dataStateBehavior;
        }

        private DataStateBehavior CreateDataStateBehaviorInTrueState()
        {
            DataStateBehavior dataStateBehavior = CreateDefaultDataStateBehavior();
            dataStateBehavior.Binding = ConditionMatchValue;
            dataStateBehavior.Value = ConditionMatchValue;
            return dataStateBehavior;
        }

        private DataStateBehavior CreateDataStateBehaviorInFalseState()
        {
            DataStateBehavior dataStateBehavior = CreateDefaultDataStateBehavior();
            dataStateBehavior.Binding = CreateObject();
            dataStateBehavior.Value = CreateObject();
            return dataStateBehavior;
        }

        private static object CreateObject()
        {
            return new object();
        }

        #endregion

        #region Test methods

        [TestMethod]
        public void Attach_NoStateNamesSet_DoesNothing()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateEmptyDataStateBehavior();
            AttachBehavior(dataStateBehavior, stateGrid);
            // Attaching an empty DataStateBehavior should not cause any problems.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Attach_TrueStateDoesNotExist_ThrowsArgumentException()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateEmptyDataStateBehavior();
            dataStateBehavior.TrueState = "NonExistantState";
            using (new StubWindow(stateGrid))
            {
                AttachBehavior(dataStateBehavior, stateGrid);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Attach_FalseStateDoesNotExist_ThrowsArgumentException()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateEmptyDataStateBehavior();
            dataStateBehavior.FalseState = "NonExistantState";
            using (new StubWindow(stateGrid))
            {
                AttachBehavior(dataStateBehavior, stateGrid);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FalseStateChangesToInvalidValue_ThrowsArgumentException()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateEmptyDataStateBehavior();
            AttachBehavior(dataStateBehavior, stateGrid);
            dataStateBehavior.FalseState = "FalseState";
            dataStateBehavior.FalseState = "NonExistantState";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TrueStateChangesToInvalidValue_ThrowsArgumentException()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateEmptyDataStateBehavior();
            AttachBehavior(dataStateBehavior, stateGrid);
            dataStateBehavior.TrueState = "TrueState";
            dataStateBehavior.TrueState = "NonExistantState";
        }

        [TestMethod]
        public void TrueStateChanges_TransitionsToNewTrueState()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateDataStateBehaviorInTrueState();
            VisualStateManagerStub vsmStub = VisualStateHelper.AttachCustomVSM(stateGrid);
            AttachBehavior(dataStateBehavior, stateGrid);

            dataStateBehavior.TrueState = VisualStateHelper.ArbitraryThirdStateName;
            Assert.AreEqual(vsmStub.LastStateName, VisualStateHelper.ArbitraryThirdStateName, "Change in TrueState should have caused a transition to the new state, as Binding matches Value.");
        }

        [TestMethod]
        public void FalseStateChanges_TransitionsToNewFalseState()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateDataStateBehaviorInFalseState();
            VisualStateManagerStub vsmStub = VisualStateHelper.AttachCustomVSM(stateGrid);
            AttachBehavior(dataStateBehavior, stateGrid);

            dataStateBehavior.FalseState = VisualStateHelper.ArbitraryThirdStateName;
            Assert.AreEqual(vsmStub.LastStateName, VisualStateHelper.ArbitraryThirdStateName, "Change in FalseState should have caused a transition to the new state, as Binding does not match Value.");
        }

        [TestMethod]
        public void ValueChangesToMatchBinding_TransitionsToTrueState()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateDataStateBehaviorInFalseState();
            VisualStateManagerStub vsmStub = VisualStateHelper.AttachCustomVSM(stateGrid);
            AttachBehavior(dataStateBehavior, stateGrid);

            dataStateBehavior.Value = dataStateBehavior.Binding;
            Assert.AreEqual(vsmStub.LastStateName, VisualStateHelper.DefaultTrueStateName, "Value change to match Binding should have caused a transition to the TrueState");
        }

        [TestMethod]
        public void ValueChangesToNotMatchBinding_TransitionsToFalseState()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateDataStateBehaviorInTrueState();
            VisualStateManagerStub vsmStub = VisualStateHelper.AttachCustomVSM(stateGrid);
            AttachBehavior(dataStateBehavior, stateGrid);

            dataStateBehavior.Value = CreateObject();
            Assert.AreEqual(vsmStub.LastStateName, VisualStateHelper.DefaultFalseStateName, "Value change to no longer match Binding should have caused a transition to the FalseState");
        }

        [TestMethod]
        public void BindingChangesToMatchValue_TransitionsToTrueState()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateDataStateBehaviorInFalseState();
            VisualStateManagerStub vsmStub = VisualStateHelper.AttachCustomVSM(stateGrid);
            AttachBehavior(dataStateBehavior, stateGrid);

            dataStateBehavior.Binding = dataStateBehavior.Value;
            Assert.AreEqual(vsmStub.LastStateName, VisualStateHelper.DefaultTrueStateName, "Binding change to match Value should have caused a transition to the TrueState");
        }

        [TestMethod]
        public void BindingChangesToNotMatchValue_TransitionsToFalseState()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            DataStateBehavior dataStateBehavior = CreateDataStateBehaviorInTrueState();
            VisualStateManagerStub vsmStub = VisualStateHelper.AttachCustomVSM(stateGrid);
            AttachBehavior(dataStateBehavior, stateGrid);

            dataStateBehavior.Binding = CreateObject();
            Assert.AreEqual(vsmStub.LastStateName, VisualStateHelper.DefaultFalseStateName, "Binding change to no longer match Value should have caused a transition to the FalseState");
        }
        #endregion
    }
}
