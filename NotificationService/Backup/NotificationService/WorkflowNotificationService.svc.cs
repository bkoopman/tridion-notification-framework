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
            var client = Client.GetCoreService();

            ApplicationData[] appDataList = client.ReadAllApplicationData("SUBJECT ID");
            
            //get list of workflow users from the app data

            // for each user:

            UserData currentUser = (UserData)client.Read("USER ID", new ReadOptions());

            // get the workflow items for the user
            UserWorkItemsFilterData userWorkItemsFilter = new UserWorkItemsFilterData();
            userWorkItemsFilter.ActivityState = ActivityState.Started | ActivityState.Assigned;

            // get assignemnt and work list
            IdentifiableObjectData[] workFlowItems = client.GetSystemWideList(userWorkItemsFilter);


            DateTime fakeDate = DateTime.Now;

            // then if there is a taks
            foreach (WorkItemData workItem in workFlowItems)
            {
                if (fakeDate < workItem.VersionInfo.CreationDate)
                {
                    t
                    // add to list of things to push
                    // hand that list, appdata, user ide, user desc & name to the notifier
                    Console.WriteLine(currentUser.Id + currentUser.Description + "APPDATA");
                }

            }
            


        }
    }
}
