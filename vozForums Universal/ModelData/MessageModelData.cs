using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;

namespace vozForums_Universal.ModelData
{
    public class MessageModelData
    {
        private HtmlDocument doc;
        private HtmlHelper helper;
        private AppSettingModel appSetting;

        public MessageModelData(string Content)
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

        public List<ThreadModel> GetMessageData()
        {
            List<ThreadModel> messageData = new List<ThreadModel>();

            List<HtmlNode> nodeSharp, nodeDivCount;
            HtmlNode nodeUser, nodetrSharpp, nodeAvatar;

            List<HtmlNode> ListAllNodePost = doc
                .DocumentNode
                .Descendants("table")
                .Where(n => n.GetAttributeValue("id", "") == "post")
                .Where(n => n.GetAttributeValue("cellpadding", "") == "6")
                .Where(n => n.GetAttributeValue("cellspacing", "") == "1")
                .Where(n => n.GetAttributeValue("border", "") == "0")
                .Where(n => n.GetAttributeValue("width", "") == "100%")
                .Where(n => n.GetAttributeValue("align", "") == "center")
                .ToList();

            foreach (var NodePost in ListAllNodePost)
            {
                ThreadModel MessageInfo = new ThreadModel();

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
                MessageInfo.PositionPost = "#1";

                // Get time
                MessageInfo.TimePost = HtmlEntity.DeEntitize(nodeDivCount[1].InnerText.Trim());

                //Get user comment
                nodeUser = (from a in NodePost
                            .Descendants("a")
                            .Where(a => a.GetAttributeValue("class", "") == "bigusername")
                            select a).FirstOrDefault();

                MessageInfo.Member = HtmlEntity.DeEntitize(nodeUser.InnerText.Trim());

                MessageInfo.MemberID = nodeUser
                    .GetAttributeValue("href", string.Empty)
                    .Split('=')
                    .LastOrDefault();

                MessageInfo.Status = NodePost
                    .Descendants("div")
                    .Where(n => n.GetAttributeValue("id", string.Empty) == "postmenu_")
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
                        MessageInfo.JoinDate = listnode[0].InnerText.Trim();
                        MessageInfo.Location = HtmlEntity.DeEntitize(listnode[1].InnerText.Trim());
                        MessageInfo.TotalPosts = listnode[2].InnerText.Trim();
                    }
                    if (listnode.Count == 3)
                    {
                        MessageInfo.JoinDate = listnode[0].InnerText.Trim();
                        MessageInfo.TotalPosts = listnode[1].InnerText.Trim();
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
                MessageInfo.TypeMember = nodeTypeMember[1].InnerText.Trim();

                //Get ImgAvatar
                nodeAvatar = (from img in NodePost
                              .Descendants("img")
                              .Where(y => y.GetAttributeValue("alt", "") == MessageInfo.Member.Trim() + "'s Avatar")
                              select img)
                              .FirstOrDefault();
                if (nodeAvatar != null)
                {
                    MessageInfo.ImgAvatar = nodeAvatar.Attributes["src"].Value.ToString();
                }
                else
                {
                    MessageInfo.ImgAvatar = null;
                }

                // Get HtmlContent
                HtmlNode nodeHTML = NodePost
                    .Descendants("td")
                    .Where(m => m.GetAttributeValue("id", "") == "td_post_")
                    .FirstOrDefault();

                MessageInfo.ContentComment = nodeHTML.OuterHtml.ToString();

                messageData.Add(MessageInfo);
            }
            return messageData;
        }
    }
}
