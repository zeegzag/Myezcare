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
    public partial class Edi835FileProcess : ServiceBase
    {
        private bool _isRunning = false;

        public Edi835FileProcess()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            long serviceExecuteInterval = Convert.ToInt64(ConfigSettings.Edi835FileIntervalTimeinMinute) * 60 * 1000;
            timerFirst.Interval = serviceExecuteInterval;
            timerFirst.Elapsed += new ElapsedEventHandler(ExecuteFirst);
            timerFirst.Enabled = true;
            Common.CreateLogFile("Edi835FileProcess Service Started.", ConfigSettings.Edi835FileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi835FileLog));
            RunEdi835FileProcessService();
        }
        private void ExecuteFirst(object sender, EventArgs e)
        {
            RunEdi835FileProcessService();
        }

        private void RunEdi835FileProcessService()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi835FileLog);
                try
                {
                    Common.CreateLogFile("Edi835FileProcess Service Call Started.", ConfigSettings.Edi835FileName, logpath);
                    //ICronJobDataProvider cronJobDataProvider = new CronJobDataProvider();
                    //ServiceResponse response = cronJobDataProvider.ProcessEdi835Files();
                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.Edi835FileProcessURL;
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("Edi835FileProcess Service URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.Edi835FileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("Edi835FileProcess Service URL Called With Unsuccessful Response is null."), ConfigSettings.Edi835FileName, logpath);
                    }
                    Common.CreateLogFile("Edi835FileProcess Service Call Completed Successfully.", ConfigSettings.Edi835FileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.Edi835FileName, logpath);
                    Common.CreateLogFile("Edi835FileProcess Service Call Completed Successfully.", ConfigSettings.Edi835FileName, logpath);
                }
                _isRunning = false;
            }
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("Edi835FileProcess Service Stopped.", ConfigSettings.Edi835FileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.Edi835FileLog));
        }
    }
}
