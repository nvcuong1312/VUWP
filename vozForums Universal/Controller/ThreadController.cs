using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Connect;
using vozForums_Universal.Views;
using vozForums_Universal.Model;
using vozForums_Universal.CommonControl;
using Windows.Security.ExchangeActiveSyncProvisioning;
using System.Threading;
using vozForums_Universal.Helper;

namespace vozForums_Universal.Controller
{
    public class ThreadController
    {
        HtmlHelper Helper = new HtmlHelper();
        ConnectServer Server = new ConnectServer();
        AppSettingModel appSetting = new AppSettingModel();

        public void PostComment(string message, string idThread, ref bool checkDone)
        {
            message = Helper.ConvertMessage(message);
            string data =
                    "message=" + message
                    + "&wysiwyg=0"
                    + "&styleid=0"
                    + "&fromquickreply=1"
                    + "&s="
                    + "&securitytoken=" + appSetting.Token
                    + "&do=postreply"
                    + "&signature=1"
                    + "&t=" + idThread
                    + "&p=who cares"
                    + "&specifiedpost=0"
                    + "&parseurl=1"
                    + "&loggedinuser=" + appSetting.Cookies_Vfuserid
                    + "&sbutton=Post Quick Reply";
            string PostURI = Resource.URL_POST_COMMENT.Replace("{rpID}", idThread);

            Server.Post(data, PostURI, ref checkDone);
        }

        public void GetContent(string url, ref string content)
        {
            Server.GetContent(url, ref content);
        }

        public void SendRating(int rating, string idThread, ref bool checkDone)
        {
            Server.VoteThread(rating, idThread, checkDone);
        }

        public void NavigateManager(string url)
        {
            if (url.Contains("{Image}"))
            {

            }
            else if (url.Contains("{Url}"))
            {
                string urlResponse = url.Replace("{Url}", string.Empty);
                ThreadView.GetInstance().SetUpPopupWebview(urlResponse);
            }
            else if (url.Contains("{Url-Quote-"))
            {
                var Id = url.Split('=').LastOrDefault();
                ThreadView.GetInstance().SetContentForPostMessage(Id);
            }
            else if (url.Contains("{Url-MultiQuote-"))
            {
                var Id = url.Split('=').LastOrDefault();
                var temp = Resource.STR_2C + Id;
                if (appSetting.Cookies_Vbmultiquote.Contains(temp))
                {
                    appSetting.Cookies_Vbmultiquote = appSetting.Cookies_Vbmultiquote.Replace(temp, Resource.STR_EMPTY);
                    DialogResult.DialogOnlyOk("Removed " + Id.ToString() + " from quote list!");
                }
                else
                {
                    appSetting.Cookies_Vbmultiquote += temp;
                    DialogResult.DialogOnlyOk("Added " + Id.ToString() + " to quote list!");
                }
            }
            else if (url.Contains("{Url-Quick-"))
            {
                ThreadView.GetInstance().DisplayPopupPostMessage();
            }
        }
    }
}
