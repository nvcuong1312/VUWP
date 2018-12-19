using Windows.UI.Xaml.Controls;
using vozForums_Universal.Model;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VCGT : Page
    {
        int idBox;
        AppSettingModel appSetting = new AppSettingModel();
        public VCGT()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ctlt.IsSelected)
            {
                idBox = 17;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if (anchoi.IsSelected)
            {
                idBox = 207;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if (dany.IsSelected)
            {
                idBox = 18;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if (diembao.IsSelected)
            {
                idBox = 33;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if (f17wl.IsSelected)
            {
                idBox = 145;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if(sp.IsSelected)
            {
                idBox = 34;
                Frame.Navigate(typeof(Views.ListThread), idBox);
            }
            if(offline.IsSelected)
            {
                idBox = 35;
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
