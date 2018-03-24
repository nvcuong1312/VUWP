using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using vozForums_Universal.Helper;
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

namespace vozForums_Universal.Views.Hidden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Thread : Page
    {
        string urlThread, idThread;
        private HttpClient client;
        private Helper.HelperHtml Hap = new HelperHtml();
        private List<Model.ThreadModel> threads;
        private string mxp, MaxPage;
        private int numBox =1 ;
        HtmlAgilityPack.HtmlDocument doc;
        HttpClient userClient = new HttpClient();

        public Thread()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            idThread = (string)e.Parameter;
            LoadUrl();
            LoadHtml();
        }
        private void LoadUrl()
        {
            if (numBox == 1)
            {
                urlThread = "http://thiendia.com/diendan/" + idThread;
            }
            else
                urlThread = "http://thiendia.com/diendan/" + idThread + "page-" + numBox.ToString() ;

        }
        private async void LoadHtml()
        {
            try
            {
                client = new HttpClient();
                doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(await client.GetStringAsync(urlThread));
                Hap.RemoveComment(doc);
                Hap.AnalyzeEmoticon(doc);
                Hap.RemoveQuoteButton(doc);

                
                fl_page_btnflyout.DataContext = numBox;

                GetComment();
            }
            catch (Exception ex)
            { }

        }

        private void pre_btn_Click(object sender, RoutedEventArgs e)
        {
            if(numBox > 1)
            {
                numBox = numBox - 1;
                LoadUrl();
                LoadHtml();
            }
        }

        private void next_btn_Click(object sender, RoutedEventArgs e)
        {
            numBox = numBox + 1;
            LoadUrl();
            LoadHtml();
        }

        private void GetComment()
        {
            threads = new List<Model.ThreadModel>();
            threads.Clear();

            List<HtmlNode> listNeed;
            listNeed = doc.DocumentNode.Descendants("blockquote").ToList();
            foreach(var item in listNeed)
            {
                Model.ThreadModel th = new Model.ThreadModel();

                th.htmlcontent = item.OuterHtml.ToString();

                threads.Add(th);
            }
            lv_Post.ItemsSource = threads;
            lv_Post.IsEnabled = true;

        }
    }   
}
