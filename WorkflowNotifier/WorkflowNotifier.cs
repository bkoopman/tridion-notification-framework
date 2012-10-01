using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Tridion.ContentManager.CoreService.Client;
using TridionCommunity.NotificationFramework.ExtensionMethods;

namespace TridionCommunity.NotificationFramework
{
    public abstract class WorkflowNotifier : INotifier
    {
        public WorkflowNotifier()
        {
        }

        public void Notify(NotificationData data)
        {
            var workflowData = data as WorkflowNotificationData;
            if (workflowData != null)
            {
                Notify(workflowData.User, workflowData.WorkItems);
            }
        }

        protected abstract void Notify(UserData userData, WorkItemData[] workItemData);

        protected XElement GetWorkflowDataXml(UserData userData, WorkItemData[] workItemData)
        {
            //Serialize userData to XElement
            var userDataNode = userData.SerializeToXElement().DescendantNodesAndSelf().FirstOrDefault();

            //Serialize workitemData to XElement
            var workItemDataNode = workItemData.SerializeToXElement().DescendantNodesAndSelf().FirstOrDefault();

            XElement wfElement = new XElement("WorkflowInfo");
            wfElement.Add(userDataNode);
            wfElement.Add(workItemDataNode);

            return wfElement;

        }
    }
}