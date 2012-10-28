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

        public string[] GetSupportedNotifierTypes()
        {
            return null;
        }

        public void SendMessage(string userName, string message)
        {
            var process = new Process
                        {
                            StartInfo =
                                {
                                    FileName = "msg.exe",
                                    Arguments = userName + " " + message,
                                    WindowStyle = ProcessWindowStyle.Hidden
                                }
                        };

            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception(string.Format(Resources.FailedToSendMessage, userName, process.ExitCode));
            }
        }
    }
}
