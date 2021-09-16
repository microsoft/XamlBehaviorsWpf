// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using TriggerAction = Microsoft.Xaml.Behaviors.TriggerAction;
using TriggerBase = Microsoft.Xaml.Behaviors.TriggerBase;

namespace Microsoft.Xaml.Interactions.UnitTests
{
    [TestClass]
    public class ChangePropertyActionTests
    {
        #region Setup and Teardown

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

        private Rectangle CreateEmptyRectangle()
        {
            return new Rectangle();
        }

        private static ChangePropertyAction CreateChangePropertyAction()
        {
            return new ChangePropertyAction();
        }

        private ChangePropertyAction CreateChangePropertyAction(string propertyName, object value)
        {
            return new ChangePropertyAction { PropertyName = propertyName, Value = value, };
        }

        private static StubTrigger CreateStubTrigger()
        {
            StubTrigger trigger = new StubTrigger();
            return trigger;
        }

        private static Rectangle CreateRectangle()
        {
            Rectangle rectangle = new Rectangle { Fill = Brushes.Red, Stroke = Brushes.Purple };
            return rectangle;
        }

        private static Button CreateButton()
        {
            return new Button();
        }

        private AdditiveObjectStub CreateAdditiveObject()
        {
            return new AdditiveObjectStub();
        }

        private ChangePropertyActionTargetStub CreateTargetStub()
        {
            return new ChangePropertyActionTargetStub();
        }

        #endregion

        #region Helper methods and classes

        private static void AttachAction(DependencyObject dependencyObject, TriggerBase trigger, TriggerAction action)
        {
            trigger.Actions.Add(action);
            trigger.Attach(dependencyObject);
        }

        private class ChangePropertyActionTargetStub
        {
            public const string DoublePropertyName = "DoubleProperty";
            public const string StringPropertyName = "StringProperty";
            public const string ObjectPropertyName = "ObjectProperty";
            public const string AdditivePropertyName = "AdditiveProperty";
            public const string WriteOnlyPropertyName = "WriteOnlyProperty";

            public double DoubleProperty
            {
                get;
                set;
            }

            public string StringProperty
            {
                get;
                set;
            }

            public object ObjectProperty
            {
                get;
                set;
            }

            public AdditiveObjectStub AdditiveProperty
            {
                get;
                set;
            }

            public object WriteOnlyProperty
            {
                set
                {
                }
            }
        }

        private class AdditiveObjectStub
        {
            public AdditiveObjectStub()
            {
                this.Added = false;
            }

            public bool Added
            {
                get;
                private set;
            }

            public string AddType
            {
                get;
                private set;
            }

            public static AdditiveObjectStub operator +(AdditiveObjectStub a, AdditiveObjectStub b)
            {
                AdditiveObjectStub stub = new AdditiveObjectStub();
                stub.Added = true;
                stub.AddType = "AdditiveObjectStub";
                return stub;
            }

            public static AdditiveObjectStub operator +(AdditiveObjectStub a, int i)
            {
                AdditiveObjectStub stub = new AdditiveObjectStub();
                stub.Added = true;
                stub.AddType = "int";
                return stub;
            }

            public static AdditiveObjectStub operator +(AdditiveObjectStub a, double d)
            {
                AdditiveObjectStub stub = new AdditiveObjectStub();
                stub.Added = true;
                stub.AddType = "double";
                return stub;
            }

            public static AdditiveObjectStub operator +(AdditiveObjectStub a, Brush brush)
            {
                AdditiveObjectStub stub = new AdditiveObjectStub();
                stub.Added = true;
                stub.AddType = "Brush";
                return stub;
            }
        }

        #endregion

        #region Test methods

