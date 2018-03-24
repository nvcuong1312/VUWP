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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Thread : Page
    {
        LoginModel myLogin = new LoginModel();
        DefineEmoticon define = new DefineEmoticon();
        private string url;
        private string idThread;
        private string titleThread = "";
        private int numPage = 1;
        HtmlDocument doc;
        string contentHtml = string.Empty;
        private HelperHtml Hap = new HelperHtml();
        private List<ThreadModel> threads;
        ThreadModel thread = new ThreadModel();
        private string mxp, MaxPage;
        BookMark b = new BookMark();
        AppSettingModel appSetting = new AppSettingModel();
        Windows.Storage.ApplicationDataContainer loginValue = Windows.Storage.ApplicationData.Current.LocalSettings;
        public Thread()
        {
            this.InitializeComponent();
            sentMessage.IsEnabled = false;
            if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
            {
                fl_Page.AllowFocusOnInteraction = true;
                tbComment.AllowFocusOnInteraction = true;
                jumpTextBox.AllowFocusOnInteraction = true;
            }
            _instance = this;
        }

        private static Thread _instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static Thread GetInstance()
        {
            return _instance;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string recevice = (string)e.Parameter;
            if (recevice.Contains("|"))
            {
                idThread = recevice.Split('|')[0];
                numPage = int.Parse(recevice.Split('|')[1]);
            }
            else idThread = recevice;
            AnalyzeUrlThread();
            Loader();
        }
        private void AnalyzeUrlThread()
        {
            if (numPage == 1)
                url = "https://vozforums.com/showthread.php?t=" + idThread;
            else
                url = "https://vozforums.com/showthread.php?t=" + idThread + "&page=" + numPage.ToString();
        }
        private async void Loader()
        {
            lv_Post.IsEnabled = false;
            pgRing.IsActive = true;
            tbComment.Text = string.Empty;
            
            try
            {   
                doc = new HtmlAgilityPack.HtmlDocument();
                await Task.Run(() => ThreadController.GetContent(url, ref contentHtml));
                doc.LoadHtml(contentHtml);
                Hap.RemoveComment(doc);
                Hap.AnalyzeEmoticon(doc);
                Hap.RemoveQuoteButton(doc);

                if (!string.IsNullOrEmpty(appSetting.token))
                {
                    if (appSetting.token == "guest")
                    {
                        tbComment.IsEnabled = false;
                        sentMessage.IsEnabled = false;
                        btnEmoticon.IsEnabled = false;
                    }
                    else
                    {
                        tbComment.IsEnabled = true;
                        btnEmoticon.IsEnabled = true;
                    }
                }
                else
                {
                    tbComment.IsEnabled = false;
                    sentMessage.IsEnabled = false;
                    btnEmoticon.IsEnabled = false;
                }

                GetMaxPage();
                GetTitleThread();
                GetComment();
                CheckRate();
                if (string.IsNullOrEmpty(mxp) == false) mxp = mxp.Replace("of", "/").Replace("Page", string.Empty).Replace(" ", string.Empty).Trim();
                fl_page_btnflyout.DataContext = mxp;
                fly_PanelEmoticon.Hide();

            }
            catch (Exception ex)
            {
                LogError("Lỗi mạng: \n * Kiểm tra lại kết nối, DNS \n * Có thể lỗi máy chủ. \n *********** \n Chi tiết lỗi: \n" + ex.ToString());
            }
        }

        private void CheckRate()
        {
            //var checkRate = (from rate in doc.DocumentNode.Descendants("form") where (rate.GetAttributeValue("id", "") == "showthread_threadrate_form") select rate).FirstOrDefault();       
            //if (HtmlEntity.DeEntitize(checkRate.InnerText.Trim()).Contains("Excellen"))
            //    BtnRating.IsEnabled = true;
            //else
            //    BtnRating.IsEnabled = false;

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

        private void GetTitleThread()
        {
            try
            {
                var Title = (from td in doc.DocumentNode.Descendants("td") where (td.GetAttributeValue("class", "") == "navbar") select td).FirstOrDefault();
                TitleThread.Text = HtmlEntity.DeEntitize(Title.InnerText.Trim());
                titleThread = HtmlEntity.DeEntitize(Title.InnerText.Trim());
                if (TitleThread.Text == "vBulletin Message")
                    ShowEmptyThread();
            }
            catch { }


        }
        private async void ShowEmptyThread()
        {
            var msg = new MessageDialog("Bị Min/Mod del bài mất rồi .:'<");
            var okbtn = new UICommand("Ok");
            msg.Commands.Add(okbtn);
            IUICommand result = await msg.ShowAsync();
        }
        private async void GetComment()
        {
            // InlineUIContainer inline = new InlineUIContainer();
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                threads = new List<ThreadModel>();
                threads.Clear();

                List<HtmlNode> nodeCm, nodeSharp, nodeDivCount;
                HtmlNode nodeUser, nodetrSharpp, nodeAvatar;

                List<HtmlNode> nodeTb = doc.DocumentNode.Descendants("table").Where(n => n.GetAttributeValue("class", "") == "tborder voz-postbit").ToList();
                foreach (HtmlNode tb in nodeTb)
                {
                    ThreadModel Threadinfo = new ThreadModel();
                    //List<HtmlNode> test = new List<HtmlNode>();

                    if (appSetting.token.Length > 10) Threadinfo.enableQuote = "true";
                    else Threadinfo.enableQuote = "false";

                    Threadinfo.BackgroundThread = appSetting.BackgroundThread;
                    Threadinfo.TextblockMessageColor = appSetting.TextblockMessageColor;
                    Threadinfo.TextblockQuoteColor = appSetting.TextblockQuoteColor;
                    Threadinfo.BackgroundTimeColor = appSetting.BackgroundTimeColor;
                    Threadinfo.BackgroundPanelUserColor = appSetting.BackgroundPanelUserColor;
                    Threadinfo.TextblockTimePostColor = appSetting.TextblockTimePostColor;

                    Hap.AnalyzeQuote(tb);
                    //Get # comment and timepost
                    List<HtmlNode> first_node = tb.Elements("tr").ToList();
                    nodetrSharpp = first_node[0];
                    nodeSharp = nodetrSharpp.Descendants("td").ToList();
                    nodeDivCount = nodeSharp[0].Elements("div").ToList();
                    // get #
                    Threadinfo.PostCount = HtmlEntity.DeEntitize(nodeDivCount[0].InnerText.Trim());
                    string[] s = Threadinfo.PostCount.Split();
                    Threadinfo.PostCount = string.Join(" ", s);
                    //time
                    Threadinfo.TimePost = HtmlEntity.DeEntitize(nodeDivCount[1].InnerText.Trim());

                    //Get idComment
                    Threadinfo.idComment = tb.Attributes["id"].Value.Remove(0, 4);

                    //Get user comment and iduser
                    nodeUser = (from a in tb.Descendants("a").Where(a => a.GetAttributeValue("class", "") == "bigusername") select a).FirstOrDefault();
                    Threadinfo.UserName = HtmlEntity.DeEntitize(nodeUser.InnerText.Trim());
                    //Threadinfo.idUser = nodeUser.Attributes["href"].Value.ToString().ToLower();
                    HtmlNode nodeiu = (from iu in tb.Descendants("td") where (iu.GetAttributeValue("nowrap", "") == "nowrap" && iu.GetAttributeValue("valign", "") == "top") select iu).FirstOrDefault();
                    List<HtmlNode> nodeDivinNodeiu = nodeiu.Elements("div").ToList();
                    foreach (var a in nodeDivinNodeiu)
                    {
                        List<HtmlNode> listnode = a.Elements("div").ToList();
                        if (listnode.Count == 4)
                        {
                            Threadinfo.JoinDate = listnode[0].InnerText.Trim();
                            Threadinfo.location = HtmlEntity.DeEntitize(listnode[1].InnerText.Trim());
                            Threadinfo.totalPosts = listnode[2].InnerText.Trim();
                        }
                        if (listnode.Count == 3)
                        {
                            Threadinfo.JoinDate = listnode[0].InnerText.Trim();
                            Threadinfo.totalPosts = listnode[1].InnerText.Trim();
                        }
                    }
                    List<HtmlNode> hn = tb.Descendants("td").Where(hnx => hnx.GetAttributeValue("nowrap", "") == "nowrap").ToList();
                    List<HtmlNode> nodeTypeMember = hn[0].Descendants("div").ToList();
                    try
                    {
                        Threadinfo.TypeMember = nodeTypeMember[1].InnerText.Trim();
                    }
                    catch
                    { }

                    //Get ImgAvatar
                    nodeAvatar = (from img in tb.Descendants("img").Where(y => y.GetAttributeValue("alt", "") == Threadinfo.UserName.Trim() + "'s Avatar") select img).FirstOrDefault();
                    if (nodeAvatar != null)
                    {
                        Threadinfo.ImgAvatar = nodeAvatar.Attributes["src"].Value.ToString();
                    }
                    else Threadinfo.ImgAvatar = null;

                    nodeCm = tb.Descendants("div").Where(m => m.GetAttributeValue("class", "") == "voz-post-message").ToList();
                    foreach (var x in nodeCm)
                    {
                        Threadinfo.htmlcontent = x.OuterHtml.ToString();
                    }

                    threads.Add(Threadinfo);
                }
                lv_Post.ItemsSource = threads;
                pgRing.IsActive = false;
                lv_Post.IsEnabled = true;
            });

        }

        private void GetMaxPage()
        {
            HtmlNode mp = (from td in doc.DocumentNode.Descendants("td") where td.GetAttributeValue("style", " ") == "font-weight:normal" && td.GetAttributeValue("class", " ") == "vbmenu_control" select td).FirstOrDefault();
            if (mp != null)
            {
                mxp = mp.InnerText;
                Int32 x = Int32.Parse(mxp.Split(' ').Last());
                MaxPage = x.ToString();
            }
            else MaxPage = 1.ToString();

        }
        private void pre_btn_Click(object sender, RoutedEventArgs e)
        {
            Previous();
        }

        private void Previous()
        {
            if (numPage == 1) numPage = 1;
            if (numPage >= 1)
            {
                numPage--;
                AnalyzeUrlThread();
                Loader();
            }
        }

        private void fl_page_btn_Click(object sender, RoutedEventArgs e)
        {

            if (fl_Page.Text != " ")
            {
                numPage = int.Parse(fl_Page.Text);
                AnalyzeUrlThread();
                Loader();
                fl_Page.Text = " ";
            }
            else
            {
                numPage = 1;
                AnalyzeUrlThread();
                Loader();
            }
            fl_flyout.Hide();

        }

        private void lp_click_btn_Click(object sender, RoutedEventArgs e)
        {

            numPage = int.Parse(MaxPage);
            AnalyzeUrlThread();
            Loader();
            fl_flyout.Hide();
        }

        private void fp_click_btn_Click(object sender, RoutedEventArgs e)
        {

            numPage = 1;
            AnalyzeUrlThread();
            Loader();
            fl_flyout.Hide();
        }

        private void fl_Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            fl_Page.IsEnabled = true;
        }

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void Next()
        {
            numPage++;
            AnalyzeUrlThread();
            Loader();
        }

        private void Quote_Click(object sender, RoutedEventArgs e)
        {
            myLogin.LoginMethod(LoginModel.userName,LoginModel.passWord, true);
            string Quote_Id = ((Button)sender).Tag.ToString();
            string Quote_Url = "https://vozforums.com/newreply.php?do=newreply&p=" + Quote_Id;
            GetContenFromQuote(Quote_Url);
        }

        private void GetContenFromQuote(string urlQuote)
        {
            HtmlDocument docQuote = new HtmlDocument();
            string contentHtml = string.Empty;
            ThreadController.GetContent(urlQuote, ref contentHtml);
            docQuote.LoadHtml(contentHtml);
            var contentQuote = (from tx in docQuote.DocumentNode.Descendants("textarea") where (tx.GetAttributeValue("id", "") == "vB_Editor_001_textarea") select tx).FirstOrDefault();
            tbComment.Text = HtmlEntity.DeEntitize(contentQuote.InnerText.Trim());
        }

        private void fl_Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                fly_PanelEmoticon.Hide();

                try
                {
                    if (fl_Page.Text != " ")
                    {
                        numPage = int.Parse(fl_Page.Text);
                        AnalyzeUrlThread();
                        Loader();
                        fl_Page.Text = " ";
                    }
                    else
                    {
                        numPage = 1;
                        AnalyzeUrlThread();
                        Loader();
                    }
                }
                catch
                {
                    ShowError("Input diff value! Please!");
                }
                fl_flyout.Hide();
                appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
                fl_Page.Text = " ";
            }

        }
        public async void ShowError(string input)
        {
            var msg = new MessageDialog(input);
            var okbtn = new UICommand("Ok");
            msg.Commands.Add(okbtn);
            IUICommand result = await msg.ShowAsync();
        }      
        private async void sentMessage_Click(object sender, RoutedEventArgs e)
        {
            tbComment.IsEnabled = false;
            lv_Post.IsEnabled = false;
            bool checkDone = false;
            await Task.Delay(TimeSpan.FromSeconds(0.3));
            ThreadController.SendMessage(thread.Message(tbComment.Text), idThread, ref checkDone);
            if (checkDone)
            {
                Loader();
                tbComment.Text = string.Empty;
                tbComment.IsEnabled = true;
            }           
        }

        private void RatingMethod(int rating)
        {
            bool checkDone = false;
            ThreadController.SendRating(rating, idThread, ref checkDone);
            if (checkDone)
            {
                Loader();
            }
        }

        public void UpdateContentTextbox(string emotion)
        {
            tbComment.Text = tbComment.Text + emotion;           
        }

        private void btnEmoticon_Click(object sender, RoutedEventArgs e)
        {
            panelEmoticon.Refresh();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (appBar.ClosedDisplayMode == AppBarClosedDisplayMode.Compact) appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Minimal;
            else appBar.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
        }

        private void tbComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbComment.Text))
                sentMessage.IsEnabled = false;
            else
                sentMessage.IsEnabled = true;
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                fly_PanelEmoticon.Hide();
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
            AnalyzeUrlThread();
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
                    Frame.Navigate(typeof(ListThread), s);
                }
                catch
                {
                    // ShowError();
                }
                fl_JumpBox.Hide();
                jumpTextBox.Text = " ";
            }
        }

        private void btnBookmark_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (numPage > 1)
                {
                    b.Add(titleThread, idThread + "|" + numPage);
                }
                else
                {
                    b.Add(titleThread, idThread);
                }

            }
            catch { }
        }
    }
}
