﻿using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;
using vozForums_Universal.CommonControl;
using vozForums_Universal.Connect;
using vozForums_Universal.Model;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.IO;
using Windows.Security.ExchangeActiveSyncProvisioning;
using vozForums_Universal.Views;

namespace vozForums_Universal.Helper
{
    class HtmlHelper : UserControl
    {
        private string ThreadHTML = string.Empty;
        private string ThreadComment = string.Empty;

        ConnectServer Server;
        AppSettingModel appSetting;
        DefineEmoticon defineEmoticon;
        EasClientDeviceInformation EasClientDvInfo;

        public HtmlHelper()
        {
            Server = new ConnectServer();
            appSetting = new AppSettingModel();
            defineEmoticon = new DefineEmoticon();
            EasClientDvInfo = new EasClientDeviceInformation();
            GetTemplateHTML();
        }

        async void GetTemplateHTML()
        {
            string pathThreadHTML = @"Assets\HTML\ThreadHTML.txt";
            string pathThreadComment = @"Assets\HTML\ThreadComment.txt";
            StorageFolder InstallationFolder = Package.Current.InstalledLocation;
            StorageFile file = await InstallationFolder.GetFileAsync(pathThreadHTML);
            if (File.Exists(file.Path))
            {
                ThreadHTML = File.ReadAllText(file.Path);
            }

            file = await InstallationFolder.GetFileAsync(pathThreadComment);
            if (File.Exists(file.Path))
            {
                ThreadComment = File.ReadAllText(file.Path);
            }
        }

        public void RemoveComment(HtmlDocument doc)
        {
            List<HtmlNode> cmt = doc
                .DocumentNode
                .Descendants()
                .Where(n => n.NodeType == HtmlNodeType.Comment)
                .ToList();
            foreach (var i in cmt)
            {
                i.Remove();
            }
        }

        public async void GetPosts(string userId)
        {
            int posts = 0;
            string url = "https://forums.voz.vn/member.php?u=" + userId;
            string content = Resource.STR_EMPTY; ;
            await Task.Run(() => Server.GetContent(url, ref content));
            if (content != Resource.STR_ERROR)
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(content);
                HtmlNode numPosts = (from td
                                     in htmlDocument.DocumentNode.Descendants("dl")
                                     where (td.GetAttributeValue("class", "") == "smallfont list_no_decoration profilefield_list")
                                     select td)
                                     .FirstOrDefault();
                List<HtmlNode> node = numPosts.Elements("dd").ToList();
                if (node.Count < 2)
                {
                    appSetting.TotalPosts = 0;
                }
                appSetting.TotalPosts = int.TryParse(node[1].InnerText.Trim(), out posts) ? posts : 0;
            }
            else if (content == Resource.STR_ERROR)
            {
                appSetting.TotalPosts = 0;
            }
        }

        public void AnalyzeURLImage(HtmlDocument doc)
        {
            foreach (HtmlNode img in doc.DocumentNode.Descendants("img"))
            {
                try
                {
                    if (!img.Attributes["src"].Value.StartsWith("http"))
                    {
                        img.Attributes["src"].Value = "https://forums.voz.vn/" + img.Attributes["src"].Value;
                    }

                    img.SetAttributeValue("class", "responsive");
                    List<HtmlNode> listTagA = img.Descendants("a").ToList();
                    if (listTagA.Count != 0)
                    {
                        foreach (var item in listTagA)
                        {
                            item.SetAttributeValue("onclick", "window.external.notify('{Url}'); return false;");
                        }
                    }
                    else
                    {
                        img.SetAttributeValue("onclick", "window.external.notify('{Image}' + src); return false;");
                    }
                }
                catch (Exception ex)
                {
                    ;
                }
            }

            List<HtmlNode> nodeImgQuote = doc
                .DocumentNode
                .Descendants("img")
                .Where(n => n.GetAttributeValue("alt", string.Empty) == Resource.STR_REPLY_WITH_QUOTE)
                .ToList();

            foreach (var item in nodeImgQuote)
            {
                var xxx = item.ParentNode;
                var att = xxx.GetAttributeValue("onclick", string.Empty);
                var id = xxx.GetAttributeValue("href", string.Empty).Split('=').LastOrDefault();
                var tempAtt = string.Format("window.external.notify('{{Url-Quote-{0}}}' + href); return false;", id);
                xxx.SetAttributeValue("onclick", tempAtt);
            }

            List<HtmlNode> nodeImgMultiQuote = doc
                .DocumentNode
                .Descendants("img")
                .Where(n => n.GetAttributeValue("alt", string.Empty) == Resource.STR_MULTI_QUOTE)
                .ToList();

            foreach (var item in nodeImgMultiQuote)
            {
                var xxx = item.ParentNode;
                var att = xxx.GetAttributeValue("onclick", string.Empty);
                var id = xxx.GetAttributeValue("href", string.Empty).Split('=').LastOrDefault();
                var tempAtt = string.Format("window.external.notify('{{Url-MultiQuote-{0}}}' + href); return false;", id);
                xxx.SetAttributeValue("onclick", tempAtt);
            }

            List<HtmlNode> nodeImgReply = doc
                .DocumentNode
                .Descendants("img")
                .Where(n => n.GetAttributeValue("alt", string.Empty) == Resource.STR_QUICK_REPLY)
                .ToList();

            foreach (var item in nodeImgReply)
            {
                var xxx = item.ParentNode;
                var att = xxx.GetAttributeValue("onclick", string.Empty);
                var id = xxx.GetAttributeValue("href", string.Empty).Split('=').LastOrDefault();
                var tempAtt = string.Format("window.external.notify('{{Url-Quick-{0}}}' + href); return false;", id);
                xxx.SetAttributeValue("onclick", tempAtt);
            }
        }

