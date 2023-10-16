using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Timers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;


namespace Zarepath.WindowServices
{
    partial class SendMissingDocumentEmail : ServiceBase
    {

        private bool _isRunning = false;
        private Timer _timerFirst;
        public SendMissingDocumentEmail()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            timerFirst.Interval = Convert.ToInt64(ConfigSettings.SendMissingDocumentEmailIntervalTimeinMinute) * 60 * 1000;////(int)MillisecondsToNextTargetTime(_runTime);
            timerFirst.Elapsed += new ElapsedEventHandler(SendMissingDocEmail);
            timerFirst.Enabled = true;
            Common.CreateLogFile("SendMissingDocumentEmail Service Started.", ConfigSettings.SendMissingDocumentEmailFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.SendMissingDocumentEmailLog));
            RunSendMissingDocEmailService();
        }



        private void SendMissingDocEmail(object sender, EventArgs e)
        {
            RunSendMissingDocEmailService();
        }
        
        private void RunSendMissingDocEmailService()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.SendMissingDocumentEmailLog);
                try
                {
                    Common.CreateLogFile("SendMissingDocumentEmail Service Call Started.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);

                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.SendMissingDocumentEmailURL;
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("SendMissingDocumentEmailLog Service URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("SendMissingDocumentEmailFileName Service URL Called With Unsuccessful Response is null."), ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                    }
                    Common.CreateLogFile("SendMissingDocumentEmailFileName Service Call Completed Successfully.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                    Common.CreateLogFile("SendMissingDocumentEmailFileName Service Call Completed Successfully.", ConfigSettings.SendMissingDocumentEmailFileName, logpath);
                }

                //SetNextCallInterval();
                _isRunning = false;
            }
        }
        
        public void SetNextCallInterval()
        {
            _timerFirst.Enabled = false;
            DateTime currentDate = DateTime.Now;
            DateTime nextMonthLastDate = GetLastDayOfMonth(new DateTime(currentDate.Year, currentDate.Month + 1, 1));
            TimeSpan timediff = nextMonthLastDate - DateTime.Now;
            _timerFirst.Interval = Convert.ToInt64(timediff.TotalMinutes) * 60 * 1000;
            _timerFirst.Enabled = true;

        }
        
        public static DateTime GetLastDayOfMonth(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("SendMissingDocumentEmail Service Stoped.", ConfigSettings.SendMissingDocumentEmailFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.SendMissingDocumentEmailLog));
        }
    }
}
