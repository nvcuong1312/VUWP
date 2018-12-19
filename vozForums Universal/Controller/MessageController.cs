using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Connect;
using vozForums_Universal.Helper;
using vozForums_Universal.Model;
using vozForums_Universal.Views.Account;

namespace vozForums_Universal.Controller
{
    public class MessageController
    {
        HtmlHelper Helper = new HtmlHelper();
        ConnectServer Server = new ConnectServer();
        AppSettingModel appSetting = new AppSettingModel();

        public void GetContent(string url, ref string content)
        {
            Server.GetContent(url, ref content);
        }

        /// <summary>
        /// DataContent[0] : mpid
        /// DataContent[1] : title
        /// DataContent[2] : recipients
        /// </summary>
        /// <param name="message"></param>
        /// <param name="DataContent"></param>
        /// <param name="checkDone"></param>
        public void PostMessage(string message, string[] DataContent, ref bool checkDone, bool isNewMessage = false)
        {
            message = Helper.ConvertMessage(message);
            string PostURI = Resource.STR_EMPTY;
            string data = Resource.STR_EMPTY;

            if (!isNewMessage)
            {
                data = "message=" + message
                        + "&wysiwyg=0"
                        + "&styleid=0"
                        + "&fromquickreply=1"
                        + "&signature=1"
                        + "&s="
                        + "&securitytoken=" + appSetting.Token
                        + "&do=insertpm&pmid=" + DataContent[0]
                        + "&loggedinuser=" + appSetting.Cookies_Vfuserid
                        + "&parseurl=1"
                        + "&title=" + DataContent[1]
                        + "&recipients=" + DataContent[2]
                        + "&forward=0"
                        + "&savecopy=1"
                        + "&sbutton=Post Message";
                PostURI = Resource.URL_POST_MESSAGE.Replace("{rpID}", DataContent[0]);
            }
            else
            {
                data = "message=" + message
                        + "&wysiwyg=0"
                        + "&iconid=0"
                        + "&signature=1"
                        + "&s="
                        + "&securitytoken=" + appSetting.Token
                        + "&do=insertpm&pmid="
                        + "&loggedinuser=" + appSetting.Cookies_Vfuserid
                        + "&parseurl=1"
                        + "&title=" + DataContent[1]
                        + "&recipients=" + DataContent[2]
                        + "&bccrecipients="
                        + "&forward="
                        + "&savecopy=1"
                        + "&sbutton=Submit Message";
                PostURI = Resource.URL_POST_MESSAGE.Replace("{rpID}", Resource.STR_EMPTY);
            }

            Server.PostComment(data, PostURI, ref checkDone);
        }

        public void NavigateManager(string url)
        {
            if (url.Contains("{Image}"))
            {

            }
            else if (url.Contains("{Url}"))
            {
                string urlResponse = url.Replace("{Url}", string.Empty);
                MessageView.GetInstance().OpenBrowser(urlResponse);
            }
            else if (url.Contains("{Url-Quote-"))
            {
                MessageView.GetInstance().DisplayPopupPostMessage();
            }
        }
    }
}
