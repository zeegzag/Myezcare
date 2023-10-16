using System;
using System.Configuration;
using System.ServiceProcess;
using System.Timers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;

namespace Zarepath.EmailService
{
    partial class EmailService : ServiceBase
    {
        private Timer timerFirst = null;
        private bool _IsRunningFirst;

        public EmailService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _IsRunningFirst = false;
            timerFirst = new Timer();
            long serviceExecuteInterval = Convert.ToInt64(ConfigurationManager.AppSettings["EmailServiceInterval"]) * 1000;
            timerFirst.Interval = serviceExecuteInterval;
            timerFirst.Elapsed += new ElapsedEventHandler(ExecuteFirst);
            timerFirst.Enabled = true;
        }

        protected void ExecuteFirst(object sender, EventArgs e)
        {
            string logFileName = "";
            string logpath = AppDomain.CurrentDomain.BaseDirectory + ConfigSettings.LogPath + ConfigSettings.EmailLog;
            if (!_IsRunningFirst)
            {
                _IsRunningFirst = true;
                try
                {
                    //Code
                    Common.CreateLogFile("Email service process is started", ConfigSettings.EmailFileName, logpath);

                    //Send Mail
                    IEmailServiceDataProvider _emailServiceDataProvider = new EmailServiceDataProvider();
                    ServiceResponse response = _emailServiceDataProvider.SendEmail();
                    //End

                    Common.CreateLogFile(Convert.ToString(response.Data), ConfigSettings.EmailFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(ex.ToString(), ConfigSettings.EmailFileName, logpath);
                }
                _IsRunningFirst = false;
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
