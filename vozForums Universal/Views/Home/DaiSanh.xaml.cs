using Windows.UI.Xaml.Controls;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DaiSanh : Page
    {
        AppSettingModel appSetting = new AppSettingModel();

        int idBox;
        public DaiSanh()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            lbItem.SelectedIndex = -1;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (thongbao.IsSelected) 
            {
                idBox = 2;
                Frame.Navigate(typeof(ListThread), idBox);             
            }
            if (GopY.IsSelected)
            {
                idBox = 3;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (News.IsSelected)
            {
                idBox = 26;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (Review.IsSelected)
            {
                idBox = 27;
                Frame.Navigate(typeof(ListThread), idBox);
            }
        }      

        private void btnHambuger_Click(object sender, RoutedEventArgs e)
        {
            MainView.GetInstance().HideOpenSplitView();
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
