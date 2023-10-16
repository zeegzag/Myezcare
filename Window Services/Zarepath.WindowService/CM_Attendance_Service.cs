using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Zarephath.Core.Infrastructure;

namespace Zarepath.WindowServices
{
    public partial class CM_Attendance_Service : ServiceBase
    {
        private bool _isRunning = false;

        public CM_Attendance_Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            long serviceExecuteInterval = Convert.ToInt64(ConfigSettings.AttendanceNotificationTimeinMinute) * 60 * 1000;
            timerFirst.Interval = serviceExecuteInterval;
            timerFirst.Elapsed += new ElapsedEventHandler(ExecuteFirst);
            timerFirst.Enabled = true;
            Common.CreateLogFile("CM Attendance Service Started.", ConfigSettings.AttendanceNotificationFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.AttendanceNotificationLog));
            RunCM_Attendance_Service();
        }

        private void ExecuteFirst(object sender, EventArgs e)
        {
            RunCM_Attendance_Service();
        }


        private void RunCM_Attendance_Service()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.AttendanceNotificationLog);
                try
                {
                    Common.CreateLogFile("CM Attendance Service Call Started.", ConfigSettings.AttendanceNotificationFileName, logpath);

                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.AttendanceNotificationURL;
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("CM Attendance Service URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.AttendanceNotificationFileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("CM Attendance Service URL Called With Unsuccessful Response is null."), ConfigSettings.AttendanceNotificationFileName, logpath);
                    }
                    Common.CreateLogFile("CM Attendance Service Call Completed Successfully.", ConfigSettings.AttendanceNotificationFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.AttendanceNotificationFileName, logpath);
                    Common.CreateLogFile("CM Attendance Service Call Completed Successfully.", ConfigSettings.AttendanceNotificationFileName, logpath);
                }
                _isRunning = false;
            }
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("CM Attendance Service Stopped.", ConfigSettings.AttendanceNotificationFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.AttendanceNotificationLog));
        }
    }
}
