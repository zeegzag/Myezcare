using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class ScheduleMasterModel
    {
        public ScheduleMasterModel()
        {
            ScheduleStatuses = new List<ScheduleStatus>();
            TransportLocation = new List<TransportLocationModel>();
            Facilities = new List<FacilityModel>();
            Regions = new List<NameValueData>();

            SearchScheduleMasterModel = new SearchScheduleMasterModel();
            ScheduleMaster = new ScheduleMaster();
            CancellationReasons = new List<NameValueDataInString>();
            ScheduleMasterDataModel = new ScheduleMasterDataModel();
            ScheduleEmailModel = new ScheduleEmailModel();
            ScheduleSmsModel = new ScheduleSmsModel();
            Weeks = new List<WeekMaster>();
            Languages = new List<NameValueData>();
            PreferredCommunicationMethod = new List<NameValueData>();
        }

        public List<ScheduleStatus> ScheduleStatuses { get; set; }

        public List<TransportLocationModel> TransportLocation { get; set; }

        public List<FacilityModel> Facilities { get; set; }

        public List<NameValueData> Regions { get; set; }
        public List<WeekMaster> Weeks { get; set; }
        public List<NameValueData> Languages { get; set; }

        [Ignore]
        public SearchScheduleMasterModel SearchScheduleMasterModel { get; set; }

        [Ignore]
        public ScheduleMaster ScheduleMaster { get; set; }

        [Ignore]
        public List<NameValueDataInString> CancellationReasons { get; set; }

        [Ignore]
        public bool IsPartial { get; set; }

        [Ignore]
        public ScheduleMasterDataModel ScheduleMasterDataModel { get; set; }

        [Ignore]
        public ScheduleEmailModel ScheduleEmailModel { get; set; }

        [Ignore]
        public ScheduleSmsModel ScheduleSmsModel { get; set; }

        [Ignore]
        public List<NameValueDataInString> ScheduleBatchServiceTypeList { get; set; }

        [Ignore]
        public ScheduleBatchService AddScheduleBatchService { get; set; }

        [Ignore]
        public List<NameValueData> PreferredCommunicationMethod { get; set; }

    }

    public class PendingSchedulesPageModel
    {
        public PendingSchedulesPageModel()
        {
            EmployeeList = new List<EmployeeModel>();
            SearchPendingScheduleListPage = new SearchPendingSchedules();
        }

        public List<EmployeeModel> EmployeeList { get; set; }
        [Ignore]
        public SearchPendingSchedules SearchPendingScheduleListPage { get; set; }

    }

    public class SearchPendingSchedules
    {
        public long EmployeeID { get; set; }
        public long CreatedBy { get; set; }
        public string EmployeeName { get; set; }
        public string PatientName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }



    public class PendingScheduleListModel
    {
        public long PendingScheduleID { get; set; }
        public long ScheduleID { get; set; }
        public string PatientName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }

        private DateTime _createdDate { get; set; }

        public DateTime CreatedDate {get { return _createdDate; } set { _createdDate = Common.ConvertDateToOrgTimeZone(value); } }

        public int Count{ get; set; }

        [Ignore]
        public string EncryptedPendingScheduleID {get { return Crypto.Encrypt(Convert.ToString(PendingScheduleID)); }}
    }


    public class HC_ScheduleMasterModel
    {
        public HC_ScheduleMasterModel()
        {
            ScheduleStatuses = new List<ScheduleStatus>();
            TransportLocation = new List<TransportLocationModel>();
            Facilities = new List<FacilityModel>();
            Regions = new List<NameValueData>();

            SearchScheduleMasterModel = new SearchScheduleMasterModel();
            ScheduleMaster = new ScheduleMaster();
            CancellationReasons = new List<NameValueDataInString>();
            ScheduleMasterDataModel = new ScheduleMasterDataModel();
            ScheduleEmailModel = new ScheduleEmailModel();
            ScheduleSmsModel = new ScheduleSmsModel();
            Weeks = new List<WeekMaster>();
            Languages = new List<NameValueData>();
            PreferredCommunicationMethod = new List<NameValueData>();
            EmployeeList=new List<EmployeeSchModel>();
        }

        public List<ScheduleStatus> ScheduleStatuses { get; set; }

        public List<TransportLocationModel> TransportLocation { get; set; }

        public List<FacilityModel> Facilities { get; set; }

        public List<NameValueData> Regions { get; set; }
        public List<WeekMaster> Weeks { get; set; }
        public List<NameValueData> Languages { get; set; }
        public List<EmployeeSchModel> EmployeeList { get; set; }

        [Ignore]
        public SearchScheduleMasterModel SearchScheduleMasterModel { get; set; }

        [Ignore]
        public ScheduleMaster ScheduleMaster { get; set; }

        [Ignore]
        public List<NameValueDataInString> CancellationReasons { get; set; }

        [Ignore]
        public bool IsPartial { get; set; }

        [Ignore]
        public ScheduleMasterDataModel ScheduleMasterDataModel { get; set; }

        [Ignore]
        public ScheduleEmailModel ScheduleEmailModel { get; set; }

        [Ignore]
        public ScheduleSmsModel ScheduleSmsModel { get; set; }

        [Ignore]
        public List<NameValueDataInString> ScheduleBatchServiceTypeList { get; set; }

        [Ignore]
        public ScheduleBatchService AddScheduleBatchService { get; set; }

        [Ignore]
        public List<NameValueData> PreferredCommunicationMethod { get; set; }

    }

    public class SetScheduleAggregatorLogsListPage
    {
        public SetScheduleAggregatorLogsListPage()
        {
            SearchScheduleAggregatorLogsModel = new SearchScheduleAggregatorLogsModel();
            EmployeeList = new List<EmployeeSchModel>();
            ClaimProcessorList = new List<NameValueStringData>();
        }

        public List<EmployeeSchModel> EmployeeList { get; set; }
        public List<NameValueStringData> ClaimProcessorList { get; set; }

        [Ignore]
        public SearchScheduleAggregatorLogsModel SearchScheduleAggregatorLogsModel { get; set; }

    }

    public class SearchScheduleAggregatorLogsModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public long ScheduleID { get; set; }
        public long EmployeeID { get; set; }
        public string ClaimProcessor { get; set; }
        public DateTime? LastSent { get; set; }
        public string Status { get; set; }
    }

    public class SearchScheduleMasterModel
    {
        public SearchScheduleMasterModel()
        {
            IsShowListing = true;
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ScheduleStatusID { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public long? DropOffLocation { get; set; }
        public long? FacilityID { get; set; }
        public long? RegionID { get; set; }
        public long? LanguageID { get; set; }
        public string ReferralID { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public bool IsShowListing { get; set; }
        public long ScheduleID { get; set; }
        public long EmployeeID { get; set; }
        public bool IsPartial { get; set; }
        public int PreferredCommunicationMethodID { get; set; }

        public string AttendanceStatus { get; set; }
        public string Reason { get; set; }

    }

    public class TransportLocationModel
    {
        public long TransportLocationID { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; }
    }

    [TableName("Facilities")]
    public class FacilityModel
    {
        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
        public bool IsDeleted { get; set; }
    }

    
    public class FacilityDDL
    {
        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
    }

    public class ValidateScheduleMasterModel
    {
        public int ScheduleConflictCount { get; set; }
        public int OutOfBadCapacityCount { get; set; }
        public int OutOfRoomCapacityCount { get; set; }
    }


    public class HC_ValidateScheduleMasterModel
    {
        public int ScheduleConflictCount { get; set; }
        public int PatientPreferenceCount { get; set; }
        public int OutOfRoomCapacityCount { get; set; }
    }


    public class ScheduleEmailModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "SubjectRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceName = "EmailTemplateBodyRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Body { get; set; }
        public long ScheduleID { get; set; }

        public DateTime StartDate { get; set; }

        public string ZerpathLogoImage { get; set; }

        public string ParentName { get; set; }
        public string Phone { get; set; }
        public long ReferralID { get; set; }
    }

    public class ScheduleSmsModel
    {
        [Required(ErrorMessageResourceName = "NumberRequired", ErrorMessageResourceType = typeof(Resource))]
        public string ToNumber { get; set; }

        [Required(ErrorMessageResourceName = "SmsTextRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Body { get; set; }

        public long ScheduleID { get; set; }

        public DateTime StartDate { get; set; }

        public string ParentName { get; set; }
        public string Phone { get; set; }
        public long ReferralID { get; set; }

    }

    public class ListScheduleEmail : EmailToken
    {
        CacheHelper _cacheHelper = new CacheHelper();
        public string MobileNumber { get; set; }
        public long ScheduleID { get; set; }
        public string ScheduleStatusID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FacilityID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string DropOffImage { get; set; }
        public string PickUpImage { get; set; }

        public string DropOffLocation { get; set; }
        public string DropOffAddress { get; set; }
        public string DropOffCity { get; set; }
        public string DropOffZip { get; set; }
        public string DropoffStatename { get; set; }

        public string PickUPLocation { get; set; }
        public string PickAddress { get; set; }
        public string PickCity { get; set; }
        public string PickZip { get; set; }
        public string PickUpStatename { get; set; }

        public string MondayPickUp { get; set; }
        public string TuesdayPickUp { get; set; }
        public string WednesdayPickUp { get; set; }
        public string ThursdayPickUp { get; set; }
        public string FridayPickUp { get; set; }
        public string SaturdayPickUp { get; set; }
        public string SundayPickUp { get; set; }
        public string MondayDropOff { get; set; }
        public string TuesdayDropOff { get; set; }
        public string WednesdayDropOff { get; set; }
        public string ThursdayDropOff { get; set; }
        public string FridayDropOff { get; set; }
        public string SaturdayDropOff { get; set; }
        public string SundayDropOff { get; set; }

        public string WeekSMSDate { get; set; }
        public string WeekEmailDate { get; set; }
        public string Email { get; set; }
        public string MailStatus { get; set; }

        public string ConfirmationUrl { get; set; }
        public string CancellatioUrl { get; set; }
        public string LastWeekGenrateDate { get; set; }
        public string DropOffTime { get; set; }
        public string DropOffDay { get; set; }
        public string PickUpTime { get; set; }
        public string PickUpDay { get; set; }
        public string DropOffStateCode { get; set; }
        public string PickUpStateCode { get; set; }

        public string ZerpathLogoImage { get; set; }
        public string DropOffFullAddress
        {
            get
            {
                var lst = new List<string>
                {
                    DropOffLocation, 
                    DropOffAddress, 
                    DropOffCity+ ( string.IsNullOrEmpty(DropOffStateCode)?"":(", " +DropOffStateCode + ".")) +( string.IsNullOrEmpty(DropOffZip)?"":("-" +DropOffZip + "."))
                };
                return string.Join(",<br/>", lst.Where(m => !string.IsNullOrEmpty(m)).ToList());
            }
        }
        public string PickUpFullAddress
        {
            get
            {
                var lst = new List<string>
                {
                    PickUPLocation, 
                    PickAddress, 
                    PickCity + ( string.IsNullOrEmpty(PickUpStateCode)?"":(", " +PickUpStateCode + "")  +( string.IsNullOrEmpty(PickZip)?"":("-" +PickZip + ".")))
                };
                return string.Join(",<br/>", lst.Where(m => !string.IsNullOrEmpty(m)).ToList());
            }
        }

        public string DropOffFullAddressforURL
        {
            get
            {
                var lst = new List<string>
                {
                    DropOffLocation, 
                    DropOffAddress, 
                    DropOffCity ,
                    DropOffStateCode + (string.IsNullOrEmpty(DropOffZip)?"":("+" + DropOffZip))
                };
                return string.Join("+", lst.Where(m => !string.IsNullOrEmpty(m)).ToList());
            }
        }

        public string PickUpFullAddressforURL
        {
            get
            {
                var lst = new List<string>
                {
                    PickUPLocation, 
                    PickAddress, 
                    PickCity ,
                    PickUpStateCode +( string.IsNullOrEmpty(PickZip)?"":("+"+PickZip ))     
                };
                return string.Join("+", lst.Where(m => !string.IsNullOrEmpty(m)).ToList());
            }
        }

        public bool PermissionForEmail { get; set; }
        public bool PermissionForSMS { get; set; }
        public string Phone1 { get; set; }
        public string to { get; set; }
        public string ScheduleDateString { get; set; }

        public string DropOffPhone { get; set; }
        public string PickUpPhone { get; set; }
        public string ParentLastName { get; set; }
        public string ParentFirstName { get; set; }
        public string AtPickUp { get; set; }
        public string AtDropOff { get; set; }
        public string ClientNickName { get; set; }

        public string StrPickUpPhone
        {
            get
            {
                if (!string.IsNullOrEmpty(PickUpPhone))
                {
                    return "24 Hour Line: " + Convert.ToInt64(PickUpPhone).ToString("(###) ###-####");
                }
                return null;
            }
        }

        public string strDropOffPhone
        {
            get
            {
                if (!string.IsNullOrEmpty(DropOffPhone))
                {
                    return "24 Hour Line: " + Convert.ToInt64(DropOffPhone).ToString("(###) ###-####");
                }
                return null;
            }
        }
        public string FaceBookImage
        {
            get
            {
                
                return "<img src='" + _cacheHelper.SiteBaseURL + Constants.FaceBookLogoImage + "' height='30' width='30' style='float:left;'/>";
            }
        }
        public string FaceBookImageforEmail
        {
            get
            {
                return "<img src='" + _cacheHelper.SiteBaseURL + Constants.FaceBookLogoImage + "' height='30' width='30' />";
            }
        }
    }

    public class ScheduleDetailEmailSMSParam
    {
        public string ScheduleIds { get; set; }
        public bool IsWeekMonthFromService { get; set; }
        public bool IsSendSMS { get; set; }
        public bool IsSendEmail { get; set; }
        public long? CreatedBy { get; set; }
    }

    public class ScheduleBatchServiceModel
    {
        public ScheduleBatchServiceModel()
        {
            SearchScheduleBatchServiceModel = new SearchScheduleBatchServiceModel();
            ScheduleBatchServiceTypeList = new List<NameValueData>();
            ScheduleBatchServiceStatusList = new List<NameValueDataInString>();
        }
        [Ignore]
        public SearchScheduleBatchServiceModel SearchScheduleBatchServiceModel { get; set; }

        [Ignore]
        public List<NameValueData> ScheduleBatchServiceTypeList { get; set; }

        [Ignore]
        public List<NameValueDataInString> ScheduleBatchServiceStatusList { get; set; }
    }

    public class SearchScheduleBatchServiceModel
    {
        public string ScheduleBatchServiceID { get; set; }
        public string ScheduleBatchServiceName { get; set; }
        public string ScheduleBatchServiceType { get; set; }
        public string ScheduleBatchServiceStatus { get; set; }
        public string CreatedDate { get; set; }
        public string AddedBy { get; set; }
        public string FilePath { get; set; }
        public string ListOfIdsInCSV { get; set; }
        public int IsDeleted { get; set; }
        public int Count { get; set; }
    }

    public class ScheduleBatchServiceList
    {
        public string ScheduleBatchServiceID { get; set; }
        public string ScheduleBatchServiceName { get; set; }
        public int ScheduleBatchServiceType { get; set; }
        public string ScheduleBatchServiceStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StrScheduleBatchServiceType
        {
            get
            {
                return Common.GetEnumDisplayValue((ScheduleBatchService.ScheduleBatchServiceTypes)ScheduleBatchServiceType);
            }
        }
        public string AddedBy { get; set; }
        public string FilePath { get; set; }
        public int Count { get; set; }
        [Ignore]
        public string AWSSignedFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(FilePath))
                    return null;

                AmazonFileUpload az = new AmazonFileUpload();
                return az.GetPreSignedUrl(ConfigSettings.ZarephathBucket, FilePath);
            }
        }
    }

    public class ScheduleDetailEmailTemplate
    {
        public string BodyText { get; set; }
    }

    public class HC_ListScheduleEmail
    {
        public long ScheduleID { get; set; }
        public string PatientName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public string PatientPhone { get; set; }
        public string PatientEmail { get; set; }
        public string PatientAddress { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeePhone { get; set; }
        public string HomeCareLogoImage { get; set; }
        public string SiteName { get; set; }
    }

    public class YearlyCalenderScheduleModel
    {
        public long ScheduleID { get; set; }
        public long EmployeeId { get; set; }
        public long ReferralId { get; set; }
        public string CareTypeId { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int FrequencyChoice { get; set; }
        public int Frequency { get; set; }
        public int DaysOfWeek { get; set; }
        public int WeeklyInterval { get; set; }
        public int MonthlyInterval { get; set; }
        public bool IsSundaySelected { get; set; }

        public bool IsMondaySelected { get; set; }
        public bool IsTuesdaySelected { get; set; }
        public bool IsWednesdaySelected { get; set; }
        public bool IsThursdaySelected { get; set; }
        public bool IsFridaySelected { get; set; }
        public bool IsSaturdaySelected { get; set; }
        public bool IsFirstWeekOfMonthSelected { get; set; }
        public bool IsSecondWeekOfMonthSelected { get; set; }
        public bool IsThirdWeekOfMonthSelected { get; set; }
        public bool IsFourthWeekOfMonthSelected { get; set; }
        public bool IsLastWeekOfMonthSelected { get; set; }
        public bool IsOneTimeEvent { get; set; }
        public string RecurrencePattern { get; set; }
        public string FrequencyTypeOptions { get; set; }
        public string MonthlyIntervalOptions { get; set; }
        public string ScheduleRecurrence { get; set; }
    }


    public class EventData
    {
        public string EventName { get; set; }
        public long ScheduleID { get; set; }
        public string ReasonCode { get; set; }
        public string ActionCode { get; set; }
    }

    public class AggregatorData
    {
        public string EventName { get; set; }
        public long ScheduleID { get; set; }
        public long OrganizationID { get; set; }
        public string Visit { get; set; }
    }

    public class HHAXData : AggregatorData
    {
        public string Caregiver { get; set; }
    }

    public class CareBridgeData : AggregatorData
    {
        public string ProviderTaxID { get; set; }
        public string StateInitial { get; set; }
    }

    public class TellusData : AggregatorData
    {
        public string ProviderTaxID { get; set; }
    }

    public class SandataData : AggregatorData
    {
        public string Client { get; set; }
        public string Employee { get; set; }
    }

    public class AggregatorVisitData<T>
    {
        public List<T> Data { get; set; }
        public object Detail { get; set; }
        public long ResultID { get; set; }
    }

}
