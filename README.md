# XamlBehaviors for WPF

XAML Behaviors is an easy-to-use means of adding common and reusable interactivity to your WPF applications with minimal code. Use of XAML Behaviors is governed by the MIT License. We are working with a [committee of Microsoft MVP leaders](https://github.com/Microsoft/XamlBehaviorsWpf/wiki/About-the-Team) to guide the Behaviors project and evaluate incoming pull requests.

Getting Started
-------------------
**Where to get it**
 - [Source Code](https://github.com/Microsoft/XamlBehaviorsWpf)

**Resources**
 - [Samples](https://github.com/Microsoft/XamlBehaviorsWpf/tree/master/samples)

**More Info**
 - [Report a bug](https://github.com/Microsoft/XamlBehaviorsWpf/issues)
 - [License](https://opensource.org/licenses/MIT)

Using Behaviors SDK
-------------------
To use behaviors in your project, add the [Microsoft.Xaml.Behaviors.Wpf](https://www.nuget.org/packages/Microsoft.Xaml.Behaviors.Wpf) NuGet package to your project.

Buiding Behaviors from Source
------------------------------
**What You Need**
 - [Visual Studio 2017](https://www.visualstudio.com/features/windows-apps-games-vs)

**Clone the Repository**
 - Go to 'View' -> 'Team Explorer' -> 'Local Git Repositories' -> 'Clone'
 - Add the XAML Behaviors for WPF repository URL (https://github.com/Microsoft/XamlBehaviorsWpf) and hit 'Clone'

**Build and Create XAML Behaviors NuGet**
 - Open a Visual Studio developer command prompt
 - Navigate to the XAML Behaviors for WPF repository
 - Run msbuild src\Microsoft.Xaml.Behaviors\Microsoft.Xaml.Behaviors.csproj /t:Pack

# Contributing

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
