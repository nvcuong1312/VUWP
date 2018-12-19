using Windows.UI.Xaml.Controls;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using Windows.UI.Xaml;
using System.Reflection;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Games : Page
    {
        int idBox;
        AppSettingModel appSetting = new AppSettingModel();
        public Games()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (thaoluan.IsSelected)
            {
                idBox = 11;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (garena.IsSelected)
            {
                idBox = 254;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (mmo.IsSelected)
            {
                idBox = 12;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (pokemon.IsSelected)
            {
                idBox = 233;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (overwatch.IsSelected)
            {
                idBox = 237;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (hearthstone.IsSelected)
            {
                idBox = 241;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (fps.IsSelected)
            {
                idBox = 249;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (lol.IsSelected)
            {
                idBox = 178;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (leol.IsSelected)
            {
                idBox = 253;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (dota.IsSelected)
            {
                idBox = 245;
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
