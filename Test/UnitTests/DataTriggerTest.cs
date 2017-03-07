// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System.Windows.Shapes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Interactions.Core;
    using Microsoft.Xaml.Interactivity;
    using SysWindows = System.Windows;

    [TestClass]
    public sealed class DataTriggerTest
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

        #region Factory methods

        private static StubAction CreateStubAction()
        {
            return new StubAction();
        }

        private static Rectangle CreateRectangle()
        {
            return new Rectangle();
        }

        private static DataTrigger CreateDataTrigger(object bindingValue, ComparisonConditionType comparisonType, object valueValue)
        {
            return new DataTrigger()
            {
                Binding = bindingValue,
                Comparison = comparisonType,
                Value = valueValue,
            };
        }

        private static StubAction AttachActionToDataTrigger(DataTrigger dataTrigger, SysWindows.DependencyObject hostObject)
        {
            StubAction stubAction = CreateStubAction();
            dataTrigger.Actions.Add(stubAction);
            Interaction.GetTriggers(hostObject).Add(dataTrigger);
            return stubAction;
        }

        #endregion

        #region Test methods

        [TestMethod]
        public void SetBinding_ToSatisfyCondition_CausesReevaluation()
        {
            DataTrigger dataTrigger = CreateDataTrigger("Foo", ComparisonConditionType.Equal, "Bar");
            Rectangle hostRectangle = CreateRectangle();
            StubAction stubAction = AttachActionToDataTrigger(dataTrigger, hostRectangle);

            dataTrigger.Binding = "Bar";
            Assert.AreEqual(stubAction.InvokeCount, 1, "The trigger should have been invoked once.");
        }

        [TestMethod]
        public void SetValue_ToSatisfyCondition_CausesReevaluation()
        {
            DataTrigger dataTrigger = CreateDataTrigger("Foo", ComparisonConditionType.Equal, "Bar");
            Rectangle hostRectangle = CreateRectangle();
            StubAction stubAction = AttachActionToDataTrigger(dataTrigger, hostRectangle);

            dataTrigger.Value = "Foo";
            Assert.AreEqual(stubAction.InvokeCount, 1, "The trigger should have been invoked once.");
        }

        [TestMethod]
        public void SetComparison_ToSatisfyCondition_CausesReevaluation()
        {
            DataTrigger dataTrigger = CreateDataTrigger("Foo", ComparisonConditionType.NotEqual, "Foo");
            Rectangle hostRectangle = CreateRectangle();
            StubAction stubAction = AttachActionToDataTrigger(dataTrigger, hostRectangle);

            dataTrigger.Comparison = ComparisonConditionType.Equal;
            Assert.AreEqual(stubAction.InvokeCount, 1, "The trigger should have been invoked once.");
        }

        [TestMethod]
        public void SetComparison_DoesNotSatisfyCondition_DoesNotCauseReevaluation()
        {
            DataTrigger dataTrigger = CreateDataTrigger(0, ComparisonConditionType.NotEqual, 0);
            Rectangle hostRectangle = CreateRectangle();
            StubAction stubAction = AttachActionToDataTrigger(dataTrigger, hostRectangle);

            dataTrigger.Comparison = ComparisonConditionType.LessThan;
            Assert.AreEqual(stubAction.InvokeCount, 0, "The trigger should not have been invoked.");
        }

        #endregion
    }
}
