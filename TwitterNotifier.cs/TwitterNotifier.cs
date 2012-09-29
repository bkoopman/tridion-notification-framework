using System;
using Twitterizer;

namespace TridionCommunity.NotificationFramework
{
    public class TwitterNotifier : INotifier
    {

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

        public void Notify(NotificationData data)
        {
            throw new NotImplementedException();
        }
    }

}


