// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Expression.Interactivity.UnitTests
{
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Interactivity;
	using System.Windows.Shapes;
    using Microsoft.Expression.Interactivity.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SysWindows = System.Windows;

	[TestClass]
	public sealed class InvokeCommandActionTest
	{
		#region Setup and teardown methods

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

		#region Factory methods

		private static Button CreateButton()
		{
			return new Button();
		}

		private static StubBehavior CreateStubBehavior()
		{
			StubBehavior stubBehavior = new StubBehavior();
			return stubBehavior;
		}

		private static StubTrigger CreateTrigger()
		{
			StubTrigger stubTrigger = new StubTrigger();
			return stubTrigger;
		}

		private static InvokeCommandAction CreateInvokeCommandAction()
		{
			return new InvokeCommandAction();
		}

		private static InvokeCommandAction CreateInvokeCommandActionWithCommandName(string commandName)
		{
			InvokeCommandAction invokeCommandAction1 = CreateInvokeCommandAction();
			invokeCommandAction1.CommandName = commandName;
			return invokeCommandAction1;
		}

		private InvokeCommandAction CreateInvokeCommandActionWithCommandNameAndParameter(string p, object parameter)
		{
			InvokeCommandAction action = CreateInvokeCommandActionWithCommandName(p);
			action.CommandParameter = parameter;
			return action;
		}

		private InvokeCommandAction CreateInvokeCommandActionWithCommand(ICommand command)
		{
			InvokeCommandAction action = CreateInvokeCommandAction();
			action.Command = command;
			return action;
		}

		private Rectangle CreateRectangle()
		{
			return new Rectangle();
		}

		private CommandHelper CreateCommandHelper()
		{
			return new CommandHelper();
		}

		#endregion

		#region Helper methods

		private static StubTrigger AttachActionToObject(InvokeCommandAction invokeCommandAction, SysWindows.DependencyObject dependencyObject)
		{
			TriggerCollection triggersCollection = Interaction.GetTriggers(dependencyObject);
			StubTrigger stubTrigger = CreateTrigger();
			triggersCollection.Add(stubTrigger);
			stubTrigger.Actions.Add(invokeCommandAction);
			return stubTrigger;
		}

		private class CommandHelper
		{
			public enum CommandResults
			{
				NotCalled,
				Failure,
				Success,
			}

			public CommandResults Result
			{
				get;
				private set;
			}

			public ICommand SuccessCommand
			{
				get;
				private set;
			}

			public ICommand FailCommand
			{
				get;
				private set;
			}

			public bool Successful
			{
				get { return this.Result == CommandResults.Success; }
			}

			public CommandHelper()
			{
				this.Result = CommandResults.NotCalled;

				this.SuccessCommand = new ActionCommand(() =>
					{
						this.Result = CommandResults.Success;
					});
				this.FailCommand = new ActionCommand(() =>
					{
						this.Result = CommandResults.Failure;
					});
			}
		}

		#endregion

		#region Test methods
		[TestMethod]
		public void Invoke_LegalCommandName_InvokesCommand()
		{
			InvokeCommandAction invokeCommandAction = CreateInvokeCommandActionWithCommandName("StubCommand");
			StubBehavior stubBehavior = CreateStubBehavior();
			StubTrigger trigger = AttachActionToObject(invokeCommandAction, stubBehavior);

			trigger.FireStubTrigger();
			Assert.AreEqual(stubBehavior.ExecutionCount, 1, "stubBehavior.ExecutionCount == 1");
		}

		[TestMethod]
		public void Invoke_ActionWithCommandParameter_UsesParameterCorrectly()
		{
			Rectangle triggerParameter = CreateRectangle();
			Button behaviorParameter = CreateButton();
			InvokeCommandAction invokeCommandAction = CreateInvokeCommandActionWithCommandNameAndParameter("StubCommandWithParameter", behaviorParameter);
			StubBehavior stubBehavior = CreateStubBehavior();
			StubTrigger trigger = AttachActionToObject(invokeCommandAction, stubBehavior);

			trigger.FireStubTrigger(triggerParameter);
			Assert.AreEqual(stubBehavior.LastParameter, behaviorParameter, "stubBehavior.LastParameter == button");
		}

		[TestMethod]
		public void Invoke_WithCommand_InvokesCommand()
		{
			CommandHelper commandHelper = CreateCommandHelper();
			InvokeCommandAction invokeCommandAction = CreateInvokeCommandActionWithCommand(commandHelper.SuccessCommand);
			Button button = CreateButton();
			StubTrigger trigger = AttachActionToObject(invokeCommandAction, button);

			trigger.FireStubTrigger();
			Assert.IsTrue(commandHelper.Successful, "Command should have been invoked.");
		}

		[TestMethod]
		public void Invoke_WithCommandNameAndCommand_InvokesCommand()
		{
			CommandHelper commandHelper = CreateCommandHelper();
			InvokeCommandAction invokeCommandAction = CreateInvokeCommandActionWithCommandName("Command");
			invokeCommandAction.Command = commandHelper.SuccessCommand;
			Button button = CreateButton();
			button.Command = commandHelper.FailCommand;
			StubTrigger trigger = AttachActionToObject(invokeCommandAction, button);

			trigger.FireStubTrigger();
			Assert.IsTrue(commandHelper.Successful, "Command should have been invoked, CommandName should not have been invoked.");
		}
		#endregion
	}
}
