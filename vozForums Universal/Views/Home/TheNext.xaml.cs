using vozForums_Universal.Model;
using Windows.UI.Xaml.Controls;


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
    }
}
