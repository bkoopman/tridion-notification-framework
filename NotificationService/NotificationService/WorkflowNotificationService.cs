using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace NotificationService
{
    public partial class WorkflowNotificationService : ServiceBase
    {
        public WorkflowNotificationService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Logger.SetLogLevel();
            // To be able to start stervice without waiting
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();

            // Thread.Sleep(Convert.ToInt16(ConfigurationManager.AppSettings.Get("pollingInterval")) * 1000);
        }

        
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Worker.DoWork();                
            }
            catch (Exception ex)
            {
                Logger.WriteToLog(ex.Message, EventLogEntryType.Error);
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.WriteToLog(e.Error.Message, EventLogEntryType.Error);
            }
        }



        protected override void OnStop()
        {
                            
            
        }


    }
}
