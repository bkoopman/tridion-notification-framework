using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TridionCommunity.NotificationFramework
{
    public class TwitterNotifier : INotifier
    {
        private string twitterUser = "TridionNotifications";
        private string twitterPassword = "PIqQywvnseNcbR8nmqKR";

        public void Tweet(string user, string status)
        {
            ServicePointManager.Expect100Continue = false;
            using (var webClient = new WebClient())
            {
                webClient.BaseAddress = "http://twitter.com";
                var credentials = new System.Net.NetworkCredential();
                credentials.UserName = this.twitterUser;
                credentials.Password = this.twitterPassword;
                webClient.Credentials = credentials;
                using (var stream = webClient.OpenWrite("statuses/update.xml"))
                {
                    var writer = new System.IO.StreamWriter(stream);
                    writer.Write("status=" + status);
                }
            }
        }
    }

}


