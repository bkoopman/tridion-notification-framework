using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Tridion.ContentManager.CoreService.Client;

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

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                Indent = false,
                OmitXmlDeclaration = false
            };

            XNode userDataNode = null;
            XNode workItemDataNode = null;
            //Serialize userData to XML
            XmlSerializer udSerializer = new XmlSerializer(userData.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    udSerializer.Serialize(xmlWriter, userData);
                }
                userDataNode = XElement.Parse(textWriter.ToString()).DescendantNodesAndSelf().FirstOrDefault();
            }

            //Serialize WorkItemData[] to XML
            XmlSerializer wiSerializer = new XmlSerializer(workItemData.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    wiSerializer.Serialize(xmlWriter, workItemData);
                }
                workItemDataNode = XElement.Parse(textWriter.ToString()).DescendantNodesAndSelf().FirstOrDefault();
            }

            XElement wfElement = new XElement("WorkflowInfo");
            wfElement.Add(userDataNode);
            wfElement.Add(workItemDataNode);
            return wfElement;

        }
    }
}