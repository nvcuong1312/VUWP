using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using vozForums_Universal.Connect;

namespace vozForums_Universal.Controller
{
    public class ListThreadController
    {
        ConnectServer Server = new ConnectServer();
        public void GetContent(string url, ref string contentHtml)
        {
            Server.GetContent(url, ref contentHtml);
        }
    }
}
