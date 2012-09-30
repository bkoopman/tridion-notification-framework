using System;
using System.Diagnostics;
using NetworkMessageNotifier;

namespace TridionCommunity.NotificationFramework
{
    public class NetworkMessageNotifier : INotifier
    {
        public void Notify(NotificationData data)
        {
            var notificationData = data as WorkflowNotificationData;
            if (notificationData == null) return;

            var userName = notificationData.User.Title;
            var message = string.Format(Resources.NotificationMessage, notificationData.WorkItems.Length);
            SendMessage(userName, message);
        }

        public void SendMessage(string userName, string message)
        {
            var process = GetProcess(userName, message);
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception(string.Format(Resources.FailedToSendMessage, userName, process.ExitCode));
            }
        }

        internal Process GetProcess(string userName, string message)
        {
            return new Process
            {
                StartInfo =
                {
                    FileName = "msg.exe",
                    Arguments = userName + " " + message,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
        }
    }
}
