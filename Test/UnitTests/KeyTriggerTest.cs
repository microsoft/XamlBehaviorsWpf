using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Microsoft.Xaml.Behaviors.Input;
using System.Windows.Interop;
using System;

namespace Microsoft.Xaml.Interactions.UnitTests
{
    [TestClass]
    public class KeyTriggerTest
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
        [DataRow(Key.A)]
        [DataRow(Key.B)]
        [DataRow(Key.C)]
        [DataRow(Key.D)]
        [DataRow(Key.E)]
        [DataRow(Key.F)]
        [DataRow(Key.G)]
        [DataRow(Key.H)]
        [DataRow(Key.I)]
        [DataRow(Key.J)]
        [DataRow(Key.K)]
        [DataRow(Key.L)]
        [DataRow(Key.M)]
        [DataRow(Key.N)]
        [DataRow(Key.O)]
        [DataRow(Key.P)]
        [DataRow(Key.Q)]
        [DataRow(Key.R)]
        [DataRow(Key.S)]
        [DataRow(Key.T)]
        [DataRow(Key.U)]
        [DataRow(Key.V)]
        [DataRow(Key.W)]
        [DataRow(Key.X)]
        [DataRow(Key.Y)]
        [DataRow(Key.Z)]
        [DataRow(Key.NumPad1)]
        [DataRow(Key.NumPad2)]
        [DataRow(Key.NumPad3)]
        [DataRow(Key.NumPad4)]
        [DataRow(Key.NumPad5)]
        [DataRow(Key.NumPad6)]
        [DataRow(Key.NumPad7)]
        [DataRow(Key.NumPad8)]
        [DataRow(Key.NumPad9)]
        [DataRow(Key.Enter)]
        [DataRow(Key.Tab)]
        [DataRow(Key.LeftCtrl)]
        [DataRow(Key.RightCtrl)]
        [DataRow(Key.LeftAlt)]
        [DataRow(Key.RightAlt)]
        [DataRow(Key.System)]
        [DataRow(Key.LeftShift)]
        [DataRow(Key.RightShift)]
        public void KeyTrigger_InvokesActions_WhenKeyIsPressed(Key key)
        {
            var textBox = new TextBox();
            var keyTrigger = new KeyTrigger { Key = key };
            var action = new StubAction();
            keyTrigger.Actions.Add(action);
            keyTrigger.Attach(textBox);

            Grid grid = new Grid();
            grid.Children.Add(textBox);
            using (StubWindow window = new StubWindow(grid))
            {
                var inputSource = PresentationSource.FromVisual(textBox) ?? new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero);
                var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, inputSource, 0, key);
                keyEventArgs.RoutedEvent = Keyboard.KeyDownEvent;
                textBox.RaiseEvent(keyEventArgs);

