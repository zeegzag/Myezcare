using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Myezcare_Admin.Resources;
using PetaPoco;
using System.Web.Mvc;

namespace Myezcare_Admin.Models.ViewModel
{
    #region Organization Models

    public class MyEzcareOrganizationModel
    {
        public MyEzcareOrganizationModel()
        {
            MyEzcareOrganization = new MyEzcareOrganization();
        }
        public MyEzcareOrganization MyEzcareOrganization { get; set; }
    }

    public class SetAddOrganizationModel
    {
        public SetAddOrganizationModel()
        {
            MyEzcareOrganization = new MyEzcareOrganization();
            OrganizationTypeList = new List<NameValueDataInString>();
        }
        public MyEzcareOrganization MyEzcareOrganization { get; set; }
        public List<NameValueDataInString> OrganizationTypeList { get; set; }
    }

    public class OrganizationListModel
    {
        public long OrganizationID { get; set; }
        public long OrganizationEsignID { get; set; }
        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public string DomainName { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }
        public string WorkPhone { get; set; }


        public long OrganizationTypeID { get; set; }
        public string OrganizationTypeName { get; set; }

        public long OrganizationStatusID { get; set; }
        public string OrganizationStatusName { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public int Row { get; set; }
        public int Count { get; set; }

        public string EncryptedOrganizationID { get { return OrganizationID > 0 ? Crypto.Encrypt(Convert.ToString(OrganizationID)) : ""; } }
        public string EncryptedOrganizationEsignID { get { return OrganizationEsignID > 0 ? Crypto.Encrypt(Convert.ToString(OrganizationEsignID)) : ""; } }
    }

    public class SearchOrganizationModel
    {
        public string OrganizationType { get; set; }
        public long OrganizationTypeID { get; set; }
        public long OrganizationStatusID { get; set; }

        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
        public string DomainName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long IsDeleted { get; set; }
        public string ListOfIdsInCsv { get; set; }
    }

    public class SetOrganizationListModel
    {
        public SetOrganizationListModel()
        {
            OrganizationTypeList = new List<NameValueData>();
            OrganizationStatusList = new List<NameValueData>();
            SearchOrganizationModel = new SearchOrganizationModel();
            ActiveFilter = new List<NameValueData>();
            AddOrganizationModel = new AddOrganizationModel();
        }


        public List<NameValueData> OrganizationTypeList { get; set; }
        public List<NameValueData> OrganizationStatusList { get; set; }

        [Ignore]
        public SearchOrganizationModel SearchOrganizationModel { get; set; }

        [Ignore]
        public List<NameValueData> ActiveFilter { get; set; }


        [Ignore]
        public AddOrganizationModel AddOrganizationModel { get; set; }
    }

    public class AddOrganizationModel
    {

