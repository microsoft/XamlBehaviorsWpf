// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Shapes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Interactivity;
    using SysWindows = System.Windows;

    [TestClass]
    public class EventTriggerTest
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

        private SysWindows.RoutedEventArgs CreateClickEvent()
        {
            return new SysWindows.RoutedEventArgs(Button.ClickEvent);
        }

        private static Grid CreateGrid()
        {
            Grid grid = new Grid();
            SysWindows.NameScope.SetNameScope(grid, new SysWindows.NameScope());
            return grid;
        }

        private static EventObjectStub CreateEventObjectStub()
        {
            return new EventObjectStub();
        }

        private static EventTrigger CreateEventTrigger()
        {
            return new EventTrigger();
        }

        private static EventTrigger CreateEventTrigger(string eventName)
        {
            return new EventTrigger(eventName);
        }

        private static Button CreateButton()
        {
            Button button = new Button();
            return button;
        }

        private static ClrEventClassStub CreateClrEventClassStub()
        {
            return new ClrEventClassStub();
        }

        #endregion

        #region Helper methods and classes

        private static StubAction AttachTriggerToObject(EventTrigger eventTrigger, SysWindows.DependencyObject host)
        {
            StubAction eventAction = new StubAction();
            eventTrigger.Actions.Add(eventAction);
            eventTrigger.Attach(host);
            return eventAction;
        }

        private static void AddChildElement(Grid grid, SysWindows.FrameworkElement element)
        {
            grid.RegisterName(element.Name, element);
            grid.Children.Add(element);
        }

        private static T CreateNamedElement<T>(string name) where T : SysWindows.FrameworkElement, new()
        {
            T frameworkElement = new T()
            {
                Name = name
            };
            return frameworkElement;
        }

        private class ClrEventClassStub
        {
            public event EventHandler Event;
            public static readonly string EventName = "Event";

            public void Fire()
            {
                if (this.Event != null)
                {
                    this.Event(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Test methods

        [TestMethod]
        public void Constructor_DefaultConstructor_SetsEventNameToLoaded()
        {
            EventTrigger eventTrigger = new EventTrigger();
            Assert.IsTrue(eventTrigger.EventName.Equals("Loaded", StringComparison.Ordinal), "eventTrigger1.EventName is 'Loaded'.");
        }

        [TestMethod]
        public void Constructor_EventNameConstructor_SetsEventNameToSpecifiedValue()
        {
            EventTrigger eventTrigger = new EventTrigger("Click");
            Assert.AreEqual(eventTrigger.EventName, "Click", "eventTrigger.EventName == Click");
        }

        [TestMethod]
        public void EventFired_SimpleEvent_Fires()
        {
            EventTrigger eventTrigger = CreateEventTrigger("StubEvent");
            EventObjectStub eventStub = CreateEventObjectStub();
            StubAction eventAction = AttachTriggerToObject(eventTrigger, eventStub);

            eventStub.FireStubEvent();
            Assert.AreEqual(eventAction.InvokeCount, 1, "Action was invoked in response to event.");
        }

        [TestMethod]
        public void OnAttached_UnsetEventName_DoesNothing()
        {
            // attach an empty EventTrigger
            Button button = CreateButton();
            EventTrigger eventTrigger = CreateEventTrigger();
            AttachTriggerToObject(eventTrigger, button);
            Assert.AreEqual(((IAttachedObject)eventTrigger).AssociatedObject, button, "EventTrigger should be attached to button.");
        }

        [TestMethod]
        public void EventFired_EventFiredOnSourceNameObject_Fires()
        {
            EventTrigger eventTrigger = CreateEventTrigger();

            Grid grid = CreateGrid();
            Rectangle rect = CreateNamedElement<Rectangle>("rect");
            Button button = CreateNamedElement<Button>("button");
            AddChildElement(grid, rect);
            AddChildElement(grid, button);

            using (StubWindow window = new StubWindow(grid))
            {
                StubAction action = AttachTriggerToObject(eventTrigger, rect);
                eventTrigger.SourceName = "button";
                eventTrigger.EventName = "Click";
                System.Windows.RoutedEventArgs args = CreateClickEvent();

                button.RaiseEvent(args);
                Assert.AreEqual(action.InvokeCount, 1, "Click on button while Source=button should not invoke the action.");
            }
        }

        [TestMethod]
        public void EventFired_EventFiredOnAssociatedObjectWhenSourceNameObjectIsDifferent_DoesNotFire()
        {
            EventTrigger eventTrigger = CreateEventTrigger();

            Grid grid = CreateGrid();
            Rectangle rect = CreateNamedElement<Rectangle>("rect");
            Button button = CreateNamedElement<Button>("button");
            AddChildElement(grid, rect);
            AddChildElement(grid, button);

            using (StubWindow window = new StubWindow(grid))
            {
                StubAction action = AttachTriggerToObject(eventTrigger, rect);
                eventTrigger.SourceName = "button";
                eventTrigger.EventName = "Click";
                System.Windows.RoutedEventArgs args = CreateClickEvent();

                rect.RaiseEvent(args);
                Assert.AreEqual(action.InvokeCount, 0, "Click on rect while Source=button should not invoke the action.");
            }
        }

        [TestMethod]
        public void SourceObjectEventFired_EventNameMatchesFiredEvent_Fires()
        {
            Button host = CreateButton();
            EventObjectStub eventSource = CreateEventObjectStub();
            EventTrigger eventTrigger = CreateEventTrigger("StubEvent");
            StubAction action = AttachTriggerToObject(eventTrigger, host);

            eventTrigger.SourceObject = eventSource;
            eventSource.FireStubEvent();

            Assert.AreEqual(action.InvokeCount, 1, "Trigger should be invoked when source object fires the event it is listening to.");
        }

        [TestMethod]
        public void EventFired_OldEventFiredAfterEventNameChanged_DoesNotFires()
        {
            Button host = CreateButton();
            EventObjectStub eventSource = CreateEventObjectStub();
            EventTrigger eventTrigger = CreateEventTrigger("StubEvent");
            StubAction action = AttachTriggerToObject(eventTrigger, host);

            eventTrigger.SourceObject = eventSource;
            eventTrigger.EventName = "StubEvent2";
            eventSource.FireStubEvent();

            Assert.AreEqual(action.InvokeCount, 0, "Trigger should not be invoked when source object fires its event it is not listening to.");
        }

        [TestMethod]
        public void EventFired_NewEventFiredAfterEventNameChanged_Fires()
        {
            Button host = CreateButton();
            EventObjectStub eventSource = CreateEventObjectStub();
            EventTrigger eventTrigger = CreateEventTrigger("StubEvent");
            StubAction action = AttachTriggerToObject(eventTrigger, host);

            eventTrigger.SourceObject = eventSource;
            eventTrigger.EventName = "StubEvent2";
            eventSource.FireStubEvent2();

            Assert.AreEqual(action.InvokeCount, 1, "Trigger should be invoked when source object fires the event it is listening to.");
        }

        [TestMethod]
        public void EventFired_OldSourceObjectFiresEvent_DoesNotFire()
        {
            Button host = CreateButton();
            EventObjectStub oldEventSource = CreateEventObjectStub();
            EventObjectStub newEventSource = CreateEventObjectStub();
            EventTrigger eventTrigger = CreateEventTrigger("StubEvent");
            StubAction action = AttachTriggerToObject(eventTrigger, host);

            eventTrigger.SourceObject = oldEventSource;
            eventTrigger.SourceObject = newEventSource;
            oldEventSource.FireStubEvent();

            Assert.AreEqual(action.InvokeCount, 0, "Trigger should not be invoked when an old source object fires the event it is listening to.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EventNameSet_NonExistantEventWithSetSourceObject_ThrowsArgumentException()
        {
            EventObjectStub eventSource = CreateEventObjectStub();
            EventTrigger eventTrigger = CreateEventTrigger();
            AttachTriggerToObject(eventTrigger, eventSource);

            eventTrigger.SourceObject = eventSource;
            eventTrigger.EventName = "FooEvent";
        }

        [TestMethod]
        public void EventNameSet_NonExistantEventWithNoSourceObject_DoesNothing()
        {
            EventTrigger eventTrigger = CreateEventTrigger();
            EventObjectStub eventStub = CreateEventObjectStub();
            AttachTriggerToObject(eventTrigger, eventStub);
            eventTrigger.EventName = "FooEvent";
            // With no source object set, non-existant event did not throw.
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EventNameSet_IncompatibleEventSignatureWithSetSourceObject_ThrowsArgumentException()
        {
            EventObjectStub eventSource = CreateEventObjectStub();
            EventTrigger eventTrigger = CreateEventTrigger();

            AttachTriggerToObject(eventTrigger, eventSource);
            eventTrigger.SourceObject = eventSource;
            eventTrigger.EventName = "IntEvent";
        }

        [TestMethod]
        public void EventNameSet_IncompatibleEventSignatureWithNoSourceObject_DoesNothing()
        {
            EventTrigger eventTrigger = CreateEventTrigger();
            EventObjectStub eventStub = CreateEventObjectStub();
            AttachTriggerToObject(eventTrigger, eventStub);
            eventTrigger.EventName = "IntEvent";
            // With no source object set, non-existant event did not throw.
        }

        [TestMethod]
        public void NonDependencyObjectEventSource_EventFired_InvokesActions()
        {
            EventTrigger eventTrigger = CreateEventTrigger();
            ClrEventClassStub clrEventClass = CreateClrEventClassStub();
            Button host = CreateButton();

            eventTrigger.SourceObject = clrEventClass;
            eventTrigger.EventName = ClrEventClassStub.EventName;
            StubAction action = AttachTriggerToObject(eventTrigger, host);

            clrEventClass.Fire();
            Assert.AreEqual(action.InvokeCount, 1, "Event firing on a source CLR object should fire the EventTrigger.");
        }

        [TestMethod]
        public void EventFired_SourceObjectAndSourceNameSet_ListensOnlyToSourceObject()
        {
            EventTrigger eventTrigger = CreateEventTrigger();

            Grid grid = CreateGrid();
            Rectangle host = CreateNamedElement<Rectangle>("rect");
            Button sourceButton = CreateNamedElement<Button>("sourcebutton");
            Button sourceNameButton = CreateNamedElement<Button>("sourceNameButton");
            AddChildElement(grid, host);
            AddChildElement(grid, sourceButton);
            AddChildElement(grid, sourceNameButton);

            using (StubWindow window = new StubWindow(grid))
            {
                StubAction action = AttachTriggerToObject(eventTrigger, host);
                eventTrigger.EventName = "Click";
                eventTrigger.SourceName = "sourceNameButton";
                eventTrigger.SourceObject = sourceButton;
                System.Windows.RoutedEventArgs args = CreateClickEvent();

                sourceNameButton.RaiseEvent(args);
                Assert.AreEqual(action.InvokeCount, 0, "Click on source name button when SourceObject is set to another object should not invoke the action.");
                sourceButton.RaiseEvent(args);
                Assert.AreEqual(action.InvokeCount, 1, "Click on source object button should invoke the action.");
            }
        }

        [TestMethod]
        public void EventFired_SourceObjectChangesToNull_ListensToSourceName()
        {
            EventTrigger eventTrigger = CreateEventTrigger();

            Grid grid = CreateGrid();
            Rectangle host = CreateNamedElement<Rectangle>("rect");
            Button sourceButton = CreateNamedElement<Button>("sourcebutton");
            Button sourceNameButton = CreateNamedElement<Button>("sourceNameButton");
            AddChildElement(grid, host);
            AddChildElement(grid, sourceButton);
            AddChildElement(grid, sourceNameButton);

            using (StubWindow window = new StubWindow(grid))
            {
                StubAction action = AttachTriggerToObject(eventTrigger, host);
                eventTrigger.SourceObject = sourceButton;
                eventTrigger.SourceName = "sourceNameButton";
                eventTrigger.EventName = "Click";
                eventTrigger.ClearValue(EventTriggerBase.SourceObjectProperty);
                System.Windows.RoutedEventArgs args = CreateClickEvent();

                sourceButton.RaiseEvent(args);
                Assert.AreEqual(action.InvokeCount, 0, "Click on source object button should not invoke the action, as the SourceObject has been cleared.");
                sourceNameButton.RaiseEvent(args);
                Assert.AreEqual(action.InvokeCount, 1, "Click on source name button when SourceObject has been cleared should invoke the action.");
            }
        }

        [TestMethod]
        public void TestLoadedEvent()
        {
            Rectangle rectangle = new Rectangle();
            EventTrigger eventTrigger1 = new EventTrigger("Loaded");
            EventTrigger eventTrigger2 = new EventTrigger("MouseLeftButtonDown");
            StubAction loadedAction = new StubAction();
            StubAction mouseDownAction = new StubAction();

            eventTrigger1.Actions.Add(loadedAction);
            eventTrigger1.Attach(rectangle);

            using (StubWindow window = new StubWindow(rectangle))
            {
                Assert.AreEqual(loadedAction.InvokeCount, 1, "Loaded action was invoked");
                eventTrigger1.EventName = "GotMouseCapture";
                rectangle.CaptureMouse();
                Assert.AreEqual(loadedAction.InvokeCount, 2, "GotMouseCapture action was invoked");
                rectangle.ReleaseMouseCapture();
                eventTrigger1.EventName = "Loaded";
                rectangle.CaptureMouse();
                Assert.AreEqual(loadedAction.InvokeCount, 2, "GotMouseCapture action was not invoked");
                rectangle.ReleaseMouseCapture();
            }
            eventTrigger1.Detach();
            Assert.IsNull(((IAttachedObject)eventTrigger1).AssociatedObject, "Trigger was detached");
        }

        #endregion
    }
}