using System.Reflection;
using vozForums_Universal.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TheNext : Page
    {
        AppSettingModel appSetting = new AppSettingModel();
        int idBox;
        public TheNext()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (familynet.IsSelected)
            {
                idBox = 273;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (build.IsSelected)
            {
                idBox = 265;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (minipc.IsSelected)
            {
                idBox = 277;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (car.IsSelected)
            {
                idBox = 269;
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
