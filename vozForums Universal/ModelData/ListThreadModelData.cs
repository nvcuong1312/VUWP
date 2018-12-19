﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;

namespace vozForums_Universal.ModelData
{
    public class ListThreadModelData
    {
        private HtmlDocument doc;
        private HtmlHelper helper;
        private AppSettingModel appSetting;

        public ListThreadModelData(string Content)
        {
            doc = new HtmlDocument();
            helper = new HtmlHelper();
            appSetting = new AppSettingModel();

            doc.LoadHtml(Content);
        }

        public List<ListThreadModel> GetListThreadData(string idBox)
        {
            List<ListThreadModel> listThreadData = new List<ListThreadModel>();

            List<HtmlNode> nodeTd, nodeDivTitle, nodeTitle, nodeSpan;
            HtmlNode nodeLastPost, nodeReply, nodeViews;

            List<HtmlNode> AllNodeThread = doc
                .DocumentNode.Descendants("tbody")
                .Where(n => n.GetAttributeValue("id", Resource.STR_EMPTY) == "threadbits_forum_" + idBox)
                .First()
                .Elements("tr")
                .ToList();
            foreach (HtmlNode c in AllNodeThread)
            {
                ListThreadModel listThreadItem = new ListThreadModel();
                listThreadItem.BackgroundHomeColor = appSetting.BackgroundHomeColor;
                listThreadItem.TextblockExtraThreadColor = appSetting.TextblockExtraThreadColor;
                listThreadItem.TextblockCreateUserColor = appSetting.TextblockCreateUserColor;

                //set Foreground and Sticky
                HtmlNode ForeStick = (from foreground
                                      in c.Descendants("img")
                                      where (foreground.GetAttributeValue("src", Resource.STR_EMPTY) == "images/misc/sticky.gif")
                                      select foreground)
                                      .FirstOrDefault();
                if (ForeStick != null)
                {
                    listThreadItem.Stick = "/Assets/sticky.png";
                    listThreadItem.HeightStick = "18";
                    listThreadItem.WidthStick = "18";
                    listThreadItem.ForegroundStick = "#FE2E2E";
                    listThreadItem.MarginStick = "18,0,0,0";
                }
                else
                {
                    if (appSetting.ThemeSetting == "Dark")
                    {
                        listThreadItem.ForegroundStick = "#3399ff";
                    }
                    else
                    {
                        listThreadItem.ForegroundStick = "#23497C";
                    }
                    listThreadItem.Stick = null;
                    listThreadItem.HeightStick = null;
                    listThreadItem.WidthStick = null;
                    listThreadItem.MarginStick = null;
                }

                nodeTd = c.Elements("td").ToList();
                if (nodeTd[1].Attributes["class"].Value == "alt2")
                {
                    nodeTd.RemoveAt(1);
                }
                if (nodeTd[2].InnerText.Contains("Thread deleted"))
                {
                    continue;
                }

                //get last user post
                nodeLastPost = nodeTd[2];
                listThreadItem.TimePost = HtmlEntity.DeEntitize(nodeLastPost.InnerText.Trim());
                string[] s = listThreadItem.TimePost.Split();
                listThreadItem.TimePost = Resource.STR_LAST_POST + string.Join(Resource.STR_SPACE, s);

                //Get Count Reply
                nodeReply = nodeTd[3];
                listThreadItem.CountReply = nodeReply.InnerText.Trim();

                //get Count Views
                nodeViews = nodeTd[4];
                listThreadItem.CountViews = nodeViews.InnerText.Trim();

                //get titlepost
                nodeDivTitle = nodeTd[1].Elements("div").ToList();
                nodeTitle = nodeDivTitle[0].Elements("a").ToList();
                foreach (HtmlNode title in nodeTitle)
                {
                    listThreadItem.ThreadName += title.InnerText;
                }
                listThreadItem.ThreadName = HtmlEntity.DeEntitize(listThreadItem.ThreadName.Trim());

                // get user create
                nodeSpan = nodeDivTitle[1].Elements("span").ToList();
                if (nodeSpan.Count() == 2)
                {
                    listThreadItem.ThreadCreate = Resource.STR_CREATOR + nodeSpan[1].InnerText;
                    HtmlNode img = nodeSpan[0].Descendants("img").First();
                    listThreadItem.rating = Resource.URL_HOMEPAGE + img.Attributes["src"].Value.ToString();
                }
                else
                {
                    listThreadItem.ThreadCreate = Resource.STR_CREATOR + nodeSpan[0].InnerText;
                }

                //get id thread
                listThreadItem.ThreadId =
                    nodeTd[0].Attributes["id"].Value.Remove(0, 20)
                    + "|"
                    + listThreadItem.CountReply.Replace(",", string.Empty);

                //get extra title
                string extra = nodeTd[1].Attributes["title"].Value.ToString().Trim();
                extra = HtmlEntity.DeEntitize(extra);
                if (extra.Length > Resource.MAX_LENGTH)
                {
                    extra = extra.Substring(0, Resource.MAX_LENGTH);
                }
                string[] a = extra.Split();
                listThreadItem.extraTitle = string.Join(Resource.STR_SPACE, a);

                listThreadData.Add(listThreadItem);
            }
            return listThreadData;
        }

        public string GetTitle()
        {
            HtmlNode title = (from td in doc.DocumentNode.Descendants("td")
                              where td.GetAttributeValue("class", Resource.STR_EMPTY) == "navbar"
                              select td).FirstOrDefault();
            return HtmlEntity.DeEntitize(title.InnerText.Trim());
        }

        public string GetMaxPage()
        {
            HtmlNode mp = (from td
                           in doc.DocumentNode.Descendants("td")
                           where td.GetAttributeValue("style", Resource.STR_SPACE) == "font-weight:normal"
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
    }
}
