// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------
namespace Microsoft.Expression.Interactivity
{
	using System;
	using System.Diagnostics.CodeAnalysis;

	[SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic", Justification="This isn't an exception.")]
	interface ITickTimer
	{
		event EventHandler Tick;
		void Start();
		void Stop();
		TimeSpan Interval { get; set; }
	}
}