        public void AnalyzeURL(HtmlDocument doc)
        {
            foreach (HtmlNode url in doc.DocumentNode.Descendants("a"))
            {
                try
                {
                    if (!url.Attributes["href"].Value.StartsWith("http"))
                    {
                        url.Attributes["href"].Value = "https://forums.voz.vn/" + url.Attributes["href"].Value;
                    }

                    url.SetAttributeValue("onclick", "window.external.notify('{Url}' + href); return false;");
                    url.SetAttributeValue("onclick", "window.external.notify('{Url}' + href); return false;");
                    List<HtmlNode> listTagImg = url.Descendants("img").ToList();
                    foreach (var item in listTagImg)
                    {
                        item.SetAttributeValue("onclick", string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    ;
                }
            }
        }

        public string GetContenFromQuote(string contentHtml)
        {
            try
            {
                HtmlDocument docQuote = new HtmlDocument();
                docQuote.LoadHtml(contentHtml);
                var contentQuote = (from tx
                                    in docQuote.DocumentNode.Descendants("textarea")
                                    where (tx.GetAttributeValue("id", "") == "vB_Editor_001_textarea")
                                    select tx).FirstOrDefault();
                return HtmlEntity.DeEntitize(contentQuote.InnerText.Trim());
            }
            catch (Exception)
            {
                return Resource.STR_EMPTY;
            }
        }

        public void AnalyzeContent(HtmlDocument doc)
        {
            foreach (var item in doc.DocumentNode.Descendants("pre"))
            {
                item.SetAttributeValue("style", "margin: 0px;padding: 3px;border: 1px inset;width: 100%; max-height: 300px;text - align: left;overflow: auto;");
            }

            List<HtmlNode> htmlNodesPHP =
                doc
                .DocumentNode
                .Descendants("div")
                .Where(n => n.GetAttributeValue("class", Resource.STR_EMPTY) == "alt2"
                && n.GetAttributeValue("dir", Resource.STR_EMPTY) == "ltr"
                && n.GetAttributeValue("style", Resource.STR_EMPTY).Contains("overflow: auto")).ToList();

            foreach (var item in htmlNodesPHP)
            {
                item.SetAttributeValue("style", "margin: 0px;padding: 3px;border: 1px inset;width: 100%; max-height: 300px;text - align: left;overflow: auto;");
            }
        }

        public string FullPageThreadHtml(List<ThreadModel> objList)
        {
            string html = string.Empty;
            string tempbody = string.Empty;
            foreach (var item in objList)
            {
                tempbody += ThreadComment
                    .Replace("{rpPostCount}", item.PositionPost)
                    .Replace("{rpTime}", item.TimePost)
                    .Replace("{rpUrlMember}", "https://forums.voz.vn/member.php?u=" + item.MemberID)
                    .Replace("{rpID}", item.MemberID)
                    .Replace("{rpAvatar}", item.ImgAvatar)
                    .Replace("{rpUserName}", item.Member)
                    .Replace("{rpLevelMember}", item.TypeMember)
                    .Replace("{rpJoinDate}", item.JoinDate)
                    .Replace("{rpLocation}", item.Location)
                    .Replace("{rpPosts}", item.TotalPosts)
                    .Replace("{rpImgOnline}", item.Status)
                    .Replace("{rpHTML}", item.HTML);
            }
            Package package = Package.Current;
            var Arch = package.Id.Architecture;

            if (Arch == Windows.System.ProcessorArchitecture.Arm)
            {
                html = ThreadHTML
                .Replace("{rpContent}", tempbody)
                .Replace("{rpMarginRight}", "0");
            }
            else
            {
                html = ThreadHTML
               .Replace("{rpContent}", tempbody)
               .Replace("{rpMarginRight}", "8px");
            }

            return html
                .Replace("{rpColorBody}", appSetting.ColorBody)
                .Replace("{rpBackgroundBody}", appSetting.ColorBackground)
                .Replace("{rpBackgroundBodyTD}", appSetting.ColorBackgroundTD)
                .Replace("{rpAColor}", appSetting.LinkHref);
        }

        public string ConvertMessage(string message)
        {
            if (appSetting.TotalPosts < Resource.TOTAL_POST_URL_IMAGE)
            {
                GetPosts(appSetting.Cookies_Vfuserid);
            }
            string DeviceName = Resource.STR_EMPTY;
            if (appSetting.DeviceName != null)
            {
                DeviceName = appSetting.DeviceName;
            }
            else
            {
                DeviceName = EasClientDvInfo.SystemProductName.ToString() + Resource.STR_SPACE;
            }

            string usingApp = Resource.STR_EMPTY;
            if (appSetting.TotalPosts > Resource.TOTAL_POST_URL_IMAGE)
            {
                usingApp = "[I][B][SIZE=\"1\"]Sent from [B][I]" + DeviceName + "[/I][/B] " + " using [URL=\"https://www.microsoft.com/vi-vn/store/p/vozforums-universal/9nblggh438xd\"]Vozforums Universal[/URL][/SIZE][/B][/I]";
            }
            else
            {
                usingApp = "[I][B][SIZE=\"1\"]Sent from [B][I]" + DeviceName + "[/I][/B] " + " using Vozforums Universal[/SIZE][/B][/I]";
                message = message
                    .Replace("[URL", "[url")
                    .Replace("[/URL", "[/url")
                    .Replace("[IMG", "[img")
                    .Replace("[/IMG", "[/img")
                    .Replace("[url]", string.Empty)
                    .Replace("[/url]", string.Empty)
                    .Replace("[url=\"", string.Empty)
                    .Replace("[img]", string.Empty)
                    .Replace("[/img]", string.Empty)
                    .Replace("[/img]", string.Empty)
                    .Replace("https://www.", "https:// ")
                    .Replace("http://www.", "http:// ")
                    .Replace("https://", "https:// ")
                    .Replace("http://", "http:// ");
            }

            message = DefineEmoticon.ConvertMessage(message);
            message = message + Environment.NewLine + Environment.NewLine + usingApp;
            return message;
        }

        public void GetMessagePrivate(string html)
        {
            if (appSetting.Token.Length < 10)
            {
                return;
            }

            int Unread = 0;
            int Total = 0;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var kkk = doc.DocumentNode
                .Descendants("td")
                .Where(n => n.GetAttributeValue("class", string.Empty) == "alt2")
                .Where(n => n.GetAttributeValue("nowrap", string.Empty) == "nowrap")
                .FirstOrDefault()
                .Descendants("div")
                .Where(n => n.GetAttributeValue("class", string.Empty) == "smallfont")
                .FirstOrDefault()
                .Descendants("div")
                .FirstOrDefault();
            var Mess = HtmlEntity.DeEntitize(kkk.InnerText)
                .Trim()
                .ToLower()
                .Split(':')
                .LastOrDefault()
                .Split(',');

            bool isNumUnread = true;
            bool isNumTotal = true;
            if (Mess.Count() == 1)
            {
                var tempUnread = Mess[0].Split(':').LastOrDefault().Trim();
                isNumUnread = int.TryParse(tempUnread, out Unread);
            }
            if (Mess.Count() == 2)
            {
                var tempUnread = Mess[0].Replace("unread", string.Empty).Trim();
                isNumUnread = int.TryParse(tempUnread, out Unread);

                var tempTotal = Mess[1].Replace("total", string.Empty).Replace(".", string.Empty).Trim();
                isNumTotal = int.TryParse(tempTotal, out Total);
            }

            // Update to UI
            var mainView = MainView.GetInstance();
            mainView.UpdateInfoMessageBtn(Unread, Total);
            //if (isNumTotal && isNumUnread && Unread > 0)
            //{
                
            //}            
            //else
            //{
            //    var mainView = MainView.GetInstance();
            //    mainView.UpdateInfoMessageBtn(0, 0);
            //}
        }
    }
}