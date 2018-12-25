// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
namespace Microsoft.Xaml.Interactions.UnitTests
{
    using System.Windows.Threading;

    public static class DispatcherHelper
    {
        private static DispatcherFrame Frame;

        public static void ClearFrames(Dispatcher dispatcher)
        {
            Frame = new DispatcherFrame();
            dispatcher.BeginInvoke(DispatcherPriority.SystemIdle,
                new DispatcherOperationCallback(DispatcherHelper.ExitFrame), DispatcherHelper.Frame);
            Dispatcher.PushFrame(DispatcherHelper.Frame);
        }

        public static void ForceDataBinding()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Inactive, new DispatcherOperationCallback(DispatcherHelper.Placeholder), null);
        }

        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            DispatcherHelper.Frame = null;
            return null;
        }

        private static object Placeholder(object frame)
        {
            return null;
        }
    }
}