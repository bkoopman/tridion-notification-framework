using System;
using System.Diagnostics;

namespace TridionCommunity.NotificationFramework
{
    public class NetworkMessageNotifier : INotifier
    {
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
                throw new Exception(string.Format("Failed to notify the user '{0}'. Exit code was {1}.", userName, process.ExitCode));
            }
        }

        public void Notify(NotificationData data)
        {
            throw new NotImplementedException();
        }
    }
}
