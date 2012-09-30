using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotificationService;
using Tridion.ContentManager.CoreService.Client;
using TridionCommunity.NotificationFramework;

namespace NotificationServiceTests
{
    [TestClass]
    public class NotifiersTests
    {
        [TestMethod]
        public void LoadAssemblies()
        {
            var workItems = new WorkItemData[2];
            workItems[0] = new WorkItemData { Id = "tcm:0-0-0", Title = "Test Item 01" };
            workItems[1] = new WorkItemData { Id = "tcm:0-0-0", Title = "Test Item 02" };

            var notifiers = Notifiers.GetAll();
            var notificationData = new WorkflowNotificationData { 
                User = new UserData { Title = Environment.UserName } ,
                WorkItems = workItems
            };

            foreach (var notifier in notifiers)
            {
                notifier.Notify(notificationData);
            }
        }

        [TestMethod]
        public void LoadFromInvalidLocation()
        {
            ConfigurationManager.AppSettings["notifiersFolder"] = "InvalidLocation";
            
            try
            {
                Notifiers.GetAll();
                Assert.Fail("Expected an exception about an invalid configuration setting for notifiersFolder.");
            }
            catch (NotificationFailedException ex)
            {
                Assert.AreEqual("Failed to load the notifiers from 'InvalidLocation'.", ex.Message);
            }
        }
    }
}
