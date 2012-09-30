using Tridion.ContentManager.CoreService.Client;

namespace TridionCommunity.NotificationFramework
{
    public class WorkflowNotificationData : NotificationData
    {
        public UserData User { get; set; }
        public WorkItemData[] WorkItems { get; set; }
    }
}
