namespace TridionCommunity.NotificationFramework
{
    public interface INotifier
    {
        void Notify(NotificationData data);
        // Returning null means "I'm just not fussy, send me anything"
        // REVIEW - is there a better way to express this contract?
        string[] GetSupportedNotifierTypes();
    }
}