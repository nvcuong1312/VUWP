using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Reflection;
using vozForums_Universal.Views;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace vozForums_Universal.CommonControl
{
    public sealed partial class RatingControl : UserControl
    {
        public RatingControl()
        {
            this.InitializeComponent();
            InitRadioBtn();
        }
        private void InitRadioBtn()
        {
            Rdo5Star.IsChecked = true;
        }

        private void BtnVote_Click(object sender, RoutedEventArgs e)
        {
            int rating = 0;
            if (Rdo5Star.IsChecked == true)
                rating = 5;
            else if (Rdo4Star.IsChecked == true)
                rating = 4;
            else if (Rdo3Star.IsChecked == true)
                rating = 3;
            else if (Rdo2Star.IsChecked == true)
                rating = 2;
            else if (Rdo1Star.IsChecked == true)
                rating = 1;

            var instance = Thread.GetInstance();
            MethodInfo method = instance.GetType().GetMethod("RatingMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(instance, new object[] { rating });
        }
    }
}
