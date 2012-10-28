using System;
using System.IO;
using System.Net.Mail;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using Tridion.ContentManager.CoreService.Client;

namespace TridionCommunity.NotificationFramework
{
    public class EmailNotifier : WorkflowNotifier
    {
        private static string xslt = null;

        protected override void Notify(UserData userData, WorkItemData[] workItemData, XElement applicationData)
        {

            var xml = GetWorkflowDataXml(userData, workItemData, applicationData);
            
            if (xslt == null)
            {
                xslt = ""; //client.Read("tcm:7-159-2048", new ReadOptions()) as TemplateBuildingBlockData;
            }
 
            XslCompiledTransform myXslTrans = new XslCompiledTransform();
            myXslTrans.Load(new XmlTextReader(new StringReader(xslt)));            
            using (StringWriter sr = new StringWriter())
            {
                myXslTrans.Transform( xml.CreateNavigator(), null, sr);
                    
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
