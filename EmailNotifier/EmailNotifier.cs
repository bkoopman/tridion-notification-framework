using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading;
using Tridion.ContentManager.CoreService.Client;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using TridionCommunity.NotificationFramework.ExtensionMethods;

namespace TridionCommunity.NotificationFramework
{
    public class EmailNotifier : INotifier
    {
        private static string xslt = null;

        public void Notify(NotificationData data)
        {
            var workflowData = data as WorkflowNotificationData;
            if (workflowData != null)
            {
                Notify(workflowData.User, workflowData.WorkItems);
            }
        }

        private void Notify(UserData userData, WorkItemData[] workItemData)
        {
            //Serialize userData to XElement
            var userDataNode = userData.SerializeToXElement().DescendantNodesAndSelf().FirstOrDefault();

            //Serialize workitemData to XElement
            var workItemDataNode = workItemData.SerializeToXElement().DescendantNodesAndSelf().FirstOrDefault();

            XElement wfElement = new XElement("WorkflowInfo");            
            wfElement.Add(userDataNode);
            wfElement.Add(workItemDataNode);

            var xml = wfElement.ToString();
            if (xslt == null)
            {
                //Read XSLT from Tridion
                xslt = ""; //client.Read("tcm:7-159-2048", new ReadOptions()) as TemplateBuildingBlockData;
            }
            //Transform
            XPathDocument myXPathDoc = new XPathDocument(xml);
            XslCompiledTransform myXslTrans = new XslCompiledTransform();
            myXslTrans.Load(new XmlTextReader(new StringReader(xslt)));
            using (StringWriter sr = new StringWriter())
            {
                //Write the mailbody to the StringWriter
                myXslTrans.Transform(myXPathDoc, null, sr);

                SendMail("you@domain.com", "asdf@asf.com", "Yeah", sr.ToString());
            }

        }

        private void SendMail(string mailTo, string mailFrom, string subject, string mailMessage)
        {

            using (MailMessage mail = new MailMessage(mailFrom, mailTo))
            {
                mail.Subject = subject;
                mail.Body = mailMessage;

                using (SmtpClient smtp = new SmtpClient())
                {
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (ArgumentNullException e)
                    {
                        throw new NotificationFailedException(TridionCommunity.NotificationFramework.Properties.Resources.FailedToSendMailMessage, e);
                    }
                    catch (InvalidOperationException e)
                    {
                        throw new NotificationFailedException(TridionCommunity.NotificationFramework.Properties.Resources.FailedToSendMailMessage, e);
                    }
                    catch (SmtpException e)
                    {
                        throw new NotificationFailedException(TridionCommunity.NotificationFramework.Properties.Resources.FailedToSendMailMessage, e);
                    }
                }
            }
        }

        
    }
}
