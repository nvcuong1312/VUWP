using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Connect;
using vozForums_Universal.Model;

namespace vozForums_Universal.Controller
{
    public class ListMessageController
    {
        ConnectServer Server = new ConnectServer();
        AppSettingModel appSetting = new AppSettingModel();

        public void GetContent(string url, ref string contentHtml)
        {
            Server.Get(url, ref contentHtml);
        }

        public void DeleteMsg(string ID, string FolderID)
        {
            string PostURI = Resource.STR_EMPTY;
            string data = Resource.STR_EMPTY;

            data = "securitytoken=" + appSetting.Token
                        + "&do=managepm"
                        + "&folderid=" + FolderID
                        + "&pm[{rpID}]=true"
                        + "&s="
                        + "&dowhat=delete";
            PostURI = Resource.URL_POST_MESSAGE.Replace("{rpID}", ID);
            bool checkDone = false;
            Server.Post(data.Replace("{rpID}", ID), PostURI, ref checkDone);
        }
    }
}
