param($installPath, $toolsPath, $package, $project)

# Remove the reference to the .Design.dll, which is incorrectly referenced during
# the NuGet package installation in .NET Framework applications (not .NET Core).
$project.Object.References | Where-Object { $_.Name -eq 'Microsoft.Xaml.Behaviors.Design' } | ForEach-Object { $_.Remove() }