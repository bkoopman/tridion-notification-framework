using System.ServiceProcess;
using NotificationService.CoreService;
using Tridion.ContentManager.CoreService.Client;
using System.Collections.Generic;

namespace NotificationService
{
    partial class WorkflowNotificationService : ServiceBase
    {

        protected override void OnStart(string[] args)
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

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "WorkflowNotificationService";
        }

        #endregion
    }
}
