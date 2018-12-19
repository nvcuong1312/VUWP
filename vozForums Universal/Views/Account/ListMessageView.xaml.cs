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
    public sealed partial class ListMessageView : MtPage
    {
        private string url;

        private HtmlHelper helper;
        private AppSettingModel appSetting;
        private ListMessageController messageController;

        public ListMessageView()
        {
            this.InitializeComponent();

            helper = new HtmlHelper();
            appSetting = new AppSettingModel();
            messageController = new ListMessageController();
        }

        protected override void OnNavigatedTo(MtNavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                Loader(true);
            }
            MainView.GetInstance().UpdatePosSelectedListView(Resource.ID_MESSAGE.ToString());
        }

        private async void Loader(bool isInbox)
        {
            url = isInbox ? Resource.URL_MESSAGE_IN : Resource.URL_MESSAGE_OUT;
            Message.Text = isInbox ? Resource.STR_INBOX : Resource.STR_OUTBOX;
            pgbarLoading.Visibility = Visibility.Visible;
            pgbarLoading.IsIndeterminate = true;
            Outbox.IsEnabled = false;
            Inbox.IsEnabled = false;
            string contentHtml = Resource.STR_EMPTY;

            try
            {
                await Task.Run(() => messageController.GetContent(url, ref contentHtml));
                if (!string.IsNullOrEmpty(contentHtml) && contentHtml != Resource.STR_ERROR)
                {
                    appSetting.Token = AccountHelper.GetToken(contentHtml);

                    helper.GetMessagePrivate(contentHtml);

                    ListMessageModelData listThreadModelData = new ListMessageModelData(contentHtml);

                    LvMessageList.ItemsSource = listThreadModelData.GetListMessagesData();
                }
                else if (contentHtml == Resource.STR_ERROR)
                {
                    Message.Text = Resource.STR_ERROR;
                }
            }
            catch (Exception ex)
            {
                Message.Text = Resource.STR_ERROR;
            }

            Outbox.IsEnabled = true;
            Inbox.IsEnabled = true;
            pgbarLoading.IsIndeterminate = false;
            pgbarLoading.Visibility = Visibility.Collapsed;
        }

        private void Inbox_Click(object sender, RoutedEventArgs e)
        {
            Loader(true);
        }

        private void Outbox_Click(object sender, RoutedEventArgs e)
        {
            Loader(false);
        }

        private void LvMessageList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LvMessageList.SelectedIndex != -1)
            {
                var listview = sender as ListView;
                var messageModel = listview.SelectedValue as ListMessageModel;
                var Id = messageModel.ID;
                var title = messageModel.Title;
                var User = messageModel.UserName;
                Frame.NavigateAsync(typeof(MessageView), new object[] 
                {
                    1, Id, title, User
                });
                LvMessageList.SelectedIndex = -1;
            }
        }

        private void BtnNewPM_Click(object sender, RoutedEventArgs e)
        {
            Frame.NavigateAsync(typeof(MessageView), new object[] { 2 });
        }
    }
}
