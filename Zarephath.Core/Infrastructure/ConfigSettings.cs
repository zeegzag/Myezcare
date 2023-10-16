using System;
using System.Configuration;
using System.Web;
using Zarephath.Core.Helpers;

namespace Zarephath.Core.Infrastructure
{
    public class ConfigSettings
    {
        public static readonly string SiteBaseURL = Common.GetSiteBaseUrl();
        public static readonly string Domain = ConfigurationManager.AppSettings["Domain"];

        public static readonly long SiteCacheExpiration = Convert.ToInt64(ConfigurationManager.AppSettings["SiteCacheExpiration"]);

        //public static readonly string HomeCareSiteBaseURL = ConfigurationManager.AppSettings["HomeCareSiteBaseURL"];

        //public static readonly string SiteName = ConfigurationManager.AppSettings["SiteName"];
        //public static readonly string SiteLogo = ConfigurationManager.AppSettings["SiteLogo"];
        //public static readonly string SupportEmail = ConfigurationManager.AppSettings["SupportEmail"];

        public static readonly string TrustedConnection = ConfigurationManager.AppSettings["Trusted_Connection"];
        public static readonly string SchedulingEmail = ConfigurationManager.AppSettings["SchedulingEmail"];
        public static readonly string CCEmailAddress = ConfigurationManager.AppSettings["CCEmailAddress"];
        public static readonly string RecordCCEmailAddress = ConfigurationManager.AppSettings["RecordCCEmailAddress"];
        public static readonly string RecordLogEmailAddress = ConfigurationManager.AppSettings["RecordLogEmailAddress"];


        public static readonly int DbCommandTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["DBCommandTimeOut"]);
        public static readonly int  PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        public static readonly int  PageIndex = Convert.ToInt32(ConfigurationManager.AppSettings["PageIndex"]);

        public static readonly string LogPath = ConfigurationManager.AppSettings["LogPath"];
        public static readonly string GroupNoteLog = ConfigurationManager.AppSettings["GroupNoteLog"];
        public static readonly string ChangeServiceCodeLogs = ConfigurationManager.AppSettings["ChangeServiceCodeLogs"];

        public static readonly int RememberMeDuration = Convert.ToInt16(ConfigurationManager.AppSettings["RememberMeDuration"]);
        public static readonly bool EnableBundlingMinification = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableBundlingMinification"]);
        public static readonly int EmailVerificationLinkExpirationTime = Convert.ToInt16(ConfigurationManager.AppSettings["EmailVerificationLinkExpirationTime"]);
        public static readonly string ClientSideDateTimeFormat = ConfigurationManager.AppSettings["ClientSideDateTimeFormat"];
        public static readonly string ClientSideDateFormat = ConfigurationManager.AppSettings["ClientSideDateFormat"];
        public static readonly string ScheduleDateFormat = ConfigurationManager.AppSettings["ScheduleDateFormat"];
        public static readonly string DisplayDateFormat = ConfigurationManager.AppSettings["DisplayDateFormat"];
        public static readonly string DisplayDateTimeFormat = ConfigurationManager.AppSettings["DisplayDateTimeFormat"];



        public static readonly string AmazoneUploadPath = ConfigurationManager.AppSettings["AmazoneUploadPath"];
        public static readonly string ReferralUploadPath = ConfigurationManager.AppSettings["ReferralUploadPath"];
        public static readonly string ReferralAssessmentResultUploadPath = ConfigurationManager.AppSettings["ReferralAssessmentResultUploadPath"];
        public static readonly string Transportation = ConfigurationManager.AppSettings["Transportation"];
        public static readonly string EmpSignatures = ConfigurationManager.AppSettings["EmpSignatures"];

        public static readonly string TempFiles = ConfigurationManager.AppSettings["TempFiles"];

        public static readonly int FileSize = Convert.ToInt32(ConfigurationManager.AppSettings["FileSize"]);
        public static readonly string DateFormatForSaveFile = ConfigurationManager.AppSettings["DateFormatForSaveFile"];
        public static readonly string AppDateTimeFormat = ConfigurationManager.AppSettings["AppDateTimeFormat"];

        public static readonly string NoImageAvailable = ConfigurationManager.AppSettings["NoImageAvailable"];
        public static readonly string NoSignatureAvailable = ConfigurationManager.AppSettings["NoSignatureAvailable"];

