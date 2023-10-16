using System;
using System.Configuration;
using System.Web;

namespace Myezcare_Admin.Infrastructure
{
    public class ConfigSettings
    {
        public static readonly string SiteBaseURL = Common.GetSiteBaseUrl();

        public static readonly bool EnableBundlingMinification = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableBundlingMinification"]);

        public static readonly int DbCommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["DBCommandTimeOut"]);
        public static readonly int RememberMeDuration = Convert.ToInt16(ConfigurationManager.AppSettings["RememberMeDuration"]);
        public static readonly int PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

        public static readonly string LogPath = ConfigurationManager.AppSettings["LogPath"];
        public static readonly string UploadPath = ConfigurationManager.AppSettings["UploadPath"];
        public static readonly string TemplateBasePath = ConfigurationManager.AppSettings["TemplateBasePath"];

        public static readonly string DateFormatForSaveFile = ConfigurationManager.AppSettings["DateFormatForSaveFile"];

        public static readonly string AppDateTimeFormat = ConfigurationManager.AppSettings["AppDateTimeFormat"];


        public static readonly string PublicSiteUrl = ConfigurationManager.AppSettings["PublicSiteUrl"];
        public static readonly string UpdateSiteCacheUrl = ConfigurationManager.AppSettings["UpdateSiteCacheUrl"];
        public static readonly string SiteLogo = ConfigurationManager.AppSettings["SiteLogo"];

        public static readonly string ImportPath = ConfigurationManager.AppSettings["ImportPath"];
        public static readonly string DataImportUploadPath = string.Format("{0}{1}", UploadPath, DataImportUploadPath);

        public static readonly string EbriggsUrl = ConfigurationManager.AppSettings["EbriggsUrl"];
        public static readonly string EbriggsUserName = ConfigurationManager.AppSettings["EbriggsUserName"];
        public static readonly string EbriggsPassword = ConfigurationManager.AppSettings["EbriggsPassword"];
        public static readonly string MyezcareFormsUrl = ConfigurationManager.AppSettings["MyezcareFormsUrl"];
    }
}