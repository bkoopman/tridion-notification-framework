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

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8, 
                Indent = false,
                OmitXmlDeclaration = false
            };

            XNode userDataNode = null;
            XNode workItemDataNode = null;
            //Serialize userData to XML
            System.Xml.Serialization.XmlSerializer udSerializer = new System.Xml.Serialization.XmlSerializer(userData.GetType());           
            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    udSerializer.Serialize(xmlWriter, userData);
                }
                userDataNode = XElement.Parse(textWriter.ToString()).DescendantNodesAndSelf().FirstOrDefault();
            }

            //Serialize WorkItemData[] to XML
            System.Xml.Serialization.XmlSerializer wiSerializer = new System.Xml.Serialization.XmlSerializer(workItemData.GetType());
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

            var xml = wfElement.ToString();
            if (xslt == null)
            {
                xslt = ""; //client.Read("tcm:7-159-2048", new ReadOptions()) as TemplateBuildingBlockData;
            }
            //Transform
            XPathDocument myXPathDoc = new XPathDocument(xml);
            XslCompiledTransform myXslTrans = new XslCompiledTransform();
            myXslTrans.Load(xslt);
            using (XmlTextWriter myWriter = new XmlTextWriter("result.html", null))
            {
                using (StringWriter sr = new StringWriter())
                {
                    //Write the mailbody to the StringWriter
                    myXslTrans.Transform(myXPathDoc, null, sr);
                    
                    SendMail("you@domain.com", "asdf@asf.com", "Yeah", sr.ToString());
                }
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
                    }
                    catch (InvalidOperationException e)
                    {
                    }
                    catch (SmtpException e)
                    {
                    }
                }
            }
        }

        
    }
}
