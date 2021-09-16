// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Xaml.Behaviors
{
    [SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic", Justification = "This isn't an exception.")]
    interface ITickTimer
    {
        TimeSpan Interval { get; set; }
        event EventHandler Tick;
        void Start();
        void Stop();
    }
}
