using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tridion.Extensions.SMP.Configuration;
using System.Xml;
using Tridion.Extensions.SMP.Model;
using Tridion.ContentManager.CoreService.Client;
using System.ServiceModel;

namespace Tridion.Extensions.SMP.Handlers
{
    public class BaseHandler
    {
        protected SessionAwareCoreServiceClient session;

        protected string _className;
        protected string _assembly;
        protected Dictionary<string, string> _settings;

        public Dictionary<string, string> GetSettings()
        {
            return _settings;
        }

        public string GetClass()
        {
            return _className;
        }

        public string GetAssembly()
        {
            return _assembly;
        }

        public void Load(XmlNode handlerNode)
        {
            
            //Settings
            _settings = new Dictionary<string, string>();
            XmlNodeList settingsList = handlerNode.SelectNodes(".//Setting");
            foreach (XmlNode setting in settingsList)
            {
                String key = setting.Attributes["Key"].Value;
                String value = setting.Attributes["Value"].Value;
                _settings.Add(key, value);
            }
            //ClassName
            _className = handlerNode.Attributes["Class"].Value;

            //AssemblyName 
            _assembly = handlerNode.Attributes["Assembly"].Value;
        }




        


        protected NetTcpBinding GetTcpBinding()
        {
            NetTcpBinding binding = new NetTcpBinding();
            binding.Name = "netTcp";
            binding.TransactionFlow = true;
            binding.TransactionProtocol = TransactionProtocol.WSAtomicTransaction11;
            binding.MaxReceivedMessageSize = 10485760;
            binding.ReaderQuotas.MaxStringContentLength = 10485760;
            binding.ReaderQuotas.MaxArrayLength = 10485760;

            return binding;
        }

        protected void OpenSession()
        {
            if (session == null || !(session.State == CommunicationState.Opened))
            {
                NetTcpBinding binding = GetTcpBinding();
                EndpointAddress address = new EndpointAddress(string.Format("net.tcp://localhost:2660/CoreService/2011/netTcp", Environment.MachineName));
                session = new SessionAwareCoreServiceClient(binding, address);

            }
            session.Open();
        }

        protected void OpenSession(string userName)
        {
            session.Open();
            session.Impersonate(userName);
        }

        protected void CloseSession()
        {
            if (session.State != CommunicationState.Closed)
                session.Close();
        }

    }
}
