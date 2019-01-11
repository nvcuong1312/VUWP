using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using vozForums_Universal.CommonControl;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using vozForums_Universal.ModelData;
using vozForums_Universal.Views.Account;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using MyToolkit.Paging;
using MyToolkit.Paging.Animations;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : MtPage
    {
        int idSelected = 0;
        HtmlHelper Helper = null;
        AccountHelper loginModel = null;
        AppSettingModel appSetting = null;

        public MainView()
        {
            this.InitializeComponent();
            Helper = new HtmlHelper();
            loginModel = new AccountHelper();
            appSetting = new AppSettingModel();

            CheckStatusSplitView();
            RefreshSplitView();
            LoginFirst();
            _instance = this;
            PageAnimation = new ScalePageTransition();
            frMainFrame.NavigateAsync(typeof(ListThreadView), appSetting.BoxStart);

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Colors.DarkSlateGray;
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Colors.DarkSlateGray;
                    titleBar.ForegroundColor = Colors.White;
                }
            }
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Colors.Black;
                    statusBar.ForegroundColor = Colors.White;
                }
            }
        }

        public void UpdatePosSelectedListView(string ID)
        {
            dynamic xxx = lvSplitPane.Items.ToList();
            bool isFound = false;
            for (int i = 0; i < xxx.Count; i++)
            {
                if (xxx[i].Id == ID)
                {
                    lvSplitPane.SelectedIndex = i;
                    isFound = true;
                    break;
                }
            }
            if (!isFound)
            {
                lvSplitPane.SelectedIndex = -1;
            }
            idSelected = int.Parse(ID);
        }

        public void RefreshSplitView()
        {
            BoxModelData homeData = new BoxModelData();
            var ListBox = homeData.GetListBox();
            var group = from c in ListBox
                        group c by c.NameParentBox;
            this.cvs.Source = group;
        }

        private static MainView _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static MainView GetInstance()
        {
            return _instance;
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            HideOpenSplitView();
        }

        public void HideOpenSplitView()
        {
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                splMainSplitView.OpenPaneLength = ActualWidth;
            }
            else
            {
                splMainSplitView.DisplayMode = SplitViewDisplayMode.Inline;
                splMainSplitView.OpenPaneLength = 300;
            }
            splMainSplitView.IsPaneOpen = !splMainSplitView.IsPaneOpen;
        }

        private async void LoginFirst()
        {
            string UserName = appSetting.UserName;
            string Password = appSetting.Password;
            string CookiePW = appSetting.Cookies_Vfpassword;
            string CookieID = appSetting.Cookies_Vfuserid;

            BtnGotoAccount.Visibility = Visibility.Collapsed;
            BtnGotoMessage.Visibility = Visibility.Collapsed;

            if (appSetting.isSaveLogin)
            {
                if (CookiePW.Length > 10)
                {
                    UpdateNameAndStatusBtnAccount(UserName);
                }
                else
                {
                    PrgRLogin.IsActive = true;
                    BtnAccountOption.IsEnabled = false;
                    tblFullName.Text = Resource.STR_LOGGING;
                    await Task.Run(async () =>
                    {
                        if (loginModel.LoginMethod(UserName, Password, true))
                        {
                            UpdateNameAndStatusBtnAccount(UserName);
                        }
                        else
                        {
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                tblFullName.Text = Resource.STR_VISIT;
                            });
                        }
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            PrgRLogin.IsActive = false;
                            BtnAccountOption.IsEnabled = true;
                        });
                    });
                }
            }
            else
            {
                appSetting.UserName = Resource.STR_EMPTY;
                appSetting.Password = Resource.STR_EMPTY;
                appSetting.Token = Resource.STR_EMPTY;
                appSetting.Cookies_Vfuserid = Resource.STR_EMPTY;
                appSetting.Cookies_Vfpassword = Resource.STR_EMPTY;

                UpdateNameAndStatusBtnAccount(Resource.STR_VISIT);
            }
        }

        public async void UpdateNameAndStatusBtnAccount(string fullName)
        {
            string[] temp = fullName.Split(' ');
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                tblFullName.Text = fullName;
                if (temp.Length == 1)
                {
                    tblShortName.Text = temp[0].ToArray()[0].ToString().ToUpper();
                }
                else
                {
                    tblShortName.Text = temp[0].ToArray()[0].ToString().ToUpper() + temp[temp.Length - 1].ToArray()[0].ToString().ToUpper();
                }

                if (appSetting.Cookies_Vfpassword.Length > 10)
                {
                    //BtnGotoAccount.Visibility = Visibility.Visible;
                    BtnGotoMessage.Visibility = Visibility.Visible;
                }
                else
                {
                    BtnGotoAccount.Visibility = Visibility.Collapsed;
                    BtnGotoMessage.Visibility = Visibility.Collapsed;
                    brNotifi.Visibility = Visibility.Collapsed;
                }
            });
        }

        public async void UpdateInfoMessageBtn(int Unread, int Total)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (Unread > 0)
                {
                    brNotifi.Visibility = Visibility.Visible;
                    brNotifiPM.Visibility = Visibility.Visible;
                }
                else
                {
                    brNotifi.Visibility = Visibility.Collapsed;
                    brNotifiPM.Visibility = Visibility.Collapsed;
                }

                if (Total == 0)
                {
                    var MsgInfo = Unread.ToString();
                    tbTotalMessage.Text = MsgInfo;
                }
                else
                {
                    var MsgInfo = "(" + Unread.ToString() + " / " + Total.ToString() + ")";
                    tbTotalMessage.Text = MsgInfo;
                }
            });
        }

        private void MyFrame_Navigated(object sender, MtNavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (frMainFrame.CanGoBack)
            {
                e.Handled = true;
                frMainFrame.GoBackAsync();
            }
            else
            {
                DialogResult.AskToExitApp();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckStatusSplitView();
        }

        private void CheckStatusSplitView()
        {
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.DisplayMode = SplitViewDisplayMode.Overlay;
                splMainSplitView.OpenPaneLength = ActualWidth;
                btnHambuger.Visibility = Visibility.Visible;
            }
            else
            {
                splMainSplitView.IsPaneOpen = true;
                splMainSplitView.DisplayMode = SplitViewDisplayMode.Inline;
                splMainSplitView.OpenPaneLength = 300;
                btnHambuger.Visibility = Visibility.Collapsed;
            }
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.IsPaneOpen = !splMainSplitView.IsPaneOpen;
            }

            int id = int.Parse(((Grid)sender).Tag.ToString());
            if (idSelected != id)
            {
                frMainFrame.NavigateAsync(typeof(ListThreadView), id);
            }
        }

        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.IsPaneOpen = !splMainSplitView.IsPaneOpen;
            }

            if (idSelected != Resource.ID_SETTING)
            {
                frMainFrame.Navigate(typeof(SettingView));
            }
        }

        private void BtnBookmark_Click(object sender, RoutedEventArgs e)
        {
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.IsPaneOpen = !splMainSplitView.IsPaneOpen;
            }

            if (idSelected != Resource.ID_BOOKMARK)
            {
                frMainFrame.Navigate(typeof(Home.BookmarkView));
            }
        }

        private void BtnGotoAboutApp_Click(object sender, RoutedEventArgs e)
        {
            FlyBtnAccount.Hide();
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.IsPaneOpen = !splMainSplitView.IsPaneOpen;
            }

            if (idSelected != Resource.ID_SUPPORT)
            {
                frMainFrame.Navigate(typeof(Home.SupportView));
            }
        }

        private void BtnGotoAccount_Click(object sender, RoutedEventArgs e)
        {
            FlyBtnAccount.Hide();
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.IsPaneOpen = !splMainSplitView.IsPaneOpen;
            }

            if (idSelected != Resource.ID_ACCOUNT)
            {
                frMainFrame.Navigate(typeof(AccountView));
            }
        }

        private void BtnGotoMessage_Click(object sender, RoutedEventArgs e)
        {
            FlyBtnAccount.Hide();
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.IsPaneOpen = !splMainSplitView.IsPaneOpen;
            }

            if (idSelected != Resource.ID_MESSAGE)
            {
                frMainFrame.Navigate(typeof(ListMessageView));
            }
        }

        private void BtnToolForApp_Click(object sender, RoutedEventArgs e)
        {
            FlyBtnAccount.Hide();
            if (ActualWidth < Resource.SIZE_WIDTH_SCREEN_600)
            {
                splMainSplitView.IsPaneOpen = !splMainSplitView.IsPaneOpen;
            }
            frMainFrame.Navigate(typeof(Private.DownLoadPictureView));
        }
    }
}
