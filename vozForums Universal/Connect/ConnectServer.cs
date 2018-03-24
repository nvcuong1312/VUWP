using System;
using System.Threading.Tasks;
using System.Net.Http;
using vozForums_Universal.Model;
namespace vozForums_Universal.Connect
{
    public class ConnectServer
    {
        public static HttpClient client = new HttpClient();
        public static HttpResponseMessage responseMessage = new HttpResponseMessage();
        private static string loginUrl = "https://vozforums.com/login.php?do=login";

        public static void GetContent(string url, ref string outContent)
        {
            var checkDone = Task.Run(async () =>
            {
                return await client.GetStringAsync(url);
            });
            if (checkDone.Result != null)
            {
                outContent = checkDone.Result.ToString();
            }
        }

        public static void Login(string user, string password, ref string contentHtml)
        {
            string postData = "&vb_login_username="
                + user
                + "&vb_login_password="
                + password
                + "&cookieuser=checked&"
                + "do=login";
            string applicationEncoded = "application/x-www-form-urlencoded";
            var response = Task.Run(async () => {
                    responseMessage = await client.PostAsync(loginUrl, 
                        new StringContent(postData, System.Text.Encoding.UTF8, applicationEncoded));
                    HttpResponseMessage resultPlaylist = await client.GetAsync(loginUrl);
                return resultPlaylist.Content.ReadAsStringAsync();
            });
            contentHtml = response.Result.Result.ToString();
        }

        public static void SendMessage(string message, string idThread,ref bool checkDone)
        {
            checkDone = false;
            string requestUri = "https://vozforums.com/newreply.php?do=postreply&t=" + idThread;
            string data = "message=" + message
                    + "&wysiwyg=0&styleid=0&fromquickreply=1&s="
                    + "&securitytoken=" + LoginModel.token
                    + "&do=postreply&t=" + idThread
                    + "&p=who cares&specifiedpost=0&parseurl=1&loggedinuser="
                    + LoginModel.userId
                    + "&sbutton=Post Quick Reply";
            string application = "application/x-www-form-urlencoded";
            var a = Task.Run(async () => responseMessage = await client.PostAsync(requestUri, new StringContent(data, System.Text.Encoding.UTF8, application)));
            if (a.Result.IsSuccessStatusCode == true)
            {
                checkDone = true;
            }
        }
        public static void VoteThread(int rating, string idThread, bool checkDone)
        {
            string voteUri = "https://vozforums.com/threadrate.php?t=" + idThread;
            string data = "vote=" + rating
                     + "&s=&securitytoken=" 
                     + LoginModel.token
                     + "&t=" + idThread
                     + "&pp=10&page=1&button=Vote Now";
            string application = "application/x-www-form-urlencoded";
            var a = Task.Run(async () => responseMessage = await client.PostAsync(voteUri, new StringContent(data, System.Text.Encoding.UTF8, application)));
            if (a.Result.IsSuccessStatusCode == true)
            {
                checkDone = true;
            }
        }
    }
}
