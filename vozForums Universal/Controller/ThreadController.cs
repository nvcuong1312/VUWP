using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Connect;
namespace vozForums_Universal.Controller
{   
    public class ThreadController
    {
        public static void SendMessage(string message, string idThread, ref bool checkDone)
        {
           ConnectServer.SendMessage(message, idThread,ref checkDone);
        }
        public static void GetContent(string url, ref string content)
        {
            ConnectServer.GetContent(url, ref content);
        }
        public static void SendRating(int rating, string idThread, ref bool checkDone)
        {
            ConnectServer.VoteThread(rating, idThread, checkDone);
        }
    }
}
