namespace Myezcare_Admin.Infrastructure
{
    public class Constants
    {
        #region Special Character
        public const char CommaChar = ',';
        public const string Comma = ",";
        public const char CaretChar = '^';
        public const string Caret = "^";
        public const string Slash = "/";
        public const string PipeChar = "|";
        public const string Underscore = "_";
        public const string Space = " ";
        public const string Hyphen = "-";
        public const char PipeCharacter = '|';
        public const string Colon = ":";
        #endregion

        public const  string MyezcareOrganizationConnectionString="MyezcareOrganization";

        public const string NotFound = "notfound";
        public const string DbDateFormat = "yyyy/MM/dd";
        public const string DbDateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public const string FileNameDateTimeFormat = "yyyyMMddHHmmss";
        public const string ReadableFileNameDateTimeFormat = "MM_dd_yyyy_HH_mm_ss";
        public const string DateFormatMMDD = "MM/dd";
        public const string DefaultDateFormat = "dd-MM-yyyy";

        public const string DataTypeString = "string";
        public const string DataTypeBoolean = "bool";

        public const string RegxPassword = @"^(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!@#$%^&*()_+}{"":;\'?\/>.<,]).{8,20}$";
        public const string RegxEmail = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
        public const string RegxPhone = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        public const string RegxEIN = @"^\(?([0-9]{9})$";
        public const string RegxAHCCCSID = @"^\(?([0-9]{6})$";
        public const string RegxNPI = @"^\(?([0-9]{10})$";
        public const string RegxVisitTime = @"^\(?((\d*\.)?\d{1,2})$";
        public const string RegxClientAhcccsId = @"^[a-zA-Z]{1}[0-9]{1,9}$";
        public const string RegxNumericSixDigit = @"^[0-9]{1,6}$";
        public const string RegxTimeFormat = @"^(?:0?[0-9]|1[0-2]):[0-5][0-9] [ap]m$";
        public const string RegxAllow2DecimalPlacesOnly = @"^(\d{1,9}|\d{0,9}\.\d{1,2})$";

        public static string All = "All";
        public static string AllRecords = "All Records";
        public static string Yes = "Yes";
        public static string CapsYes = "YES";
        public static string CapsNo = "NO";
        public static string No = "No";
        public static string NA = "NA";
        public static string Y = "Y";
        public static string N = "N";
        public static string NotApplicable = "Not Applicable";
        public static string Enabled = "Enabled";
        public static string Disabled = "Disabled";
        public static string Deleted = "Deleted";

        public static string Active = "Active";
        public static string InActive = "InActive";

        public static string EnableSelected = "Enable Selected";
        public static string DisableSelected = "Disable Selected";
        public static string InverseSelected = "Inverse - Enable/Disable";
        public const int AllRecordsConstant = -1;
        public const string RecordSavedSuccessfully = "Record saved successfully";
        public const string RecordCombinedAlreadyExists = "Sorry, record with same {0} combined already exists";
        public const string EncryptedQueryString = "EncryptedQueryString";
        public const string StatusCode401 = "401";



        public const string NotAuthorized = "NotAuthorized";
        public const string AnonymousPermission = "Anonymous";
        public const string RememberMePermission = "RememberMePermission";
        public const string AnonymousLoginPermission = "AnonymousLoginPermission";

        public static string AccountLocked = "AccountLocked";


        #region ErrorCode
        public static string ErrorCode_AccessDenied = "403";
        public static string ErrorCode_InternalError = "500";
        public static string ErrorCode_NotFound = "404";
        public static string ErrorCode_Warning = "204";
        #endregion ErrorCode

        #region FileExtension
        public static string FileExtension_Csv = ".csv";
        public static string FileExtension_Xlsx = ".xlsx";
        public static string FileExtension_Pdf = ".pdf";
        public static string FileExtension_Hippa = ".hippa";
        public static string FileExtension_Clm = ".clm";
        public static string FileExtension_Txt = ".txt";
        public const string ImageJpg = ".jpg";

        #endregion FileExtension

        #region Import 

        public static string AdminTempPatientTable = "AdminTempPatient";
        public static string AdminTempPatientColumns = "PatientID,FirstName,LastName,Dob,Gender,AccountNumber,LanguagePreference";
        public static string AdminTempPatientContactTable = "AdminTempPatientContact";
        public static string AdminTempPatientContactColumns = "PatientID,ContactType,FirstName,LastName,Email,Phone,Address,City,State,ZipCode,LanguagePreference,IsEmergencyContact";
        public static string AdminTempEmployeeTable = "AdminTempEmployee";
        public static string AdminTempEmployeeColumns = "EmployeeID,FirstName,LastName,Email,Address,City,State,ZipCode,Username,Role";

        #endregion

        #region Site Url

        public const string SecurityQAURL = "/security/securityquestion";
        public const string AccessDeniedURL = "/security/accessdenied";
        public const string LogoutURL = "/security/logout";
        public const string LoginURL = "/security/index";
        public const string DashboardURL = "/home/dashboard";

        public const string ReleaseNoteAddURL = "/releasenote/addreleasenote";
        public const string ReleaseNoteListURL = "/releasenote/releasenotelist";

        public const string OrganizationAddURL = "/organization/addorganization";
        public const string OrganizationListURL = "/organization/organizationlist";

        public const string ServicePlanAddURL = "/serviceplan/addserviceplan";
        public const string ServicePlanListURL = "/serviceplan/serviceplanlist";

        public const string EsignViewUrl = "/organization/esignview/";

        public const string FormListURL = "/form/formlist";
        public const string PermissionsListURL = "/Permission/PermissionList";
        public const string AddPermissionsURL = "/Permission/AddPermission";
        public const string OrgPermissionURL = "/Security/RolePermission";

        #region Invoice
        public const string AddInvoiceURL = "/Invoice/Add";
        public const string InvoiceListURL = "/Invoice/InvoiceList";

        #endregion
        #endregion

        #region Service Plan

        public const string Permission_Administrative_Permission = "1001";

        #endregion Service Plan

        public static string SecureText = "SECURE";

    }
}

