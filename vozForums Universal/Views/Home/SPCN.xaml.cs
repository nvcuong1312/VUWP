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
    public sealed partial class SPCN : Page
    {
        int idBox;
        AppSettingModel appSetting = new AppSettingModel();

        public SPCN()
        {            
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Laptop.IsSelected)
            {
                idBox = 47;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (Apple.IsSelected)
            {
                idBox = 108;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (pc.IsSelected)
            {
                idBox = 112;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (mobile.IsSelected)
            {
                idBox = 32;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (Election.IsSelected)
            {
                idBox = 10;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (multimedio.IsSelected)
            {
                idBox = 31;
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