        public static readonly int TransportationGroupCapacity = Convert.ToInt32(ConfigurationManager.AppSettings["TransportationGroupCapacity"]);
        public static readonly int MaxStaff = Convert.ToInt32(ConfigurationManager.AppSettings["MaxStaff"]);
        public static readonly string TransPortationConfirmCancelTime = Convert.ToString(ConfigurationManager.AppSettings["TransPortationConfirmCancelTime"]);

        public static readonly string VersionNumber = Convert.ToString(ConfigurationManager.AppSettings["VersionNumber"]);
        public static readonly string TempPath = Convert.ToString(ConfigurationManager.AppSettings["TempPath"]);
        public static readonly string EmployeeSignature = Convert.ToString(ConfigurationManager.AppSettings["EmployeeSignature"]);

        public static readonly string CareFormClientSignatures = Convert.ToString(ConfigurationManager.AppSettings["CareFormClientSignatures"]);

        public static readonly string CareFormPath = Convert.ToString(ConfigurationManager.AppSettings["CareFormPath"]);

        public static readonly string MIFSignaturePath = Convert.ToString(ConfigurationManager.AppSettings["MIFSignaturePath"]);
        public static readonly string ReCapchaSiteKey = Convert.ToString(ConfigurationManager.AppSettings["ReCapchaSiteKey"]);

        #region EDI FILE PATH
        public static readonly string EdiFilePath = ConfigurationManager.AppSettings["EDIFilePath"];
        public static readonly string EdiFileUploadPath = ConfigurationManager.AppSettings["EDIFileUploadPath"];
        public static readonly string EdiFileDownloadPath = ConfigurationManager.AppSettings["EDIFileDownloadPath"];
        public static readonly string TempEdiFileValidationErrorPath = ConfigurationManager.AppSettings["EDIFileValidationErrorPath"];
        public static readonly string EdiFile837Path = ConfigurationManager.AppSettings["EdiFile837Path"];
        public static readonly string EdiFile835Path = ConfigurationManager.AppSettings["EdiFile835Path"];
        public static readonly string EdiFile835CsvPath = ConfigurationManager.AppSettings["EDIFile835CSVPath"];

        public static readonly string EdiFile270Path = ConfigurationManager.AppSettings["EdiFile270Path"];

        public static readonly string EdiFile271Path = ConfigurationManager.AppSettings["EdiFile271Path"];
        public static readonly string EdiFile271CsvPath = ConfigurationManager.AppSettings["EdiFile271CsvPath"];
        public static readonly string Edi271FileLog = ConfigurationManager.AppSettings["Edi271FileLog"];

        public static readonly string EdiFile277Path = ConfigurationManager.AppSettings["EdiFile277Path"];
        public static readonly string EdiFile277CsvPath = ConfigurationManager.AppSettings["EdiFile277CsvPath"];
        public static readonly string Edi277FileLog = ConfigurationManager.AppSettings["Edi277FileLog"];
        public static readonly string Edi277FileName = ConfigurationManager.AppSettings["Edi277FileName"];
        public static readonly string Edi277FileProcessURL = ConfigurationManager.AppSettings["Edi277FileProcessURL"];

        #endregion

        #region Process Event PATH
        public static readonly string ProcessEventPath = ConfigurationManager.AppSettings["ProcessEventPath"];
        private static readonly string ProcessEventLogFolder = ConfigurationManager.AppSettings["ProcessEventLogFolder"];
        public static readonly string ProcessEventLogFileNamePrefix = ConfigurationManager.AppSettings["ProcessEventLogFileNamePrefix"];
        public static readonly string ProcessEventLogPath = string.Format("{0}/{1}", ProcessEventPath, ProcessEventLogFolder);
        #endregion

        #region HHAX FILE PATH
        private static readonly string HHAXFolder = ConfigurationManager.AppSettings["HHAXFolder"];
        public static readonly string HHAXFileNamePrefix = ConfigurationManager.AppSettings["HHAXFileNamePrefix"];
        public static readonly string HHAXPath = string.Format("{0}/{1}", ProcessEventPath, HHAXFolder);
        #endregion

        #region CareBridge FILE PATH
        private static readonly string CareBridgeFolder = ConfigurationManager.AppSettings["CareBridgeFolder"];
        public static readonly string CareBridgeFileNamePrefix = ConfigurationManager.AppSettings["CareBridgeFileNamePrefix"];
        public static readonly string CareBridgePath = string.Format("{0}/{1}", ProcessEventPath, CareBridgeFolder);
        #endregion

