// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System;
    using System.Windows;
    using System.Windows.Shapes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Behaviors;
    using Microsoft.Xaml.Behaviors.Core;

    [TestClass]
    public sealed class CallMethodActionTest
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

        #region Test methods
        [TestMethod]
        public void Invoke_UniqueMethodWithNoParameters_IsCalled()
        {
            MethodObjectStub methodObject = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("UniqueMethodWithNoParameters");
            StubTrigger trigger = AttachAction(action, methodObject);

            trigger.FireStubTrigger();

            Assert.AreEqual(methodObject.LastMethodCalled, "UniqueMethodWithNoParameters", "UniqueMethodWithNoParameters was not called.");
        }

        [TestMethod]
        public void Invoke_MultipleMethodsWithSameName_EventHandlerSignatureIsCalled()
        {
            MethodObjectStub methodObject = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("DuplicatedMethod");
            StubTrigger trigger = AttachAction(action, methodObject);

            trigger.FireStubTrigger(EventArgs.Empty);

            Assert.AreEqual(methodObject.LastMethodCalled, "DuplicatedMethodWithEventHandlerSignature", "DuplicatedMethodWithEventHandlerSignature was not called.");
        }

        [TestMethod]
        public void Invoke_MultipleMethodsWithSpecificParameter_MostDerivedSignatureIsCalled()
        {
            MethodObjectStub methodObject = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("DuplicatedMethod");
            StubTrigger trigger = AttachAction(action, methodObject);

            trigger.FireStubTrigger(new StubEventArgs());

            Assert.AreEqual(methodObject.LastMethodCalled, "DuplicatedMethodWithStubEventArgsSignature", "DuplicatedMethodWithStubEventArgsSignature was not called.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_NonExistantMethodWithTargetSet_ThrowsArgumentException()
        {
            Rectangle host = CreateRectangle();
            MethodObjectStub target = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("NonExistantMethodName");
            StubTrigger trigger = AttachAction(action, host);

            action.TargetObject = target;
            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_IncorrectReturnTypeWithTargetSet_ThrowsArgumentException()
        {
            Rectangle host = CreateRectangle();
            MethodObjectStub target = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("IncompatibleReturnType");
            StubTrigger trigger = AttachAction(action, host);

            action.TargetObject = target;
            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_IncorrectParametersWithTargetSet_ThrowsArgumentException()
        {
            Rectangle host = CreateRectangle();
            MethodObjectStub target = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("IncompatibleParameters");
            StubTrigger trigger = AttachAction(action, host);

            action.TargetObject = target;
            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_NonExistantMethodWithNoTarget_DoesNothing()
        {
            MethodObjectStub host = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("NonExistantMethodName");
            StubTrigger trigger = AttachAction(action, host);

            trigger.FireStubTrigger();
            Assert.AreEqual(host.LastMethodCalled, "None", "No method should be called");
        }

        [TestMethod]
        public void Invoke_IncorrectReturnTypeWithTargetSet_DoesNothing()
        {
            MethodObjectStub host = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("IncompatibleReturnType");
            StubTrigger trigger = AttachAction(action, host);

            trigger.FireStubTrigger();
            Assert.AreEqual(host.LastMethodCalled, "None", "No method should be called");
        }

        [TestMethod]
        public void Invoke_IncorrectParametersWithTargetSet_DoesNothing()
        {
            MethodObjectStub host = CreateMethodObject();
            CallMethodAction action = CreateCallMethodAction("IncompatibleParameters");
            StubTrigger trigger = AttachAction(action, host);

            trigger.FireStubTrigger();
            Assert.AreEqual(host.LastMethodCalled, "None", "No method should be called");
        }
        #endregion

        #region Helper methods and classes

        private StubTrigger AttachAction(CallMethodAction action, DependencyObject host)
        {
            StubTrigger trigger = CreateTrigger();
            trigger.Actions.Add(action);
            Interaction.GetTriggers(host).Add(trigger);
            return trigger;
        }

        private class MethodObjectStub : DependencyObject
        {
            public string LastMethodCalled
            {
                get;
                private set;
            }

            public MethodObjectStub()
            {
                this.LastMethodCalled = "None";
            }

            public void UniqueMethodWithNoParameters()
            {
                this.LastMethodCalled = "UniqueMethodWithNoParameters";
            }

            public void DuplicatedMethod()
            {
                this.LastMethodCalled = "DuplicatedMethodWithNoParameters";
            }

            public void DuplicatedMethod(object sender, EventArgs args)
            {
                this.LastMethodCalled = "DuplicatedMethodWithEventHandlerSignature";
            }

            public void DuplicatedMethod(object sender, StubEventArgs args)
            {
                this.LastMethodCalled = "DuplicatedMethodWithStubEventArgsSignature";
            }

            public int IncompatibleReturnType()
            {
                this.LastMethodCalled = "IncompatibleReturnType";
                return 0;
            }

            public void IncompatibleParameters(double d)
            {
                this.LastMethodCalled = "IncompatibleParameters";
            }
        }

        private class StubEventArgs : EventArgs
        {

        }

        #endregion

        #region Factory methods

        private CallMethodAction CreateCallMethodAction(string methodName)
        {
            CallMethodAction action = new CallMethodAction();
            action.MethodName = methodName;
            return action;
        }

        private Rectangle CreateRectangle()
        {
            return new Rectangle();
        }

        private MethodObjectStub CreateMethodObject()
        {
            return new MethodObjectStub();
        }

        private StubTrigger CreateTrigger()
        {
            return new StubTrigger();
        }

        #endregion

    }
}