                Assert.AreEqual(1, action.InvokeCount);
            }
        }

        [TestMethod]
        [DataRow(Key.A)]
        [DataRow(Key.B)]
        [DataRow(Key.C)]
        [DataRow(Key.D)]
        [DataRow(Key.E)]
        [DataRow(Key.F)]
        [DataRow(Key.G)]
        [DataRow(Key.H)]
        [DataRow(Key.I)]
        [DataRow(Key.J)]
        [DataRow(Key.K)]
        [DataRow(Key.L)]
        [DataRow(Key.M)]
        [DataRow(Key.N)]
        [DataRow(Key.O)]
        [DataRow(Key.P)]
        [DataRow(Key.Q)]
        [DataRow(Key.R)]
        [DataRow(Key.S)]
        [DataRow(Key.T)]
        [DataRow(Key.U)]
        [DataRow(Key.V)]
        [DataRow(Key.W)]
        [DataRow(Key.X)]
        [DataRow(Key.Y)]
        [DataRow(Key.Z)]
        [DataRow(Key.NumPad1)]
        [DataRow(Key.NumPad2)]
        [DataRow(Key.NumPad3)]
        [DataRow(Key.NumPad4)]
        [DataRow(Key.NumPad5)]
        [DataRow(Key.NumPad6)]
        [DataRow(Key.NumPad7)]
        [DataRow(Key.NumPad8)]
        [DataRow(Key.NumPad9)]
        [DataRow(Key.Enter)]
        [DataRow(Key.Tab)]
        [DataRow(Key.LeftCtrl)]
        [DataRow(Key.RightCtrl)]
        [DataRow(Key.LeftAlt)]
        [DataRow(Key.RightAlt)]
        [DataRow(Key.System)]
        [DataRow(Key.LeftShift)]
        [DataRow(Key.RightShift)]
        public void KeyTrigger_DoesNotInvokeAction_WhenKeyIsPressed(Key key)
        {
            var textBox = new TextBox();
            var keyTrigger = new KeyTrigger { Key = key, Modifiers = ModifierKeys.Control }; //since we have a modifier key, the action should not be invoked
            var action = new StubAction();
            keyTrigger.Actions.Add(action);
            keyTrigger.Attach(textBox);

            Grid grid = new Grid();
            grid.Children.Add(textBox);
            using (StubWindow window = new StubWindow(grid))
            {
                var inputSource = PresentationSource.FromVisual(textBox) ?? new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero);
                var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, inputSource, 0, key);
                keyEventArgs.RoutedEvent = Keyboard.KeyDownEvent;
                textBox.RaiseEvent(keyEventArgs);

                Assert.AreEqual(0, action.InvokeCount);
            }
        }

        [TestMethod]
        [DataRow(Key.A)]
        [DataRow(Key.B)]
        [DataRow(Key.C)]
        [DataRow(Key.D)]
        [DataRow(Key.E)]
        [DataRow(Key.F)]
        [DataRow(Key.G)]
        [DataRow(Key.H)]
        [DataRow(Key.I)]
        [DataRow(Key.J)]
        [DataRow(Key.K)]
        [DataRow(Key.L)]
        [DataRow(Key.M)]
        [DataRow(Key.N)]
        [DataRow(Key.O)]
        [DataRow(Key.P)]
        [DataRow(Key.Q)]
        [DataRow(Key.R)]
        [DataRow(Key.S)]
        [DataRow(Key.T)]
        [DataRow(Key.U)]
        [DataRow(Key.V)]
        [DataRow(Key.W)]
        [DataRow(Key.X)]
        [DataRow(Key.Y)]
        [DataRow(Key.Z)]
        [DataRow(Key.NumPad1)]
        [DataRow(Key.NumPad2)]
        [DataRow(Key.NumPad3)]
        [DataRow(Key.NumPad4)]
        [DataRow(Key.NumPad5)]
        [DataRow(Key.NumPad6)]
        [DataRow(Key.NumPad7)]
        [DataRow(Key.NumPad8)]
        [DataRow(Key.NumPad9)]
        [DataRow(Key.Enter)]
        [DataRow(Key.Tab)]
        [DataRow(Key.LeftCtrl)]
        [DataRow(Key.RightCtrl)]
        [DataRow(Key.LeftAlt)]
        [DataRow(Key.RightAlt)]
        [DataRow(Key.System)]
        [DataRow(Key.LeftShift)]
        [DataRow(Key.RightShift)]
        public void KeyTrigger_InvokesActions_WhenKeyIsReleased(Key key)
        {
            var textBox = new TextBox();
            var keyTrigger = new KeyTrigger { Key = key, FiredOn = KeyTriggerFiredOn.KeyUp };
            var action = new StubAction();
            keyTrigger.Actions.Add(action);
            keyTrigger.Attach(textBox);

            Grid grid = new Grid();
            grid.Children.Add(textBox);
            using (StubWindow window = new StubWindow(grid))
            {
                var inputSource = PresentationSource.FromVisual(textBox) ?? new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero);
                var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, inputSource, 0, key);
                keyEventArgs.RoutedEvent = Keyboard.KeyUpEvent;
                textBox.RaiseEvent(keyEventArgs);

                Assert.AreEqual(1, action.InvokeCount);
            }
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void KeyTrigger_InvokesActionsOnce_WhenLoadedEventFiredMultipleTimes(bool activeOnFocus)
        {
            var textBox = new TextBox();
            var keyTrigger = new KeyTrigger { ActiveOnFocus = activeOnFocus, Key = Key.Enter };
            var action = new StubAction();
            keyTrigger.Actions.Add(action);
            keyTrigger.Attach(textBox);

            Grid grid = new Grid();
            grid.Children.Add(textBox);
            using (StubWindow window = new StubWindow(grid))
            {
                //simulate the loaded event being invoked multiple times; for example, when using an element in a tab control
                textBox.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent));

                var inputSource = PresentationSource.FromVisual(textBox) ?? new HwndSource(0, 0, 0, 0, 0, "", IntPtr.Zero);
                var keyEventArgs = new KeyEventArgs(Keyboard.PrimaryDevice, inputSource, 0, Key.Enter);
                keyEventArgs.RoutedEvent = Keyboard.KeyDownEvent;
                textBox.RaiseEvent(keyEventArgs);

                Assert.AreEqual(1, action.InvokeCount);
            }
        }

        #endregion
    }
}
