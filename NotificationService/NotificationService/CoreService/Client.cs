using System;
using System.Configuration;
using System.ServiceModel;
using Tridion.ContentManager.CoreService.Client;


namespace NotificationService.CoreService
{
    public class Client
    {
        public static SessionAwareCoreServiceClient GetCoreService()
        {
            var result = GetNewClient<SessionAwareCoreServiceClient>();


            result.Impersonate(ConfigurationManager.AppSettings.Get("adminUser"));
            return result;
        }

        public static EndpointAddress SetCorrectServer(EndpointAddress currentAddress)
        {
            var newHost = new Uri(ConfigurationManager.AppSettings.Get("server"));

            if (currentAddress.Uri.Scheme == "net.tcp")
            {
                currentAddress = new EndpointAddress(currentAddress.ToString().Replace(currentAddress.Uri.Host, newHost.Host));
            }
            else
            {
                currentAddress = new EndpointAddress(currentAddress.ToString().Replace(currentAddress.Uri.Authority, newHost.Authority));
            }

            return currentAddress;
        }

        public static T GetNewClient<T>()
        {
            object obj;
                        
            if (typeof(T) == typeof(SessionAwareCoreServiceClient))
            {
                SessionAwareCoreServiceClient client = new SessionAwareCoreServiceClient(ConfigurationManager.AppSettings.Get("endpointName"));
                client.ChannelFactory.Endpoint.Address = SetCorrectServer(client.ChannelFactory.Endpoint.Address);
                obj = client;
            }

            else
            {
                CoreServiceClient client = new CoreServiceClient(ConfigurationManager.AppSettings.Get("endpointName"));
                client.ChannelFactory.Endpoint.Address = SetCorrectServer(client.ChannelFactory.Endpoint.Address);
                obj = client;
            }

            return (T)obj;
        }


    }
}
