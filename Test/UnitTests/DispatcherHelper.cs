// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Windows.Threading;

namespace Microsoft.Xaml.Interactions.UnitTests
{
    public static class DispatcherHelper
    {
        private static DispatcherFrame Frame;

        public static void ClearFrames(Dispatcher dispatcher)
        {
            Frame = new DispatcherFrame();
            dispatcher.BeginInvoke(DispatcherPriority.SystemIdle,
                new DispatcherOperationCallback(ExitFrame), Frame);
            Dispatcher.PushFrame(Frame);
        }

        public static void ForceDataBinding()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Inactive,
                new DispatcherOperationCallback(Placeholder), null);
        }

        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            Frame = null;
            return null;
        }

        private static object Placeholder(object frame)
        {
            return null;
        }
    }
}
