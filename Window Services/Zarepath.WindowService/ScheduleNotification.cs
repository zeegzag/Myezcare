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
    partial class ScheduleNotification : ServiceBase
    {

        private bool _isRunning = false;
        public ScheduleNotification()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            timerFirst.Interval = Convert.ToInt64(ConfigSettings.ScheduleNotificationIntervalTimeinMinute) * 60 * 1000;
            timerFirst.Elapsed += new ElapsedEventHandler(ScheduleNotificationFirst);
            timerFirst.Enabled = true;
            Common.CreateLogFile("Schedule Notification Service Started.", ConfigSettings.ScheduleNotificationFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.ScheduleNotificationLog));
            RunScheduleNotificationService();
        }

        private void ScheduleNotificationFirst(object sender, EventArgs e)
        {
            RunScheduleNotificationService();
        }


        private void RunScheduleNotificationService()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ScheduleNotificationLog);
                try
                {
                    Common.CreateLogFile("Schedule Notification Service Call Started.", ConfigSettings.ScheduleNotificationFileName, logpath);

                    //ICronJobDataProvider cronJobDataProvider = new CronJobDataProvider();
                    //ServiceResponse response = cronJobDataProvider.SendEmail();
                    //Common.CreateLogFile(Convert.ToString(response.Data), ConfigSettings.ScheduleNotificationFileName, logpath);

                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.ScheduleNotificationURL;
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("Schedule Notification Service URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.ScheduleNotificationFileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("Schedule Notification Service URL Called With Unsuccessful Response is null."), ConfigSettings.ScheduleNotificationFileName, logpath);
                    }
                    Common.CreateLogFile("Schedule Notification Service Call Completed Successfully.", ConfigSettings.ScheduleNotificationFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.ScheduleNotificationFileName, logpath);
                    Common.CreateLogFile("Schedule Notification Service Call Completed Successfully.", ConfigSettings.ScheduleNotificationFileName, logpath);
                }
                _isRunning = false;
            }
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("Schedule Notification Service Stopped.", ConfigSettings.ScheduleNotificationFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.ScheduleNotificationLog));
        }
    }
}
