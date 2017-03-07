// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Shapes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Interactivity;
    using Microsoft.Xaml.Interactions.Core;

    [TestClass]
    public class RemoveElementActionTests
    {
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

        private StubTrigger SetupRemoveAction(DependencyObject target)
        {
            StubTrigger trigger = new StubTrigger();
            RemoveElementAction action = new RemoveElementAction();
            trigger.Actions.Add(action);

            trigger.Attach(target);
            return trigger;
        }

        [TestMethod]
        public void RemoveFromPanelTest()
        {
            // Panel test
            Rectangle rectangle = new Rectangle();
            Canvas canvas = new Canvas();
            canvas.Children.Add(rectangle);
            StubTrigger trigger = SetupRemoveAction(rectangle);

            trigger.FireStubTrigger();
            Assert.IsNull(rectangle.Parent, "rectangle has no parent");
            Assert.AreEqual(canvas.Children.Count, 0, "canvas has no children");

            // A second removal should not thrown an exception.
            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void RemoveFromContentControlTest()
        {
            Rectangle rectangle = new Rectangle();
            StubTrigger trigger = SetupRemoveAction(rectangle);

            ContentControl contentControl = new ContentControl();
            contentControl.Content = rectangle;
            trigger.FireStubTrigger();
            Assert.IsNull(rectangle.Parent, "rectangle has no parent");
            Assert.IsNull(contentControl.Content, "contentControl has no content");

            // A second removal should not throw an exception.
            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void RemoveFromPageTest()
        {
            Rectangle rectangle = new Rectangle();
            StubTrigger trigger = SetupRemoveAction(rectangle);

            Page page = new Page();
            page.Content = rectangle;
            trigger.FireStubTrigger();
            Assert.IsNull(rectangle.Parent, "rectangle has no parent");
            Assert.IsNull(page.Content, "page has no content");

            // A second removal should not throw an exception.
            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void RemoveFromDecoratorTest()
        {
            Rectangle rectangle = new Rectangle();
            StubTrigger trigger = SetupRemoveAction(rectangle);

            Decorator decorator = new Decorator();
            decorator.Child = rectangle;
            trigger.FireStubTrigger();
            Assert.IsNull(rectangle.Parent, "rectangle has no parent");
            Assert.IsNull(decorator.Child, "page has no content");

            // A second removal should not throw an exception.
            trigger.FireStubTrigger();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveFromInvalidComponent()
        {
            Rectangle rectangle = new Rectangle();
            StubTrigger trigger = SetupRemoveAction(rectangle);

            RichTextBox textBox = new RichTextBox();
            textBox.Document.Blocks.Add(new BlockUIContainer(rectangle));
            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void RemoveReadd()
        {
            Rectangle rectangle = new Rectangle();
            StubTrigger trigger = SetupRemoveAction(rectangle);
            Canvas canvas = new Canvas();
            canvas.Children.Add(rectangle);
            trigger.FireStubTrigger();

            ContentControl contentControl = new ContentControl();
            contentControl.Content = rectangle;
            trigger.FireStubTrigger();
            Assert.IsNull(rectangle.Parent, "rectangle has no parent");
            Assert.IsNull(contentControl.Content, "contentControl has no content");

            // A second removal should not throw an exception.
            trigger.FireStubTrigger();
        }

        [TestMethod]
        public void Invoke_TargetObjectSetButNotAttached_DoesNotRemove()
        {
            Grid grid = new Grid();
            Rectangle rectangle = new Rectangle();
            RemoveElementAction action = new RemoveElementAction();
            StubTrigger trigger = new StubTrigger();

            trigger.Actions.Add(action);
            grid.Children.Add(rectangle);
            action.TargetObject = rectangle;
            trigger.FireStubTrigger();

            Assert.AreEqual(rectangle.Parent, grid, "Because the action is not attached to anything, invoking it should not do anything.");
        }
    }
}