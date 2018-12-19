using Windows.UI.Xaml.Controls;
using vozForums_Universal.Model;
using Windows.UI.Xaml;
using System.Reflection;
using Windows.UI.Xaml.Navigation;
using MyToolkit.Paging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SupportView : MtPage
    {
        AppSettingModel appSetting = new AppSettingModel();
        public SupportView()
        {
            this.InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sp.IsSelected)
            {
                Frame.NavigateAsync(typeof(ThreadView), "6578773");
            }
            if (About.IsSelected)
            {
                Frame.Navigate(typeof(VersionView));
            }
        }

        protected override void OnNavigatedTo(MtNavigationEventArgs e)
        {
            MainView.GetInstance().UpdatePosSelectedListView(Resource.ID_SUPPORT.ToString());
        }

        protected override void OnNavigatingFrom(MtNavigatingCancelEventArgs e)
        {
            lbItem.SelectedIndex = -1;
        }       
    }
}