        [TestMethod]
        public void Invoke_NoPropertyName_IsNoOp()
        {
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction(null, Brushes.Green);
            trigger.Actions.Add(action);

            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_NotAttached_IsNoOp()
        {
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Foo", Brushes.Green);

            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_InvalidPropertyName_ThrowsArgumentException()
        {
            Rectangle rectangle = CreateRectangle();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Foo", Brushes.Green);
            AttachAction(rectangle, trigger, action);

            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_IncompatiblePropertyWithValueType_ThrowsArgumentException()
        {
            Rectangle rectangle = CreateRectangle();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Width", Brushes.Green);
            AttachAction(rectangle, trigger, action);

            trigger.FireStubTrigger();
        }

        [TestMethod]
#if NETCOREAPP
        [ExpectedException(typeof(ArgumentException))]
#else
        [ExpectedException(typeof(Exception))]
#endif
        public void Invoke_InvalidPropertyFormatString_ThrowsException()
        {
            Rectangle rectangle = CreateRectangle();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Width", "0.0.0.0");
            AttachAction(rectangle, trigger, action);

            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_ReadOnlyProperty_ThrowsArgumentException()
        {
            Button button = CreateButton();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("IsDefaulted", true);
            AttachAction(button, trigger, action);

            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_InaccessibleProperty_ThrowsArgumentException()
        {
            Button button = CreateButton();
            StubTrigger trigger = CreateStubTrigger();
            // EffectiveValuesInitialSize is an internal property on Button, and should be inaccessible
            ChangePropertyAction action = CreateChangePropertyAction("EffectiveValuesInitialSize", 10);
            AttachAction(button, trigger, action);

            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invoke_AnimateChangeOnNonDependencyProperty_ThrowsInvalidOperationException()
        {
            Button button = CreateButton();
            ChangePropertyActionTargetStub stubTarget = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.DoublePropertyName, 10);
            AttachAction(button, trigger, action);
            action.Duration = TimeSpan.FromMilliseconds(10);
            action.TargetObject = stubTarget;

            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void Constructor_ValueIsNull()
        {
            ChangePropertyAction action = new ChangePropertyAction();
            Assert.IsNull(action.Value, "action.Value == null");
        }

        [TestMethod]
        public void Constructor_PropertyNameIsNull()
        {
            ChangePropertyAction action = new ChangePropertyAction();
            Assert.IsTrue(string.IsNullOrEmpty(action.PropertyName), "action.PropertyName == \"\"");
        }

        [TestMethod]
        public void Invoke_RedFillToGreen_ChangesToGreen()
        {
            Rectangle rectangle = CreateRectangle();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Fill", Brushes.Green);

            AttachAction(rectangle, trigger, action);
            trigger.FireStubTrigger();

            Assert.AreEqual(rectangle.Fill, Brushes.Green, "rectangle.Fill == Green");
        }

        [TestMethod]
        public void Invoke_ChangePropertyFromFillToStroke_ChangesAppropriateProperty()
        {
            Rectangle rectangle = CreateRectangle();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Fill", Brushes.Green);

            AttachAction(rectangle, trigger, action);
            trigger.FireStubTrigger();

            action.PropertyName = "Stroke";
            trigger.FireStubTrigger();

            Assert.AreEqual(rectangle.Stroke, Brushes.Green, "rectangle.Stroke == Green");
        }

        [TestMethod]
        public void Invoke_ChangeValueFromGreenToYellow_ChangesToAppropriateValue()
        {
            Rectangle rectangle = CreateRectangle();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Fill", Brushes.Green);

            AttachAction(rectangle, trigger, action);
            trigger.FireStubTrigger();

            action.Value = Brushes.Yellow;
            trigger.FireStubTrigger();

            Assert.AreEqual(rectangle.Fill, Brushes.Yellow, "rectangle.Fill == Yellow");
        }

        [TestMethod]
        public void Invoke_AnimatedDoubleChange_Changes()
        {
            Ellipse e = new Ellipse();
            ChangePropertyAction c = CreateChangePropertyAction("Opacity", 0.0d);
            StubTrigger t = CreateStubTrigger();
            AttachAction(e, t, c);
            c.Duration = TimeSpan.FromMilliseconds(5);
            t.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_AnimatedPointChange_Changes()
        {
            Ellipse e = new Ellipse();
            ChangePropertyAction c = this.CreateChangePropertyAction("RenderTransformOrigin", new Point(0.3, 0.3));
            StubTrigger t = CreateStubTrigger();
            AttachAction(e, t, c);
            c.Duration = TimeSpan.FromMilliseconds(5);
            t.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_AnimatedColorChange_Changes()
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);
            ChangePropertyAction c = CreateChangePropertyAction("Color", Colors.Gray);
            StubTrigger t = CreateStubTrigger();
            AttachAction(brush, t, c);
            c.Duration = TimeSpan.FromMilliseconds(5);
            t.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_AnimatedObjectChange_Changes()
        {
            Ellipse e = new Ellipse();
            ChangePropertyAction c = CreateChangePropertyAction("Fill", Brushes.Gray);
            StubTrigger t = CreateStubTrigger();
            AttachAction(e, t, c);
            c.Duration = TimeSpan.FromMilliseconds(5);
            t.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_TargetObjectValueTypeProperty_SetsProperty()
        {
            double value = 10.0d;
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.DoublePropertyName, value);

            action.TargetObject = target;
            AttachAction(host, trigger, action);
            trigger.FireStubTrigger();

            Assert.AreEqual(target.DoubleProperty, value, "target.DoubleProperty should be 10 after Invoke");
        }

        [TestMethod]
        public void Invoke_TargetObjectReferenceTypeProperty_SetsProperty()
        {
            Button value = CreateButton();
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.ObjectPropertyName, value);

            action.TargetObject = target;
            AttachAction(host, trigger, action);
            trigger.FireStubTrigger();

            Assert.AreEqual(target.ObjectProperty, value,
                "target.ObjectProperty should point to the Button after Invoke");
        }

        [TestMethod]
        public void Invoke_IncrementByNull_SetsNullInstead()
        {
            ChangePropertyActionTargetStub target = CreateTargetStub();
            Button host = CreateButton();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.ObjectPropertyName, null);
            AttachAction(host, trigger, action);
            action.TargetObject = target;
            action.Increment = true;
            target.ObjectProperty = "not null";

            trigger.FireStubTrigger();
            Assert.IsNull(target.ObjectProperty,
                "Target.ObjectProperty should have been set to null, ignoring the increment.");
        }

        [TestMethod]
        public void Invoke_IncrementDoubleProperty_IncrementsProperty()
        {
            ChangePropertyActionTargetStub target = CreateTargetStub();
            Button host = CreateButton();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.DoublePropertyName, 10.5d);

            target.DoubleProperty = 10.0d;
            action.TargetObject = target;
            action.Increment = true;
            AttachAction(host, trigger, action);
            trigger.FireStubTrigger();

            Assert.AreEqual(target.DoubleProperty, 20.5d,
                "DoubleProperty should have been incremented by 10.5 to a total of 20.5");
        }

        [TestMethod]
        public void Invoke_IncrementStringProperty_AppendsStringValue()
        {
            ChangePropertyActionTargetStub target = CreateTargetStub();
            Button host = CreateButton();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.StringPropertyName, " World!");

            target.StringProperty = "Hello";
            action.TargetObject = target;
            action.Increment = true;
            AttachAction(host, trigger, action);
            trigger.FireStubTrigger();

            Assert.AreEqual(target.StringProperty, "Hello World!",
                "StringProperty should have been appended with 'World!', resulting in 'Hello World!'");
        }

        [TestMethod]
        public void Invoke_IncrementOpAdditionOverrideType_CallsOpAdditionOverride()
        {
            ChangePropertyActionTargetStub target = CreateTargetStub();
            Button host = CreateButton();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction(ChangePropertyActionTargetStub.ObjectPropertyName,
                CreateAdditiveObject());
            AdditiveObjectStub addObject = CreateAdditiveObject();

            target.ObjectProperty = addObject;
            action.TargetObject = target;
            action.Increment = true;
            AttachAction(host, trigger, action);
            trigger.FireStubTrigger();

            Assert.IsTrue(((AdditiveObjectStub)target.ObjectProperty).Added,
                "AddObject.op_Addition should have been called.");
        }

        [TestMethod]
        // todo: might already have this one
        public void Invoke_PropertyIsChanged_NewPropertyIsSet()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.ObjectPropertyName, 10.0d);

            action.TargetObject = target;
            AttachAction(host, trigger, action);
            action.PropertyName = "DoubleProperty";
            trigger.FireStubTrigger();

            Assert.AreEqual(target.DoubleProperty, 10.0d, "target.DoubleProperty should be 10 after Invoke");
        }

        [TestMethod]
        // todo: might already have this one
        public void Invoke_PropertyIsChanged_OldPropertyIsNotSet()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.ObjectPropertyName, 10.0d);

