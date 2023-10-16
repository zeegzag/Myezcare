using HomeCareApi.Infrastructure.Attributes;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.ViewModel
{
    [TableName("OrganizationSettings")]
    [PrimaryKey("OrganizationID")]
    [Sort("OrganizationID", "ASC")]
    public class OrganizationSetting 
    {
        public long OrganizationID { get; set; }
        public string SiteLogo { get; set; }
        public string SiteName { get; set; }
        public int PageSize { get; set; }
        public string SupportEmail { get; set; }
        public string FavIcon { get; set; }
        public string LoginScreenLogo { get; set; }
        public string TemplateLogo { get; set; }
        public string NetworkHost { get; set; }
        public string NetworkPort { get; set; }
        public string FromTitle { get; set; }
        public string FromEmail { get; set; }
        public string FromEmailPassword { get; set; }
        public bool EnableSSL { get; set; }
        public string TwilioAccountSID { get; set; }
        public string TwilioAuthToken { get; set; }
        public string TwilioServiceSID { get; set; }
        public string TwilioFromNo { get; set; }
        public string TwilioDefaultCountryCode { get; set; }
        public string GoogleRecaptchaKey { get; set; }
        public string TimeZone { get; set; }
        public string OrganizationAddress { get; set; }
        public string OrganizationCity { get; set; }
        public string OrganizationState { get; set; }
        public string OrganizationZipcode { get; set; }
        public string UploadPath { get; set; }

    }
}