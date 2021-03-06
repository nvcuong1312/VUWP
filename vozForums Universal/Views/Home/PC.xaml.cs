﻿using Windows.UI.Xaml.Controls;
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
    public sealed partial class PC : Page
    {
        AppSettingModel appSetting = new AppSettingModel();
        int idBox;
        public PC()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OC.IsSelected)
            {
                idBox = 6;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (Modding.IsSelected)
            {
                idBox = 151;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (amd.IsSelected)
            {
                idBox = 25;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (intel.IsSelected)
            {
                idBox = 24;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (main.IsSelected)
            {
                idBox = 7;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (graphic.IsSelected)
            {
                idBox = 8;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (hw.IsSelected)
            {
                idBox = 9;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (pk.IsSelected)
            {
                idBox = 30;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (sw.IsSelected)
            {
                idBox = 13;
                Frame.Navigate(typeof(ListThread), idBox);
            }
            if (road.IsSelected)
            {
                idBox = 15;
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
