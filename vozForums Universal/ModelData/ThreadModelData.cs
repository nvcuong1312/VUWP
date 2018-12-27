using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Model;
using HtmlAgilityPack;
using vozForums_Universal.Helper;

namespace vozForums_Universal.ModelData
{
    public class ThreadModelData
    {
        private HtmlDocument doc;
        private HtmlHelper helper;
        private AppSettingModel appSetting;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Content"></param>
        public ThreadModelData(string Content)
        {
            doc = new HtmlDocument();
            helper = new HtmlHelper();
            appSetting = new AppSettingModel();

            doc.LoadHtml(Content);
            //helper.RemoveComment(doc);
            helper.AnalyzeURL(doc);
            helper.AnalyzeURLImage(doc);
            helper.AnalyzeContent(doc);
        }

        /// <summary>
        /// Get data of thread
        /// </summary>
        /// <returns></returns>
        public List<ThreadModel> GetThreadData()
        {
            List<ThreadModel> threadData = new List<ThreadModel>();

            List<HtmlNode> nodeSharp, nodeDivCount;
            HtmlNode nodeUser, nodetrSharpp, nodeAvatar;

            List<HtmlNode> ListAllNodePost = doc.DocumentNode
                                       .Descendants("div")
                                       .Where(n => n.GetAttributeValue("id", Resource.STR_EMPTY) == "posts")
                                       .FirstOrDefault()
                                       .Descendants("table")
                                       .Where(n => n.GetAttributeValue("width", Resource.STR_EMPTY) == "100%")
                                       .Where(n => n.GetAttributeValue("cellspacing", Resource.STR_EMPTY) == "1")
                                       .Where(n => n.GetAttributeValue("cellpadding", Resource.STR_EMPTY) == "6")
                                       .Where(n => n.GetAttributeValue("border", Resource.STR_EMPTY) == "0")
                                       .Where(n => n.GetAttributeValue("align", Resource.STR_EMPTY) == "center")
                                       .ToList();

            foreach (var NodePost in ListAllNodePost)
            {
                ThreadModel Threadinfo = new ThreadModel();

                // Get ID Post
                try
                {
                    Threadinfo.PostID = NodePost.GetAttributeValue("id", string.Empty).Replace("post", string.Empty);
                }
                catch (Exception ex)
                {
                    continue;
                }

                // Get # comment and time
                List<HtmlNode> first_node = NodePost.Elements("tr").ToList();
                nodetrSharpp = first_node[0];
                nodeSharp = nodetrSharpp.Descendants("td").ToList();
                nodeDivCount = nodeSharp[0].Elements("div").ToList();
                if (nodeDivCount.Count < 2)
                {
                    continue;
                }

                // Get #
                Threadinfo.PositionPost = string.Join(" ", HtmlEntity.DeEntitize(nodeDivCount[0].InnerText.Trim()).Split());
                // Get time
                Threadinfo.TimePost = HtmlEntity.DeEntitize(nodeDivCount[1].InnerText.Trim());
                // Get idComment
                Threadinfo.PostID = NodePost.Attributes["id"].Value.Remove(0, 4);
                //Get user comment
                nodeUser = (from a in NodePost
                            .Descendants("a")
                            .Where(a => a.GetAttributeValue("class", "") == "bigusername")
                            select a).FirstOrDefault();

                Threadinfo.Member = HtmlEntity.DeEntitize(nodeUser.InnerText.Trim());

                Threadinfo.MemberID = nodeUser
                    .GetAttributeValue("href", string.Empty)
                    .Split('=')
                    .LastOrDefault();

                Threadinfo.Status = NodePost
                    .Descendants("div")
                    .Where(n => n.GetAttributeValue("id", string.Empty) == "postmenu_" + Threadinfo.PostID)
                    .FirstOrDefault()
                    .Descendants("img")
                    .FirstOrDefault()
                    .GetAttributeValue("src", string.Empty);

                // Get infomation user comment
                HtmlNode nodeiu = (from iu
                                   in NodePost.Descendants("td")
                                   where (iu.GetAttributeValue("nowrap", "") == "nowrap" && iu.GetAttributeValue("valign", "") == "top")
                                   select iu).FirstOrDefault();
                List<HtmlNode> nodeDivinNodeiu = nodeiu.Elements("div").ToList();
                foreach (var a in nodeDivinNodeiu)
                {
                    List<HtmlNode> listnode = a.Elements("div").ToList();
                    if (listnode.Count == 4)
                    {
                        Threadinfo.JoinDate = listnode[0].InnerText.Trim();
                        Threadinfo.Location = HtmlEntity.DeEntitize(listnode[1].InnerText.Trim());
                        Threadinfo.TotalPosts = listnode[2].InnerText.Trim();
                    }
                    if (listnode.Count == 3)
                    {
                        Threadinfo.JoinDate = listnode[0].InnerText.Trim();
                        Threadinfo.TotalPosts = listnode[1].InnerText.Trim();
                    }
                }

                // Get type member
                List<HtmlNode> hn = NodePost
                    .Descendants("td")
                    .Where(hnx => hnx.GetAttributeValue("nowrap", "") == "nowrap")
                    .ToList();
                if (hn.Count == 0)
                {
                    continue;
                }
                List<HtmlNode> nodeTypeMember = hn[0].Descendants("div").ToList();
                if (nodeTypeMember.Count < 2)
                {
                    continue;
                }
                Threadinfo.TypeMember = nodeTypeMember[1].InnerText.Trim();

                //Get ImgAvatar
                nodeAvatar = (from img in NodePost
                              .Descendants("img")
                              .Where(y => y.GetAttributeValue("alt", "") == Threadinfo.Member.Trim() + "'s Avatar")
                              select img)
                              .FirstOrDefault();
                if (nodeAvatar != null)
                {
                    Threadinfo.ImgAvatar = nodeAvatar.Attributes["src"].Value.ToString();
                }
                else
                {
                    Threadinfo.ImgAvatar = null;
                }

                // Get HtmlContent
                HtmlNode nodeHTML = NodePost
                    .Descendants("td")
                    .Where(m => m.GetAttributeValue("id", "") == "td_post_" + Threadinfo.PostID)
                    .FirstOrDefault();

                Threadinfo.ContentComment = nodeHTML.OuterHtml.ToString();

                threadData.Add(Threadinfo);
            }
            return threadData;
        }

        /// <summary>
        /// Get max page of thread
        /// </summary>
        /// <returns></returns>
        public string GetMaxPage()
        {
            HtmlNode mp = (from td
                           in doc.DocumentNode.Descendants("td")
                           where td.GetAttributeValue("style", Resource.STR_SPACE) == "font-weight:normal"
                           where td.GetAttributeValue("class", Resource.STR_SPACE) == "vbmenu_control"
                           select td).FirstOrDefault();
            if (mp != null)
            {
                return mp.InnerText
                .Replace("of", "/")
                .Replace("Page", string.Empty)
                .Replace(Resource.STR_SPACE, string.Empty)
                .Trim();
            }
            else
            {
                return "1";
            }
        }

        /// <summary>
        /// Get title of thread
        /// </summary>
        /// <returns></returns>
        public string TitleThread()
        {
            try
            {
                var Title = (from td
                             in doc.DocumentNode.Descendants("td")
                             where (td.GetAttributeValue("class", Resource.STR_EMPTY) == "navbar")
                             select td).FirstOrDefault();

                string titleThread = HtmlEntity.DeEntitize(Title.InnerText.Trim());
                return titleThread;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
