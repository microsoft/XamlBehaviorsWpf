// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Layout;

namespace Microsoft.Xaml.Interactions.UnitTests
{
    [TestClass]
    public class MouseDragElementBehaviorTest
    {
        #region Helper classes

        class TestMouseDragElementBehavior : MouseDragElementBehavior
        {
            public void SimulateDragByDelta(double x, double y)
            {
                this.ApplyTranslationTransform(x, y);
            }
        }

        #endregion

        #region Factory methods

        private static Rectangle CreateRectangleInGrid(
            double rectWidth = 10.0d,
            double rectHeight = 10.0d,
            double gridWidth = 100.0,
            double gridHeight = 100.0,
            double top = 0.0d,
            double left = 0.0d)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = rectWidth,
                Height = rectHeight,
                Margin = new Thickness(left, top, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            Grid parentGrid = new Grid { Width = gridWidth, Height = gridHeight, };

            parentGrid.Children.Add(rectangle);
            return rectangle;
        }

        private static TranslateTransform CreateTranslateTransform(double x, double y)
        {
            return new TranslateTransform(x, y);
        }

        private static ScaleTransform CreateScaleTransform(double x, double y)
        {
            return new ScaleTransform(x, y);
        }

        private static SkewTransform CreateSkewTransform(double x, double y)
        {
            return new SkewTransform(x, y);
        }

        private static RotateTransform CreateRotateTransform(double angle)
        {
            return new RotateTransform(angle);
        }

        private static TransformGroup CreateTransformGroup(params Transform[] transforms)
        {
            TransformGroup transformGroup = new TransformGroup();
            foreach (Transform transform in transforms)
            {
                transformGroup.Children.Add(transform);
            }

            return transformGroup;
        }

        private Transform CreateTransformGroupInCanonicalForm()
        {
            return CreateTransformGroup(
                CreateScaleTransform(1, 1),
                CreateSkewTransform(0, 0),
                CreateRotateTransform(0),
                CreateTranslateTransform(10, -10));
        }

        private static MatrixTransform CreateMatrixTransform(double m11, double m12, double m21, double m22, double x,
            double y)
        {
            return new MatrixTransform(m11, m12, m21, m22, x, y);
        }

        private static TestMouseDragElementBehavior CreateAndAttachMouseDragElementBehavior(
            DependencyObject dependencyObject)
        {
            TestMouseDragElementBehavior mouseDragElementBehavior = new TestMouseDragElementBehavior();
            mouseDragElementBehavior.Attach(dependencyObject);
            return mouseDragElementBehavior;
        }

        #endregion

        #region Test methods

        #region TransformIsCloned

        [TestMethod]
        public void OnPositionUpdated_DefaultTransform_TransformIsCloned()
        {
            Transform draggedTransform = VerifyTransformIsClonedOnDrag(Transform.Identity);

            Assert.AreNotSame(draggedTransform, Transform.Identity,
                "Identity transform should not be identical after a drag.");
        }

        [TestMethod]
        public void OnPositionUpdated_TranslateTransform_TransformIsCloned()
        {
            TranslateTransform translateTransform = CreateTranslateTransform(1, 0);
            Transform draggedTransform = VerifyTransformIsClonedOnDrag(translateTransform);

            Assert.AreNotSame(draggedTransform, translateTransform,
                "Translate transform should not be identical after a drag.");
        }

        [TestMethod]
        public void OnPositionUpdated_RotateTransform_TransformIsCloned()
        {
            RotateTransform rotateTransform = CreateRotateTransform(1);
            Transform draggedTransform = VerifyTransformIsClonedOnDrag(rotateTransform);

            Assert.AreNotSame(draggedTransform, rotateTransform,
                "Rotate transform should not be identical after a drag.");
        }

        [TestMethod]
        public void OnPositionUpdated_SkewTransform_TransformIsCloned()
        {
            SkewTransform skewTransform = CreateSkewTransform(1, 0);
            Transform draggedTransform = VerifyTransformIsClonedOnDrag(skewTransform);

            Assert.AreNotSame(draggedTransform, skewTransform, "Skew transform should not be identical after a drag.");
        }

