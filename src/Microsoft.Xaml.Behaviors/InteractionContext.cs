// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Windows;
using System.Windows.Navigation;

namespace Microsoft.Xaml.Behaviors
{
    internal static class InteractionContext
    {
        static InteractionContext()
        {
            runtimeAssembly = FindPlatformRuntimeAssembly();

            if (runtimeAssembly != null)
            {
                InitializeRuntimeNavigation();
                LibraryName = (string)libraryNamePropertyInfo.GetValue(playerContextInstance, null);

                LoadNavigationData(LibraryName);
            } else
            {
                InitalizePlatformNavigation();
            }
        }

        #region private fields

        // Our Navigation actions can no longer take a hard dependency on PlayerContext. Fortunately we use very little of the PlayerContext from the runtime
        // so we accumulate reflection info here to use to call the runtime dynamically.
        private static readonly Assembly runtimeAssembly;
        private static object playerContextInstance;
        private static object activeNavigationViewModelObject;
        private static PropertyInfo libraryNamePropertyInfo;
        private static PropertyInfo activeNavigationViewModelPropertyInfo;
        private static PropertyInfo canGoBackPropertyInfo;
        private static PropertyInfo canGoForwardPropertyInfo;
        private static PropertyInfo sketchFlowAnimationPlayerPropertyInfo;
        private static MethodInfo goBackMethodInfo;
        private static MethodInfo goForwardMethodInfo;
        private static MethodInfo navigateToScreenMethodInfo;
        private static MethodInfo invokeStateChangeMethodInfo;
        private static MethodInfo playSketchFlowAnimationMethodInfo;

        private static NavigationService navigationService;
        private static readonly string LibraryName;

        private static readonly Dictionary<string, Serializer.Data> NavigationData =
            new Dictionary<string, Serializer.Data>(StringComparer.OrdinalIgnoreCase);

        #endregion private fields

        #region properties

        public static object ActiveNavigationViewModelObject
        {
            get
            {
                return
                    activeNavigationViewModelObject ??
                    activeNavigationViewModelPropertyInfo.GetValue(
                        playerContextInstance,
                        null);
            }

            internal set // for unit test
            {
                activeNavigationViewModelObject = value;
            }
        }

        private static bool IsPrototypingRuntimeLoaded
        {
            get { return runtimeAssembly != null; }
        }

        private static bool CanGoBack
        {
            get { return (bool)canGoBackPropertyInfo.GetValue(ActiveNavigationViewModelObject, null); }
        }

        private static bool CanGoForward
        {
            get { return (bool)canGoForwardPropertyInfo.GetValue(ActiveNavigationViewModelObject, null); }
        }

        #endregion properties

        #region public methods

        public static void GoBack()
        {
            if (IsPrototypingRuntimeLoaded)
            {
                if (CanGoBack)
                {
                    goBackMethodInfo.Invoke(ActiveNavigationViewModelObject, null);
                }
            } else
            {
                PlatformGoBack();
            }
        }

        public static void GoForward()
        {
            if (IsPrototypingRuntimeLoaded)
            {
                if (CanGoForward)
                {
                    goForwardMethodInfo.Invoke(ActiveNavigationViewModelObject, null);
                }
            } else
            {
                PlatformGoForward();
            }
        }

        public static bool IsScreen(string screenName)
        {
            if (!IsPrototypingRuntimeLoaded)
            {
                return false;
            }

            return GetScreenClassName(screenName) != null;
        }

        public static void GoToScreen(string screenName, Assembly assembly)
        {
            if (IsPrototypingRuntimeLoaded)
            {
                string screenClassName = GetScreenClassName(screenName);

                if (string.IsNullOrEmpty(screenClassName))
                {
                    return;
                }

                //	an array that ends up being parameters to NavigationViewModel.NavigateToScreen(string name, bool record)
                object[] paramArrary = { screenClassName, true };

                navigateToScreenMethodInfo.Invoke(ActiveNavigationViewModelObject, paramArrary);
            } else
            {
                // Verify we could tell where we were
                if (assembly == null)
                {
                    return;
                }

                // The Assembly which is hosting the calling behavior is the one we want to go to the component in
                AssemblyName assemblyName = new AssemblyName(assembly.FullName);
                if (assemblyName != null)
                {
                    string hostAssembly = assemblyName.Name;
                    PlatformGoToScreen(hostAssembly, screenName);
                }
            }
        }

        public static void GoToState(string screen, string state)
        {
            // If you have XAML like the following -

            //	<i:Interaction.Triggers>
            //		<i:EventTrigger>
            //			<pb:ActivateStateAction/>
            //		</i:EventTrigger>
            //	</i:Interaction.Triggers>
            //
            // ...then the Action fires on the Load event without any sort of initialization.
            // The param checks below are intended to keep such actions (and others like it)
            // from doing any harm when triggered
            if (string.IsNullOrEmpty(screen) || string.IsNullOrEmpty(state))
            {
                return;
            }

            if (IsPrototypingRuntimeLoaded)
            {
                //	an array that ends up being parameters to NavigationViewModel.InvokeStateChange(string screen, string state, bool record)
                object[] paramArrary = { screen, state, false };

                invokeStateChangeMethodInfo.Invoke(ActiveNavigationViewModelObject, paramArrary);
            }
        }

