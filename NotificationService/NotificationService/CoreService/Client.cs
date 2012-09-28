using System.ServiceModel;
using Tridion.ContentManager.CoreService.Client;


namespace NotificationService.CoreService
{
    public class Client
    {
        public static SessionAwareCoreServiceClient GetCoreService()
        {
            var result = new SessionAwareCoreServiceClient();
            result.Impersonate("HOW WILL WE GET THE USER NAME");
            return result;
        }
    }
}