        [TestMethod]
        public void OnPositionUpdated_ScaleTransform_TransformIsCloned()
        {
            ScaleTransform scaleTransform = CreateScaleTransform(1, 0);
            Transform draggedTransform = VerifyTransformIsClonedOnDrag(scaleTransform);

            Assert.AreNotSame(draggedTransform, scaleTransform,
                "Scale transform should not be identical after a drag.");
        }

        [TestMethod]
        public void OnPositionUpdated_MatrixTransform_TransformIsCloned()
        {
            MatrixTransform matrixTransform = CreateMatrixTransform(1, 0, 0, 1, 1, 0);
            Transform draggedTransform = VerifyTransformIsClonedOnDrag(matrixTransform);

            Assert.AreNotSame(draggedTransform, matrixTransform,
                "Matrix transform should not be identical after a drag.");
        }

        [TestMethod]
        public void OnPositionUpdated_TransformGroup_TransformIsCloned()
        {
            TransformGroup transformGroup = CreateTransformGroup();
            Transform draggedTransform = VerifyTransformIsClonedOnDrag(transformGroup);

            Assert.AreNotSame(draggedTransform, transformGroup,
                "Transform group should not be identical after a drag.");
        }

        [TestMethod]
        public void OnPositionUpdated_NullTransform_IsNotNullAfterDrag()
        {
            Transform transform = GetTransformAfterDrag(null, 1, 1);
            Assert.IsNotNull(transform, "Null transform should not be null after drag.");
        }

        private Transform VerifyTransformIsClonedOnDrag(Transform transform)
        {
            Rectangle rectangle = CreateRectangleInGrid();
            TestMouseDragElementBehavior mouseDragElementBehavior = CreateAndAttachMouseDragElementBehavior(rectangle);
            SetRenderTransform(rectangle, transform);

            mouseDragElementBehavior.SimulateDragByDelta(1, 0);

            return rectangle.RenderTransform;
        }

        #endregion

        #region TransformIsCorrect

        [TestMethod]
        public void OnPositionUpdated_IdentityTransform_TransformIsCorrect()
        {
            Transform identityTransform = Transform.Identity;
            Transform draggedTransform = GetTransformAfterDrag(identityTransform, 1, 1);

            this.VerifyOffset(draggedTransform, 1.0, 1.0);
        }

        [TestMethod]
        public void OnPositionUpdated_TranslateTransform_TransformIsCorrect()
        {
            Transform translationTransform = CreateTranslateTransform(10, 10);
            Transform draggedTransform = GetTransformAfterDrag(translationTransform, 1, 1);

            this.VerifyOffset(draggedTransform, 11.0, 11.0);
        }

        [TestMethod]
        public void OnPositionUpdated_RotateTransform_TransformIsCorrect()
        {
            Transform rotateTransform = CreateRotateTransform(30);
            Transform draggedTransform = GetTransformAfterDrag(rotateTransform, 1, 1);

            this.VerifyOffset(draggedTransform, 1.0, 1.0);
        }

        [TestMethod]
        public void OnPositionUpdated_SkewTransform_TransformIsCorrect()
        {
            Transform skewTransform = CreateSkewTransform(2.0, 0.5);
            Transform draggedTransform = GetTransformAfterDrag(skewTransform, 1, 1);

            this.VerifyOffset(draggedTransform, 1.0, 1.0);
        }

        [TestMethod]
        public void OnPositionUpdated_ScaleTransform_TransformIsCorrect()
        {
            Transform scaleTransform = CreateScaleTransform(2.0, 0.5);
            Transform draggedTransform = GetTransformAfterDrag(scaleTransform, 1, 1);

            this.VerifyOffset(draggedTransform, 1.0, 1.0);
        }

        #endregion

        #region IsInCanonicalForm

