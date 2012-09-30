using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;


using System.Collections.Generic;
using System.Collections;
using Tridion.ContentManager;


using System.Reflection;
using System.IO;

using System.Web;
using Tridion.ContentManager.CoreService.Client;
using System.Runtime.Serialization;
using System.Xml;
using Tridion.Web.UI.Core.Extensibility;
using System.Text;
using System.Web.Script.Serialization;
using Tridion.Extensions.Base;
using System.Xml.Linq;





namespace Tridion.Extensions.Services.Base
{


    /// <summary>
    /// Implementation of the Services
    /// </summary>
    /// <remarks></remarks>    
    public class BaseServices
    {
        public readonly string CONFIGURATION_KEY = "{0}-Settings";
        protected Dictionary<string, string> settings = null;
        protected SessionAwareCoreServiceClient session;
        protected StreamDownloadClient downloadChannel;

        private string userName = "Administrator";

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

        protected BasicHttpBinding GetBasicHttpStreamBinding(){
            BasicHttpBinding downloadBinding = new BasicHttpBinding();
            downloadBinding.Name = "streamDownload_basicHttp_2011";
            downloadBinding.MaxReceivedMessageSize = 209715200;
            downloadBinding.TransferMode = TransferMode.StreamedResponse;
            downloadBinding.MessageEncoding = WSMessageEncoding.Mtom;
            downloadBinding.SendTimeout = TimeSpan.FromMinutes(10);
            downloadBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            downloadBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            return downloadBinding;
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

        protected void OpenChannel() { 
            if(downloadChannel == null || !(downloadChannel.State == CommunicationState.Opened)){
                BasicHttpBinding downloadBinding = GetBasicHttpStreamBinding();
                EndpointAddress address = new EndpointAddress(string.Format("http://localhost/webservices/CoreService2011.svc/streamDownload_basicHttp", Environment.MachineName));
                downloadChannel = new StreamDownloadClient(downloadBinding, address);
            }
            downloadChannel.Open();
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

        protected void CloseChannel()
        {
            if (downloadChannel.State != CommunicationState.Closed)
                downloadChannel.Close();
        }


        public BaseServices()
        {



        }

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void SaveNotificationSettings(BaseSettings settings, string appKey)
        {
            try
            {
                this.OpenSession();

                ApplicationData appData = new ApplicationData();
                appData.ApplicationId = appKey;

                XElement xSettings = new XElement("appKey");
                xSettings.Add(new XElement("SavedDate", settings.SavedDate));
                xSettings.Add(new XElement("UserId", settings.UserId));
                appData.Data = Encoding.UTF8.GetBytes(xSettings.ToString());
                session.SaveApplicationData(settings.UserId, new ApplicationData[] { appData });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseSession();
            }
        }

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public BaseSettings GetNotificationSettings(string userId, string appKey)
        {
            try
            {
                this.OpenSession(userName);

                ApplicationData appData = session.ReadApplicationData(userId, appKey);
                XElement xSettings = XElement.Parse(Encoding.UTF8.GetString(appData.Data));
                BaseSettings settings = new BaseSettings();

                settings.SavedDate = xSettings.Element("SavedDate").Value;                
                settings.UserId = xSettings.Element("UserId").Value;

                return settings;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseSession();
            }
        }

        public byte[] GetMultimediaAsBytes(string tcmId)
        {
            byte[] buffer = null;
            try
            {
                this.OpenChannel();
                Stream s = downloadChannel.DownloadBinaryContent(tcmId);
                MemoryStream m = new MemoryStream();
                int b;
                do
                {
                    b = s.ReadByte();
                    m.WriteByte((byte)b);
                } while (b != -1);

                buffer = m.ToArray();
                return buffer;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                this.CloseChannel();
            }
        }

        public Stream GetMultimediaAsStream(string tcmId)
        {
            
            try
            {
                this.OpenChannel();
                Stream s = downloadChannel.DownloadBinaryContent(tcmId);
                return s;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.CloseChannel();
            }
        }

        

    }
}