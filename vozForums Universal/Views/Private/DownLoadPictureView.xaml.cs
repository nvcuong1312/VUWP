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
using System.Threading;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Private
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DownLoadPictureView : MtPage
    {
        int TotalPage;
        AppSettingModel appSetting;
        private CancellationTokenSource cts;
        private List<DownloadOperation> activeDownloads;

        public DownLoadPictureView()
        {
            this.InitializeComponent();
            TotalPage = 0;
            cts = new CancellationTokenSource();
            appSetting = new AppSettingModel();
        }

        protected override void OnNavigatedTo(MtNavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                var Value = (Dictionary<string, string>)e.Parameter;
                tbIdThread.Text = Value.Keys.FirstOrDefault();
                tbToPage.Text = Value.Values.FirstOrDefault();
                tbFromPage.Text = Value.Values.FirstOrDefault();
            }
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
            // Veryfi Input
            var IDThread = tbIdThread.Text;
            var tempFromPage = tbFromPage.Text;
            var tempToPage = tbToPage.Text;
            var FolderSave = tbFolderSave.Text;
            var backgroundFlag = (chbDownLoadBgr.IsChecked == true) ? true : false;
            int FromPage, ToPage;

            if (string.IsNullOrEmpty(IDThread)
                || !int.TryParse(tempFromPage, out FromPage)
                || !int.TryParse(tempToPage, out ToPage)
                || string.IsNullOrEmpty(FolderSave))
            {
                DialogResult.DialogOnlyOk(Resource.DIALOG_VALUE_INPUT_INVALID);
                return;
            }

            // Get Image
            HtmlHelper helper = new HtmlHelper();
            activeDownloads = new List<DownloadOperation>();

            // Set status disable for control UI
            SetStatusControlWhenStartRun();

            Task.Run(()=>
            {
                Run(helper, IDThread, FromPage, ToPage, backgroundFlag);
            });
        }

        private void SetStatusControlWhenStartRun()
        {
            btnChooseFolderSave.IsEnabled = false;
            btnStart.IsEnabled = false;

            tbIdThread.IsEnabled = false;
            tbFromPage.IsEnabled = false;
            tbToPage.IsEnabled = false;
            tbFolderSave.IsEnabled = false;
            prgProgressBar.Visibility = Visibility.Visible;
        }

        private async void SetStatusControlWhenDone()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => 
            {
                btnChooseFolderSave.IsEnabled = true;
                btnStart.IsEnabled = true;

                tbIdThread.IsEnabled = true;
                tbFromPage.IsEnabled = true;
                tbToPage.IsEnabled = true;
                tbFolderSave.IsEnabled = true;
            });
        }

        private async void Run(HtmlHelper helper, string IDThread, int FromPage, int ToPage, bool backgroundFlag)
        {
            ResetProgressBar();
            TotalPage = ToPage - FromPage + 1;
            bool WaitNextPageFlag = false;
            for (int i = FromPage; i <= ToPage; i++)
            {
                while (WaitNextPageFlag && !backgroundFlag)
                {
                    continue;
                }

                WaitNextPageFlag = true;
                List<string> ListImage = null;
                ListImage = helper.DownLoadImage(IDThread, i);
                if (ListImage != null)
                {
                    int count = 0;
                    foreach (var ImageURL in ListImage)
                    {
                        var FileName = IDThread + "_Page_" + i.ToString() + "_" + count.ToString() + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                        if (backgroundFlag)
                        {
                            await StartDownload(BackgroundTransferPriority.High, ImageURL, FileName);
                        }
                        else
                        {
                            var checkTaskDone = StartDownload(BackgroundTransferPriority.High, ImageURL, FileName);
                            while (!checkTaskDone.Result)
                            {
                                // Continue wait for task done
                                continue;
                            }
                            WaitNextPageFlag = false;
                        }
                        count++;
                    }
                    UpdateProgressBar();
                }
            }

            SetStatusControlWhenDone();
            DialogResult.DialogOnlyOk(Resource.DIALOG_DONE);
        }

        private async Task<bool> StartDownload(BackgroundTransferPriority priority, string URL, string FileToSave)
        {
            Uri source;
            if (!Uri.TryCreate(URL, UriKind.Absolute, out source))
            {
                return true;
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
            catch (Exception ex)
            {
                return true;
            }

            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(source, destinationFile);

            download.Priority = priority;

            try
            {
                activeDownloads.Add(download);
                await download.StartAsync().AsTask(cts.Token);
            }
            catch (Exception)
            {

            }
            finally
            {
                activeDownloads.Remove(download);                
            }
            return true;
        }

        private async void UpdateProgressBar()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                var curr = (prgProgressBar.Value * (double)TotalPage) / 100;
                curr++;
                prgProgressBar.Value = ((double)curr / (double)TotalPage) * 100;
            });
        }

        private async void ResetProgressBar()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                prgProgressBar.Value = 0;
            });
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
            cts.Dispose();
            cts = new CancellationTokenSource();
            activeDownloads = new List<DownloadOperation>();
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
