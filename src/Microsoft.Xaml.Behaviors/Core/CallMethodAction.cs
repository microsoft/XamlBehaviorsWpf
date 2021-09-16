// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Microsoft.Xaml.Behaviors.Core
{
    /// <summary>
    /// Calls a method on a specified object when invoked.
    /// </summary>
    public class CallMethodAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TargetObjectProperty =
            DependencyProperty.Register(nameof(TargetObject), typeof(object), typeof(CallMethodAction),
                new PropertyMetadata(OnTargetObjectChanged));

        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(nameof(MethodName),
            typeof(string), typeof(CallMethodAction), new PropertyMetadata(OnMethodNameChanged));

        private List<MethodDescriptor> methodDescriptors;

        public CallMethodAction()
        {
            this.methodDescriptors = new List<MethodDescriptor>();
        }

        /// <summary>
        /// The object that exposes the method of interest. This is a dependency property.
        /// </summary>
        public object TargetObject
        {
            get { return this.GetValue(TargetObjectProperty); }
            set { this.SetValue(TargetObjectProperty, value); }
        }

        /// <summary>
        /// The name of the method to invoke. This is a dependency property.
        /// </summary>
        public string MethodName
        {
            get { return (string)this.GetValue(MethodNameProperty); }
            set { this.SetValue(MethodNameProperty, value); }
        }

        private object Target
        {
            get
            {
                return this.TargetObject ?? this.AssociatedObject;
            }
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter of the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        ///// <exception cref="ArgumentException">A method with <c cref="MethodName"/> could not be found on the <c cref="TargetObject"/>.</exception>
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject != null)
            {
                MethodDescriptor methodDescriptor = this.FindBestMethod(parameter);
                if (methodDescriptor != null)
                {
                    ParameterInfo[] parameters = methodDescriptor.Parameters;

                    // todo jekelly: reconcile these restrictions with spec questions (see below)
                    if (parameters.Length == 0)
                    {
                        methodDescriptor.MethodInfo.Invoke(this.Target, null);
                    } else if (parameters.Length == 2 && this.AssociatedObject != null && parameter != null)
                    {
                        if (parameters[0].ParameterType.IsInstanceOfType(this.AssociatedObject)
                            && parameters[1].ParameterType.IsInstanceOfType(parameter))
                        {
                            methodDescriptor.MethodInfo.Invoke(this.Target, new[] { this.AssociatedObject, parameter });
                        }
                    }
                } else if (this.TargetObject != null)
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        ExceptionStringTable.CallMethodActionValidMethodNotFoundExceptionMessage,
                        this.MethodName,
                        this.TargetObject.GetType().Name));
                }
            }
        }

        /// <summary>
        /// Called after the action is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.UpdateMethodInfo();
        }

        /// <summary>
        /// Called when the action is getting detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected override void OnDetaching()
        {
            this.methodDescriptors.Clear();
            base.OnDetaching();
        }

        private MethodDescriptor FindBestMethod(object parameter)
        {
            return this.methodDescriptors.FirstOrDefault(methodDescriptor => !methodDescriptor.HasParameters ||
                                                                             (parameter != null &&
                                                                              methodDescriptor.SecondParameterType
                                                                                  .IsInstanceOfType(parameter)));
        }

        private void UpdateMethodInfo()
        {
            this.methodDescriptors.Clear();

            if (this.Target == null || string.IsNullOrEmpty(this.MethodName))
            {
                return;
            }

            Type targetType = this.Target.GetType();
            MethodInfo[] methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (MethodInfo method in methods)
            {
                if (!this.IsMethodValid(method))
                {
                    continue;
                }

                ParameterInfo[] methodParams = method.GetParameters();

                if (!AreMethodParamsValid(methodParams))
                {
                    continue;
                }

                this.methodDescriptors.Add(new MethodDescriptor(method, methodParams));
            }

            this.methodDescriptors = this.methodDescriptors.OrderByDescending(methodDescriptor =>
            {
                int distanceFromBaseClass = 0;

                if (!methodDescriptor.HasParameters)
                {
                    return methodDescriptor.ParameterCount;
                }

                Type typeWalker = methodDescriptor.SecondParameterType;
                while (typeWalker != typeof(EventArgs))
                {
                    distanceFromBaseClass++;
                    typeWalker = typeWalker?.BaseType;
                }

                return methodDescriptor.ParameterCount + distanceFromBaseClass;
            }).ToList();
        }

        private bool IsMethodValid(MethodInfo method)
        {
            if (!string.Equals(method.Name, this.MethodName, StringComparison.Ordinal))
            {
                return false;
            }

            return method.ReturnType == typeof(void);
        }

        private static bool AreMethodParamsValid(ParameterInfo[] methodParams)
        {
            if (methodParams.Length == 2)
            {
                if (methodParams[0].ParameterType != typeof(object))
                {
                    return false;
                }

                if (!typeof(EventArgs).IsAssignableFrom(methodParams[1].ParameterType))
                {
                    return false;
                }
            } else if (methodParams.Length != 0)
            {
                return false;
            }

            return true;
        }

        private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CallMethodAction callMethodAction = (CallMethodAction)sender;
            callMethodAction.UpdateMethodInfo();
        }

        private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CallMethodAction callMethodAction = (CallMethodAction)sender;
            callMethodAction.UpdateMethodInfo();
        }

        private class MethodDescriptor
        {
            public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParams)
            {
                this.MethodInfo = methodInfo;
                this.Parameters = methodParams;
            }

            public MethodInfo MethodInfo
            {
                get;
                private set;
            }

            public bool HasParameters
            {
                get { return this.Parameters.Length > 0; }
            }

            public int ParameterCount
            {
                get { return this.Parameters.Length; }
            }

            public ParameterInfo[] Parameters
            {
                get;
                private set;
            }

            public Type SecondParameterType
            {
                get
                {
                    return this.Parameters.Length >= 2 ? this.Parameters[1].ParameterType : null;
                }
            }
        }
    }
}