        #region Tellus FILE PATH
        private static readonly string TellusFolder = ConfigurationManager.AppSettings["TellusFolder"];
        public static readonly string TellusFileNamePrefix = ConfigurationManager.AppSettings["TellusFileNamePrefix"];
        public static readonly string TellusPath = string.Format("{0}/{1}", ProcessEventPath, TellusFolder);
        #endregion

        #region Sandata FILE PATH
        private static readonly string SandataFolder = ConfigurationManager.AppSettings["SandataFolder"];
        public static readonly string SandataFileNamePrefix = ConfigurationManager.AppSettings["SandataFileNamePrefix"];
        public static readonly string SandataPath = string.Format("{0}/{1}", ProcessEventPath, SandataFolder);
        #endregion

        #region Referral Respite UsageLimit

        public static readonly int RespiteUsageLimit = Convert.ToInt32(ConfigurationManager.AppSettings["RespiteUsageLimit"]);
        public static readonly int ResetRespiteUsageMonth = Convert.ToInt32(ConfigurationManager.AppSettings["ResetRespiteUsageMonth"]);
        public static readonly int ResetRespiteUsageDay = Convert.ToInt32(ConfigurationManager.AppSettings["ResetRespiteUsageDay"]);
        public static readonly bool CheckRespiteFlag = Convert.ToBoolean(ConfigurationManager.AppSettings["CheckRespiteFlag"]);

        #endregion

        #region Report
        public static readonly int ResetRespiteUsageToMonth = Convert.ToInt32(ConfigurationManager.AppSettings["ResetRespiteUsageToMonth"]);
        public static readonly string ReportExcelFilePath = ConfigurationManager.AppSettings["ReportExcelFilePath"];
        #endregion

        #region Template Path
        public static readonly string TemplateBasePath = ConfigurationManager.AppSettings["TemplateBasePath"];
        public static readonly string EncounterPdfHtmlPath = ConfigurationManager.AppSettings["EncounterPdfHtmlPath"];
        #endregion

        #region Twilio

        public static readonly string TwilioAccountSID = Convert.ToString(ConfigurationManager.AppSettings["TwilioAccountSID"]);
        public static readonly string TwilioAuthToken = Convert.ToString(ConfigurationManager.AppSettings["TwilioAuthToken"]);
        public static readonly string TwilioServiceSID = Convert.ToString(ConfigurationManager.AppSettings["TwilioServiceSid"]);

        public static readonly string TwilioFromNo = Convert.ToString(ConfigurationManager.AppSettings["TwilioFromNo"]);
        public static readonly string DefaultCountryCodeForSms = Convert.ToString(ConfigurationManager.AppSettings["DefaultCountryCodeForSms"]);

        #endregion

        #region Windows Service

        //public static readonly string ServiceMode = ConfigurationManager.AppSettings["ServiceMode"];
        public static readonly string Edi835FileIntervalTimeinMinute = ConfigurationManager.AppSettings["Edi835FileIntervalTimeinMinute"];
        public static readonly int ScheduleNotificationIntervalTimeinMinute = Convert.ToInt32(ConfigurationManager.AppSettings["ScheduleNotificationIntervalTimeinMinute"]);
        public static readonly int RespiteHoursIntervalTimeinMinute = Convert.ToInt32(ConfigurationManager.AppSettings["RespiteHoursIntervalTimeinMinute"]);
        public static readonly int DeleteEdiFileTimeinMinute = Convert.ToInt32(ConfigurationManager.AppSettings["DeleteEdiFileTimeinMinute"]);
        public static readonly int ScheduleBatchServicesTimeinMinute = Convert.ToInt32(ConfigurationManager.AppSettings["ScheduleBatchServicesTimeinMinute"]);
        public static readonly string SendMissingDocumentEmailIntervalTimeinMinute = ConfigurationManager.AppSettings["SendMissingDocumentEmailIntervalTimeinMinute"];

        public static readonly string Edi835FileLog = ConfigurationManager.AppSettings["Edi835FileLog"];
        public static readonly string Edi835FileName = ConfigurationManager.AppSettings["Edi835FileName"];
        public static readonly string Edi835FileProcessURL = ConfigurationManager.AppSettings["Edi835FileProcessURL"];

        public static readonly string RespiteHourseLog = ConfigurationManager.AppSettings["RespiteHourseLog"];
        public static readonly string RespiteHoursFileName = ConfigurationManager.AppSettings["RespiteHoursFileName"];
        public static readonly string RespiteHourseURL = ConfigurationManager.AppSettings["RespiteHourseURL"];

