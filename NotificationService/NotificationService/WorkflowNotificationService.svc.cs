using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NotificationService.CoreService;
using Tridion.ContentManager.CoreService.Client;
using System.Xml.Linq;

namespace NotificationService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WorkflowNotificationService" in code, svc and config file together.
    public class WorkflowNotificationService : IWorkflowNotificationService
    {
        public void DoWork()
        {
            // get a corservice client
            var client = Client.GetCoreService("CURRENT USER");

            var users = client.GetSystemWideList(new UsersFilterData { BaseColumns = ListBaseColumns.IdAndTitle, IsPredefined = false });
            var userIds = users.Select(f => f.Id).Distinct().ToArray();
            var applicationDatas = client.ReadApplicationDataForSubjectsIds(userIds, new[] { "test" }).Where(a => a.Value.Length > 0);

            foreach (var applicationDataElement in applicationDatas)
            {
                if (true)
                {
                    client.Impersonate(users.Single(u => u.Id == applicationDataElement.Key).Title);


                    // get the workflow items for the user
                    UserWorkItemsFilterData userWorkItemsFilter = new UserWorkItemsFilterData();
                    userWorkItemsFilter.ActivityState = ActivityState.Started | ActivityState.Assigned;

                    // get assignemnt and work list
                    IdentifiableObjectData[] workFlowItems = client.GetSystemWideList(userWorkItemsFilter);

                    // Read
                    DateTime fakeDate = DateTime.Now;

                    IList<WorkItemData> relevantWorkFlowDataItems = new List<WorkItemData>();

                    // then if there is a taks
                    foreach (WorkItemData workItem in workFlowItems)
                    {
                        if (fakeDate < workItem.VersionInfo.CreationDate)
                        {
                            relevantWorkFlowDataItems.Add(workItem);
                            // add to list of things to push
                            // hand that list, appdata, user ide, user desc & name to the notifier
                        }

                    }

                    // push these buggers :)


                }
            }

        }
    }
}
