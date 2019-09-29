// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Shapes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Behaviors;
    using Microsoft.Xaml.Behaviors.Core;

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

        internal class EventArgsMock : EventArgs
        {
            public PocoMock Poco { get; set; }

            public string Name { get; set; } = "default";

            public EventArgsMock() { }

            public EventArgsMock(string name)
            {
                Name = name;
                Poco = new PocoMock { Name = name, Child = new PocoMock { Name = name } };
            }
        }

        internal class PocoMock
        {
            public PocoMock Child { get; set; }
            public string Name { get; set; }
        }

        internal class EventArgsMockConverter : IValueConverter
        {
            public object Parameter { get; private set; }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                Parameter = parameter;
                return "convertedValue";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
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

        [TestMethod]
        public void Invoke_ActionWithEventArgsParameterPath_PassesEventArgsValueToCommand()
        {
            StubBehavior stubBehavior = CreateStubBehavior();

            InvokeCommandAction invokeCommandAction = CreateInvokeCommandAction();
            invokeCommandAction.Command = stubBehavior.StubCommandWithParameter;
            invokeCommandAction.EventArgsParameterPath = "Name";

            StubTrigger trigger = AttachActionToObject(invokeCommandAction, stubBehavior);

            var args = new EventArgsMock();
            trigger.FireStubTrigger(args);

            Assert.IsNotNull(stubBehavior.LastParameter);
            Assert.AreEqual("default", stubBehavior.LastParameter);
        }

        [TestMethod]
        public void Invoke_ActionWithEventArgsParameterPath_PassesNestedEventArgsValueToCommand()
        {
            StubBehavior stubBehavior = CreateStubBehavior();

            InvokeCommandAction invokeCommandAction = CreateInvokeCommandAction();
            invokeCommandAction.Command = stubBehavior.StubCommandWithParameter;
            invokeCommandAction.EventArgsParameterPath = "Poco.Child.Name";

            StubTrigger trigger = AttachActionToObject(invokeCommandAction, stubBehavior);

            var args = new EventArgsMock("value");
            trigger.FireStubTrigger(args);

            Assert.IsNotNull(stubBehavior.LastParameter);
            Assert.AreEqual("value", stubBehavior.LastParameter);
        }

        [TestMethod]
        public void Invoke_ActionWithEventArgsConverter_PassesConvertedValueToCommand()
        {
            StubBehavior stubBehavior = CreateStubBehavior();

            InvokeCommandAction invokeCommandAction = CreateInvokeCommandAction();
            invokeCommandAction.Command = stubBehavior.StubCommandWithParameter;
            invokeCommandAction.EventArgsConverter = new EventArgsMockConverter();

            StubTrigger trigger = AttachActionToObject(invokeCommandAction, stubBehavior);

            var args = new EventArgsMock();
            trigger.FireStubTrigger(args);

            Assert.IsNotNull(stubBehavior.LastParameter);
            Assert.AreEqual("convertedValue", stubBehavior.LastParameter);
        }

        [TestMethod]
        public void Invoke_ActionWithEventArgsConverterWithParameter_PassesConvertedValueToCommand()
        {
            StubBehavior stubBehavior = CreateStubBehavior();

            InvokeCommandAction invokeCommandAction = CreateInvokeCommandAction();
            invokeCommandAction.Command = stubBehavior.StubCommandWithParameter;

            var converterParameter = CreateButton();
            var converter = new EventArgsMockConverter();
            invokeCommandAction.EventArgsConverter = converter;
            invokeCommandAction.EventArgsConverterParameter = converterParameter;

            StubTrigger trigger = AttachActionToObject(invokeCommandAction, stubBehavior);

            var args = new EventArgsMock();
            trigger.FireStubTrigger(args);

            Assert.IsNotNull(stubBehavior.LastParameter);
            Assert.AreEqual("convertedValue", stubBehavior.LastParameter);
            Assert.AreEqual(converterParameter, converter.Parameter);
        }

        [TestMethod]
        public void Invoke_ActionWithPassEventArgsToCommandFalse_DoesNotPassEventArgsToCommand()
        {
            StubBehavior stubBehavior = CreateStubBehavior();

            InvokeCommandAction invokeCommandAction = CreateInvokeCommandAction();
            invokeCommandAction.Command = stubBehavior.StubCommandWithParameter;
            invokeCommandAction.PassEventArgsToCommand = false;

            StubTrigger trigger = AttachActionToObject(invokeCommandAction, stubBehavior);

            var args = new EventArgsMock();
            trigger.FireStubTrigger(args);

            Assert.IsNull(stubBehavior.LastParameter);
        }

        [TestMethod]
        public void Invoke_ActionWithPassEventArgsToCommandTrue_PassesEventArgsToCommand()
        {
            StubBehavior stubBehavior = CreateStubBehavior();

            InvokeCommandAction invokeCommandAction = CreateInvokeCommandAction();
            invokeCommandAction.Command = stubBehavior.StubCommandWithParameter;
            invokeCommandAction.PassEventArgsToCommand = true;
            
            StubTrigger trigger = AttachActionToObject(invokeCommandAction, stubBehavior);

            var args = new EventArgsMock();

            trigger.FireStubTrigger(args);

            Assert.IsNotNull(stubBehavior.LastParameter);
            Assert.IsInstanceOfType(stubBehavior.LastParameter, typeof(EventArgsMock));
        }
        #endregion
    }
}