        public static readonly string ScheduleNotificationLog = ConfigurationManager.AppSettings["ScheduleNotificationLog"];
        public static readonly string ScheduleReminderLog = ConfigurationManager.AppSettings["ScheduleReminderLog"];
        public static readonly string ScheduleReminderFileName = ConfigurationManager.AppSettings["ScheduleReminderFileName"];

        public static readonly string ScheduleNotificationFileName = ConfigurationManager.AppSettings["ScheduleNotificationFileName"];
        public static readonly string ScheduleNotificationURL = ConfigurationManager.AppSettings["ScheduleNotificationURL"];

        public static readonly string ScheduleBatchServicesLog = ConfigurationManager.AppSettings["ScheduleBatchServicesLog"];
        public static readonly string ScheduleBatchServicesFileName = ConfigurationManager.AppSettings["ScheduleBatchServicesFileName"];
        public static readonly string ScheduleBatchServicesURL = ConfigurationManager.AppSettings["ScheduleBatchServicesURL"];

        public static readonly string SendMissingDocumentEmailLog = ConfigurationManager.AppSettings["SendMissingDocumentEmailLog"];
        public static readonly string SendMissingDocumentEmailFileName = ConfigurationManager.AppSettings["SendMissingDocumentEmailFileName"];
        public static readonly string SendMissingDocumentEmailURL = ConfigurationManager.AppSettings["SendMissingDocumentEmailURL"];

        public static readonly string TakeDbBackUpLog = ConfigurationManager.AppSettings["TakeDbBackUpLog"];
        public static readonly string TakeDbBackUpFileName = ConfigurationManager.AppSettings["TakeDbBackUpFileName"];

        public static readonly string SiteUrlPath = ConfigurationManager.AppSettings["SiteUrlPath"];

        public static readonly int GetDaysForDeleteEDIFileList = Convert.ToInt32(ConfigurationManager.AppSettings["DaysForDeleteEDIFile"]);

        public static readonly string DeleteEDIFileLog = ConfigurationManager.AppSettings["DeleteEDIFileLog"];
        public static readonly string DeleteEDIFileName = ConfigurationManager.AppSettings["DeleteEDIFileName"];
        public static readonly string DeleteEDIFileLogURL = ConfigurationManager.AppSettings["DeleteEDIFileLogURL"];

        public static readonly int AttendanceNotificationTimeinMinute = Convert.ToInt32(ConfigurationManager.AppSettings["AttendanceNotificationTimeinMinute"]);
        public static readonly string AttendanceNotificationLog = ConfigurationManager.AppSettings["AttendanceNotificationLog"];
        public static readonly string AttendanceNotificationFileName = ConfigurationManager.AppSettings["AttendanceNotificationFileName"];
        public static readonly string AttendanceNotificationURL = ConfigurationManager.AppSettings["AttendanceNotificationURL"];


        public static readonly int ServicePlanTimeinMinute = Convert.ToInt32(ConfigurationManager.AppSettings["ServicePlanTimeinMinute"]);
        public static readonly string ServicePlanLog = ConfigurationManager.AppSettings["ServicePlanLog"];
        public static readonly string ServicePlanFileName = ConfigurationManager.AppSettings["ServicePlanFileName"];
        public static readonly string ServicePlanURL = ConfigurationManager.AppSettings["ServicePlanURL"];

        #endregion

        #region Amazon S3 File Upload

        public static readonly string AccessKeyID = Convert.ToString(ConfigurationManager.AppSettings["AccessKeyID"]);
        public static readonly string SecretAccessKeyID = Convert.ToString(ConfigurationManager.AppSettings["SecretAccessKeyID"]);
        public static readonly int SignUrlExpireTimeLimit = Convert.ToInt32(ConfigurationManager.AppSettings["SignUrlExpireTimeLimit"]);
        public static readonly string AmazonS3Url = Convert.ToString(ConfigurationManager.AppSettings["AmazonS3Url"]);
        public static readonly string ZarephathBucket = Convert.ToString(ConfigurationManager.AppSettings["ZarephathBucket"]);
        public static readonly string PrivateAcl = Convert.ToString(ConfigurationManager.AppSettings["PrivateAcl"]);
        public static readonly string PublicAcl = Convert.ToString(ConfigurationManager.AppSettings["PublicAcl"]);

        #endregion

