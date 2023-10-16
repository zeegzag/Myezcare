using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Infrastructure;

namespace Zarephath.Core.Models.ViewModel
{
    public class Dashboard
    {
        public Dashboard()
        {
            ReferralInternalMessageModel = new List<ReferralInternalMessageListModel>();
            ReferralSparFormChekListModel = new List<ReferralSparFormChekListModel>();
            ReferralMissingandExpireDocumentListModel = new List<ReferralMissingandExpireDocumentListModel>();
            ReferralResolvedInternalMessageListModel = new List<ReferralResolvedInternalMessageListModel>();
            ReferralInternalMissingandExpireDocumentListModel=new List<ReferralMissingandExpireDocumentListModel>();

        }
        public List<ReferralInternalMessageListModel> ReferralInternalMessageModel { get; set; }
        public List<ReferralSparFormChekListModel> ReferralSparFormChekListModel { get; set; }
        public List<ReferralMissingandExpireDocumentListModel> ReferralMissingandExpireDocumentListModel { get; set; }
        public List<ReferralResolvedInternalMessageListModel> ReferralResolvedInternalMessageListModel { get; set; }
        public List<ReferralMissingandExpireDocumentListModel> ReferralInternalMissingandExpireDocumentListModel { get; set; }
    }

    public class DashboardModel
    {
        public DashboardModel()
        {
            ReferralInternalMessageModel = new Page<ReferralInternalMessageListModel>();
            ReferralSparFormChekListModel = new Page<ReferralSparFormChekListModel>();
            ReferralMissingandExpireDocumentListModel = new Page<ReferralMissingandExpireDocumentListModel>();
            ReferralResolvedInternalMessageListModel = new Page<ReferralResolvedInternalMessageListModel>();
            ReferralInternalMissingandExpireDocumentListModel = new Page<ReferralMissingandExpireDocumentListModel>();
            ReferralAnsellCaseyReviewListModel = new Page<ReferralAnsellCaseyListModel>();
            ReferralAssignedNotesReviewListModel = new Page<ReferralAssignedNotesReviewListModel>();
        }
        public Page<ReferralInternalMessageListModel> ReferralInternalMessageModel { get; set; }
        public Page<ReferralSparFormChekListModel> ReferralSparFormChekListModel { get; set; }
        public Page<ReferralMissingandExpireDocumentListModel> ReferralMissingandExpireDocumentListModel { get; set; }
        public Page<ReferralResolvedInternalMessageListModel> ReferralResolvedInternalMessageListModel { get; set; }
        public Page<ReferralMissingandExpireDocumentListModel> ReferralInternalMissingandExpireDocumentListModel { get; set; }
        public Page<ReferralAnsellCaseyListModel> ReferralAnsellCaseyReviewListModel { get; set; }
        public Page<ReferralAssignedNotesReviewListModel> ReferralAssignedNotesReviewListModel { get; set; }
        
        
    }

