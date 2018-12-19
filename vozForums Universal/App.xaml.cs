using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using vozForums_Universal.Model;
using vozForums_Universal.Views;
using MyToolkit.Controls;
using MyToolkit.Paging;
using MyToolkit.Paging.Animations;
using MyToolkit.UI;

namespace vozForums_Universal
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MtApplication
    {
        AppSettingModel appSetting = null;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            appSetting = new AppSettingModel();
            if (appSetting.ThemeSetting == "Light")
            {
                Application.Current.RequestedTheme = ApplicationTheme.Light;
            }
            else
            {
                appSetting.ThemeSetting = "Dark";
                Application.Current.RequestedTheme = ApplicationTheme.Dark;
            }

            this.InitializeComponent();
        }

        public override Type StartPageType => typeof(MainView);

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            ApplicationViewUtilities.ConnectRootElementSizeToVisibleBounds();
        }
    }
}