        [TestMethod]
        public void OnPositionUpdated_TransformGroupInCanonicalForm_IsInCanonicalForm()
        {
            Transform canonicalTransform = CreateTransformGroupInCanonicalForm();
            Transform draggedTransform = GetTransformAfterDrag(canonicalTransform, 1, 1);

            bool isCanonical = IsTransformInCanonicalForm(canonicalTransform);
            Assert.IsTrue(isCanonical, "Canonical form should be preserved on drag.");
        }

        #endregion

        [TestMethod]
        public void IsConstrainedFalse_DragOutsideParentBounds_LeavesParentBounds()
        {
            Rectangle rect = CreateRectangleInGrid();
            MouseDragElementBehavior behavior = CreateAndAttachMouseDragElementBehavior(rect);

            this.PerformSingleDrag(behavior, new Point(5, 5), new Point(101, 101));
            this.VerifyOffset(rect.RenderTransform, 96, 96);
        }

        [TestMethod]
        public void IsConstrainedTrue_NoMovement_DoesNotApplyTransform()
        {
            Rectangle rect = CreateRectangleInGrid();
            MouseDragElementBehavior behavior = CreateAndAttachMouseDragElementBehavior(rect);
            behavior.ConstrainToParentBounds = true;
            this.PerformSingleDrag(behavior, new Point(5, 5), new Point(5, 5));
            this.VerifyOffset(rect.RenderTransform, 0, 0);
        }

        [TestMethod]
        public void IsConstrainedTrue_DragOutsideHorizontalBounds_DoesNotLeaveParent()
        {
            Rectangle rect = CreateRectangleInGrid();
            MouseDragElementBehavior behavior = CreateAndAttachMouseDragElementBehavior(rect);

            behavior.ConstrainToParentBounds = true;
            this.PerformSingleDrag(behavior, new Point(5, 5), new Point(150, 5));
            this.VerifyOffset(rect.RenderTransform, 90, 0);
        }

        [TestMethod]
        public void IsConstrainedTrue_DragOutsideVerticalBounds_DoesNotLeaveParent()
        {
            Rectangle rect = CreateRectangleInGrid();
            MouseDragElementBehavior behavior = CreateAndAttachMouseDragElementBehavior(rect);

            behavior.ConstrainToParentBounds = true;
            this.PerformSingleDrag(behavior, new Point(5, 5), new Point(5, 101));
            this.VerifyOffset(rect.RenderTransform, 0, 90);
        }

        [TestMethod]
        public void IsConstrainedTrue_DragDownSide_MovesAlongUnboundedAxis()
        {
            Rectangle rect = CreateRectangleInGrid();
            MouseDragElementBehavior behavior = CreateAndAttachMouseDragElementBehavior(rect);
            behavior.ConstrainToParentBounds = true;

            using (TestDragToken token = new TestDragToken(behavior, new Point(5, 5)))
            {
                token.PerformDrag(new Point(-5, 25));
                this.VerifyOffset(rect.RenderTransform, 0, 20);
                token.PerformDrag(new Point(-5, 120));
                this.VerifyOffset(rect.RenderTransform, 0, 90);
            }
        }

        [TestMethod]
        public void IsConstrainedTrue_DragOutAndBackIn_DoesNotMoveUntilBackAtStartPoint()
        {
            Rectangle rect = CreateRectangleInGrid();
            MouseDragElementBehavior behavior = CreateAndAttachMouseDragElementBehavior(rect);
            behavior.ConstrainToParentBounds = true;

            using (TestDragToken token = new TestDragToken(behavior, new Point(5, 5)))
            {
                token.PerformDrag(new Point(-50, 5));
                this.VerifyOffset(rect.RenderTransform, 0, 0);
                token.PerformDrag(new Point(-25, 5));
                this.VerifyOffset(rect.RenderTransform, 0, 0);
                token.PerformDrag(new Point(-1, 5));
                this.VerifyOffset(rect.RenderTransform, 0, 0);
                token.PerformDrag(new Point(4, 5));
                this.VerifyOffset(rect.RenderTransform, 0, 0);
                token.PerformDrag(new Point(6, 5));
                this.VerifyOffset(rect.RenderTransform, 1, 0);
            }
        }

