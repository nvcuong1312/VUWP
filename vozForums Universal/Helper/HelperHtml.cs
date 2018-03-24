using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;
using vozForums_Universal.CommonControl;
namespace vozForums_Universal.Helper
{
    class HelperHtml
    {
        DefineEmoticon defineEmoticon = new DefineEmoticon();
        public void RemoveComment(HtmlDocument doc)
        {
            List<HtmlNode> cmt = doc.DocumentNode.Descendants().Where(n => n.NodeType == HtmlAgilityPack.HtmlNodeType.Comment).ToList();
            foreach (var i in cmt)
            {
                i.Remove();
            }   

        }

        public async Task<int> GetPosts(int userId)
        {
            int posts = 0;
            string url = "https://vozforums.com/member.php?u=" + userId;
            HtmlDocument htmlDocument = new HtmlDocument();
            HttpClient httpClient = new HttpClient();
            htmlDocument.LoadHtml(await httpClient.GetStringAsync(url));

            HtmlNode numPosts = (from td in htmlDocument.DocumentNode.Descendants("dl") where (td.GetAttributeValue("class", "") == "smallfont list_no_decoration profilefield_list") select td).FirstOrDefault();
            List<HtmlNode> node = numPosts.Elements("dd").ToList();
            posts = int.Parse(node[1].InnerText.Trim());
            return posts;
        }

        public void RemoveQuoteButton(HtmlDocument doc)
        {
            List<HtmlNode> removeQuoteButton = (from rq in doc.DocumentNode.Descendants("div") where (rq.GetAttributeValue("style", " ") == "margin-top: 10px" && rq.GetAttributeValue("align", " ") == "right") select rq).ToList();
            foreach (var i in removeQuoteButton)
            {
                i.RemoveAll();
                
            }
        }    
        public void AnalyzeQuote(HtmlNode quote)
        {
            List<HtmlNode> removeQuote = (from rq in quote.Descendants("div") where( rq.GetAttributeValue("class"," ") == "smallfont" && rq.GetAttributeValue("style", " ") == "margin-bottom:2px") select rq).ToList();
            foreach (var i in removeQuote)
            {
                i.RemoveAll();
            }           
        }
        public void AnalyzeEmoticon(HtmlDocument doc)
        {
            foreach (HtmlNode img in doc.DocumentNode.Descendants("img"))
            {
                if (img.Attributes["src"].Value == "images/buttons/viewpost.gif")
                    img.Attributes["src"].Value = "http";
                if (img.Attributes["src"].Value == "images/smilies/brick.png")
                    img.Attributes["src"].Value = "https://vozforums.com/images/smilies/brick.png";
                if (img.Attributes["src"].Value.ToString().Contains("/images/smilies/Off/"))
                {
                    img.Attributes["src"].Value = "ms-appx:///Assets/iconvoz/" + img.Attributes["src"].Value.ToString().Remove(0, 20);

                    img.Attributes["src"].Value = img.Attributes["src"].Value.ToString().Remove(img.Attributes["src"].Value.ToString().Length - 4, 4) + ".png";
                    string t = img.Attributes["src"].Value;
                    if (t.Contains("byebye.png") )
                    { }
                    foreach (var item in defineEmoticon.popo.ToList())
                    {
                        string ga = img.Attributes["src"].Value;
                        if (img.Attributes["src"].Value.ToString().Contains(item))
                        {
                            if ((img.Attributes["src"].Value.ToString().Length == 36) && img.Attributes["src"].Value.ToString().Contains("bye.png"))
                            { continue; }
                            img.Attributes["src"].Value = img.Attributes["src"].Value.ToString().Replace(".png", ".gif");
                            string s = img.Attributes["src"].Value;

                        }
                    }
                }
                if (!img.Attributes["src"].Value.StartsWith("http") && !img.Attributes["src"].Value.ToString().Contains("Assets/iconvoz/"))
                    img.Attributes["src"].Value = "https://vozforums.com/" + img.Attributes["src"].Value;
            }
        }
    }
}
