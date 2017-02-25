// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Expression.Interactivity.UnitTests
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