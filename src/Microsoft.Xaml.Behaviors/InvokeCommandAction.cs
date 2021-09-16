// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Microsoft.Xaml.Behaviors
{
    /// <summary>
    /// Executes a specified ICommand when invoked.
    /// </summary>
    public sealed class InvokeCommandAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(InvokeCommandAction), null);

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(InvokeCommandAction), null);

        public static readonly DependencyProperty EventArgsConverterProperty =
            DependencyProperty.Register(nameof(EventArgsConverter), typeof(IValueConverter),
                typeof(InvokeCommandAction), new PropertyMetadata(null));

        public static readonly DependencyProperty EventArgsConverterParameterProperty =
            DependencyProperty.Register(nameof(EventArgsConverterParameter), typeof(object),
                typeof(InvokeCommandAction), new PropertyMetadata(null));

        public static readonly DependencyProperty EventArgsParameterPathProperty =
            DependencyProperty.Register(nameof(EventArgsParameterPath), typeof(string), typeof(InvokeCommandAction),
                new PropertyMetadata(null));

        private string commandName;

        /// <summary>
        /// Gets or sets the name of the command this action should invoke.
        /// </summary>
        /// <value>The name of the command this action should invoke.</value>
        /// <remarks>This property will be superseded by the Command property if both are set.</remarks>
        public string CommandName
        {
            get
            {
                this.ReadPreamble();
                return this.commandName;
            }
            set
            {
                if (this.CommandName == value)
                {
                    return;
                }

                this.WritePreamble();
                this.commandName = value;
                this.WritePostscript();
            }
        }

        /// <summary>
        /// Gets or sets the command this action should invoke. This is a dependency property.
        /// </summary>
        /// <value>The command to execute.</value>
        /// <remarks>This property will take precedence over the CommandName property if both are set.</remarks>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command parameter. This is a dependency property.
        /// </summary>
        /// <value>The command parameter.</value>
        /// <remarks>This is the value passed to ICommand.CanExecute and ICommand.Execute.</remarks>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the IValueConverter that is used to convert the EventArgs passed to the Command as a parameter.
        /// </summary>
        /// <remarks>If the <see cref="Command"/> or <see cref="EventArgsParameterPath"/> properties are set, this property is ignored.</remarks>
        public IValueConverter EventArgsConverter
        {
            get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
            set { SetValue(EventArgsConverterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to the EventArgsConverter.
        /// </summary>
        public object EventArgsConverterParameter
        {
            get { return this.GetValue(EventArgsConverterParameterProperty); }
            set { SetValue(EventArgsConverterParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter path used to extract a value from an <see cref= "EventArgs" /> property to pass to the Command as a parameter.
        /// </summary>
        /// <remarks>If the <see cref="Command"/> propert is set, this property is ignored.</remarks>
        public string EventArgsParameterPath
        {
            get { return (string)GetValue(EventArgsParameterPathProperty); }
            set { SetValue(EventArgsParameterPathProperty, value); }
        }

        /// <summary>
        /// Specifies whether the EventArgs of the event that triggered this action should be passed to the Command as a parameter.
        /// </summary>
        /// <remarks>If the <see cref="Command"/>, <see cref="EventArgsParameterPath"/>, or <see cref="EventArgsConverter"/> properties are set, this property is ignored.</remarks>
        public bool PassEventArgsToCommand { get; set; }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject != null)
            {
                ICommand command = this.ResolveCommand();

                if (command != null)
                {
                    object commandParameter = this.CommandParameter;

                    //if no CommandParameter has been provided, let's check the EventArgsParameterPath
                    if (commandParameter == null && !string.IsNullOrWhiteSpace(this.EventArgsParameterPath))
                    {
                        commandParameter = GetEventArgsPropertyPathValue(parameter);
                    }

                    //next let's see if an event args converter has been supplied
                    if (commandParameter == null && this.EventArgsConverter != null)
                    {
                        commandParameter = this.EventArgsConverter.Convert(parameter, typeof(object),
                            EventArgsConverterParameter, CultureInfo.CurrentCulture);
                    }

                    //last resort, let see if they want to force the event args to be passed as a parameter
                    if (commandParameter == null && this.PassEventArgsToCommand)
                    {
                        commandParameter = parameter;
                    }

                    if (command.CanExecute(commandParameter))
                    {
                        command.Execute(commandParameter);
                    }
                } else
                {
                    Debug.WriteLine(ExceptionStringTable.CommandDoesNotExistOnBehaviorWarningMessage, this.CommandName,
                        this.AssociatedObject.GetType().Name);
                }
            }
        }

        private object GetEventArgsPropertyPathValue(object parameter)
        {
            object propertyValue = parameter;
            string[] propertyPathParts = EventArgsParameterPath.Split('.');
            foreach (string propertyPathPart in propertyPathParts)
            {
                PropertyInfo propInfo = propertyValue?.GetType().GetProperty(propertyPathPart);
                propertyValue = propInfo?.GetValue(propertyValue, null);
            }

            object commandParameter = propertyValue;
            return commandParameter;
        }

        private ICommand ResolveCommand()
        {
            ICommand command = null;

            if (this.Command != null)
            {
                command = this.Command;
            } else if (this.AssociatedObject != null)
            {
                // todo jekelly 06/09/08: we could potentially cache some or all of this information if needed, updating when AssociatedObject changes
                Type associatedObjectType = this.AssociatedObject.GetType();
                PropertyInfo[] typeProperties =
                    associatedObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo propertyInfo in typeProperties)
                {
                    if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        if (string.Equals(propertyInfo.Name, this.CommandName, StringComparison.Ordinal))
                        {
                            command = (ICommand)propertyInfo.GetValue(this.AssociatedObject, null);
                        }
                    }
                }
            }

            return command;
        }
    }
}
