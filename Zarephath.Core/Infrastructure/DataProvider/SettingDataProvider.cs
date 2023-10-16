using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zarephath.Core.Helpers;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    public class SettingDataProvider : BaseDataProvider, ISettingDataProvider
    {
        /// <summary>
        /// This method will return setting detail
        /// </summary>
        /// <returns></returns>
        public ServiceResponse GetSettings()
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                AddOrganizationSettingModel List = new AddOrganizationSettingModel();
                List = GetMultipleEntity<AddOrganizationSettingModel>(StoredProcedure.GetOrganizationSettingsPage);
                //OrganizationSetting settings = GetEntity<OrganizationSetting>();
                //addOrganizationSettingModel.OrganizationSetting = settings ?? new OrganizationSetting();
                //AddOrganizationSettingModel List = addOrganizationSettingModel;
                if (List != null)
                {
                    List.OrganizationSetting.FromEmailPasswords = List.OrganizationSetting.FromEmailPassword;
                    List.OrganizationSetting.TwilioAccountSIDs = List.OrganizationSetting.TwilioAccountSID;
                    List.OrganizationSetting.TwilioAuthTokens = List.OrganizationSetting.TwilioAuthToken;
                    List.OrganizationSetting.TwilioFromNos = List.OrganizationSetting.TwilioFromNo;
                    List.OrganizationSetting.TwilioServiceSIDs = List.OrganizationSetting.TwilioServiceSID;
                    List.OrganizationSetting.GoogleRecaptchaKeys = List.OrganizationSetting.GoogleRecaptchaKey;
                    List.OrganizationSetting.FromEmailPassword = "";
                    List.OrganizationSetting.TwilioAccountSID = "";
                    List.OrganizationSetting.TwilioAuthToken = "";
                    List.OrganizationSetting.TwilioFromNo = "";
                    List.OrganizationSetting.TwilioServiceSID = "";
                    List.OrganizationSetting.GoogleRecaptchaKey = "";
                }
                response.Data = List;
                response.IsSuccess = true;
            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }

        public ServiceResponse SaveTermsAndConditions(long organizationId, string termsAndConditions, long userId)
        {
            var response = new ServiceResponse();

            try
            {
                List<SearchValueData> param = new List<SearchValueData>
                    {
                        new SearchValueData {Name = "OrganizationID", Value = organizationId.ToString()},
                        new SearchValueData {Name = "TermsCondition", Value = termsAndConditions},
                        new SearchValueData {Name = "UserID", Value = userId.ToString()},
                    };
                GetScalar(StoredProcedure.UpdateOrganizationSettings, param);
                
                response.IsSuccess = true;

            }
            catch (Exception)
            {
                response.Message = Resource.ExceptionMessage;
            }

            return response;
        }

        /// <summary>
        /// This method will save system settings
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ServiceResponse SaveSettings(OrganizationSetting organizationSetting, long userId)
        {
            CacheHelper _cacheHelper = new CacheHelper();
            ServiceResponse response = new ServiceResponse();

            #region Check SiteLogo Exist
            if (string.IsNullOrEmpty(organizationSetting.LogoImageName) && string.IsNullOrEmpty(organizationSetting.SiteLogo))
            {
                response.Message = Resource.SiteLogoRequired;
                return response;
            }
            #endregion
            #region Check FavIcon Exist
            if (string.IsNullOrEmpty(organizationSetting.FavIconName) && string.IsNullOrEmpty(organizationSetting.FavIcon))
            {
                response.Message = Resource.FavIconRequired;
                return response;
            }
            #endregion
            #region Check LoginScreenLogo Exist
            if (string.IsNullOrEmpty(organizationSetting.LoginScreenLogoName) && string.IsNullOrEmpty(organizationSetting.LoginScreenLogo))
            {
                response.Message = Resource.LoginScreenLogoRequired;
                return response;
            }
            #endregion
            #region Check TemplateLogo Exist
            if (string.IsNullOrEmpty(organizationSetting.TemplateLogoName) && string.IsNullOrEmpty(organizationSetting.TemplateLogo))
            {
                response.Message = Resource.TemplateLogoRequired;
                return response;
            }
            #endregion

            try
            {
                string destPath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.SiteLogoPath +
                                                      userId + "/";
                string previousSiteLogoPath = string.Empty;
                string previousFavIconPath = string.Empty;
                string previousLoginLogoPath = string.Empty;
                string previousTemplateLogoPath = string.Empty;

                #region Move SiteLogo
                ServiceResponse SiteLogofileMoveResponse = new ServiceResponse { IsSuccess = true };
                if (organizationSetting.TempLogoImagePath != organizationSetting.SiteLogo)
                {
                    previousSiteLogoPath = HttpContext.Current.Server.MapCustomPath(organizationSetting.SiteLogo);
                    SiteLogofileMoveResponse = Common.MoveFile(organizationSetting.TempLogoImagePath, destPath);

                    if (SiteLogofileMoveResponse.IsSuccess)
                        organizationSetting.SiteLogo = (string)SiteLogofileMoveResponse.Data;
                }
                #endregion

                #region Move FavIcon
                ServiceResponse FavIconfileMoveResponse = new ServiceResponse { IsSuccess = true };
                if (organizationSetting.TempFavIconPath != organizationSetting.FavIcon)
                {
                    previousFavIconPath = HttpContext.Current.Server.MapCustomPath(organizationSetting.FavIcon);

                    FavIconfileMoveResponse = Common.MoveFile(organizationSetting.TempFavIconPath, destPath);

                    if (FavIconfileMoveResponse.IsSuccess)
                        organizationSetting.FavIcon = (string)FavIconfileMoveResponse.Data;
                }
                #endregion

                #region Move Login Screen Logo
                ServiceResponse LoginLogofileMoveResponse = new ServiceResponse { IsSuccess = true };
                if (organizationSetting.TempLoginScreenLogoPath != organizationSetting.LoginScreenLogo)
                {
                    previousLoginLogoPath = HttpContext.Current.Server.MapCustomPath(organizationSetting.LoginScreenLogo);

                    LoginLogofileMoveResponse = Common.MoveFile(organizationSetting.TempLoginScreenLogoPath, destPath);

                    if (LoginLogofileMoveResponse.IsSuccess)
                        organizationSetting.LoginScreenLogo = (string)LoginLogofileMoveResponse.Data;
                }
                #endregion

                #region Move Template Logo
                ServiceResponse TemplateLogofileMoveResponse = new ServiceResponse { IsSuccess = true };
                if (organizationSetting.TempTemplateLogoPath != organizationSetting.TemplateLogo)
                {
                    previousTemplateLogoPath = HttpContext.Current.Server.MapCustomPath(organizationSetting.TemplateLogo);
                    TemplateLogofileMoveResponse = Common.MoveFile(organizationSetting.TempTemplateLogoPath, destPath);

                    if (TemplateLogofileMoveResponse.IsSuccess)
                        organizationSetting.TemplateLogo = (string)TemplateLogofileMoveResponse.Data;
                }
                #endregion

                //organizationSetting.CreatedBy = userId;
                organizationSetting.UpdatedBy = userId;
                //organizationSetting.CreatedDate = DateTime.UtcNow;
                organizationSetting.UpdatedDate = Common.GetOrgCurrentDateTime();
                organizationSetting.SystemID = Common.GetHostAddress();
                if (organizationSetting.TwilioServiceSID == null)
                {
                    organizationSetting.TwilioServiceSID = organizationSetting.TwilioServiceSIDs;
                }
                if (organizationSetting.TwilioAccountSID == null)
                {
                    organizationSetting.TwilioAccountSID = organizationSetting.TwilioAccountSIDs;
                }
                if (organizationSetting.TwilioAuthToken == null)
                {
                    organizationSetting.TwilioAuthToken = organizationSetting.TwilioAuthTokens;
                }
                if (organizationSetting.TwilioFromNo == null)
                {
                    organizationSetting.TwilioFromNo = organizationSetting.TwilioFromNos;
                }
                if (organizationSetting.FromEmailPassword == null)
                {
                    organizationSetting.FromEmailPassword = organizationSetting.FromEmailPasswords;
                }
                if (organizationSetting.GoogleRecaptchaKey == null)
                {
                    organizationSetting.GoogleRecaptchaKey = organizationSetting.GoogleRecaptchaKeys;
                }

                if (organizationSetting.FaxTwilioAccountSID == null)
                {
                    organizationSetting.FaxTwilioAccountSID = organizationSetting.FaxTwilioAccountSID;
                }
                if (organizationSetting.FaxAuthToken == null)
                {
                    organizationSetting.FaxAuthToken = organizationSetting.FaxAuthToken;
                }

                if (organizationSetting.Fax == null)
                {
                    organizationSetting.Fax = organizationSetting.Fax;
                }

                if (organizationSetting.TermsCondition == null)
                {
                    organizationSetting.TermsCondition = organizationSetting.TermsCondition;
                }

                SaveObject(organizationSetting, userId);

                if (previousSiteLogoPath != null && File.Exists(previousSiteLogoPath))
                    File.Delete(previousSiteLogoPath);

                if (previousFavIconPath != null && File.Exists(previousFavIconPath))
                    File.Delete(previousFavIconPath);

                if (previousLoginLogoPath != null && File.Exists(previousLoginLogoPath))
                    File.Delete(previousLoginLogoPath);

                if (previousTemplateLogoPath != null && File.Exists(previousTemplateLogoPath))
                    File.Delete(previousTemplateLogoPath);

                CacheHelper cacheHelper = new CacheHelper();
                cacheHelper.RefreshCacheData<OrganizationSetting>(cacheHelper.OrganizationSettingCachedName);

                response.Message = Resource.RecordSavedSuccessfully;
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.Message = Resource.ExceptionMessage;
            }
            return response;
        }


        public ServiceResponse SendTestEmail(OrganizationSetting organizationSetting, long userId)
        {
            bool isSentMail = false;
            ServiceResponse objResponse = new ServiceResponse();
            //string client_id =System.Configuration.ConfigurationManager.AppSettings["ClientID"].ToString();
            
           

            //bool value = SendMailForTest(organizationSetting);
            //objResponse.Data = value;
            objResponse= SendMailForTest(organizationSetting);
           // objResponse.IsSuccess = true;
            return objResponse;
        }

        private ServiceResponse SendMailForTest(OrganizationSetting organizationSetting)
        {
           // bool isSentMail = false;
            ServiceResponse response = new ServiceResponse();
            //string client_id =System.Configuration.ConfigurationManager.AppSettings["ClientID"].ToString();
            string toEmailId = organizationSetting.ToEmail.ToString();
            string fromEmailId = organizationSetting.FromEmail.ToString();

            response = Common.SendEmailTest("Test Email Work", fromEmailId.ToString(), toEmailId, organizationSetting.Body.ToString(), EnumEmailType.HomeCare_EmailPayment_Notification.ToString(),organizationSetting.CC);
           // return isSentMail;
            return response;
        }

    }
}
