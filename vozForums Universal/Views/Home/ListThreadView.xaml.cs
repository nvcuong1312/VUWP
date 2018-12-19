using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using vozForums_Universal.Model;
using vozForums_Universal.Helper;
using vozForums_Universal.Controller;
using System.Threading.Tasks;
using vozForums_Universal.CommonControl;
using System.Threading;
using vozForums_Universal.ModelData;
using Windows.UI.Xaml.Media.Animation;
using MyToolkit.Paging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class ListThreadView : MtPage
    {
        private int idBox;
        private int CurrentPage;

        private string url;
        private string MaxPage;
        private string countRep;
        private string iconcolor;

        private HtmlHelper helper;
        private AppSettingModel appSetting;
        private ListThreadController threadController;

        public ListThreadView()
        {
            this.InitializeComponent();
            CurrentPage = 1;
            appBar.Width = ActualWidth;
            countRep = Resource.STR_EMPTY;

            helper = new HtmlHelper();
            appSetting = new AppSettingModel();
            threadController = new ListThreadController();
            iconcolor = appSetting.ThemeSetting;
            new_thread.Visibility = Visibility.Collapsed;

            if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
            {
                page_btn_commandbar.AllowFocusOnInteraction = true;
                getIdthread.AllowFocusOnInteraction = true;
                jumpTextBox.AllowFocusOnInteraction = true;
            }
        }

        protected override void OnNavigatedTo(MtNavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                idBox = (int)e.Parameter;
                CurrentPage = 1;
                Loader();
            }
            MainView.GetInstance().UpdatePosSelectedListView(idBox.ToString());
        }

        protected override void OnNavigatingFrom(MtNavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                helper = null;
                threadController = null;
                GC.Collect();
            }
        }

        private async void Loader()
        {
            pre_btn.IsEnabled = false;
            next_btn.IsEnabled = false;
            lb_views.IsEnabled = false;
            pgbarLoading.Visibility = Visibility.Visible;
            pgbarLoading.IsIndeterminate = true;
            string contentHtml = Resource.STR_EMPTY;

            url = Resource.URL_LIST_THREAD.Replace("{rpIDBox}", idBox.ToString()).Replace("{rpIDPage}", CurrentPage.ToString());
            try
            {
                await Task.Run(() => threadController.GetContent(url, ref contentHtml));
                if (!string.IsNullOrEmpty(contentHtml) && contentHtml != Resource.STR_ERROR)
                {
                    appSetting.Token = AccountHelper.GetToken(contentHtml);

                    helper.GetMessagePrivate(contentHtml);

                    ListThreadModelData listThreadModelData = new ListThreadModelData(contentHtml);

                    tblTitle.Text = listThreadModelData.GetTitle();

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                    {
                        lb_views.ItemsSource = listThreadModelData.GetListThreadData(idBox.ToString());
                    });

                    var CurrentPagePeerMaxPage = listThreadModelData.GetMaxPage();
                    page_btn_commandbar.DataContext = CurrentPagePeerMaxPage;
                    MaxPage = CurrentPagePeerMaxPage.Split('/').LastOrDefault();

                    pre_btn.IsEnabled = true;
                    next_btn.IsEnabled = true;
                }
                else if (contentHtml == Resource.STR_ERROR)
                {
                    tblTitle.Text = Resource.STR_ERROR;
                }
            }
            catch (Exception ex)
            {
                tblTitle.Text = Resource.STR_ERROR;
            }

            if (appSetting.TotalPosts >= Resource.TOTAL_POST_URL_IMAGE
                && appSetting.Token.Length > 10)
            {
                new_thread.Visibility = Visibility.Visible;
            }
            else
            {
                new_thread.Visibility = Visibility.Collapsed;
            }

            lb_views.IsEnabled = true;
            pgbarLoading.IsIndeterminate = false;
            pgbarLoading.Visibility = Visibility.Collapsed;
        }

        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < int.Parse(MaxPage))
            {
                CurrentPage++;
                Loader();
            }
        }

        private void Pre_btn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage >= 2)
            {
                CurrentPage--;
                Loader();
            }
        }

        private void StackPanel_Tapped(object sender, RoutedEventArgs e)
        {
            string idThread = ((StackPanel)sender).Tag.ToString();
            Frame.NavigateAsync(typeof(ThreadView), idThread.Split('|').FirstOrDefault());
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = int.Parse(MaxPage);
            Loader();
            fl_listthread_flyout.Hide();
        }

        private void Fl_Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            fl_Page.IsEnabled = true;
        }

        private void GotoThreadwithId_Click(object sender, RoutedEventArgs e)
        {
            string s = getIdthread.Text;
            Frame.NavigateAsync(typeof(ThreadView), s);
        }

        private async void New_thread_Click(object sender, RoutedEventArgs e)
        {
            string ReplyUrl = Resource.URL_NEW_THREAD + idBox;
            await Windows.System.Launcher.LaunchUriAsync(new Uri(ReplyUrl));
        }

        private void Fl_Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (!string.IsNullOrEmpty(fl_Page.Text))
                {
                    int tempCurrentPage = 0;
                    bool isNumber = int.TryParse(fl_Page.Text, out tempCurrentPage);
                    if (!isNumber)
                    {
                        //DialogResult.DialogOnlyOk(Resource.STR_ERROR_INPUT);
                        fl_listthread_flyout.Hide();
                        fl_Page.Text = Resource.STR_SPACE;
                        return;
                    }
                    if (tempCurrentPage > int.Parse(MaxPage) || tempCurrentPage < 1)
                    {
                        fl_listthread_flyout.Hide();
                        fl_Page.Text = Resource.STR_SPACE;
                        return;
                    }
                    CurrentPage = tempCurrentPage;
                    Loader();
                    fl_Page.Text = Resource.STR_SPACE;
                }
                fl_listthread_flyout.Hide();
                fl_Page.Text = Resource.STR_SPACE;
            }
        }

        private void GetIdthread_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                int InThreadInput = 0;
                bool isNumber = int.TryParse(getIdthread.Text, out InThreadInput);
                if (!isNumber)
                {
                    //DialogResult.DialogOnlyOk(Resource.STR_ERROR_INPUT);
                    return;
                }
                Frame.NavigateAsync(typeof(ThreadView), InThreadInput.ToString());
                fl_id_Thread.Hide();
                getIdthread.Text = Resource.STR_SPACE;
            }
        }

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem goLastPage = new MenuFlyoutItem
            {
                Text = Resource.STR_LAST_PAGE,
                Height = 30,
                Width = 100,
                Padding = new Thickness(0, 0, 0, 0)
            };

            countRep = ((StackPanel)sender).Tag.ToString();
            goLastPage.Click += Fly_Click;
            myFlyout.Items.Add(goLastPage);
            myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private void Fly_Click(object sender, RoutedEventArgs e)
        {
            string id = countRep.Split('|').FirstOrDefault();
            int TotalComment = int.Parse(countRep.Split('|')[1]);
            int Surplus = TotalComment % 10;
            int IdLastPageOfThread =
                (TotalComment <= 10)
                ? 1
                : TotalComment / 10;

            if (Surplus == 0 || TotalComment <= 10)
            {
                Frame.NavigateAsync(typeof(ThreadView), id + "|" + IdLastPageOfThread.ToString());
            }
            else
            {
                Frame.NavigateAsync(typeof(ThreadView), id + "|" + (IdLastPageOfThread + 1).ToString());
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (appBar.ClosedDisplayMode == AppBarClosedDisplayMode.Compact)
            {
                appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
            }
            else
            {
                appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Loader();
            fl_listthread_flyout.Hide();
        }

        private async void openWithBrowser_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }

        private void JumpTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                try
                {
                    int s = int.Parse(jumpTextBox.Text);
                    Frame.NavigateAsync(typeof(ListThreadView), s);
                }
                catch
                {
                    //ShowError();
                }
                fl_JumpBox.Hide();
                jumpTextBox.Text = Resource.STR_SPACE;
            }
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            Loader();
            fl_listthread_flyout.Hide();
        }

        private void BtnHambuger_Click(object sender, RoutedEventArgs e)
        {
            MainView.GetInstance().HideOpenSplitView();
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
            appBar.Width = ActualWidth;
        }
    }
}