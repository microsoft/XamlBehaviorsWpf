// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Expression.Interactivity.UnitTests
{
	using System;
    using System.Diagnostics;
    using System.Windows.Controls;
	using System.Windows.Interactivity;
	using System.Windows.Shapes;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using WindowsBase = System.Windows;

	[TestClass]
	public class TriggerTest
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
			StubTrigger stubTrigger = new StubTrigger();
			BehaviorTestUtilities.TestIAttachedObject<Button>((IAttachedObject)stubTrigger);

			StubEventTriggerBase stubEventTrigger = new StubEventTriggerBase();
			BehaviorTestUtilities.TestIAttachedObject<StubBehavior>((IAttachedObject)stubEventTrigger);
		}

		[TestMethod]
		public void ConstrainedTriggerTest()
		{
			StubButtonTrigger stubButtonTrigger = new StubButtonTrigger();
			BehaviorTestUtilities.TestConstraintOnAssociatedObject<Button>(stubButtonTrigger);
		}

		[TestMethod]
		public void TriggerAssociatedObjectAccessorTest()
		{
			Button button = new Button();
			StubButtonTrigger stubButtonTrigger = new StubButtonTrigger();
			stubButtonTrigger.Attach(button);
			Assert.AreEqual(stubButtonTrigger.AttachedButton, button, "trigger.AssociatedObject == button");
		}

		[TestMethod]
		public void CreateInstanceCoreTest()
		{
			StubButtonTrigger stubButtonTrigger = new StubButtonTrigger();
			Assert.IsTrue(stubButtonTrigger.CreateInstaceCoreStub().GetType().Equals(typeof(StubButtonTrigger)), "TriggerBase.CreateInstanceCore works.");
		}

		[TestMethod]
		public void AddTriggerTest()
		{
			Button button = new Button();
			StubTrigger trigger = new StubTrigger();

			TriggerCollection tc = Interaction.GetTriggers(button);
			tc.Add(trigger);

			Assert.IsTrue(trigger.AddedHost, "Trigger has been parented.");
			Assert.IsFalse(trigger.ChangedHost, "Trigger has not changed parents.");
			Assert.IsFalse(trigger.RemovedHost, "Trigger has not been unparented.");
		}


		[TestMethod]
		public void TestCreateInstanceCore()
		{
			StubTrigger action = new StubTrigger();
			WindowsBase.Freezable freezable = action.GetCreateInstanceCore();
			Assert.AreEqual(freezable.GetType(), typeof(StubTrigger), "freezable.GetType() == typeof(StubTrigger)");
		}

		[TestMethod]
		public void RemoveTriggerTest()
		{
			Button button = new Button();
			StubTrigger trigger = new StubTrigger();

			TriggerCollection tc = Interaction.GetTriggers(button);
			tc.Add(trigger);
			Assert.AreEqual(trigger.HostObject, button, "trigger.AssociatedObject == button");
			tc.Remove(trigger);

			Assert.IsTrue(trigger.AddedHost, "Trigger has been parented.");
			Assert.IsFalse(trigger.ChangedHost, "Trigger has not changed parents.");
			Assert.IsTrue(trigger.RemovedHost, "Trigger has been unparented.");
		}

		[TestMethod]
		public void MoveTriggerTest()
		{
			Button button = new Button();
			Rectangle rectangle = new Rectangle();
			StubTrigger trigger = new StubTrigger();

			TriggerCollection tc = Interaction.GetTriggers(rectangle);
			TriggerCollection tc2 = Interaction.GetTriggers(button);

			tc.Add(trigger);
			tc.Remove(trigger);
			tc2.Add(trigger);

			Assert.IsTrue(trigger.AddedHost, "Trigger has been parented.");
			Assert.IsTrue(trigger.ChangedHost, "Trigger has changed parents.");
			Assert.IsTrue(trigger.RemovedHost, "Trigger has been unparented.");
			Assert.AreEqual(trigger.HostObject, button, "trigger.AssociatedObject == button");
			Assert.AreEqual(Interaction.GetTriggers(rectangle).Count, 0, "rectangle.Triggers.Count == 0");
		}

		[TestMethod]
		public void AttachMultipleTimesTest()
		{
			EventTriggerBase trigger = new StubTrigger();
			Rectangle rectangle1 = new Rectangle();
			Rectangle rectangle2 = new Rectangle();
			TriggerCollection collection1 = Interaction.GetTriggers(rectangle1);
			TriggerCollection collection2 = Interaction.GetTriggers(rectangle2);

			collection1.Add(trigger);
			// it is illegal to add the same trigger to more than one object
			try
			{
				collection2.Add(trigger);
				Debug.Fail("Attaching the same trigger twice should throw a InvalidOperationException");
			}
			catch(InvalidOperationException)
			{
			}
		}

		[TestMethod]
		public void InvokeTriggerTest()
		{
			Button parameter = new Button();
			Rectangle parameter2 = new Rectangle();
			StubAction action1 = new StubAction();
			StubAction action2 = new StubAction();
			ParameterTriggerAction parameterAction = new ParameterTriggerAction();
			StubTrigger trigger = new StubTrigger();

			trigger.Actions.Add(action1);
			trigger.FireStubTrigger();
			Assert.AreEqual(action1.InvokeCount, 1, "action1.InvokeCount == 1");
			Assert.AreEqual(action2.InvokeCount, 0, "action2.InvokeCount == 0");
			Assert.AreEqual(parameterAction.InvokeCount, 0, "action3.InvokeCount == 0");

            // Adding action 2
			trigger.Actions.Add(action2);
			trigger.FireStubTrigger();
			Assert.AreEqual(action1.InvokeCount, 2, "action1.InvokeCount == 2");
			Assert.AreEqual(action2.InvokeCount, 1, "action2.InvokeCount == 1");
			Assert.AreEqual(parameterAction.InvokeCount, 0, "action3.InvokeCount == 0");

            // Adding action 3
			trigger.Actions.Add(parameterAction);
			trigger.FireStubTrigger(parameter);
			Assert.AreEqual(action1.InvokeCount, 3, "action1.InvokeCount == 3");
			Assert.AreEqual(action2.InvokeCount, 2, "action2.InvokeCount == 2");
			Assert.AreEqual(parameterAction.InvokeCount, 1, "parameterAction.InvokeCount == 1");
			Assert.AreEqual(parameterAction.Parameter, parameter, "parameterAction.Parameter == parameter");

			// Firing with parameter 2
			trigger.FireStubTrigger(parameter2);
			Assert.AreEqual(action1.InvokeCount, 4, "action1.InvokeCount == 4");
			Assert.AreEqual(action2.InvokeCount, 3, "action2.InvokeCount == 3");
			Assert.AreEqual(parameterAction.InvokeCount, 2, "parameterAction.InvokeCount == 2");
			Assert.AreEqual(parameterAction.Parameter, parameter2, "parameterAction.Parameter == parameter2");
		}

		[TestMethod]
		public void ActionInvocationOrderTest()
		{
			Rectangle rectangle = new Rectangle();
			TimedAction action1 = new TimedAction();
			TimedAction action2 = new TimedAction();
			TimedAction action3 = new TimedAction();
			TriggerCollection collection = Interaction.GetTriggers(rectangle);
			StubTrigger trigger = new StubTrigger();
			trigger.Actions.Add(action1);
			trigger.Actions.Add(action2);
			trigger.Actions.Add(action3);
			collection.Add(trigger);

			trigger.FireStubTrigger();
			Assert.IsTrue(action1.Order < action2.Order, "action1.Order < action2.Order");
			Assert.IsTrue(action2.Order < action3.Order, "action2.Order < action3.Order");
		}
	}
}