        [TestMethod]
        public void IsConstrainedFalse_ParentIsRotated_ElementIsPositionedCorrectly()
        {
            Rectangle rect = CreateRectangleInGrid();
            MouseDragElementBehavior behavior = CreateAndAttachMouseDragElementBehavior(rect);
            RotateTransform transform = new RotateTransform(30);
            ((Grid)rect.Parent).RenderTransform = transform;
            Point startPoint = transform.Transform(new Point(5, 5));
            Point dragPoint = transform.Transform(new Point(50, 50));

            this.PerformSingleDrag(behavior, startPoint, dragPoint);
            this.VerifyOffset(rect.RenderTransform, 45.0d, 45.0d);
        }

        #endregion

        #region Helper functions

        private class TestDragToken : IDisposable
        {
            private readonly MouseDragElementBehavior behavior;
            private readonly FrameworkElement draggedElement;
            private readonly StubWindow stubWindow;

            public TestDragToken(MouseDragElementBehavior behavior, Point startPoint)
            {
                this.behavior = behavior;
                this.draggedElement = (FrameworkElement)((IAttachedObject)behavior).AssociatedObject;
                ;
                this.stubWindow = new StubWindow(draggedElement.Parent);
                GeneralTransform rootToElement = this.stubWindow.TransformToVisual(draggedElement);
                behavior.StartDrag(rootToElement.Transform(startPoint));
            }

            public void Dispose()
            {
                this.behavior.EndDrag();
                this.stubWindow.Dispose();
            }

            public void PerformDrag(Point toPoint)
            {
                Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, new DispatcherOperationCallback(o =>
                {
                    GeneralTransform rootToElement = this.stubWindow.TransformToVisual(draggedElement);
                    behavior.HandleDrag(rootToElement.Transform(toPoint));
                    return null;
                }), null);
            }
        }

        private void PerformSingleDrag(MouseDragElementBehavior behavior, Point from, Point to)
        {
            using (TestDragToken token = new TestDragToken(behavior, from))
            {
                token.PerformDrag(to);
            }
        }

        private bool IsTransformInCanonicalForm(Transform nonCanonicalTransform)
        {
            bool isCanonical = false;
            TransformGroup transformGroup = nonCanonicalTransform as TransformGroup;
            if (transformGroup != null && transformGroup.Children.Count == 4)
            {
                ScaleTransform scaleTransform = transformGroup.Children[0] as ScaleTransform;
                SkewTransform skewTransform = transformGroup.Children[1] as SkewTransform;
                RotateTransform rotateTransform = transformGroup.Children[2] as RotateTransform;
                TranslateTransform translateTransform = transformGroup.Children[3] as TranslateTransform;

                if (scaleTransform != null && skewTransform != null && rotateTransform != null &&
                    translateTransform != null)
                {
                    isCanonical = true;
                }
            }

            return isCanonical;
        }

        private void VerifyOffset(Transform draggedTransform, double x, double y)
        {
            Assert.AreEqual(draggedTransform.Value.OffsetX, x, 0.000000000005,
                string.Format("Expected OffsetX of {0}, got {1}", x, draggedTransform.Value.OffsetX));
            Assert.AreEqual(draggedTransform.Value.OffsetY, y, 0.000000000005,
                string.Format("Expected OffsetY of {0}, got {1}", y, draggedTransform.Value.OffsetY));
        }

        private Transform GetTransformAfterDrag(Transform transform, double x, double y)
        {
            Rectangle rectangle = CreateRectangleInGrid();
            TestMouseDragElementBehavior mouseDragElementBehavior = CreateAndAttachMouseDragElementBehavior(rectangle);
            SetRenderTransform(rectangle, transform);

            mouseDragElementBehavior.SimulateDragByDelta(x, y);

            return rectangle.RenderTransform;
        }

        private static void SetRenderTransform(FrameworkElement frameworkElement, Transform transform)
        {
            frameworkElement.RenderTransform = transform;
        }

        #endregion
    }
}
