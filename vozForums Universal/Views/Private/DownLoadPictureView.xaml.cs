using MyToolkit.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using vozForums_Universal.CommonControl;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using System.Threading.Tasks;
using System.Net.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Private
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DownLoadPictureView : MtPage
    {
        int MaxFile;
        AppSettingModel appSetting;

        public DownLoadPictureView()
        {
            this.InitializeComponent();
            MaxFile = 0;
            appSetting = new AppSettingModel();
        }

        StorageFolder _Folder;
        private async void BtnChooseFolderSave_Click(object sender, RoutedEventArgs e)
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", folder);
                tbFolderSave.Text = folder.Path;
                _Folder = folder;
            }
            else
            {
                tbFolderSave.Text = Resource.STR_EMPTY;
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ResetProgressBar();

            // Veryfi Input
            var IDThread = tbIdThread.Text;
            var tempFromPage = tbFromPage.Text;
            var tempToPage = tbToPage.Text;
            var FolderSave = tbFolderSave.Text;

            int FromPage, ToPage;

            if (string.IsNullOrEmpty(IDThread)
                || !int.TryParse(tempFromPage, out FromPage)
                || !int.TryParse(tempToPage, out ToPage)
                || string.IsNullOrEmpty(FolderSave))
            {
                DialogResult.DialogOnlyOk(Resource.DIALOG_VALUE_INPUT_INVALID);
                return;
            }
            //StartDownload(BackgroundTransferPriority.High, "http://farm3.static.flickr.com/2795/5721108811_243df04b90_z.jpg", "cuong");
            //StartDownload(BackgroundTransferPriority.High, "http://farm3.static.flickr.com/2795/5721108811_243df04b90_z.jpg", "cuong2");
            //StartDownload(BackgroundTransferPriority.High, "http://farm3.static.flickr.com/2795/5721108811_243df04b90_z.jpg", "cuong3");
            //StartDownload(BackgroundTransferPriority.High, "http://farm3.static.flickr.com/2795/5721108811_243df04b90_z.jpg", "cuong4");
            //StartDownload(BackgroundTransferPriority.High, "http://farm3.static.flickr.com/2795/5721108811_243df04b90_z.jpg", "cuong5");
            
            // Get Image
            HtmlHelper helper = new HtmlHelper();
            var ListImage = helper.DownLoadImage(IDThread, FromPage, ToPage);

            MaxFile = ListImage.Count();
            int count = 0;
            foreach (var ImageURL in ListImage)
            {
                var FileName = IDThread + "_" + count.ToString() + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                StartDownload(BackgroundTransferPriority.High, ImageURL, FileName);
                count++;
            }
        }

        private async void StartDownload(BackgroundTransferPriority priority, string URL, string FileToSave)
        {
            Uri source;
            if (!Uri.TryCreate(URL, UriKind.Absolute, out source))
            {
                return;
            }

            string destination;
            if (URL.Split('.').LastOrDefault() == "gif")
            {
                destination = FileToSave + ".gif";
            }
            else
            {
                destination = FileToSave + ".png";
            }

            StorageFile destinationFile;
            try
            {                
                destinationFile = await _Folder.CreateFileAsync(destination, CreationCollisionOption.GenerateUniqueName);
            }
            catch (FileNotFoundException ex)
            {
                return;
            }

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(source, destinationFile);

            download.Priority = priority;

            try
            {
                await download.StartAsync();
                ResponseInformation response = download.GetResponseInformation();
                // GetResponseInformation() returns null for non-HTTP transfers (e.g., FTP).
                string statusCode = response != null ? response.StatusCode.ToString() : String.Empty;
                UpdateProgressBar();
            }
            catch (Exception)
            {
                
            }

            //HandleDownloadAsync(download, true);
        }

        private async void UpdateProgressBar()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                var curr = prgProgressBar.Value;
                curr++;
                prgProgressBar.Value = ((double)curr / (double)MaxFile) * 100;
            });
        }

        private async void ResetProgressBar()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                prgProgressBar.Value = 0;
            });
        }

        //private async void HandleDownloadAsync(DownloadOperation download, bool start)
        //{
        //    try
        //    {

        //        ResponseInformation response = download.GetResponseInformation();
        //        // GetResponseInformation() returns null for non-HTTP transfers (e.g., FTP).
        //        string statusCode = response != null ? response.StatusCode.ToString() : String.Empty;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
