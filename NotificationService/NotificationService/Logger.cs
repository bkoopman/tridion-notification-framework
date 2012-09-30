using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NotificationService
{
    public static class Logger
    {
        private  static EventLogEntryType logLevel = EventLogEntryType.Error;
        private const string logName = "Application";

        public static void WriteToLog(string message, EventLogEntryType entryType)
        {
            switch (entryType)
            {
                case EventLogEntryType.Error:
                    {
                        EventLog.WriteEntry(logName, message, entryType);
                        break;
                    }
                default:
                case EventLogEntryType.Information:
                    {
                        if (logLevel == EventLogEntryType.Information)
                        {
                            EventLog.WriteEntry(logName, message, entryType);
                        }
                        break;
                    }
                case EventLogEntryType.Warning:
                    {
                        if (logLevel == EventLogEntryType.Information | logLevel == EventLogEntryType.Warning)
                        {
                            EventLog.WriteEntry(logName, message, entryType);
                        }
                        break;
                    }
            }
        }

        public static void SetLogLevel()
        {            
            switch (ConfigurationManager.AppSettings.Get("logLevel").ToUpper())
            {                    
                case "INFO":
                    {
                        logLevel = EventLogEntryType.Information;
                        break;
                    }
                case "ERROR":
                    {
                        logLevel = EventLogEntryType.Information;
                        break;
                    }
                case "WARNING":
                    {
                        logLevel = EventLogEntryType.Information;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
