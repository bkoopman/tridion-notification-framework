using Tridion.ContentManager.CoreService.Client;


namespace NotificationService.CoreService
{
    public class Client
    {
        public static SessionAwareCoreServiceClient GetCoreService(string username)
        {
            var result = new SessionAwareCoreServiceClient();
            result.Impersonate(username);
            return result;
        }
    }
}
