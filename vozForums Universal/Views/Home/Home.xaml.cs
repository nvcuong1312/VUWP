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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace vozForums_Universal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AppSettingModel appSetting = new AppSettingModel();
        public MainPage()
        {

            this.InitializeComponent();
            DateTime d = DateTime.Now;
            Vozforumsapp.Text = "Vozforums App © " + d.Year.ToString();
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Colors.DarkBlue;
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Colors.Blue;
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


        private string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;
            var hostname =
                NetworkInformation.GetHostNames()
                    .SingleOrDefault(
                        hn =>
                            hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                            == icp.NetworkAdapter.NetworkAdapterId);
            // the ip address
            //GetDns.Text = hostname.ToString();
            return hostname?.CanonicalName;
            
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (DS.IsSelected) Frame.Navigate(typeof(Views.Home.DaiSanh));
            else if (PC.IsSelected) Frame.Navigate(typeof(Views.Home.PC));
            else if (Games.IsSelected) Frame.Navigate(typeof(Views.Home.Games));
            else if (SPCN.IsSelected) Frame.Navigate(typeof(Views.Home.SPCN));
            else if (DNND.IsSelected) Frame.Navigate(typeof(Views.Home.DNND));
            else if (VCGT.IsSelected) Frame.Navigate(typeof(Views.Home.VCGT));
            else if (PC.IsSelected) Frame.Navigate(typeof(Views.Home.PC));
            else if (MB.IsSelected) Frame.Navigate(typeof(Views.Home.Muaban));
            else if (AS.IsSelected) Frame.Navigate(typeof(Views.Account.Setting));
            else if (Support.IsSelected) Frame.Navigate(typeof(Views.Home.Support));
            else if (BM.IsSelected) Frame.Navigate(typeof(Views.Home.Bookmark));
            else if(TheNext.IsSelected) Frame.Navigate(typeof(Views.Home.TheNext));
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
        private async void GoodFun()
        {
            var msg = new MessageDialog("Thím đã nhập sai code. :'> vui lòng nhập code khác :'> ");
            var okBtn = new UICommand("Em biết rồi. :'>");
            msg.Commands.Add(okBtn);
            IUICommand result = await msg.ShowAsync();

        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DateTime dt = DateTime.Now;
            string Temp = dt.Hour.ToString() + dt.Minute.ToString();
            if(GoToTd.Text != null)
            {
                if (GoToTd.Text == Temp)
                    Frame.Navigate(typeof(Views.Hidden.HiddenPage));
                else GoodFun();
            }
            
        }
    }
}
