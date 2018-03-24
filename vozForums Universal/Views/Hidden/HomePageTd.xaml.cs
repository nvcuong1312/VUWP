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
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace vozForums_Universal.Views.Hidden
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePageTd : Page
    {
        List<HomeTdModel> ListHome;
        Helper.HelperHtml helper = new Helper.HelperHtml();
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        HttpClient client = new HttpClient();
        private string url = "http://thiendia.com/diendan/";
        public HomePageTd()
        {
            this.InitializeComponent();
            load();
        }

        public async void load()
        {
            try
            {
                client = new HttpClient();
                doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(await client.GetStringAsync(url));
                helper.RemoveComment(doc);
                GetAllnodeTitle();
            }
            catch(Exception ex)
            {
                ShowError(ex.ToString());
            }
            
        }
        private async void ShowError(string ex)
        {
            var msg = new MessageDialog(ex);
            var okBtn = new UICommand("Refresh");
            var canBtn = new UICommand("Cancel");
            msg.Commands.Add(okBtn);
            msg.Commands.Add(canBtn);
            IUICommand result = await msg.ShowAsync();
            if(result != null && result.Label == "Refresh")
            {
                load();
            }
        }

        private void GetAllnodeTitle()
        {
            ListHome = new List<HomeTdModel>();
            ListHome.Clear();

            List<HtmlNode> allNodeTitle;
            HtmlNode nodeTitle, nodeID;

            allNodeTitle = doc.DocumentNode.Descendants("div").Where(n => n.GetAttributeValue("class", "") == "nodeText").ToList();
            foreach (var item in allNodeTitle)
            {
                HomeTdModel htd = new HomeTdModel();

                //Get list Box
                nodeTitle = item.Descendants("h3").FirstOrDefault();

                htd.NameTd = nodeTitle.InnerText.Trim();
                htd.NameTd = HtmlEntity.DeEntitize(htd.NameTd).ToString();

                nodeID = nodeTitle.Descendants("a").FirstOrDefault();
                htd.IdTd = nodeID.Attributes["href"].Value.ToString();

                ListHome.Add(htd);
            }   
            Td_lb_views.ItemsSource = ListHome;
            Td_lb_views.IsEnabled = true;

        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string idthread = ((StackPanel)sender).Tag.ToString();
            Frame.Navigate(typeof(Hidden.ListThread), idthread);
        }
    }
}
