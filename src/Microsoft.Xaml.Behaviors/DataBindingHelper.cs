// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
namespace Microsoft.Xaml.Behaviors
{

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Helper class for managing binding expressions on dependency objects.
    /// </summary>
    internal static class DataBindingHelper
    {
        private static Dictionary<Type, CacheNode> dpCache = new Dictionary<Type, CacheNode>();
        /// <summary>
        /// Ensure that all DP on an action with binding expressions are
        /// up to date. DataTrigger fires during data binding phase. Since
        /// actions are children of the trigger, any bindings on the action
        /// may not be up-to-date. This routine is called before the action
        /// is invoked in order to guarantee that all bindings are up-to-date
        /// with the most current data. 
        /// </summary>
        public static void EnsureDataBindingUpToDateOnMembers(DependencyObject dpObject)
        {
            CacheNode node = GetDependencyPropertyCacheNode(dpObject.GetType());

            do
            {
                if (node.Properties != null)
                {
                    foreach (DependencyProperty property in node.Properties)
                    {
                        EnsureBindingUpToDate(dpObject, property);
                    }
                }

                node = node.Base;

            } while (node != null);
        }
        private static CacheNode GetDependencyPropertyCacheNode(Type type)
        {
            if (dpCache.TryGetValue(type, out CacheNode node))
            {
                return node;
            }

            dpCache[type] = node = new CacheNode();

            CacheNode currentNode = node;
            List<DependencyProperty> list = new List<DependencyProperty>();

            while (type != typeof(DependencyObject))
            {
                list.Clear();

                IEnumerable<FieldInfo> fields = type.GetTypeInfo().DeclaredFields;

                foreach (FieldInfo field in fields)
                {
                    if (!field.IsPublic || !field.IsStatic || field.FieldType != typeof(DependencyProperty))
                    {
                        continue;
                    }

                    if (field.GetValue(null) is DependencyProperty dependencyProperty)
                    {
                        list.Add(dependencyProperty);
                    }
                }

                if (list.Count > 0)
                {
                    currentNode.Properties = list.ToArray();
                }

                type = type.BaseType;

                if (!dpCache.TryGetValue(type, out CacheNode baseNode))
                {
                    currentNode.Base = new CacheNode();
                    dpCache[type] = currentNode = currentNode.Base;
                } else
                {
                    currentNode.Base = baseNode;
                    break;
                }
            }

            return node;
        }

        /// <summary>
        /// Ensures that all binding expression on actions are up to date
        /// </summary>
        public static void EnsureDataBindingOnActionsUpToDate(TriggerBase<DependencyObject> trigger)
        {
            // Update the bindings on the actions. 
            foreach (TriggerAction action in trigger.Actions)
            {
                DataBindingHelper.EnsureDataBindingUpToDateOnMembers(action);
            }
        }

        /// <summary>
        ///  This helper function ensures that, if a dependency property on a dependency object
        ///  has a binding expression, the binding expression is up-to-date. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dp"></param>
        public static void EnsureBindingUpToDate(DependencyObject target, DependencyProperty dp)
        {
            BindingExpression binding = BindingOperations.GetBindingExpression(target, dp);
            if (binding != null)
            {
                binding.UpdateTarget();
            }
        }

        private class CacheNode
        {
            public DependencyProperty[] Properties;

            public CacheNode Base;
        }
    }
}
