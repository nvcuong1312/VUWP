using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vozForums_Universal.Connect;
using vozForums_Universal.Model;

namespace vozForums_Universal.Controller
{
    public class ListThreadController
    {
        AppSettingModel appSetting = new AppSettingModel();
        ConnectServer Server = new ConnectServer();

        public void GetContent(string url, ref string contentHtml)
        {
            Server.Get(url, ref contentHtml);
        }

        public void Post(string idBox, string message, string title, ref bool checkDone)
        {
            string URL = Resource.URL_NEW_THREAD.Replace("{rpID}", idBox);
            string data = "message=" + message
                        + "&subject=" + title
                        + "&wysiwyg=0"
                        + "&s="
                        + "&securitytoken=" + appSetting.Token
                        + "&f=" + idBox
                        + "&do=postthread"
                        + "&posthash=" + appSetting.Token
                        + "&poststarttime=" + appSetting.Token.Split('-').FirstOrDefault()
                        + "&loggedinuser=" + appSetting.Cookies_Vfuserid
                        + "&sbutton=Submit New Thread"
                        + "&signature=1"
                        + "&parseurl=1"
                        + "&emailupdate=9999";

            Server.Post(data, URL, ref checkDone);
        }
    }
}
