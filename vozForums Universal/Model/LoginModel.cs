using System;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using Windows.UI.Popups;
using vozForums_Universal.Helper;
using vozForums_Universal.Connect;

namespace vozForums_Universal.Model
{
    public class LoginModel
    {
        static Windows.Storage.ApplicationDataContainer loginValue = Windows.Storage.ApplicationData.Current.LocalSettings;
        static AppSettingModel appSetting = new AppSettingModel();
        HelperHtml helper = new HelperHtml();

        static public string userName { get { return appSetting.UserName; } }
        static public string passWord { get { return appSetting.PassWord; } }
        static public string token { get { return appSetting.token; } }
        static public string userId { get { return appSetting.UserId; } }
        static public string checkPost { get { return appSetting.CheckPosts; } }
        public async void LoginMethod(string userName, string passWord, Boolean save)
        {           
            try
            {
                string contentHtml = string.Empty;
                ConnectServer.Login(userName, passWord, ref contentHtml);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(contentHtml);
                StringReader reader = new StringReader(doc.DocumentNode.OuterHtml);
                string result = reader.ReadToEnd();
                //Get token.
                string token = Regex.Match(result, "SECURITYTOKEN = \".+\"").Value + Environment.NewLine;
                token = Regex.Match(token, "\".+\"").Value;
                token = token.Remove(0, 1);
                token = token.Remove(token.Length - 1, 1);
                
                //Get user ID
                string user_id = "";
                if (result.Contains("Welcome, <a href=\"member.php?u="))
                {
                    int startIndex = result.IndexOf("Welcome, <a href=\"member.php?u=") + 31;
                    for (int i = startIndex; i < result.Count(); i++)
                    {
                        if (result[i] == '"')
                        {
                            user_id = result.Substring(startIndex, i - startIndex);
                            break;
                        }
                    }
                }
                //Check Posts
                int checkPost = await helper.GetPosts(int.Parse(user_id));
                appSetting.UserName = userName;
                appSetting.PassWord = passWord;
                appSetting.token = token;
                appSetting.UserId = user_id;

                if (save)
                {
                    appSetting.SaveState = "true";
                }
                else
                {
                    loginValue.Values["saveState"] = "false";
                }

                if (checkPost >= 20)
                {
                    appSetting.CheckPosts = "Ok";
                }
                else appSetting.CheckPosts = "NotOk";

                if (token == "guest")
                {
                    showlog();
                    appSetting.UserName = "";
                    appSetting.PassWord = "";
                }                
            }
            catch
            { }
        }
        private async void showlog()
        {
            var msg = new MessageDialog("Errol Login.\n Token is null");
            var btn = new UICommand("ok");
            msg.Commands.Add(btn);
            IUICommand result = await msg.ShowAsync();
        }

    }
   
}
