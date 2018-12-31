using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using vozForums_Universal.CommonControl;
using Windows.UI.Popups;
using System.Threading.Tasks;
using vozForums_Universal.Controller;
using System.Reflection;
using vozForums_Universal.ModelData;
using Windows.UI.Core;
using System.Diagnostics;
using System.Threading;
using Windows.UI.Xaml.Media;
using MyToolkit.Paging;
using Windows.UI;
using MyToolkit.Paging.Animations;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThreadView : MtPage
    {
        private int CurrentPage = 1;

        private string url;
        private string MaxPage;
        private string idThread;

        private HtmlHelper helper;
        private BookmarkModelData bookMark;
        private ThreadModel thread;
        private AccountHelper myLogin;
        private DefineEmoticon define;
        private AppSettingModel appSetting;
        private ThreadController threadController;

        public ThreadView()
        {
            this.InitializeComponent();

            CurrentPage = 1;
            appBar.Width = ActualWidth;

            btnPopupPostMessage.IsEnabled = false;
            BtnRating.IsEnabled = false;
            comment.IsEnabled = false;

            bookMark = new BookmarkModelData();
            helper = new HtmlHelper();
            thread = new ThreadModel();
            myLogin = new AccountHelper();
            define = new DefineEmoticon();
            appSetting = new AppSettingModel();
            threadController = new ThreadController();

            if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
            {
                fl_Page.AllowFocusOnInteraction = true;
                tbMessage.AllowFocusOnInteraction = true;
                jumpTextBox.AllowFocusOnInteraction = true;
            }

            _instance = this;
            //this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private static ThreadView _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static ThreadView GetInstance()
        {
            return _instance;
        }

        protected override void OnNavigatedTo(MtNavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                string recevice = (string)e.Parameter;
                if (recevice.Contains("|"))
                {
                    idThread = recevice.Split('|')[0];
                    CurrentPage = int.Parse(recevice.Split('|')[1]);
                }
                else
                {
                    idThread = recevice;
                }
                appSetting.Cookies_Vbmultiquote = Resource.STR_EMPTY;
                Loader();
            }
            _instance = this;
        }

        protected override void OnNavigatingFrom(MtNavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                _instance = null;

                mainGridThread.Children.Remove(myWebview);
                helper = null;
                bookMark = null;
                thread = null;
                myLogin = null;
                define = null;
                appSetting = null;
                threadController = null;
                myWebview = null;
                tbMessage.Text = Resource.STR_EMPTY;
                if (wbPopup != null)
                {
                    wbPopup.Navigate(new Uri("about:blank"));
                    wbPopup = null;
                }
                GC.Collect();
            }
            else
            {
                if (wbPopup != null)
                {
                    wbPopup.Navigate(new Uri("about:blank"));
                }
            }
        }

        private async void Loader()
        {
            pre_btn.IsEnabled = false;
            next_btn.IsEnabled = false;
            pgbarLoading.IsIndeterminate = true;
            pgbarLoading.Visibility = Visibility.Visible;
            tbMessage.Text = string.Empty;
            StatusButton();
            string contentHtml = Resource.STR_EMPTY;
            url = Resource.URL_THREAD.Replace("{rpIdThread}", idThread).Replace("{rpIDPage}", CurrentPage.ToString());
            try
            {
                await Task.Run(() => threadController.GetContent(url, ref contentHtml));
                if (!string.IsNullOrEmpty(contentHtml) && contentHtml != Resource.STR_ERROR)
                {
                    appSetting.Token = AccountHelper.GetToken(contentHtml);

                    helper.GetMessagePrivate(contentHtml);

                    ThreadModelData threadModelData = new ThreadModelData(contentHtml);

                    // Title
                    TitleThread.Text = threadModelData.TitleThread();
                    if (TitleThread.Text == Resource.STR_VB_MSG)
                    {
                        DialogResult.DialogOnlyOk(Resource.STR_THREAD_DELETED);
                        return;
                    }

                    // Get max page
                    var CurrentPagePeerMaxPage = threadModelData.GetMaxPage();
                    fl_page_btnflyout.DataContext = CurrentPagePeerMaxPage;
                    MaxPage = CurrentPagePeerMaxPage.Split('/').LastOrDefault();

                    // Display Content
                    myWebview.NavigateToString(helper.FullPageThreadHtml(threadModelData.GetThreadData()));

                    pre_btn.IsEnabled = true;
                    next_btn.IsEnabled = true;
                }
                else if (contentHtml == Resource.STR_ERROR)
                {
                    TitleThread.Text = Resource.STR_ERROR;
                }
            }
            catch (Exception)
            {
                TitleThread.Text = Resource.STR_ERROR;
            }
            pgbarLoading.IsIndeterminate = false;
            pgbarLoading.Visibility = Visibility.Collapsed;
        }

        private void StatusButton()
        {
            if (appSetting.Token.Length >= 10)
            {
                tbMessage.IsEnabled = true;
                btnEmoticon.IsEnabled = true;
                comment.IsEnabled = true;
                BtnRating.IsEnabled = true;
            }
            else
            {
                tbMessage.IsEnabled = false;
                btnPopupPostMessage.IsEnabled = false;
                comment.IsEnabled = false;
                btnEmoticon.IsEnabled = false;
                BtnRating.IsEnabled = false;
            }
        }

        //private void GetMaxPage()
        //{
        //    string CurrentPagePeerMaxPage = threadModelData.GetMaxPage();
        //    if (CurrentPagePeerMaxPage != "1")
        //    {
        //        Int32 x = Int32.Parse(CurrentPagePeerMaxPage.Split('/').Last());
        //        MaxPage = x.ToString();
        //    }
        //    else
        //    {
        //        MaxPage = "1";
        //    }

        //    fl_page_btnflyout.DataContext = CurrentPagePeerMaxPage;
        //}

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < int.Parse(MaxPage))
            {
                CurrentPage++;
                Loader();
            }
        }

        private void pre_btn_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage >= 2)
            {
                CurrentPage--;
                Loader();
            }
        }

        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = int.Parse(MaxPage);
            Loader();
            fl_flyout.Hide();
        }

        private void FirstPage_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
            Loader();
            fl_flyout.Hide();
        }

        private void fl_Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            fl_Page.IsEnabled = true;
        }

        private void fl_Page_KeyDown(object sender, KeyRoutedEventArgs e)
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
                        fl_flyout.Hide();
                        appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
                        fl_Page.Text = Resource.STR_SPACE;
                        return;
                    }
                    if (tempCurrentPage > int.Parse(MaxPage) || tempCurrentPage < 1)
                    {
                        fl_flyout.Hide();
                        appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
                        fl_Page.Text = Resource.STR_SPACE;
                        return;
                    }
                    CurrentPage = tempCurrentPage;
                    Loader();
                    fl_Page.Text = Resource.STR_SPACE;
                }
                fl_flyout.Hide();
                appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
                fl_Page.Text = Resource.STR_SPACE;
            }
        }

        private async void sentMessage_Click(object sender, RoutedEventArgs e)
        {
            tbMessage.IsEnabled = false;
            bool checkDone = false;
            await Task.Delay(TimeSpan.FromSeconds(0.3));
            threadController.PostComment(tbMessage.Text, idThread, ref checkDone);
            if (checkDone)
            {
                Loader();
                tbMessage.Text = string.Empty;
                tbMessage.IsEnabled = true;
                myPopupPostMessage.IsOpen = false;
            }
        }

        public void RatingThread(int rating)
        {
            bool checkDone = false;
            threadController.SendRating(rating, idThread, ref checkDone);
            if (checkDone)
            {
                Loader();
            }
        }

        public void UpdateContentTextbox(string emotion)
        {
            int currentSelect = tbMessage.SelectionStart;
            tbMessage.Text = tbMessage.Text.Insert(currentSelect, emotion);
            tbMessage.SelectionStart = currentSelect + emotion.Length;
            fly_PanelEmoticon.Hide();
        }

        private void btnEmoticon_Click(object sender, RoutedEventArgs e)
        {
            //panelEmoticon.Refresh();
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

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            Loader();
            fl_flyout.Hide();
        }

        private async void openWithBrowser_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }

        private void jumpTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
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
                    // ShowError();
                }
                fl_JumpBox.Hide();
                jumpTextBox.Text = Resource.STR_SPACE;
            }
        }

        private void btnBookmark_Click(object sender, RoutedEventArgs e)
        {
            //bookMark.Add(titleThread, new string[] { idThread, CurrentPage.ToString() });
            bookMark.Add(idThread, TitleThread.Text, CurrentPage.ToString());
        }

        private void btnHambuger_Click(object sender, RoutedEventArgs e)
        {
            MainView.GetInstance().HideOpenSplitView();
        }

        private void myWebview_ScriptNotify(object sender, NotifyEventArgs e)
        {
            string urlInvoke = e.Value;
            threadController.NavigateManager(urlInvoke);
        }

        public void SetUpPopupWebview(string uriSource)
        {
            myPopup.IsOpen = true;
            tbUrlPopup.Text = uriSource;
            wbPopup.Height = ActualHeight - 55;
            wbPopup.Width = ActualWidth;
            wbPopup.Navigate(new Uri(uriSource));
        }

        private void btnClosePopup_Click(object sender, RoutedEventArgs e)
        {
            wbPopup.Navigate(new Uri("about:blank"));
            //wbPopup = null;
            myPopup.IsOpen = false;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (myPopup.IsOpen)
            {
                wbPopup.Height = ActualHeight - 55;
                wbPopup.Width = ActualWidth;
            }
            if (myPopupPostMessage.IsOpen)
            {
                myGridPostMessage.Height = ActualHeight / 2;
                myGridPostMessage.Width = ActualWidth;
                int marTop = (int)ActualHeight / 4;
                myGridPostMessage.Margin = new Thickness(0, marTop, 0, 0);
            }

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

        static string UriToString(Uri uri)
        {
            return (uri != null) ? uri.ToString() : "";
        }

        private void wbPopup_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            prgPopup.IsIndeterminate = true;
            pageIsLoading = true;
            tbUrlPopup.Text = UriToString(args.Uri);
        }

        private void wbPopup_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {

        }

        private void wbPopup_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {

        }

        private void wbPopup_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            prgPopup.IsIndeterminate = false;
        }
        private void wbPopup_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            prgPopup.IsIndeterminate = false;
            pageIsLoading = false;
        }

        private void NavigateBackward_Click()
        {
            if (wbPopup.CanGoBack)
            {
                wbPopup.GoBack();
            }
        }

        private void NavigateForward_Click()
        {
            if (wbPopup.CanGoForward)
            {
                wbPopup.GoForward();
            }
        }

        private bool _pageIsLoading;
        bool pageIsLoading
        {
            get { return _pageIsLoading; }
            set
            {
                _pageIsLoading = value;

                if (!value)
                {
                    NavigateBackButton.IsEnabled = wbPopup.CanGoBack;
                    NavigateForwardButton.IsEnabled = wbPopup.CanGoForward;
                }
            }
        }

        private bool _isSwipedPopup;
        private void SwipeableTextBlock_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e.IsInertial && !_isSwipedPopup)
            {
                var swipedDistance = e.Cumulative.Translation.X;

                if (Math.Abs(swipedDistance) <= 10) return;

                if (swipedDistance > 0)
                {
                    // Goback.
                    if (wbPopup.CanGoBack)
                    {
                        wbPopup.GoBack();
                    }
                }
                else
                {
                    if (wbPopup.CanGoForward)
                    {
                        wbPopup.GoForward();
                    }
                }
                _isSwipedPopup = true;
            }
        }

        private void SwipeableTextBlock_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            _isSwipedPopup = false;
        }

        private void Comment_Click(object sender, RoutedEventArgs e)
        {
            DisplayPopupPostMessage();
        }

        public async void SetContentForPostMessage(string Id)
        {
            pgbarLoading.IsIndeterminate = true;
            pgbarLoading.Visibility = Visibility.Visible;
            string multiquote = appSetting.Cookies_Vbmultiquote;
            if (!string.IsNullOrEmpty(multiquote))
            {
                appSetting.Cookies_Vbmultiquote = multiquote.Remove(0, 3);
            }
            string urlQuote = Resource.URL_GET_CONTENT_MESSAGE.Replace("{rpID}", Id.ToString());
            string contentHTML = string.Empty;
            try
            {
                await Task.Run(() =>
                {
                    threadController.GetContent(urlQuote, ref contentHTML);
                });
                if (!string.IsNullOrEmpty(contentHTML) && contentHTML != Resource.STR_ERROR)
                {
                    tbMessage.Text = helper.GetContenFromQuote(contentHTML);
                    appSetting.Cookies_Vbmultiquote = Resource.STR_EMPTY;
                    DisplayPopupPostMessage();
                }
                else if (contentHTML == Resource.STR_ERROR)
                {
                    //pgbarLoading.IsIndeterminate = false;
                    //pgbarLoading.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                DialogResult.DialogOnlyOk(Resource.STR_ERROR_NETWORK);
            }
            pgbarLoading.IsIndeterminate = false;
            pgbarLoading.Visibility = Visibility.Collapsed;
        }

        public void DisplayPopupPostMessage()
        {
            myGridPostMessage.Height = ActualHeight / 2;
            int marTop = (int)ActualHeight / 4;
            myGridPostMessage.Margin = new Thickness(0, marTop, 0, 0);
            myGridPostMessage.Width = ActualWidth;
            myPopupPostMessage.IsOpen = !myPopupPostMessage.IsOpen;
        }

        private void BtnClosePopupPostMessage_Click(object sender, RoutedEventArgs e)
        {
            myPopupPostMessage.IsOpen = false;
        }

        private void TbMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            //SolidColorBrush brush = (sender as TextBox).Foreground as SolidColorBrush;
            //if (null != brush)
            //{
            //    brush.Color = Colors.Red;
            //}

            //var background = (sender as TextBox).Background as SolidColorBrush;
        }

        private void TbMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            //SolidColorBrush brush = (sender as TextBox).Foreground as SolidColorBrush;
            //if (null != brush)
            //{
            //    brush.Color = Colors.Blue;
            //}
        }
    }
}