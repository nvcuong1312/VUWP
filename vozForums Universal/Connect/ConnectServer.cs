using System;
using System.Threading.Tasks;
using vozForums_Universal.Model;
using System.Text;
using System.Threading;
using System.Net.Http;
using System.Net;
using vozForums_Universal.CommonControl;
using System.Collections.Generic;
using System.Linq;

namespace vozForums_Universal.Connect
{
    public class ConnectServer
    {
        //private static HttpClient client;
        private CookieContainer cookies;
        private HttpClientHandler handler;
        private AppSettingModel appSetting;

        public ConnectServer()
        {
            appSetting = new AppSettingModel();

            cookies = new CookieContainer();
            handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
        }

        public void Get(string url, ref string outContent)
        {
            try
            {
                HttpClient client = new HttpClient(handler);
                cookies.Add(new Uri(url), new Cookie(Resource.COOKIES_VFPASSWORD, appSetting.Cookies_Vfpassword));
                cookies.Add(new Uri(url), new Cookie(Resource.COOKIES_VFUSERID, appSetting.Cookies_Vfuserid));
                cookies.Add(new Uri(url), new Cookie(Resource.COOKIES_VBULLETIN_MULTIQUOTE, appSetting.Cookies_Vbmultiquote));

                client.DefaultRequestHeaders.Add(Resource.USER_AGENT, Resource.USER_AGENT_VALUE);
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        outContent = content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                outContent = Resource.DIALOG_ERROR;
                DialogResult.DialogOnlyOk(Resource.DIALOG_ERROR_NETWORK);
            }
        }

        public void Login(string user, string password, ref string contentHtml)
        {
            string postData = "&vb_login_username="
                + user
                + "&vb_login_password="
                + password
                + "&cookieuser=1"
                + "&do=login"
                + "&s="
                + "securitytoken=guest"
                + "&vb_login_md5password="
                + "&vb_login_md5password_utf=&";
            string applicationEncoded = "application/x-www-form-urlencoded";

            try
            {
                HttpClient client = new HttpClient(handler);
                client.DefaultRequestHeaders.Add(Resource.USER_AGENT, Resource.USER_AGENT_VALUE);

                using (HttpResponseMessage response = client.PostAsync(Resource.URL_LOGIN, new StringContent(postData, Encoding.UTF8, applicationEncoded)).Result)
                {
                    using (HttpResponseMessage responseMessage = client.GetAsync(Resource.URL_HOMEPAGE).Result)
                    {
                        using (HttpContent content = responseMessage.Content)
                        {
                            contentHtml = content.ReadAsStringAsync().Result;

                            IEnumerable<Cookie> responseCookies = cookies.GetCookies(new Uri(Resource.URL_HOMEPAGE)).Cast<Cookie>();
                            var listName = responseCookies.Select(n => n.Name).ToList();
                            if (listName.Contains(Resource.COOKIES_VFPASSWORD))
                            {
                                //var xxx = responseCookies.Where(n => n.Name == Resource.COOKIES_VFPASSWORD).Select(n => n.Value).FirstOrDefault();
                                //var yyy = responseCookies.Where(n => n.Name == Resource.COOKIES_VFUSERID).Select(n => n.Value).FirstOrDefault();
                                appSetting.Cookies_Vfpassword = responseCookies.Where(n => n.Name == Resource.COOKIES_VFPASSWORD).Select(n => n.Value).FirstOrDefault();
                                appSetting.Cookies_Vfuserid = responseCookies.Where(n => n.Name == Resource.COOKIES_VFUSERID).Select(n => n.Value).FirstOrDefault();
                            }
                            else
                            {
                                contentHtml = Resource.DIALOG_ERROR;
                            }
                            //foreach (Cookie cookie in responseCookies)
                            //{
                            //    if (cookie.Name == Resource.COOKIES_VFPASSWORD)
                            //    {
                            //        appSetting.Cookies_Vfpassword = cookie.Value;
                            //    }
                            //    if (cookie.Name == Resource.COOKIES_VFUSERID)
                            //    {
                            //        appSetting.Cookies_Vfuserid = cookie.Value;
                            //    }
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                contentHtml = Resource.DIALOG_ERROR;
            }
        }

        public void Post(string message, string PostURI, ref bool checkDone)
        {
            checkDone = false;
            try
            {
                HttpClient client = new HttpClient(handler);
                cookies.Add(new Uri(PostURI), new Cookie(Resource.COOKIES_VFPASSWORD, appSetting.Cookies_Vfpassword));
                cookies.Add(new Uri(PostURI), new Cookie(Resource.COOKIES_VFUSERID, appSetting.Cookies_Vfuserid));
                client.DefaultRequestHeaders.Add(Resource.USER_AGENT, Resource.USER_AGENT_VALUE);

                using (var response = client.PostAsync(PostURI, new StringContent(message, Encoding.UTF8, Resource.APPLICATION)).Result)
                {
                    checkDone = true;
                }
            }
            catch (Exception ex)
            {
                checkDone = true;
            }
        }

        //public void VoteThread(int rating, string idThread, bool checkDone)
        //{
        //    string voteUri = "https://forums.voz.vn/threadrate.php?t=" + idThread;
        //    string data = "vote=" + rating
        //             + "&s=&securitytoken="
        //             + appSetting.Token
        //             + "&t=" + idThread
        //             + "&pp=10&page=1&button=Vote Now";
        //    string application = "application/x-www-form-urlencoded";

        //    try
        //    {
        //        HttpClient client = new HttpClient(handler);
        //        cookies.Add(new Uri(voteUri), new Cookie(Resource.COOKIES_VFPASSWORD, appSetting.Cookies_Vfpassword));
        //        cookies.Add(new Uri(voteUri), new Cookie(Resource.COOKIES_VFUSERID, appSetting.Cookies_Vfuserid));
        //        client.DefaultRequestHeaders.Add(Resource.USER_AGENT, Resource.USER_AGENT_VALUE);

        //        using (HttpResponseMessage response = client.PostAsync(voteUri, new StringContent(data, Encoding.UTF8, application)).Result)
        //        {
        //            checkDone = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        checkDone = true;
        //    }
        //}
    }
}