            action.TargetObject = target;
            AttachAction(host, trigger, action);
            action.PropertyName = "DoubleProperty";
            trigger.FireStubTrigger();

            Assert.IsNull(target.ObjectProperty, "target.DoubleProperty should be null after Invoke");
        }

        [TestMethod]
        // todo: might already have this one
        public void Invoke_ValueIsChanged_NewValueIsSet()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.ObjectPropertyName, 10.0d);

            action.TargetObject = target;
            AttachAction(host, trigger, action);
            action.Value = host;
            trigger.FireStubTrigger();

            Assert.AreEqual(target.ObjectProperty, host, "target.DoubleProperty should be host after Invoke");
        }

        [TestMethod]
        public void Invoke_IncrementIsSetToTrue_IncrementBehaviorIsInvoked()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.DoublePropertyName, 10.0d);

            // setting to 5 so we have an assurance that we didn't just increment twice
            target.DoubleProperty = 5.0d;
            action.TargetObject = target;
            AttachAction(host, trigger, action);

            trigger.FireStubTrigger();
            action.Increment = true;
            trigger.FireStubTrigger();

            Assert.AreEqual(target.DoubleProperty, 20.0d,
                "target.DoubleProperty should be 20 after Invoke, then incremental Invoke");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invoke_IncrementWriteOnlyProperty_ThrowsInvalidOperationException()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.WriteOnlyPropertyName, null);

            action.TargetObject = target;
            AttachAction(host, trigger, action);

            action.Increment = true;
            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Invoke_IncrementWithDuration_ThrowsInvalidOperationException()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Opacity", 0.5d);

            action.Increment = true;
            action.Duration = new Duration(TimeSpan.FromSeconds(1));
            AttachAction(host, trigger, action);

            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_IncrementWithMultipleAdditionOverrides_CallsCorrectOverride()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.ObjectPropertyName, 0.5d);

            target.ObjectProperty = CreateAdditiveObject();
            action.TargetObject = target;
            action.Increment = true;
            AttachAction(host, trigger, action);

            trigger.FireStubTrigger();

            Assert.AreEqual(((AdditiveObjectStub)target.ObjectProperty).AddType, "double",
                "Double addition override should have been called.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Invoke_IncrementWithAmbiguousAdditionOverride_ThrowsArgumentException()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.ObjectPropertyName, "#FF00FF00");

            target.ObjectProperty = CreateAdditiveObject();
            action.TargetObject = target;
            action.Increment = true;
            AttachAction(host, trigger, action);

            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_IncrementWithInvalidAdditionType_SetsValueInstead()
        {
            Rectangle host = CreateRectangle();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Fill", Brushes.Red);
            action.Increment = true;
            AttachAction(host, trigger, action);

            trigger.FireStubTrigger();
            Assert.AreEqual(host.Fill, Brushes.Red, "Rectangle.Fill should be Red, as Increment was not possible.");
        }

        [TestMethod]
        public void Invoke_IncrementUnsetNonAdditiveProperty_SetsPropertyToSpecifiedValue()
        {
            Rectangle host = CreateEmptyRectangle();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action = CreateChangePropertyAction("Fill", Brushes.Red);
            action.Increment = true;
            AttachAction(host, trigger, action);

            trigger.FireStubTrigger();
            Assert.AreEqual(host.Fill, Brushes.Red,
                "Rectangle's Fill should have been set to Red, ignoring the Increment.");
        }

        [TestMethod]
        public void Invoke_TargetObjectValueConflictingTypeProperty_AssignDoubleToString()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.StringPropertyName, 100.5d);
            AttachAction(host, trigger, action);

            action.TargetObject = target;
            trigger.FireStubTrigger();

            Assert.AreEqual(target.StringProperty, "100.5",
                "StringProperty should have been assigned 100.5 as a string");
        }

        [TestMethod]
        public void Invoke_TargetObjectValueConflictingTypeProperty_AssignIntToString()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                this.CreateChangePropertyAction(ChangePropertyActionTargetStub.StringPropertyName, -100);
            AttachAction(host, trigger, action);

            action.TargetObject = target;
            trigger.FireStubTrigger();

            Assert.AreEqual(target.StringProperty, "-100", "StringProperty should have been assigned -100 as a string");
        }

        [TestMethod]
        public void Invoke_TargetObjectValueConflictingTypeProperty_AssignUintToString()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.StringPropertyName, (uint)100);
            AttachAction(host, trigger, action);

            action.TargetObject = target;
            trigger.FireStubTrigger();

            Assert.AreEqual(target.StringProperty, "100", "StringProperty should have been assigned 100 as a string");
        }

        [TestMethod]
        public void Invoke_TargetObjectValueConflictingTypeProperty_AssignBoolToString()
        {
            Button host = CreateButton();
            ChangePropertyActionTargetStub target = CreateTargetStub();
            StubTrigger trigger = CreateStubTrigger();
            ChangePropertyAction action =
                CreateChangePropertyAction(ChangePropertyActionTargetStub.StringPropertyName, true);
            AttachAction(host, trigger, action);

            action.TargetObject = target;
            trigger.FireStubTrigger();

            Assert.AreEqual(target.StringProperty, "True", "StringProperty should have been assigned True as a string");
        }

        #endregion
    }
}
