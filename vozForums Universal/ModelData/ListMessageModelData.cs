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
    public class ListMessageModelData
    {
        private HtmlDocument doc;
        private HtmlHelper helper;
        private AppSettingModel appSetting;

        public ListMessageModelData(string Content)
        {
            doc = new HtmlDocument();
            helper = new HtmlHelper();
            appSetting = new AppSettingModel();

            doc.LoadHtml(Content);
        }

        public List<ListMessageModel> GetListMessagesData()
        {
            List<ListMessageModel> listMessageData = new List<ListMessageModel>();

            List<HtmlNode> tbodyMessageList = doc
                .DocumentNode
                .Descendants("tbody")
                .Where(n => n.GetAttributeValue("id", "")
                .Contains("collapseobj_pmf"))
                .ToList();

            List<HtmlNode> listTrMessage = new List<HtmlNode>();
            foreach (var tbodyMessage in tbodyMessageList)
            {
                foreach (var trMessage in tbodyMessage.Descendants("tr").ToList())
                {
                    listTrMessage.Add(trMessage);
                }
            }

            foreach (var item in listTrMessage)
            {
                var tdList = item.Descendants("td").ToList();
                if (tdList.Count == 4)
                {
                    HtmlNode tdMessage = tdList[2];
                    var divList = tdMessage.Descendants("div").ToList();
                    if (divList.Count == 2)
                    {
                        ListMessageModel messageData = new ListMessageModel();

                        // Get Date
                        messageData.Date = HtmlEntity.DeEntitize(
                            divList[0]
                            .Descendants("span")
                            .FirstOrDefault()
                            .InnerText.Trim())
                            .Replace("-","/");

                        // Get Title
                        messageData.Title = HtmlEntity.DeEntitize(
                            divList[0]
                            .Descendants("a")
                            .FirstOrDefault()
                            .InnerText.Trim())
                            .Replace("\t", string.Empty)
                            .Replace("\n",string.Empty);

                        // Get ID
                        messageData.ID = HtmlEntity.DeEntitize(
                            divList[0]
                            .Descendants("a")
                            .FirstOrDefault()
                            .Attributes["href"]
                            .Value.Trim()
                            .Split('=')
                            .LastOrDefault());

                        // Check is new message
                        var strongNode = divList[0].Descendants("strong").ToList();
                        messageData.FontWeight = (strongNode.Count == 0) ? "Normal" : "Bold";

                        var spanList = divList[1].Descendants("span").ToList();
                        if (spanList.Count == 2)
                        {
                            // Get Time
                            messageData.Time = HtmlEntity.DeEntitize(spanList[0].InnerText.Trim());

                            // Get UserName
                            messageData.UserName = HtmlEntity.DeEntitize(spanList[1].InnerText.Trim());

                            // Get ShortUserName
                            var temp = messageData.UserName.Split(' ');
                            if (temp.Length == 1)
                            {
                                messageData.ShortUserName = temp[0].ToArray()[0].ToString().ToUpper();
                            }
                            else
                            {
                                messageData.ShortUserName = temp[0].ToArray()[0].ToString().ToUpper() + temp[temp.Length - 1].ToArray()[0].ToString().ToUpper();
                            }
                        }

                        messageData.Date = messageData.Time + Resource.STR_SPACE + messageData.Date;

                        listMessageData.Add(messageData);
                    }
                }
            }
            return listMessageData;
        }
    }
}
