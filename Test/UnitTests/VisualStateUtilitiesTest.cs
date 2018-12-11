// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xaml.Behaviors;
#if NETCOREAPP3_0
    using TestClassAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.STAExtensions.STATestClassAttribute;
#endif

    [TestClass]
    public sealed class VisualStateUtilitiesTest
    {
        #region Factory methods

        private static UserControl CreateUserControlWithContent(object content)
        {
            return new UserControl()
            {
                Content = content,
            };
        }

        private static Button CreateButton()
        {
            return new Button();
        }

        private static Button CreateButtonWithCustomTemplate()
        {
            return CreateButtonWithCustomState("TestState");
        }

        private static Button CreateButtonWithCustomState(string stateName)
        {
            return CreateButtonWithCustomTemplate(string.Format(CultureInfo.InvariantCulture,
                                @"<ControlTemplate 
                                    TargetType='{{x:Type Button}}'
                                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                                    xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                                    <Grid>
                                        <VisualStateManager.VisualStateGroups>
                                            <VisualStateGroup Name='Hello'>
                                                <VisualState Name='{0}'/>
                                            </VisualStateGroup>
                                        </VisualStateManager.VisualStateGroups>
                                    </Grid>
                                  </ControlTemplate>", stateName));
        }

        private static Button CreateButtonWithCustomTemplate(string templateString)
        {
            Button button = CreateButton();
            ControlTemplate controlTemplate = new ControlTemplate();

            XamlFragmentParser.TryParseXaml<ControlTemplate>(templateString, out controlTemplate);

            button.Template = controlTemplate;
            button.ApplyTemplate();
            return button;
        }

        private static Grid CreateGridWithChild(FrameworkElement childElement)
        {
            Grid grid = new Grid();
            grid.Children.Add(childElement);
            return grid;
        }

        #endregion

        #region Test methods

        [TestMethod]
        public void FindNearestStatefulControl_ContextNestedSeveralLevelsDeep_FindsAppropriateParent()
        {
            Button button = CreateButton();
            Grid grid = VisualStateHelper.CreateObjectWithStates<Grid>();
            UserControl userControl = CreateUserControlWithContent(grid);
            grid.Children.Add(button);

            FrameworkElement nearestControl;
            VisualStateUtilities.TryFindNearestStatefulControl(button, out nearestControl);
            Assert.AreEqual(nearestControl, userControl, "Using child Grid of UserControl as context, closest stateful control should be UserControl.");
        }

        [TestMethod]
        public void FindNearestStatefulControl_ControlTemplateChild_FindsAppropriateParent()
        {
            Button templatedButton = CreateButtonWithCustomTemplate();
            FrameworkElement child = VisualTreeHelper.GetChild(templatedButton, 0) as FrameworkElement;

            FrameworkElement nearestControl;
            VisualStateUtilities.TryFindNearestStatefulControl(child, out nearestControl);
            Assert.AreEqual(nearestControl, templatedButton, "Using child Grid of Button's ControlTemplate as context, closest stateful control should be Button.");
        }

        [TestMethod]
        public void GoToState_OnUserControl_WorksProperly()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            UserControl userControl = CreateUserControlWithContent(stateGrid);
            userControl.Content = stateGrid;

            bool success = VisualStateUtilities.GoToState(userControl, VisualStateHelper.ArbitraryThirdStateName, true);
            Assert.IsTrue(success, "GoToState on a valid UserControl state should navigate to the state correctly.");
        }

        [TestMethod]
        public void GoToState_OnCustomControlTemplate_WorksProperly()
        {
            string testStateName = "Test";
            Button statefulButton = CreateButtonWithCustomState(testStateName);

            bool success = VisualStateUtilities.GoToState(statefulButton, testStateName, true);
            Assert.IsTrue(success, "GoToState on a valid state in a ControlTemplate should navigate to the state correctly.");
        }

        [TestMethod]
        public void GetVisualStateGroups_UserControlControlWithChildStates_ReturnsChildStates()
        {
            Grid stateGrid = VisualStateHelper.CreateObjectWithStates<Grid>();
            UserControl userControl = CreateUserControlWithContent(stateGrid);

            using (new StubWindow(userControl))
            {
                IList vsgs = VisualStateUtilities.GetVisualStateGroups(userControl);
                Assert.AreEqual(vsgs.Count, 1, "Should find 1 VisualStateGroup on the UserControl");
            }
        }

        #endregion
    }
}
