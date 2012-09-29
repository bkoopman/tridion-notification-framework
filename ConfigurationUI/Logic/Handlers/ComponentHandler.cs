using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Tridion.Extensions.SMP.Handlers;
using Tridion.Extensions.SMP.Model;
using Tridion.ContentManager.CoreService.Client;
using Tridion.ContentManager;
using Tridion.Extensions.SMP.Configuration;

namespace Tridion.Extensions.SMP.Handlers
{
    class ComponentHandler:BaseHandler, IItemHandler
    {

        public bool Execute(Account acc, ItemData data,  SocialInput input)
        {
            
            string filePath = _settings[acc.Type + "File"];
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement postNode = doc.CreateElement("Post"); 
            postNode.SetAttribute("account", acc.Email);
            postNode.SetAttribute("date", input.Date);
            postNode.SetAttribute("delay", input.Propagate.ToString());
            postNode.SetAttribute("type", "C");


            XmlElement passNode = doc.CreateElement("Password");
            passNode.InnerText = acc.Password;
            postNode.AppendChild(passNode);
            
            XmlElement nameNode = doc.CreateElement("Name");
            nameNode.InnerText = acc.Name;
            postNode.AppendChild(nameNode);

            XmlElement nickNode = doc.CreateElement("Nickname");
            nickNode.InnerText = acc.Nickname;
            postNode.AppendChild(nickNode);

            XmlElement imgNode = doc.CreateElement("AvatarImage");
            imgNode.InnerText = acc.AvatarImage;
            postNode.AppendChild(imgNode);


            



            string text = "";
            string linkURL = "";
            string templateId = templateId = data.ComponentTemplateId; ;
            if (input.GetTextFromItem)
            {
                
                templateId = _settings["ComponentTemplate" + acc.Type];
                if (templateId == TcmUri.UriNull.ToString()) {
                    templateId = data.ComponentTemplateId;
                }
                text = RenderItem(data.ComponentId, templateId);
            }
            else
            {                
                text = input.Text;
            }
            
            XmlElement textNode = doc.CreateElement("Text");
            XmlCDataSection cdata = doc.CreateCDataSection(text);
            textNode.AppendChild(cdata);
            postNode.AppendChild(textNode);

            if (input.GetLink)
            {
                linkURL = _settings["LinkURL"] + "?componentId=" + data.ComponentId + "&templateId=" + templateId;
            }

            XmlElement linkNode = doc.CreateElement("Link");
            linkNode.InnerText = linkURL;
            postNode.AppendChild(linkNode);

            doc.DocumentElement.InsertBefore(postNode, doc.DocumentElement.FirstChild);            
            doc.Save(filePath);
            return true;
        }

        private string RenderItem(string itemId, string templateId)
        {
            try
            {
                this.OpenSession();
                ResolveInstructionData resolveInstruction = new ResolveInstructionData()
                {
                    IncludeWorkflow = true,
                    IncludeComponentLinks = false,
                    IncludeChildPublications = false,
                    Purpose = ResolvePurpose.Publish,
                    StructureResolveOption = StructureResolveOption.OnlyItems
                };
                RenderInstructionData renderInstruction = new RenderInstructionData()
                {
                    RenderMode = RenderMode.PreviewDynamic
                };
                PublishInstructionData publishInstruction = new PublishInstructionData()
                {
                    MaximumNumberOfRenderFailures = 0,
                    ResolveInstruction = resolveInstruction,
                    RenderInstruction = renderInstruction,
                    StartAt = DateTime.UtcNow,
                    DeployAt = DateTime.MinValue
                };


                RenderedItemData result = session.RenderItem(itemId, templateId, publishInstruction, TcmUri.UriNull.ToString());

                if (result.Content != null && result.Content.Length > 0)
                {
                    string contents = System.Text.Encoding.UTF8.GetString(result.Content);
                    return contents;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                this.CloseSession();
            }
            return "";

        }
        
    }
}
