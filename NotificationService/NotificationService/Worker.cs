using System.Runtime.Serialization;
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
            const string NOTIFICATION_FRAMEWORK_APPID = "code.google.com/p/tridion-notification-framework";
            const string NOTIFICATION_FREQUENCY = "notification_frequency";
            const string NOTIFICATION_LAST_SEND = "notification_last_send";
            var client = Client.GetCoreService();
            var users = client.GetSystemWideList(new UsersFilterData { BaseColumns = ListBaseColumns.IdAndTitle, IsPredefined = false });
            var userIds = users.Select(f => f.Id).Distinct().ToArray();
            var userApplicationDataDict = client.ReadApplicationDataForSubjectsIds(userIds, new[] { NOTIFICATION_FRAMEWORK_APPID }).Where(a => a.Value.Length > 0);
            // REVIEW - too much logging?
            Logger.WriteToLog(string.Format("{0} users with application data found", userApplicationDataDict.Count()), EventLogEntryType.Information);


            // <NotificationFramework> 
            //     <Notifier type="WorkflowEmailNotifier"
            //                notification_frequency="3D"
            //                notification_last_send="2012-10-07T23:13Z">
            //        <EmailAddress>punter@outfit.org</EmailAddress>
            //    </Notifier>
            //    <Notifier type="WorkflowTwitterNotifier"
            //            notification_frequency="3D"
            //            notification_last_send="2012-10-07T23:13Z">
            //        <TwitterName>TridionLovingHackyGeek</TwitterName>
            //    </Notifier>
            //</NotificationFramework> 
            var pollingInterval = GetPollingInterval();
            foreach (var notifier in notifiers)
            {
                foreach (KeyValuePair<string, ApplicationData[]> userApplicationData in userApplicationDataDict) 
                {
                    var user = (UserData)users.Single(u => u.Id == userApplicationData.Key);
                    // REVIEW: The query expression is redundant, as we've asked the API for a filtered list.... 
                    // Oh wait - we want it to barf if there are two?
                    ApplicationData userNotificationApplicationData = userApplicationData.Value.Single(ad => ad.ApplicationId == NOTIFICATION_FRAMEWORK_APPID);
                    string xmlData = Encoding.UTF8.GetString(userNotificationApplicationData.Data);
                    var doc = XDocument.Parse(xmlData);
                    var notifierElements = doc.Element("NotificationFramework").Elements("Notifier"); 
                    foreach (var notifierElement in notifierElements)
                    {

                        var notificationFrequency = GetNotificationFrequency(notifierElement.Attribute(NOTIFICATION_FREQUENCY).Value);
                        var lastNotificationAttribute = notifierElement.Attribute(NOTIFICATION_LAST_SEND);
                        var lastNotificationTime = ParseDate(lastNotificationAttribute.Value);
                        var nextNotificationCheckTime = lastNotificationTime.Add(notificationFrequency);                        

                        bool notificationIsNeeded = DateTime.Now.ToUniversalTime().Subtract(nextNotificationCheckTime) > pollingInterval;
                        if (notificationIsNeeded)
                        {
                            Logger.WriteToLog(string.Format("Impersonating as {0}", user.Title), EventLogEntryType.Information);
                            client.Impersonate(user.Title);

                            // Could factor this out more to allow for creating other kinds of notification data 
                            // than WorkflowNotificationData, but for now YAGNI
                            // TODO: Do we check the CreationDate of the right subject???
                            var relevantWorkFlowDataItems = GetUserWorkflowItems(client).Where(
                                item => lastNotificationTime < client.Read(item.Subject.IdRef, null).VersionInfo.CreationDate).ToArray<WorkItemData>();

                            var notificationData = new WorkflowNotificationData()
                                                       {
                                                           ApplicationData = notifierElement.ToString(),
                                                           User = user,
                                                           WorkItems = relevantWorkFlowDataItems
                                                       };
                            
                            notifier.Notify(notificationData);

                            lastNotificationAttribute.Value = DateTime.Now.ToUniversalTime().ToString("u");
                        }
                    }

                    userNotificationApplicationData.Data = Encoding.UTF8.GetBytes(doc.ToString());
                    client.SaveApplicationData(userApplicationData.Key, new[] { userNotificationApplicationData });
                }
                client.Close();
            }
        }

        private static WorkItemData[] GetUserWorkflowItems(SessionAwareCoreServiceClient client)
        {
            var userWorkItemsFilter = new UserWorkItemsFilterData()
                                          {
                                              ActivityState = ActivityState.Started | ActivityState.Assigned,                                               
                                          };
                    
            var workItemDataList = new List<WorkItemData>();
            client.GetSystemWideList(userWorkItemsFilter).ToList().ForEach(idObject => workItemDataList.Add(idObject as WorkItemData));  
            return workItemDataList.ToArray();            
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
            return DateTime.Parse(stringDate, CultureInfo.InvariantCulture).ToUniversalTime();
        }

        private static TimeSpan GetPollingInterval()
        {
            var pollingInterval = Convert.ToInt16(ConfigurationManager.AppSettings.Get("pollingInterval"));
            return new TimeSpan(0, 0, pollingInterval);

        }

    }
}
