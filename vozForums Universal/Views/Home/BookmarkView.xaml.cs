using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using vozForums_Universal.Helper;
using System.Threading.Tasks;
using vozForums_Universal.Model;
using System.Reflection;
using MyToolkit.Paging;
using vozForums_Universal.ModelData;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookmarkView : MtPage
    {
        //StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        AppSettingModel appSetting;
        BookmarkHelper b;
        BookmarkModelData BmData;

        public BookmarkView()
        {
            this.InitializeComponent();

            appSetting = new AppSettingModel();

            GetListBookmark();
        }

        protected override void OnNavigatedTo(MtNavigationEventArgs e)
        {
            MainView.GetInstance().UpdatePosSelectedListView(Resource.ID_BOOKMARK.ToString());
        }

        protected override void OnNavigatingFrom(MtNavigatingCancelEventArgs e)
        {
            MainView.GetInstance().UpdatePosSelectedListView(Resource.ID_OUT.ToString());
        }

            private void GetListBookmark()
        {
            BmData = new BookmarkModelData();
            lvBookmark.ItemsSource = BmData.GetDataBookmark();
            lvBookmark.IsEnabled = true;            
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private async void btnDeleteBookmark_Click(object sender, RoutedEventArgs e)
        {
            string sen = ((Button)sender).Tag.ToString();
            var ID = sen.Split('_')[0];
            var Page = sen.Split('_')[1];

            BmData.Delete(ID, Page);
            await Task.Delay(TimeSpan.FromSeconds(2));
            GetListBookmark();
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string sen = ((TextBlock)sender).Tag.ToString();
            var ID = sen.Split('_')[0];
            var Page = sen.Split('_')[1];
            Frame.NavigateAsync(typeof(ThreadView), ID + "|" + Page);
        }
    }
}
