// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;

namespace Microsoft.Xaml.Interactions.UnitTests
{
    using SysWindows = System.Windows;

    [TestClass]
    public class GoToStateActionTest
    {
        #region Factory methods

        private static TestGoToStateAction CreateTestGoToStateAction()
        {
            return new TestGoToStateAction();
        }

        private static Grid CreateEmptyGrid()
        {
            return new Grid();
        }

        #endregion

        #region Helper methods

        private void AttachTo(IAttachedObject attachedObject, SysWindows.DependencyObject element)
        {
            attachedObject.Attach(element);
        }

        private StubTrigger AttachAction(TriggerAction triggerAction, SysWindows.DependencyObject obj)
        {
            WindowedStubTrigger trigger = new WindowedStubTrigger();
            trigger.Actions.Add(triggerAction);
            Interaction.GetTriggers(obj).Add(trigger);
            return trigger;
        }

        private static void AddNamedChild(Panel panel, SysWindows.FrameworkElement childElement)
        {
            if (panel.GetValue(SysWindows.NameScope.NameScopeProperty) == null)
            {
                SysWindows.NameScope.SetNameScope(panel, new SysWindows.NameScope());
            }

            panel.Children.Add(childElement);
            panel.RegisterName(childElement.Name, childElement);
        }

        #endregion

        #region Test methods

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OnTargetChanged_AttachedToGrid_ThrowsNoStateGroupsFoundException()
        {
            TestGoToStateAction action = CreateTestGoToStateAction();
            Grid grid = CreateEmptyGrid();

            this.AttachTo(action, grid);

            action.ChangeTarget(null, grid);
        }

        [TestMethod]
        public void Invoke_UseTransitionsIsFalse_TransitionsAreNotUsed()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            GoToStateAction goToStateAction = CreateTestGoToStateAction();
            VisualStateManagerStub vsm = VisualStateHelper.AttachCustomVSM(stateGrid);
            StubTrigger trigger = AttachAction(goToStateAction, stateGrid);

            goToStateAction.StateName = VisualStateHelper.ArbitraryThirdStateName;
            goToStateAction.UseTransitions = false;
            trigger.FireStubTrigger();

            Assert.IsTrue(vsm.LastUseTransitions.HasValue && !vsm.LastUseTransitions.Value,
                "UseTransitions should be respected by the GoToState call.");
        }

        [TestMethod]
        public void OnTargetChanged_NotAttached_DoesNothing()
        {
            TestGoToStateAction action = CreateTestGoToStateAction();
            Grid grid = CreateEmptyGrid();

            action.ChangeTarget(null, grid);
            // Target changing while not attached should not throw.
        }

        [TestMethod]
        public void Invoke_TargetNameSet_CallsGoToStateOnTarget()
        {
            UserControl statefulUC = VisualStateHelper.CreateObjectWithStates<UserControl>();
            Grid statefulGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            VisualStateManagerStub gridVSM = VisualStateHelper.AttachCustomVSM(statefulGrid);
            Grid childGrid = CreateEmptyGrid();
            GoToStateAction goToStateAction = CreateTestGoToStateAction();
            StubTrigger trigger = AttachAction(goToStateAction, childGrid);

            // set up the tree structure
            statefulGrid.Name = "Target";
            AddNamedChild(childGrid, statefulGrid);
            statefulUC.Content = childGrid;

            // target the action and set the StateName
            goToStateAction.TargetName = "Target";
            goToStateAction.StateName = VisualStateHelper.ArbitraryThirdStateName;

            trigger.FireStubTrigger();
            Assert.AreEqual(gridVSM.LastStateName, VisualStateHelper.ArbitraryThirdStateName, "test");
        }

        [TestMethod]
        public void Invoke_TargetObjectSet_CallsGoToStateOnTarget()
        {
            UserControl statefulUC = VisualStateHelper.CreateObjectWithStates<UserControl>();
            Grid statefulGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            VisualStateManagerStub gridVSM = VisualStateHelper.AttachCustomVSM(statefulGrid);
            Grid childGrid = CreateEmptyGrid();
            GoToStateAction goToStateAction = CreateTestGoToStateAction();
            StubTrigger trigger = AttachAction(goToStateAction, childGrid);

            // set up the tree structure
            statefulUC.Content = childGrid;

            // target the action and set the StateName
            goToStateAction.TargetObject = statefulGrid;
            goToStateAction.StateName = VisualStateHelper.ArbitraryThirdStateName;

            trigger.FireStubTrigger();
            Assert.AreEqual(gridVSM.LastStateName, VisualStateHelper.ArbitraryThirdStateName, "test");
        }

        #endregion

        #region Helper classes

        private class WindowedStubTrigger : StubTrigger
        {
            public override void FireStubTrigger(object parameter)
            {
                object content = this.FindContent();
                using (new StubWindow(content))
                {
                    base.FireStubTrigger(parameter);
                }
            }

            private object FindContent()
            {
                SysWindows.FrameworkElement content = this.AssociatedObject as SysWindows.FrameworkElement;
                while (content != null)
                {
                    SysWindows.FrameworkElement parent = content.Parent as SysWindows.FrameworkElement ??
                                                         content.TemplatedParent as SysWindows.FrameworkElement;
                    if (parent == null || parent is SysWindows.Window)
                    {
                        break;
                    }

                    content = parent;
                }

                return content;
            }
        }

        private class TestGoToStateAction : GoToStateAction
        {
            public SysWindows.FrameworkElement StateElement
            {
                get;
                set;
            }

            public void ChangeTarget(SysWindows.FrameworkElement oldTarget, SysWindows.FrameworkElement newTarget)
            {
                this.OnTargetChanged(oldTarget, newTarget);
            }

            protected override void OnTargetChanged(SysWindows.FrameworkElement oldTarget,
                SysWindows.FrameworkElement newTarget)
            {
                base.OnTargetChanged(oldTarget, newTarget);
            }
        }

        #endregion
    }
}
