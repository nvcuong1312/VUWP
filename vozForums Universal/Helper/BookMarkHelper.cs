using System;
using vozForums_Universal.CommonControl;
using Windows.Storage;
using Windows.UI.Popups;

namespace vozForums_Universal.Helper
{    
    public class BookMarkHelper
    {
        private string bookmark;
        private StorageFile sampleFile;
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public BookMarkHelper()
        {
            GetBookmark();
        }

        public async void GetBookmark()
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

        /// <summary>
        /// Add thread to book mark
        /// </summary>
        /// <param name="name">Title Thread</param>
        /// <param name="Info[0]">Id Thread</param>
        /// <param name="Info[1]">Page of Thread</param>
        public void Add(string name, string[] Info)
        {
            GetBookmark();
            string inputValue = name + "|" + Info[0] + "|" + Info[1] + Environment.NewLine;
            if (bookmark.Contains(inputValue))
            {
                DialogResult.DialogOnlyOk(Resource.STR_CONTENT_EXIST);
            }
            else
            {
                bookmark += inputValue;
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
                DialogResult.DialogOnlyOk(Resource.STR_DONE);
            }
            catch (Exception ex)
            {
                DialogResult.DialogOnlyOk(Resource.STR_ERROR);
            }
        }
    }
}
