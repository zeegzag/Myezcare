using System;
using System.Configuration;

namespace ClockInOutSmsService
{
    public class ConfigSettings
    {
        public static readonly string SiteBaseURL = ConfigurationManager.AppSettings["SiteBaseURL"];
        // time interval for the sms send
        public static readonly long SmsTimerInterval = Convert.ToInt64(ConfigurationManager.AppSettings["SmsTimerInterval"]);
        public static readonly string LogPath = ConfigurationManager.AppSettings["LogPath"];
        public static readonly string ClockInOutServiceLogFileName = ConfigurationManager.AppSettings["ClockInOutServiceLogFileName"];
        public static readonly string ClockInOutServiceLog = ConfigurationManager.AppSettings["ClockInOutServiceLog"];
        public static readonly string SendClockInOutSmsURL = ConfigurationManager.AppSettings["SendClockInOutSmsURL"];
    }
}
