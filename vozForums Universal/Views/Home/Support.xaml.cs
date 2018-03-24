using Windows.UI.Xaml.Controls;
using vozForums_Universal.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Support : Page
    {
        AppSettingModel appSetting = new AppSettingModel();
        public Support()
        {
            this.InitializeComponent();           
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sp.IsSelected)
            {
                Frame.Navigate(typeof(Views.Thread), 6045489.ToString());
            }
            if (About.IsSelected)
            {
                Frame.Navigate(typeof(Version));
            }
        }
    }
}
