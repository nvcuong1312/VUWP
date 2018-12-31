using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using vozForums_Universal.Views;
using System.Reflection;
using vozForums_Universal.Model;
using vozForums_Universal.Views.Account;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace vozForums_Universal.CommonControl
{
    public sealed partial class Emoticon : UserControl
    {
        AppSettingModel appSetting;
        public Emoticon()
        {
            this.InitializeComponent();
            appSetting = new AppSettingModel();
            SetHiden();
        }

        private void Image_Tap(object sender, TappedRoutedEventArgs e)
        {
            //if (TypeInput == 1)
            //{
            //    ThreadView.GetInstance().UpdateContentTextbox(((Image)sender).Tag.ToString());
            //}
            //else if (TypeInput == 2)
            //{
            //    MessageView.GetInstance().UpdateContentTextbox(((Image)sender).Tag.ToString());
            //}
            
        }

        /// <summary>
        /// 1 : Thread
        /// 2 : Message
        /// 3 : ListThread
        /// </summary>
        public int TypeInput
        {
            get { return (int)GetValue(TypeInputProperty); }
            set { SetValue(TypeInputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeInputProperty =
            DependencyProperty.Register("TypeInput", typeof(int), typeof(Emoticon), new PropertyMetadata(0));

        public void SetHiden()
        {
            if (appSetting.TotalPosts < Resource.TOTAL_POST_URL_IMAGE)
            {
                PvNamek.Visibility = Visibility.Collapsed;
                Pv30Shades.Visibility = Visibility.Collapsed;
                PvIS.Visibility = Visibility.Collapsed;
                PvPink.Visibility = Visibility.Collapsed;
            }
        }

        private void ScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            ScrollContentPresenter scroll = new ScrollContentPresenter();                    
        }

        private void Image_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TypeInput == 1)
            {
                ThreadView.GetInstance().UpdateContentTextbox(((Button)sender).Tag.ToString());
            }
            else if (TypeInput == 2)
            {
                MessageView.GetInstance().UpdateContentTextbox(((Button)sender).Tag.ToString());
            }
            else if (TypeInput == 3)
            {
                ListThreadView.GetInstance().UpdateContentTextbox(((Button)sender).Tag.ToString());
            }
        }

        private void Button_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem extent = new MenuFlyoutItem { Text = ((Button)sender).Tag.ToString() };
            myFlyout.Items.Add(extent);
            myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }
    }
}
