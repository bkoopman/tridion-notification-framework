using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TridionCommunity.NotificationFramework;

namespace NetworkMessageNotifierTests
{
    [TestClass]
    public class NetworkMessageNotifierTests
    {
        [TestMethod]
        public void SendMessageSuccess()
        {
            var notifier = new NetworkMessageNotifier();
            notifier.SendMessage(Environment.UserName, "This is a test from the unit test!");
        }

        [TestMethod]
        public void SendMessageFailure()
        {
            var notifier = new NetworkMessageNotifier();
            const string userName = "NonExistingUser";
            const string message = "This is a test from the unit test!";

            try
            {
                notifier.SendMessage(userName, message);
                Assert.Fail("Using an non-existing user should have thrown an exception.");
            }
            catch (Exception ex)
            {
                var expectedMessage = string.Format("Failed to notify the user '{0}'. Exit code was 1.", userName);
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }
    }
}
