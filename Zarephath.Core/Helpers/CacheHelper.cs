using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Runtime.Caching;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Helpers
{
    public class CacheHelper
    {

        //public readonly string OrganizationSettingCachedName = !string.IsNullOrEmpty(Domain) ? Domain : ""; // "CachedOrganizationSetting";

        public string OrganizationSettingCachedName
        {
            get
            {
                return !string.IsNullOrEmpty(Domain) ? Domain : "";
            }
        }



        public string SiteBaseURL
        {
            get
            {
                try
                {
                    //OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                    return String.Format(ConfigurationManager.AppSettings["SiteBaseURL"], SessionHelper.DomainName);
                }
                catch (Exception)
                {

                    return ConfigurationManager.AppSettings["SiteBaseURL"];
                }
            }
        }

        public string SiteBaseURLMonile
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteBaseURLMobile"];
            }
        }

        //public string SiteBaseUrl
        //{
        //    get
        //    {
        //        OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
        //        if (cache != null)
        //        {
        //            return cache.SiteBaseUrl;
        //        }
        //        return null;
        //    }
        //}

        public string SiteLogo
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.SiteLogo;
                return null;
            }
        }
        public string SiteName
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.SiteName;
                return null;
            }
        }


        public string SupportEmail
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.SupportEmail;
                return null;
            }
        }

        public string OrganizationID
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return Convert.ToString(cache.OrganizationID);
                return null;
            }
        }

        public string OrganizationAddress
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.OrganizationAddress;
                return null;
            }
        }
        public string OrganizationCity
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.OrganizationCity;
                return null;
            }
        }
        public string OrganizationState
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.OrganizationState;
                return null;
            }
        }
        public string OrganizationZipcode
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.OrganizationZipcode;
                return null;
            }
        }

        public int PageSize
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.PageSize;
                return 0;
            }
        }
        public string GoogleRecaptchaKey
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.GoogleRecaptchaKey;
                return null;
            }
        }
        public string TimeZone
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.TimeZone;
                return null;
            }
        }
        public string FavIcon
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.FavIcon;
                return null;
            }
        }
        public string LoginScreenLogo
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.LoginScreenLogo;
                return null;
            }
        }

        public string TemplateLogo
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.TemplateLogo;
                return null;
            }
        }

        public string NetworkHost
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.NetworkHost;
                return null;
            }
        }
        public string NetworkPort
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.NetworkPort;
                return null;
            }
        }
        public string FromTitle
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.FromTitle;
                return null;
            }
        }
        public string FromEmail
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.FromEmail;
                return null;
            }
        }
        public string FromEmailPassword
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.FromEmailPassword;
                return null;
            }
        }

        public bool PatientResignatureNeeded
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.PatientResignatureNeeded;
                return true;
            }
        }

        public bool EnableSSL
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.EnableSSL;
                return true;
            }
        }

        public string TwilioDefaultCountryCode
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.TwilioDefaultCountryCode;
                return null;
            }
        }
        public string TwilioAccountSID
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.TwilioAccountSID;
                return null;
            }
        }
        public string TwilioAuthToken
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.TwilioAuthToken;
                return null;
            }
        }
        public string TwilioServiceSID
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.TwilioServiceSID;
                return null;
            }
        }
        public string TwilioFromNo
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.TwilioFromNo;
                return null;
            }
        }
        public float InvoiceTaxRate
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.InvoiceTaxRate;
                return 0;
            }
        }
        public string InvoiceNote
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.InvoiceNote;
                return null;
            }
        }
        public string InvoiceGenerationFrequency
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.InvoiceGenerationFrequency;
                return null;
            }
        }
        public bool? InvoiceAddressIsBilltoPayor
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                {
                    return cache.InvoiceAddressIsBilltoPayor;
                }
                return null;
            }
        }
        public bool? InvoiceAddressIsIncludePatientAddress
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                {
                    return cache.InvoiceAddressIsIncludePatientAddress;
                }
                return null;
            }
        }
        public bool? InvoiceIsIncludePatientDOB
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                {
                    return cache.InvoiceIsIncludePatientDOB;
                }
                return null;
            }
        }
        public bool? InvoiceAddressIsIncludePatientAddressLine1
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                {
                    return cache.InvoiceAddressIsIncludePatientAddressLine1;
                }
                return null;
            }
        }
        public bool? InvoiceAddressIsIncludePatientAddressLine2
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                {
                    return cache.InvoiceAddressIsIncludePatientAddressLine2;
                }
                return null;
            }
        }
        public bool? InvoiceAddressIsIncludePatientAddressZip
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                {
                    return cache.InvoiceAddressIsIncludePatientAddressZip;
                }
                return null;
            }
        }
        public string MIF_Appendix
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.MIF_Appendix;
                return null;
            }
        }
        public string MIF_Description
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.MIF_Description;
                return null;
            }
        }
        public string MIF_Revision
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.MIF_Revision;
                return null;
            }
        }

        public string Domain
        {
            get
            {
                var domain = "";
                try
                {
                    domain = SessionHelper.DomainName;

                    if (string.IsNullOrEmpty(domain))
                    {
                        Common.GetSubDomain();
                        domain = SessionHelper.DomainName;
                    }


                }
                catch (Exception)
                {
                    Common.GetSubDomain();
                    domain = SessionHelper.DomainName;
                }


                if (string.IsNullOrEmpty(SessionHelper.DomainName) || SessionHelper.DomainName == Resource.NotFound)
                {
                    Common.ThrowErrorMessage(Resource.DomainNotExist);
                }

                //string cachedName = domain + "0124";
                //ObjectCache cache = MemoryCache.Default;
                //cache.Remove(cachedName);
                //CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                //cache.Add(cachedName, domain, cacheItemPolicy);

                return domain;
            }
        }

        public string GoogleDriveRequestToken
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.GoogleDriveRequestToken;
                return null;
            }
        }

        public string GoogleDriveAccessToken
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.GoogleDriveAccessToken;
                return null;
            }
        }

        public string GoogleDriveRefreshToken
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.GoogleDriveRefreshToken;
                return null;
            }
        }

        public bool GoogleDriveValidated
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.GoogleDriveValidated;
                return false;
            }
        }

        public bool ShowInvoiceReadyRibbon
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.ShowInvoiceReadyRibbon;
                return false;
            }
        }

        public bool HasAggregator
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.HasAggregator;
                return false;
            }
        }

        public string ClaimMDAccountKey
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.ClaimMDAccountKey;
                return null;
            }
        }

        public string ClaimMDUserID
        {
            get
            {
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null)
                    return cache.ClaimMDUserID;
                return null;
            }
        }

        public string UploadPath
        {
            get
            {
                string path = null;
                OrganizationSetting cache = GetCachedSettingConfig<OrganizationSetting>(OrganizationSettingCachedName);
                if (cache != null) { path = cache.UploadPath; }
                if (string.IsNullOrEmpty(path)) { path = ConfigurationManager.AppSettings["UploadPath"]; }
                return path;
            }
        }

        public string CareFormClientSignaturesFullPath { get => string.Format("{0}{1}", UploadPath, ConfigSettings.CareFormClientSignatures); }

        public string CareFormFullPath { get => string.Format("{0}{1}", UploadPath, ConfigSettings.CareFormPath); }

        public string MIFSignatureFullPath { get => string.Format("{0}{1}", UploadPath, ConfigSettings.MIFSignaturePath); }

        public string EdiFile270UploadPath
        { get => string.Format("{0}{1}{2}{3}", UploadPath, ConfigSettings.EdiFilePath, ConfigSettings.EdiFileUploadPath, ConfigSettings.EdiFile270Path); }
        public string EdiFile271DownLoadPath
        { get => string.Format("{0}{1}{2}{3}", UploadPath, ConfigSettings.EdiFilePath, ConfigSettings.EdiFileDownloadPath, ConfigSettings.EdiFile271Path); }
        public string EdiFile271CsvDownLoadPath
        { get => string.Format("{0}{1}{2}{3}", UploadPath, ConfigSettings.EdiFilePath, ConfigSettings.EdiFileDownloadPath, ConfigSettings.EdiFile271CsvPath); }
        public string EdiFile277DownLoadPath
        { get => string.Format("{0}{1}{2}{3}", UploadPath, ConfigSettings.EdiFilePath, ConfigSettings.EdiFileDownloadPath, ConfigSettings.EdiFile277Path); }
        public string EdiFile837UploadPath
        { get => string.Format("{0}{1}{2}{3}", UploadPath, ConfigSettings.EdiFilePath, ConfigSettings.EdiFileUploadPath, ConfigSettings.EdiFile837Path); }
        public string EdiFile835DownLoadPath
        { get => string.Format("{0}{1}{2}{3}", UploadPath, ConfigSettings.EdiFilePath, ConfigSettings.EdiFileDownloadPath, ConfigSettings.EdiFile835Path); }
        public string EdiFile837ValidationErrorPath
        { get => string.Format("{0}{1}{2}{3}", UploadPath, ConfigSettings.EdiFilePath, ConfigSettings.TempEdiFileValidationErrorPath, ConfigSettings.EdiFile837Path); }
        public string EdiFile835CsvDownLoadPath
        { get => string.Format("{0}{1}{2}{3}", UploadPath, ConfigSettings.EdiFilePath, ConfigSettings.EdiFileDownloadPath, ConfigSettings.EdiFile835CsvPath); }

        public string ReportExcelFileUploadPath { get => string.Format("{0}{1}", UploadPath, ConfigSettings.ReportExcelFilePath); }

        public string RespiteNoticePrintFileUploadPath { get => string.Format("{0}{1}", UploadPath, ConfigSettings.RespiteNoticePrintFilePath); }

        public string ScheduleDayCareUploadPath { get => string.Format("{0}{1}", UploadPath, ConfigSettings.ScheduleDayCarePath); }

        public string ReferralDocumentFullPath { get => string.Format("{0}{1}", UploadPath, ConfigSettings.ReferralDocumentPath); }
        public string EmployeeDocumentFullPath { get => string.Format("{0}{1}", UploadPath, ConfigSettings.EmployeeDocumentPath); }


        #region Public Methods

        public T GetCachedSettingConfig<T>(string cachedName)
        {
            ObjectCache cache = MemoryCache.Default;

            //Check Whether CachedOrganizationSetting content is in Cache or not.
            if (cache.Contains(cachedName))
            {
                //If Config exist in Cache then retuen detail.
                T tokenvalue = (T)cache.Get(cachedName);
                return tokenvalue;
            }


            T siteSettingDetail = RefreshCacheData<T>(cachedName);
            return siteSettingDetail;

        }

        public void RemoveCacheData(string cachedName)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(cachedName);
        }

        public T RefreshCacheData<T>(string cachedName)
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Remove(cachedName);

            //If Config not exist in Cache then Bring from Database.
            ISecurityDataProvider securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = new ServiceResponse();

            if (cachedName == OrganizationSettingCachedName)
            {
                response = securityDataProvider.ChechedSettingData();
            }
            //else if (cachedName == MenuListCachedName)
            //{
            //    response = securityDataProvider.ChechedMenuList();
            //}

            T siteSettingDetail = (T)response.Data;

            // Store data in the cache    
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            //cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(ConfigSettings.SiteCacheExpiration);
            cache.Add(cachedName, siteSettingDetail, cacheItemPolicy);
            return siteSettingDetail;
        }

        #endregion
    }

    public class CacheHelperName
    {
        public static string ReleaseNote = "ReleaseNote";
    }

    public class CacheHelper_MyezCare
    {

        public string CachedName
        {
            get
            {
                return !string.IsNullOrEmpty(Domain) ? Domain + "023543" : "";
            }
        }

        public string Domain
        {
            get
            {
                var domain = "";
                try
                {
                    domain = SessionHelper.DomainName;

                    if (string.IsNullOrEmpty(domain))
                    {
                        Common.GetSubDomain();
                        domain = SessionHelper.DomainName;
                    }


                }
                catch (Exception)
                {
                    Common.GetSubDomain();
                    domain = SessionHelper.DomainName;
                }


                if (string.IsNullOrEmpty(SessionHelper.DomainName) || SessionHelper.DomainName == Resource.NotFound)
                {
                    Common.ThrowErrorMessage(Resource.DomainNotExist);
                }

                //string cachedName = domain + "0124";
                //ObjectCache cache = MemoryCache.Default;
                //cache.Remove(cachedName);
                //CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                //cache.Add(cachedName, domain, cacheItemPolicy);

                return domain;
            }
        }


        #region Public Methods

        public T GetCachedData<T>(string cachedName = null)
        {

            if (string.IsNullOrEmpty(cachedName))
                cachedName = CachedName;

            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(cachedName))
            {
                //If Config exist in Cache then retuen detail.
                T tokenvalue = (T)cache.Get(cachedName);
                return tokenvalue;
            }
            return default(T);
        }


        public T AddCacheData<T>(T cachedData, string cachedName = null)
        {

            if (string.IsNullOrEmpty(cachedName))
                cachedName = CachedName;

            ObjectCache cache = MemoryCache.Default;
            cache.Remove(cachedName);
            if (cachedData != null)
            {
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
                cache.Add(cachedName, cachedData, cacheItemPolicy);
            }
            return cachedData;
        }


        public void RemoveCacheData(string cachedName = null)
        {
            if (string.IsNullOrEmpty(cachedName))
                cachedName = CachedName;

            ObjectCache cache = MemoryCache.Default;
            cache.Remove(cachedName);
        }
        #endregion
    }


}
