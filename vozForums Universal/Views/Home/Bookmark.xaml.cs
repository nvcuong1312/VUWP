using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using vozForums_Universal.Helper;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Home
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Bookmark : Page
    {
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        BookMark b = new BookMark();

        private List<ListBookmark> list;
        public Bookmark()
        {
            this.InitializeComponent();
            GetListBookmark();
        }
        private async void GetListBookmark()
        {
            list = new List<ListBookmark>();
            list.Clear();
            string line;            
            //Get all content from Bookmark.txt
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync("Bookmark.txt");
                String timestamp = await FileIO.ReadTextAsync(sampleFile);

                StringReader reader = new StringReader(timestamp);
                while ((line = reader.ReadLine()) != null)
                {
                    ListBookmark bookmark = new ListBookmark();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    try
                    {
                        bookmark.Name = line.Split('|')[0];
                        bookmark.idThread = line.Split('|')[1];
                        bookmark.numPage = line.Split('|')[2];
                        bookmark.idThread = bookmark.idThread + "|" + bookmark.numPage;
                        bookmark.contentRemove = bookmark.Name + "|" + bookmark.idThread;
                    }
                    catch {
                        bookmark.Name = line.Split('|')[0];
                        bookmark.idThread = line.Split('|')[1];
                        bookmark.contentRemove = bookmark.Name + "|" + bookmark.idThread;

                    }
                    list.Add(bookmark);
                }

                lvBookmark.ItemsSource = list;
                lvBookmark.IsEnabled = true;
            }
            catch
            { }
            
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string idThread = ((TextBlock)sender).Tag.ToString();
            Frame.Navigate(typeof(Thread), idThread);
        }



        private async void btnDeleteBookmark_Click(object sender, RoutedEventArgs e)
        {
            string sen = ((Button)sender).Tag.ToString();
            b.Remove(sen + Environment.NewLine);
            await Task.Delay(TimeSpan.FromSeconds(2));
            GetListBookmark();
        }
    }
    public class ListBookmark
    {
        public string Name { get; set; }
        public string idThread { get; set; }
        public string contentRemove { get; set; }
        public string numPage { get; set; }
    }

}
