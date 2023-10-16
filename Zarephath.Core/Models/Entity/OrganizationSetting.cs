using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("OrganizationSettings")]
    [PrimaryKey("OrganizationID")]
    [Sort("OrganizationID", "ASC")]
    public class OrganizationSetting : BaseEntity
    {
        public long OrganizationID { get; set; }

        [Required(ErrorMessageResourceName = "SidebarLogoRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SiteLogo { get; set; }

        [Required(ErrorMessageResourceName = "SiteNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string SiteName { get; set; }

        public string OrganizationAddress { get; set; }
        public string OrganizationCity { get; set; }
        public string OrganizationState { get; set; }
        public string OrganizationZipcode { get; set; }

        //[Required(ErrorMessageResourceName = "SiteBaseUrlRequired", ErrorMessageResourceType = typeof(Resource))]
        //public string SiteBaseUrl { get; set; }

        [Required(ErrorMessageResourceName = "PageSizeRequired", ErrorMessageResourceType = typeof(Resource))]
        [AssertThat("PageSize > 0", ErrorMessageResourceName = "PageSizeValidation", ErrorMessageResourceType = typeof(Resource))]
        public int PageSize { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string SupportEmail { get; set; }

        [Required(ErrorMessageResourceName = "FavIconRequired", ErrorMessageResourceType = typeof(Resource))]
        public string FavIcon { get; set; }

        [Required(ErrorMessageResourceName = "LoginScreenLogoRequired", ErrorMessageResourceType = typeof(Resource))]
        public string LoginScreenLogo { get; set; }

        [Required(ErrorMessageResourceName = "TemplateLogoRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TemplateLogo { get; set; }

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

        //[Required(ErrorMessageResourceName = "TwilioCountryCodeRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TwilioDefaultCountryCode { get; set; }

        //[Required(ErrorMessageResourceName = "TwilioAccountSIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TwilioAccountSID { get; set; }
        //[Required(ErrorMessageResourceName = "TwilioAuthTokenRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TwilioAuthToken { get; set; }
        
        //[Required(ErrorMessageResourceName = "TwilioServiceSIDRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TwilioServiceSID { get; set; }
      
        //[Required(ErrorMessageResourceName = "TwilioFromNoRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TwilioFromNo { get; set; }
       
        [Required(ErrorMessageResourceName = "GoogleReCaptchaKeyRequired", ErrorMessageResourceType = typeof(Resource))]
        public string GoogleRecaptchaKey { get; set; }
        
        [Required(ErrorMessageResourceName = "TimeZoneRequired", ErrorMessageResourceType = typeof(Resource))]
        public string TimeZone { get; set; }

        public bool PatientResignatureNeeded { get; set; }

        [Ignore]
        public string LogoImageName { get; set; }
        [Ignore]
        public string TempLogoImagePath { get; set; }
        [Ignore]
        public string TempLogoImageName { get; set; }

        [Ignore]
        public string LoginScreenLogoName { get; set; }
        [Ignore]
        public string TempLoginScreenLogoPath { get; set; }
        [Ignore]
        public string TempLoginScreenLogoName { get; set; }

        [Ignore]
        public string FavIconName { get; set; }
        [Ignore]
        public string TempFavIconPath { get; set; }
        [Ignore]
        public string TempFavIconName { get; set; }

        [Ignore]
        public string TemplateLogoName { get; set; }
        [Ignore]
        public string TempTemplateLogoPath { get; set; }
        [Ignore]
        public string TempTemplateLogoName { get; set; }

        public string FaxTwilioAccountSID { get; set; }
        public string FaxAuthToken { get; set; }
        public string Fax { get; set; }

        [Ignore]
        public string DomainName { get; set; }

        //Billing Settings - Submitter
        public string Submitter_NM103_NameLastOrOrganizationName { get; set; }
        public string Submitter_NM109_IdCode { get; set; }
        public string Submitter_EDIContact1_PER02_Name { get; set; }
        [RegularExpression(Constants.RegxPhone, ErrorMessageResourceName = "PhoneInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Submitter_EDIContact1_PER04_CommunicationNumber { get; set; }
        [RegularExpression(Constants.RegxEmail, ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(Resource))]
        public string Submitter_EDIContact1_PER08_CommunicationNumber { get; set; }

        //Billing Settings - Billing Provider
        public string BillingProvider_NM103_NameLastOrOrganizationName { get; set; }
        public string BillingProvider_NM109_IdCode { get; set; }
        public string BillingProvider_N301_Address { get; set; }
        public string BillingProvider_N401_City { get; set; }
        public string BillingProvider_N402_State { get; set; }
        public string BillingProvider_N403_Zipcode { get; set; }
        public string BillingProvider_REF02_ReferenceIdentification { get; set; }

        public string InvoiceGenerationFrequency { get; set; } //TODO: this is int datatype but due to used in dropdown i have converted string
        public float InvoiceTaxRate { get; set; }
        public string InvoiceNote { get; set; }
        public int InvoiceDueDays { get; set; }
        public bool? InvoiceAddressIsBilltoPayor { get; set; }
        public bool? InvoiceAddressIsIncludePatientAddress { get; set; }
        public bool? InvoiceIsIncludePatientDOB { get; set; }
        public bool? InvoiceAddressIsIncludePatientAddressLine1 { get; set; }
        public bool? InvoiceAddressIsIncludePatientAddressLine2 { get; set; }
        public bool? InvoiceAddressIsIncludePatientAddressZip { get; set; }
        public string MIF_Appendix { get; set; }
        public string MIF_Description { get; set; }
        public string MIF_Revision { get; set; }
        [Ignore]
        public string FromEmailPasswords { get; set; }
        [Ignore]
        public string TwilioAccountSIDs { get; set; }
        [Ignore]
        public string TwilioAuthTokens { get; set; }
        [Ignore]
        public string TwilioFromNos { get; set; }
        [Ignore]
        public string TwilioServiceSIDs { get; set; }
        [Ignore]
        public string GoogleRecaptchaKeys { get; set; }

        public string GoogleDriveRequestToken { get; set; }
        public string GoogleDriveAccessToken { get; set; }
        public string GoogleDriveRefreshToken { get; set; }
        public bool GoogleDriveValidated { get; set; }
        public bool ShowInvoiceReadyRibbon { get; set; }
        public string HHAXClientId { get; set; }
        public string HHAXClientSecret { get; set; }
        [AllowHtml]
        public string TermsCondition { get; set; }
        public bool HasAggregator { get; set; }
        public string ClaimMDAccountKey { get; set; }
        public string ClaimMDUserID { get; set; }
        public string SandataBusinessEntityID { get; set; }
        public string SandataBusinessEntityMedicaidIdentifier { get; set; }
        public string SandataUserID { get; set; }
        public string SandataPassword { get; set; }
        public bool SandataIsProduction { get; set; }
        public string UploadPath { get; set; }
        public string ToEmail { get; set; }
        public string CC { get; set; }
        public string Body { get; set; }
        public bool ScheduleType { get; set; }
        public bool EnforceAcrossAllClients { get; set; }
        public bool EnvironmentType { get; set; }
    }
}