        #region DMT DataMigrationConfig
        public static readonly int DTMMaxFolderLevel = Convert.ToInt32(ConfigurationManager.AppSettings["MaxFolderLevel"]);
        public static readonly string DTMReferalsFolderPath = ConfigurationManager.AppSettings["ReferalsFolderPath"];
        public static readonly string DTMAdminUserName = ConfigurationManager.AppSettings["AdminUserName"];
        public static readonly string DTMLogFileName = ConfigurationManager.AppSettings["DTMLogFileName"];


        #endregion


        #region Print Respite Notice

        public static readonly string RespiteNoticePrintFilePath = ConfigurationManager.AppSettings["RespiteNoticePrintFilePath"];

        #endregion

        #region Bitly Data

        public static readonly string BitlyUserName = Convert.ToString(ConfigurationManager.AppSettings["BitlyUserName"]);
        public static readonly string BitlyApiKey = Convert.ToString(ConfigurationManager.AppSettings["BitlyApiKey"]);
        public static readonly bool UseBitly = Convert.ToBoolean(ConfigurationManager.AppSettings["UseBitly"]);
        #endregion

        #region Monthly Summary All Attachment

        public static readonly string NoteAttachment = ConfigurationManager.AppSettings["NoteAttachment"];

        #endregion


        #region Services Flag To Make Them ON/OFF
        public static readonly bool Service_Client_ScheduleNotification_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_Client_ScheduleNotification_ON"]);
        public static readonly bool Service_Client_ScheduleBatchNotification_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_Client_ScheduleBatchNotification_ON"]);
        public static readonly bool Service_CM_Compliance_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_CM_Compliance_ON"]);
        public static readonly bool Service_CM_Attendance_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_CM_Attendance_ON"]);
        public static readonly bool Service_CM_ServicePlan_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_CM_ServicePlan_ON"]);
        public static readonly bool Service_RespiteHoursReset_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_RespiteHoursReset_ON"]);
        public static readonly bool Service_Edi835FileProcess_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_Edi835FileProcess_ON"]);
        public static readonly bool Service_DeleteEDIFileLog_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_DeleteEDIFileLog_ON"]);

        public static readonly bool Service_Client_ScheduleReminder_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_Client_ScheduleReminder_ON"]);
        #endregion

