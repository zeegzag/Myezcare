using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure
{
    public class Constants
    {


        public const string Enroll_ERA = "era";
        public const string Enroll_CMS1500 = "1500";


        public const string MyezcareOrganizationConnectionString = "MyezcareOrganization";
        public const string ZarephathConnectionString = "ZarePhath";
        public const string OrbeonConnectionString = "OrbeonConnectionString";

        public const int PatientAuthorizationsComplianceID = -1;
        public const int VisitsComplianceID = -2;

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

        public const string HC_Permission_EmailTEmplate = "EmailTEmplate";
        public const string Permission_AddEmailTemplate = "AddEmailTemplate";
        public const string Permission_EmailTemplateList = "EmailTemplateList";
        public const string Permission_EditEmailTemplate = "EditEmailTemplate";
        public const string Permission_HideEmailTemplate = "HideEmailTemplate";


        public const string NotFound = "notfound";
        public const string ImageFormatJPG = ".jpg";
        public const string ImageFormatPNG = ".png";

        public const string DXCodeType_ICD10_Primary = "ABK";
        public const string DXCodeType_ICD10_Secondary = "ABF";
        public const string DXCodeType_ICD09_Primary = "BK";
        public const string DXCodeType_ICD09_Secondary = "BF";
        public const string DXCodeGroup_ICD10 = "ICD-10";
        public const string DXCodeGroup_ICD09 = "ICD-9";

        public const string DataTypeString = "string";
        public const string DataTypeBoolean = "bool";
        public const string RecordCombinedAlreadyExists = "Sorry, record with same {0} combined already exists";
        public const string RecordSavedSuccessfully = "Record saved successfully";
        public const string ImageJpg = ".jpg";
        public const int AllRecordsConstant = -1;
        public const string DbDateFormat = "yyyy/MM/dd";
        public const string DbDateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public const string DbTimeFormat = "hh:mm tt";
        public const string FileNameDateTimeFormat = "yyyyMMddHHmmss";
        public const string ReadableFileNameDateTimeFormat = "MM_dd_yyyy_HH_mm_ss";

        public const string GlobalDateFormat = "MM/dd/yyyy";
        public const string GlobalDatePlaceholder = "mm/dd/yy";

        public const string DateFormatMMDD = "MM/dd";

        public const string DefaultDateFormat = "dd-MM-yyyy";

        public const string DefaultStringForRoundUpMinutes = "Round Up If >= Minute(s)";

        public const string StatusCode401 = "401";

        //public const string RegxSSN = @"^\d{3}-\d{2}-\d{4}$";
        public const string RegxIDCard = @"^[a-zA-Z0-9]{13}$";




        #region HomeCareUrl

        public const string HC_ScheduleDayCareAttendence = "/hc/schedule/ScheduleDayCareAttendence";
        public const string HC_Process270_271 = "/hc/batch/process270_271";
        public const string HC_AddFacilityHouseURL = "/hc/facilityhouse/addfacilityhouse/";
        public const string HC_FacilityHouseListURL = "/hc/facilityhouse/facilityhouselist/";

        public const string HC_AddReferral = "/hc/referral/addreferral/";
        public const string ST_AddReferral = "/st/facility/addreferral/";
        public const string HC_ListReferral = "/hc/referral/referrallist/";
        public const string AddVisitTaskURL = "/hc/visittask/addvisittask/";
        public const string VisitTaskListURL = "/hc/visittask/visittasklist";
        public const string AddPreferenceURL = "/hc/preference/addpreference/";
        public const string PreferenceListURL = "/hc/preference/preferencelist";
        public const string HC_EmployeeListURL = "/hc/employee/employeelist";
        public const string HCAddEmployeeURL = "/hc/employee/addemployee";
        public const string HCDashboardURL = "/hc/home/dashboard";
        public const string HC_NotificationURL = "/hc/home/Notification";
        public const string HCEmployeeTimeSlotsURL = "/hc/employee/employeetimeslots";
        public const string HC_ScheduleAssignmentInHomeURL = "/hc/schedule/scheduleassignmentinhome";
        public const string HC_ScheduleAssignmentInHome01URL = "/hc/schedule/scheduleassignmentinhome01";
        public const string HC_ScheduleAssignmentDayCareURL = "/hc/schedule/scheduleassignmentdaycare";
        public const string HC_ScheduleAssignmentPrivateDutyURL = "/hc/schedule/scheduleassignmentprivateduty";
        public const string HC_ScheduleAssignmentCaseManagementURL = "/hc/schedule/scheduleassignmentcasemanagement";
        public const string HC_ScheduleMasterURL = "/hc/schedule/schedulemaster";
        public const string HC_DayCare_ScheduleMasterURL = "/hc/schedule/schedulemasterdaycare";
        public const string HC_ScheduleAggregatorLogsURL = "/hc/schedule/scheduleaggregatorlogs";
        public const string HC_PendingSchedulesURL = "/hc/schedule/pendingschedules";
        public const string HC_VirtualVisitsURL = "/hc/schedule/virtualvisits";
        public const string HC_EmployeeVisitsURL = "/hc/schedule/employeevisits";

        public const string HC_GroupSMSURL = "/hc/employee/sendbulksms/";
        public const string HC_ReceviedImURL = "/hc/employee/receivedmessages/";
        public const string HC_SentImURL = "/hc/employee/sentmessages/";
        public const string HC_BroadcastNotificationURL = "/hc/employee/broadcastnotification/";


        public const string HC_EmployeeCalenderURL = "/hc/employee/employeecalender";
        public const string HC_EmployeeTimeSlotsURL = "/hc/employee/employeetimeslots";
        public const string HC_EmployeeDayOffURL = "/hc/employee/employeedayoff";
        public const string HC_ReferralCalenderURL = "/hc/referral/referralcalender";
        public const string HC_NurseSchedulerURL = "/hc/schedule/nursescheduler";
        public const string HC_ReferralTimeSlotsURL = "/hc/referral/referraltimeslots";
        public const string HC_ReferralCareTypeTimeSlotsURL = "/hc/referral/referralcaretypetimeslots";

        public const string HC_AddServiceCodeURL = "/hc/servicecode/addservicecode/";
        public const string HC_GetServiceCodeList = "/hc/servicecode/servicecodelist/";

        public const string HC_AddEmployee = "/hc/employee/addemployee/";
        public const string HC_GenerateEmployeeIDCardPdf = "/hc/employee/GenerateEmployeeIDCard/";
        public const string HC_EmployeeIDCard = "/hc/employee/EmployeeIDCard/";
        public const string HC_GeneratePcaTimeSheet = "/hc/report/generatepcatimesheet/";
        public const string HC_GeneratePcaTimeSheetDaycare = "/hc/report/generatepcatimesheetdaycare/";
        public const string HC_GeneratePcaTimeSheetPdf = "/hc/report/generatepcatimesheetpdf/";

        public const string GeneratePatientActivePdf = "/hc/report/GeneratePatientActivePdf";
        public const string GenerateForm90Pdf = "/hc/report/GenerateDMAS_90FormsPdf/";
        public const string GenerateWeeklyTimeSheetPdf = "/hc/report/GenerateWeeklyTimeSheetPdf/";
        public const string HC_GeneratePatientActivePdfURL = "/hc/report/GeneratePatientActivePdfurl";

        public const string HC_GeneratePatientTimeSheetPdfURL = "/hc/patienttimesheet/generatepatienttimesheetpdf/";

        public const string HC_AgencyListPageURL = "/hc/agency/agencylist";
        public const string HC_AgencyAddPageURL = "/hc/agency/addagency";

        public const string HC_ReportEmployeeVisitListURL = "/hc/report/employeevisitlist";
        public const string HC_ReportNurseSignatureURL = "/hc/report/nursesignature";
        public const string HC_ReportGroupTimesheetListURL = "/hc/report/grouptimesheet";
        public const string HC_ReportReferralActivityURL = "/hc/report/ReferralActivity";
        public const string HC_EmployeeBillingReportURL = "/hc/report/employeebillingreport";
        public const string HC_ReportURL = "/hc/report/reportmaster";
        public const string HC_SetPasswordUrl = "/k/";//"/security/setpassword/";

        public const string HC_GetPayorList = "/hc/payor/payorlist/";
        public const string HC_AddPayorURL = "/hc/payor/addpayor/";


        public const string HC_OrganizationForms = "/hc/form/organizationforms";

        public const string HC_GeneralMasterDetail = "/hc/generalmaster/generalmasterdetail";
        public const string HC_ComplianceDetail = "/hc/compliance/compliancedetail";

        public const string HC_AddDXCodeURL = "/hc/dxcode/adddxcode/";
        public const string HC_DXCodeListURL = "/hc/dxcode/dxcodelist";

        public const string HC_PhysicianListURL = "/hc/physician/physicianlist";
        public const string HC_AddPhysicianURL = "/hc/physician/addphysician";
        public const string PhysicianListURL = "/hc/physician/physicianlist";

        public const string HC_CategoryListURL = "/hc/category/categorylist";
        public const string HC_AddCategoryURL = "/hc/category/addcategory";
        public const string CategryListURL = "/hc/category/categorylist";

        public const string HC_EBMarketsListURL = "/hc/ebmarkets/ebmarketslist";
        public const string HC_AddEBMarketsURL = "/hc/ebmarkets/addebmarkets";
        public const string EBMarketsListURL = "/hc/ebmarkets/ebmarketslist";

        public const string HC_EBFormsListURL = "/hc/ebforms/ebformslist";
        public const string HC_AddEBFormsURL = "/hc/ebforms/addebforms";
        public const string EBFormsListURL = "/hc/ebforms/ebformslist";


        public const string HC_ARAgingReport = "/hc/batch/aragingreport";
        public const string HC_BatchMaster = "/hc/batch/batchmaster";
        public const string HC_Upload835 = "/hc/batch/upload835";
        public const string HC_LatestERA = "/hc/batch/latestERA";
        public const string HC_Reconcile835 = "/hc/batch/reconcile835";
        public const string HC_EdiFileLogs = "/hc/batch/edifileloglist";


        public const string HC_GeneralMasterURL = "/hc/generalmaster/generalmasterdetail/";

        public const string HC_PatientTimeSheetURL = "/hc/patienttimesheet/patienttimesheet";
        public const string HC_InvoiceDetailURL = "/hc/invoice/invoicedetail/";
        public const string HC_InvoiceListURL = "/hc/invoice/invoicelist";

        public const string HC_GenerateInvoicePdf = "/hc/invoice/generateinvoicepdf/";
        public const string HC_GenerateInvoice = "/hc/invoice/generateinvoice/";
        public const string HC_JoinMeeting = "/#/upcoming-visit-details/";
        public const string HC_JoinMeeting_Patient = "/#/patient-virtual-visit/";

        public const string HC_GenerateMIF = "/hc/referral/GenerateMIF/";

        public const string HC_GenerateCareForm = "/hc/referral/generatecareform/";
        public const string HC_GenerateCareFormHeader = "/hc/referral/careformheader/";

        public const string HC_GenerateMIFPdfURL = "/hc/referral/GenerateMIFPdf/";

        //Link for Total Patient URL
        public const string HC_TotalPatientURL = "/hc/report/TotalPatientReport/";

        //Link for DMAD FORMS URL
        public const string HC_DMAS_90URL = "/hc/report/DMAS_90Form/";
        public const string HC_WeeklyTimeSheetURL = "/hc/report/WeeklyTimeSheet/";
        public const string GenerateDMAS_90FormsPdfURL = "/hc/report/GenerateDMAS_90FormsPdf/";
        public const string HC_DMAS_97_ABURL = "/hc/report/DMAS_97_ABForms/";
        public const string HC_DMAS_99URL = "/hc/report/DMAS_99Forms/";
        public const string HC_DMAS_98URL = "/hc/report/DMAS_98Forms/";

        public const string HC_OnBoarding_GetStarted = "/hc/onboarding/getstarted";

        public const string HC_ManageClaims = "/hc/batch/ManageClaims";
        public const string HC_Organization_Preference = "/hc/orgpreference/preference";

        #endregion

        public const string HC_EmployeeAttendanceClockinoutURL = "/hc/attendance/clockinout";
        public const string HC_EmployeeAttendanceCalenderURL = "/hc/attendance/calendar";

        #region TransportService
        public const string HC_Permission_TransportContact_AddUpdate = "Masters_TransportContact_AddUpdate";
        public const string HC_Permission_TransportContact_List = "Masters_TransportContact_List";
        public const string HC_Permission_TransportContact_Delete = "Masters_TransportContact_Delete";

        public const string HC_Permission_TransportVehicle_AddUpdate = "Masters_TransportVehicle_AddUpdate";
        public const string HC_Permission_TransportVehicle_List = "Masters_TransportVehicle_List";
        public const string HC_Permission_TransportVehicle_Delete = "Masters_TransportVehicle_Delete";

        public const string HC_Permission_TransportService_AddUpdate = "Masters_TransportService_AddUpdate";
        public const string HC_Permission_TransportService_Delete = "Masters_TransportService_Delete";
        public const string HC_Permission_TransportService_List = "Masters_TransportService_List";

        public const string HC_Permission_TransportGroupService_AddUpdate = "Masters_TransportGroupService_AddUpdate";
        public const string HC_Permission_TransportGroupService_Delete = "Masters_TransportGroupService_Delete";
        public const string HC_Permission_TransportGroupService_List = "Masters_TransportGroupService_List";

        public const string HC_TransportContactAddPageURL = "/hc/transportservice/addtransportcontact";
        public const string HC_TransportContactListPageURL = "/hc/transportservice/transportcontactlist";
        public const string HC_AssignTransportGroupAddPageURL = "/hc/transportservice/assigntransportgroup";

        #endregion TransportService

        #region Vehicle
        public const string HC_Permission_Vehicle_AddUpdate = "Masters_Vehicle_AddUpdate";
        public const string HC_Permission_Vehicle_Delete = "Masters_Vehicle_Delete";
        public const string HC_Permission_Vehicle_List = "Masters_Vehicle_List";

        public const string HC_VehicleAddPageURL = "/hc/transportservice/addvehicle";
        public const string HC_VehicleListPageURL = "/hc/transportservice/vehiclelist";
        public const string HC_TransportAssignmentURL = "/hc/transportservice/transportassignment";

        #endregion Vehicle


        #region Setting
        public const string Organization_SettingPageUrl = "/hc/setting/organizationsetting";
        #endregion Setting















        public const string AnonymousLoginPermission = "AnonymousLoginPermission";

        public const string EncryptedQueryString = "EncryptedQueryString";
        public const string LoginURL = "/security/index";
        public const string MarketingUrl = "/security/marketing";
        public const string PaymentNowUrl = "/hc/Payment/PaymentNow";

        public const string DashboardURL = "/hc/home/dashboard";
        public const string SecurityQAURL = "/security/securityquestion";
        public const string ForgotPasswordURL = "/security/forgotpassword";
        public const string UnlockAccountURL = "/security/unlockaccount";
        public const string AccessDeniedURL = "/security/accessdenied";
        public const string NotFoundURL = "/security/notfound";
        public const string DomainNotFoundURL = "/general/domainnotfound";


        public const string AccountHistoryURL = "/hc/invoice/CompanyClientInvoice";
        public const string LogoutURL = "/security/logout";
        public const string AddDepartmentURL = "/department/adddepartment";
        public const string DepartmentListURL = "/department/departmentlist";
        public const string AddEmployeeURL = "/employee/addemployee";
        public const string EmployeeListURL = "/employee/employeelist";
        public const string AccountVerificationURL = "/security/accountverification";
        public const string EditProfileURL = "/security/editprofile";
        public const string RolePermissionURL = "/security/rolepermission";
        public const string HC_NotificationConfigurationURL = "/hc/notificationconfiguration";
        public const string HCRolePermissionURL = "/hc/security/rolepermission";
        public const string CaseManagerListURL = "/casemanager/casemanagerlist";
        public const string ParentListURL = "/parent/parentlist";
        public const string UIKITURL = "/hc/UIKit/UIKit";

        public const string AddCaseManagerURL = "/casemanager/addcasemanager/";


        public const string AddParentURL = "/parent/addparent/";

        public const string ReferralTrackingURL = "/referral/referraltracking";
        public const string ReferralListURL = "/referral/referrallist";
        public const string AddReferralURL = "/referral/addreferral/";
        public const string AddFacilityHouseURL = "/facilityhouse/addfacilityhouse";
        public const string FacilityHouseListURL = "/facilityhouse/facilityhouselist";
        public const string ScheduleMasterURL = "/schedule/schedulemaster/";
        public const string ScheduleAssignmentURL = "/schedule/scheduleassignment";
        public const string ScheduleAssignmentInHomeURL = "/schedule/scheduleassignmentinhome";

        public const string AddTransPortationModelURL = "/transportlocation/addtransportlocation/";
        public const string TransPortationModelListURL = "/transportlocation/transportlocationlist/";
        public const string TransportationAssignment = "/transportationgroup/transportationassignment/";
        public const string AttendanceMaster = "/schedule/attendancemaster/";
        public const string AddPayorURL = "/payor/addpayor/";
        public const string GetPayorList = "/payor/payorlist/";
        public const string AddServiceCodeURL = "/servicecode/addservicecode/";
        public const string GetServiceCodeList = "/servicecode/servicecodelist/";
        public const string AddDXCodeURL = "/dxcode/adddxcode/";
        public const string DXCodeListURL = "/dxcode/dxcodelist";
        public const string AddNoteSentenceURL = "/notesentence/addnotesentence/";
        public const string NoteSentenceListURL = "/notesentence/notesentencelist";



        public const string RememberMePermission = "RememberMePermission";
        public const string NotAuthorized = "NotAuthorized";
        public const string AnonymousPermission = "Anonymous";
        public const string ReferralPermission = "Referral,Dashboard";
        public const string EmailTemplateListURL = "/emailtemplate/emailtemplatelist";
        public const string AddEmailTemplateURL = "/emailtemplate/addemailtemplate";
        public const string BatchMaster = "/batch/batchmaster";
        public const string Upload835 = "/batch/upload835";
        public const string Reconcile835 = "/batch/reconcile835";
        public const string note = "/note/";
        public const string ClientNote = "/note/clientnotes";
        public const string EdiFileLogs = "/batch/edifileloglist";
        public const string Process270_271 = "/batch/process270_271";

        public const string Process277CA = "/batch/process277";
        public const string ReleaseNoteUrl = "/hc/myezconnect/releasenote/";


        public const string AddGroupNote = "/note/addgroupnote";
        public const string ChangeServiceCode = "/note/changeservicecode";

        public const string AgencyListPageURL = "/agency/agencylist";
        public const string AgencyAddPageURL = "/agency/addagency";
        public const string AddGroupMonthlySummaryURL = "/referral/addgroupmonthlysummary";
        //Added By Sanjay Start
        public const string AddCaptureCallUrl = "/hc/capturecall/AddCaptureCall/";
        public const string CaptureCallListUrl = "/hc/capturecall/capturecalllist/";
        //Added By Sanjay End


        // public const string ReportURL = "/report";

        public const string ReportLsTeamMemberCaseLoadURL = "/report/lsteammembercaseload";
        public const string MonthalySummaryList = "/referral/monthlysummarylist";

        public const string BackendProcess277File = "/batch/backendprocess277file";
        public const string BackendProcess835File = "/batch/backendprocessupload835file";



        public const string HC_BackendProcess835File = "/hc/batch/backendprocessupload835file";



        public const string HC_Get_Download_Process_AllERA_URL = "/hc/cronjob/get_download_process_allera";
        public const string HC_CronJob_Download_Process_AllERA_URL = "/hc/cronjob/download_process_allera";
        public const string HC_CronJob_Download_Process_AllERA_ProcessUpdate_URL = "/hc/cronjob/download_process_allera_processupdate";

        public const string HC_CronJob_Download_Process_AllERA_ProcessName = "download_process_allera";


        public const string StateCode_NewJersey = "NJ";

        public const string HC_VirtualVistTermsAndConditions = "/hc/setting/termsandconditions";

        /// <summary>
        /// For now, set default state and statecode for department
        /// </summary>
        public const string DefaultStateName = "Arizona";
        public const string DefaultStateCode = "AZ";

        /// <summary>
        /// RoleID set Default Admin 
        /// </summary>
        public const int DefaultAdminRoleId = 1;

        public const string RegxPassword = @"^(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[!@#$%^&*()_+}{"":;\'?\/>.<,]).{8,20}$";
        public const string RegxEmail = @"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}";
        public const string RegxPhone = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        public const string RegxEIN = @"^\(?([0-9]{9})$";
        public const string RegxAHCCCSID = @"^\(?([0-9]{6})$";
        public const string RegxNPI = @"^\(?([0-9]{10})$";
        public const string RegxVisitTime = @"^\(?((\d*\.)?\d{1,2})$";

        public const string RegxClientAhcccsId = @"^[a-zA-Z]{1}[0-9]{1,9}$";

        public const string RegxNumericSixDigit = @"^[0-9]{1,6}$";
        public const string RegxNumericTenDigit = @"^[0-9]{1,10}$";
        public const string RegxAlphabetsOnly = @"^(([A-za-a]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$";
        //public const string RegxTimeFormat = @"^(hh:mm tt)$";
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

        public static string Accepted = "Accepted";
        public static string NotAccepted = "Not Accepted";

        public static string Expirein90Days = "Expire in 90 Days";


        public static string Internal = "Internal";
        public static string External = "External";

        public static string Section = "Folder";
        public static string Subsection = "Subfolder";

        public static string Directory = "Directory";
        public static string SubDirectory = "SubDirectory";

        public static string Male = "Male";
        public static string Female = "Female";

        public static string Authorized = "Authorized";
        public static string NotAuthorize = "Not Authorized";

        public static string Child = "Child";
        public static string RomanNumeral = "XIX";
        public static string Verbal = "Verbal";
        public static string Written = "Written";
        public static int PrimaryPlacement = 1;
        public static int LegalGuardian = 2;
        public static int Relation = 4;
        public static string True = "true";
        public static string LegalGuardianType = "Legal Guardian";
        public static string Guardian = "Guardian";
        public static string Parent = "Parent";
        public static string Placement = "Placement";
        public static string Case_Manager = "Case Manager";
        public static string Staff = "Staff";
        // public static string Office = "Office";
        public static string AddAction = "Add";
        public static string InternalShort = "I";
        public static string ExternalShort = "E";
        public static string DefaultDateValue = "01-01-0001";

        public static string AssignedMe = "Assigned Me";

        public static string Primary = "Primary";
        public static string Secondary = "Secondary";
        public static string Tertiary = "Tertiary";

        public const string RegxMultipleEmail = @"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;]{0,1}\s*)+$";

        #region Email Template Constant

        public const string SendEmployeeAccountActivationEmail = "SendEmployeeAccountActivationEmail";
        public const string SendEmployeeEmailChangedVerificationMail = "SendEmployeeEmailChangedVerificationMail";
        public const string ReSendEmployeeVerificationMail = "ReSendEmployeeVerificationMail";
        public const string MissingDocumentMail = "MissingDocumentMail";
        public const string ReceiptNotificationMail = "ReceiptNotificationMail";

        public const string WeeklyNotificationEmail = "WeeklyNotificationEmail";
        public const string MonthlyNotificationEmail = "MonthlyNotificationEmail";

        #endregion

        #region ErrorCode
        public static string ErrorCode_AccessDenied = "403";
        public static string ErrorCode_InternalError = "500";
        public static string ErrorCode_NotFound = "404";
        public static string ErrorCode_Warning = "204";

        #endregion ErrorCode

        #region MissingDocuments

        public static string AgencyROI = "Agency ROI";
        public static string NetworkServicePlan = "Agency/Network Service Plan";
        public static string BXAssessment = "BX Assessment";
        public static string Demographic = "Demographic";
        public static string SNCD = "SNCD";
        public static string NetworkCrisisPlan = "Network Crisis Plan";
        public static string CAZOnly = "CAZ Only";

        public static string CareConsent = "Care Consent";
        public static string SelfAdministrationofMedication = "Self Administration of Medication";
        public static string HealthInformationDisclosure = "Health Information Disclosure";
        public static string AdmissionRequirements = "Admission Requirements";
        public static string AdmissionOrientation = "Admission Orientation";
        public static string ZarephathCrisisPlan = "Zarephath Crisis Plan";
        public static string PHI = "ROI TO Agency";
        public static string ZarephathServicePlan = "Zarephath Service Plan";



        public static string Missing = "Missing";
        public static string Expired = "Expired";

        #endregion MissingDocuments

        #region Referral

        public const string Respite = "Respite";
        public const string Life_Skills = "Life Skills";
        public const string Counselling = "Counselling";
        public const string ConnectingFamiliesService = "Connecting Families";

        #endregion

        public const string Billable = "Billable";
        public const string Non_Billable = "Non-Billable";

        public const string Complete = "Completed";
        public const string Incomplete = "Incomplete";
        public const string Sub = "Sub";

        public const string ios = "ios";
        public const string android = "android";


        public static int MaxReferalAge = 18;
        public static int MaxHCReferalAge = 100;

        public static string BeforeClockIn = "-15";
        public static string BeforeClockInWeb = "-120";

        public static string Gender_Male = "M";
        public static string Gender_FeMale = "F";

        #region Email Service

        public static int WeekValue = 7;
        public static int MonthValue = 30;
        public static int MonthValuenew = 0;

        public static string ConfirmationUrl = "/schedule/scheduleemailconfiramation/";
        public static string CancellationUrl = "/schedule/scheduleemailcancellation/";
        public static string ZerpathLogoImage = "/Assets/images/logo-zrpath.png";
        public static string AsapCareLogoImage = "/Assets/images/asapcare.png";
        public static string NoMapImageUrl = "/Assets/images/no-image-available.jpg";
        public const string DefaultDateFormatEmailService = "MM'/'dd'/'yyyy";
        public static string FaceBookLogoImage = "/Assets/images/FacebooklogoImage.png";
        public static string ConfirmSMSurl = "/m/";

        public static string AHCCCSLogoImage = "/Assets/images/AHCCCS-logo-100-transparent.gif";

        #endregion

        #region Role and Permissions

        //public const string Permission_CaseManager_AddUpdate = "7";
        //public const string Permission_CaseManager_List = "12";
        public const string Permission_Department_AddUpdate = "8";
        public const string Permission_Department_Delete = "8";
        public const string Permission_Department_List = "13";
        public const string Permission_Employee_AddUpdate = "9";
        public const string Permission_Employee_Delete = "9";
        public const string Permission_Employee_List = "14";
        public const string Permission_Facility_House_AddUpdate = "10";
        public const string Permission_Facility_House_Delete = "10";
        public const string Permission_Facility_House_List = "15";
        public const string Permission_Transportation_AddUpdate = "11";
        public const string Permission_Transportation_Delete = "11";
        public const string Permission_Transportation_List = "16";
        public const string Permission_Referral_Details = "21";
        public const string Permission_Referral_Documents = "22";
        public const string Permission_Referral_Checklist = "23";
        public const string Permission_Referral_Spar_Form = "24";
        public const string Permission_Internal_Messaging = "25";
        public const string Permission_Schedule_Hisotry = "26";
        public const string Permission_Dashboard = "20";
        public const string Permission_View_All_Referral = "33";
        public const string Permission_View_Assinged_Referral = "34";
        public const string Permission_Schedule_List = "31";
        public const string Permission_Schedule_Update = "32";
        public const string Permission_Schedule_Transportation_Groups = "30";
        public const string Permission_Schedule_Assignment_Permission = "29";
        public const string Permission_Payor_AddUpdate = "36";
        public const string Permission_Payor_Delete = "36";
        public const string Permission_Payor_List = "37";
        public const string Permission_Payor_ServiceCode_Mapping = "38";
        public const string Permission_Attendance_Permission = "39";
        //public const string Permission_RoleAndPermission = "1001";
        public const string Permission_DxCode_List = "42";
        public const string Permission_DxCode_AddUpdate = "41";
        public const string Permission_EmailTemplate_AddUpdate = "44";
        public const string Permission_EmailTemplate_List = "45";
        public const string Permission_EmailTemplate_Delete = "44";
        public const string Permission_Claim_Processing = "46";
        public const string Permission_Billing_Notes = "47";
        public const string Permission_Reports = "48";
        public const string Permission_Agency_AddUpdate = "50";
        public const string Permission_Agency_Listing = "51";
        public const string Permission_Referral_Tracking = "52";
        public const string Permission_Report_Client_Status = "53";
        public const string Permission_Report_Referral_Detail = "54";
        public const string Permission_Report_Client_Information = "55";
        public const string Permission_Report_Internal_Service_Plan_Expiration_Dates = "56";
        public const string Permission_Report_Respite_Usage = "57";
        public const string Permission_Report_Attendance = "58";
        public const string Permission_Report_Behaviour_Contract = "59";
        public const string Permission_Report_Encounter_Print = "60";
        public const string Permission_Report_DSP_Roster = "61";
        public const string Permission_Report_Schedule_Attendance = "62";
        public const string Permission_Report_LSOutcomeMeasurementsReport = "63";
        public const string Permission_Report_LSTeamMemberCaseloads = "64";
        public const string Permission_Report_GeneralNotice = "65";
        public const string Permission_Referral_ReviewMeasurement = "66";
        public const string Permission_Report_Snapshot_Print = "67";
        public const string Permission_Report_DTR_Print = "68";
        public const string Permission_Report_Required_Documents_For_Attendance = "69";
        public const string Permission_Report_LSTeamMemberCaseloads_View_All = "70";
        public const string Permission_Report_LSTeamMemberCaseloads_View_Assigned = "71";
        public const string Permission_Report_RequestClientListReport = "124";


        //public const string Permission_NoteSentence_AddUpdate = "73";
        //public const string Permission_NoteSentence_List = "74";


        public const string Permission_ReferralDetails_ViewOnly = "75";
        public const string Permission_ReferralDetails_AddUpdate = "76";
        public const string Permission_ReferralDetails_View_AddUpdate = Permission_ReferralDetails_ViewOnly + "," + Permission_ReferralDetails_AddUpdate;

        public const string Permission_ReferralDocuments_ViewOnly = "77";
        public const string Permission_ReferralDocuments_AddUpdate = "78";
        public const string Permission_ReferralDocuments_View_AddUpdate = Permission_ReferralDocuments_ViewOnly + "," + Permission_ReferralDocuments_AddUpdate;

        public const string Permission_ReferralChecklist_ViewOnly = "79";
        public const string Permission_ReferralChecklist_AddUpdate = "80";
        public const string Permission_ReferralChecklist_View_AddUpdate = Permission_ReferralChecklist_ViewOnly + "," + Permission_ReferralChecklist_AddUpdate;

        public const string Permission_ReferralSparForm_ViewOnly = "81";
        public const string Permission_ReferralSparForm_AddUpdate = "82";
        public const string Permission_ReferralSparForm_View_AddUpdate = Permission_ReferralSparForm_ViewOnly + "," + Permission_ReferralSparForm_AddUpdate;

        public const string Permission_ReferralInternalMessaging_ViewOnly = "83";
        public const string Permission_ReferralInternalMessaging_AddUpdate = "84";
        public const string Permission_ReferralInternalMessaging_View_AddUpdate = Permission_ReferralInternalMessaging_ViewOnly + "," + Permission_ReferralInternalMessaging_AddUpdate;


        public const string Permission_ReviewMeasurement_OutcomesMeasurement_ViewOnly = "86";
        public const string Permission_ReviewMeasurement_OutcomesMeasurement_AddUpdate = "87";
        public const string Permission_ReviewMeasurement_OutcomesMeasurement_View_AddUpdate = Permission_ReviewMeasurement_OutcomesMeasurement_ViewOnly + "," + Permission_ReviewMeasurement_OutcomesMeasurement_AddUpdate;

        public const string Permission_ReviewMeasurement_AnsellCaseyReview_ViewOnly = "89";
        public const string Permission_ReviewMeasurement_AnsellCaseyReview_ViewAssigned = "90";
        public const string Permission_ReviewMeasurement_AnsellCaseyReview_AddUpdate = "91";
        public const string Permission_ReviewMeasurement_AnsellCaseyReview_View_AddUpdate = Permission_ReviewMeasurement_AnsellCaseyReview_ViewOnly + "," + Permission_ReviewMeasurement_AnsellCaseyReview_ViewAssigned + "," + Permission_ReviewMeasurement_AnsellCaseyReview_AddUpdate;

        public const string Permission_ReviewMeasurement_MonthlySummary_ViewOnly = "93";
        public const string Permission_ReviewMeasurement_MonthlySummary_AddUpdate = "94";
        public const string Permission_ReviewMeasurement_MonthlySummary_View_AddUpdate = Permission_ReviewMeasurement_MonthlySummary_ViewOnly + "," + Permission_ReviewMeasurement_MonthlySummary_AddUpdate;


        public const string Permission_ReviewMeasurement_All_View_AddUpdate = Permission_ReviewMeasurement_OutcomesMeasurement_View_AddUpdate + "," + Permission_ReviewMeasurement_AnsellCaseyReview_View_AddUpdate + "," + Permission_ReviewMeasurement_MonthlySummary_View_AddUpdate;


        public const string Permission_Snapshot_Add = "96";
        public const string Permission_Snapshot_Review = "97";

        //public const string Permission_Referral_ClientNotes = "98";

        public const string Permission_Dashboard_AssignedIMNotifications = "98";
        public const string Permission_Dashboard_ResolvedIMNotifications = "99";
        public const string Permission_Dashboard_MissingIncompleteCheckListSPAR = "100";
        public const string Permission_Dashboard_MissingExpiredDocuments = "101";
        public const string Permission_Dashboard_MissingInternalDocument = "102";


        public const string Permission_Billing_Batch837 = "103";
        public const string Permission_Billing_Upload835 = "104";
        public const string Permission_Billing_Reconcile835 = "105";
        public const string Permission_Billing_EDIFileLogs = "106";
        public const string Permission_Billing_EDI270_271 = "121";
        public const string Permission_Billing_EDI277CA = "122";

        public const string Permission_NoteList = "107";
        public const string Permission_NoteList_ViewAll = "112";
        public const string Permission_NoteList_ViewAssigned = "113";

        public const string Permission_NoteReview = "108";
        public const string Permission_NoteReview_ViewAll = "114";
        public const string Permission_NoteReview_ViewAssigned = "115";

        public const string Permission_GroupNote = "109";
        public const string Permission_ChangeServiceCode = "123";



        public const string Permission_Dashboard_AnsellCaseyReview = "110";
        public const string Permission_Dashboard_AssignedNotesReview = "116";

        public const string Permission_Additional_RolePermission = "118";
        public const string Permission_Additional_UploadSignature = "119";
        public const string Permission_Additional_ViewNotesAmount = "120";



        public const string Permission_ServiceCode_AddUpdate = "121";
        public const string Permission_ServiceCode_List = "122";
        public const string Permission_Additional_ReportPermission = "123";

        public const string Permission_Administrative_Permission = "1001";


        #endregion



        #region HomeCare Role and Permissions

        #region Scrap Code

        //public const string HC_Permission_Dashboard = "2001";
        //public const string HC_Permission_Dashboard_EmployeeClockIn = "2002";
        //public const string HC_Permission_Dashboard_EmployeeOverTime = "2003";
        //public const string HC_Permission_Dashboard_PatientNew = "2004";
        //public const string HC_Permission_Dashboard_PatientFullySchedule = "2005";
        //public const string HC_Permission_Dashboard_AssignInternalMessage = "2006";
        //public const string HC_Permission_Dashboard_ResolveAssignInternalMessage = "2007";
        //public const string HC_Permission_Dashboard_EmployeesAverageDelays = "2039";

        //public const string HC_Permission_Masters = "2008";
        //public const string HC_Permission_Employee = "2009";
        //public const string HC_Permission_PreferenceSkill = "2010";
        //public const string HC_Permission_ServiceCode = "2011";
        //public const string HC_Permission_VisitTask = "2012";
        //public const string HC_Permission_Employee_AddUpdate = "2013";
        //public const string HC_Permission_Employee_List = "2014";
        //public const string HC_Permission_Employee_Delete = "2041";
        //public const string HC_Permission_Employee_Calendar = "2015";
        //public const string HC_Permission_Employee_Schedule = "2016";
        //public const string HC_Permission_Employee_PTO = "2017";

        //public const string HC_Permission_PreferenceSkill_AddUpdate = "2018";
        //public const string HC_Permission_PreferenceSkill_List = "2019";
        //public const string HC_Permission_PreferenceSkill_Delete = "2042";

        //public const string HC_Permission_ServiceCode_AddUpdate = "2020";
        //public const string HC_Permission_ServiceCode_List = "2021";

        //public const string HC_Permission_VisitTask_AddUpdate = "2022";
        //public const string HC_Permission_VisitTask_List = "2023";
        //public const string HC_Permission_VisitTask_Delete = "2040";

        //public const string HC_Permission_Patient = "2024";
        //public const string HC_Permission_Patient_AddUpdate = "2025";
        //public const string HC_Permission_Patient_List = "2026";
        //public const string HC_Permission_Patient_Delete = "2043";
        //public const string HC_Permission_Patient_Calendar = "2027";
        //public const string HC_Permission_Patient_Schedule = "2028";

        //public const string HC_Permission_Schedule = "2029";
        //public const string HC_Permission_Schedule_ScheduleMaster = "2030";
        //public const string HC_Permission_Schedule_ScheduleLog = "2031";

        //public const string HC_Permission_Message = "2032";
        //public const string HC_Permission_Message_ReceivedMessage = "2033";
        //public const string HC_Permission_Message_SentMessage = "2034";
        //public const string HC_Permission_Message_GroupSMS = "2035";

        //public const string HC_Permission_Reports = "2036";

        //public const string HC_Permission_AdditionalPermission = "2037";
        //public const string HC_Permission_AdditionalPermission_RolePermissions = "2038";

        //public const string HC_Permission_AdministrativePermission = "2999";

        #endregion

        //SSN Permissions
        //SSN Permissions 
        public const string HC_Can_See_SSN = "Can_See_SSN";
        public const string HC_Can_Not_See_SSN = "Can_See_SSN";
        public const string HC_Can_See_Last_Four_Digit = "Can_See_Last_Four_Digit";

        //Employee Role
        public const string HC_Can_See_Role = "Can_See_Role";
        public const string HC_Can_Update_Role = "Can_Update_Role";

        //patient ssn 
        public const string HC_Can_See_SSN_Patient = "Can_See_SSN_Patient";
        public const string HC_Can_Not_See_SSN_Patient = "Can_See_SSN_Patient";
        public const string HC_Can_See_Last_Four_Digit_Patient = "Can_See_Last_Four_Digit_SSN_Patient";
        //end patient SSN

        public const string HC_Permission_Dashboard = "Dashboard";
        public const string HC_Permission_Dashboard_BroadcastNotifications = "Dashboard_BroadcastNotifications";
        public const string HC_Permission_Dashboard_EmployeeClockIn = "Dashboard_Emp_DidNot_ClockInOut";
        public const string HC_Permission_Dashboard_ViewMap = "ViewMap";
        public const string HC_Permission_Dashboard_EmployeeOverTime = "Dashboard_Emp_OverTime_In7Days";
        public const string HC_Permission_Dashboard_PatientNew = "Dashboard_Patient_New";
        public const string HC_Permission_Dashboard_PatientFullySchedule = "Dashboard_Patient_Fully_NotSch";
        public const string HC_Permission_Dashboard_AssignInternalMessage = "Dashboard_Received_InternalMessage";
        public const string HC_Permission_Dashboard_ResolveAssignInternalMessage = "Dashboard_Sent_InternalMessage";
        public const string HC_Permission_Dashboard_EmployeesAverageDelays = "Dashboard_Emp_Average_Delays";
        public const string HC_Permission_Dashboard_WebNotifications = "Dashboard_WebNotifications";

        public const string HC_Permission_Masters = "Masters";// "2008";
        public const string HC_Permission_Employee = "Masters_Employee";//"2009";
        public const string HC_Permission_PreferenceSkill = "Masters_PreferenceSkill";// "2010";
        public const string HC_Permission_ServiceCode = "Masters_ServiceCode"; //"2011";
        // public const string HC_Permission_VisitTask = "Masters_VisitTask";
        public const string HC_Permission_Employee_Group_AddUpdate = "Employee_EmployeeInfo_Group_AddUpdate"; //"2011";
        public const string HC_Permission_Patient_Group_AddUpdate = "PatientIntake_PatientDetails_Group_AddUpdate"; //"2011";

        public const string HC_Permission_PreferenceSkill_AddUpdate = "Masters_PreferenceSkill_AddUpdate";
        public const string HC_Permission_PreferenceSkill_List = "Masters_PreferenceSkill_List";
        public const string HC_Permission_PreferenceSkill_Delete = "Masters_PreferenceSkill_Delete";

        public const string HC_Permission_OrganizationFormMasters = "Masters_OrganizationForm";


        public const string HC_Permission_GeneralMaster = "Masters_GM";
        public const string HC_Permission_GeneralMaster_AddUpdate = "Masters_GM_AddUpdate";
        public const string HC_Permission_GeneralMaster_Delete = "Masters_GM_Delete";
        public const string HC_Permission_GeneralMaster_List = "Masters_GM_List";

        public const string HC_Permission_Agency_AddUpdate = "Masters_Agency_AddUpdate";
        public const string HC_Permission_Agency_Delete = "Masters_Agency_Delete";
        public const string HC_Permission_Agency_List = "Masters_Agency_List";

        public const string HC_Permission_Compliance = "Master_Compliance";
        public const string HC_Permission_Compliance_AddUpdate = "Master_Compliance_AddUpdate";
        public const string HC_Permission_Compliance_Delete = "Master_Compliance_Delete";
        public const string HC_Permission_Compliance_List = "Master_Compliance_List";
        public const string HC_Permission_Compliance_Sorting = "Compliance_Sorting";

        public const string HC_Permission_Payor_AddUpdate = "Masters_Payor_AddUpdate";
        public const string HC_Permission_Payor_Delete = "Masters_Payor_Delete";
        public const string HC_Permission_Payor_List = "Masters_Payor_List";
        public const string HC_Permission_Payor_ServiceCode_Mapping = "Masters_Payor_ServiceCode_Mapping";

        public const string HC_Permission_ServiceCode_AddUpdate = "Masters_ServiceCode_AddUpdate";
        public const string HC_Permission_ServiceCode_Delete = "Masters_ServiceCode_Delete";
        public const string HC_Permission_ServiceCode_List = "Masters_ServiceCode_List";

        public const string HC_Permission_Masters_CaseManager = "Masters_CaseManager";
        public const string Permission_CaseManager_AddUpdate = "Masters_CaseManager_AddUpdate";
        public const string Permission_CaseManager_List = "Masters_CaseManager_List";

        public const string HC_Permission_Notification_Configuration = "Notification_Configuration";
        public const string Permission_Notification_Configuration_Update = "Notification_Configuration_Update";
        public const string Permission_Notification_Configuration_ViewOnly = "Notification_Configuration_ViewOnly";

        public const string HC_Permission_Masters_NoteSentence = "Masters_NoteSentence";
        public const string Permission_NoteSentence_AddUpdate = "Masters_NoteSentence_AddUpdate";
        public const string Permission_NoteSentence_List = "Masters_NoteSentence_List";

        public const string HC_Permission_Masters_Billing_Information = "Billing_Information";
        public const string Permission_View_Billing_Information = "View_Billing_Information";

        public const string HC_Permission_DxCode_AddUpdate = "Masters_DXCode_AddUpdate";
        public const string HC_Permission_DxCode_Delete = "Masters_DXCode_Delete";
        public const string HC_Permission_DxCode_List = "Masters_DXCode_List";

        public const string HC_Permission_FacilityHouse_AddUpdate = "Masters_FacilityHouse_AddUpdate";
        public const string HC_Permission_FacilityHouse_Delete = "Masters_FacilityHouse_Delete";
        public const string HC_Permission_FacilityHouse_List = "Masters_FacilityHousen_List";

        public const string HC_Permission_Physician_AddUpdate = "Masters_Physician_AddUpdate";
        public const string HC_Permission_Physician_Delete = "Masters_Physician_Delete";
        public const string HC_Permission_Physician_List = "Masters_Physician_List";

        public const string HC_Permission_VisitTask_AddUpdate = "Masters_VisitTask_AddUpdate";
        public const string HC_Permission_VisitTask_List = "Masters_VisitTask_List";
        public const string HC_Permission_VisitTask_Delete = "Masters_VisitTask_Delete";

        public const string HC_Permission_EBCategory_AddUpdate = "Masters_Category_AddUpdate";
        public const string HC_Permission_EBCategory_Delete = "Masters_Category_Delete";
        public const string HC_Permission_EBCategory_List = "Masters_Category_List";

        public const string HC_Permission_EBMarket_AddUpdate = "Masters_Market_AddUpdate";
        public const string HC_Permission_EBMarket_Delete = "Masters_Market_Delete";
        public const string HC_Permission_EBMarket_List = "Masters_Market_List";


        public const string HC_Permission_EForm_AddUpdate = "Masters_Form_AddUpdate";
        public const string HC_Permission_EBForm_Delete = "Masters_Form_Delete";
        public const string HC_Permission_EBForm_List = "Masters_Form_List";

        public const string HC_Permission_Schedule = "Scheduling";
        public const string HC_Permission_Schedule_ScheduleMaster = "Scheduling_ScheduleMaster";
        public const string HC_Permission_Schedule_ScheduleLog = "Scheduling_ScheduleLog";
        public const string HC_Permission_Schedule_PendingSchedule = "Scheduling_PendingSchedule";
        public const string HC_Permission_Schedule_VirtualVisits = "Scheduling_VirtualVisits";
        public const string HC_Permission_Schedule_EmployeeVisit = "Scheduling_EmployeeVisit";


        public const string HC_Permission_Message = "Messages";
        public const string HC_Permission_Message_ReceivedMessage = "Message_Received_Msg";
        public const string HC_Permission_Message_SentMessage = "Message_Sent_Msg";
        public const string HC_Permission_Message_GroupSMS = "GroupSMS";
        public const string HC_Permission_Message_BroadcastNotifications = "BroadcastNotifications";

        public const string HC_Permission_Reports = "Reports";
        public const string HC_Permission_GroupTimesheet = "Report_GroupTimesheet";
        public const string HC_Permission_EmployeeVisitReports = "Report_EmployeeVisitReport";
        public const string HC_Permission_NurseSignature = "Report_NurseSignature";
        public const string HC_Permission_EmployeeBillingReports = "Report_EmployeeBillingReport";
        public const string HC_Permission_DMASFrom = "DMAS_From";
        public const string HC_Permission_WeeklyTimeSheet = "Weekly_TimeSheet";
        public const string HC_Permission_SSRS_Reports = "SSRS_Reports";
        public const string HC_Permission_ActivePatient = "ActivePatient";
        public const string HC_Permission_PatientVisitReport = "PatientVisitReport";
        public const string HC_Permission_EmployeeVisitReport = "EmployeeVisitReport";
        public const string HC_Permission_EmployeeTimeSheetReoprt = "EmployeeTimeSheetReoprt";
        public const string HC_Permission_PatientHourSummaryReoprt = "PatientHourSummaryReoprt";
        public const string HC_Permission_AdditionalPermission = "AdditionalPermissions";
        public const string HC_Permission_AdditionalPermission_RolePermissions = "AdditionalPermissions_RolePermission_Page";
        public const string HC_Permission_AdditionalPermission_OrganizationSettings = "AdditionalPermissions_OrganizationSettings";
        public const string HC_Permission_Document_Email = "Document_Email";
        public const string HC_Permission_Email_Signature = "Email_Signature";
        public const string HC_Permission_Employee_Update_Location = "Employee_Update_Location";

        public const string HC_Permission_AdministrativePermission = "AdministrativePermission";

        public const string HC_Permission_Billing_Batch837 = "Billing_Batch837";
        public const string HC_Permission_Billing_Upload835 = "Billing_Upload835";
        public const string HC_Permission_Billing_Reconcile835 = "Billing_Reconcile835";
        public const string HC_Permission_Billing_EDIFileLogs = "Billing_EDIFileLogs";
        public const string HC_Permission_Billing_EDI270_271 = "Billing_EDI270_271";
        public const string HC_Permission_Billing_EDI277CA = "Billing_EDI277CA";
        public const string myEZcareClearingHouse = "myEZcareClearingHouse";

        public const string HC_Permission_Add_Notes = "Add_Notes";
        public const string HC_Can_Approve_Bypass_ClockInOut = "Can_Approve_Bypass_ClockInOut";


        public const string Mobile_ApprovalRequired_IVR_Bypass_ClockInOut = "Mobile_ApprovalRequired_IVR_Bypass_ClockInOut";

        public const string Empoyee_TimeSheet_SimpleTask = "Empoyee_TimeSheet_SimpleTask";
        public const string Empoyee_TimeSheet_DetailTask = "Empoyee_TimeSheet_DetailTask";

        public const string HC_Permission_Organization_Preference = "Organization_Preference";
        public const string HC_Permission_Invoice = "Invoice";
        public const string HC_Permission_InvoiceList = "InvoiceList";

        public const string HC_Permission_Employee_Attendance = "Employee_Attendance";
        public const string HC_Permission_Employee_Attendance_Clockinout = "Employee_Attendance_Clockinout";
        public const string HC_Permission_Employee_Attendance_Calender = "Employee_Attendance_Calender";
        public const string HC_Permission_All_Employee_Attendance_Calender = "All_Employee_Attendance_Calender";

        #region Employee Section Permissons
        //public const string HC_Permission_Employee_AddUpdate = "Masters_Employee_AddUpdate";
        public const string HC_Permission_Employee_List = "Masters_Employee_List";
        public const string HC_Permission_Employee_Delete = "Masters_Employee_Delete";
        public const string HC_Permission_Employee_Calendar = "Masters_Employee_Calender";
        public const string HC_Permission_Employee_Schedule = "Masters_Employee_Schedule";
        public const string HC_Permission_Employee_PTO = "Masters_Employee_PTO";


        public const string HC_Permission_Employee_AddUpdate = "Masters_Employee_AddUpdate";



        public const string HC_Permission_Employee_EmployeeInfo = "Employee_EmployeeInfo";
        public const string HC_Permission_Employee_EmployeeInfo_View = "Employee_EmployeeInfo_View";
        public const string HC_Permission_Employee_EmployeeInfo_Add = "Employee_EmployeeInfo_Add";


        public const string HC_Permission_Employee_EmployeeDocuments = "Employee_EmployeeDocuments";
        public const string HC_Permission_Employee_EmployeeDocuments_AddSection = "Employee_EmployeeDocuments_AddSection";
        public const string HC_Permission_Employee_EmployeeDocuments_AddSubSection = "Employee_EmployeeDocuments_AddSubSection";
        public const string HC_Permission_Employee_EmployeeDocuments_View = "Employee_EmployeeDocuments_View";
        public const string HC_Permission_Employee_EmployeeDocuments_Add = "Employee_EmployeeDocuments_Add";
        public const string HC_Permission_Employee_EmployeeDocuments_Delete = "Employee_EmployeeDocuments_Delete";

        public const string HC_Permission_Employee_EmployeeSchedule = "Employee_EmployeeSchedule";
        public const string HC_Permission_Employee_EmployeeSchedule_View = "Employee_EmployeeSchedule_View";
        public const string HC_Permission_Employee_EmployeeSchedule_Add = "Employee_EmployeeSchedule_Add";

        public const string ADC_Scheduling_Visitor_Attendance = "Scheduling_Visitor_Attendance";


        public const string HC_Permission_Employee_EmployeePTO = "Employee_EmployeePTO";
        public const string HC_Permission_Employee_EmployeePTO_View = "Employee_EmployeePTO_View";
        public const string HC_Permission_Employee_EmployeePTO_Add = "Employee_EmployeePTO_Add";
        public const string HC_Permission_Employee_EmployeePTO_Delete = "Employee_EmployeePTO_Delete";

        public const string HC_Permission_Employee_EmployeeCalendar = "Employee_EmployeeCalendar";

        public const string HC_Permission_Empoyee_TimeSheet = "Empoyee_TimeSheet";
        public const string HC_Permission_Empoyee_TimeSheet_View = "Empoyee_TimeSheet_View";
        public const string HC_Permission_Empoyee_TimeSheett_Add = "Empoyee_TimeSheett_Add";
        public const string HC_Permission_Empoyee_TimeSheet_Delete = "Empoyee_TimeSheet_Delete";



        public const string HC_Permission_Empoyee_Notes = "Empoyee_Notes";
        public const string HC_Permission_Empoyee_Notes_View = "Empoyee_Notes_View";
        public const string HC_Permission_Empoyee_Notest_Add = "Empoyee_Notest_Add";
        public const string HC_Permission_Empoyee_Notes_Delete = "Empoyee_Notes_Delete";

        public const string HC_Permission_Employee_Certificate = "Employee_Certificate";
        public const string HC_Permission_Employee_Certificate_View = "Employee_Certificate_View";
        public const string HC_Permission_Employee_Certificate_AddUpdate = "Employee_Certificate_AddUpdate";
        public const string HC_Permission_Employee_Certificate_Delete = "Employee_Certificate_Delete";

        public const string HC_Permission_Employee_Checklist = "Employee_Checklist";
        public const string HC_Permission_Employee_Checklist_View = "Employee_Checklist_View";
        public const string HC_Permission_Employee_Checklist_Add = "Employee_Checklist_Add";

        public const string HC_Permission_Employee_Notification_Preferences = "Employee_Notification_Preferences";
        public const string HC_Permission_Employee_Notification_Preferences_View = "Employee_Notification_Preferences_View";
        public const string HC_Permission_Employee_Notification_Preferences_Update = "Employee_Notification_Preferences_Update";

        //Not in use
        public const string HC_Permission_Employee_AdditionalContact = "AdditionalContact";
        public const string HC_Permission_Employee_Add_Update_AdditionalContact = "Add_Update_AdditionalContact";
        public const string HC_Permission_Employee_Delete_AdditionalContact = "Delete_AdditionalContact";
        public const string HC_Permission_Employee_View_AdditionalContact = "View_AdditionalContact";
        //End
        public const string HC_Permission_Billing_Note = "Billing_Note";
        #endregion


        public const string HC_Permission_Billing_ARAgingReport = "AR_Aging_Report";
        public const string AttendanceSkipPatientTask = "ATTENDANCE_SKIP_PATIENT_TASK";


        #region Patient Intake Related Permissions
        public const string HC_Permission_Patient = "PatientIntake";
        //public const string HC_Permission_Patient_AddUpdate = "Patient_AddUpdate";
        public const string HC_Permission_Patient_List = "Patient_List";
        public const string HC_Permission_Patient_Delete = "Patient_Delete";
        public const string HC_Permission_Patient_Calendar = "Patient_Calendar";
        public const string HC_Permission_Nurse_Scheduler = "NurseScheduler";
        public const string HC_Permission_Patient_Schedule = "Patient_Schedule";

        public const string HC_Permission_Patient_AddUpdate = "PatientIntake_PatientDetails_ViewOnly,PatientIntake_Documents_View,PatientIntake_Billing_PatientPayor_View" +
                                                              "PatientIntake_Billing_Details_View,PatientIntake_Billing_PR_View,PatientIntake_CarePlan_TaskMapping_View" +
                                                              "PatientIntake_BlockEmployees_View,PatientIntake_IM_View,PatientIntake_TimeSheet_View,PatientIntake_Notes_View," +
                                                              "PatientIntake_Forms_View";


        public const string HC_Permission_PatientIntake_PatientDetails = "PatientIntake_PatientDetails";
        public const string HC_Permission_PatientIntake_PatientDetails_ViewOnly = "PatientIntake_PatientDetails_ViewOnly";
        public const string HC_Permission_PatientIntake_PatientDetails_AddUpdate = "PatientIntake_PatientDetails_AddUpdate";

        public const string HC_Permission_PatientDetails_ReferralHistory_View = "PatientDetails_ReferralHistory_View";
        public const string HC_Permission_PatientDetails_ReferralHistory_AddUpdate = "PatientDetails_ReferralHistory_AddUpdate";
        public const string HC_Permission_PatientDetails_ReferralHistory_Delete = "PatientDetails_ReferralHistory_Delete";

        public const string HC_Permission_PatientDetails_Physician_View = "PatientDetails_Physician_View";
        public const string HC_Permission_PatientDetails_Physician_AddUpdate = "PatientDetails_Physician_AddUpdate";
        public const string HC_Permission_PatientDetails_Physician_Delete = "PatientDetails_Physician_Delete";

        public const string HC_Permission_PatientDetails_DxCode_View = "PatientDetails_DxCode_View";
        public const string HC_Permission_PatientDetails_DxCode_AddUpdate = "PatientDetails_DxCode_AddUpdate";
        public const string HC_Permission_PatientDetails_DxCode_Delete = "PatientDetails_DxCode_Delete";

        public const string HC_Permission_PatientDetails_Medication_View = "PatientDetails_Medication_View";
        public const string HC_Permission_PatientDetails_Medication_AddUpdate = "PatientDetails_Medication_AddUpdate";

        public const string HC_Permission_PatientDetails_Allergy_AddUpdate = "PatientDetails_Allergy_AddUpdate";
        public const string HC_Permission_PatientDetails_Medication_Delete = "PatientDetails_Medication_Delete";

        public const string HC_Permission_PatientDetails_AuditLog_View = "PatientIntake_PatientDetails_AuditLog_View";

        public const string HC_Permission_PatientIntake_Documents = "PatientIntake_Documents";
        public const string HC_Permission_PatientIntake_Documents_AddSection = "PatientIntake_Documents_AddSection";
        public const string HC_Permission_PatientIntake_Documents_AddSubSection = "PatientIntake_Documents_AddSubSection";
        public const string HC_Permission_PatientIntake_Documents_View = "PatientIntake_Documents_View";
        public const string HC_Permission_PatientIntake_Documents_Add = "PatientIntake_Documents_Add";
        public const string HC_Permission_PatientIntake_Documents_Delete = "PatientIntake_Documents_Delete";

        public const string HC_Permission_PatientIntake_Billing = "PatientIntake_Billing";

        public const string HC_Permission_PatientIntake_Billing_View = "PatientIntake_Billing_PatientPayor_View,PatientIntake_Billing_Details_View," +
                                                                       "PatientIntake_Billing_PR_View";


        public const string HC_Permission_PatientIntake_Billing_PatientPayor = "PatientIntake_Billing_PatientPayor";
        public const string HC_Permission_PatientIntake_Billing_PatientPayor_View = "PatientIntake_Billing_PatientPayor_View";
        public const string HC_Permission_PatientIntake_Billing_PatientPayor_Add = "PatientIntake_Billing_PatientPayor_Add";
        public const string HC_Permission_PatientIntake_Billing_PatientPayor_Delete = "PatientIntake_Billing_PatientPayor_Delete";

        public const string HC_Permission_PatientIntake_Billing_Details = "PatientIntake_Billing_Details";
        public const string HC_Permission_PatientIntake_Billing_Details_View = "PatientIntake_Billing_Details_View";
        public const string HC_Permission_PatientIntake_Billing_Details_Add = "PatientIntake_Billing_Details_Add";
        public const string HC_Permission_PatientIntake_Billing_Details_Delete = "PatientIntake_Billing_Details_Delete";

        public const string HC_Permission_PatientIntake_PatientIntake_Billing_PR = "PatientIntake_Billing_PR";
        public const string HC_Permission_PatientIntake_Billing_PR_View = "PatientIntake_Billing_PR_View";
        public const string HC_Permission_PatientIntake_Billing_PR_Add = "PatientIntake_Billing_PR_Add";
        public const string HC_Permission_PatientIntake_Billing_PR_Delete = "PatientIntake_Billing_PR_Delete";


        public const string HC_Permission_PatientIntake_CarePlan = "PatientIntake_CarePlan";
        public const string HC_Permission_PatientIntake_CarePlan_View = "PatientIntake_CarePlan_TaskMapping_View,PatientIntake_CarePlan_PatientSchedule_View" +
                                                                        "PatientIntake_CarePlan_Frequency_View,PatientIntake_CarePlan_CaseLoad_View";

        public const string HC_Permission_PatientIntake_CarePlan_TaskMapping = "PatientIntake_CarePlan_TaskMapping";
        public const string HC_Permission_PatientIntake_CarePlan_TaskMapping_View = "PatientIntake_CarePlan_TaskMapping_View";
        public const string HC_Permission_PatientIntake_CarePlan_TaskMapping_Add = "PatientIntake_CarePlan_TaskMapping_Add";
        public const string HC_Permission_PatientIntake_CarePlan_TaskMapping_Delete = "PatientIntake_CarePlan_TaskMapping_Delete";

        public const string HC_Permission_PatientIntake_CarePlan_PatientSchedule = "PatientIntake_CarePlan_PatientSchedule";
        public const string HC_Permission_PatientIntake_CarePlan_PatientSchedule_View = "PatientIntake_CarePlan_PatientSchedule_View";
        public const string HC_Permission_PatientIntake_CarePlan_PatientSchedule_Add = "PatientIntake_CarePlan_PatientSchedule_Add";

        public const string HC_Permission_PatientIntake_CarePlan_Frequency = "PatientIntake_CarePlan_Frequency";
        public const string HC_Permission_PatientIntake_CarePlan_Frequency_View = "PatientIntake_CarePlan_Frequency_View";
        public const string HC_Permission_PatientIntake_CarePlan_Frequency_Add = "PatientIntake_CarePlan_Frequency_Add";
        public const string HC_Permission_PatientIntake_CarePlan_Frequency_Delete = "PatientIntake_CarePlan_Frequency_Delete";

        public const string HC_Permission_PatientIntake_CarePlan_CaseLoad = "PatientIntake_CarePlan_CaseLoad";
        public const string HC_Permission_PatientIntake_CarePlan_CaseLoad_View = "PatientIntake_CarePlan_CaseLoad_View";
        public const string HC_Permission_PatientIntake_CarePlan_CaseLoad_Add = "PatientIntake_CarePlan_CaseLoad_Add";
        public const string HC_Permission_PatientIntake_CarePlan_CaseLoad_Delete = "PatientIntake_CarePlan_CaseLoad_Delete";




        public const string HC_Permission_PatientIntake_PatientCalendar = "PatientIntake_PatientCalendar";


        public const string HC_Permission_PatientIntake_BlockEmployees = "PatientIntake_BlockEmployees";
        public const string HC_Permission_PatientIntake_BlockEmployees_View = "PatientIntake_BlockEmployees_View";
        public const string HC_Permission_PatientIntake_BlockEmployees_Add = "PatientIntake_BlockEmployees_Add";
        public const string HC_Permission_PatientIntake_BlockEmployees_Delete = "PatientIntake_BlockEmployees_Delete";


        public const string HC_Permission_PatientIntake_IM = "PatientIntake_IM";
        public const string HC_Permission_PatientIntake_IM_View = "PatientIntake_IM_View";
        public const string HC_Permission_PatientIntake_IM_Add = "PatientIntake_IM_Add";
        public const string HC_Permission_PatientIntake_IM_Delete = "PatientIntake_IM_Delete";

        public const string HC_Permission_PatientIntake_TimeSheet = "PatientIntake_TimeSheet";
        public const string HC_Permission_PatientIntake_TimeSheet_View = "PatientIntake_TimeSheet_View";
        public const string HC_Permission_PatientIntake_TimeSheet_Add = "PatientIntake_TimeSheet_Add";
        public const string HC_Permission_PatientIntake_TimeSheet_Delete = "PatientIntake_TimeSheet_Delete";

        public const string HC_Permission_PatientIntake_Notes = "PatientIntake_Notes";
        public const string HC_Permission_PatientIntake_Notes_View = "PatientIntake_Notes_View";
        public const string HC_Permission_PatientIntake_Notes_Add = "PatientIntake_Notes_Add";
        public const string HC_Permission_PatientIntake_Notes_Delete = "PatientIntake_Notes_Delete";


        public const string HC_Permission_PatientIntake_Forms = "PatientIntake_Forms";
        public const string HC_Permission_PatientIntake_Forms_View = "PatientIntake_Forms_View";
        public const string HC_Permission_PatientIntake_Forms_Add = "PatientIntake_Forms_Add";
        /// <summary>
        ///By Akhilesh PatientIntake_Forms Permission
        /// </summary>
        public const string HC_Permission_PatientIntake_DMAS97 = "PatientIntake_DMAS97";
        public const string HC_Permission_PatientIntake_DMAS97_View = "PatientIntake_DMAS97_View";
        public const string HC_Permission_PatientIntake_DMAS97_Add = "PatientIntake_DMAS97_Add";
        public const string HC_Permission_PatientIntake_DMAS97_Delete = "PatientIntake_DMAS97_Delete";

        public const string HC_Permission_PatientIntake_DMAS99 = "PatientIntake_DMAS99";
        public const string HC_Permission_PatientIntake_DMAS99_View = "PatientIntake_DMAS99_View";
        public const string HC_Permission_PatientIntake_DMAS99_Add = "PatientIntake_DMAS99_Add";
        public const string HC_Permission_PatientIntake_DMAS99_Delete = "PatientIntake_DMAS99_Delete";

        public const string HC_Permission_PatientIntake_CMS485 = "PatientIntake_CMS485";
        public const string HC_Permission_PatientIntake_CMS485_View = "PatientIntake_CMS485_View";
        public const string HC_Permission_PatientIntake_CMS485_Add = "PatientIntake_CMS485_Add";
        public const string HC_Permission_PatientIntake_CMS485_Delete = "PatientIntake_CMS485_Delete";


        public const string HC_Permission_PatientDetails_Allergy = "PatientDetails_Allergy";
        public const string HC_Permission_PatientDetails_Allergy_View = "PatientDetails_Allergy_View";
        public const string HC_Permission_PatientDetails_Allergy_Add = "PatientDetails_Allergy_Add";
        public const string HC_Permission_PatientDetails_Allergy_Delete = "PatientDetails_Allergy_Delete";

        public const string HC_Permission_Patient_Chart_Internal = "Patient_Chart_Internal";
        public const string HC_Permission_Patient_Chart_External = "Patient_Chart_External";
        public const string HC_Permission_Incident_Report = "Incident_Report";
        public const string HC_Permission_FaceSheet_Form = "FaceSheet_Form";
        public const string HC_Permission_Vital_Sign_Tracking = "Vital_Sign_Tracking";

        #endregion
        #region Capture Call
        public const string CaptureCallDetails = "CaptureCallDetails";
        public const string HC_Permission_CaptureCall_AddUpdate = "Masters_CaptureCall_AddUpdate";
        public const string HC_Permission_CaptureCall_Delete = "Masters_CaptureCall_Delete";
        public const string HC_Permission_CaptureCall_List = "Masters_CaptureCall_List";


        public const string HC_Permission_ReferralTracking = "ReferralTracking";
        public const string HC_Permission_ReferralTrackingAdd = "ReferralTrackingAdd";
        public const string HC_Permission_ReferralTrackingList = "ReferralTrackingList";
        public const string HC_Permission_ReferralTrackingDelete = "ReferralTrackingDelete";
        public const string HC_Permission_ReferralTrackingAttachForm = "AttachForm";
        #endregion





















        #endregion

        public static string EnableSelected = "Enable Selected";
        public static string DisableSelected = "Disable Selected";
        public static string InverseSelected = "Inverse - Enable/Disable";

        public static string SetBulkAssignee = "Set Bulk Assignee";
        public static string SetBulkStatus = "Set Bulk Status";
        public static string SetBulkGroup = "Set Bulk Group";
        public static string SelectBulkGroup = "Select Bulk Option";


        #region Weekdays
        //WeekDays Name
        public const string Monday = "Monday";
        public const string Tuesday = "Tuesday";
        public const string Wednesday = "Wednesday";
        public const string Thursday = "Thursday";
        public const string Friday = "Friday";
        public const string Saturday = "Saturday";
        public const string Sunday = "Sunday";
        public const string Wed = "Wed";

        #endregion

        #region Batch

        public static string MarkAsSent = "Mark As Sent";
        public static string MarkAsUnSent = "Mark As UnSent ";
        public static string Sent = "Sent";
        public static string UnSent = "UnSent";

        #endregion


        #region FileSize

        public static string SizeIn_TB = "TB";
        public static string SizeIn_GB = "GB";
        public static string SizeIn_MB = "MB";
        public static string SizeIn_KB = "KB";
        public static string SizeIn_Bytes = "Bytes";

        #endregion


        #region Report

        public static string Extention_xlsx = ".xlsx";
        public static string Extention_csv = ".csv";
        public static string Extention_pdf = ".pdf";
        public static string Extention_zip = ".zip";
        public static string Extention_txt = ".txt";
        public static string ReportName_ClientStatus = "ClientStatus";
        public static string ReportName_RequestClientList = "ClientList";

        public static string ReportName_ClientNotes = "ClientNotes";
        public static string ReportName_ClaimsOutcomeStatus = "ClaimsOutcomeStatus";
        public static string ReportName_LSTeamMemberCaseloads = "LSTeamMemberCaseloads";

        public static string ReportName_ReferralDetails = "ReferralDetails";
        public static string ReportName_ClientInformation = "ClientInformation";
        public static string ReportName_ServicePlanExpiration = "ServicePlanExpiration";
        public static string ReportName_Attendance = "Attendance";
        public static string ReportName_BillingSummary = "BillingSummary";
        public static string ReportName_RespiteUsage = "RespiteUsage";
        public static string ReportName_EncounterPrint = "EncounterPrint";
        public static string ReportName_DTRPrint = "DTRPrint";
        public static string ReportName_GeneralNotice = "GeneralNotice";
        public static string ReportName_DspRoster = "DSPRoster";
        public static string ReportName_ScheduleAttendance = "ScheduleAttendance";
        public static string ReportName_RequestedDocsForAttendance = "RequestedDocsForAttendance";
        public static string ReportName_LifeSkillsOutcomeMeasurements = "LifeSkillsOutcomeMeasurements";
        public static string ReportName_BXContractsTracking = "BXContractsTracking";
        public static string ReportName_Edi277 = "Edi277";
        public static string ReportName_ARAgingReport = "ARAgingReport";


        public static int FiscalYearStartDate = 01;
        public static int FiscalYearEndDate = 30;

        public static string FiscalYear = "Fiscal Year";

        #endregion

        #region Overview File

        public static string OverViewFile = "OverViewFile";
        public static string PaperRemitsEOBTemplate = "PaperRemitsEOBTemplate";

        #endregion


        public static string FileExtension_Csv = ".csv";
        public static string FileExtension_Xlsx = ".xlsx";
        public static string FileExtension_Pdf = ".pdf";

        public static string FileExtension_Hippa = ".hippa";
        public static string FileExtension_Clm = ".clm";
        public static string FileExtension_Txt = ".txt";

        public static string DefaultCountryCodeForSms = "+1";



        #region Send SMS

        public static string ScheduleNotificationSMS = "Schedule Notification SMS";

        #endregion

        public const string SuccessAction = "201";

        public static int PeriodforDeleteEdiFile = 6;


        public static string FacilityForOtherServiceCode = "Outpatient Main";


        public static string ScheduleNotificationNotice = "Schedule Notification Notice";


        public static string WinServiceMode = "Live";


        public static int IsActiveStatus = 1;

        public static string SecureText = "SECURE";




        #region BitLy

        public static string BitlyUrl = "http://api.bitly.com/v3/shorten/?login={0}&apiKey={1}&longUrl={2}&format={3}";

        #endregion



        #region Payor Short Code

        public static string Payor_MMIC = "MMIC";
        public static string Payor_CAZ = "CAZ";
        public static string Payor_PY = "PY";
        public static string Payor_UHC = "MMIC";
        public static string Payor_ProBono = "Pro Bono";
        #endregion

        #region VirtualVisits TabHashUrlID
        public static string HashUrl_VirtualVisits_TodaysVisits = "todaysVisits";
        public static string HashUrl_VirtualVisits_FutureVisits = "futureVisits";
        public static string HashUrl_VirtualVisits_PastVisits = "pastVisits";
        #endregion

        public static string HashUrl_BillingSettings_AuthScheduleLink_Past = "billingSettings_AuthScheduleLink_Past";
        public static string HashUrl_BillingSettings_AuthScheduleLink_Future = "billingSettings_AuthScheduleLink_Future";

        #region ReferralTabHashUrlID
        public static string HashUrl_ReferralDetails = "addReferralDetails";
        public static string HashUrl_ReferralDocument = "referralDocument";
        public static string HashUrl_ReferralChecklist = "checklist";
        public static string HashUrl_ReferralSparform = "sparform";
        public static string HashUrl_ReferralReviewMeasurement = "rm";
        public static string HashUrl_ReferralInternalMessage = "internalMessage";
        public static string HashUrl_ReferralTimesheet = "patientTimesheet";

        public static string HashUrl_ReferralBlockEmployee = "blockemployee";
        public static string HashUrl_ReferralTaskMapping = "TaskMapping";

        public static string HashUrl_ReferralTimeslots = "referraltimeslots";
        public static string HashUrl_ReferralPatientCalender = "patientcalender";

        public static string HashUrl_ReferralHistory = "referralHistory";
        public static string HashUrl_ReferralNote = "referralNote";
        public static string HashUrl_ReferralMIF = "referralMIF";
        public static string HashUrl_EbForms = "ebForms";
        public static string HashUrl_ReferralAccessDenied = "accessdenied";

        public static string HashUrl_Billings = "billings";
        public static string HashUrl_BillingSettings = "BillingSettings";
        public static string HashUrl_ReferralPatientPayors = "PatientPayors";

        public static string HashUrl_CarePlan = "CarePlan";
        #endregion

        public static string HashUrl_Process270 = "process270";
        public static string HashUrl_Process271 = "process271";

        public static int TransportationAssignmentRemoveAction = 1;
        public static int TransportationAssignmentKeepAction = 2;

        #region for Update Schedule Sent Email Status,SentSMS Status,Also Update Genrate Print Notice

        public static string Email = "Email";
        public static string SMS = "SMS";
        public static string Notice = "Notice";
        public static string ShowCaptch = "ShowCaptch";
        public static string AccountLocked = "AccountLocked";


        public const string ViaSite = "Via Site";
        public const string ViaService = "Via Service";
        #endregion

        #region Referral Monthly Summary

        public static string Happy = "Happy";
        public static string Sad = "Sad";
        public static string Quiet = "Quiet";
        public static string Talkative = "Talkative";
        public static string Sleepy = "Sleepy";
        public static string FeelingSick = "Feeling_Sick";
        public static string Helpful = "Helpful";
        public static string Whiny = "Whiny";
        public static string Overactive = "Overactive";
        public static string Bossy = "Bossy";
        public static string Aggressive = "Aggressive";
        public static string onwStaff = "1_on_1_with_Staff";
        public static string Playful = "Playful";
        public static string Demanding = "Demanding";
        public static string Cuddly = "Cuddly";
        public static string Silly = "Silly";
        public static string Angry = "Angry";
        public static string Inquisitive = "Inquisitive";
        public static string Excitable = "Excitable";
        public static string OtherDetails = "Other_Details";
        public static string Sociable = "Sociable";


        public static string InPerson = "In Person";
        public static string ViaPhone = "Via Phone";
        public static string MedicationLable = "Medication";
        public static string BXReport = "BXReport";
        public static string Other = "Other";
        public static string Activity = "Activity";
        public static string Some = "Some";
        public static string Hungry = "I_was_not_hungry";
        public static string Explanation = "Explanation: ";
        public static string OtherDetailsLable = "Other Details: ";

        public static string OtherLabel = "Other: ";

        public static string MonthlySummary = "MonthlySummary";

        public static string Childrens = "Children's";
        public static string Childs = "Child's";

        #endregion

        public static string DspRosterStatus = "1,2,3,5,6,7,8,9,10,11,,12,13,14";
        public static string Eligibilty270Status = "1,5,10,11,12,14";
        public static long NotStaff = -2;



        #region POS
        public static string Pharmacy = "01 - Pharmacy";
        public static string Telehealth = "02 - Telehealth";
        public static string School = "03 - School";
        public static string HomelessShelter = "04 - Homeless Shelter";
        public static string IndianHealthServiceFreeStandingFacility = "05 - Indian Health Service Free-standing Facility";
        public static string IndianHealthServiceProviderBasedFacility = "06 - Indian Health Service Provider-based Facility";
        public static string Tribal638FreeStandingFacility = "07 - Tribal 638 Free-standing Facility";
        public static string Tribal638ProviderBasedFacility = "08 - Tribal 638 Provider-based Facility";
        public static string PrisonCorrectionalFacility = "09 - Prison/ Correctional Facility";
        public static string Office = "11 - Office";
        public static string Home = "12 - Home";
        public static string AssistedLivingFacility = "13 - Assisted Living Facility";
        public static string GroupHome = "14 - Group Home";
        public static string MobileUnit = "15 - Mobile Unit";
        public static string TemporaryLodging = "16 - Temporary Lodging";
        public static string WalkInRetailHealthClinic = "17 - Walk-in Retail Health Clinic";
        public static string PlaceofEmploymentWorksite = "18 - Place of Employment/Worksite";
        public static string OffCampusOutpatientHospital = "19 - Off Campus-Outpatient Hospital";
        public static string UrgentCareFacility = "20 - Urgent Care Facility";
        public static string InpatientHospital = "21 - Inpatient Hospital";
        public static string OnCampusOutpatientHospital = "22 - On Campus-Outpatient Hospital";
        public static string EmergencyRoomHospital = "23 - Emergency Room - Hospital";
        public static string AmbulatorySurgicalCenter = "24 - Ambulatory Surgical Center";
        public static string BirthingCenter = "25 - Birthing Center";
        public static string MilitaryTreatmentFacility = "26 - Military Treatment Facility";
        public static string SkilledNursingFacility = "31 - Skilled Nursing Facility";
        public static string NursingFacility = "32 - Nursing Facility";
        public static string CustodialCareFacility = "33 - Custodial Care Facility";
        public static string Hospice = "34 - Hospice";
        public static string AmbulanceLand = "41 - Ambulance-Land";
        public static string AmbulanceAirorWater = "42 - Ambulance-Air or Water";
        public static string IndependentClinic = "49 - Independent Clinic";
        public static string FederallyQualifiedHealthCenter = "50 - Federally Qualified Health Center";
        public static string InpatientPsychiatricFacility = "51 - Inpatient Psychiatric Facility";
        public static string PsychiatricFacilityPartialHospitalization = "52 - Psychiatric Facility-Partial Hospitalization";
        public static string CommunityMentalHealthCenter = "53 - Community Mental Health Center";
        public static string IntermediateCareFacilityMentallyRetarded = "54 - Intermediate Care Facility/Mentally Retarded";
        public static string ResidentialSubstanceAbuseTreatmentFacility = "55 - Residential Substance Abuse Treatment Facility";
        public static string PsychiatricResidentialTreatmentCenter = "56 - Psychiatric Residential Treatment Center";
        public static string NonresidentialSubstanceAbuseTreatmentFacility = "57 - Non-residential Substance Abuse Treatment Facility";
        public static string MassImmunizationCenter = "60 - Mass Immunization Center";
        public static string ComprehensiveInpatientRehabilitationFacility = "61 - Comprehensive Inpatient Rehabilitation Facility";
        public static string ComprehensiveOutpatientRehabilitationFacility = "62 - Comprehensive Outpatient Rehabilitation Facility";
        public static string EndStageRenalDiseaseTreatmentFacility = "65 - End-Stage Renal Disease Treatment Facility";
        public static string PublicHealthClinic = "71 - Public Health Clinic";
        public static string RuralHealthClinic = "72 - Rural Health Clinic";
        public static string IndependentLaboratory = "81 - Independent Laboratory";
        public static string OtherPlaceOfService = "99 - Other Place of Service";
        #endregion

        #region AdmissionType
        public static string Emergency = "01 - Emergency";
        public static string Urgent = "02 - Urgent";
        public static string Elective = "03 - Elective";
        public static string Newborn = "04 - Newborn";
        public static string Trauma = "05 - Trauma";
        public static string InformationNotAvailable = "09 - Information Not Available";
        #endregion

        #region AdmissionSource
        public static string PhysicianReferral = "01 - Physician Referral";
        public static string ClinicReferral = "02 - Clinic Referral";
        public static string HMOReferral = "03 - HMO Referral";
        public static string TransferFromHospital = "04 - Transfer from Hospital";
        public static string TransferFromSNF = "05 - Transfer from SNF";
        public static string TransferFromAnotherHealthCareFacility = "06 - Transfer From Another Health Care Facility";
        public static string EmergencyRoom = "07 - Emergency Room";
        public static string CourtLawEnforcement = "08 - Court/Law Enforcement";

        // In the Case of Newborn
        public static string NormalDelivery = "01 - Normal Delivery";
        public static string PrematureDelivery = "02 - Premature Delivery";
        public static string SickBaby = "03 - Sick Baby";
        public static string ExtramuralBirth = "04 - Extramural Birth";

        #endregion

        #region Patient Discharge Status

        public static string DischargedToHomeOrSelfCare = "01 - Discharged to home or self care (routine discharge)";
        public static string DischargedTransferredToAnotherShortTermGeneral = "02 - Discharged/transferred to another short-term general hospital";
        public static string DischargedTransferredToSkilledNursingFacility = "03 - Discharged/transferred to skilled nursing facility (SNF)";
        public static string DischargedTransferredToAnIntermediateCareFacility = "04 - Discharged/transferred to an intermediate care facility (ICF)";
        public static string DischargedTransferredToAnotherTypeOfInstitution = "05 - Discharged/transferred to another type of institution";
        public static string DischargedTransferredToHomeUnderCareOfOrganizedHomeHealthServiceOrganization = "06 - Discharged/transferred to home under care of organized home health service organization";
        public static string LeftAgainstMedicalAdvice = "07 - Left against medical advice";
        public static string Reserved = "08 - Reserved";
        public static string AdmittedAsAnInpatientToThisHospital = "09 - Admitted as an inpatient to this hospital(Medicare Outpatient Only)";
        public static string ExpiredOrDidNotRecover = "20 - Expired (or did not recover - Christian Science patient)";
        public static string StillaPatient = "30 - Still a patient";
        public static string ExpiredAtHome = "40 - Expired at home";
        public static string ExpiredInaMedicalFacility = "41 - Expired in a medical facility; e.g., hospital, SNF, ICF, or free-standing hospice (Medicare Hospice Care Only)";
        public static string ExpiredPlaceUnknown = "42 - Expired - place unknown (Medicare Hospice Care Only)";
        public static string DischargedToFederalHealthCareFacility = "43 - Discharged to Federal Health Care Facility";
        public static string HospiceHome = "50 - Hospice- Home";
        public static string HospiceMedicalFacility = "51 - Hospice – Medical Facility";
        public static string DischargeToHospitalBasedSwingBed = "61 - Discharge to Hospital Based Swing Bed";
        public static string DischargedToInpatientRehab = "62 - Discharged to Inpatient Rehab";
        public static string DischargedToLongTermCareHospital = "63 - Discharged to Long Term Care Hospital";
        public static string DischargedToNursingFacility = "64 - Discharged to Nursing Facility";
        public static string DischargedToPsychiatricHospital = "65 - Discharged to Psychiatric Hospital";
        public static string DischargedToCriticalAccessHospital = "66 - Discharged to Critical Access Hospital";

        #endregion

        public static string TempReferralTable = "TempReferral";
        public static string TempReferralColumns = "Id,FirstName,LastName,IsShow";

        public static string TimeFormat = "HH:mm tt";
        public static string DateFormatWithDay = "dddd, MM/dd/yyy";

        public static int SuperAdminRole = 1;
        public static int PatientRole = 26;

        public static string LimitedRecordsAccess = "Record_Access_Limited_Record";
        public static string SameGroupAndLimitedEmployeeRecordAccess = "EmployeeRecord_Access_Group_And_Limited_Record";
        public static string SameGroupAndLimitedPatientRecordAccess = "PatientRecord_Access_Group_And_Limited_Record";
        public static string AllRecordAccess = "Record_Access_All_Record";

        #region MedicationURL
        public const string AddMedicationURL = "/hc/Medication/AddMedication";

        #endregion


        public const string HC_Dmas97 = "/hc/DMAS/DMAS_97_AB/";
        public const string HC_Dmas97List = "/hc/DMAS/DMAS97ABList/";
        public const string Generate_Dmas97_Pdf = "/hc/DMAS/GenerateDmas97Pdf/";
        public const string GenerateDMAS_97_AB = "/hc/DMAS/GenerateDmas97AB/";
        public const string Clone_Dmas97AB = "/hc/DMAS/CloneDmas97AB/";
        public const string CloneDataDmas97 = "/hc/DMAS/CloneDataDMAS97AB/";

        public const string HC_Dmas99 = "/hc/DMAS/DMAS_99/";
        public const string HC_Dmas99List = "/hc/DMAS/DMAS99List/";
        public const string Generate_Dmas99_Pdf = "/hc/DMAS/GenerateDmas99Pdf/";
        public const string GenerateDMAS99Form = "/hc/DMAS/GenerateDMAS99Form/";
        public const string Clone_Dmas99 = "/hc/DMAS/CloneDmas99/";

        public const string GenerateCms485Form = "/hc/DMAS/GenerateCms485Form/";
        public const string Generate_Cms485_Pdf = "/hc/DMAS/GenerateCms485Pdf/";
        public const string CertificateDownload = "/hc/referral/Download/";

        #region DashboardCard
        public const string HC_Permission_DashboardCard = "DashboardCard";
        public const string HC_Permission_DashboardCard_Emp_OverTime = "DashboardCard_Emp_OverTime";
        public const string HC_Permission_DashboardCard_Emp_NewPatient = "DashboardCard_Emp_NewPatient";
        public const string HC_Permission_Dashboard_ActivePatientCount = "Dashboard_ActivePatientCount";
        public const string HC_Permission_DashboardCard_Emp_PatientFullySchedule = "DashboardCard_Emp_PatientFullySchedule";
        public const string HC_Permission_DashboardCard_Emp_AssignInternalMessage = "DashboardCard_Emp_AssignInternalMessage";
        public const string HC_Permission_DashboardCard_Emp_ApprovalPendingForVisits = "DashboardCard_Emp_ApprovalPendingForVisits";
        public const string HC_Permission_DashboardCard_Emp_TotalSchedule = "DashboardCard_Emp_TotalSchedule";
        public const string HC_Permission_DashboardCard_Emp_PriorAuthExpiring = "DashboardCard_Emp_PriorAuthExpiring";
        public const string HC_Permission_DashboardCard_Emp_PriorAuthExpired = "DashboardCard_Emp_PriorAuthExpired";
        public const string HC_Permission_Dashboard_EmployeeBirthday = "Dashboard_EmployeeBirthday";
        public const string HC_Permission_Dashboard_PatientBirthday = "Dashboard_PatientBirthday";
        public const string HC_Permission_DashboardCard_Patient_TotalSchedule = "Dashboard_Patient_TotalSchedule";
        public const string HC_Permission_DashboardCard_Patient_TotalPresent = "Dashboard_Patient_TotalPresent";
        public const string HC_Permission_DashboardCard_Patient_TotalAbsent = "Dashboard_Patient_TotalAbsent";
        public const string HC_Permission_Dashboard_Patient_OnHold = "Dashboard_Patient_OnHold";
        public const string HC_Permission_Dashboard_Patient_Transfer = "Dashboard_Patient_Transfer";
        public const string HC_Permission_Dashboard_Patient_Pending = "Dashboard_Patient_Pending";
        public const string HC_Permission_Dashboard_Patient_Discharged = "Dashboard_Patient_Discharged";
        public const string HC_Permission_Dashboard_Patient_Medicaid = "Dashboard_Patient_Medicaid";
        public const string HC_Permission_DashboardCard_Patient_TotalPayor = "DashboardCard_Patient_TotalPayor";
        public const string HC_Permission_DashboardCard_Patient_PayorList = "DashboardCard_Patient_PayorList";
        #endregion



        public const string SuperAdmin = "SuperAdmin";
        public const string ST_AddAgencyURL = "/st/facility/addreferral";
        public const string ST_ListAgencyURL = "/st/facility/referrallist/";

        public static string ReconcileStatus_NA = "-2";
        public static string Cookie_AgingReportFilters = "Cookie_AgingReportFilters";

        public const string Paid = "Paid";
        public const string Denied = "Denied";
        public const string InProcess = "InProcess";
    }


    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomRegularExpressionAttribute : ValidationAttribute
    {
        public CustomRegularExpressionAttribute(string patternResourceName)
        {
            PatternResourceName = patternResourceName;
        }
        private string PatternResourceName { get; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pattern = SSNRegex(PatternResourceName);

            if (value != null && !Regex.IsMatch(value.ToString(), pattern))
            {
                return new ValidationResult(ErrorMessage ?? $"Invalid {validationContext.DisplayName} number.");
            }
            return ValidationResult.Success;
        }

        private string SSNRegex(string resourceName)
        {
            return @"^\d{3}-\d{2}-\d{4}$";// Resource.RegxSSN;
        }
    }

}