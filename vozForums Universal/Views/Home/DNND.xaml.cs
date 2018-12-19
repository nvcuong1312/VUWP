using Windows.UI.Xaml.Controls;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using Windows.UI.Xaml;
using System.Reflection;
using System;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DNND : Page
    {
        int idBox;
        AppSettingModel appSetting = new AppSettingModel();
        public DNND()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dell.IsSelected)
            {
                idBox = 213;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (Hoangha.IsSelected)
            {
                idBox = 222;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (saiback.IsSelected)
            {
                idBox = 257;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (wd.IsSelected)
            {
                idBox = 224;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (wecando.IsSelected)
            {
                idBox = 170;
                Frame.Navigate(typeof(ListThread), idBox);
            }

        }       

        private void btnHambuger_Click(object sender, RoutedEventArgs e)
        {
            MainView.GetInstance().HideOpenSplitView();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            lbItem.SelectedIndex = -1;
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
