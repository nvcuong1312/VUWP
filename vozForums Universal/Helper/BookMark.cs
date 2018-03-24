using System;
using Windows.Storage;
using Windows.UI.Popups;

namespace vozForums_Universal.Helper
{
    
    public class BookMark
    {
        private string bookmark;
        StorageFile sampleFile;

        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        public BookMark()
        {
            GetBm();
        }
        public async void GetBm()
        {
            try
            {
                sampleFile = await localFolder.GetFileAsync("Bookmark.txt");
                bookmark = await FileIO.ReadTextAsync(sampleFile);
            }
            catch
            {
                sampleFile = await localFolder.CreateFileAsync("Bookmark.txt", CreationCollisionOption.ReplaceExisting);
                bookmark = await FileIO.ReadTextAsync(sampleFile);
            }
        }

        public void Add(string name, string id)
        {
            if(bookmark.Contains(name + "|" + id + Environment.NewLine))
            {
                ShowDialog("Content really exist");
            }
            else
            {
                bookmark += name + "|" + id + Environment.NewLine;
                Update(bookmark);
            }
            
        }
        public void Remove(string content)
        {
            bookmark = bookmark.Replace(content, string.Empty);
            Update(bookmark);
        }
        public async void Update(string content)
        {
            try
            {
                await FileIO.WriteTextAsync(sampleFile, content);
                ShowDialog("Done!");
            }
            catch (Exception ex)
            {
                ShowDialog("Error!");
            }

        }
        private async void ShowDialog(string input)
        {
            var msg = new MessageDialog(input);
            var okBtn = new UICommand("Ok");
            msg.Commands.Add(okBtn);
            IUICommand result = await msg.ShowAsync();
        }
    }
}
