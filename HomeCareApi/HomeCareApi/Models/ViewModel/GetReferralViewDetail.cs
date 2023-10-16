using HomeCareApi.Controllers;
using HomeCareApi.Infrastructure;
using HomeCareApi.Models.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HomeCareApi.Models.ViewModel
{
    public class GetReferralViewDetail
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        //public string ImageUrl => string.Format(Constants.http+ConfigSettings.UserImageInitialPath, FirstName[0]);
        //public string FullName => FirstName + " " + LastName;
        public string FullName { get; set; }
        public DateTime ServiceDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public long ContactID { get; set; }
        public string Account { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmergencyPhone { get; set; }
        public bool IsVirtualVisit { get; set; }
    }

    public class GetPatientDetail
    {
        public long ScheduleID { get; set; }
        public long CareTypeTimeSlotID { get; set; }
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Account { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmergencyPhone { get; set; }
    }

    public class GetPastVisitModel
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public long Row { get; set; }
        public long Count { get; set; }
        public string FirstName { get; set; }
        public string EmployeeName { get; set; }
        public string ImageUrl { get; set; }
        public string ScheduleStatusName { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }

        // [JsonConverter(typeof(CustomUTCDateTimeConverter))]
        public DateTime StartDate { get; set; }
        // [JsonConverter(typeof(CustomUTCDateTimeConverter))]
        public DateTime EndDate { get; set; }
        public bool IsDenied { get; set; }
    }



    public class GetIMListModel
    {
        public long Row { get; set; }
        public long Count { get; set; }

        public string AssignedBy { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string PatientName { get; set; }

        public string Message { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolveDate { get; set; }
        public string ResolvedComment { get; set; }
        public long ReferralInternalMessageID { get; set; }
        public long UnreadMsgCount { get; set; }
        //public bool MarkAsResolvedRead { get; set; }
    }



    public class ClockInModel
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public int Type { get; set; }
        public bool IsByPass { get; set; }
        public bool IsCaseManagement { get; set; }
        public bool IsApprovalRequired { get; set; }
        public string EarlyClockOutComment { get; set; }
        public string ByPassReason { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public bool IsEarlyClockIn { get; set; }
        public string EarlyClockInComment { get; set; }
        public long RoleID { get; set; }
        public enum BypassAction
        {
            Pending = 1,
            Approved,
            Rejected
        }
    }

    public class ReferralTaskModel
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string CurrentTime { get; set; }
        public enum TaskType
        {
            Task = 1,
            Conclusion = 2
        }

    }

    public class ReferralTaskListModel
    {
        public ReferralTaskListModel()
        {
            TaskCategoryList = new List<TaskCategory>();
            ReferralTaskList = new List<ReferralTaskList>();
        }
        public List<TaskCategory> TaskCategoryList { get; set; }
        public List<ReferralTaskList> ReferralTaskList { get; set; }
        public long EmployeeVisitID { get; set; }
    }

    public class TaskCategory
    {

        public long VisitTaskCategoryID { get; set; }
        public string VisitTaskCategoryName { get; set; }
        public long? ParentCategoryLevel { get; set; }
        public string ParentCategoryName { get; set; }

    }

    public class ReferralTaskList
    {
        public long ReferralTaskMappingID { get; set; }
        public long VisitTaskCategoryID { get; set; }
        //public long EmployeeVisitID {get;set; }
        public string VisitTaskDetail { get; set; }
        public long? MinimumTimeRequired { get; set; }
        public bool IsRequired { get; set; }
    }

    public class AddTaskModel
    {
        public long ReferralTaskMappingID { get; set; }
        public long EmployeeVisitID { get; set; }
        public long EmployeeVisitNoteID { get; set; }
        public long EmployeeID { get; set; }
        public string Description { get; set; }
        public long ServiceTime { get; set; }
        public bool setAsIncomplete { get; set; }
        public bool SimpleTaskType { get; set; }
    }
    public class DeleteTaskModel
    {
        public long EmployeeVisitNoteID { get; set; }
    }
    public class AddNoteModel
    {
        public long CommonNoteID { get; set; }
        public long ReferralID { get; set; }
        public string NoteDetail { get; set; }
        public long LoggedInID { get; set; }
        public long CategoryID { get; set; }
    }
    public class GetNoteListModel
    {
        public long Row { get; set; }
        public long Count { get; set; }

        public long CommonNoteID { get; set; }
        public string AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public string NoteDetail { get; set; }
        public string CategoryName { get; set; }
    }
    public class SearchNoteModel
    {
        public long ReferralID { get; set; }
        public string NoteDetail { get; set; }
    }

    public class TaskListModel
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public enum TaskType
        {
            Task = 1,
            Conclusion = 2
        }
    }

    public class MenuPermissionModel
    {
        public MenuPermissionModel()
        {
            UnreadMsgCountModel = new UnreadMsgCountModel();
            PermissionCode = new List<string>();
            Version = new VersionCode();
            Employee = new Employee();
        }
        public UnreadMsgCountModel UnreadMsgCountModel { get; set; }
        public List<string> PermissionCode { get; set; }
        public VersionCode Version { get; set; }
        public Employee Employee { get; set; }
    }

    public class VersionCode
    {
        public string AndroidMinimumVersion { get; set; }
        public string AndroidCurrentVersion { get; set; }
        public string IOSMinimumVersion { get; set; }
        public string IOSCurrentVersion { get; set; }
        public string TermsConditionMobile { get; set; }
    }

    public class UnreadMsgCountModel
    {
        public long EmployeeID { get; set; }
        public long UnreadMsgCount { get; set; }
    }

    public class TaskList
    {
        public long EmployeeVisitNoteID { get; set; }
        public long ReferralTaskMappingID { get; set; }
        public string VisitTaskDetail { get; set; }
        public long ServiceTime { get; set; }
        public string ParentCategoryName { get; set; }
        public string VisitTaskCategoryName { get; set; }
        public string Description { get; set; }
        public bool SimpleTaskType { get; set; }
    }

    public class CareTypeList
    {
        public long CareTypeID { get; set; }
        public string CareType { get; set; }
    }

    public class ConclusionDetailList
    {
        public ConclusionDetailList()
        {
            ConclusionDetail = new List<ConclusionDetail>();
        }
        public List<ConclusionDetail> ConclusionDetail { get; set; }
        public long EmployeeVisitID { get; set; }
        public long EmployeeID { get; set; }
        public long ScheduleID { get; set; }
        public string Conclusion { get; set; }
    }

    public class ConclusionDetail
    {
        public long ReferralTaskMappingID { get; set; }
        public string Answer { get; set; }
        public string AlertComment { get; set; }
    }

    public class TaskFormModel
    {
        public long EmployeeVisitID { get; set; }
        public long ReferralTaskMappingID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
    }

    public class TaskFormMappingModel
    {
        public long ReferralTaskFormMappingID { get; set; }
        public long TaskFormMappingID { get; set; }
        public long VisitTaskID { get; set; }
        public string FormNumber { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormName { get; set; }
        public bool IsRequired { get; set; }
        public string FormURL { get; set; }
        public bool IsInternalForm { get; set; }
        public bool IsOrbeonForm { get; set; }
        public string OrbeonFormID { get; set; }
        public long ComplianceID { get; set; }
        public string InternalFormPath { get; set; }
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public bool IsFilled { get; set; }
    }

    public class OnGoingVisitDetail
    {
        //[JsonConverter(typeof(CustomUTCDateTimeConverter))]
        public DateTime? ClockInTime { get; set; }
        //[JsonConverter(typeof(CustomUTCDateTimeConverter))]
        public DateTime? ClockOutTime { get; set; }
        public bool SurveyCompleted { get; set; }
        public bool IsSigned { get; set; }
        public bool IsPCACompleted { get; set; }
        public bool IsDenied { get; set; }
        public bool IVRClockOut { get; set; }
        public bool HasConclusion { get; set; }
        public long EmployeeVisitID { get; set; }
        public string DeniedReason { get; set; }
        public string PCA_PDF_URL
        {
            get
            {
                if (IsPCACompleted)
                {
                    string url = string.Format("{0}{1}{2}", string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()), Constants.GeneratePCAPDFURL, Crypto.Encrypt(Convert.ToString(EmployeeVisitID)));
                    return url;
                }
                return string.Empty;
            }
        }
        public int AddTaskCount { get; set; }
        public bool AnyTimeClockIn { get; set; }
        public int MappedTaskCount { get; set; }
        public string CareType { get; set; }
    }

    public class PatientVisitStatus
    {
        public bool SurveyCompleted { get; set; }
        public bool IsValid { get; set; }
        public bool IsClockOut { get; set; }
    }

    public class PatientModel
    {
        public long ScheduleID { get; set; }
        public string Note { get; set; }
        public long EmployeeID { get; set; }
    }

    public class PatientDetails
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        //public string EmployeeName { get; set; }
        public string Contact { get; set; }
        public string ScheduleCode { get; set; }
    }

    public class PatientLatLongModel
    {
        public long ContactID { get; set; }
        public long EmployeeID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }



    public class ClockInOutLogModel
    {
        public int PatientID { get; set; }
        public int EmployeeID { get; set; }
        public int ScheduleID { get; set; }
        public int OrganizationID { get; set; }
        public string ClockInOutType { get; set; }
        public DateTime Time { get; set; }
        public decimal PatientLat { get; set; }
        public decimal PatientLong { get; set; }
        public decimal EmployeeLat { get; set; }
        public decimal EmployeeLong { get; set; }

    }

    public class GetTaskListModel
    {
        public GetTaskListModel()
        {
            TaskLists = new List<TaskLists>();
        }
        public List<TaskLists> TaskLists { get; set; }
        public long EmployeeVisitID { get; set; }
    }
    public class GetConclusionListModel
    {
        public GetConclusionListModel()
        {
            TaskLists = new List<TaskLists>();
            FinalComment = new FinalComment();
        }
        public List<TaskLists> TaskLists { get; set; }
        public FinalComment FinalComment { get; set; }

    }
    public class FinalComment
    {
        public long EmployeeVisitID { get; set; }
        public string SurveyComment { get; set; }
    }

    public class TaskLists
    {
        public string VisitTaskDetail { get; set; }
        public string CategoryName { get; set; }
        public long? CategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public long? SubCategoryId { get; set; }
        public long ReferralTaskMappingID { get; set; }
        public long MinimumTimeRequired { get; set; }
        public string Answer { get; set; }
        public bool ConclusionAnswer { get; set; }
        public bool IsRequired { get; set; }
        public bool SendAlert { get; set; }
        public bool IsDone { get; set; }
        public string AlertComment { get; set; }
        //public string SurveyComment { get; set; }

    }


    //public class CategoryGroupClass
    //{
    //    public Category Category { get; set; }
    //    public List<TaskLists> TaskLists { get; set; }
    //}

    public class CategoryResponse
    {
        public List<Category> CategoryList { get; set; }
        public long EmployeeVisitID { get; set; }
        public string SurveyComment { get; set; }
    }

    public class Category
    {
        public Category()
        {
            SubCategory = new List<SubCategory>();
        }
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SubCategory> SubCategory { get; set; }
        public List<TaskLists> TaskLists { get; set; }
    }

    public class SubCategory
    {
        public SubCategory()
        {
            TaskLists = new List<TaskLists>();
        }
        public long? SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public List<TaskLists> TaskLists { get; set; }
    }

    public class Task
    {
        public string VisitTaskDetail { get; set; }
    }

    public class BeneficiaryDetail
    {
        public long EmployeeVisitID { get; set; }
        public long EmployeeID { get; set; }
        public string PatientName { get; set; }
        public string AHCCCSID { get; set; }
        public string ClockInTime { get; set; }
        public string ClockOutTime { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string DayOfWeek { get; set; }
        public string PlaceOfService { get; set; }
        public string HHA_PCA_Name { get; set; }
        public string HHA_PCA_NP { get; set; }
        public string TotalTime { get; set; }
    }

    public class PCATaskModel
    {
        public PCATaskModel()
        {
            TaskList = new List<TaskList>();
            ServiceCodeList = new List<ServiceCodeList>();
            CareTypeList = new List<CareTypeList>();
            OtherTask = new OtherTask();
        }
        public List<TaskList> TaskList { get; set; }
        public List<ServiceCodeList> ServiceCodeList { get; set; }
        public List<CareTypeList> CareTypeList { get; set; }
        public OtherTask OtherTask { get; set; }
        public int RemainingTime { get; set; }
    }

    public class OtherTask
    {
        public long EmployeeVisitID { get; set; }
        public string OtherActivity { get; set; }
        public int RemainingTime { get; set; }
        public long? OtherActivityTime { get; set; }
        public long EmployeeID { get; set; }
        public long ServiceCodeID { get; set; }
        public long CareTypeID { get; set; }
        public string CareType { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public bool IsBillable { get; set; }
        public string ServiceCode { get; set; }
    }

    public class ServiceCodeList
    {
        public long ServiceCodeID { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public bool IsBillable { get; set; }
        public string CareTypeTitle { get; set; }
        public string ServiceCode { get; set; }
    }



    public class GetPCASiganture
    {
        public long EmployeeID { get; set; }
        public long EmployeeVisitID { get; set; }
        public long EmployeeSignatureID { get; set; }
        public string SignaturePath { get; set; }
        public string PatientSignature { get; set; }
        public string MobileNumber { get; set; }
        public bool IVRClockOut { get; set; }
        public DateTime? ClockOutTime { get; set; }
    }

    public class PCAModel
    {
        public long ScheduleID { get; set; }
        public long EmployeeVisitID { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string IPAddress { get; set; }
    }

    public class Signature
    {
        public string PatientSignature { get; set; }
    }

    public class SendAlertModel
    {
        public string AlertComment { get; set; }
        public string MobileNumber { get; set; }
        public string EmployeeName { get; set; }
        public string PatientName { get; set; }
        public string ByPassReasonClockIn { get; set; }
        public string ByPassReasonClockOut { get; set; }
        public string DeviceType { get; set; }
        public string FcmTokenId { get; set; }
    }

    public class SendAlertToken
    {
        public string Message { get; set; }
        public string EmployeeName { get; set; }
        public string PatientName { get; set; }
        public string AlertFor { get; set; }
    }

    public class EmpVisitModel
    {
        public long EmployeeVisitID { get; set; }
        public long ScheduleID { get; set; }
        public long EmployeeID { get; set; }
        public string ClockInTime { get; set; }
        public string ClockOutTime { get; set; }
    }

    public class EmpVisitHistory
    {
        public long EmployeeVisitID { get; set; }
        public long ReferralID { get; set; }
        public long ScheduleID { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Editable { get; set; }
        public string ImageUrl { get; set; }
        //public string ImageUrl => string.Format(ConfigSettings.WebSiteUrl, Common.GetDatabaseNameFromApi()) + ImageUrl;
        public long Row { get; set; }
        public long Count { get; set; }
    }

    public class SearchEmpPTOModel
    {
        public long EmployeeID { get; set; }
        public int DayOffTypeID { get; set; }
        public string DayOffStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class GetEmpPTOListModel
    {
        public long Row { get; set; }
        public long Count { get; set; }

        public long EmployeeDayOffID { get; set; }
        public int DayOffTypeID { get; set; }
        public string DayOffTypeName { get { return Enum.GetName(typeof(EmployeeDayOffModel.EmpDayOffType), DayOffTypeID); } }
        public string DayOffStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string EmployeeComment { get; set; }
        public string ApproverComment { get; set; }
        public string ActionTakenBy { get; set; }
        public DateTime ActionTakenDate { get; set; }
    }
    public class EmployeeDayOffModel
    {
        public long EmployeeDayOffID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DayOffStatus { get; set; }
        public string EmployeeComment { get; set; }
        public long DayOffTypeID { get; set; }

        public enum EmpDayOffType
        {
            Other = 1,
            Sick = 2,
            Vacation = 3
        }

        public enum EmployeeDayOffStatus
        {
            InProgress = 1,
            Approved = 2,
            Denied = 3
        }
    }


    public class NoteSentenceModel
    {
        public long NoteSentenceID { get; set; }
        public string NoteSentenceTitle { get; set; }
        public string NoteSentenceDetails { get; set; }
    }
    public class EmployeeModel
    {
        public long EmployeeID { get; set; }
    }
    public class NoteSentenceList
    {
        public List<NoteSentenceModel> NoteSentences { get; set; }
    }
    public class ChangeScheduleModel
    {
        public long ScheduleID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public long EmployeeID { get; set; }
    }

    public class NotesCategoryModel
    {
        public string Title { get; set; }
        public long Value { get; set; }
    }


    public class PriorAuthorizationRequestModel { 
        public long ReferralID { get; set; }
        public long BillingAuthorizationID { get; set; }
    }

    public class PriorAuthorizationModel
    {
        public string AuthorizationCode { get; set; }
        public string Available { get; set; }
        public string Allocated { get; set; }
        public string Used { get; set; }
        public string Remaining { get; set; }
        public string Unallocated { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ServiceCode { get; set; }
        public string CareType { get; set; }

    }

    public class ReferralGroupModel
    {
        public long ReferralGroupID { get; set; }
        public string GroupName { get; set; }
        public long EmployeeID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string SystemID { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ReferralGroupMappingModel
    {
        public long ReferralGroupMappingID { get; set; }
        public long ReferralID { get; set; }
        public long ReferralGroupID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string SystemID { get; set; }
    }
}