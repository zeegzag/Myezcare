using System;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Timers;
using Zarephath.Core.Infrastructure;

namespace Zarepath.WindowServices
{
    public partial class DeleteEDIFileLog : ServiceBase
    {
        private bool _isRunning = false;

        public DeleteEDIFileLog()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            long serviceExecuteInterval = Convert.ToInt64(ConfigSettings.DeleteEdiFileTimeinMinute) * 60 * 1000;
            timerFirst.Interval = serviceExecuteInterval;
            timerFirst.Elapsed += new ElapsedEventHandler(ExecuteFirst);
            timerFirst.Enabled = true;
            Common.CreateLogFile("Delete EDIFileLog Service Started.", ConfigSettings.DeleteEDIFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.DeleteEDIFileLog));
            RunDeleteEdiFileService();
        }

        private void ExecuteFirst(object sender, EventArgs e)
        {
            RunDeleteEdiFileService();

            #region Scrap Code

            //if (!_isRunning)
            //{
            //    _isRunning = true;
            //    string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.DeleteEDIFileLog);
            //    try
            //    {
            //        Common.CreateLogFile("Delete EDIFileLog Service Call Started.", ConfigSettings.DeleteEDIFileName, logpath);

            //        string url = ConfigSettings.SiteBaseURL + ConfigSettings.DeleteEDIFileLogURL;
            //        WebRequest request = WebRequest.Create(url);
            //        HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            //        if (response != null && response.StatusCode == HttpStatusCode.OK)
            //        {
            //            Common.CreateLogFile(string.Format("Delete EDIFileLog Service URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.DeleteEDIFileName, logpath);
            //        }
            //        else
            //        {
            //            Common.CreateLogFile(string.Format("Delete EDIFileLog Service URL Called With Unsuccessful Response is null."), ConfigSettings.DeleteEDIFileName, logpath);
            //        }
            //        Common.CreateLogFile("Delete EDIFileLog Service Call Completed Successfully.", ConfigSettings.DeleteEDIFileName, logpath);
            //    }
            //    catch (Exception ex)
            //    {
            //        Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.DeleteEDIFileName, logpath);
            //    }
            //    _isRunning = false;
            //}

            #endregion
        }


        private void RunDeleteEdiFileService()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.DeleteEDIFileLog);
                try
                {
                    Common.CreateLogFile("Delete EDIFileLog Service Call Started.", ConfigSettings.DeleteEDIFileName, logpath);

                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.DeleteEDIFileLogURL;
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("Delete EDIFileLog Service URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.DeleteEDIFileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("Delete EDIFileLog Service URL Called With Unsuccessful Response is null."), ConfigSettings.DeleteEDIFileName, logpath);
                    }
                    Common.CreateLogFile("Delete EDIFileLog Service Call Completed Successfully.", ConfigSettings.DeleteEDIFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.DeleteEDIFileName, logpath);
                    Common.CreateLogFile("Delete EDIFileLog Service Call Completed Successfully.", ConfigSettings.DeleteEDIFileName, logpath);
                }
                _isRunning = false;
            }
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("Delete EDIFileLog Service Stopped.", ConfigSettings.DeleteEDIFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.DeleteEDIFileLog));
        }
    }
}
