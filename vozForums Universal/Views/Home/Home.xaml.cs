using System;
using System.Linq;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Xaml.Input;
using Windows.UI;
using Windows.Networking.Connectivity;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using vozForums_Universal.Views;
using System.Reflection;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using vozForums_Universal.CommonControl;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace vozForums_Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AppSettingModel appSetting = null;
        AccountModel loginModel = null;
        public MainPage()
        {
            this.InitializeComponent();
            appSetting = new AppSettingModel();
            loginModel = new AccountModel();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            Vozforumsapp.Text = "Vozforums App © " + DateTime.Now.Year.ToString();
            //if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            //{
            //    var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            //    if (titleBar != null)
            //    {
            //        titleBar.ButtonBackgroundColor = Colors.DarkBlue;
            //        titleBar.ButtonForegroundColor = Colors.White;
            //        titleBar.BackgroundColor = Colors.Blue;
            //        titleBar.ForegroundColor = Colors.White;                    
            //    }
            //}
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

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            lbItem.SelectedIndex = -1;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainView.GetInstance().UpdatePosSelectedListView("-1");
        }

        //private string GetLocalIp()
        //{
        //    var icp = NetworkInformation.GetInternetConnectionProfile();

        //    if (icp?.NetworkAdapter == null) return null;
        //    var hostname =
        //        NetworkInformation.GetHostNames()
        //            .SingleOrDefault(
        //                hn =>
        //                    hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
        //                    == icp.NetworkAdapter.NetworkAdapterId);
        //    // the ip address
        //    //GetDns.Text = hostname.ToString();
        //    return hostname?.CanonicalName;

        //}

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DS.IsSelected) Frame.Navigate(typeof(Views.Home.DaiSanh), null, new DrillInNavigationTransitionInfo());
            else if (PC.IsSelected) Frame.Navigate(typeof(Views.Home.PC), null, new DrillInNavigationTransitionInfo());
            else if (Games.IsSelected) Frame.Navigate(typeof(Views.Home.Games), null, new DrillInNavigationTransitionInfo());
            else if (SPCN.IsSelected) Frame.Navigate(typeof(Views.Home.SPCN), null, new DrillInNavigationTransitionInfo());
            else if (DNND.IsSelected) Frame.Navigate(typeof(Views.Home.DNND), null, new DrillInNavigationTransitionInfo());
            else if (VCGT.IsSelected) Frame.Navigate(typeof(Views.Home.VCGT), null, new DrillInNavigationTransitionInfo());
            else if (PC.IsSelected) Frame.Navigate(typeof(Views.Home.PC), null, new DrillInNavigationTransitionInfo());
            else if (MB.IsSelected) Frame.Navigate(typeof(Views.Home.Muaban), null, new DrillInNavigationTransitionInfo());
            else if (AS.IsSelected) Frame.Navigate(typeof(Views.Account.Setting), null, new DrillInNavigationTransitionInfo());
            else if (Support.IsSelected) Frame.Navigate(typeof(Views.Home.Support), null, new DrillInNavigationTransitionInfo());
            else if (BM.IsSelected) Frame.Navigate(typeof(Views.Home.Bookmark), null, new DrillInNavigationTransitionInfo());
            else if(TheNext.IsSelected) Frame.Navigate(typeof(Views.Home.TheNext), null, new DrillInNavigationTransitionInfo());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void Tesst_KeyDown(object sender, KeyRoutedEventArgs e)
        //{
        //    if (e.Key == Windows.System.VirtualKey.Enter)
        //    {
        //        DateTime dt = DateTime.Now;
        //        string Temp = dt.Hour.ToString() + dt.Minute.ToString();
        //        if (GoToTd.Text == Temp)
        //            Frame.Navigate(typeof(Views.Hidden.HiddenPage));
        //        else GoodFun();
        //    }

        //}
        //private async void GoodFun()
        //{
        //    var msg = new MessageDialog("Thím đã nhập sai code. :'> vui lòng nhập code khác :'> ");
        //    var okBtn = new UICommand("Em biết rồi. :'>");
        //    msg.Commands.Add(okBtn);
        //    IUICommand result = await msg.ShowAsync();

        //}

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //DateTime dt = DateTime.Now;
            //string Temp = dt.Hour.ToString() + dt.Minute.ToString();
            //if(GoToTd.Text != null)
            //{
            //    if (GoToTd.Text == Temp)
            //        Frame.Navigate(typeof(Views.Hidden.HiddenPage));
            //    else GoodFun();
            //}
        }

        private void btnHambuger_Click(object sender, RoutedEventArgs e)
        {
            var instance = MainView.GetInstance();
            MethodInfo method = instance.GetType().GetMethod("HideOpenSplitView", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(instance, new object[] { });
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Width < Resource.SIZE_WIDTH_SCREEN_600)
            {
                btnHambuger.Visibility = Visibility.Visible;
            }
            else
            {
                btnHambuger.Visibility = Visibility.Collapsed;
            }
        }
    }
}