        [Required(ErrorMessageResourceName = "CompanyNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CompanyName { get; set; }
        [Required(ErrorMessageResourceName = "DisplayNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DisplayName { get; set; }

        public string DomainName { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "MobileNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(Resource))]
        public string Mobile { get; set; }


        [Required(ErrorMessageResourceName = "WorkPhoneRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidWorkPhone", ErrorMessageResourceType = typeof(Resource))]
        public string WorkPhone { get; set; }

        [Required(ErrorMessageResourceName = "OrganizationTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long OrganizationTypeID { get; set; }


        public string DefaultEsignTerms { get; set; }


    }


    public class SetOrganizationEsignModel
    {
        public SetOrganizationEsignModel()
        {
            ServiceTypeList = new List<NameValueDataInString>();
            OrganizationTypeList = new List<NameValueDataInString>();
            OrganizationDetails = new OrganizationEsignDetails();
            ServicePlans = new List<OrganizationEsignSelectPlan>();
            ServicePlanComponents = new List<ServicePlanComponentModel>();
            TransactionResult = new TransactionResult();
        }
        public List<NameValueDataInString> ServiceTypeList { get; set; }
        public List<NameValueDataInString> OrganizationTypeList { get; set; }
        public OrganizationEsignDetails OrganizationDetails { get; set; }
        public List<OrganizationEsignSelectPlan> ServicePlans { get; set; }
        public List<ServicePlanComponentModel> ServicePlanComponents { get; set; }
        public TransactionResult TransactionResult { get; set; }
    }

    public class OrganizationEsignSelectPlan
    {
        public long ServicePlanID { get; set; }
        public string ServicePlanName { get; set; }
        public float PerPatientPrice { get; set; }
        public double? SetupFees { get; set; }
        public int NumberOfDaysForBilling { get; set; }
        public int Patient { get; set; }
        public int Facility { get; set; }
        public int Task { get; set; }
        public int Employee { get; set; }
        public int Billing { get; set; }
        public bool IsSelected { get; set; }
    }

    public class OrganizationEsignModel
    {
        public OrganizationEsignModel()
        {
            OrganizationDetails = new OrganizationEsignDetails();
            ServicePlans = new List<OrganizationEsignSelectPlan>();
            ServicePlanComponents = new List<ServicePlanComponentModel>();
            ServiceTypeList = new List<NameValueDataInString>();
            OrganizationTypeList = new List<NameValueDataInString>();
            TransactionResult = new TransactionResult();
        }
        public OrganizationEsignDetails OrganizationDetails { get; set; }
        public List<OrganizationEsignSelectPlan> ServicePlans { get; set; }
        public List<ServicePlanComponentModel> ServicePlanComponents { get; set; }
        public List<NameValueDataInString> OrganizationTypeList { get; set; }
        public List<NameValueDataInString> ServiceTypeList { get; set; }
        public TransactionResult TransactionResult { get; set; }
    }

    public class OrganizationEsignDetails
    {
        public long OrganizationEsignID { get; set; }
        public long OrganizationID { get; set; }

        [Required(ErrorMessageResourceName = "CompanyNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "DisplayNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DisplayName { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "MobileNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceName = "WorkPhoneRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidWorkPhone", ErrorMessageResourceType = typeof(Resource))]
        public string WorkPhone { get; set; }

        [Required(ErrorMessageResourceName = "EsignTermsRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DefaultEsignTerms { get; set; }

        [Required(ErrorMessageResourceName = "OrganizationTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long OrganizationTypeID { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsInProcess { get; set; }
    }

    public class OrganizationEsignToken
    {
        public string DisplayName { get; set; }
        public string EsignUrl { get; set; }
        public string LogoImage { get; set; }
    }

    #endregion



    #region Form Model

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
    }








    public class FormPageModel
    {
        public FormPageModel()
        {
            MarketList=new List<MarketModel>();
            FormCategoryList=new List<CategoryModel>();
            SearchFormModel = new SearchFormModel();
            ConfigEBFormModel = new ConfigEBFormModel();
        }

        public List<MarketModel> MarketList { get; set; }
        public List<CategoryModel> FormCategoryList { get; set; }

        [Ignore]
        public ConfigEBFormModel ConfigEBFormModel { get; set; }
        [Ignore]
        public SearchFormModel SearchFormModel { get; set; }

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

        public decimal? FormPrice { get; set; }
        public string FormCategory { get; set; }
        public string FormMarkets { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsInternalForm { get; set; }
        public string InternalFormPath { get; set; }

        public int Count { get; set; }
    }



    #endregion




    public class SetCustomerEsignModel
    {
        public SetCustomerEsignModel()
        {
            CustomerEsignDetails = new CustomerEsignDetails();
            OrganizationSettingDetails = new OrganizationSettingDetails();
            ServicePlans = new List<OrganizationEsignSelectPlan>();
            ServicePlanComponents = new List<ServicePlanComponentModel>();
            TransactionResult = new TransactionResult();
            IsSuccess = false;
            OrganizationFormPageModel = new OrganizationFormPageModel();
        }
        public CustomerEsignDetails CustomerEsignDetails { get; set; }
        public OrganizationSettingDetails OrganizationSettingDetails { get; set; }
        public List<OrganizationEsignSelectPlan> ServicePlans { get; set; }
        public List<ServicePlanComponentModel> ServicePlanComponents { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public bool IsSuccess { get; set; }
        public OrganizationFormPageModel OrganizationFormPageModel { get; set; }
    }

    public class CustomerEsignModel
    {
        public CustomerEsignModel()
        {
            CustomerEsignDetails = new CustomerEsignDetails();
            OrganizationSettingDetails = new OrganizationSettingDetails();
            ServicePlans = new List<OrganizationEsignSelectPlan>();
            ServicePlanComponents = new List<ServicePlanComponentModel>();
            TransactionResult = new TransactionResult();
        }
        public CustomerEsignDetails CustomerEsignDetails { get; set; }
        public OrganizationSettingDetails OrganizationSettingDetails { get; set; }
        public List<OrganizationEsignSelectPlan> ServicePlans { get; set; }
        public List<ServicePlanComponentModel> ServicePlanComponents { get; set; }
        public TransactionResult TransactionResult { get; set; }
    }

    public class CustomerEsignDetails
    {
        public long OrganizationEsignID { get; set; }
        public long OrganizationID { get; set; }

        [Required(ErrorMessageResourceName = "CompanyNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string CompanyName { get; set; }

        [Required(ErrorMessageResourceName = "DisplayNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DisplayName { get; set; }

        [Remote("ValidateDomain", "Organization", ErrorMessageResourceName = "DomainNameExists", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"[A-Za-z0-9](?:[A-Za-z0-9\-]{0,61}[A-Za-z0-9])?", ErrorMessageResourceName = "EnterValidSubdomainName", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "DomainNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DomainName { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "MobileNumberRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceName = "WorkPhoneRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "InvalidWorkPhone", ErrorMessageResourceType = typeof(Resource))]
        public string WorkPhone { get; set; }

        [Required(ErrorMessageResourceName = "EsignNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EsignName { get; set; }

        public string DefaultEsignTerms { get; set; }
    }

    public class OrganizationSettingDetails
    {
        public OrganizationSettingDetails()
        {
            TwilioCountryCode = "US";
        }

        public long OrganizationSettingID { get; set; }
        public long OrganizationID { get; set; }

        public bool IsSMTPSettingsEntered { get; set; }

        //[Required(ErrorMessageResourceName = "NetworkHostRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NetworkHost { get; set; }

        //[Required(ErrorMessageResourceName = "NetworkPortRequired", ErrorMessageResourceType = typeof(Resource))]
        public string NetworkPort { get; set; }

        //[Required(ErrorMessageResourceName = "FromTitleRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FromTitle { get; set; }

        //[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string FromEmail { get; set; }

        //[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FromEmailPassword { get; set; }

        //[Required(ErrorMessageResourceName = "EnableSSLRequired", ErrorMessageResourceType = typeof(Resource))]
        public bool EnableSSL { get; set; }

        public bool IsTwilioSettingsEntered { get; set; }

        public string TwilioCountryCode { get; set; }
        public string TwilioLocation { get; set; }
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

    }

    #region Upload Excel 

    public class ImportDataModel
    {
        public ImportDataModel()
        {
            ImportDataTypeModel = new ImportDataTypeModel();
        }

        public ImportDataTypeModel ImportDataTypeModel { get; set; }
    }

    public class PatientImportModel
    {
        public PatientImportModel()
        {
            TransactionResult = new TransactionResult();
            Patients = new List<AdminTempPatient>();
            PatientContacts = new List<AdminTempPatientContact>();
        }

        public TransactionResult TransactionResult { get; set; }
        public List<AdminTempPatient> Patients { get; set; }
        public List<AdminTempPatientContact> PatientContacts { get; set; }
    }

    public class EmployeeImportModel
    {
        public EmployeeImportModel()
        {
            TransactionResult = new TransactionResult();
            Employees = new List<AdminTempEmployee>();
        }

        public TransactionResult TransactionResult { get; set; }
        public List<AdminTempEmployee> Employees { get; set; }
    }

    public class ImportDataTypeModel
    {
        public string FilePath { get; set; }
        public string ImportDataType { get; set; }
        public string EncryptedOrganizationID { get; set; }
    }

    #endregion

}