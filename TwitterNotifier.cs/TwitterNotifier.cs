using System;
using System.Xml.Linq;
using Tridion.ContentManager.CoreService.Client;
using Twitterizer;

namespace TridionCommunity.NotificationFramework
{
    public class TwitterNotifier : WorkflowNotifier
    {
        //    <Notifier type="WorkflowTwitterNotifier"
        //            notification_frequency="3D"
        //            notification_last_send="2012-10-07T23:13Z">
        //        <TwitterName>TridionLovingHackyGeek</TwitterName>
        //    </Notifier>
        public void Tweet(string targetUser, string status)
        {
            OAuthTokens tokens = new OAuthTokens();
            tokens.AccessToken = Settings.Default.TwitterOAuthAccessToken;
            tokens.AccessTokenSecret = Settings.Default.TwitterOAuthAccessTokenSecret;
            tokens.ConsumerKey = Settings.Default.TwitterConsumerKey;
            tokens.ConsumerSecret = Settings.Default.TwitterConsumerSecret;

            TwitterResponse<TwitterStatus> tweetResponse 
                = TwitterStatus.Update(tokens, string.Format("@{0} {1}", targetUser, status));
            if (tweetResponse.Result == RequestResult.Success)
            {
                return;
            }
            throw new NotificationFailedException(string.Format("Twitter response was not Success: {0}", tweetResponse.Content));
        }

        protected override void Notify(UserData userData, WorkItemData[] workItemData, XElement applicationData)
        {
            var workflowDataXml = GetWorkflowDataXml(userData, workItemData, applicationData );
            string twitterName = applicationData.Element("Notifier").Element("TwitterName").Value;
            // TODO 
            Tweet(twitterName, "You have something in your workflow queue");
        }

        public sealed override string[] GetSupportedNotifierTypes()
        {
            return new string[] { "WorkflowTwitterNotifier" };
        }
    }

}


