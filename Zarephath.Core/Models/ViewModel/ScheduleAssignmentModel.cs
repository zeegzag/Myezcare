using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using PetaPoco;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class ScheduleAssignmentModel
    {

        public ScheduleAssignmentModel()
        {

            RegionList = new List<Region>();
            PayorList = new List<PayorModel>();
            FrequencyCodes = new List<FrequencyCode>();
            ScheduleStatuses = new List<ScheduleStatus>();
            TransportLocation = new List<TransportLocationModel>();
            Facilities = new List<FacilityModel>();
            GenderList = Common.SetGenderList();
            ServiceList = Common.SetServicesFilter();
            SearchReferralModel = new SearchReferralListForSchedule();
            ScheduleMaster = new ScheduleMaster();
            Facility = new Facility();
            Counts = new Count();
            WeekMaster = new WeekMaster();
            ScheduleSearchModel = new ScheduleSearchModel();
            CancellationReasons = new List<NameValueDataInString>();
            ScheduleMasterDataModel = new ScheduleMasterDataModel();
            EmployeeList = new List<Employee>();
        }

        public List<Region> RegionList { get; set; }
        public List<PayorModel> PayorList { get; set; }
        public List<FrequencyCode> FrequencyCodes { get; set; }
        public List<ScheduleStatus> ScheduleStatuses { get; set; }
        public List<TransportLocationModel> TransportLocation { get; set; }
        public List<FacilityModel> Facilities { get; set; }
        public List<WeekMaster> WeekMasterList { get; set; }
        [Ignore]
        public List<NameValueData> GenderList { get; set; }
        [Ignore]
        public List<NameValueData> ServiceList { get; set; }
        [Ignore]
        public SearchReferralListForSchedule SearchReferralModel { get; set; }
        [Ignore]
        public ScheduleMaster ScheduleMaster { get; set; }
        [Ignore]
        public Facility Facility { get; set; }
        [Ignore]
        public Count Counts { get; set; }
        [Ignore]
        public WeekMaster WeekMaster { get; set; }
        [Ignore]
        public ScheduleSearchModel ScheduleSearchModel { get; set; }
        [Ignore]
        public List<NameValueDataInString> CancellationReasons { get; set; }
        [Ignore]
        public ScheduleMasterDataModel ScheduleMasterDataModel { get; set; }

        [Ignore]
        public List<Employee> EmployeeList { get; set; }
    }




    public class HC_ScheduleAssignmentModel
    {
        public HC_ScheduleAssignmentModel()
        {

            RegionList = new List<Region>();
            PayorList = new List<PayorModel>();
            FrequencyCodes = new List<FrequencyCode>();
            ScheduleStatuses = new List<ScheduleStatus>();
            TransportLocation = new List<TransportLocationModel>();
            Facilities = new List<FacilityModel>();
            GenderList = Common.SetGenderList();
            ServiceList = Common.SetServicesFilter();
            SearchReferralModel = new SearchReferralListForSchedule();
            ScheduleMaster = new ScheduleMaster();
            Facility = new Facility();
            Counts = new Count();
            WeekMaster = new WeekMaster();
            ScheduleSearchModel = new ScheduleSearchModel();
            CancellationReasons = new List<NameValueDataInString>();
            ScheduleMasterDataModel = new ScheduleMasterDataModel();
            EmployeeList = new List<EmployeeSchModel>();
        }

        public List<Region> RegionList { get; set; }
        public List<PayorModel> PayorList { get; set; }
        public List<FrequencyCode> FrequencyCodes { get; set; }
        public List<ScheduleStatus> ScheduleStatuses { get; set; }
        public List<TransportLocationModel> TransportLocation { get; set; }
        public List<FacilityModel> Facilities { get; set; }
        public List<WeekMaster> WeekMasterList { get; set; }
        [Ignore]
        public List<NameValueData> GenderList { get; set; }
        [Ignore]
        public List<NameValueData> ServiceList { get; set; }
        [Ignore]
        public SearchReferralListForSchedule SearchReferralModel { get; set; }
        [Ignore]
        public ScheduleMaster ScheduleMaster { get; set; }
        [Ignore]
        public Facility Facility { get; set; }
        [Ignore]
        public Count Counts { get; set; }
        [Ignore]
        public WeekMaster WeekMaster { get; set; }
        [Ignore]
        public ScheduleSearchModel ScheduleSearchModel { get; set; }
        [Ignore]
        public List<NameValueDataInString> CancellationReasons { get; set; }
        [Ignore]
        public ScheduleMasterDataModel ScheduleMasterDataModel { get; set; }

        public List<EmployeeSchModel> EmployeeList { get; set; }
    }




    public class HC_ScheduleAssignmentModel01
    {
        public HC_ScheduleAssignmentModel01()
        {

            Preference = new List<Preference>();
            Skills = new List<Preference>();
            FrequencyCodes = new List<FrequencyCode>();
            ScheduleStatuses = new List<ScheduleStatus>();
            SearchSchEmployeeModel = new SearchEmployeeListForSchedule();
            CancellationReasons = new List<NameValueDataInString>();
            ScheduleSearchModel = new ScheduleSearchModel();
            EmployeeList = new List<EmployeeSchModel>();
            ScheduleMaster = new ScheduleMaster();
            ServiceType = new List<ServiceTypeModel>();

        }

        public List<Preference> Preference { get; set; }
        public List<Preference> Skills { get; set; }
        public List<FrequencyCode> FrequencyCodes { get; set; }
        public List<ScheduleStatus> ScheduleStatuses { get; set; }
        public List<EmployeeSchModel> EmployeeList { get; set; }
        public List<ServiceTypeModel> ServiceType { get; set; }

        [Ignore]
        public List<NameValueDataInString> CancellationReasons { get; set; }
        [Ignore]
        public SearchEmployeeListForSchedule SearchSchEmployeeModel { get; set; }
        [Ignore]
        public ScheduleSearchModel ScheduleSearchModel { get; set; }
        [Ignore]
        public ScheduleMaster ScheduleMaster { get; set; }

    }





    public class HC_DayCare_SetScheduleAttendenceModel
    {
        public HC_DayCare_SetScheduleAttendenceModel()
        {
            SearchScheduledPatientModel = new SearchScheduledPatientModel();
            FacilityList = new List<FacilityModel>();

        }

        public List<FacilityModel> FacilityList { get; set; }


        [Ignore]
        public SearchScheduledPatientModel SearchScheduledPatientModel { get; set; }


    }

    public class HC_DayCare_ScheduleAssignmentModel
    {
        public HC_DayCare_ScheduleAssignmentModel()
        {
            SearchSchEmployeeModel = new SearchEmployeeListForSchedule();
            ScheduleSearchModel = new ScheduleSearchModel();
            FacilityList = new List<FacilityModel>();
            ScheduleMaster = new ScheduleMaster();

        }

        public List<FacilityModel> FacilityList { get; set; }


        [Ignore]
        public SearchEmployeeListForSchedule SearchSchEmployeeModel { get; set; }
        [Ignore]
        public ScheduleSearchModel ScheduleSearchModel { get; set; }
        [Ignore]
        public ScheduleMaster ScheduleMaster { get; set; }
        [Ignore]
        public CreateSchModel CreateSchModel { get; set; }

    }



    public class HC_PrivateDuty_ScheduleAssignmentModel
    {
        public HC_PrivateDuty_ScheduleAssignmentModel()
        {

            Preference = new List<Preference>();
            Skills = new List<Preference>();
            FrequencyCodes = new List<FrequencyCode>();
            ScheduleStatuses = new List<ScheduleStatus>();
            SearchSchEmployeeModel = new SearchEmployeeListForSchedule();
            CancellationReasons = new List<NameValueDataInString>();
            ScheduleSearchModel = new ScheduleSearchModel();
            EmployeeList = new List<EmployeeSchModel>();
            ScheduleMaster = new ScheduleMaster();

        }

        public List<Preference> Preference { get; set; }
        public List<Preference> Skills { get; set; }
        public List<FrequencyCode> FrequencyCodes { get; set; }
        public List<ScheduleStatus> ScheduleStatuses { get; set; }
        public List<EmployeeSchModel> EmployeeList { get; set; }


        [Ignore]
        public List<NameValueDataInString> CancellationReasons { get; set; }
        [Ignore]
        public SearchEmployeeListForSchedule SearchSchEmployeeModel { get; set; }
        [Ignore]
        public ScheduleSearchModel ScheduleSearchModel { get; set; }
        [Ignore]
        public ScheduleMaster ScheduleMaster { get; set; }

    }




    public class ReferralSchModel
    {
        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferralName { get; set; }
        public bool IsDeleted { get; set; }

        //[Ignore]
        //public string ReferralName { get { return Common.GetReferralNameFormat(FirstName, LastName); } }

    }


    public class EmployeeSchModel
    {
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public string MobileNumber { get; set; }
        public bool IsDeleted { get; set; }

        [Ignore]
        public string id
        {
            get { return Convert.ToString(EmployeeID); }
        }



        public float EmployeeUsedHours { get; set; }

        [Ignore]
        public float EmployeeRemainingHours
        {
            //get { return ConfigSettings.RespiteUsageLimit - EmployeeUsedHours; }
            get { return 40 - (EmployeeUsedHours > 40 ? 40 : EmployeeUsedHours); }
        }

        [Ignore]
        public float TotalHours
        {
            get
            {
                return 40; //ConfigSettings.RespiteUsageLimit; 

            }
        }

        [Ignore]
        public string eventColor
        {
            get
            {
                return Common.GetRandomColor(Convert.ToString(EmployeeID));


                if (EmployeeID == 2)
                {
                    return "red";
                }

                if (EmployeeID % 4 == 0)
                {
                    return "green";
                }
                if (EmployeeID % 5 == 0)
                {
                    return "orange";
                }
                if (EmployeeID % 6 == 0)
                {
                    return "#f751d8";
                }
                return "";
            }
        }
    }


    public class ReferralScheduleModel
    {
        public long ReferralID { get; set; }
        public string EncryptedReferralID
        {
            get { return Crypto.Encrypt(Convert.ToString(ReferralID)); }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferralName { get; set; }
        public string MobileNumber { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsPatientChecked { get; set; }

        [Ignore]
        public string id
        {
            get { return Convert.ToString(ReferralID); }
        }
        //[Ignore]
        //public string ReferralName { get { return Common.GetGeneralNameFormat(FirstName, LastName); } }



        public float NewAllocatedHrs { get; set; }
        public float NewUsedHrs { get; set; }
        [Ignore]
        public float NewRemainingHours { get { return NewAllocatedHrs - (NewUsedHrs > NewAllocatedHrs ? NewAllocatedHrs : NewUsedHrs); } }


        public string FacilityName { get; set; }


        //public float EmployeeUsedHours { get; set; }

        //[Ignore]
        //public float EmployeeRemainingHours
        //{
        //    //get { return ConfigSettings.RespiteUsageLimit - EmployeeUsedHours; }
        //    get { return 40 - (EmployeeUsedHours > 40 ? 40 : EmployeeUsedHours); }
        //}

        //[Ignore]
        //public float TotalHours
        //{
        //    get
        //    {
        //        return 40; //ConfigSettings.RespiteUsageLimit; 

        //    }
        //}

        [Ignore]
        public string eventColor
        {
            get
            {
                return Common.GetRandomColor(Convert.ToString(ReferralID));


            }
        }
    }


    public class EmployeeDetailSchModel
    {
        public EmployeeDetailSchModel()
        {
            Employee = new NameValueEmp();
            Referral = new NameValueEmp();
            EmployeeMatchingPreference = new List<EmployeeMatchingPreference>();
        }

        public NameValueEmp Employee { get; set; }
        public NameValueEmp Referral { get; set; }
        public List<EmployeeMatchingPreference> EmployeeMatchingPreference { get; set; }
    }

    public class NameValueEmp
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string EncryptedID
        {
            get { return Crypto.Encrypt(Convert.ToString(Value)); }
        }
    }
    public class EmployeeMatchingPreference
    {
        public long ReferralID { get; set; }
        public string PreferenceName { get; set; }
        public string ClientName { get; set; }
        public long PreferenceID { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public bool IsMatched { get; set; }
        public string MatchedPercent { get; set; }
    }






















    public class ScheduleSearchModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long WeekMasterID { get; set; }
        public long RegionID { get; set; }
        public string FaciltyIds { get; set; }
        public long ReferralID { get; set; }
        public string EmployeeName { get; set; }
        public string ServiceTypeID { get; set; }

    }

    public class SearchReferralListForSchedule
    {
        public SearchReferralListForSchedule()
        {
            CacheHelper _chHelper = new CacheHelper();
            MaxAge = 18;
            MinAge = 0;
            PageSize = _chHelper.PageSize > 0 ? _chHelper.PageSize : ConfigSettings.PageSize;
        }

        public long? RegioinID { get; set; }
        public long? FrequencyCodeID { get; set; }
        public int? Gender { get; set; }
        public long? ServiceID { get; set; }
        public long MaxAge { get; set; }
        public long MinAge { get; set; }
        public long? PayorID { get; set; }
        public string ContactName { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
        public DateTime? LastAttFromDate { get; set; }
        public DateTime? LastAttToDate { get; set; }
    }


    public class SearchEmployeeListForSchedule
    {

        public SearchEmployeeListForSchedule()
        {
            CacheHelper _chHelper = new CacheHelper();
            PageSize = _chHelper.PageSize > 0 ? _chHelper.PageSize : ConfigSettings.PageSize;
        }

        public long FrequencyCodeID { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }

        public long SkillId { get; set; }
        public long PreferenceId { get; set; }


        public string StrSkillList { get; set; }
        public string StrPreferenceList { get; set; }


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long EmployeeID { get; set; }


    }


    public class EmployeeListForSchedule
    {
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID
        {
            get { return EmployeeID > 0 ? Crypto.Encrypt(EmployeeID.ToString()) : null; }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }

        //public string EmployeeName
        //{
        //    get { return Common.GetEmpGeneralNameFormat(FirstName, LastName); }
        //}
        public long Row { get; set; }
        public int Count { get; set; }


        public float NewAllocatedHrs { get; set; }
        public float NewUsedHrs { get; set; }
        public float NewRemainingHrs { get; set; }
        //{
        //    get { return NewAllocatedHrs - (NewUsedHrs > NewAllocatedHrs ? NewAllocatedHrs : NewUsedHrs); }
        //}

    }














    public class ReferralListForSchedule
    {
        public long ReferralID { get; set; }
        public string EncryptedReferralID
        {
            get { return ReferralID > 0 ? Crypto.Encrypt(ReferralID.ToString()) : null; }
        }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Age { get; set; }
        public string PayorName { get; set; }
        public string ShortName { get; set; }
        public string PlacementRequirement { get; set; }
        public string DropOffLocationName { get; set; }
        public string PickUpLocationName { get; set; }
        public long DropOffLocation { get; set; }
        public long PickUpLocation { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public string Code { get; set; }
        public DateTime? LastAttendedDate { get; set; }
        public DateTime? NextAttDate { get; set; }


        public string RequestDate { get; set; }

        public int DefaultScheduleDays { get; set; }
        public bool NeedPrivateRoom { get; set; }
        public float UsedRespiteHours { get; set; }
        public long Row { get; set; }
        public int Count { get; set; }

        public string ReferralSiblingMappingVlaue { get; set; }

        public string SiblingsName
        {
            get
            {
                string name = "";
                if (!string.IsNullOrEmpty(ReferralSiblingMappingVlaue))
                {
                    string[] value = ReferralSiblingMappingVlaue.Split('~');

                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] sblingsplitvalue = value[i].Split(';');
                        name = i == 0 ? sblingsplitvalue[0] : string.Format("{0},{1}", name, sblingsplitvalue[0]);
                    }
                }
                return name;
            }
        }


        public List<ListofReferralSiblingList> ReferralSiblingMapping
        {
            get
            {
                List<ListofReferralSiblingList> listofReferralSiblingList = new List<ListofReferralSiblingList>();

                if (!string.IsNullOrEmpty(ReferralSiblingMappingVlaue))
                {
                    string[] value = ReferralSiblingMappingVlaue.Split('~');

                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] sblingsplitvalue = value[i].Split(';');

                        listofReferralSiblingList.Add(new ListofReferralSiblingList
                        {
                            ClientName = sblingsplitvalue[0],
                            ReferralID = sblingsplitvalue[1],
                        });
                    }
                }
                return listofReferralSiblingList;
            }
        }


        public float NewRemainingHrs
        {
            //get { return ConfigSettings.RespiteUsageLimit - UsedRespiteHours; }
            get { return 40 - (UsedRespiteHours > 40 ? 40 : UsedRespiteHours); }
        }


        public float NewAllocatedHrs
        {
            //get { return ConfigSettings.RespiteUsageLimit - UsedRespiteHours; }
            get { return 40; }
        }

        public float NewUsedHrs
        {
            //get { return ConfigSettings.RespiteUsageLimit - UsedRespiteHours; }
            get { return UsedRespiteHours > 40 ? 40 : UsedRespiteHours; }
        }
    }

    public class ListofReferralSiblingList
    {
        public string ClientName { get; set; }
        public string ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
    }

    public class ReferralDetailForPopup
    {
        public ReferralDetailForSchedule ReferralDetail { get; set; }
        public List<ScheduleListForPopup> ScheduleList { get; set; }
    }


    public class EmployeeDetailForPopup
    {
        public EmployeeDetailForSchedule EmployeeDetail { get; set; }
        public EmployeeTimeSlotMaster ETSMaster { get; set; }
        public List<EmployeeTimeSlotDetail> ListETSDetail { get; set; }
        public List<EmployeePTO> ListEmployeePTO { get; set; }
        public List<Preference> SkillList { get; set; }
        public List<Preference> PreferenceList { get; set; }
        public List<PatientForEmployee> PatientList { get; set; }
    }

    public class PatientForEmployee
    {
        public long ReferralID { get; set; }
        public string PatientName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class EmployeeDetailForSchedule
    {
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(EmployeeID.ToString()); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        //public string EmployeeName
        //{
        //    get { return Common.GetEmpGeneralNameFormat(FirstName, LastName); }
        //}

        public string EmployeeUniqueID { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string FullAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(Address))
                    return String.Format("{0}, {1} - {2} {3}", Address, City, ZipCode, StateCode);

                return "";
            }
        }

    }

    public class EmployeePTO : EmployeeDayOff
    {

        public long ScheduleCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferralName
        {
            get { return Common.GetGenericNameFormat(FirstName, "", LastName); }
        }

        public TimeSpan ScheduleStartTime { get; set; }
        public TimeSpan ScheduleEndTime { get; set; }



    }



    public class ReferralDetailForSchedule
    {
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get { return Crypto.Encrypt(ReferralID.ToString()); } }
        public string PrimaryContactName { get; set; }
        public string ReferralName { get; set; }
        public string AHCCCSID { get; set; }
        public string CISNumber { get; set; }
        public bool NeedPrivateRoom { get; set; }
        public string PlacementRequirement { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string FrequencyCode { get; set; }
        public DateTime? LastAttendedDate { get; set; }
        public bool RespiteService { get; set; }
        public bool LifeSkillsService { get; set; }
        public bool CounselingService { get; set; }
        public DateTime? ZSPCounsellingExpirationDate { get; set; }
        public DateTime? ZSPLifeSkillsExpirationDate { get; set; }
        public DateTime? ZSPRespiteExpirationDate { get; set; }

    }

    public class ScheduleListForPopup
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ScheduleStatusName { get; set; }
        public string FacilityName { get; set; }
        public string EmployeeName { get; set; }
    }

    public class SearchScheduledPatientModel
    {
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public string PatientName { get; set; }
        public long FacilityID { get; set; }
    }

    public class SearchScheduleListByFacility
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> FacilityIDs { get; set; }
        public List<string> EmployeeIDs { get; set; }
        public List<string> ReferralIDs { get; set; }
        public long ReferralID { get; set; }
        public String EmployeeName { get; set; }
        public String ReferralName { get; set; }
        public String SchStatus { get; set; }
        public String IsScheduled { get; set; }



        public long FacilityID { get; set; }
        public long WeekMasterID { get; set; }
        public string ServiceTypeID { get; set; }
    }

    public class SearchRCLEmpRefSchOption
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public long FacilityID { get; set; }
        public string EmployeeName { get; set; }

        public long? MileRadius { get; set; }
        //public long? ToMile { get; set; }


        public long? SkillId { get; set; }
        public long? PreferenceId { get; set; }

        public string StrSkillList { get; set; }
        public string StrPreferenceList { get; set; }

        public long ScheduleID { get; set; }
        public string EmployeeTimeSlotDetailIDs { get; set; }
        public bool SameDateWithTimeSlot { get; set; }

        public string ReferralTimeSlotDetailIDs { get; set; }
        public bool IsRescheduleAction { get; set; }



        //Saurabh
        public string Days { get; set; }
        public string StartTimes { get; set; }
        public string EndTimes { get; set; }

        public long PayorID { get; set; }
        public long CareTypeID { get; set; }

        public long ReferralTSDateID { get; set; }
    }

    public class SearchEmpRefSchOption
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public long FacilityID { get; set; }
        public string EmployeeName { get; set; }

        public long? MileRadius { get; set; }
        //public long? ToMile { get; set; }


        public long? SkillId { get; set; }
        public long? PreferenceId { get; set; }

        public string StrSkillList { get; set; }
        public string StrPreferenceList { get; set; }

        public long ScheduleID { get; set; }
        public string EmployeeTimeSlotDetailIDs { get; set; }
        public bool SameDateWithTimeSlot { get; set; }

        public string ReferralTimeSlotDetailIDs { get; set; }
        public bool IsRescheduleAction { get; set; }



        //Saurabh
        public string Days { get; set; }
        public bool IsForcePatientSchedules { get; set; }

        public long PayorID { get; set; }
        public long CareTypeID { get; set; }

        public long ReferralTSDateID { get; set; }

        public long ReferralBillingAuthorizationID { get; set; }
        public bool IsVirtualVisit { get; set; }
    }

    public class GetRCLEmpRefSchOptionsModel
    {
        public List<EmployeeRCLModel> EmployeeTSList { get; set; }

        [Ignore]
        public Page<EmployeeRCLModel> Page { get; set; }
    }

    public class GetEmpRefSchOptions_PatientVisitFrequencyModel
    {
        public List<DDMaster> CareTypeList { get; set; }
        public List<PayorList> PatientPayorList { get; set; }
    }
    public class GetEmpRefSchOptions_ReferralInfoModel
    {
        public PatientDetail PatientDetail { get; set; }
        public ReferralTimeSlotMaster RTSMaster { get; set; }
        public List<ReferralTimeSlotDetail> ListRTSDetail { get; set; }
    }
    public class GetEmpRefSchOptions_ScheduleInfoModel
    {
        public List<EmployeeTSModel> EmployeeTSList { get; set; }
        public List<ReferralAuthorizationServiceCodeList> ReferralAuthorizationServiceCodeList { get; set; }
        [Ignore]
        public Page<EmployeeTSModel> Page { get; set; }
    }
    public class GetEmpRefSchOptions_ClientOnHoldDataModel
    {
        public List<PatientHoldDetail> PatientHoldDetailList { get; set; }
    }
    public class GetEmpRefSchOptionsModel
    {
        public List<DDMaster> CareTypeList { get; set; }
        public List<PayorList> PatientPayorList { get; set; }
        public List<PatientHoldDetail> PatientHoldDetailList { get; set; }
        public PatientDetail PatientDetail { get; set; }
        public ReferralTimeSlotMaster RTSMaster { get; set; }
        public List<ReferralTimeSlotDetail> ListRTSDetail { get; set; }
        public List<EmployeeTSModel> EmployeeTSList { get; set; }
        public List<ReferralAuthorizationServiceCodeList> ReferralAuthorizationServiceCodeList { get; set; }

        [Ignore]
        public Page<EmployeeTSModel> Page { get; set; }
    }


    public class ReferralBillingAuthorizatioSearchModel
    {
        public long PayorID { get; set; }
        public long ReferralID { get; set; }
    }

    public class ReferralBillingAuthorizatioModel
    {
        //public long ReferralBillingAuthorizationID { get; set; }
        //public string ReferralBillingAuthorizationName { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
        public long ReferralID { get; set; }
        //public string AuthorizationCode { get; set; }
        public string ReferralBillingAuthorizationName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public long? AllowedTime { get; set; }
        public int UnitType { get; set; }
        public int MaxUnit { get; set; }
        public int DailyUnitLimit { get; set; }
        public int UnitLimitFrequency { get; set; }
        public string ServiceCode { get; set; }
        public string CareType { get; set; }
    }


    public class DayCare_GetEmpRefSchOptionsModel
    {
        public List<FacilityModel> FacilityList { get; set; }
        public List<PayorList> PatientPayorList { get; set; }
        public CreateSchModel CreateSchModel { get; set; }

        public List<PatientHoldDetail> PatientHoldDetailList { get; set; }
        public PatientDetail PatientDetail { get; set; }
        public ReferralTimeSlotMaster RTSMaster { get; set; }
        public List<ReferralTimeSlotDetail> ListRTSDetail { get; set; }
        public List<CareTypeList> CareTypeList { get; set; }

    }
    public class CareTypeList
    {
        public long CareTypeID { get; set; }
        public string CareTypeName { get; set; }
    }

    public class CaseManagement_GetEmpRefSchOptionsModel
    {
        public List<PatientHoldDetail> PatientHoldDetailList { get; set; }
        public PatientDetail PatientDetail { get; set; }
    }
    public class CreateSchModel
    {
        public long PayorID { get; set; }
        public long FacilityID { get; set; }
        public long ScheduleID { get; set; }
        public long ReferralTSDateID { get; set; }
    }


    public class GetEmpRefSchPageModel
    {
        public GetEmpRefSchPageModel()
        {
            Preferences = new List<Preference>();
            Skills = new List<Preference>();
        }

        public List<Preference> Preferences { get; set; }
        public List<Preference> Skills { get; set; }
    }


    public class PatientDetail
    {

        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferralName { get; set; }

        //[Ignore]
        //public string ReferralName
        //{
        //    get { return Common.GetGeneralNameFormat(FirstName, LastName); }
        //}
        [Ignore]
        public string EncryptedReferralID
        {
            get { return Crypto.Encrypt(Convert.ToString(ReferralID)); }
        }

        public long PatientPayorID { get; set; }


        public long ScheduleID { get; set; }
        public long ReferralTSDateID { get; set; }
        public string EncryptedReferralTSDateID { get { return Crypto.Encrypt(Convert.ToString(ReferralTSDateID)); } }

        public string Title { get; set; }

        //public string ScheduledHours { get; set; }

        //public string AllowedTime { get; set; }

        //public string UsedHours { get; set; }

        //public string PendingHours { get; set; }

        //public string AllowedTimeType { get; set; }

    }

    public class EmployeeTSModel
    {

        public long EmployeeID { get; set; }
        public string EmployeeTimeSlotDetailIDs { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        //public string EmployeeName
        //{
        //    get { return Common.GetEmpGeneralNameFormat(FirstName, LastName); }
        //}

        public int Day { get; set; }

        public string Frequency { get; set; }



        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [Ignore]
        public string StrStartTime
        {
            get
            {

                if (StartTime.TotalMilliseconds > 0)
                {
                    DateTime time = DateTime.Today.Add(StartTime);
                    return time.ToString("hh:mm tt");
                }
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    StartTime = timeOnly.TimeOfDay;
                }
                else
                {
                    StartTime = DateTime.Now.TimeOfDay;
                }
            }
        }
        [Ignore]
        public string StrEndTime
        {
            get
            {
                if (EndTime.TotalMilliseconds > 0)
                {
                    DateTime time = DateTime.Today.Add(EndTime);
                    return time.ToString("hh:mm tt");
                }
                return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    DateTime timeOnly = DateTime.ParseExact(value, "hh:mm tt",
                                                            System.Globalization.CultureInfo.CurrentCulture);
                    EndTime = timeOnly.TimeOfDay;
                }
                else
                {
                    EndTime = DateTime.Now.TimeOfDay;
                }
            }
        }



        public int PreferencesMatchPercent { get; set; }
        public int SkillsMatchPercent { get; set; }
        public double? Distance { get; set; }

        public string StrDistance
        {
            get
            {
                if (Distance == null)
                    return Resource.NALbl;

                return String.Format("+{0}", Convert.ToString(Math.Floor(Distance.Value)));


            }
        }

        public int Conflicts { get; set; }


        public DateTime? ETMStartDate { get; set; }
        public DateTime? ETMEndDate { get; set; }


        public string Email { get; set; }
        public string PhoneWork { get; set; }

        public int Count { get; set; }
    }

    public class EmployeeRCLModel
    {

        public long EmployeeID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        //public string EmployeeName
        //{
        //    get { return Common.GetEmpGeneralNameFormat(FirstName, LastName); }
        //}

        public int PreferencesMatchPercent { get; set; }
        public int SkillsMatchPercent { get; set; }
        public double? Distance { get; set; }

        public string StrDistance
        {
            get
            {
                if (Distance == null)
                    return "0";

                return String.Format("+{0}", Convert.ToString(Math.Floor(Distance.Value)));


            }
        }
        public int Count { get; set; }
    }




    public class FacilityScheduleDetails
    {
        public List<ScheduleListByFacility> ScheduleList { get; set; }
        public List<DateWiseScheduleCount> DateWiseScheduleCountList { get; set; }
        public FacilityDetailForCalender Facility { get; set; }
    }


    public class EmployeeScheduleDetails
    {
        public List<ScheduleListByFacility> ScheduleList { get; set; }
        public List<DateWiseScheduleCount> DateWiseScheduleCountList { get; set; }
        //public Employee Employee { get; set; }
        //public List<EmployeeSchModel> EmployeeList { get; set; }
        //[Ignore]
        //public ScheduleSearchModel ScheduleSearchModel { get; set; }

    }


    public class Daycare_SavePatient_AttendecenModel
    {
        public Daycare_GetScheduledPatientList Daycare_GetScheduledPatientList { get; set; }
        public SearchScheduledPatientModel SearchScheduledPatientModel { get; set; }

    }


    public class DayCare_GetSchedulePatientTask
    {
        public long ReferralTaskMappingID { get; set; }
        public long VisitTaskID { get; set; }
        public string VisitTaskDetail { get; set; }
        public bool IsSelected { get; set; }
    }
    public class VisitTaskOptionList
    {
        public long VisitTaskID { get; set; }
        public long TaskOptionID { get; set; }
        public string TaskOption { get; set; }
        public string DefaultTaskOption { get; set; }
        public long ReferralID { get; set; }
    }

    public class DayCare_GetSchedulePatientTaskList
    {
        public DayCare_GetSchedulePatientTaskList()
        {

            //DayCare_GetSchedulePatientTask = new DayCare_GetSchedulePatientTask();
            DayCare_GetSchedulePatientTask = new List<DayCare_GetSchedulePatientTask>();
            VisitTaskOptionList = new List<VisitTaskOptionList>();

        }
        //public DayCare_GetSchedulePatientTask DayCare_GetSchedulePatientTask { get; set; }
        public List<DayCare_GetSchedulePatientTask> DayCare_GetSchedulePatientTask { get; set; }
        public List<VisitTaskOptionList> VisitTaskOptionList { get; set; }
    }


    public class Daycare_GetScheduledPatientList
    {


        public long ReferralID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatientName { get; set; }
        //public string PatientName
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}
        public string Dob { get; set; }

        public long ScheduleID { get; set; }
        public bool? IsPatientAttendedSchedule { get; set; }
        public bool? IsPatientNotAttendedSchedule
        {
            get
            {
                if (IsPatientAttendedSchedule.HasValue)
                {
                    return !IsPatientAttendedSchedule;
                }
                return IsPatientAttendedSchedule;

            }
        }
        public string PatientProfileImage { get; set; }
        public string AbsentReason { get; set; }
        public string FacilityName { get; set; }

        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }

        public string StrClockInTime
        {
            get
            {
                if (ClockInTime.HasValue)
                {
                    return ClockInTime.Value.ToString(Constants.DbTimeFormat);
                }
                else { return DateTime.Now.ToString(Constants.DbTimeFormat); }
            }
        }
        public string StrClockOutTime
        {

            get
            {
                if (ClockOutTime.HasValue)
                {
                    return ClockOutTime.Value.ToString(Constants.DbTimeFormat);
                }
                else { return DateTime.Now.ToString(Constants.DbTimeFormat); }
            }


        }

        public string ScheduleStartDate { get; set; }
        public string ScheduleEndDate { get; set; }
        public string PatientSignature_ClockIN { get; set; }
        public string PatientSignature_ClockOut { get; set; }
        public long EmployeeVisitID { get; set; }

        public bool IsClockInCompleted { get { return ClockInTime.HasValue ? true : false; } }
        public bool IsClockOutCompleted { get { return ClockOutTime.HasValue ? true : false; } }

        public string ReferralTaskMappingIDs { get; set; }

        public bool IsSelf { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string ProfileImagePath { get; set; }
        public string Attendence { get; set; }

    }

    public class ReferralScheduleDetails
    {
        public List<PatientScheduleList> ScheduleList { get; set; }
    }

    public class ADC_ReferralScheduleDetails
    {
        public List<ADC_PatientScheduleList> ScheduleList { get; set; }
    }


    public class ADC_PatientScheduleList
    {
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferralName { get; set; }
        //public string ReferralName
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}
        public string Email { get; set; }
        public string Phone1 { get; set; }

        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EmployeeName { get; set; }
        //public string EmployeeName
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}


        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityNPI { get; set; }



        public string EmpEmail { get; set; }
        public string EmpMobile { get; set; }
        public string EmployeeUniqueID { get; set; }


        public long ReferralID { get; set; }

        public long ScheduleID { get; set; }
        public long ScheduleStatusID { get; set; }

        public bool UnAllocated { get; set; }

        public bool UsedInScheduling { get; set; }
        public bool OnHold { get; set; }
        public bool IsDenied { get; set; }
        public string ScheduleComment { get; set; }



        public long ReferralTSDateID { get; set; }
        public long EmployeeTSDateID { get; set; }


        public string ScheduleStatusName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public bool? IsPatientAttendedSchedule { get; set; }

        [Ignore]
        public bool? TempIsPatientAttendedSchedule
        {
            get { return IsPatientAttendedSchedule; }
        }

        public string AbsentReason { get; set; }

        //public DateTime? ClockInTime { get; set; }
        public bool? IsPCACompleted { get; set; }
        public bool IsPendingSchProcessed { get; set; }

        [Ignore]
        public string StrStartTime
        {
            get
            {
                //03/03/2018 3:00 am
                return String.Format("{0:MM/dd/yyyy hh:mm tt}", StartDate);
            }
        }
        [Ignore]
        public string StrEndTime
        {
            get
            {
                return String.Format("{0:MM/dd/yyyy hh:mm tt}", EndDate);
            }
        }

        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public bool IVRClockIn { get; set; }
        public bool IVRClockOut { get; set; }
        //public bool IsPCACompleted { get; set; }
        public string EVVClockIn { get; set; }
        public string EVVClockOut { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string strClockInTime { get; set; }
        public string strClockOutTime { get; set; }
        public string Payor { get; set; }
        public string CareType { get; set; }
        public int CareTypeId { get; set; }
        public bool IsApprovalRequired { get; set; }
    }


    public class PatientScheduleList
    {
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ReferralName { get; set; }
        //public string ReferralName
        //{
        //    get { return Common.GetReferralNameFormat(FirstName, LastName); }
        //}
        public string Email { get; set; }
        public string Phone1 { get; set; }

        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EmployeeName { get; set; }
        //public string EmployeeName
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}


        public long FacilityID { get; set; }
        public string FacilityName { get; set; }
        public string FacilityNPI { get; set; }



        public string EmpEmail { get; set; }
        public string EmpMobile { get; set; }
        public string EmployeeUniqueID { get; set; }


        public long ReferralID { get; set; }

        public string ReferralIDEncrypted
        {
            get
            {
                return Crypto.Encrypt(ReferralID.ToString());
            }
        }

        public long ScheduleID { get; set; }
        public long ScheduleStatusID { get; set; }

        public bool UnAllocated
        {
            get { return ScheduleID == 0; }
        }

        public bool UsedInScheduling { get; set; }
        public bool OnHold { get; set; }
        public bool IsDenied { get; set; }
        public string ScheduleComment { get; set; }



        public long ReferralTSDateID { get; set; }
        public long EmployeeTSDateID { get; set; }


        public string ScheduleStatusName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public bool? IsPatientAttendedSchedule { get; set; }

        [Ignore]
        public bool? TempIsPatientAttendedSchedule
        {
            get { return IsPatientAttendedSchedule; }
        }

        public string AbsentReason { get; set; }

        //public DateTime? ClockInTime { get; set; }
        public bool? IsPCACompleted { get; set; }
        public bool IsPendingSchProcessed { get; set; }

        [Ignore]
        public string StrStartTime
        {
            get
            {
                //03/03/2018 3:00 am
                return String.Format("{0:MM/dd/yyyy hh:mm tt}", StartDate);
            }
        }
        [Ignore]
        public string StrEndTime
        {
            get
            {
                return String.Format("{0:MM/dd/yyyy hh:mm tt}", EndDate);
            }
        }

        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public bool IVRClockIn { get; set; }
        public bool IVRClockOut { get; set; }
        //public bool IsPCACompleted { get; set; }
        public string EVVClockIn { get; set; }
        public string EVVClockOut { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string strClockInTime { get; set; }
        public string strClockOutTime { get; set; }
        public string Payor { get; set; }
        public string CareType { get; set; }
        public int CareTypeId { get; set; }
        public bool IsApprovalRequired { get; set; }
        public bool IsVirtualVisit { get; set; }
        public bool IsBetween { get; set; }
        public bool IsPastVisit { get; set; }
        public long ReferralBillingAuthorizationID { get; set; }
    }

    public class ScheduleListByFacility : ScheduleMaster
    {
        public string DropOffLocationName { get; set; }
        public string PickUpLocationName { get; set; }
        public string ScheduleStatusName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int DefaultScheduleDays { get; set; }
        public string Age { get; set; }
        public bool NeedPrivateRoom { get; set; }
        public bool IsRefrralDeleted { get; set; }
        public string Comments { get; set; }

        public DateTime Dob { get; set; }

        public string ReferralSiblingMappingVlaue { get; set; }
        public string PlacementRequirement { get; set; }
        public string PrimaryContactName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public string CareTypeId { get; set; }
        public string CareType { get; set; }

        public List<ListofReferralSiblingList> ReferralSiblingMapping
        {
            get
            {
                List<ListofReferralSiblingList> listofReferralSiblingList = new List<ListofReferralSiblingList>();

                if (!string.IsNullOrEmpty(ReferralSiblingMappingVlaue))
                {
                    string[] value = ReferralSiblingMappingVlaue.Split('~');

                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] sblingsplitvalue = value[i].Split(';');

                        listofReferralSiblingList.Add(new ListofReferralSiblingList
                        {
                            ClientName = sblingsplitvalue[0],
                            ReferralID = sblingsplitvalue[1],
                        });
                    }
                }
                return listofReferralSiblingList;
            }
        }


        [Ignore]
        public string eventColor
        {
            get
            {
                return Common.GetRandomColor(Convert.ToString(EmployeeID));
            }
        }
        [Ignore]
        public string rendering
        {
            get
            {
                return "inverse-background";
            }
        }
    }

    public class FacilityDetailForCalender : Facility
    {
        public string PayorApproved { get; set; }
    }

    public class DateWiseScheduleCount
    {
        public DateTime Date { get; set; }
        public int TotalScheduleCount { get; set; }
        public int ConfirmedScheduleCount { get; set; }
        public int RequiredPrivateRoom { get; set; }
        public int ConfirmedPrivateRoom { get; set; }

    }

    public class Count
    {
        public int Counts { get; set; }
    }


    public class ClientCountAtUPLocationsModel
    {
        public string SchStartDate { get; set; }
        public int ClientCount { get; set; }
        public string Location { get; set; }
    }


    public class ClientCountAtDownLocationsModel
    {
        public string SchEndDate { get; set; }
        public int ClientCount { get; set; }
        public string Location { get; set; }


    }

    public class ClientCountAtLocationsListModel
    {
        public List<ClientCountAtUPLocationsModel> UpTripList { get; set; }
        public List<ClientCountAtDownLocationsModel> DownTripList { get; set; }

    }

    public class ScheduleNotificationLogModel
    {
        public string NotificationType { get; set; }
        public string CreatedBy { get; set; }
        [SetValueOnAdd((int)SetValueOnAddAttribute.SetValueEnum.CurrentTime)]
        [JsonConverter(typeof(BaseController.CustomUTCDateTimeConverter))]
        [PropertyBinder(typeof(JsonModelBinder))]
        public DateTime CreatedDate { get; set; }
        public string ToEmailAddress { get; set; }
        public string ToPhone { get; set; }

    }


    public class SearchEmpRefMatchModel
    {
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string MatchType { get; set; }
    }

    public class DeleteEmpRefScheduleModel
    {
        public long ReferralTimeSlotDetailID { get; set; }
        public long ReferralTimeSlotMasterID { get; set; }
        public int Day { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }


    public class PatientOnHoldModel
    {
        public PatientOnHoldModel()
        {
            notificationUserDetails = new List<DeniedNotificationUserDetails>();
        }
        public int Result { get; set; }
        public List<DeniedNotificationUserDetails> notificationUserDetails { get; set; }
    }

    public class DeniedNotificationUserDetails
    {
        public long EmployeeID { get; set; }
        public string FcmTokenId { get; set; }
        public string DeviceType { get; set; }
        public string PatientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }



    public class ScheduleAttendaceDetail
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public bool IsPatientAttendedSchedule { get; set; }
        public string AbsentReason { get; set; }
    }

    public class PatientHoldDetail
    {
        public long ReferralOnHoldDetailID { get; set; }
        public long ReferralID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? UnHoldDate { get; set; }
        public string PatientOnHoldReason { get; set; }


        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }


        public bool CurrentActiveGroup { get; set; }
        public bool OldActiveGroup { get; set; }

        public bool PatientOnHoldAction { get; set; }
        public bool NotifyEmployee { get; set; }

    }



    public class ReferralTimeSlotModel
    {
        public long ReferralTimeSlotDetailID { get; set; }
        public long ReferralTimeSlotMasterID { get; set; }
        public long ReferralID { get; set; }
        public int Day { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }


    public class GetAssignedEmployeeModel
    {
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(Convert.ToString(EmployeeID)); } }
        public string EmployeeName { get; set; }
        public string MobileNumber { get; set; }
    }


    public class GetAssignedFacilityModel
    {
        public long FacilityID { get; set; }
        public string EncryptedFacilityID { get { return Crypto.Encrypt(Convert.ToString(FacilityID)); } }
        public string FacilityName { get; set; }
        public string FacilityNPI { get; set; }

    }


    public class GetAssignedEmployee
    {
        public long EmployeeID { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(Convert.ToString(EmployeeID)); } }
        public string EmployeeName { get; set; }

        public string StrDayName { get; set; }
        public string MobileNumber { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetAssignedEmployeeGroup
    {
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public long ClickCount { get; set; }
        public string MobileNumber { get; set; }
        public string EncryptedEmployeeID { get; set; } //{get { return Crypto.Decrypt(Convert.ToString(EmployeeID)); } }
        public List<GetAssignedEmployee> GetAssignedEmployee { get; set; }
    }

    public class SchEmpRefModel
    {
        public SchEmpRefModel()
        {
            ListEmpRefPreferenceModel = new List<EmpRefPreferenceModel>();
            SearchEmpRefMatchModel = new SearchEmpRefMatchModel();
        }

        public List<EmpRefPreferenceModel> ListEmpRefPreferenceModel { get; set; }
        [Ignore]
        public SearchEmpRefMatchModel SearchEmpRefMatchModel { get; set; }

    }


    public class EmpRefPreferenceModel
    {
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string PreferenceName { get; set; }
        public bool IsMatched { get; set; }

    }

    public class RemoveScheduleModel
    {
        public long ScheduleID { get; set; }
        public string RemoveScheduleReason { get; set; }
        public bool IsSaveNoteOnly { get; set; }
        public long ReferralTSDateID { get; set; }
    }

    public class ChangeScheduleModel
    {
        public long ScheduleID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ReferralName { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public DateTime ScheduleDate { get; set; }
    }

    public class GetVisitReasonModel
    {
        public string Type { get; set; }
        public string CompanyName { get; set; }
    }

    public class VisitReasonModel
    {
        public long ScheduleID { get; set; }
        public string ReasonType { get; set; }
        public string ReasonCode { get; set; }
        public string ActionCode { get; set; }
        public string CompanyName { get; set; }
    }

    public class ReferralCsvModel
    {
        public long FacilityID { get; set; }
        public string FilePath { get; set; }
        public DateTime ScheduleDate { get; set; }
    }

    public class VisitReason
    {
        public long VisitReasonID { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
    }

    public class HC_SetEmployeeVisitsPage
    {
        public HC_SetEmployeeVisitsPage()
        {
            EmployeeList = new List<EmployeeListModel>();
            SearchReferralEmployeeModel = new SearchReferralEmployeeModel();
        }
        public List<EmployeeListModel> EmployeeList { get; set; }

        [Ignore]
        public SearchReferralEmployeeModel SearchReferralEmployeeModel { get; set; }
        [Ignore]
        public SaveEmployeeVisitsTransportLog SaveEmployeeVisitsTransportLog { get; set; }

    }

    public class SearchReferralEmployeeModel
    {
        public SearchReferralEmployeeModel()
        {
            TransportationType = 2;
        }
        public long EmployeeID { get; set; }
        public DateTime SlotDate { get; set; }
        public int? TransportationType { get; set; }
    }
    public class GetReferralEmployeeVisitsModel
    {
        public GetReferralEmployeeVisitsModel() { }
        public long ReferralID { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public long? TransportGroupID { get; set; }
        public long? TransportAssignPatientID { get; set; }
        public long? EmployeeVisitsTransportLogId { get; set; }
        public long? EmployeeVisitsTransportLogDetailId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string OtherPhone { get; set; }
        public string Email { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
    }

    public class SaveEmployeeVisitsTransportLog
    {
        public SaveEmployeeVisitsTransportLog() { }
        public long EmployeeID { get; set; }
        public long? TransportGroupID { get; set; }
        public long? TransportAssignPatientID { get; set; }
        public long? EmployeeVisitsTransportLogId { get; set; }
        public long? EmployeeVisitsTransportLogDetailId { get; set; }

        public long ReferralID { get; set; }

        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? TransportationType { get; set; }
    }
}
