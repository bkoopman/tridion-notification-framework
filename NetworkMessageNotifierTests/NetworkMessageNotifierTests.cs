using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetworkMessageNotifierTests
{
    [TestClass]
    public class NetworkMessageNotifierTests
    {
        [TestMethod]
        public void GetProcess()
        {
            var notifier = new TridionCommunity.NotificationFramework.NetworkMessageNotifier();
            const string userName = "Test\\TestUser";
            const string message = "This is a test.";;

            var process = notifier.GetProcess(userName, message);
            Assert.AreEqual("msg.exe", process.StartInfo.FileName);
            Assert.AreEqual(userName + " " + message, process.StartInfo.Arguments);
            Assert.AreEqual((int)ProcessWindowStyle.Hidden, (int)process.StartInfo.WindowStyle);
        }

        [TestMethod]
        public void SendMessageFailure()
        {
            var notifier = new TridionCommunity.NotificationFramework.NetworkMessageNotifier();
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
