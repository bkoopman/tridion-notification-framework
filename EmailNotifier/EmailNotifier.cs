using System;
using System.Configuration;
using System.IO;
using System.Net;
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
        //     <Notifier type="WorkflowEmailNotifier"
        //                notification_frequency="3D"
        //                notification_last_send="2012-10-07T23:13Z">
        //        <EmailAddress>punter@outfit.org</EmailAddress>
        //    </Notifier>
        private static string xslt = null;

        protected override void Notify(UserData userData, WorkItemData[] workItemData, XElement applicationData)
        {
            var emailaddress = applicationData.Element("EmailAddress").Value;
            var xml = GetWorkflowDataXml(userData, workItemData, applicationData);
            
            if (xslt == null)
            {
                using (var client = new SessionAwareCoreServiceClient("wsHttp_2011")) //TODO: Refactor
                {
                    
                    client.Impersonate(userData.Title);
                    var xsltBody =
                        client.Read(ConfigurationManager.AppSettings.Get("EmailNotifier.TcmIdXslt"), new ReadOptions())
                        as TemplateBuildingBlockData;
                    xslt = xsltBody.Content;
                }
            }
 
            var myXslTrans = new XslCompiledTransform();
            myXslTrans.Load(new XmlTextReader(new StringReader(xslt)));            
            using (var sr = new StringWriter())
            {
                myXslTrans.Transform( xml.CreateNavigator(), null, sr);

                SendMail(emailaddress, ConfigurationManager.AppSettings.Get("EmailNotifier.SmtpUsername") , "Tridion Community Email notifier", sr.ToString());
            }
                  
        }

        private void SendMail(string mailTo, string mailFrom, string subject, string mailMessage)
        {

            using (var mail = new MailMessage(mailFrom, mailTo))
            {
                mail.Subject = subject;
                mail.Body = mailMessage;
                
                var host = ConfigurationManager.AppSettings.Get("EmailNotifier.SmtpHost");
                var userName = ConfigurationManager.AppSettings.Get("EmailNotifier.SmtpUsername");
                var password = ConfigurationManager.AppSettings.Get("EmailNotifier.SmtpPassword");
                var port = ConfigurationManager.AppSettings.Get("EmailNotifier.SmtpPort");

                using (var smtp = new SmtpClient()
                                      {
                                          Host =  host,
                                          Credentials = new NetworkCredential(userName, password),
                                          DeliveryMethod = SmtpDeliveryMethod.Network,
                                          Port = Int16.Parse(port)
                                      }
                    
                    )
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

        public sealed override string[] GetSupportedNotifierTypes()
        {
            return new string[] { "WorkflowEmailNotifier" };
        }        
    }
}
