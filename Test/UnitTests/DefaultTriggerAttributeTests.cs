// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Expression.Interactivity.UnitTests
{
	using System;
	using System.Collections;
    using System.Diagnostics;
    using System.Windows.Controls;
	using System.Windows.Interactivity;
	using  Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class DefaultTriggerAttributeTests
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
		public void TestDefaultTriggerAttribute()
		{
			this.TestConstructor();
			this.TestInstantiate();
		}

		private void TestConstructor()
		{
			object[] objects = { "test", 0.5 };
			DefaultTriggerAttribute attribute = new DefaultTriggerAttribute(typeof(Button), typeof(StubTrigger), objects);

			Assert.AreEqual(attribute.TargetType, typeof(Button), "attribute.TargetType == typeof(Button)");
			Assert.AreEqual(attribute.TriggerType, typeof(StubTrigger), "attribute.TriggerType == typeof(StubTrigger)");
			IEnumerator parameterEnumerator = attribute.Parameters.GetEnumerator();
			parameterEnumerator.MoveNext();
			Assert.AreEqual(parameterEnumerator.Current, "test", "attribute.Parameters[0] == \"test\"");
			parameterEnumerator.MoveNext();
			Assert.AreEqual(parameterEnumerator.Current, 0.5, "attribute.Parameters[1] == 0.5");
			Assert.AreEqual(parameterEnumerator.MoveNext(), false, "attribute.Parameters.Length == 2");

			try
			{
				DefaultTriggerAttribute illegalAttribute = new DefaultTriggerAttribute(typeof(Button), typeof(Button), new object[0]);
				Debug.Fail("ArgumentException should be thrown.");
			}
			catch(ArgumentException)
			{
			}
		}

		private void TestInstantiate()
		{
			DefaultTriggerAttribute eventTriggerAttribute = new DefaultTriggerAttribute(typeof(Button), typeof(EventTrigger), new object[] { "Click" });
			EventTrigger trigger = eventTriggerAttribute.Instantiate() as EventTrigger;

			Assert.IsNotNull(trigger, "Instantiated trigger is an EventTrigger");
			Assert.AreEqual(trigger.EventName, "Click", "EventTrigger.EventName == Click");

			DefaultTriggerAttribute illegalAttribute = new DefaultTriggerAttribute(typeof(Button), typeof(SingleConstructorArgumentTrigger), new object[0]);
			try
			{
				TriggerBase triggerBase = illegalAttribute.Instantiate();
				Assert.IsNull(triggerBase, "Illegal call to instantiate results in null result, no exception thrown.");
			}
			catch
			{
				Debug.Fail("Unexpected exception thrown.");
			}
		}
	}
}
