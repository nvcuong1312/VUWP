using MyToolkit.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using vozForums_Universal.Controller;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using vozForums_Universal.ModelData;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Account
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MessageView : MtPage
    {
        private string url;
        private string IdMessage;
        private string TitleMessage;
        private string UserName;

        private HtmlHelper helper;
        private AppSettingModel appSetting;
        private MessageController messageController;

        public MessageView()
        {
            this.InitializeComponent();

            helper = new HtmlHelper();
            appSetting = new AppSettingModel();
            messageController = new MessageController();

            _instance = this;
        }

        private static MessageView _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static MessageView GetInstance()
        {
            return _instance;
        }

        protected override void OnNavigatedTo(MtNavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                if ((int)e.Parameters[0] == 1)
                {
                    IdMessage = e.Parameters[1].ToString();
                    TitleMessage = e.Parameters[2].ToString();
                    UserName = e.Parameters[3].ToString();
                    Loader();
                }
                else if ((int)e.Parameters[0] == 2)
                {
                    TblTitleMsg.Text = Resource.STR_NEW_MESSAGE;
                    DisplayPopupPostMessage();                    
                }
            }
        }

        private async void Loader()
        {
            url = Resource.URL_MESSAGE.Replace("{rpID}", IdMessage);
            string contentHtml = Resource.STR_EMPTY;
            pgbarLoading.IsIndeterminate = true;
            pgbarLoading.Visibility = Visibility.Visible;
            try
            {
                await Task.Run(() => messageController.GetContent(url, ref contentHtml));
                if (!string.IsNullOrEmpty(contentHtml) && contentHtml != Resource.DIALOG_ERROR)
                {
                    helper.GetMessagePrivate(contentHtml);

                    MessageModelData messageModelData = new MessageModelData(contentHtml);

                    // Display Content
                    myWebview.NavigateToString(helper.FullPageThreadHtml(messageModelData.GetMessageData()));

                    TblTitleMsg.Text = TitleMessage;
                }
                else if (contentHtml == Resource.DIALOG_ERROR)
                {
                    TblTitleMsg.Text = Resource.DIALOG_ERROR;
                }
            }
            catch (Exception ex)
            {
                var xxx = ex.ToString();
                TblTitleMsg.Text = Resource.DIALOG_ERROR;
            }

            pgbarLoading.IsIndeterminate = false;
            pgbarLoading.Visibility = Visibility.Collapsed;
        }

        private void myWebview_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var vl = e.Value;
            messageController.NavigateManager(vl);
        }

        private void btnEmoticon_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void sentMessage_Click(object sender, RoutedEventArgs e)
        {
            tbMessage.IsEnabled = false;
            bool checkDone = false;
            await Task.Delay(TimeSpan.FromSeconds(0.3));
            messageController.PostMessage(tbMessage.Text, new string[] { IdMessage, tbTitle.Text, tbUser.Text }, ref checkDone);
            if (checkDone)
            {
                if (!string.IsNullOrEmpty(IdMessage))
                {
                    Loader();
                }
                TblTitleMsg.Text = Resource.STR_POST_MESSAGE_DONE;
                tbMessage.Text = string.Empty;
                tbMessage.IsEnabled = true;
                myPopupPostMessage.IsOpen = false;
            }
        }

        private void BtnClosePopupPostMessage_Click(object sender, RoutedEventArgs e)
        {
            myPopupPostMessage.IsOpen = false;
        }

        private void tbComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbMessage.Text))
            {
                btnPopupPostMessage.IsEnabled = false;
            }
            else
            {
                btnPopupPostMessage.IsEnabled = true;
            }
        }

        private void btnEmoticon_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                fly_PanelEmoticon.Hide();
            }
        }

        public async void OpenBrowser(string url)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }

        public void DisplayPopupPostMessage()
        {
            myGridPostMessage.Height = ActualHeight / 2;
            int marTop = (int)ActualHeight / 4;
            myGridPostMessage.Margin = new Thickness(0, marTop, 0, 0);
            myGridPostMessage.Width = ActualWidth;
            myPopupPostMessage.IsOpen = !myPopupPostMessage.IsOpen;
            tbTitle.Text = TitleMessage;
            tbUser.Text = UserName;
            tbUser.IsEnabled = false;
        }

        public void UpdateContentTextbox(string emotion)
        {
            int currentSelect = tbMessage.SelectionStart;
            tbMessage.Text = tbMessage.Text.Insert(currentSelect, emotion);
            tbMessage.SelectionStart = currentSelect + emotion.Length;
            fly_PanelEmoticon.Hide();
        }
    }
}
