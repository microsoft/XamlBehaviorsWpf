// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using Microsoft.Xaml.Behaviors;

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: CLSCompliant(true)]

[module: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace", Target = "Microsoft.Xaml.Behaviors.Input")]
[module: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace", Target = "Microsoft.Xaml.Behaviors.Layout")]
[module: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace", Target = "Microsoft.Xaml.Media.Effects")]

// TODO: JKuhne- the following seem to be quirks with the current build of VS
[module: SuppressMessage("Microsoft.MSInternal", "CA904:DeclareTypesInMicrosoftOrSystemNamespace",
    Scope = "namespace", Target = "XamlGeneratedNamespace")]
[module: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
    Scope = "namespace", Target = "XamlGeneratedNamespace")]

[assembly: SuppressMessage("Microsoft.Performance", "CA1824:MarkAssembliesWithNeutralResourcesLanguage")]
[assembly: System.Resources.NeutralResourcesLanguage("en", System.Resources.UltimateResourceFallbackLocation.MainAssembly)]

[assembly: XmlnsPrefix(@"http://schemas.microsoft.com/xaml/behaviors", "b")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors.Core")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors.Input")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors.Layout")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors.Media")]

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Microsoft.Xaml.Behaviors")]

[assembly: AssemblyKeyFile("Behaviors.snk")]
[assembly: InternalsVisibleTo("UnitTests, PublicKey='0024000004800000940000000602000000240000525341310004000001000100e5435599803109fe684072f487ec0670f2766325a25d47089633ffb5d9a56bf115a705bc0632660aeecfe00248951540865f481613845080859feafc5d9b55750395e7ca4c2124136d17bc9e73f0371d802fc2c9e8308f6f8b0ab3096661d2d1b0cbbbcb6de3fe711ef415f29271088537081b09ad1ee08ce8020b22031cdebd")]
