using System;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Timers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;

namespace Zarepath.WindowServices
{
    public partial class RespiteHoursReset : ServiceBase
    {
        private bool _isRunning = false;

        public RespiteHoursReset()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            timerFirst.Interval = Convert.ToInt64(ConfigSettings.RespiteHoursIntervalTimeinMinute) * 60 * 1000;
            timerFirst.Elapsed += new ElapsedEventHandler(ExecuteFirst);
            timerFirst.Enabled = true;
            Common.CreateLogFile("RespiteHours Service Started.", ConfigSettings.RespiteHoursFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.RespiteHourseLog));
            RunRespiteHoursResetService();
        }

        private void ExecuteFirst(object sender, EventArgs e)
        {
            RunRespiteHoursResetService();
        }


        private void RunRespiteHoursResetService()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.RespiteHourseLog);
                try
                {
                    Common.CreateLogFile("RespiteHours Service Call Started.", ConfigSettings.RespiteHoursFileName, logpath);
                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.RespiteHourseURL;

                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("RespiteHours Service URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.RespiteHoursFileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("RespiteHours Service URL Called With Unsuccessful Response is null."), ConfigSettings.RespiteHoursFileName, logpath);
                    }
                    Common.CreateLogFile("RespiteHours Service Call Completed Successfully.", ConfigSettings.RespiteHoursFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.RespiteHoursFileName, logpath);
                    Common.CreateLogFile("RespiteHours Service Call Completed Successfully.", ConfigSettings.RespiteHoursFileName, logpath);
                }
                _isRunning = false;
            }
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("RespiteHours Service Stopped.", ConfigSettings.RespiteHoursFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.RespiteHourseLog));
        }
    }
}
