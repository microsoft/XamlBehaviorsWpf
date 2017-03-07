// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System;
    using System.Windows.Input;
    using System.Windows.Shapes;
    using Microsoft.Xaml.Interactions.Core;
    using Microsoft.Xaml.Interactivity;
    using SysWindows = System.Windows;

    public sealed class SingleConstructorArgumentTrigger : TriggerBase<System.Windows.Controls.Button>
    {
        public SingleConstructorArgumentTrigger(string s)
        {
        }
    }

    public sealed class StubEventTriggerBase : EventTriggerBase<System.Windows.DependencyObject>
    {
        public string EventName { get; set; }

        protected override string GetEventName()
        {
            return this.EventName;
        }
    }

    public sealed class EventObjectStub : SysWindows.DependencyObject
    {
        public delegate void IntEventHandler(int i);

        public event EventHandler StubEvent;
        public event EventHandler StubEvent2;
        public event IntEventHandler IntEvent;

        public void FireStubEvent()
        {
            if (this.StubEvent != null)
            {
                this.StubEvent(this, new EventArgs());
            }
        }

        public void FireStubEvent2()
        {
            if (this.StubEvent2 != null)
            {
                this.StubEvent2(this, new EventArgs());
            }
        }

        public void FireIntEvent()
        {
            if (this.IntEvent != null)
            {
                this.IntEvent(0);
            }
        }
    }

    public sealed class ParameterTriggerAction : StubAction
    {
        public object Parameter { get; set; }

        protected override void Invoke(object parameter)
        {
            base.Invoke(parameter);
            this.Parameter = parameter;
        }
    }

    public sealed class BindingTrigger : StubTrigger
    {
        public static readonly SysWindows.DependencyProperty TestProperty = SysWindows.DependencyProperty.Register("Test", typeof(object), typeof(BindingTrigger));

        public object Test
        {
            get
            {
                return (object)this.GetValue(TestProperty);
            }
            set
            {
                this.SetValue(TestProperty, value);
            }
        }

        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new BindingTrigger();
        }
    }

    public class StubTrigger : StubTrigger<SysWindows.DependencyObject>
    {
        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new StubTrigger();
        }

        protected override string GetEventName()
        {
            return string.Empty;
        }
    }

    public abstract class StubTrigger<T> : EventTriggerBase<T> where T : SysWindows.DependencyObject
    {
        public T HostObject
        {
            get { return (T)base.AssociatedObject; }
        }

        private T firstHost;

        public bool AddedHost { get; set; }
        public bool ChangedHost { get; set; }
        public bool RemovedHost { get; set; }

        public void FireStubTrigger()
        {
            this.FireStubTrigger(null);
        }

        public virtual void FireStubTrigger(object parameter)
        {
            this.InvokeActions(parameter);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AddedHost = true;
            if (this.firstHost == null)
            {
                this.firstHost = (T)this.AssociatedObject;
            }
            else if (!this.AssociatedObject.Equals(this.firstHost))
            {
                this.ChangedHost = true;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.RemovedHost = true;
        }

        public SysWindows.Freezable GetCreateInstanceCore()
        {
            return this.CreateInstanceCore();
        }
    }

    public class BindingAction : TriggerAction<System.Windows.DependencyObject>
    {
        public static readonly SysWindows.DependencyProperty BindingObjectProperty = SysWindows.DependencyProperty.Register("BindingObject",
                                                                                                                                typeof(object),
                                                                                                                                typeof(BindingAction));
        public object BindingObject
        {
            get
            {
                return this.GetValue(BindingObjectProperty);
            }
            set
            {
                this.SetValue(BindingObjectProperty, value);
            }
        }

        protected override void Invoke(object parameter)
        {

        }

        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new BindingAction();
        }
    }

    public class TimedAction : TriggerAction<System.Windows.DependencyObject>
    {
        private static int invokeToken = 0;

        private int order;
        public int Order
        {
            get { return this.order; }
        }

        protected override void Invoke(object parameter)
        {
            this.order = TimedAction.invokeToken++;
        }

        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new TimedAction();
        }
    }

    public class StubAction : TriggerAction<System.Windows.DependencyObject>
    {
        private int invokeCount;
        public int InvokeCount
        {
            get { return this.invokeCount; }
        }

        public void StubInvoke()
        {
            this.Invoke(null);
        }

        protected override void Invoke(object parameter)
        {
            ++this.invokeCount;
        }

        public SysWindows.Freezable GetCreateInstanceCore()
        {
            return this.CreateInstanceCore();
        }
    }

    public sealed class StubTargetedTriggerAction : TargetedTriggerAction<System.Windows.DependencyObject>
    {
        public int TargetChangedCount
        {
            get;
            private set;
        }

        public int InvocationCount
        {
            get;
            private set;
        }

        public new SysWindows.DependencyObject Target
        {
            get { return base.Target; }
        }

        protected override void OnTargetChanged(System.Windows.DependencyObject oldTarget, System.Windows.DependencyObject newTarget)
        {
            base.OnTargetChanged(oldTarget, newTarget);
            this.TargetChangedCount++;
        }

        protected override void Invoke(object parameter)
        {
            this.InvocationCount++;
        }
    }

    public delegate void DelegateCommand();

    public class StubDelegateCommand : ICommand
    {
        private DelegateCommand delegateCommand;

        public StubDelegateCommand(DelegateCommand delegateCommand)
        {
            this.delegateCommand = delegateCommand;
        }

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        private EventHandler CanExecuteChangedHandler;

        event EventHandler ICommand.CanExecuteChanged
        {
            add
            {
                lock (this.CanExecuteChangedHandler)
                {
                    this.CanExecuteChangedHandler += value;
                }
            }
            remove
            {
                lock (this.CanExecuteChangedHandler)
                {
                    this.CanExecuteChangedHandler -= value;
                }
            }
        }

        public void Execute(object parameter)
        {
            this.delegateCommand();
        }

        #endregion
    }

    public class StubBehavior : Behavior<SysWindows.DependencyObject>
    {
        public ICommand StubCommand
        {
            get;
            private set;
        }

        public ICommand StubCommandWithParameter
        {
            get;
            private set;
        }

        public int ExecutionCount
        {
            get;
            private set;
        }

        private object lastParameter;
        public object LastParameter
        {
            get { return this.lastParameter; }
        }

        public StubBehavior()
        {
            this.StubCommand = new ActionCommand(this.ExecuteStub);
            this.StubCommandWithParameter = new ActionCommand(new Action<object>(this.ExecuteWithParameterStub));
        }

        private void ExecuteStub()
        {
            this.ExecutionCount++;
        }

        private void ExecuteWithParameterStub(object parameter)
        {
            this.ExecutionCount++;
            this.lastParameter = parameter;
        }

        public SysWindows.DependencyObject AttachedObject
        {
            get
            {
                return this.AssociatedObject;
            }
        }

        public SysWindows.Freezable GetCreateInstanceCore()
        {
            return this.CreateInstanceCore();
        }
    }

    public sealed class StubRectangleBehavior : Behavior<System.Windows.Shapes.Rectangle>
    {
    }

    public sealed class StubButtonTrigger : TriggerBase<System.Windows.Controls.Button>
    {
        public System.Windows.Controls.Button AttachedButton
        {
            get
            {
                return base.AssociatedObject;
            }
        }

        public System.Windows.Freezable CreateInstaceCoreStub()
        {
            return this.CreateInstanceCore();
        }
    }

    public sealed class StubButtonTriggerAction : TriggerAction<System.Windows.Controls.Button>
    {
        protected override void Invoke(object parameter)
        {
        }
    }

    public sealed class StubButtonEventTriggerBaseWithoutAttribute : EventTriggerBase<System.Windows.Controls.Button>
    {
        protected override string GetEventName()
        {
            return string.Empty;
        }
    }

    [TypeConstraint(typeof(Rectangle))]
    public sealed class StubButtonEventTriggerBaseWithAttribute : EventTriggerBase<System.Windows.Controls.Button>
    {
        protected override string GetEventName()
        {
            return string.Empty;
        }
    }

    public sealed class StubButtonTargetedTriggerActionWithoutAttribute : TargetedTriggerAction<System.Windows.Controls.Button>
    {
        protected override void Invoke(object parameter)
        {
        }
    }

    [TypeConstraint(typeof(Rectangle))]
    public sealed class StubButtonTargetedTriggerActionWithAttribute : TargetedTriggerAction<System.Windows.Controls.Button>
    {
        protected override void Invoke(object parameter)
        {
        }
    }
}