    public class ReferralInternalMessageListModel
    {
        public long ReferralID { get; set; }
        public long ReferralInternalMessageID { get; set; }
        public string ClientID { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        // public string FullName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Note { get; set; }
        public string CreatedByName { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }
        public bool IsResolved { get; set; }
        public string AgencyName { get; set; }
        public string AgencyLocationName { get; set; }
        public string CaseManager { get; set; }
        public string PayorName { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }

        [Ignore]
        public string EncryptedReferralInternalMessageId { get { return Crypto.Encrypt(Convert.ToString(ReferralInternalMessageID == 0 ? 0 : ReferralInternalMessageID)); } }
        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID == 0 ? 0 : ReferralID)); } }
    }

    public class ReferralSparFormChekListModel
    {
        public long ReferralID { get; set; }
        public string ClientID { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public bool IsCheckListCompleted { get; set; }
        public bool IsSparFormCompleted { get; set; }
        public string AgencyName { get; set; }
        public string AgencyLocationName { get; set; }
        public string CaseManager { get; set; }
        public string PayorName { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }

        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID == 0 ? 0 : ReferralID)); } }
    }

    public class ReferralMissingandExpireDocumentListModel
    {
        public long ReferralID { get; set; }
        public string ClientID { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PayorName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string AgencyName { get; set; }
        public string AgencyLocationName { get; set; }
        public string CaseManager { get; set; }
        public string AROI { get; set; }
        public bool NetworkServicePlan { get; set; }
        public bool BXAssessment { get; set; }
        public string Demographic { get; set; }
        public string SNCD { get; set; }
        public string NetworkCrisisPlan { get; set; }
        public DateTime AROIExpirationDate { get; set; }
        public DateTime NSPExpirationDate { get; set; }
        public DateTime BXAssessmentExpirationDate { get; set; }
        public DateTime DemographicExpirationDate { get; set; }
        public DateTime SNCDExpirationDate { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }

        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID == 0 ? 0 : ReferralID)); } }
    }

    public class MissingDocumentListModel
    {
        public string MissingDocumentName { get; set; }
        public string MissingDocumentType { get; set; }
    }

    public class ReferralResolvedInternalMessageListModel
    {
        public long ReferralID { get; set; }
        public long ReferralInternalMessageID { get; set; }
        public string ClientID { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
       // public string FullName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string Note { get; set; }
        public string CreatedByName { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }
        public bool IsResolved { get; set; }
        public string AgencyName { get; set; }
        public string AgencyLocationName { get; set; }
        public string CaseManager { get; set; }
        public string PayorName { get; set; }
        public bool MarkAsResolvedRead { get; set; }

        public string ResolvedByName { get; set; }

        public string ResolvedComment { get; set; }

        public DateTime? ResolveDate { get; set; }
        

        public long Row { get; set; }
        public int Count { get; set; }

        [Ignore]
        public string EncryptedReferralInternalMessageId { get { return Crypto.Encrypt(Convert.ToString(ReferralInternalMessageID == 0 ? 0 : ReferralInternalMessageID)); } }
        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID == 0 ? 0 : ReferralID)); } }
    }




    public class ReferralAnsellCaseyListModel
    {
        public long ReferralID { get; set; }
        public long ReferralAssessmentID { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        // public string FullName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string Age { get; set; }
        public string Gender { get; set; }

        public string CreatedByName { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        public string AssignedByName { get; set; }
        

        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime AssessmentDate { get; set; }

        public string OverAllAverage { get; set; }

        public string AgencyName { get; set; }
        public string AgencyLocationName { get; set; }
        public string CaseManager { get; set; }
        public string PayorName { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }

        [Ignore]
        public string EncryptedReferralAssessmentID { get { return Crypto.Encrypt(Convert.ToString(ReferralAssessmentID == 0 ? 0 : ReferralAssessmentID)); } }
        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID == 0 ? 0 : ReferralID)); } }
    }



    public class ReferralAssignedNotesReviewListModel
    {
        public long ReferralID { get; set; }
        public long NoteID { get; set; }
        public string CISNumber { get; set; }
        public string AHCCCSID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
       // public string FullName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }
        public string Age { get; set; }
        public string Gender { get; set; }

        public string CreatedByName { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }

        public string AssignedByName { get; set; }
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime? NoteAssignedDate { get; set; }


        public string NoteComments { get; set; }


        public string AgencyName { get; set; }
        public string AgencyLocationName { get; set; }
        public string CaseManager { get; set; }
        public string PayorName { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }

        [Ignore]
        public string EncryptedNoteID { get { return Crypto.Encrypt(Convert.ToString(NoteID == 0 ? 0 : NoteID)); } }
        [Ignore]
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID == 0 ? 0 : ReferralID)); } }
    }





    public class EmployeeTimeStaticSearchModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class HC_GetDashboardModel
    {
        public HC_GetDashboardModel()
        {
            EmployeeTimeStaticSearchModel = new EmployeeTimeStaticSearchModel();
            EmpClockInOutListSearchModel = new EmployeeTimeStaticSearchModel();
            PatientClockInOutListSearchModel = new EmployeeTimeStaticSearchModel();
            WeeklySearchModel = new EmployeeTimeStaticSearchModel();
            EmpOverTimeListSearchModel = new EmployeeTimeStaticSearchModel();
            PatientNotScheduleListSearchModel = new EmployeeTimeStaticSearchModel();
            PatientNewListSearchModel = new EmployeeTimeStaticSearchModel();
            PatientBirthdaySearchModel = new EmployeeTimeStaticSearchModel();
            EmpBirthdaySearchModel = new EmployeeTimeStaticSearchModel();
            PatientDischargedListSearchModel = new EmployeeTimeStaticSearchModel();
            PatientTransferListSearchModel = new EmployeeTimeStaticSearchModel();
            PatientPendingListSearchModel = new EmployeeTimeStaticSearchModel();
            PatientOnHoldListSearchModel = new EmployeeTimeStaticSearchModel();
            PatientMedicaidListSearchModel = new EmployeeTimeStaticSearchModel();
            EmpClockInOutListResetSearchModel = new EmployeeTimeStaticSearchModel();
        }

        public EmployeeTimeStaticSearchModel EmployeeTimeStaticSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel EmpClockInOutListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientClockInOutListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel WeeklySearchModel { get; set; }
        public EmployeeTimeStaticSearchModel EmpOverTimeListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientNotScheduleListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientNewListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientBirthdaySearchModel { get; set; }
        public EmployeeTimeStaticSearchModel EmpBirthdaySearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientDischargedListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientTransferListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientPendingListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientOnHoldListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel PatientMedicaidListSearchModel { get; set; }
        public EmployeeTimeStaticSearchModel EmpClockInOutListResetSearchModel { get; set; }
    }

    //public class GetEmpClockInOutListModel
    //{
    //    public long ScheduleID { get; set; }
    //    public long ReferralID { get; set; }
    //    public long EmployeeID { get; set; }
    //    public string EmpFirstName { get; set; }
    //    public string EmpLastName { get; set; }
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string CareType { get; set; }
    //    public string EncryptedEmployeeID { get { return Crypto.Encrypt(Convert.ToString(EmployeeID)); } }
    //    public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
    //    public string Employee {
    //        get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
    //    }

    //    public string Patient
    //    {
    //        get { return Common.GetReferralNameFormat(FirstName, LastName); }
    //    }


    //    public DateTime ScheduleStartTime { get; set; }
    //    public DateTime ScheduleEndTime { get; set; }

    //    public bool ClockIn { get; set; }
    //    public bool ClockOut { get; set; }
    //    public string Address { get; set; }
    //    public string lat { get; set; }
    //    public string lng { get; set; }
    //    public string Phone { get; set; }
    //    public string StartTime { get; set; }
    //    public string EndTime { get; set; }
    //    public int Count { get; set; }
    //    public int TotalSchedule { get; set; }
    //    public int Inprogress { get; set; }
    //    public int ClocOutnDone { get; set; }
    //    public int MissedSchedule { get; set; }
    //    public int TotalComplete { get; set; }
    //    public string MobileNumber { get; set; }
    //    public string Status { get; set; }
    //}

    public class GetEmpClockInOutListModel
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string ReferralName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CareType { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(Convert.ToString(EmployeeID)); } }
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        public string Employee { get; set; }
        public string Patient { get; set; }

        //public string Employee
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}

        //public string Patient
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}


        public DateTime ScheduleStartTime { get; set; }
        public DateTime ScheduleEndTime { get; set; }

        public bool ClockIn { get; set; }
        public bool ClockOut { get; set; }
        public string Address { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string Phone { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Count { get; set; }
        public int TotalSchedule { get; set; }
        public int Inprogress { get; set; }
        public int ClocOutnDone { get; set; }
        public int MissedSchedule { get; set; }
        public int TotalComplete { get; set; }
        public string MobileNumber { get; set; }
        public string Status { get; set; }
    }



    public class GetEmpOverTimeListModel
    {
        public long EmployeeID { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(Convert.ToString(EmployeeID)); } }
        public string Employee { get; set; }
        //public string Employee
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}

        public float WeeklyAllocatedHours { get; set; }
        public float WeeklyUsedHours { get; set; }
        public float WeeklyOverTimeHours { get; set; }

        public int Count { get; set; }

    }




    public class GetNewPatientListModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        //public string Patient
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}
        public string Patient { get; set; }

        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string CreatedBy { get; set; }
        //public string CreatedBy
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}

        public DateTime CreatedDate { get; set; }


        public int Count { get; set; }

    }
    public class GetActivePatientCountListModel
    {
        //public long ReferralID { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        //public string Patient
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}
        //public string EmpFirstName { get; set; }
        //public string EmpLastName { get; set; }
        //public string CreatedBy
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}
        //public DateTime CreatedDate { get; set; }
        public int Count { get; set; }

    }



    public class GetPatientNotScheduledListModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        public string Patient { get; set; }
        //public string Patient
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}

        public float WeeklyAllocatedHours { get; set; }
        public float WeeklyUsedHours { get; set; }
        public float WeeklyRemainingHours { get; set; }
        public float WeeklyUnusedHours { get; set; }

        public int Count { get; set; }

    }


    public class GetEmployeeTimeStaticsList
    {
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Employee { get; set; }

        //public string Employee
        //{
        //    get { return Common.GetEmpGeneralNameFormat(FirstName, LastName); }
        //}

        public float AvgDelay { get; set; }

        public string Color { get { return Common.GetRandomColor(Convert.ToString(EmployeeID)); }}
    }


    public class GetEmpTimeStaticsListModel
    {
        public List<string> EmployeeList { get; set; }
        public List<float> AvgDelayList { get; set; }
        public List<string> ColorList { get; set; }
        


    }

    public class LateClockInNotificationModelList
    {
        public LateClockInNotificationModelList()
        {
            LateClockInNotificationModel = new List<LateClockInNotificationModel>();

        }
        public List<LateClockInNotificationModel> LateClockInNotificationModel { get; set; }


    }

    public class LateClockInNotificationModel
    {
        public long ScheduleID { get; set; }
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Employee { get; set; }
        public string ScheduleDate { get; set; }
        public string ScheduleEndDate { get; set; }
        public string StartTime { get; set; }
        public string ClockInTime { get; set; }
        public string ClockOutTime { get; set; }
  
        //   public List<DateTime> DateTime { get; set; }
    }
    
  
    public class SetWebNotificationListPage
    {
        public SetWebNotificationListPage()
        {
            WebNotificationModel = new WebNotificationModel();
            DeleteFilter = new List<NameValueData>();
        }

        public WebNotificationModel WebNotificationModel { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
    }

    public class WebNotificationModel
    {
        public long WebNotificationID { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public int MsgCount { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class PriorAuthExpiringModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long TotalVisits { get; set; }
        public string AuthorizationCode { get; set; }
        public string PayorName { get; set; }
        public string ExpireDate { get; set; }
        public long RemainingVisit { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        //public string Patient
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}
        public string Patient { get; set; }
        public int Count { get; set; }
    }

    public class PatientBirthdayModel
    {
        public long ReferralID { get; set; }
        public string PatientName { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public string DobInDateFormat
        {
            get
            {
                if (DateOfBirth != null)
                {
                    return DateOfBirth = Convert.ToDateTime(DateOfBirth).ToString("yyyy-MM-dd");

                }
                return string.Empty;
            }
        }
        public bool IsDeleted { get; set; }
        public int Count { get; set; }
    }

    public class EmployeeBirthdayModel
    {
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Phone { get; set; }
        public string DateOfBirth { get; set; }
        public string DobInDateFormat
        {
            get
            {
                if (DateOfBirth != null)
                {
                    return DateOfBirth = Convert.ToDateTime(DateOfBirth).ToString("yyyy-MM-dd");

                }
                return string.Empty;
            }
        }
        public bool IsDeleted { get; set; }
        public int Count { get; set; }
    }

    public class GetPatientClockInOutListModel
    {
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(Convert.ToString(EmployeeID)); } }
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        public string Employee { get; set; }
        //public string Employee
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}
        public string Patient { get; set; }
        //public string Patient
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}

        public DateTime ScheduleStartTime { get; set; }
        public DateTime ScheduleEndTime { get; set; }
        public bool ClockIn { get; set; }
        public bool ClockOut { get; set; }
        public bool IsPatientAttendedSchedule { get; set; }
        public string Attendence { get; set; }
        public string FacilityName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public int Count { get; set; }
        public int TotalPatientSchedule { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }

    }

    public class GetReferralPayorModel
    {
        public int PayorID { get; set; }
        public string PayorName { get; set; }
        public int PayorCount { get; set; }
        public int TotalPayor { get; set; }
        public int Count { get; set; }
    }

    public class GetReferralMedicaidModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        public string Patient { get; set; }
        //public string Patient
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}
        public string Dose { get; set; }
        public string Unit { get; set; }
        public string Frequency { get; set; }
        public string Route { get; set; }
        public string Quantity { get; set; }
        public string CreatedDate { get; set; }
        public int Count { get; set; }
    }

    public class GetPatientListModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(Convert.ToString(ReferralID)); } }
        public string Patient { get; set; }
        //public string Patient
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}


        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }

        public string CreatedBy { get; set; }
        //public string CreatedBy
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}

        public string CreatedDate { get; set; }


        public int Count { get; set; }

    }

}
