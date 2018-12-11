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

[assembly: XmlnsPrefix(@"http://schemas.microsoft.com/xaml/behaviors", "Interactions")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors.Core")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors.Input")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors.Layout")]
[assembly: XmlnsDefinition(@"http://schemas.microsoft.com/xaml/behaviors", "Microsoft.Xaml.Behaviors.Media")]

[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Microsoft.Xaml.Behaviors")]

#if SIGN_REAL
[assembly: AssemblyKeyFile("FinalPublicKey.snk")]
[assembly: AssemblyDelaySign(true)]
[assembly: InternalsVisibleTo("UnitTests, PublicKey='002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
[assembly: InternalsVisibleTo("UnitTests.NetCore, PublicKey='002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]
#else
[assembly: InternalsVisibleTo("UnitTests")]
[assembly: InternalsVisibleTo("UnitTests.NetCore")]
#endif