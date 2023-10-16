using System;
using System.Configuration;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace ClockInOutSmsService
{
    public class Common
    {
        public static string CreateLogFile(string message, string logFileName = "", string logPath = "")
        {
            string virtualPath = logPath;
            if (String.IsNullOrEmpty(logFileName))
                logFileName = "Exception" + DateTime.Today.ToString("MMddyyyy") + ".txt";

            if (String.IsNullOrEmpty(logPath))
            {
                if (HttpContext.Current != null)
                {
                    logPath = HttpContext.Current.Server.MapPath(ConfigSettings.LogPath);
                    virtualPath = ConfigSettings.LogPath;
                }
                else
                {
                    logPath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["Logpath"];
                    virtualPath = ConfigurationManager.AppSettings["Logpath"];
                }
            }
            else
            {
                if (HttpContext.Current != null)
                    logPath = HttpContext.Current.Server.MapPath(logPath);
                else
                    logPath = AppDomain.CurrentDomain.BaseDirectory + logPath;
            }

            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string fileName = logFileName;
            string fileFullPath = logPath + fileName;
            virtualPath = virtualPath + fileName;
            var sr = new StreamWriter(fileFullPath, true);
            sr.WriteLine("DateTime:" + DateTime.Now);
            sr.WriteLine(message);
            sr.WriteLine("===============================================================================================================");
            sr.Flush();
            sr.Close();

            return virtualPath;
        }

        public static string SerializeObject<T>(T objectData)
        {
            string defaultJson = JsonConvert.SerializeObject(objectData);
            return defaultJson;
        }

    }
}
