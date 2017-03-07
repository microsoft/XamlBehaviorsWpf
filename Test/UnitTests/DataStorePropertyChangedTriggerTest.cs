// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Interactions.Core;
    using Microsoft.Xaml.Interactivity;

    internal class StubDataStore : INotifyPropertyChanged
    {
        private string foo;
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public string Foo
        {
            get
            {
                return this.foo;
            }
            set
            {
                if (this.foo != value)
                {
                    this.foo = value;
                    this.OnPropertyChanged("Foo");
                }
            }
        }
    }

    [TestClass]
    public sealed class DataStorePropertyChangedTriggerTest
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

        private StubDataStore CreateDataStore()
        {
            return new StubDataStore();
        }

        private StubAction CreateStubAction()
        {
            return new StubAction();
        }

        private TriggerType CreateTrigger<TriggerType>(object bindingValue, string propertyName) where TriggerType : DependencyObject, new()
        {
            Binding binding = new Binding();
            binding.Source = bindingValue;
            binding.Path = new PropertyPath(propertyName);

            TriggerType trigger = new TriggerType();
            BindingOperations.SetBinding(trigger, PropertyChangedTrigger.BindingProperty, binding);
            return trigger;
        }

        private void Test_SetBindingValue_ValueChanged(TriggerBase<DependencyObject> trigger, StubAction stubAction, StubDataStore dataStore)
        {
            using (StubWindow window = new StubWindow(null))
            {
                // Change the value on the data store. 
                dataStore.Foo = "foo";
                // Force the Data binding phase
                DispatcherHelper.ForceDataBinding();
                Assert.AreEqual(stubAction.InvokeCount, 1, "The trigger should have been invoked once.");
            }
        }

        private void Test_SetBindingValue_ValueChangedWithSameValue(TriggerBase<DependencyObject> trigger, StubAction stubAction, StubDataStore dataStore)
        {
            using (StubWindow window = new StubWindow(null))
            {
                // Change the value on the data store. 
                dataStore.Foo = "foo";
                // Force the Data binding phase
                DispatcherHelper.ForceDataBinding();
                // Assigned same value
                dataStore.Foo = "foo";
                // Force the Data binding phase
                DispatcherHelper.ForceDataBinding();
                Assert.AreEqual(stubAction.InvokeCount, 1, "The trigger should have been invoked once.");
            }
        }

        private void Test_SetBindingValue_ValueChangedTwice(TriggerBase<DependencyObject> trigger, StubAction stubAction, StubDataStore dataStore)
        {
            using (StubWindow window = new StubWindow(null))
            {
                // Change the value on the data store. 
                dataStore.Foo = "foo";
                // Force the Data binding phase
                DispatcherHelper.ForceDataBinding();
                // Change the to a different value
                dataStore.Foo = "bar";
                // Force the Data binding phase
                DispatcherHelper.ForceDataBinding();
                Assert.AreEqual(stubAction.InvokeCount, 2, "The trigger should have been invoked twice.");
            }
        }
        #endregion

        #region Test methods

        [TestMethod]
        public void DataStoreChangeTrigger_SetBindingValue_ValueChanged()
        {
            StubDataStore dataStore = this.CreateDataStore();
            DataStoreChangedTrigger trigger = CreateTrigger<DataStoreChangedTrigger>(dataStore, "Foo");
            StubAction stubAction = CreateStubAction();
            trigger.Actions.Add(stubAction);

            this.Test_SetBindingValue_ValueChanged(trigger, stubAction, dataStore);
        }

        [TestMethod]
        public void DataStoreChangeTrigger_SetBindingValue_ValueChangedWithSameValue()
        {
            StubDataStore dataStore = this.CreateDataStore();
            DataStoreChangedTrigger trigger = CreateTrigger<DataStoreChangedTrigger>(dataStore, "Foo");
            StubAction stubAction = CreateStubAction();
            trigger.Actions.Add(stubAction);

            this.Test_SetBindingValue_ValueChangedWithSameValue(trigger, stubAction, dataStore);
        }

        [TestMethod]
        public void DataStoreChangeTrigger_SetBindingValue_ValueChangedTwice()
        {
            StubDataStore dataStore = this.CreateDataStore();
            DataStoreChangedTrigger trigger = CreateTrigger<DataStoreChangedTrigger>(dataStore, "Foo");
            StubAction stubAction = CreateStubAction();
            trigger.Actions.Add(stubAction);

            this.Test_SetBindingValue_ValueChangedTwice(trigger, stubAction, dataStore);
        }

        [TestMethod]
        public void PropertyChangedTrigger_SetBindingValue_ValueChanged()
        {
            StubDataStore dataStore = this.CreateDataStore();
            PropertyChangedTrigger trigger = this.CreateTrigger<PropertyChangedTrigger>(dataStore, "Foo");
            StubAction stubAction = CreateStubAction();
            trigger.Actions.Add(stubAction);

            this.Test_SetBindingValue_ValueChanged(trigger, stubAction, dataStore);
        }

        [TestMethod]
        public void PropertyChangedTrigger_SetBindingValue_ValueChangedWithSameValue()
        {
            StubDataStore dataStore = this.CreateDataStore();
            PropertyChangedTrigger trigger = CreateTrigger<PropertyChangedTrigger>(dataStore, "Foo");
            StubAction stubAction = CreateStubAction();
            trigger.Actions.Add(stubAction);

            this.Test_SetBindingValue_ValueChangedWithSameValue(trigger, stubAction, dataStore);
        }

        [TestMethod]
        public void PropertyChangedTrigger_SetBindingValue_ValueChangedTwice()
        {
            StubDataStore dataStore = this.CreateDataStore();
            PropertyChangedTrigger trigger = CreateTrigger<PropertyChangedTrigger>(dataStore, "Foo");
            StubAction stubAction = CreateStubAction();
            trigger.Actions.Add(stubAction);

            this.Test_SetBindingValue_ValueChangedTwice(trigger, stubAction, dataStore);
        }
        #endregion
    }
}
