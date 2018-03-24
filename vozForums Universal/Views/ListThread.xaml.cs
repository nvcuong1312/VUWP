using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using System.Net.Http;
using vozForums_Universal.Model;
using vozForums_Universal.Helper;
using Windows.UI.Popups;
using vozForums_Universal.Controller;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class ListThread : Page
    {
        private string iconcolor;
        HelperHtml helper = new HelperHtml();
        string contentHtml = string.Empty;
        private string url;
        private int idBox;
        private int Idpage = 1;
        private HtmlDocument doc;
        private List<ThreadListModel> listThread;
        private int MaxLength = 70;
        private string MaxPage, mxp;
        string countRep = "";
        
        AppSettingModel appSetting = new AppSettingModel();
        public ListThread()
        {
            this.InitializeComponent();
            iconcolor = appSetting.ThemeSetting;
            if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
            {
                page_btn_commandbar.AllowFocusOnInteraction = true;
                getIdthread.AllowFocusOnInteraction = true;
                jumpTextBox.AllowFocusOnInteraction = true;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                idBox = (int)e.Parameter;
                Idpage = 1;
                AnalyzeUrlBox();
                AnalyzeHtml();
            }
        }
        private void AnalyzeUrlBox()
        {
            if (Idpage == 1)
                url = "https://vozforums.com/forumdisplay.php?f=" + idBox.ToString();
            else
                url = "https://vozforums.com/forumdisplay.php?f=" + idBox.ToString() + "&order=desc&page=" + Idpage;
        }
        private async void Loader()
        {
            lb_views.IsEnabled = false;
            pgRing.IsActive = true;
            try
            {
                doc = new HtmlDocument();
                await Task.Run(() => ListThreadController.GetContent(url, ref contentHtml));
                doc.LoadHtml(contentHtml);
                helper.RemoveComment(doc);
                GetMaxPage();
                GetTitle();
                GetListThread();
                if (string.IsNullOrEmpty(mxp) == false) mxp = mxp.Replace("of", "/").Replace("Page", string.Empty).Replace(" ", string.Empty).Trim();
                page_btn_commandbar.DataContext = mxp;
            }
            catch (Exception ex)
            {
                LogError("Lỗi mạng: \n * Kiểm tra lại kết nối, DNS \n * Có thể lỗi máy chủ. \n *********** \n Chi tiết lỗi: \n" + ex.ToString());
            }

        }
        private async void LogError(string ex)
        {
            var msg = new MessageDialog(ex);
            var okBtn = new UICommand("Refresh");
            var canBtn = new UICommand("Cancel");
            msg.Commands.Add(okBtn);
            msg.Commands.Add(canBtn);
            IUICommand result = await msg.ShowAsync();
            if (result != null && result.Label == "Refresh")
                Loader();
        }
        private void AnalyzeHtml()
        {
            Loader();
        }
        private async void GetListThread()
        {
            listThread = new List<ThreadListModel>();
            listThread.Clear();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                List<HtmlNode> nodeTd, nodeDivTitle, nodeTitle, nodeSpan;
                HtmlNode nodeLastPost, nodeReply, nodeViews;
                List<HtmlNode> AllNodeThread = doc.DocumentNode.Descendants("tbody").Where(n => n.GetAttributeValue("id", "") == "threadbits_forum_" + idBox).First().Elements("tr").ToList();
                foreach (HtmlNode c in AllNodeThread)
                {
                    ThreadListModel thread = new ThreadListModel();
                    thread.BackgroundHomeColor = appSetting.BackgroundHomeColor;
                    thread.TextblockExtraThreadColor = appSetting.TextblockExtraThreadColor;
                    thread.TextblockCreateUserColor = appSetting.TextblockCreateUserColor;
                    //set Foreground and Sticky
                    HtmlNode ForeStick = (from foreground in c.Descendants("img") where (foreground.GetAttributeValue("src", "") == "images/misc/sticky.gif") select foreground).FirstOrDefault();
                    if (ForeStick != null)
                    {
                        thread.Stick = "/Assets/sticky.png";
                        thread.HeightStick = "18";
                        thread.WidthStick = "18";
                        thread.ForegroundStick = "#FE2E2E";
                        thread.MarginStick = "18,0,0,0";
                    }
                    else
                    {
                        if (appSetting.ThemeSetting == "Dark")
                        {
                            thread.ForegroundStick = "#3399ff";
                        }
                        else
                        {
                            thread.ForegroundStick = "#23497C";

                        }
                        thread.Stick = null;
                        thread.HeightStick = null;
                        thread.WidthStick = null;
                        thread.MarginStick = null;
                    }


                    nodeTd = c.Elements("td").ToList();
                    if (nodeTd[1].Attributes["class"].Value == "alt2") nodeTd.RemoveAt(1);
                    if (nodeTd[2].InnerText.Contains("Thread deleted")) continue;


                    //get last user post
                    nodeLastPost = nodeTd[2];
                    thread.TimePost = HtmlEntity.DeEntitize(nodeLastPost.InnerText.Trim());
                    string[] s = thread.TimePost.Split();
                    thread.TimePost = "Lastpost: " + string.Join(" ", s);

                    //Get Count Reply
                    nodeReply = nodeTd[3];
                    thread.CountReply = nodeReply.InnerText.Trim();
                    //get Count Views
                    nodeViews = nodeTd[4];
                    thread.CountViews = nodeViews.InnerText.Trim();

                    //get titlepost
                    // nodeDivTitle chua danh sach the "div" cua nodeTd[1]
                    //
                    nodeDivTitle = nodeTd[1].Elements("div").ToList();
                    nodeTitle = nodeDivTitle[0].Elements("a").ToList();
                    foreach (HtmlNode title in nodeTitle)
                    {
                        thread.ThreadName += title.InnerText;
                    }
                    thread.ThreadName = HtmlEntity.DeEntitize(thread.ThreadName.Trim());
                    // get user create
                    nodeSpan = nodeDivTitle[1].Elements("span").ToList();
                    if (nodeSpan.Count() == 2)
                    {
                        thread.ThreadCreate = "Người tạo:  " + nodeSpan[1].InnerText;
                        HtmlNode img = nodeSpan[0].Descendants("img").First();
                        thread.rating = "https://vozforums.com/" + img.Attributes["src"].Value.ToString();
                    }
                    else
                        thread.ThreadCreate = "Người tạo:  " + nodeSpan[0].InnerText;
                    //get id thread
                    thread.ThreadId = nodeTd[0].Attributes["id"].Value.Remove(0, 20) + "|" + thread.CountReply.Replace(",", string.Empty);
                    //get extra title
                    String extra = nodeTd[1].Attributes["title"].Value.ToString().Trim();
                    extra = HtmlEntity.DeEntitize(extra);
                    if (extra.Length > MaxLength)
                        extra = extra.Substring(0, MaxLength);
                    string[] a = extra.Split();
                    thread.extraTitle = string.Join(" ", a);
                    listThread.Add(thread);
                }
                lb_views.ItemsSource = listThread;
                lb_views.IsEnabled = true;
                pgRing.IsActive = false;
            });
        }
        private void GetTitle()
        {
            HtmlNode title = (from td in doc.DocumentNode.Descendants("td")
                              where td.GetAttributeValue("class", "") == "navbar"
                              select td).FirstOrDefault();
            resultHtml.Text = HtmlEntity.DeEntitize(title.InnerText.Trim());
        }
        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }
        private void Next()
        {
            Idpage++;
            AnalyzeUrlBox();
            AnalyzeHtml();
        }
        private void pre_btn_Click(object sender, RoutedEventArgs e)
        {
            Previous();
        }
        private void Previous()
        {
            if (Idpage == 1) Idpage = 1;
            if (Idpage > 1)
            {
                Idpage--;
                AnalyzeUrlBox();
                AnalyzeHtml();
            }
        }
        private void GetMaxPage()
        {
            HtmlNode mp = (from td in doc.DocumentNode.Descendants("td") where td.GetAttributeValue("style", " ") == "font-weight:normal" select td).FirstOrDefault();
            mxp = mp.InnerText;
            Int32 x = Int32.Parse(mxp.Split(' ').Last());
            MaxPage = x.ToString();
        }
        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string idThread = ((StackPanel)sender).Tag.ToString();
            Frame.Navigate(typeof(Thread), idThread.Split('|')[0]);
        }

        private void lastPage_Click(object sender, RoutedEventArgs e)
        {
            Idpage = int.Parse(MaxPage);
            AnalyzeUrlBox();
            AnalyzeHtml();
            fl_listthread_flyout.Hide();
        }

        private void fl_Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            fl_Page.IsEnabled = true;
        }
        private void gotoThreadwithId_Click(object sender, RoutedEventArgs e)
        {
            string s = getIdthread.Text;
            Frame.Navigate(typeof(Thread), s);
        }
        private async void new_thread_Click(object sender, RoutedEventArgs e)
        {
            string ReplyUrl = "https://vozforums.com/newthread.php?do=newthread&f=" + idBox;
            await Windows.System.Launcher.LaunchUriAsync(new Uri(ReplyUrl));
        }
        private void fl_Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (!string.IsNullOrEmpty(fl_Page.Text))
                {
                    try
                    {
                        Idpage = int.Parse(fl_Page.Text);
                        AnalyzeUrlBox();
                        AnalyzeHtml();
                        fl_Page.Text = " ";
                    }
                    catch
                    { ShowError(); }
                }
                else
                {
                    Idpage = 1;
                    AnalyzeUrlBox();
                    AnalyzeHtml();
                }
                fl_listthread_flyout.Hide();
                fl_Page.Text = " ";
            }

        }
        public async void ShowError()
        {
            var msg = new MessageDialog("Input diff value! Please!");
            var okbtn = new UICommand("I Know");
            msg.Commands.Add(okbtn);
            IUICommand result = await msg.ShowAsync();
        }

        private void getIdthread_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                try
                {
                    int s = int.Parse(getIdthread.Text);
                    Frame.Navigate(typeof(Thread), s.ToString());
                }
                catch
                {
                    ShowError();
                }
                fl_id_Thread.Hide();
                getIdthread.Text = " ";
            }
        }

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem goLastPage = new MenuFlyoutItem { Text = "Last page", Height = 30, Width = 100, Padding = new Thickness(0, 0, 0, 0) };
            countRep = ((StackPanel)sender).Tag.ToString();
            goLastPage.Click += fly_Click;
            myFlyout.Items.Add(goLastPage);
            myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }
        private void fly_Click(object sender, RoutedEventArgs e)
        {
            string id = countRep.Split('|')[0];
            int endpage = (int.Parse(countRep.Split('|')[1]) / 10) + 1;
            Frame.Navigate(typeof(Thread), id + "|" + endpage);
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (appBar.ClosedDisplayMode == AppBarClosedDisplayMode.Compact) appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
            else appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
        }
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            AnalyzeUrlBox();
            AnalyzeHtml();
            fl_listthread_flyout.Hide();
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
                    Frame.Navigate(typeof(ListThread), s);
                }
                catch
                {
                    //ShowError();
                }
                fl_JumpBox.Hide();
                jumpTextBox.Text = " ";
            }
        }
        private void firstPage_Click(object sender, RoutedEventArgs e)
        {
            Idpage = 1;
            AnalyzeUrlBox();
            AnalyzeHtml();
            fl_listthread_flyout.Hide();
        }
    }
}
