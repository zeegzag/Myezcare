using PetaPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Infrastructure.Utility.eBriggsForms;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class EmployeeModel
    {
        public long EmployeeID { get; set; }
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class AddEmployeeModel
    {
        public AddEmployeeModel()
        {
            Employee = new Employee();
            DepartmentList = new List<Department>();
            SecurityQuestionList = new List<SecurityQuestion>();
            RoleList = new List<Role>();
            CredentialList = new List<EmployeeCredential>();
            AmazonSettingModel = new AmazonSettingModel();
        }
        public Employee Employee { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<SecurityQuestion> SecurityQuestionList { get; set; }
        public List<Role> RoleList { get; set; }
        public List<EmployeeCredential> CredentialList { get; set; }
        public bool IsEditMode { get; set; }
        public AmazonSettingModel AmazonSettingModel { get; set; }
        [Ignore]
        public string TempSignaturePath { get; set; }
    }

    public class HC_AddEmployeeModel
    {
        public HC_AddEmployeeModel()
        {
            Employee = new Employee();
            DepartmentList = new List<Department>();
            SecurityQuestionList = new List<SecurityQuestion>();
            RoleList = new List<Role>();
            CredentialList = new List<EmployeeCredential>();
            AmazonSettingModel = new AmazonSettingModel();
            PreferenceList = new List<EmployeePreferenceModel>();
            StateList = new List<State>();
            SkillList = new List<Preference>();
            EmployeeSkillList = new List<string>();
            ReferralDocument = new ReferralDocument();
            // EmployeeDesignation = new List<NameValueData>();
            DocumentTypeList = new List<DocumentTypeModel>();
            CareTypeList = new List<CareType>();
            DesignationList = new List<NameValueData>();
            EmployeeChecklistModel = new EmployeeChecklist();
            EmployeeNotificationPrefsModel = new EmployeeNotificationPrefsModel();
            OrgTypeList = new List<NameValueDataInString>();

            LanguageList = new List<Language>();
            GenderList = new List<NameValueDataInString>();
            ContactTypeList = new List<ContactType>();
            ContactInformationList = new List<AddAndListContactInformation>();
            AddAndListContactInformation = new AddAndListContactInformation();
            AddContactModel = new AddAndListContactInformation();
            Contact = new Contact();
            ContactMapping = new ContactMapping();
            EmergencyContactList = new List<NameValueDataBoolean>();
            NoticeProviderOnFileList = new List<NameValueDataBoolean>();
            ReferenceMaster = new ReferenceMaster();
            SetEmployeeBillingReportListPage = new
                SetEmployeeBillingReportListPage();
            FacilityList = new List<FacilityModel>();
            Organization = new Organization();
        }
        public Employee Employee { get; set; }
        public List<Department> DepartmentList { get; set; }
        public List<SecurityQuestion> SecurityQuestionList { get; set; }
        public List<Role> RoleList { get; set; }
        public List<EmployeeCredential> CredentialList { get; set; }
        public List<State> StateList { get; set; }
        public List<EmployeePreferenceModel> PreferenceList { get; set; }
        public List<Preference> SkillList { get; set; }
        public List<string> EmployeeSkillList { get; set; }
        [Ignore]
        public EmployeeChecklist EmployeeChecklistModel { get; set; }
        [Ignore]
        public EmployeeNotificationPrefsModel EmployeeNotificationPrefsModel { get; set; }
        [Ignore]
        public string StrEmployeeSkillList { get; set; }
        [Ignore]
        public bool IsEditMode { get; set; }
        [Ignore]
        public AmazonSettingModel AmazonSettingModel { get; set; }
        [Ignore]
        public string TempSignaturePath { get; set; }
        [Ignore]
        public ReferralDocument ReferralDocument { get; set; }
        //[Ignore]
        //public List<NameValueData> EmployeeDesignation { get; set; }
        public List<DocumentTypeModel> DocumentTypeList { get; set; }
        public List<CareType> CareTypeList { get; set; }
        public List<NameValueData> DesignationList { get; set; }
        public List<NameValueData> EmployeeGroupList { get; set; }
        [Ignore]
        public List<NameValueDataInString> OrgTypeList { get; set; }

        public List<Language> LanguageList { get; set; }
        public List<NameValueDataInString> GenderList { get; set; }
        public List<ContactType> ContactTypeList { get; set; }
        public List<AddAndListContactInformation> ContactInformationList { get; set; }
        public AddAndListContactInformation AddAndListContactInformation { get; set; }
        public AddAndListContactInformation AddContactModel { get; set; }
        public Contact Contact { get; set; }
        public ContactMapping ContactMapping { get; set; }
        public List<NameValueDataBoolean> EmergencyContactList { get; set; }
        public List<NameValueDataBoolean> NoticeProviderOnFileList { get; set; }
        public ReferenceMaster ReferenceMaster { get; set; }
        public SetEmployeeBillingReportListPage SetEmployeeBillingReportListPage { get; set; }
        public List<FacilityModel> FacilityList { get; set; }
        public Organization Organization { get; set; }
    }
    public class Organization
    {
        public long OrganizationID { get; set; }
        public string SiteLogo { get; set; }
        public string SiteName { get; set; }
        public string SiteBaseUrl { get; set; }
        public string FavIcon { get; set; }
        public string TwilioFromNo { get; set; }
    }

    public class ReferenceMaster
    {
        public long ReferenceID { get; set; }
        public bool IsActive { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceCode { get; set; }
    }
    public class EmployeeTokens
    {
        public string HomeCareLogoImage { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string IvrCode { get; set; }
        public string IvrPin { get; set; }
        public string ResetPasswordLink { get; set; }
        public string SiteName { get; set; }
    }







    public class HC_EmpCalenderModel
    {
        public HC_EmpCalenderModel()
        {
            EmployeeList = new List<EmployeeSchModel>();
            SearchEmpCalender = new SearchEmpCalender();
        }

        public List<EmployeeSchModel> EmployeeList { get; set; }

        [Ignore]
        public SearchEmpCalender SearchEmpCalender { get; set; }
        [Ignore]
        public bool IsPartial { get; set; }
    }



    public class HC_RefCalenderModel
    {
        public HC_RefCalenderModel()
        {
            ReferralList = new List<ReferralSchModel>();
            EmployeeList = new List<EmployeeSchModel>();
            SearchRefCalender = new SearchRefCalender();
        }


        public List<ReferralSchModel> ReferralList { get; set; }

        public List<EmployeeSchModel> EmployeeList { get; set; }

        [Ignore]
        public SearchRefCalender SearchRefCalender { get; set; }
        [Ignore]
        public bool IsPartial { get; set; }
    }

    public class SearchRefCalender
    {
        public List<string> EmployeeID { get; set; }
        public string EmployeeIDs { get; set; }


        public List<string> ReferralID { get; set; }
        public string ReferralIDs { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<string> PayorID { get; set; }
        public string PayorIDs { get; set; }

        public List<string> CareTypeID { get; set; }
        public string CareTypeIDs { get; set; }
    }


    public class SearchEmpCalender
    {
        public List<string> EmployeeID { get; set; }
        public string EmployeeIDs { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class HC_ETSModel
    {
        public HC_ETSModel()
        {
            EmployeeList = new List<NameValueData>();
            ETSMaster = new EmployeeTimeSlotMaster();
            ETSDetail = new EmployeeTimeSlotDetail();
            SearchETSMaster = new SearchETSMaster();
            SearchETSDetail = new SearchETSDetail();
            ETSDetailBulk = new ETSDetailBulk();
        }
        public List<NameValueData> EmployeeList { get; set; }
        //public List<NameValueData> EmployeeList { get; set; }
        [Ignore]
        public ETSDetailBulk ETSDetailBulk { get; set; }

        [Ignore]
        public EmployeeTimeSlotMaster ETSMaster { get; set; }
        [Ignore]
        public EmployeeTimeSlotDetail ETSDetail { get; set; }

        [Ignore]
        public SearchETSMaster SearchETSMaster { get; set; }

        [Ignore]
        public SearchETSDetail SearchETSDetail { get; set; }


        [Ignore]
        public bool IsPartial { get; set; }

        [Ignore]
        public List<NameValueData> WeekDaysList { get { return Common.SetWeekDays(); } }
    }
    public class ETSDetailBulk
    {
        public long EmployeeTimeSlotMasterID { get; set; }

        [Required(ErrorMessageResourceName = "EmployeeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long EmployeeID { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        public bool IsEndDateAvailable { get; set; }

        public bool IsDeleted { get; set; }
        public long EmployeeTimeSlotDetailID { get; set; }
        // public long EmployeeTimeSlotMasterID { get; set; }

        public DateTime ScheduleDate { get; set; }

        //[Required(ErrorMessageResourceName = "DayRequired", ErrorMessageResourceType = typeof(Resource))]
        public int Day { get; set; }

        [Ignore]
        public string StrDayName
        {
            get
            {
                return Day == 0 ? string.Empty : Common.SetWeekDays().First(c => c.Value == Day).Name;
            }
        }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; }

        //  public bool IsDeleted { get; set; }

        [Ignore]
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessageResourceName = "InvalidTimeMsg", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "StartTimeRequired", ErrorMessageResourceType = typeof(Resource))]
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
        //^(hh:mm tt)$
        [RegularExpression(Constants.RegxTimeFormat, ErrorMessageResourceName = "InvalidTimeMsg", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "EndTimeRequired", ErrorMessageResourceType = typeof(Resource))]
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

        [Ignore]
        [Required(ErrorMessageResourceName = "DayRequired", ErrorMessageResourceType = typeof(Resource))]
        public int[] SelectedDays { get; set; }

        [ResultColumn]
        public long RemainingSlotCount { get; set; }

        // [Ignore]
        //  public long EmployeeID { get; set; }
    }


    public class SearchETSMaster
    {
        public long EmployeeID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }

    public class SearchETSDetail
    {
        public long EmployeeTimeSlotMasterID { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }


    public class ListETSMaster
    {
        public long EmployeeTimeSlotMasterID { get; set; }
        public long EmployeeID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndDateAvailable { get; set; }
        public int TotalETSDetailCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool ActiveStat { get; set; }



        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public int Count { get; set; }

    }

    public class ListETSDetails
    {
        public long EmployeeTimeSlotDetailID { get; set; }
        public long EmployeeTimeSlotMasterID { get; set; }
        public long EmployeeID { get; set; }
        public int Day { get; set; }
        public string DayName
        {
            get
            {
                return Day == 0 ? string.Empty : Common.SetWeekDays().First(c => c.Value == Day).Name;
            }
        }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string Notes { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "StartTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrStartTime
        {
            get
            {

                if (StartTime.HasValue)
                {
                    DateTime time = DateTime.Today.Add(StartTime.Value);
                    return time.ToString("hh:mm tt");
                }
                return "";
            }

        }


        [Ignore]
        [Required(ErrorMessageResourceName = "EndTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrEndTime
        {
            get
            {
                if (EndTime.HasValue)
                {
                    DateTime time = DateTime.Today.Add(EndTime.Value);
                    return time.ToString("hh:mm tt");
                }
                return "";
            }

        }



        public bool IsDeleted { get; set; }
        public bool AllDay { get; set; }
        public bool Is24Hrs { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }


        public int Count { get; set; }

    }




    #region Home Care

    #region Form Model


    public class OrganizationFormModel
    {
        public long OrganizationFormID { get; set; }
        public string EBFormID { get; set; }
    }

    public class OrganizationForm
    {
        public long OrganizationFormID { get; set; }
        public string OrganizationFriendlyFormName { get; set; }
    }

    public class UDT_EBFromMappingTable
    {
        public long EbriggsFormMppingID { get; set; }
        public string EBriggsFormID { get; set; }
        public string OriginalEBFormID { get; set; }
        public string FormId { get; set; }
        public long ReferralID { get; set; }
        public string PatientName { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

    }

    public class SearchFormModel
    {
        public long MarketID { get; set; }
        public long FormCategoryID { get; set; }
        public string FormNumber { get; set; }
        public string FormName { get; set; }
        public long IsDeleted { get; set; }
        public string ListOfIdsInCSV { get; set; }

        [RegularExpression(Constants.RegxAllow2DecimalPlacesOnly, ErrorMessageResourceName = "InvalidPrice", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "PriceRequired", ErrorMessageResourceType = typeof(Resource))]
        public decimal FormPrice { get; set; }

        [Required(ErrorMessageResourceName = "FormNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FormLongName { get; set; }

        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
    }


    public class OrganizationFormPageModel
    {
        public OrganizationFormPageModel()
        {
            MarketList = new List<MarketModel>();
            FormCategoryList = new List<CategoryModel>();
            FormList = new List<FormListModel>();
            OrganizationFormList = new List<FormListModel>();
            SearchFormModel = new SearchFormModel();
            ConfigEBFormModel = new ConfigEBFormModel();
        }

        public List<MarketModel> MarketList { get; set; }
        public List<CategoryModel> FormCategoryList { get; set; }
        public List<FormListModel> FormList { get; set; }
        public List<FormListModel> OrganizationFormList { get; set; }

        [Ignore]
        public ConfigEBFormModel ConfigEBFormModel { get; set; }
        [Ignore]
        public SearchFormModel SearchFormModel { get; set; }

        [Ignore]
        public bool IsPartialPage { get; set; }

    }


    public class FormPageModel
    {
        public FormPageModel()
        {
            MarketList = new List<MarketModel>();
            FormCategoryList = new List<CategoryModel>();
            SearchFormModel = new SearchFormModel();
            ConfigEBFormModel = new ConfigEBFormModel();
            ReferralList = new List<NameValueData>();
            EmployeeList = new List<NameValueData>();
        }

        public List<MarketModel> MarketList { get; set; }
        public List<CategoryModel> FormCategoryList { get; set; }

        [Ignore]
        public ConfigEBFormModel ConfigEBFormModel { get; set; }
        [Ignore]
        public SearchFormModel SearchFormModel { get; set; }

        [Ignore]
        public List<NameValueData> ReferralList { get; set; }
        [Ignore]
        public List<NameValueData> EmployeeList { get; set; }


        [Ignore]
        public bool IsPartial { get; set; }

        [Ignore]
        public bool ForEmployee { get; set; }

        [Ignore]
        public bool ForPatient { get; set; }
    }


    public class GetPatientEmpInfoModel
    {
        public GetPatientEmpInfoModel()
        {
            ReferralList = new List<NameValueData>();
            EmployeeList = new List<NameValueData>();
        }

        public List<NameValueData> ReferralList { get; set; }
        public List<NameValueData> EmployeeList { get; set; }

    }




    public class MarketModel
    {
        public string EBMarketID { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }


    public class CategoryModel
    {
        public string EBCategoryID { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class FormListModel
    {
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormLongName { get; set; }
        public bool IsActive { get; set; }
        public bool HasHtml { get; set; }
        public string NewHtmlURI { get; set; }
        public bool HasPDF { get; set; }
        public string NewPdfURI { get; set; }
        public string OriginalFormName { get; set; }
        public string FriendlyFormName { get; set; }
        public string Tags { get; set; }

        public decimal? FormPrice { get; set; }


        public string EBCategoryID { get; set; }


        public string EbMarketIDs { get; set; }
        public List<string> EbMarketIDList
        {
            get
            {
                return new List<string>(EbMarketIDs.Split(','));
            }
        }

        public string FormCategory { get; set; }
        public string FormMarkets { get; set; }
        public bool IsDeleted { get; set; }


        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }

        public long OrganizationFormID { get; set; }

        public bool IsNewForm { get; set; }

        public int Count { get; set; }
        public bool IsOrbeonForm { get; set; }
    }



    public class SavedFormListModel
    {
        public string EBFormID { get; set; }
        public string FormId { get; set; }
        public string Id { get; set; }

        public string Name { get; set; }
        public string NameForUrl { get; set; }
        public string Version { get; set; }
        public string FormLongName { get; set; }
        public bool IsActive { get; set; }
        public bool HasHtml { get; set; }
        public string NewHtmlURI { get; set; }
        public bool HasPDF { get; set; }
        public string NewPdfURI { get; set; }

        public decimal? FormPrice { get; set; }


        public string EBCategoryID { get; set; }


        public string EbMarketIDs { get; set; }
        public List<string> EbMarketIDList
        {
            get
            {
                return new List<string>(EbMarketIDs.Split(','));
            }
        }

        public string FormCategory { get; set; }
        public string FormMarkets { get; set; }
        public bool IsDeleted { get; set; }

        public long OrganizationFormID { get; set; }

        public bool IsNewForm { get; set; }

        public int Count { get; set; }


        public long EbriggsFormMppingID { get; set; }
        public string EBriggsFormID { get; set; }
        public string OriginalEBFormID { get; set; }
        public long ReferralID { get; set; }
        public string PatientName { get; set; }
        public long EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string SavedFormCreatedBy { get; set; }
        public DateTime SavedFormCreatedDate { get; set; }
        public string SavedFormUpdatedBy { get; set; }
        public DateTime SavedFormUpdatedDate { get; set; }

        public string TEMP_FormLongName { get; set; }
        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }
        public bool IsOrbeonForm { get; set; }

    }



    #endregion




    public class HC_EBFormModel
    {
        public HC_EBFormModel()
        {
            ReferralList = new List<NameValueData>();
            EmployeeList = new List<NameValueData>();
            SearchEbForm = new SearchEbForm();
            ConfigEBFormModel = new ConfigEBFormModel();
        }

        public List<NameValueData> ReferralList { get; set; }
        public List<NameValueData> EmployeeList { get; set; }

        [Ignore]
        public SearchEbForm SearchEbForm { get; set; }

        [Ignore]
        public bool IsPartial { get; set; }

        [Ignore]
        public bool ForEmployee { get; set; }

        [Ignore]
        public bool ForPatient { get; set; }

        [Ignore]
        public List<FormApi_MarketModel> MarketList { get; set; }

        [Ignore]
        public List<FormApi_FormModel> FormList { get; set; }

        [Ignore]
        public ConfigEBFormModel ConfigEBFormModel { get; set; }



        [Ignore]
        public List<FormApi_FormCategory> FormCategoryList
        {
            get
            {

                List<FormApi_FormCategory> tempList = new List<FormApi_FormCategory>();
                foreach (var item in FormList)
                {
                    if (item.FormCategory != null && string.IsNullOrEmpty(item.FormCategory.Id) == false)
                    {
                        if (tempList.Count(c => c.Id == item.FormCategory.Id) == 0)
                            tempList.Add(item.FormCategory);
                    }
                }
                return tempList.OrderBy(c => c.Name).ToList();
            }
        }






    }

    public class SearchEbForm
    {
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public long MarketID { get; set; }
        public string MarketName { get; set; }
        public string FormNumber { get; set; }
        public string FormName { get; set; }
        public long FormCategoryID { get; set; }
        public string FormCategory { get; set; }
    }

    public class ConfigEBFormModel
    {
        public string EBBaseSiteUrl
        {
            get { return ConfigSettings.EbriggsUrl; }
        }
        public string ResuName
        {
            get { return ConfigSettings.EbriggsUserName; }
        }
        public string ResuKey
        {
            get { return ConfigSettings.EbriggsPassword; }
        }
        public string MyezcareFormsUrl
        {
            get { return ConfigSettings.MyezcareFormsUrl; }
        }
    }

    public class SaveNewEBFormModel
    {
        public long? EmployeeID { get; set; }
        public long? ReferralID { get; set; }
        public string UserType { get; set; }
        public string EBriggsFormID { get; set; }
        public string OriginalEBFormID { get; set; }
        public string FormId { get; set; }
        public string HTMLFormContent { get; set; }

        public long? SubSectionID { get; set; }

        public long EbriggsFormMppingID { get; set; }
        public bool IsEditMode { get; set; }
        public string FormName { get; set; }
        public bool UpdateFormName { get; set; }
        public string InternalFilePath { get; set; }
        public string Version { get; set; }
        public string OrgPageID { get; set; }


    }

    public class DuplicateOrbeonForm
    {
        public string NameForUrl { get; set; }
        public string DocumentID { get; set; }
    }

    [System.Xml.Serialization.XmlRoot("document-id")]
    public class DuplicateOrbeonFormDocument
    {
        [System.Xml.Serialization.XmlText()]
        public string DocumentID { get; set; }
    }



    public class HC_RTSModel
    {
        public HC_RTSModel()
        {
            ReferralList = new List<NameValueData>();
            RTSMaster = new ReferralTimeSlotMaster();
            RTSDetail = new ReferralTimeSlotDetail();
            SearchRTSMaster = new SearchRTSMaster();
            SearchRTSDetail = new SearchRTSDetail();
            CareTypeList = new List<CareType>();
            SearchReferralTimeSlotDetail = new SearchReferralTimeSlotDetail();
            AssigneeList = new List<EmployeeDropDownModel>();

        }

        public List<EmployeeDropDownModel> AssigneeList { get; set; }
        public List<NameValueData> ReferralList { get; set; }
        public List<CareType> CareTypeList { get; set; }
        [Ignore]
        public ReferralTimeSlotMaster RTSMaster { get; set; }
        [Ignore]
        public ReferralTimeSlotDetail RTSDetail { get; set; }

        [Ignore]
        public SearchRTSMaster SearchRTSMaster { get; set; }

        [Ignore]
        public SearchRTSDetail SearchRTSDetail { get; set; }
        [Ignore]
        public SearchReferralTimeSlotDetail SearchReferralTimeSlotDetail { get; set; }

        [Ignore]
        public bool IsPartial { get; set; }

        [Ignore]
        public List<NameValueData> WeekDaysList { get { return Common.SetWeekDays(); } }
    }

    public class SearchRTSMaster
    {
        public long ReferralID { get; set; }
        public long CareTypeID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public string Filter { get; set; }
        public int IsDeleted { get; set; }


    }
    public class SearchReferralTimeSlotDetail
    {
        public long ReferralID { get; set; }
        public long ReferralTimeSlotMasterID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ClientName { get; set; }

        //Change Employee
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int ScheduleID { get; set; }
    }

    public class SearchRTSDetail
    {
        public long ReferralTimeSlotMasterID { get; set; }
        public long CareTypeId { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }

    public class DeleteRTSDetail
    {
        public long TransactionResultId { get; set; }
        public List<ListRTSMaster> Data { get; set; }
    }

    public class ListRTSMaster
    {
        public long ReferralTimeSlotMasterID { get; set; }
        public long ReferralTimeSlotDetailID { get; set; }
        public long? ReferralBillingAuthorizationID { get; set; }
        public long ReferralID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEndDateAvailable { get; set; }
        public int TotalRTSDetailCount { get; set; }
        public bool IsDeleted { get; set; }
        public bool ActiveStat { get; set; }
        //public string AllowedTime { get; set; }
        //public string ScheduledHours { get; set; }
        //public string UsedHours { get; set; }
        //public string PendingHours { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public long CareTypeID { get; set; }
        public string CareType { get; set; }
        public string AuthorizationCode { get; set; }
        public string ServiceCode { get; set; }
        public bool IsWithPriorAuth { get; set; }
        public int Count { get; set; }
        public string DayName { get; set; }

    }
    public class ListReferralTimeSlotDetail
    {
        public long ReferralID { get; set; }
        public long ReferralTimeSlotMasterID { get; set; }
        public DateTime ReferralTSDate { get; set; }
        public DateTime ReferralTSStartTime { get; set; }
        public DateTime ReferralTSEndTime { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int ReferralTSDateID { get; set; }
        public int ScheduleID { get; set; }
        public int ScheduleStatusID { get; set; }
        public DateTime? EndDate { get; set; }
        public int DayNumber { get; set; }
        public string DayName { get; set; }
        public int Count { get; set; }

    }



    public class ListRTSDetails
    {
        public long ReferralTimeSlotDetailID { get; set; }
        public long ReferralTimeSlotMasterID { get; set; }
        public long ReferralID { get; set; }
        public int Day { get; set; }
        public string DayName
        {
            get
            {
                return Day == 0 ? string.Empty : Common.SetWeekDays().First(c => c.Value == Day).Name;
            }
        }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool UsedInScheduling { get; set; }
        public string Notes { get; set; }

        [Ignore]
        [Required(ErrorMessageResourceName = "StartTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrStartTime
        {
            get
            {

                //if (StartTime.TotalMilliseconds > 0)
                //{
                DateTime time = DateTime.Today.Add(StartTime);
                return time.ToString("hh:mm tt");
                //}
                //return "";
            }

        }


        [Ignore]
        [Required(ErrorMessageResourceName = "EndTimeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string StrEndTime
        {
            get
            {
                //if (EndTime.TotalMilliseconds > 0)
                //{
                DateTime time = DateTime.Today.Add(EndTime);
                return time.ToString("hh:mm tt");
                //}
                //return "";
            }

        }



        public bool IsDeleted { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public long? CareTypeId { get; set; }
        public long? ReferralBillingAuthorizationID { get; set; }
        public string Title { get; set; }
        public bool AnyTimeClockIn { get; set; }
        public int Count { get; set; }
        public bool isChecked { get; set; }
        public string AuthorizationCode { get; set; }

    }


    #endregion

    #region Referral Case Load

    public class ListRCLMaster
    {
        public long ReferralCaseLoadID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string CaseLoadType { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }

        public int Count { get; set; }

        [Ignore]
        public DateTime CustomizedMinStartDate
        {
            get
            {
                if (ReferralCaseLoadID > 0 && CaseLoadType == Common.CaseLoadTypeEnum.Temporary.ToString() && StartDate <= DateTime.Today)
                {
                    return DateTime.Today;
                }
                return StartDate;
            }
        }

        [Ignore]
        public bool AllowedToEditStartDate
        {
            get
            {
                if (ReferralCaseLoadID > 0 && CaseLoadType == Common.CaseLoadTypeEnum.Temporary.ToString())
                {
                    return StartDate > DateTime.Today;
                }
                return false;
            }
        }

        [Ignore]
        public bool IsDeleteAllowed
        {
            get
            {
                return (CaseLoadType == Common.CaseLoadTypeEnum.Permanent.ToString() && EndDate == null)
                    || (CaseLoadType == Common.CaseLoadTypeEnum.Temporary.ToString() && StartDate >= DateTime.Today);
            }
        }

        [Ignore]
        public bool IsEditAllowed
        {
            get
            {
                return CaseLoadType == Common.CaseLoadTypeEnum.Temporary.ToString() && EndDate >= DateTime.Today;
            }
        }
    }

    public class SearchRCLMaster
    {
        public long ReferralID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public string CaseLoadType { get; set; }
    }

    #endregion











    public class EmployeePreferenceModel
    {
        public long EmployeePreferenceID { get; set; }
        public long EmployeeID { get; set; }
        public long PreferenceID { get; set; }
        public string PreferenceName { get; set; }
    }

    public class ListEmployeeModel
    {
        public string EmployeeUniqueID { get; set; }
        public long EmployeeID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public long RoleID { get; set; }
        public string CredentialName { get; set; }
        public string Degree { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDepartmentSupervisor { get; set; }
        public bool IsSecurityQuestionSubmitted { get; set; }
        public string EncryptedEmployeeID { get { return Crypto.Encrypt(Convert.ToString(EmployeeID)); } }
        public int Row { get; set; }
        public int Count { get; set; }

        public string EmployeeSignatureID { get; set; }


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


        public string Certification
        {
            get
            {

                if (EmployeeID % 8 == 1)
                    return "Caregiver Core Certification";
                if (EmployeeID % 8 == 2)
                    return "Dementia Care Certification";
                if (EmployeeID % 8 == 3)
                    return "Developmental Disabilities Certification";
                if (EmployeeID % 8 == 4)
                    return "Diabetes Care Certification";
                if (EmployeeID % 8 == 5)
                    return "End of Life Care Certification";
                if (EmployeeID % 8 == 6)
                    return "Mental Health Care Certification";
                if (EmployeeID % 8 == 7)
                    return "Multiple Sclerosis Care Certification";

                return "Personal Care Aide Certification";

            }
        }
        public string ExpireDate
        {
            get
            {

                //if (EmployeeID % 8 == 1)
                //    return Convert.ToDateTime("09/15/2019").ToString(Common.GetOrgDateFormat());
                //if (EmployeeID % 8 == 2)
                //    return Convert.ToDateTime("08/02/2019").ToString(Common.GetOrgDateFormat());
                //if (EmployeeID % 8 == 3)
                //    return Convert.ToDateTime("09/25/2018").ToString(Common.GetOrgDateFormat());
                //if (EmployeeID % 8 == 4)
                //    return Convert.ToDateTime("06/15/2018").ToString(Common.GetOrgDateFormat());
                //if (EmployeeID % 8 == 5)
                //    return Convert.ToDateTime("04/27/2019").ToString(Common.GetOrgDateFormat());
                //if (EmployeeID % 8 == 6)
                //    return Convert.ToDateTime("06/17/2019").ToString(Common.GetOrgDateFormat());
                //if (EmployeeID % 8 == 7)
                //    return Convert.ToDateTime("12/29/2019").ToString(Common.GetOrgDateFormat());

                //return Convert.ToDateTime("12/13/2020").ToString(Common.GetOrgDateFormat());



                if (EmployeeID % 8 == 1)
                    return Common.ConvertToOrgDateFormat(DateTime.ParseExact("09/15/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture));
                if (EmployeeID % 8 == 2)
                    return Common.ConvertToOrgDateFormat(DateTime.ParseExact("08/02/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture));
                if (EmployeeID % 8 == 3)
                    return Common.ConvertToOrgDateFormat(DateTime.ParseExact("09/25/2018", "MM/dd/yyyy", CultureInfo.InvariantCulture)); //Common.ConvertToOrgDateFormat(Convert.ToDateTime("09/25/2018"));
                if (EmployeeID % 8 == 4)
                    return Common.ConvertToOrgDateFormat(DateTime.ParseExact("06/15/2018", "MM/dd/yyyy", CultureInfo.InvariantCulture)); 
                if (EmployeeID % 8 == 5)
                    return Common.ConvertToOrgDateFormat(DateTime.ParseExact("04/27/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture));
                if (EmployeeID % 8 == 6)
                    return Common.ConvertToOrgDateFormat(DateTime.ParseExact("06/17/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture));
                if (EmployeeID % 8 == 7)
                    return Common.ConvertToOrgDateFormat(DateTime.ParseExact("12/29/2019", "MM/dd/yyyy", CultureInfo.InvariantCulture));

                return Common.ConvertToOrgDateFormat(DateTime.ParseExact("12/13/2020", "MM/dd/yyyy", CultureInfo.InvariantCulture));

            }
        }
        public string FirstName { get; set; }
        [Ignore]
        public string FirstNameChar
        {
            get
            {
                if (!string.IsNullOrEmpty(FirstName))
                {
                    return Convert.ToString(FirstName.Substring(0, 1).ToUpper());
                }
                else
                {
                    return null;
                }
            }
        }
        public string ProfileImagePath { get; set; }
        public string Designation { get; set; }

        public bool IsAbleToReceiveNotification { get; set; }
    }


    public class SearchSentSmsModel
    {
        public string Message { get; set; }
        public string NotificationSID { get; set; }

    }


    public class SentSMSListModel
    {
        public long GroupMessageLogID { get; set; }
        public string EmployeeIDs { get; set; }
        public string Message { get; set; }
        public string NotificationSID { get; set; }

        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string SentBy { get; set; }

        //public string SentBy
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}
        public DateTime SentDate { get; set; }
        public string StrSentDate { get; set; }
        public int Row { get; set; }
        public int Count { get; set; }

    }

    public class SearchEmployeeModel
    {
        public string EmployeeUniqueID { get; set; }

        public long DepartmentID { get; set; }

        [StringLength(100, ErrorMessageResourceName = "EmailLength", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [StringLength(100, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        public string UserName { get; set; }

        public int IsSupervisor { get; set; }

        public long RoleID { get; set; }

        public string CredentialID { get; set; }
        public string Degree { get; set; }

        public string Employee { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }


        public string Address { get; set; }

        public string Certificate { get; set; }

        public string ExpirationDate { get; set; }

        public string MobileNumber { get; set; }

        public int EmployeeId { get; set; }
        public long DesignationID { get; set; }
        public List<NameValueData> EmployeeList { get; set; }

        [Ignore]
        public EmployeeTimeSlotMaster ETSMaster { get; set; }
        [Ignore]
        public EmployeeTimeSlotDetail ETSDetail { get; set; }

        [Ignore]
        public SearchETSMaster SearchETSMaster { get; set; }

        [Ignore]
        public SearchETSDetail SearchETSDetail { get; set; }


        [Ignore]
        public bool IsPartial { get; set; }

        [Ignore]
        public List<NameValueData> WeekDaysList { get { return Common.SetWeekDays(); } }

        public string[] GroupIds { get; set; }

    }


    public class SetEmployeeListPage
    {
        public SetEmployeeListPage()
        {
            RoleList = new List<RoleDropDownModel>();
            CredentialList = new List<EmployeeCredential>();
            DepartmentDropdownList = new List<DepartmentDropdownModel>();
            SearchEmployeeModel = new SearchEmployeeModel();
            DepartmentSupervisorStatusList = new List<NameValueData>();
            DeleteFilter = new List<NameValueData>();
            DesignationList = new List<DesignationList>();
        }

        public List<RoleDropDownModel> RoleList { get; set; }
        public List<EmployeeCredential> CredentialList { get; set; }
        public List<DepartmentDropdownModel> DepartmentDropdownList { get; set; }
        public SearchEmployeeModel SearchEmployeeModel { get; set; }
        public List<NameValueData> DepartmentSupervisorStatusList { get; set; }
        public List<NameValueData> DeleteFilter { get; set; }
        public List<DesignationList> DesignationList { get; set; }
    }


    public class DesignationList
    {
        public long DesignationID { get; set; }
        public string DesignationName { get; set; }
    }


    public class RoleDropDownModel
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
    }



    public class HTMLFormTokenReplaceModel
    {
        public string MEMBER_NAME { get; set; }
        public string MEMBER_LAST_NAME { get; set; }
        public string MEMBER_DOB { get; set; }
        public string MEMBER_ACCOUNT { get; set; }

    }









    #region Employee Day Off Models

    public class SearchEmpDayOffModel
    {
        public string Employee { get; set; }
        public long EmployeeID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string ListOfIdsInCsv { get; set; }
        public int IsDeleted { get; set; }
    }

    public class SetEmpDayOffListPage
    {
        public SetEmpDayOffListPage()
        {
            EmployeeList = new List<EmployeeModel>();
            SearchEmployeeDayOffModel = new SearchEmpDayOffModel();
            DeleteFilter = new List<NameValueData>();

        }

        public List<EmployeeModel> EmployeeList { get; set; }

        [Ignore]
        public EmployeeDayOff EmployeeDayOff { get; set; }

        [Ignore]
        public SearchEmpDayOffModel SearchEmployeeDayOffModel { get; set; }
        [Ignore]
        public List<NameValueData> DeleteFilter { get; set; }
        [Ignore]
        public List<NameValueData> DayOffTypes { get; set; }
        [Ignore]
        public bool IsPartial { set; get; }
    }



    public class ListEmpDayOffModel : EmployeeDayOff
    {
        public string EmployeeName { get; set; }
        public string ActionTakenByName { get; set; }
        public string SubmittedBy { get; set; }



        public string EncryptedEmployeeDayOffID { get { return Crypto.Encrypt(Convert.ToString(EmployeeDayOffID)); } }



        public int Row { get; set; }
        public int Count { get; set; }

    }

    #endregion

    #region Employee Notes Model
    public class EmployeeNotesModel
    {
        public long CommonNoteID { get; set; }
        public long EmployeeID { get; set; }
        public long ReferralID { get; set; }
        public string Note { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CommonList
    {
        public string Title { get; set; }
        public string Value { get; set; }

    }
    public class EmployeeEmail
    {
        public string Email { get; set; }
    }

    public class EmailSignature
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }

    #endregion






    #region Send Bulk SMS Models

    public class HC_SendBulkSmsModel
    {
        public HC_SendBulkSmsModel()
        {
            SearchSBSEmployeeModel = new SearchSBSEmployeeModel();
            SendSMSModel = new SendSMSModel();
            PatientModel = new PatientModel();
        }

        public SendSMSModel SendSMSModel { get; set; }
        public SearchSBSEmployeeModel SearchSBSEmployeeModel { get; set; }
        public PatientModel PatientModel { get; set; }
        [Ignore]
        public bool IsModelDetailBind { get; set; }
        [Ignore]
        public string ScheduleNotificationMessageContent { get; set; }
    }


    public class SearchSBSEmployeeModel
    {
        public string EmployeeName { get; set; }
        public string MobileNumber { get; set; }
        public string EncryptedId { get; set; }
        public string ScheduleNotificationAction { get; set; }
        public long NotificationId { get; set; }
    }

    public class SendSMSModel
    {
        public string Message { get; set; }
        public string EmployeeIds { get; set; }
        public int NotificationType { get; set; }
        public string EncryptedId { get; set; }
    }

    public class PatientModel
    {
        public long ReferralID { get; set; }
        public long ReferralTsDateID { get; set; }
        public string PatientName { get; set; }
        public string AHCCCSID { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? ReferralTSStartTime { get; set; }
        public DateTime? ReferralTSEndTime { get; set; }
        public string EncryptedId { get { return ReferralTsDateID != 0 ? Crypto.Encrypt(Convert.ToString(ReferralTsDateID)) : string.Empty; } }
        public string PreferenceNames { get; set; }
    }

    public class TwilioNotifyModel
    {
        public string binding_type { get; set; }
        public string address { get; set; }
    }






    public class GetSBSEmployeeList
    {
        public string EmployeeID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }

        //public string EmployeeName
        //{
        //    get { return Common.GetEmpGeneralNameFormat(FirstName, LastName); }
        //}

        public string EncryptedEmployeeID
        {
            get { return Crypto.Encrypt(EmployeeID); }
        }

        public string MobileNumber { get; set; }
        public bool IsAbleToReceiveNotification { get; set; }
        public bool WebNotification { get; set; }
        public bool MobileAppNotification { get; set; }

    }

    #endregion

    public class BroadcastNotificationListModel
    {
        public long NotificationId { get; set; }

        public string EmployeeIDs { get; set; }

        public string Title { get; set; }

        public int NotificationType { get; set; }

        [Ignore]
        public string StrNotificationType
        {
            get { return EnumHelper<Mobile_Notification.NotificationTypes>.GetDisplayValue((Mobile_Notification.NotificationTypes)NotificationType); }
        }

        public int InProgress { get; set; }

        [Ignore]
        public string StrInProgress
        {
            get { return EnumHelper<Mobile_Notification.NotificationStatuses>.GetDisplayValue((Mobile_Notification.NotificationStatuses)InProgress); }
        }

        public DateTime SentDate { get; set; }

        public string EmpFirstName { get; set; }

        public string EmpLastName { get; set; }
        public string SentBy { get; set; }

        //public string SentBy
        //{
        //    get { return Common.GetEmpGeneralNameFormat(EmpFirstName, EmpLastName); }
        //}

        public int Row { get; set; }

        public int Count { get; set; }
    }

    public class PDFFieldMapping
    {
        public string PDFFieldName { get; set; }
        public string DBFieldName { get; set; }
        public string DBValue { get; set; }
    }
    public class GetEmpListByRoleId
    {
        public string EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleID { get; set; }
        public string EployeesID { get; set; }
    }

    public class SelectValueModel
    {
        public string Value { get; set; }
        public string Title { get; set; }
    }
    public class RegularHoursModel
    {
        public long EmployeeID { get; set; }
        public decimal? RegularHours { get; set; }
        public int? RegularHourType { get; set; }
        public double RegularPayHours { get; set; }
        public double OvertimePayHours { get; set; }
        //[Ignore]
        //public double OvertimeHours { get; set; }
    }
    public class DisplayEmailSignature
    {
        public string EmailSignature { get; set; }

    }
    public class ReferralDoucmnetAttachmentModel
    {
        public string PatientName { get; set; }
        public string DOB { get; set; }

        public string CaseManager { get; set; }
        public string Assignee { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class RefDocument
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string StoreType { get; set; }
        public string GoogleFileId { get; set; }
    }

}
