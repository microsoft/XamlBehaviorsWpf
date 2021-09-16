// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;

namespace Microsoft.Xaml.Interactions.UnitTests
{
    using SysWindows = System.Windows;

    [TestClass]
    public class BehaviorTest
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
            StubBehavior stubBehavior = new StubBehavior();
            BehaviorTestUtilities.TestIAttachedObject<Button>(stubBehavior);

            Rectangle rectangle = new Rectangle();
            StubBehavior behavior = new StubBehavior();
            BehaviorCollection behaviorCollection = Interaction.GetBehaviors(rectangle);
            behaviorCollection.Add(behavior);
            behaviorCollection.Detach();
            BehaviorTestUtilities.TestIAttachedObject<Button>(behaviorCollection);
        }

        [TestMethod]
        public void TestCreateInstanceCore()
        {
            StubBehavior action = new StubBehavior();
            SysWindows.Freezable freezable = action.GetCreateInstanceCore();
            Assert.AreEqual(freezable.GetType(), typeof(StubBehavior), "freezable.GetType() == typeof(StubBehavior)");
        }

        [TestMethod]
        public void AttachBehaviorMultipleElements_ShouldThrow()
        {
            StubBehavior behavior = new StubBehavior();

            Button button1 = new Button();
            BehaviorCollection behaviors1 = Interaction.GetBehaviors(button1);

            Button button2 = new Button();
            BehaviorCollection behaviors2 = Interaction.GetBehaviors(button2);
            behaviors1.Add(behavior);

            try
            {
                behaviors2.Add(behavior);
                Assert.Fail("InvalidOperationexception should be thrown if same behavior is attached to two elements");
            } catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void AttachBehaviorToConstrainedCollection_ShouldThrow()
        {
            // verify we can't host a constrained Behavior on the wrong type
            StubRectangleBehavior rectangleBehavior = new StubRectangleBehavior();
            Rectangle rectangle1 = new Rectangle();
            Button button4 = new Button();
            BehaviorCollection rectangleBehaviors = Interaction.GetBehaviors(rectangle1);
            BehaviorCollection buttonBehaviors = Interaction.GetBehaviors(button4);

            rectangleBehaviors.Add(rectangleBehavior);
            Assert.AreEqual(rectangleBehaviors.Count, 1,
                "After attaching RectangleBehavior to rectangleBehaviors, rectangleBehaviors.Count should be 1");
            Assert.AreEqual(((IAttachedObject)rectangleBehavior).AssociatedObject, rectangle1,
                "rectangleBehavior.AssociatedObject == rectangle");

            rectangleBehaviors.Remove(rectangleBehavior);
            Assert.AreEqual(rectangleBehaviors.Count, 0,
                "After detaching rectangleBehavior, rectangleBehaviors.Count should be 0");
            Assert.IsNull(((IAttachedObject)rectangleBehavior).AssociatedObject,
                "rectangleBehavior.AssociatedObject == null");

            try
            {
                buttonBehaviors.Add(rectangleBehavior);
                Assert.Fail("Expected InvalidOperationException to be thrown thrown.");
            } catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void AttachBehaviorsTest()
        {
            StubBehavior behavior1 = new StubBehavior();
            StubBehavior behavior2 = new StubBehavior();

            Button button1 = new Button();

            BehaviorCollection behaviors1 = Interaction.GetBehaviors(button1);
            Assert.AreEqual(behaviors1.Count, 0, "behaviors.Count == 0");
            behaviors1.Add(behavior1);
            Assert.AreEqual(behaviors1.Count, 1, "behaviors.Count == 1");
            Assert.AreEqual(behavior1.AttachedObject, button1, "behavior1.AssociatedObject == button");
            behaviors1.Add(behavior2);
            Assert.AreEqual(behaviors1.Count, 2, "behaviors.Count == 2");
            Assert.AreEqual(behavior2.AttachedObject, button1, "behavior2.AssociatedObject == button");
        }

        [TestMethod]
        public void TestParameterlessActionCommand()
        {
            ActionCommand action = new ActionCommand(this.ParameterlessActionSuccessful);
            this.actionTestSucceeded = false;
            Assert.IsTrue(((ICommand)action).CanExecute(null), "action CanExecute(null) == true");
            action.Execute(null);
            Assert.IsTrue(this.actionTestSucceeded, "parameterlessAction test succeeded.");
        }

        [TestMethod]
        public void TestParameterActionCommand()
        {
            ActionCommand parameterAction = new ActionCommand(this.ParameterActionSuccessful);
            this.actionTestSucceeded = false;
            Assert.IsTrue(((ICommand)parameterAction).CanExecute(null), "parameterAction CanExecute(null) == true");
            parameterAction.Execute(this.actionTestButton);
            Assert.IsTrue(this.actionTestSucceeded, "parameterlessAction test succeeded.");
        }

        #region ActionCommand test cross-function state

        private bool actionTestSucceeded;
        private readonly Button actionTestButton = new Button();

        private void ParameterlessActionSuccessful()
        {
            this.actionTestSucceeded = true;
        }

        private void ParameterActionSuccessful(object o)
        {
            Assert.AreEqual(o, this.actionTestButton, "parameter == this.actionTestButton");
            this.actionTestSucceeded = true;
        }

        private void command_CanExecuteChanged(object sender, EventArgs e)
        {
            Assert.Fail("ActionCommand.CanExecuteChanged should never be called.");
        }

        #endregion
    }
}
