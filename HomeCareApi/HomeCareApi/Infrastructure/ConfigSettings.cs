using System;
using System.Configuration;
using System.Web;
using System.Web.Hosting;

namespace HomeCareApi.Infrastructure
{
    public class ConfigSettings
    {
        public static readonly bool IsShowActualError = Convert.ToBoolean(ConfigurationManager.AppSettings["IsShowActualError"]);
        public static readonly string[] AccessAction = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AccessAction"])
            ? ConfigurationManager.AppSettings["AccessAction"].Split(',')
            : new string[0];
        public static readonly string ActionRestrictMessage = ConfigurationManager.AppSettings["ActionRestrictMessage"];
        public static readonly bool IsSaveRequestResponseLog =
            Convert.ToBoolean(ConfigurationManager.AppSettings["IsSaveRequestResponseLog"]);
        public static readonly string AppDateTimeFormat = ConfigurationManager.AppSettings["AppDateTimeFormat"];
        public static readonly string LogPath = ConfigurationManager.AppSettings["LogPath"];
        public static readonly string FolderBasePath = ConfigurationManager.AppSettings["FolderBasePath"];
        public static readonly string PageSizeList = ConfigurationManager.AppSettings["PageSizeList"];
        public static readonly int DbCommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["DBCommandTimeOut"]);
        public static readonly string PageSize = ConfigurationManager.AppSettings["PageSize"];
        public static readonly string TempPath = ConfigurationManager.AppSettings["TempPath"];
        public static readonly string ReferralPath = ConfigurationManager.AppSettings["ReferralPath"];
        public static readonly string EmpSignaturePath = ConfigurationManager.AppSettings["EmpSignaturePath"];
        public static readonly int MinusNumber = Convert.ToInt32(ConfigurationManager.AppSettings["MinusNumber"]);
        public static readonly int KeyExpirationTimeInCache = Convert.ToInt32(ConfigurationManager.AppSettings["KeyExpirationTimeInCache"]);
        public static readonly int TokenExpirationTimeForMobile = Convert.ToInt32(ConfigurationManager.AppSettings["TokenExpirationTimeForMobile"]);
        //public static readonly string UserImageInitialPath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["UserImageInitialPath"]);
        public static readonly string UserImageInitialPath = HttpContext.Current.Request.Url.Authority+ConfigurationManager.AppSettings["UserImageInitialPath"];
        //public static readonly string TwilioAccountSID = ConfigurationManager.AppSettings["TwilioAccountSID"];
        //public static readonly string TwilioAuthToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
        //public static readonly string TwilioFromNo = ConfigurationManager.AppSettings["TwilioFromNo"];
        //public static readonly string DefaultCountryCodeForSms = ConfigurationManager.AppSettings["DefaultCountryCodeForSms"];
        public static readonly string TwilioVoice = ConfigurationManager.AppSettings["TwilioVoice"];
        public static readonly string TwilioLanguage = ConfigurationManager.AppSettings["TwilioLanguage"];
        //public static readonly string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        public static readonly string Distance = ConfigurationManager.AppSettings["Distance"];
        //public static readonly string LocalUrl = ConfigurationManager.AppSettings["LocalUrl"];
        public static readonly double ClockTimeBefore = Convert.ToDouble(ConfigurationManager.AppSettings["ClockTimeBefore"]);
        public static readonly double ClockTimeAfter = Convert.ToDouble(ConfigurationManager.AppSettings["ClockTimeAfter"]);
        public static readonly string ClockInOutServiceLogFileName = ConfigurationManager.AppSettings["ClockInOutServiceLogFileName"];
        public static readonly string ClockInOutServiceLog = ConfigurationManager.AppSettings["ClockInOutServiceLog"];
        public static readonly string CheckClockInOutTime = ConfigurationManager.AppSettings["CheckClockInOutTime"];
        public static readonly string MaxSendClockInOutSms= ConfigurationManager.AppSettings["MaxSendClockInOutSms"];

        public static readonly string ClockInMessageEmp = ConfigurationManager.AppSettings["ClockInMessageEmp"];
        public static readonly string ClockOutMessageEmp = ConfigurationManager.AppSettings["ClockOutMessageEmp"];
        public static readonly string ClockInMessageAdmin = ConfigurationManager.AppSettings["ClockInMessageAdmin"];
        public static readonly string ClockOutMessageAdmin = ConfigurationManager.AppSettings["ClockOutMessageAdmin"];
        public static readonly string WebSiteUrl = ConfigurationManager.AppSettings["WebSiteUrl"];
        public static readonly string APIUrl = ConfigurationManager.AppSettings["APIUrl"];
        public static readonly string ClockOutBeforeTime = ConfigurationManager.AppSettings["ClockOutBeforeTime"];
        public static readonly string ClockInBeforeTime = ConfigurationManager.AppSettings["ClockInBeforeTime"];

        public static readonly int ShowCaptchOnLoginFailedCount = Convert.ToInt16(ConfigurationManager.AppSettings["ShowCaptchOnLoginFailedCount"]);
        public static readonly int AccountLockedOnLoginFailedCount = Convert.ToInt16(ConfigurationManager.AppSettings["AccountLockedOnLoginFailedCount"]);
        public static readonly int MaxNoOfFileAllowedToUpload = Convert.ToInt16(ConfigurationManager.AppSettings["MaxNoOfFileAllowedToUpload"]);

        public static readonly string FcmAuthenticationKey = ConfigurationManager.AppSettings["FcmAuthenticationKey"];
        public static readonly string FcmSenderId = ConfigurationManager.AppSettings["FcmSenderId"];

        public static readonly string IvrLogPath = ConfigurationManager.AppSettings["IvrLogPath"];
        public static readonly string IvrLogFullPath = string.Format("{0}{1}", LogPath, IvrLogPath);
        public static readonly bool IsCaptureCallLog = Convert.ToBoolean(ConfigurationManager.AppSettings["IsCaptureCallLog"]);

        public static readonly string EbriggsUrl = ConfigurationManager.AppSettings["EbriggsUrl"];
        public static readonly string EbriggsUserName = ConfigurationManager.AppSettings["EbriggsUserName"];
        public static readonly string EbriggsPassword = ConfigurationManager.AppSettings["EbriggsPassword"];
        public static readonly string MyezcareFormsUrl = ConfigurationManager.AppSettings["MyezcareFormsUrl"];
        public static readonly string MasterPassword = ConfigurationManager.AppSettings["MasterPassword"];

        public static readonly string OrbeonFormsUsername = ConfigurationManager.AppSettings["OrbeonFormsUsername"];
        public static readonly string OrbeonFormsPassword = ConfigurationManager.AppSettings["OrbeonFormsPassword"];

        public static readonly string OrbeonBaseUrl = ConfigurationManager.AppSettings["OrbeonBaseUrl"];
    }
}