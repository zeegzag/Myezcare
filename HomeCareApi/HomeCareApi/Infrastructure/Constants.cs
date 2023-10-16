namespace HomeCareApi.Infrastructure
{
    public class Constants
    {
        public const string MyezcareOrganizationConnectionString = "MyezcareOrganization";
        public const string OrbeonConnectionString = "OrbeonConnectionString";

        public const int VisitsComplianceID = -2;

        public const string GetMethod = "get";
        public const string Post = "Post";
        public const string RequestModelName = "request";
        public const string KeyParam = "Key";
        public const string TokenParam = "Token";
        public const string CompanyNameParam = "CompanyName";
        public const string DataParam = "Data";
        public const string CacheKeyNameForValidKeys = "ValidKeys";
        public const char CommaChar = ',';
        public const string Comma = ",";
        public const int AllRecordsConstant = -1;
        public const char StarChar = '*';
        public const char ExponentChar = '^';
        public const string LeftArrowChar = "|=>|";
        public const string RightArrowChar = "|<=|";
        public const string DataTypeString = "string";
        public const string DataTypeBoolean = "bool";

        public const string Zero = "0";
        public const string One = "1";
        public static string ShowCaptch = "ShowCaptch";
        public static string AccountLocked = "AccountLocked";
        public static string Yes = "Yes";
        public static string No = "No";


        public const string DbDateFormat = "yyyy-MM-dd";
        public const string DbDateTimeFormat = "yyyy-MM-dd hh:mm:ss tt";
        public const string DisplayOnlyDateFormat = "dd-mm-yyyy";
        public const string PostItemTypeParam = "PostItemType";
        public const string PostItemIdParam = "PostItemId";
        public const string UserIdParam = "UserIdParam";
        public const string ToUserIdParam = "ToUserIdParam";
        public const string RandomNoParam = "RandomNoParam";
        public const string IsEditParam = "IsEdit";

        public const string DateFormatForSaveFile = "yyyyMMddHHmmssffff";
        public const string UploadUserProfileImage = "UserProfileImage";
        public const string ImageFormatJPG = ".jpg";

        public const string Slash = "/";
        public const char SlashChar = '/';

        public const string MobileNoRegx = @"[0-9]{10,10}";
        public const string RegexEmail = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
        public const string OTPRegx = @"[0-9]{6,6}";

        public const string PipeChar = "|";
        public const string EmailExceptionLog = "Email_Exception_";
        public const string DateFormatForLog = "MMddyyyy";
        public const string DateFormatForSearch = "dd-mmm-yyyy";

        public const string Approve = "Approve";
        public const string Deny = "Deny";
        public const string EmployeeVisitID = "EmployeeVisitID";
        public const string EmployeeSignatureID = "EmployeeSignatureID";
        public const string ScheduleID = "ScheduleID";
        public const string EmployeeID = "EmployeeID";
        public const string ReferralID = "ReferralID";
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";
        public const string UserName = "UserName";
        public const string MobileNumber = "MobileNumber";
        public const string IVRPin = "IVRPin";
        public const string Password = "Password";
        public const string EasternStandardTime = "Eastern Standard Time";
        public const string IsFingerPrintAuth = "IsFingerPrintAuth";


        public const string TxtFile = ".txt";
        public const string Underscore = "_";

        public const string DbTimeFormat = "hh:mm:ss tt";
        public const string NotificationTime = "hh:mm tt";

        public const string Space = " ";
        public const char SpaceChar = ' ';
        public const string Hyphen = "-";
        public const char PipeCharacter = '|';
        public const string Jpg = ".Jpg";
        public const string NotLoggedSmsNotification = "Not Logged SMS Notification";
        public const string Colon = ": ";
        public const string RoundBracketStart = "(";
        public const string RoundBracketEnd = ")";
        public const string Two = "2";
        public const string Three = "3";
        public const string BeforeClockIn = "-15";
        public const string ExtentionXls = ".xls";
        public const string http = "http://";
        public const string GeneratePCAPDFURL = "/hc/report/generatepcatimesheetpdf/";
        public const string UploadEmpSignatureURL = "/hc/employee/UploadEmpSignature";
        public const string UploadRefProfileImageURL = "/hc/referral/UploadRefProfileImage";
        public const string GenerateCertificateForEmployeeURL = "/hc/employee/GenerateCertificateForEmployee/";
        public const string LoadHtmlFormURL = "/hc/form/loadhtmlform/";
        public const string DeleteReferralDocumentViaAPIURL = "/hc/referral/DeleteReferralDocumentViaAPI/";
        public const string UploadDocumentViaAPIURL = "/hc/referral/UploadDocumentViaAPI";
        public const string Lat = "Lat";
        public const string Long = "Long";
        public const string IPAddress = "IPAddress";
        public const string ios = "ios";
        //public const string AutoApprovedIVRBypassPermission = "Mobile_AutoApproved_IVR_Bypass_ClockInOut";
        public const string AutoApprovedIVRBypassPermission = "Mobile_IVR_Bypass_ClockInOut";
        public const string ApprovalRequiredIVRBypassPermission = "Mobile_ApprovalRequired_IVR_Bypass_ClockInOut";
        public const string IVRInstantNoSchClockInOut = "Mobile_IVR_InstantNoSchedule_ClockInOut";
        
        public const string Delete = "DELETE";
        public const string Archieve = "ARCHIEVE";

        public const string ClockIn = "ClockIn";
        public const string ClockOut = "ClockOut";

        public const string Internal = "Internal";
        public const string External = "External";

        public const string EnglishAudioFilePath = "/assets/IvrAudioFiles/English/";
        public const string SpanishAudioFilePath = "/assets/IvrAudioFiles/Spanish/";
        public const string FilipinoAudioFilePath = "/assets/IvrAudioFiles/Filipino/";

        public static string Directory = "Directory";
        public static string SubDirectory = "SubDirectory";
    }
}