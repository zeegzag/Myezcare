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
    public partial class CM_ServicePlan_Service : ServiceBase
    {
        private bool _isRunning = false;

        public CM_ServicePlan_Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var timerFirst = new Timer();
            long serviceExecuteInterval = Convert.ToInt64(ConfigSettings.ServicePlanTimeinMinute) * 60 * 1000;
            timerFirst.Interval = serviceExecuteInterval;
            timerFirst.Elapsed += new ElapsedEventHandler(ExecuteFirst);
            timerFirst.Enabled = true;
            Common.CreateLogFile("CM ServicePlan Service Started.", ConfigSettings.ServicePlanFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.ServicePlanLog));
            RunCM_ServicePlan_Service();
        }

        private void ExecuteFirst(object sender, EventArgs e)
        {
            RunCM_ServicePlan_Service();
        }


        private void RunCM_ServicePlan_Service()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                string logpath = Path.Combine(ConfigSettings.LogPath, ConfigSettings.ServicePlanLog);
                try
                {
                    Common.CreateLogFile("CM ServicePlan Service Call Started.", ConfigSettings.ServicePlanFileName, logpath);

                    string url = ConfigSettings.SiteBaseURL + ConfigSettings.ServicePlanURL;
                    WebRequest request = WebRequest.Create(url);
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        Common.CreateLogFile(string.Format("CM ServicePlan Service URL Called Successfully. And Response Code {0}", response.StatusCode), ConfigSettings.ServicePlanFileName, logpath);
                    }
                    else
                    {
                        Common.CreateLogFile(string.Format("CM ServicePlan Service URL Called With Unsuccessful Response is null."), ConfigSettings.ServicePlanFileName, logpath);
                    }
                    Common.CreateLogFile("CM ServicePlan Service Call Completed Successfully.", ConfigSettings.ServicePlanFileName, logpath);
                }
                catch (Exception ex)
                {
                    Common.CreateLogFile(Common.SerializeObject(ex), ConfigSettings.ServicePlanFileName, logpath);
                    Common.CreateLogFile("CM ServicePlan Service Call Completed Successfully.", ConfigSettings.ServicePlanFileName, logpath);
                }
                _isRunning = false;
            }
        }

        protected override void OnStop()
        {
            Common.CreateLogFile("CM ServicePlan Service Stopped.", ConfigSettings.ServicePlanFileName, Path.Combine(ConfigSettings.LogPath, ConfigSettings.ServicePlanLog));
        }
    }
}
