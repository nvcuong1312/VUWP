using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Model;
using Windows.ApplicationModel.Core;
using Windows.UI.Popups;

namespace vozForums_Universal.CommonControl
{
    public class DialogResult
    {
        /// <summary>
        /// Display dialog with Ok button
        /// </summary>
        /// <param name="Content"></param>
        public static async void DialogOnlyOk(string Content)
        {
            var msg = new MessageDialog(Content);
            var btn = new UICommand(Resource.OK);
            msg.Commands.Add(btn);
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                async () =>
                {
                    await msg.ShowAsync();
                });
        }

        /// <summary>
        /// Ask to change theme for app
        /// </summary>
        /// <param name="isDarkTheme"></param>
        public static async void AskToChangeThemeApp(bool isDarkTheme)
        {
            var msg = new MessageDialog("Restart app to apply effect!");
            var okBtn = new UICommand(Resource.OK);
            var cancelBtn = new UICommand(Resource.NOK);
            msg.Commands.Add(okBtn);
            msg.Commands.Add(cancelBtn);

            IUICommand result = await msg.ShowAsync();
            if (result != null && result.Label == Resource.OK)
            {
                AppSettingModel appSetting = new AppSettingModel();
                if (isDarkTheme)
                {
                    appSetting.ThemeSetting = "Dark";
                }
                else
                {
                    appSetting.ThemeSetting = "Light";
                }
                Windows.UI.Xaml.Application.Current.Exit();
            }
        }

        public static async void AskToExitApp()
        {
            var msg = new MessageDialog("Exit app?");
            var okBtn = new UICommand(Resource.OK);
            var cancelBtn = new UICommand(Resource.NOK);
            msg.Commands.Add(okBtn);
            msg.Commands.Add(cancelBtn);

            IUICommand result = await msg.ShowAsync();
            if (result != null && result.Label == Resource.OK)
            {               
                Windows.UI.Xaml.Application.Current.Exit();
            }
        }
    }
}
