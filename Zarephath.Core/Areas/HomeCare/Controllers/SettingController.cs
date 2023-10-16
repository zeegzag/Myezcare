using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class SettingController : BaseController
    {

        private ISettingDataProvider _settingDataProvider;
        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_OrganizationSettings)]
        public ActionResult OrganizationSetting()
        {
            _settingDataProvider = new SettingDataProvider();
            var data = _settingDataProvider.GetSettings().Data;
            return View(data);
        }

        /// <summary>
        /// This method will return setting page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_OrganizationSettings)]
        public ActionResult TermsAndConditions()
        {
            _settingDataProvider = new SettingDataProvider();
            var data = _settingDataProvider.GetSettings().Data;
            return View("TermsAndConditions",data);
        }

        /// <summary>
        /// This method will save system settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_OrganizationSettings)]
        public ContentResult SaveSettings(OrganizationSetting settings)
        {
            _settingDataProvider = new SettingDataProvider();
            ServiceResponse response = _settingDataProvider.SaveSettings(settings, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_OrganizationSettings)]
        public ContentResult TestEmail(OrganizationSetting settings)
        {
            _settingDataProvider = new SettingDataProvider();
            ServiceResponse response = _settingDataProvider.SendTestEmail(settings, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        /// <summary>
        /// This method will save terms and conditions
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_OrganizationSettings)]
        public ContentResult SaveTermsAndConditions(long organizationId, string termsAndConditions)
        {
            _settingDataProvider = new SettingDataProvider();
            ServiceResponse response = _settingDataProvider.SaveTermsAndConditions(organizationId, termsAndConditions, SessionHelper.LoggedInID);
            return CustJson(response.IsSuccess);
        }

        public JsonResult GetGoogleUrl(int OrganizationId)
        {
            string oAuthUrl = new GoogleDriveHelper().GetAuthenticationUrl(OrganizationId);

            return Json(new { url = oAuthUrl }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GoogleAuthCallback()
        {
            ServiceResponse response = new ServiceResponse();
            response.IsSuccess = false;

            var strDrive = Request.QueryString["drive"];
            var strState = Request.QueryString["state"];

            long organizationId = 0;
            long.TryParse(strDrive, out organizationId);

            if (organizationId == 0)
            {
                if (!string.IsNullOrEmpty(strState))
                {
                    var base64EncodedBytes = System.Convert.FromBase64String(strState);
                    var driveInfoDecoded = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                    // if state is like drive=xxxx
                    if (driveInfoDecoded.Contains("orgid="))
                    {
                        long.TryParse(Server.UrlDecode(driveInfoDecoded).Substring(6), out organizationId);
                    }
                }
            }


            if (organizationId > 0)
            {
                _settingDataProvider = new SettingDataProvider();

                var organizationSetting = ((Models.ViewModel.AddOrganizationSettingModel)_settingDataProvider.GetSettings().Data).OrganizationSetting;


                // if organizationSetting exists
                if (organizationSetting != null)
                {
                    var strError = Request.QueryString["error"];
                    var strCode = Request.QueryString["code"];

                    if (!string.IsNullOrEmpty(strCode))
                    {
                        var oClient = new GoogleDriveHelper();

                        //oClient.DriveId = driveId;

                        var accessToken = oClient.GetAccessToken(organizationId, strCode);

                        if (accessToken != null)
                        {
                            organizationSetting.GoogleDriveAccessToken = accessToken.AccessToken;
                            organizationSetting.GoogleDriveRefreshToken = accessToken.RefreshToken;
                            organizationSetting.GoogleDriveRequestToken = strCode;
                            organizationSetting.GoogleDriveValidated = true;

                            // save token like AccessToken = access_token and AccessSecret = refresh_token
                            response = _settingDataProvider.SaveSettings(organizationSetting, SessionHelper.LoggedInID);
                            response.IsSuccess = true;
                            response.Message = "Your google drive is verified. You will be rediredted to main page shortly.";
                        }
                    }
                }
            }

            response.Message = "Google drive validation link is not valid.";

            //return CustJson(response);
            //return RedirectToRoute(Constants.Organization_SettingPageUrl);
            Response.Redirect(Constants.Organization_SettingPageUrl);

            return CustJson(response);
        }

    }
}
