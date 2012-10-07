using NotificationService.CoreService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Tridion.ContentManager.CoreService.Client;
using TridionCommunity.NotificationFramework;

namespace NotificationService
{
    class Worker
    {
        internal static void DoWork(List<INotifier> notifiers)
        {
            // get a coreservice client            
            var client = Client.GetCoreService();
            var users = client.GetSystemWideList(new UsersFilterData { BaseColumns = ListBaseColumns.IdAndTitle, IsPredefined = false });
            var userIds = users.Select(f => f.Id).Distinct().ToArray();
            var applicationDatas = client.ReadApplicationDataForSubjectsIds(userIds, new[] { "User_Preferences" }).Where(a => a.Value.Length > 0);
            Logger.WriteToLog(string.Format("{0} users with application data found", applicationDatas.Count()), EventLogEntryType.Information);

            foreach (var notifier in notifiers)
            {
                foreach (var applicationDataElement in applicationDatas) // basically foreach user
                {
                    var doc = XDocument.Parse(ASCIIEncoding.ASCII.GetString(applicationDataElement.Value.Single(ad => ad.ApplicationId == "User_Preferences").Data));
                    var settings = doc.Descendants("email_settings"); // TODO: read node name from notifier
                    foreach (var setting in settings)
                    {
                        var settingNodes = setting.Descendants();
                        var notificationFrequency = GetNotificationFrequency(settingNodes.SingleOrDefault(n => n.Name == "notification_frequency").Value);
                        var lastNotificationTime = ParseDate(settingNodes.SingleOrDefault(n => n.Name == "last_notification_send").Value);
                        var nextNotificationCheckTime = lastNotificationTime.Add(notificationFrequency);
                        var pollingInterval = GetPollingInterval();

                        // Check if it's time to check if notification is needed
                        if (DateTime.Now.Subtract(nextNotificationCheckTime) > pollingInterval)
                        {
                            Logger.WriteToLog(string.Format("Impersonating as {0}", users.Single(u => u.Id == applicationDataElement.Key).Title), EventLogEntryType.Information);
                            client.Impersonate(users.Single(u => u.Id == applicationDataElement.Key).Title);

                            // get the workflow items for the user
                            UserWorkItemsFilterData userWorkItemsFilter = new UserWorkItemsFilterData();
                            userWorkItemsFilter.ActivityState = ActivityState.Started | ActivityState.Assigned;

                            // get assignment and work list
                            IdentifiableObjectData[] workFlowItems = client.GetSystemWideList(userWorkItemsFilter);

                            IList<WorkItemData> relevantWorkFlowDataItems = new List<WorkItemData>();

                            // then if there is a task
                            foreach (WorkItemData workItem in workFlowItems)
                            {
                                if (lastNotificationTime < workItem.VersionInfo.CreationDate)
                                {
                                    relevantWorkFlowDataItems.Add(workItem);
                                    // add to list of things to push
                                    // hand that list, appdata, user ide, user desc & name to the notifier
                                }
                            }
                            var notificationData = new WorkflowNotificationData();
                            notificationData.ApplicationData = setting.ToString();
                            notificationData.User = client.GetCurrentUser();
                            notificationData.WorkItems = relevantWorkFlowDataItems.ToArray();                            
                            notifier.Notify(notificationData);

                            settingNodes.SingleOrDefault(n => n.Name == "last_notification_send").Value 
                                = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                    }

                    // Save update application data
                    var appDataToBeUpdated = applicationDataElement.Value.Single(ad => ad.ApplicationId == "User_Preferences");                    
                    appDataToBeUpdated.Data = ASCIIEncoding.ASCII.GetBytes(doc.ToString());
                    client.SaveApplicationData(applicationDataElement.Key, new[] { appDataToBeUpdated });
                }
                client.Close();
            }
        }
        private static TimeSpan GetNotificationFrequency(string value)
        {
            if (value.EndsWith("H"))
            {
                var hours = Convert.ToInt16(value.Substring(0, value.Length - 1));
                return new TimeSpan(0, hours, 0, 0);
            }

            if (value.EndsWith("M"))
            {
                var minutes = Convert.ToInt16(value.Substring(0, value.Length - 1));
                return new TimeSpan(0, 0, minutes, 0);
            }

            if (value.EndsWith("D"))
            {
                var days = Convert.ToInt16(value.Substring(0, value.Length - 1));
                return new TimeSpan(days, 0, 0, 0);
            }
            return new TimeSpan();
        }

        private static DateTime ParseDate(string stringDate)
        {
            return DateTime.Parse(stringDate, CultureInfo.InvariantCulture);
        }

        private static TimeSpan GetPollingInterval()
        {
            var pollingInterval = Convert.ToInt16(ConfigurationManager.AppSettings.Get("pollingInterval"));
            return new TimeSpan(0, 0, pollingInterval);

        }

    }
}
