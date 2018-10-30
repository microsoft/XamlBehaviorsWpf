// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Microsoft.Xaml.Interactions")]
[assembly: AssemblyDescription("Microsoft.Xaml.Interactions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft.Xaml.Interactions")]
[assembly: AssemblyCopyright("Copyright (c) Microsoft Corporation. All rights reserved.")]

// The Revision number needs to be different from the Revision number for the WPF Behaviors assembly. Otherwise the CLR 
// will attempt to unify these assemblies when they are loaded into Blend.
// The AssemblyVersion is in this file, rather than Version, because we don't want the daily build to be reflected in the assembly version.
[assembly: AssemblyVersion(Microsoft.Expression.BlendSDK.RuntimeVersion.AssemblyVersion)]
[assembly: AssemblyFileVersion(Microsoft.Expression.BlendSDK.VersionConstants.AssemblyFileVersion)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.CLSCompliant(true)]

[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.MSInternal", "CA905:SystemNamespacesRequireApproval", Scope="namespace", Target="System.Windows.Interactivity")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace", Target = "Microsoft.Xaml.Interactivity.Input")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace", Target = "Microsoft.Xaml.Interactivity.Layout")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace", Target = "Microsoft.Xaml.Media.Effects")]

// TODO: JKuhne- the following seem to be quirks with the current build of VS
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.MSInternal", "CA904:DeclareTypesInMicrosoftOrSystemNamespace",
	Scope = "namespace", Target = "XamlGeneratedNamespace")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace", Target = "XamlGeneratedNamespace")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1824:MarkAssembliesWithNeutralResourcesLanguage")]
[assembly: System.Resources.NeutralResourcesLanguage("en", System.Resources.UltimateResourceFallbackLocation.MainAssembly)]

[assembly: System.Windows.Markup.XmlnsPrefix(@"http://schemas.microsoft.com/expression/2010/interactions", "Interactions")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/interactions", "Microsoft.Xaml.Interactions.Core")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/interactions", "Microsoft.Xaml.Interactions.Input")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/interactions", "Microsoft.Xaml.Interactions.Layout")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/interactions", "Microsoft.Xaml.Interactions.Media")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Microsoft.Xaml.Interactivity")]

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("UnitTests, PublicKey='002400000480000094000000060200000024000052534131000400000100010007d1fa57c4aed9f0a32e84aa0faefd0de9e8fd6aec8f87fb03766c834c99921eb23be79ad9d5dcc1dd9ad236132102900b723cf980957fc4e177108fc607774f29e8320e92ea05ece4e821c0a5efe8f1645c4c0c93c1ab99285d622caa652c1dfad63d745d6f2de5f17e5eaf0fc4963d261c8a12436518206dc093344d5ad293")]