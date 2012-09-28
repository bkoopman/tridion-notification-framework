﻿using System;
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

        //public void Notify(INotificationMessage message)
        //{
        //    ThreadPool.QueueUserWorkItem(sendMail => SendMail(message));
        //}

        public void Notify(UserData userData, WorkItemData[] workItemData)
        {

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = new UnicodeEncoding(false, false), // no BOM in a .NET string
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
            XmlTextWriter myWriter = new XmlTextWriter("result.html", null);
            myXslTrans.Transform(myXPathDoc, null, myWriter);


        }
       
        private void SendMail(INotificationMessage message)
        {

            using (MailMessage mail = new MailMessage("workflowmailer@mycompany.com", "user@address.com"))
            {
                mail.Subject = "Your notifications";
                mail.Body = "Stuff waiting for you";

                using (SmtpClient smtp = new SmtpClient())
                {
                    try
                    {
                        smtp.Send(mail);
                    }
                    catch (ArgumentNullException e)
                    {
                        throw;
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
