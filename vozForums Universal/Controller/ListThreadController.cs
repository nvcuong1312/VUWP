using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vozForums_Universal.Connect;

namespace vozForums_Universal.Controller
{
    public class ListThreadController
    {
        public static void GetContent(string url, ref string contentHtml)
        {
            ConnectServer.GetContent(url, ref contentHtml);
        }
    }
}
