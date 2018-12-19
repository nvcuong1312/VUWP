using System;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using vozForums_Universal.Connect;
using vozForums_Universal.CommonControl;
using Windows.Storage;
using vozForums_Universal.Model;

namespace vozForums_Universal.Helper
{
    public class AccountHelper
    {
        private HtmlHelper helper;
        private AppSettingModel appSetting;
        private ConnectServer Server;
        private ApplicationDataContainer loginValue;

        public AccountHelper()
        {
            helper = new HtmlHelper();
            Server = new ConnectServer();
            appSetting = new AppSettingModel();
            loginValue = ApplicationData.Current.LocalSettings;
        }

        public bool LoginMethod(string userName, string passWord, bool save)
        {
            try
            {
                string contentHtml = string.Empty;
                Server.Login(userName, passWord, ref contentHtml);
                if (contentHtml == Resource.STR_ERROR)
                {
                    DialogResult.DialogOnlyOk(Resource.STR_ERROR);
                    return false;
                }

                helper.GetMessagePrivate(contentHtml);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(contentHtml);

                appSetting.isSaveLogin = save;
                appSetting.UserName = userName;
                appSetting.Password = passWord;
                appSetting.Token = GetToken(doc.DocumentNode.OuterHtml);
                helper.GetPosts(appSetting.Cookies_Vfuserid);

                if (appSetting.Token == Resource.STR_GUEST)
                {
                    appSetting.UserName = Resource.STR_EMPTY;
                    appSetting.Password = Resource.STR_EMPTY;
                    appSetting.isSaveLogin = false;
                    appSetting.Cookies_Vfpassword = Resource.STR_EMPTY;
                    appSetting.Cookies_Vfuserid = Resource.STR_EMPTY;
                    appSetting.TotalPosts = 0;

                    DialogResult.DialogOnlyOk(Resource.STR_ERROR);
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                DialogResult.DialogOnlyOk(Resource.STR_ERROR);
                return false;
            }
        }

        public static string GetToken(string HTML)
        {
            StringReader reader = new StringReader(HTML);
            string result = reader.ReadToEnd();

            string token = Regex.Match(result, "SECURITYTOKEN = \".+\"").Value + Environment.NewLine;
            token = Regex.Match(token, "\".+\"").Value;
            token = token.Remove(0, 1);
            token = token.Remove(token.Length - 1, 1);

            return token;
        }
    }
}
