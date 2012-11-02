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
            var applicationData = XElement.Parse(data.ApplicationData);
            if (!NotifierTypeIsSupported(applicationData)) return;
            var workflowData = data as WorkflowNotificationData;
            if (workflowData != null)
            {
                Notify(workflowData.User, workflowData.WorkItems, applicationData);
            }
        }

        protected abstract void Notify(UserData userData, WorkItemData[] workItemData, XElement applicationData);

        public virtual string[] GetSupportedNotifierTypes() 
        {
            return null;
        }

        protected XElement GetWorkflowDataXml(UserData userData, WorkItemData[] workItemData, XElement applicationData)
        {
            //Serialize userData to XElement
            var userDataNode = userData.SerializeToXElement().DescendantNodesAndSelf().FirstOrDefault();

            //Serialize workitemData to XElement
            var workItemDataNode = workItemData.SerializeToXElement().DescendantNodesAndSelf().FirstOrDefault();

            XElement wfElement = new XElement("WorkflowInfo");
            wfElement.Add(userDataNode);
            wfElement.Add(workItemDataNode);
            wfElement.Add(applicationData);

            return wfElement;

        }

        protected bool NotifierTypeIsSupported(XElement notifierElement){
            string[] supportedTypes = GetSupportedNotifierTypes();
            if (supportedTypes == null)
            {
                return true;
            }
            else
            {                
                var type = notifierElement.Attribute("type").Value;
                return supportedTypes.Contains(type);
            }
            
        }
    }
}