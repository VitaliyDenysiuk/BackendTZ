using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarsApiLib
{
    public class NetworkManager
    {
        public string GetHtml(string url)
        {
            return new WebClient().DownloadString(url);
        }
    }
}
