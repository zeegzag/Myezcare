using System;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Timers;

namespace ClockInOutSmsService
{
    public partial class ClockInOutSms : ServiceBase
    {
        private bool _isRunning = false;

        public ClockInOutSms()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            long serviceExecuteInterval = Convert.ToInt64(ConfigSettings.SmsTimerInterval) * 60 * 1000;
            timerFirst.Interval = serviceExecuteInterval;
            timerFirst.Elapsed += new ElapsedEventHandler(ExecuteFirst);
            timerFirst.Enabled = true;
            Common.CreateLogFile("Send clock in/out Service Started.", ConfigSettings.ClockInOutServiceLogFileName,
                Path.Combine(ConfigSettings.LogPath, ConfigSettings.ClockInOutServiceLog));
            RunSendSmsService();
        }

        private void ExecuteFirst(object sender, EventArgs e)
        {
            RunSendSmsService();
        }

        private void RunSendSmsService()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ClockInOutServiceLog);
                try
                {
                    Common.CreateLogFile("Send clock in/out Service Call Started.", ConfigSettings.ClockInOutServiceLogFileName, logpath);

                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.SendClockInOutSmsURL;
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("Send clock in/out Service URL Called Successfully. And Response Code {0}", response.StatusCode),
                            ConfigSettings.ClockInOutServiceLogFileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("Send clock in/out Service URL Called With Unsuccessful Response is null."),
                            ConfigSettings.ClockInOutServiceLogFileName, logpath);
                    }
                    Common.CreateLogFile("Send clock in/out Service Call Completed Successfully.", ConfigSettings.ClockInOutServiceLogFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.ClockInOutServiceLogFileName, logpath);
                    Common.CreateLogFile("Send clock in/out Service Call Completed Successfully.", ConfigSettings.ClockInOutServiceLogFileName, logpath);
                }
                _isRunning = false;
            }
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("Send clock in/out Service Stopped.",
                ConfigSettings.ClockInOutServiceLogFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.ClockInOutServiceLog));
        }

    }
}
