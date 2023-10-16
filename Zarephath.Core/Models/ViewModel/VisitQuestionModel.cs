using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Models.Entity;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class AddVisitTaskModel
    {
        public AddVisitTaskModel()
        {
            VisitTask = new VisitTask();
            //ListServiceCode = new List<NameValueDataInString>();
            VisitTypeList = new List<NameValueStringData>();
            TaskFrequencyCodeList = new List<NameValueData>();
            //Category = new Category();
            //Category = new Category();
            MappedFormList = new List<TaskFormMappingModel>();
            ComplianceList = new List<NameValueData>();
            TaskOptionList = new List<NameValueData>();
        }
        public VisitTask VisitTask { get; set; }
        // public List<NameValueDataInString> ListServiceCode { get; set; }
        public List<NameValueStringData> VisitTypeList { get; set; }
        public List<NameValueData> TaskFrequencyCodeList { get; set; }
        public List<TaskFormMappingModel> MappedFormList { get; set; }
        public List<NameValueData> ComplianceList { get; set; }
        [Ignore]
        public List<NameValueDataInString> VisitTaskTypes { get; set; }
        [Ignore]
        public ConfigEBFormModel ConfigEBFormModel { get; set; }
        [Ignore]
        public Category Category { get; set; }
        [Ignore]
        public bool IsEditMode { get; set; }
        public List<NameValueData> TaskOptionList { get; set; }
    }

    public class TaskFormMappingModel
    {
        public long TaskFormMappingID { get; set; }
        public long VisitTaskID { get; set; }
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public string Name { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormLongName { get; set; }
        public long ComplianceID { get; set; }
        public bool IsRequired { get; set; }
        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }
        public bool IsOrbeonForm { get; set; }
    }

    public class Category
    {
        public long VisitTaskCategoryID { get; set; }

        [Required(ErrorMessageResourceName = "TaskTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string VisitTaskType { get; set; }
        [Required(ErrorMessageResourceName = "TaskTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SubVisitTaskType { get; set; }

        [Required(ErrorMessageResourceName = "CategoryNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CategoryName { get; set; }
        [Required(ErrorMessageResourceName = "SubCategoryNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SubCategoryName { get; set; }

        [Required(ErrorMessageResourceName = "CategoryNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public long? ParentCategoryLevel { get; set; }
    }

    public class SearchCategory
    {
        public string Type { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public bool IsCategoryList { get; set; }
    }

    public class SetVisitTaskListPage
    {
        public SetVisitTaskListPage()
        {
            ListServiceCode = new List<NameValueDataInString>();
            SearchVisitTaskListPage = new SearchVisitTaskListPage();
            DeleteFilter = new List<NameValueData>();
            VisitTaskTypes = new List<NameValueDataInString>();
            VisitTaskCategory = new List<VisitTaskCategory>();
            VisitTypeList = new List<NameValueData>();
            CareTypeList = new List<NameValueData>();
        }
        public List<NameValueDataInString> ListServiceCode { get; set; }
        public List<VisitTaskCategory> VisitTaskCategory { get; set; }
        public List<NameValueData> VisitTypeList { get; set; }
        public List<NameValueData> CareTypeList { get; set; }
        [Ignore]
        public SearchVisitTaskListPage SearchVisitTaskListPage { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public List<NameValueDataInString> VisitTaskTypes { get; set; }
    }

    public class SearchVisitTaskListPage
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string VisitTaskType { get; set; }
        public string ReferralName { get; set; }
        public string SiteLogo { get; set; }
        public string OrgInfo { get; set; }
        public string AboutInfo { get; set; }
        public string Dose { get; set; }
        public string Route { get; set; }
        public string PatientInstructions { get; set; }
        public string Day { get; set; }
        public string Goal { get; set; }
        public string GoalIDs { get; set; }
        public string VisitTaskDetail { get; set; }
        public long ServiceCodeID { get; set; }
        public string ServiceCode { get; set; }
        public long? VisitTaskCategoryID { get; set; }
        public long? VisitTaskVisitTypeID { get; set; }
        public long? VisitTaskCareTypeID { get; set; }
        public long CareTypeID { get; set; }
        public int IsDeleted { get; set; }
        public int IsActive { get; set; }
        public int GoalID { get; set; }
        public string ListOfIdsInCsv { get; set; }

        public string EncryptedReferralID { get; set; }
        public long ReferralID { get; set; }

        public int TargetCareType { get; set; }
        public int TargetServiceCode { get; set; }
        public string VisitTaskCategoryType { get; set; }
        public string VisitTaskCategoryName { get; set; }
        public int ParentCategoryLevel { get; set; }
        public int VisitTaskID { get; set; }
        public string Days { get; set; }
        public string Frequency { get; set; }
        public string Comment { get; set; }
        public bool IsDefault { get; set; }
        public string Submitter_NM103_NameLastOrOrganizationName { get; set; }
        public string BillingProvider_N301_Address { get; set; }
        public string BillingProvider_N401_City { get; set; }
        public string BillingProvider_N402_State { get; set; }
        public string BillingProvider_N403_Zipcode { get; set; }

    }

    public class OrgInfoReport
    {
        public string SiteLogo { get; set; }
        public string OrgInfo { get; set; }

    }
    public class RefInfoReport
    {
        public string ReferralName { get; set; }

    }

    public class AddressInfoReport
    {
        public string AddressInfo { get; set; }

    }

    public class AboutAndAddressInfoReport
    {
        public string AboutInfo { get; set; }
        public string AddressInfo { get; set; }

    }

    public class AboutInfoReport
    {
        public string AboutInfo { get; set; }

    }

    public class GoalInfoReport
    {
        public long GoalID { get; set; }
        public string Goal { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class MedicationInfoReport
    {
        public string MedicationName { get; set; }
        public string Dose { get; set; }
        public string Route { get; set; }
        public string Frequency { get; set; }
        public string PatientInstructions { get; set; }


    }
    public class TaskCodeInfoReport
    {
        public string Day { get; set; }
        public string VisitTaskDetail { get; set; }
    }
    public class TaskCodeInfoMonReport
    {
        public string Day1 { get; set; }
        public string VisitTaskDetail1 { get; set; }
    }

    public class TaskCodeInfoTueReport
    {
        public string Day2 { get; set; }
        public string VisitTaskDetail2 { get; set; }
    }

    public class TaskCodeInfoWedReport
    {
        public string Day3 { get; set; }
        public string VisitTaskDetail3 { get; set; }
    }

    public class TaskCodeInfoThuReport
    {
        public string Day4 { get; set; }
        public string VisitTaskDetail4 { get; set; }
    }

    public class TaskCodeInfoFriReport
    {
        public string Day5 { get; set; }
        public string VisitTaskDetail5 { get; set; }
    }

    public class TaskCodeInfoSatReport
    {
        public string Day6 { get; set; }
        public string VisitTaskDetail6 { get; set; }
    }

    public class TaskCodeInfoSunReport
    {
        public string Day7 { get; set; }
        public string VisitTaskDetail7 { get; set; }
    }

    public class ServicePlanInfoReport
    {
        public string VisitTaskCategoryName { get; set; }
        public string VisitTaskDetail { get; set; }

        public long VisitTaskCategoryID { get; set; }
    }
    public class SearchModelForCareTypeList
    {
        public long VisitTypeID { get; set; }
    }

    public class RefVisitTaskModel
    {
        public long ReferralTaskMappingID { get; set; }
        public long VisitTaskID { get; set; }
        public bool IsRequired { get; set; }
        public long ReferralID { get; set; }
        public string EncryptedReferralID { get; set; }
        public long CareTypeID { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public string VisitTaskDetail { get; set; }
        public string Frequency { get; set; }
        public string Days { get; set; }
        public string Comment { get; set; }
        public string Goal { get; set; }
        public bool toggle { get; set; }
    }

    public class TaskModel
    {
        public long ReferralTaskMappingID { get; set; }
        public string Frequency { get; set; }
        public string Comment { get; set; }
    }


    public class ListVisitTaskModel
    {
        public string VisitTaskID { get; set; }
        public string VisitTaskType { get; set; }
        public string VisitTaskDetail { get; set; }
        public string EncryptedVisitTaskID { get { return Crypto.Encrypt(Convert.ToString(VisitTaskID)); } }
        public string VisitTaskCategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public bool IsDefault { get; set; }
        public bool IsRequired { get; set; }
        public long? MinimumTimeRequired { get; set; }

        public bool IsDeleted { get; set; }

        public string ServiceCode { get; set; }
        public string CareType { get; set; }
        public string VisitType { get; set; }
        public long CareTypeID { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }
    }

    public class RefVisitTaskListModel
    {
        public RefVisitTaskListModel()
        {
            PatientTaskList = new List<ReferralTaskMappingModel>();
            PatientConclusionList = new List<ReferralTaskMappingModel>();
            TaskFrequencyCodeList = new List<NameValueData>();
        }

        public List<ReferralTaskMappingModel> PatientTaskList { get; set; }
        public List<ReferralTaskMappingModel> PatientConclusionList { get; set; }
        public List<NameValueData> TaskFrequencyCodeList { get; set; }

    }
    public class ReferralTaskMappingDetailsModel
    {
        public ReferralTaskMappingDetailsModel()
        {
            RefMappedList = new List<SearchVisitTaskListPage>();
            RefUnMappeddList = new List<SearchVisitTaskListPage>();
        }

        public List<SearchVisitTaskListPage> RefMappedList { get; set; }
        public List<SearchVisitTaskListPage> RefUnMappeddList { get; set; }

    }

    public class ReferralTaskMappingReportModel
    {
        public ReferralTaskMappingReportModel()
        {
            OrgInfo = new List<OrgInfoReport>();
            ReferralInfo = new List<RefInfoReport>();
            //AboutInfo = new AboutInfoReport();
            //AddressInfo = new AddressInfoReport();
            AboutAndAddressInfo = new List<AboutAndAddressInfoReport>();
            MedicationInfo = new List<MedicationInfoReport>();
            ServicePlanInfo = new List<ServicePlanInfoReport>();
            TaskCodes1 = new List<TaskCodeInfoMonReport>();
            TaskCodes2 = new List<TaskCodeInfoTueReport>();
            TaskCodes3 = new List<TaskCodeInfoWedReport>();
            TaskCodes4 = new List<TaskCodeInfoThuReport>();
            TaskCodes5 = new List<TaskCodeInfoFriReport>();
            TaskCodes6 = new List<TaskCodeInfoSatReport>();
            TaskCodes7 = new List<TaskCodeInfoSunReport>();
            GoalInfo = new List<GoalInfoReport>();

        }

        public List<OrgInfoReport> OrgInfo { get; set; }
        public List<RefInfoReport> ReferralInfo { get; set; }
        //public AboutInfoReport AboutInfo { get; set; }
        //public AddressInfoReport AddressInfo { get; set; }
        public List<AboutAndAddressInfoReport> AboutAndAddressInfo { get; set; }

        public List<MedicationInfoReport> MedicationInfo { get; set; }
        public List<ServicePlanInfoReport> ServicePlanInfo { get; set; }
        public List<TaskCodeInfoMonReport> TaskCodes1 { get; set; }
        public List<TaskCodeInfoTueReport> TaskCodes2 { get; set; }
        public List<TaskCodeInfoWedReport> TaskCodes3 { get; set; }
        public List<TaskCodeInfoThuReport> TaskCodes4 { get; set; }
        public List<TaskCodeInfoFriReport> TaskCodes5 { get; set; }
        public List<TaskCodeInfoSatReport> TaskCodes6 { get; set; }
        public List<TaskCodeInfoSunReport> TaskCodes7 { get; set; }
        public List<GoalInfoReport> GoalInfo { get; set; }
    }
    public class ReferralTaskMappingModel
    {
        public long ReferralTaskMappingID { get; set; }
        public long VisitTaskID { get; set; }
        public string VisitTaskDetail { get; set; }
        public string VisitTaskCategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string Frequency { get; set; }
        public string Comment { get; set; }
        public bool IsRequired { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CareTypeID { get; set; }
        public string CareType { get; set; }
        public string VisitTaskType { get; set; }

        public int Count { get; set; }

    }

    public class MapFormModel
    {
        public long VisitTaskID { get; set; }
        public string EBFormIDs { get; set; }
    }

    public class VisitTaskCategoryModel
    {
        public long VisitTaskCategoryID { get; set; }
        public string VisitTaskCategoryName { get; set; }
    }


}