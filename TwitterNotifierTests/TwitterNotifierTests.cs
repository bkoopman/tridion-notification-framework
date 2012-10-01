using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TridionCommunity.NotificationFramework.Tests
{
    [TestClass]
    public class TwitterNotifierTests
    {
        [TestMethod]
        public void TestTweet()
        {
            var target = new TwitterNotifier();
            target.Tweet("DominicCronin", "This is a test");

        }
    }
}
