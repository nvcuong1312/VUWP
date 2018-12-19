using Windows.UI.Xaml.Controls;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using Windows.UI.Xaml;
using System.Reflection;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Muaban : Page
    {
        AppSettingModel appSetting = new AppSettingModel();

        public Muaban()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idBox;
            if (pc.IsSelected)
            {
                idBox = 68;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if (laptop.IsSelected)
            {
                idBox = 72;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if (mobile.IsSelected)
            {
                idBox = 76;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if (other.IsSelected)
            {
                idBox = 80;
                Frame.Navigate(typeof(Views.ListThread), idBox);
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