        public static readonly int URLCallTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["URLCallTimeOut"]);

        public static readonly string DBBackupFolder = ConfigurationManager.AppSettings["DBBackupFolder"];





        #region Home Care
        public static readonly int GenerateEmpRefTimeScheduleDays = Convert.ToInt32(ConfigurationManager.AppSettings["GenerateEmpRefTimeScheduleDays"]);

        public static readonly bool Service_GenerateEmpTimeScheduleDays_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_GenerateEmpTimeScheduleDays_ON"]);
        public static readonly string GenerateEmpTimeScheduleDaysLog = ConfigurationManager.AppSettings["GenerateEmpTimeScheduleDaysLog"];
        public static readonly string GenerateEmpTimeScheduleDaysFileName = ConfigurationManager.AppSettings["GenerateEmpTimeScheduleDaysFileName"];


        public static readonly bool Service_GenerateRefTimeScheduleDays_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_GenerateRefTimeScheduleDays_ON"]);
        public static readonly string GenerateRefTimeScheduleDaysLog = ConfigurationManager.AppSettings["GenerateRefTimeScheduleDaysLog"];
        public static readonly string GenerateRefTimeScheduleDaysFileName = ConfigurationManager.AppSettings["GenerateRefTimeScheduleDaysFileName"];


        public static readonly bool Service_GenerateBulkSchedules_ON = Convert.ToBoolean(ConfigurationManager.AppSettings["Service_GenerateBulkSchedules_ON"]);
        public static readonly string GenerateBulkSchedulesLog = ConfigurationManager.AppSettings["GenerateBulkSchedulesLog"];
        public static readonly string GenerateBulkSchedulesFileName = ConfigurationManager.AppSettings["GenerateBulkSchedulesFileName"];


        public static readonly int ShowCaptchOnLoginFailedCount = Convert.ToInt16(ConfigurationManager.AppSettings["ShowCaptchOnLoginFailedCount"]);
        public static readonly int AccountLockedOnLoginFailedCount = Convert.ToInt16(ConfigurationManager.AppSettings["AccountLockedOnLoginFailedCount"]);

        public static readonly string SiteLogoPath = ConfigurationManager.AppSettings["SiteLogoPath"];
        public static readonly string EmpProfileImg = ConfigurationManager.AppSettings["EmpProfileImg"];
        public static readonly string EmpCertificate = ConfigurationManager.AppSettings["EmpCertificate"];
        public static readonly string RefProfileImg = ConfigurationManager.AppSettings["RefProfileImg"];
        public static readonly string ReferralProfileImg = ConfigurationManager.AppSettings["ReferralProfileImg"];
        public static readonly string ReferralDocumentPath = ConfigurationManager.AppSettings["ReferralDocumentPath"];
        public static readonly string EmployeeDocumentPath = ConfigurationManager.AppSettings["EmployeeDocumentPath"];
        public static readonly string ScheduleDayCarePath = ConfigurationManager.AppSettings["ScheduleDayCarePath"];

        public static readonly bool IsUploadOnCloudServer = Convert.ToBoolean(ConfigurationManager.AppSettings["IsUploadOnCloudServer"]);
        public static readonly string NinjaInvoiceUrl = ConfigurationManager.AppSettings["NinjaInvoiceUrl"];
        public static readonly string XNinjaToken = ConfigurationManager.AppSettings["X-Ninja-Token"];
        public static readonly string NinjaTokenKey = ConfigurationManager.AppSettings["Ninja-TokenKey"];
        public static readonly string DxToken = ConfigurationManager.AppSettings["DxToken"];


        #endregion



        public static readonly string FcmAuthenticationKey = ConfigurationManager.AppSettings["FcmAuthenticationKey"];
        public static readonly string FcmSenderId = ConfigurationManager.AppSettings["FcmSenderId"];

        public static readonly int GenerateEmpRefTimeScheduleDaysForNextMonths = Convert.ToInt32(ConfigurationManager.AppSettings["GenerateEmpRefTimeScheduleDaysForNextMonths"]);


        public static readonly string EbriggsUrl = ConfigurationManager.AppSettings["EbriggsUrl"];
        public static readonly string EbriggsUserName = ConfigurationManager.AppSettings["EbriggsUserName"];
        public static readonly string EbriggsPassword = ConfigurationManager.AppSettings["EbriggsPassword"];
        public static readonly string MyezcareFormsUrl = ConfigurationManager.AppSettings["MyezcareFormsUrl"];
        public static readonly string MyezcarePdfFormsUrl = ConfigurationManager.AppSettings["MyezcarePdfFormsUrl"];

        public static readonly string MasterPassword = ConfigurationManager.AppSettings["MasterPassword"];

        public static readonly string MailChimpApiKey = ConfigurationManager.AppSettings["MailChimpApiKey"];
        public static readonly string MailChimpListId = ConfigurationManager.AppSettings["MailChimpListId"];
        public static readonly string MailChimpApiURL = ConfigurationManager.AppSettings["MailChimpApiURL"];
        public static readonly string MailChimpUserName = ConfigurationManager.AppSettings["MailChimpUserName"];

        // Google Drive Integration Setting
        public static readonly string GoogleDriveAuthCallback = ConfigurationManager.AppSettings["GoogleDriveAuthCallback"];
        public static readonly string GoogleDriveClientID = ConfigurationManager.AppSettings["GoogleDriveClientID"];
        public static readonly string GoogleDriveClientSecret = ConfigurationManager.AppSettings["GoogleDriveClientSecret"];
        public static readonly string GoogleDriveBaseUri = ConfigurationManager.AppSettings["GoogleDriveBaseUri"];
        public static readonly string GoogleDriveAuthUri = ConfigurationManager.AppSettings["GoogleDriveAuthUri"];
        public static readonly string GoogleDriveTokenUri = ConfigurationManager.AppSettings["GoogleDriveTokenUri"];
        public static readonly string GoogleDriveContentUri = ConfigurationManager.AppSettings["GoogleDriveContentUri"];
        public static readonly string GoogleDriveDeleteUri = ConfigurationManager.AppSettings["GoogleDriveDeleteUri"];
        public static readonly string GoogleDriveFilesListUri = ConfigurationManager.AppSettings["GoogleDriveFilesListUri"];


        public static readonly string OrbeonBaseUrl = ConfigurationManager.AppSettings["OrbeonBaseUrl"];
        public static readonly string OrbeonUserName = ConfigurationManager.AppSettings["OrbeonUserName"];
        public static readonly string OrbeonPassword = ConfigurationManager.AppSettings["OrbeonPassword"];

        public static readonly string DownloadAndProcessERALog = ConfigurationManager.AppSettings["DownloadAndProcessERALog"];

        
    }
}