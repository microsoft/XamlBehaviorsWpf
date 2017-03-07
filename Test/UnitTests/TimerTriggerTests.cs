// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Interactions.Core;
    using Microsoft.Xaml.Interactivity;

    [TestClass]
    public class TimerTriggerTests
    {
        [TestMethod]
        public void Tick_TickCountOfZero_TicksIndefinitely()
        {
            TimerTrigger trigger = this.CreateTimerTrigger(1, 0);
            CountAction action = this.MeasureTotalTicks(trigger, 400);
            Assert.AreNotEqual(action.Count, 0, "Timer trigger should fire continuously when TotalTicks <= 0.");
        }

        [TestMethod]
        public void Tick_TickCountOfFive_TicksFiveTimes()
        {
            TimerTrigger trigger = this.CreateTimerTrigger(1, 5);
            CountAction action = this.MeasureTotalTicks(trigger, 400);
            Assert.AreEqual(action.Count, 5, "Timer trigger should fire exactly TotalTicks times when TotalTicks > 0.");
        }

        private TimerTrigger CreateTimerTrigger(int millisecondsPerTick, int totalTicks)
        {
            return new TimerTrigger()
            {
                MillisecondsPerTick = millisecondsPerTick,
                TotalTicks = totalTicks
            };
        }

        private CountAction MeasureTotalTicks(TimerTrigger trigger, int msToRun)
        {
            CountAction action = new CountAction();

            trigger.Actions.Add(action);
            trigger.StartTimer();

            DateTime currentTime = DateTime.Now;
            DateTime stopTime = currentTime.AddMilliseconds(msToRun);
            while (DateTime.Now.CompareTo(stopTime) < 0)
            {
                DispatcherHelper.ClearFrames(Dispatcher.CurrentDispatcher);
            }
            return action;
        }

        public class CountAction : TriggerAction<DependencyObject>
        {
            public int Count
            {
                get;
                set;
            }

            protected override void Invoke(object parameter)
            {
                this.Count++;
            }
        }
    }
}
