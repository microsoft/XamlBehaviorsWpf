// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System;
    using System.Diagnostics;
    using System.Windows.Controls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Interactivity;
    using SysWindows = System.Windows;

    [TestClass]
    public class ActionTest
    {
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

        [TestMethod]
        public void AttachDetachTest()
        {
            StubAction stubAction = new StubAction();
            BehaviorTestUtilities.TestIAttachedObject<Button>((IAttachedObject)stubAction);

            StubTargetedTriggerAction stubTargetedTriggerAction = new StubTargetedTriggerAction();
            BehaviorTestUtilities.TestIAttachedObject<Button>((IAttachedObject)stubTargetedTriggerAction);
        }

        [TestMethod]
        public void ConstrainedActionTest()
        {
            BehaviorTestUtilities.TestConstraintOnAssociatedObject<Button>(new StubButtonTriggerAction());
        }

        [TestMethod]
        public void InvokeTest()
        {
            StubAction action = new StubAction();
            StubTrigger trigger = new StubTrigger();

            Assert.AreEqual(action.InvokeCount, 0, "action.InvokeCount == 0");
            action.StubInvoke();
            Assert.AreEqual(action.InvokeCount, 1, "After invoking action, action.InvokeCount == 1");
            trigger.Actions.Add(action);
            action.IsEnabled = false;
            trigger.FireStubTrigger();
            Assert.AreEqual(action.InvokeCount, 1, "action.InvokeCount == 1");
            action.IsEnabled = true;
            trigger.FireStubTrigger();
            Assert.AreEqual(action.InvokeCount, 2, "action.InvokeCount == 2");
        }

        [TestMethod]
        public void TestCreateInstanceCore()
        {
            StubAction action = new StubAction();
            SysWindows.Freezable freezable = action.GetCreateInstanceCore();
            Assert.AreEqual(freezable.GetType(), typeof(StubAction), "freezable.GetType() == typeof(StubAction)");
        }

        [TestMethod]
        public void ParentTriggerTest()
        {
            StubTrigger trigger = new StubTrigger();
            StubAction action = new StubAction();

            trigger.Actions.Add(action);
            Assert.AreEqual(((IAttachedObject)action).AssociatedObject, trigger.HostObject, "After adding action to trigger, action.AssociatedObject should equal trigger.Host");
            Assert.AreEqual(trigger.Actions.Count, 1, "trigger.Actions.Count == 1");

            trigger.Actions.Remove(action);
            Assert.IsNull(((IAttachedObject)action).AssociatedObject, "After removing action from trigger, action.AssociatedObject should be null");
            Assert.AreEqual(trigger.Actions.Count, 0, "trigger.Actions.Count == 0");
        }

        [TestMethod]
        public void ReparentActionTest()
        {
            SysWindows.Shapes.Rectangle hostRectangle = new SysWindows.Shapes.Rectangle();
            // try parenting an action more than once; should throw
            StubAction action = new StubAction();
            StubTrigger trigger1 = new StubTrigger();
            StubTrigger trigger2 = new StubTrigger();

            trigger1.Attach(hostRectangle);
            trigger2.Attach(hostRectangle);
            trigger1.Actions.Add(action);
            try
            {
                trigger2.Actions.Add(action);
                Debug.Fail("Expected InvalidOperationException to be thrown after adding an action to a second trigger.");
            }
            catch (InvalidOperationException)
            {
            }

            // now try the same, properly unhooking before reparenting
            action = new StubAction();
            trigger1 = new StubTrigger();
            trigger2 = new StubTrigger();
            trigger1.Actions.Add(action);
            trigger1.Actions.Remove(action);
            trigger2.Actions.Add(action);
            Assert.AreEqual(((IAttachedObject)action).AssociatedObject, trigger2.HostObject, "action.AssociatedObject == trigger2.Host");
            Assert.AreEqual(trigger2.Actions.Count, 1, "trigger2.Actions.Count == 1");
        }
    }
}
