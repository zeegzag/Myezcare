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
    public partial class ScheduleBatchServices : ServiceBase
    {

        private bool _isRunning = false;
        public ScheduleBatchServices()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            long serviceExecuteInterval = Convert.ToInt64(ConfigSettings.ScheduleBatchServicesTimeinMinute) * 60 * 1000;
            timerFirst.Interval = serviceExecuteInterval;
            timerFirst.Elapsed += new ElapsedEventHandler(ExecuteFirst);
            timerFirst.Enabled = true;
            Common.CreateLogFile("ScheduleBatchServices Service Started.", ConfigSettings.ScheduleBatchServicesFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.ScheduleBatchServicesLog));
            RunScheduleBatchServices();
        }

        private void ExecuteFirst(object sender, EventArgs e)
        {
            RunScheduleBatchServices();
        }

        private void RunScheduleBatchServices()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ScheduleBatchServicesLog);
                try
                {
                    Common.CreateLogFile("ScheduleBatchServices Service Call Started.", ConfigSettings.ScheduleBatchServicesFileName, logpath);

                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.ScheduleBatchServicesURL;
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("ScheduleBatchServices URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.ScheduleBatchServicesFileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("ScheduleBatchServices Service URL Called With Unsuccessful Response is null."), ConfigSettings.ScheduleBatchServicesFileName, logpath);
                    }
                    Common.CreateLogFile("ScheduleBatchServices Service Call Completed Successfully.", ConfigSettings.ScheduleBatchServicesFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.ScheduleBatchServicesFileName, logpath);
                    Common.CreateLogFile("ScheduleBatchServices Service Call Completed Successfully.", ConfigSettings.ScheduleBatchServicesFileName, logpath);
                }
                _isRunning = false;
            }
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("ScheduleBatchServices Service Stopped.", ConfigSettings.ScheduleBatchServicesFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.DeleteEDIFileLog));
        }
    }
}
