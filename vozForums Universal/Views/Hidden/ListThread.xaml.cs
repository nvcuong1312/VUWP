using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using HtmlAgilityPack;
using System.Net.Http;
using vozForums_Universal.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Hidden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ListThread : Page
    {
        private Helper.HelperHtml helper = new Helper.HelperHtml();
        private string Url;
        private HttpClient client;
        private HtmlAgilityPack.HtmlDocument doc1;
        private List<ThreadListModel> listThread;
        private List<HomeTdModel> homeTd;
        private int idPage = 1;
        private string idBox;
        public ListThread()
        {
            this.InitializeComponent();
            // this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            page_btn_commandbar.AllowFocusOnInteraction = true;
        }
        private void AnalyzeUrl()
        {
            if (idPage == 1)
                Url = "http://thiendia.com/diendan/" + idBox;
            else Url = "http://thiendia.com/diendan/" + idBox + "page-" + idPage;
        }
        public async void load()
        {
            lt_listview_td.IsEnabled = false;
            try
            {
                client = new HttpClient();
                doc1 = new HtmlAgilityPack.HtmlDocument();
                var ResultHmtl = await client.GetStringAsync(Url);
                doc1.LoadHtml(ResultHmtl);
                helper.RemoveComment(doc1);
                page_btn_commandbar.DataContext = idPage;
                Gettile();
                GetListThread();
            }
            catch
            {

            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            idBox = (string)e.Parameter;
            AnalyzeUrl();
            load();
        }

        private void Gettile()
        {
            HtmlNode nodeTitle = (from div in doc1.DocumentNode.Descendants("div") where div.GetAttributeValue("class", "") == "titleBar" select div).FirstOrDefault();
            TitleBox.Text = HtmlEntity.DeEntitize(nodeTitle.InnerText.Trim().ToString());
        }
        private void GetListThread()
        {
            listThread = new List<ThreadListModel>();
            homeTd = new List<HomeTdModel>();
            homeTd.Clear();
            listThread.Clear();

            // Get all subBox

            List<HtmlNode> allNodeTitle, allNodeThread;
            HtmlNode nodeTitle, nodeID, nodeTest;

            allNodeTitle = doc1.DocumentNode.Descendants("div").Where(n => n.GetAttributeValue("class", "") == "nodeText").ToList();
            foreach (var item in allNodeTitle)
            {
                HomeTdModel htdlt = new HomeTdModel();
                //Get list Box
                nodeTitle = item.Descendants("h3").FirstOrDefault();
                htdlt.NameTd = nodeTitle.InnerText.Trim();
                htdlt.NameTd = HtmlEntity.DeEntitize(htdlt.NameTd).ToString();
                nodeID = nodeTitle.Descendants("a").FirstOrDefault();
                htdlt.IdTd = nodeID.Attributes["href"].Value.ToString();

                homeTd.Add(htdlt);
            }

            //////////////////////

            //Get listthread

            /////////////////

            nodeTest = (from form in doc1.DocumentNode.Descendants("form") where form.GetAttributeValue("class", "") == "DiscussionList InlineModForm" select form).FirstOrDefault();
            allNodeThread = nodeTest.Descendants("ol").Where(m => m.GetAttributeValue("class", "") == "discussionListItems").ToList();
            foreach (var item in allNodeThread)
            {
                List<HtmlNode> aa = item.Elements("li").ToList();
                foreach (var itemThread in aa)
                {
                    ThreadListModel thread = new ThreadListModel();

                    //HtmlNode t = itemThread.Element("li");
                    HtmlNode title = (from div in itemThread.Descendants("a") where div.GetAttributeValue("class", "") == "PreviewTooltip" select div).FirstOrDefault();
                    thread.ThreadName = HtmlEntity.DeEntitize(title.InnerText.Trim());
                    thread.ThreadId = title.Attributes["href"].Value.ToString();
                    HtmlNode username = (from a in itemThread.Descendants("a") where a.GetAttributeValue("class", "") == "username" select a).FirstOrDefault();
                    thread.ThreadCreate = HtmlEntity.DeEntitize(username.InnerText.Trim());

                    listThread.Add(thread);

                }


            }
            lvListThread.ItemsSource = listThread;
            lvListThread.IsEnabled = true;

            lt_listview_td.ItemsSource = homeTd;
            lt_listview_td.IsEnabled = true;

        }

        private void tbTitleThiendia_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string urlThread = ((TextBlock)sender).Tag.ToString();
            Frame.Navigate(typeof(Hidden.Thread), urlThread);
        }

        private void firstPage_Click(object sender, RoutedEventArgs e)
        {
            if(idPage != 1)
            {
                idPage = 1;
                AnalyzeUrl();
                load();
            }
        }

        private void lastPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            idPage = idPage + 1;
            AnalyzeUrl();
            load();
        }

        private void pre_btn_Click(object sender, RoutedEventArgs e)
        {
            if(idPage > 1)
            {
                idPage = idPage - 1;
                AnalyzeUrl();
                load();
            }
        }

        private void fl_Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (fl_Page.Text != null)
                {
                    try
                    {
                        idPage = int.Parse(fl_Page.Text);
                        AnalyzeUrl();
                        load();
                        fl_Page.Text = " ";
                    }
                    catch
                    { /*ShowError();*/ }
                }
                else
                {
                    idPage = 1;
                    AnalyzeUrl();
                    load();
                }
                fl_listthread_flyout.Hide();
                fl_Page.Text = " ";
            }
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string sen = ((StackPanel)sender).Tag.ToString();
            Frame.Navigate(typeof(Hidden.ListThread), sen);
        }
    }
}