        public static void PlaySketchFlowAnimation(string sketchFlowAnimation, string owningScreen)
        {
            // If you have XAML like the following -

            //	<i:Interaction.Triggers>
            //		<i:EventTrigger>
            //			<pb:PlaySketchFlowAnimationAction/>
            //		</i:EventTrigger>
            //	</i:Interaction.Triggers>
            //
            // ...then the Action fires on the Load event without any sort of initialization.
            // The param checks below are intended to keep such actions (and others like it)
            // from doing any harm when triggered

            if (string.IsNullOrEmpty(sketchFlowAnimation) || string.IsNullOrEmpty(owningScreen))
            {
                return;
            }

            if (IsPrototypingRuntimeLoaded)
            {
                object activeNavigationViewModel =
                    activeNavigationViewModelPropertyInfo.GetValue(playerContextInstance, null);

                object[] paramArrary = { sketchFlowAnimation, owningScreen };

                playSketchFlowAnimationMethodInfo.Invoke(activeNavigationViewModel, paramArrary);
            }
        }

        #endregion public methods

        #region private methods

        private static void InitializeRuntimeNavigation()
        {
            Type playerContextType = runtimeAssembly.GetType("Microsoft.Expression.Prototyping.Services.PlayerContext");
            PropertyInfo instancePropertyInfo = playerContextType.GetProperty("Instance");

            activeNavigationViewModelPropertyInfo = playerContextType.GetProperty("ActiveNavigationViewModel");
            libraryNamePropertyInfo = playerContextType.GetProperty("LibraryName");
            playerContextInstance = instancePropertyInfo?.GetValue(null, null);

            Type navigationViewModelType =
                runtimeAssembly.GetType("Microsoft.Expression.Prototyping.Navigation.NavigationViewModel");
            canGoBackPropertyInfo = navigationViewModelType.GetProperty("CanGoBack");
            canGoForwardPropertyInfo = navigationViewModelType.GetProperty("CanGoForward");
            goBackMethodInfo = navigationViewModelType.GetMethod("GoBack");
            goForwardMethodInfo = navigationViewModelType.GetMethod("GoForward");
            navigateToScreenMethodInfo = navigationViewModelType.GetMethod("NavigateToScreen");
            invokeStateChangeMethodInfo = navigationViewModelType.GetMethod("InvokeStateChange");
            playSketchFlowAnimationMethodInfo = navigationViewModelType.GetMethod("PlaySketchFlowAnimation");
            sketchFlowAnimationPlayerPropertyInfo = navigationViewModelType.GetProperty("SketchFlowAnimationPlayer");
        }

        private static Serializer.Data LoadNavigationData(string assemblyName)
        {
            Serializer.Data data = null;
            if (NavigationData.TryGetValue(assemblyName, out data))
            {
                return data;
            }

            Application app = Application.Current;
            string path = string.Format(CultureInfo.InvariantCulture, "/{0};component/Sketch.Flow", assemblyName);
            try
            {
                var info = Application.GetResourceStream(new Uri(path, UriKind.Relative));
                if (info != null)
                {
                    data = Serializer.Deserialize(info.Stream);
                    NavigationData[assemblyName] = data;
                }
            } catch (IOException) { } catch (InvalidOperationException) { }

            return data ?? new Serializer.Data();
        }

        private static string GetScreenClassName(string screenName)
        {
            Serializer.Data data = null;
            NavigationData.TryGetValue(LibraryName, out data);
            if (data?.Screens == null)
            {
                return null;
            }

            if (!data.Screens.Any(screen => screen.ClassName == screenName))
            {
                screenName = data.Screens
                    .Where(screen => screen.DisplayName == screenName)
                    .Select(screen => screen.ClassName)
                    .FirstOrDefault();
            }

            return screenName;
        }

        private static void InitalizePlatformNavigation()
        {
            NavigationWindow navigationWindow = Application.Current.MainWindow as NavigationWindow;
            if (navigationWindow != null)
            {
                navigationService = navigationWindow.NavigationService;
            }
        }

        private static Assembly FindPlatformRuntimeAssembly()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetName().Name.Equals("Microsoft.Expression.Prototyping.Runtime"))
                {
                    return assembly;
                }
            }

            return null;
        }

        public static void PlatformGoBack()
        {
            if (navigationService != null && PlatformCanGoBack)
            {
                navigationService.GoBack();
            }
        }

        public static void PlatformGoForward()
        {
            if (navigationService != null && PlatformCanGoForward)
            {
                navigationService.GoForward();
            }
        }

        public static void PlatformGoToScreen(string assemblyName, string screen)
        {
            ObjectHandle handle = Activator.CreateInstance(assemblyName, screen);
            navigationService.Navigate(handle.Unwrap());
        }

        private static bool PlatformCanGoBack
        {
            get
            {
                if (navigationService != null)
                {
                    return navigationService.CanGoBack;
                }

                return false;
            }
        }

        private static bool PlatformCanGoForward
        {
            get
            {
                if (navigationService != null)
                {
                    return navigationService.CanGoForward;
                }

                return false;
            }
        }

        #endregion private methods
    }
}
