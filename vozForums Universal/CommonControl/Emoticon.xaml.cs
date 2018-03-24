using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using vozForums_Universal.Views;
using System.Reflection;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace vozForums_Universal.CommonControl
{
    public sealed partial class Emoticon : UserControl
    {
        public Emoticon()
        {
            this.InitializeComponent();
            DataContext = this;
            Refresh();
        }
        private void Image_Tap(object sender, TappedRoutedEventArgs e)
        {
            var instance = Thread.GetInstance();
            MethodInfo method = instance.GetType().GetMethod("UpdateContentTextbox", BindingFlags.Public | BindingFlags.Instance);
            method.Invoke(instance, new object[] { ((Image)sender).Tag.ToString()});
        }
        public void Refresh()
        {        
        }
        private void ScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            ScrollContentPresenter scroll = new ScrollContentPresenter();                    
        }
        private void Image_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem extent = new MenuFlyoutItem { Text = ((Image)sender).Tag.ToString()};
            myFlyout.Items.Add(extent);
            myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }
    }
}
