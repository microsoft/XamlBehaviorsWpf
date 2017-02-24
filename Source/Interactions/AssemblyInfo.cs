// -------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All Rights Reserved.
// -------------------------------------------------------------------

using System.Reflection;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Microsoft.Expression.Interactions")]
[assembly: AssemblyDescription("Microsoft.Expression.Interactions")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft.Expression.Interactions")]
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
	Scope = "namespace", Target = "Microsoft.Expression.Interactivity.Input")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace", Target = "Microsoft.Expression.Interactivity.Layout")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace", Target = "Microsoft.Expression.Media.Effects")]

// TODO: JKuhne- the following seem to be quirks with the current build of VS
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.MSInternal", "CA904:DeclareTypesInMicrosoftOrSystemNamespace",
	Scope = "namespace", Target = "XamlGeneratedNamespace")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace", Target = "XamlGeneratedNamespace")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1824:MarkAssembliesWithNeutralResourcesLanguage")]
[assembly: System.Resources.NeutralResourcesLanguage("en", System.Resources.UltimateResourceFallbackLocation.MainAssembly)]

[assembly: System.Windows.Markup.XmlnsPrefix(@"http://schemas.microsoft.com/expression/2010/interactions", "ei")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/interactions", "Microsoft.Expression.Interactivity.Core")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/interactions", "Microsoft.Expression.Interactivity.Input")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/interactions", "Microsoft.Expression.Interactivity.Layout")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/interactions", "Microsoft.Expression.Interactivity.Media")]

[assembly: System.Windows.Markup.XmlnsPrefix(@"http://schemas.microsoft.com/expression/2010/effects", "ee")]
[assembly: System.Windows.Markup.XmlnsDefinition(@"http://schemas.microsoft.com/expression/2010/effects", "Microsoft.Expression.Media.Effects")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Microsoft.Expression.Interactivity")]

// TODO jekelly 07/10/08: Remove this when IsInDesignMode is no longer needed
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Expression.DesignSurface.UnitTests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a30e8b01dfd2a6d08b877c01d267f10811f75546e16d9cd2987e966605cf8813f3beb7fa048ccb69809a2ec89eb808ddc098d379f433b3565870e054befd906092e26e8a885104329cc502032bb88c043f9f16d9db6a0536668f01e11aeddcb367f3f7c62aea0acc04fb8b6262d001e06600b2ad6fb3154b34c77939ce5f29ec")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Microsoft.Expression.BlendSDK.UnitTests, PublicKey=00240000048000009400000006020000002400005253413100040000010001009106b47ec8430d81d1a52156b542d5b6c62c8394abe8a9c8ce74147711bfcd8aaead33a316ce02e8bffc8ff6a1e393d39d433919a0fb99819c626205088a6bf27f54091ce8a651ccac920f6ea325b80706da9ea5a3d1865b65f35721817a9de5969ac81ce6ff65587a3d330976128e1ff117a21d45e061fbb9c9fadcea175ab2")]
