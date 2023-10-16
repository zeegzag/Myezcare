using System;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.WebSockets;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class SecurityController : BaseController
    {
        private ISecurityDataProvider _securityDataProvider;

        #region Login

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.RememberMePermission)]
        public ActionResult Index()
        {
            if (SessionHelper.LoggedInID == 0)
            {
                _securityDataProvider = new SecurityDataProvider();
                var response = _securityDataProvider.GetLoginPageDetail();
                HttpCookie cookie = new HttpCookie("Language");
                cookie.Value = response.Message;
                Response.Cookies.Add(cookie);
                return View(response.Data);
                //return View();
            }

            return RedirectToAction("dashboard", "home", new { area = "HomeCare" });
        }

        [HttpGet]
        public ActionResult SetPassword(string id)
        {
            _securityDataProvider = new SecurityDataProvider();
            var response = _securityDataProvider.SetPassword(id);
            if (response.IsSuccess == false)
            {
                return View("Notification", response.Data);
            }
            return View(response.Data);
        }

        [HttpPost]
        public JsonResult SaveSetPassword(SetPasswordModel model)
        {
            _securityDataProvider = new SecurityDataProvider();
            var response = _securityDataProvider.SaveSetPassword(model, SessionHelper.LoggedInID);

            return Json(response);
        }

        [HttpPost]
        public JsonResult Login(LoginModel login)
        {
            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            ch_MyezcareOrg.RemoveCacheData();

            foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            {
                if (css.ConnectionString.Contains("ASD"))
                {
                    var DBName = css.Name;
                }
            }



            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.CheckLogin(login, false);
            if (response.IsSuccess)
            {
                if (login.IsRemember)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                                                                     login.UserName,
                                                                                     DateTime.Now,
                                                                                     DateTime.Now.AddMinutes(
                                                                                         ConfigSettings
                                                                                             .RememberMeDuration),
                                                                                     true,
                                                                                     FormsAuthentication.FormsCookiePath);

                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    // Create the cookie.
                    HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                                           encTicket)
                    {
                        Expires = ticket.Expiration
                    };
                    Response.Cookies.Add(httpCookie);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(login.UserName, false);
                }


                SessionHelper.SetUserSession((LoginResponseModel)response.Data);

                var res = ((LoginResponseModel)response.Data);
                res.SessionValueData.IsCompletedWizard = SessionHelper.IsCompletedWizard;
                response.Data = res;

                //LoginResponseModel loginResponse = (LoginResponseModel)response.Data;
                //SessionHelper sessionHelper = new SessionHelper((LoginResponseModel)response.Data);
                //SessionValueData sessionValue = loginResponse.SessionValueData;
                //SessionHelper.LoggedInID = sessionValue.EmployeeID;
                //SessionHelper.Email = sessionValue.Email;
                //SessionHelper.Name = sessionValue.FirstName + sessionValue.LastName;
                //SessionHelper.IsSecurityQuestionSubmitted = sessionValue.IsSecurityQuestionSubmitted;
                //SessionHelper.RoleID = sessionValue.RoleID;
                //SessionHelper.UserName = sessionValue.UserName;
            }

            return Json(response);
        }

        // Start, Added by Sagar(19 Dec 2019) - Call the PartialView for browser compatible
        public ActionResult browsercompatible()
        {

            return PartialView("_browsercompatible");
        }
        //End 

        #endregion

        #region Forgot Password

        /// <summary>
        /// This is the Forgot Password Page. Here User will have to enter their valid username, security question/answer. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.SetForgotPasswordPage();
            return View(response.Data);
        }

        //Get Security Question by Emp ID 20220711 RN
        [HttpPost]
        public JsonResult GetSecurityQuestion(ForgotPasswordDetailModel forgotPassworddetailModel)//(ForgotPasswordModel forgotPasswordModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.GetSecurityQuestion(forgotPassworddetailModel);
            return Json(response);
            // return View(response.Data);
        }

        /// <summary>
        /// This method will save the user information in the database. Check for the user information is correct or not.
        /// </summary>
        /// <param name="forgotPasswordModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            ServiceResponse response = new ServiceResponse();
            _securityDataProvider = new SecurityDataProvider();

            if (forgotPasswordModel.IsUnlockAccountPage || !forgotPasswordModel.IsUnlockAccountPage)
            {

                response = _securityDataProvider.UnlockAccount(forgotPasswordModel);
            }
            else
            {
                response = _securityDataProvider.SaveForgotPassword(forgotPasswordModel);
                if (response.IsSuccess)
                    SessionHelper.ForgotPasswordUserID = Convert.ToInt64(response.Data);
            }



            return Json(response);
        }


        #endregion


        #region Unlock Account Process

        /// <summary>
        /// This is the Forgot Password Page. Here User will have to enter their valid username, security question/answer. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UnlockAccount()
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.SetForgotPasswordPage();
            ForgotPasswordModel model = (ForgotPasswordModel)response.Data;
            model.IsUnlockAccountPage = true;
            return View("ForgotPassword", model);
        }

        #endregion



        #region Reset Password

        /// <summary>
        /// This is the Reset Password page. Here the user have to enter the Password and Confirm Password.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetPassword()
        {
            if (SessionHelper.ForgotPasswordUserID > 0)
            {
                long loggedInUserID = SessionHelper.ForgotPasswordUserID;
                if (loggedInUserID > 0)
                {
                    Session.RemoveAll();
                    Session.Abandon();
                }
                _securityDataProvider = new SecurityDataProvider();
                return View(_securityDataProvider.SetResetPasswordPage(loggedInUserID).Data);
            }

            return RedirectToAction("index", "security");
        }

        /// <summary>
        /// This method will save the user information in the database. User new password will be saved in the database.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveResetPassword(ResetPasswordModel model)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.SaveResetPassword(model);
            return Json(response);
        }

        #endregion

        #region Security Question

        /// <summary>
        /// This is the Security Question page. Here the user username will be prepopulated from the login page and have to set up their security question/answer for further.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SecurityQuestion()
        {
            if (SessionHelper.IsSecurityQuestionSubmitted || SessionHelper.LoggedInID == 0)
                return RedirectToAction("index", "security");


            _securityDataProvider = new SecurityDataProvider();
            var securityQuestionModel = new SecurityQuestionModel();
            ServiceResponse response = _securityDataProvider.SetSecurityQuestionPage(securityQuestionModel, SessionHelper.UserName);
            return View(response.Data);

        }

        /// <summary>
        /// This method will save the user security question/answer in the database. This security question/answer will be use when user forget their password.
        /// </summary>
        /// <param name="securityQuestionModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSecurityQuestion(SecurityQuestionModel securityQuestionModel)
        {
            _securityDataProvider = new SecurityDataProvider();

            ServiceResponse response = _securityDataProvider.SaveSecurityQuestion(securityQuestionModel,
                                                                                  SessionHelper.LoggedInID);

            if (response.IsSuccess)
                SessionHelper.IsSecurityQuestionSubmitted = true;

            response.Data = SessionHelper.IsCompletedWizard;
            return Json(response);
        }
        #endregion

        #region Logout

        [HttpGet]
        public ActionResult LogOut()
        {
            if (SessionHelper.LoggedInID > 0 && SessionHelper.IsEmployeeLogin)
            {
                Common.EmployeeLoginLogs(SessionHelper.LoggedInID, false);

                CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
                ch_MyezcareOrg.RemoveCacheData();
            }

            Session.Abandon();
            FormsAuthentication.SignOut();
            const string loggedOutPageUrl = "/security/logout";
            Response.Write("<script language='javascript'>");
            Response.Write("function ClearHistory()");
            Response.Write("{");
            Response.Write(" var backlen=history.length;");
            Response.Write(" history.go(-backlen);");
            Response.Write(" window.location.href='" + loggedOutPageUrl + "'; ");
            Response.Write("}");
            Response.Write("</script>");



            return Redirect("index");
        }

        #endregion

        #region Verify Employee

        public ActionResult AccountVerification(string id)
        {
            long encryptedValue = 0;
            if (!string.IsNullOrEmpty(id))
            {
                encryptedValue = Convert.ToInt64(Crypto.Decrypt(id));

            }

            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.AccountVerification(encryptedValue);
            return View("Notification", response.Data);
        }
        #endregion

        #region Resend Verification Email

        public JsonResult RegenerateVerificationLink(string email)
        {
            _securityDataProvider = new SecurityDataProvider();
            return Json(_securityDataProvider.RegenerateVerificationLink(email));
        }

        #endregion

        #region Errros

        public ActionResult DomainNotFound()
        {
            return View("DomainNotFound");
        }


        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult AccessDenied()
        {
            return View("accessdenied");
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult InternalError()
        {
            return View("InternalError");
        }


        #endregion

        #region Edit Profile

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult EditProfile()
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.SetEditProfilePage(SessionHelper.LoggedInID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult SaveEditProfile(EditProfileModel editProfileModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.SaveEditProfile(editProfileModel, SessionHelper.LoggedInID);
            return Json(response);
        }
        #endregion

        #region Role Permission
        [CustomAuthorize(Permissions = Constants.Permission_Additional_RolePermission)]
        public ActionResult RolePermission()
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.GetRolePermission(new SearchRolePermissionModel());
            return View("rolepermission", response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Additional_RolePermission)]
        public JsonResult GetRolePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.GetRolePermission(searchRolePermissionModel);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Additional_RolePermission)]
        public JsonResult AddNewRole(Role roleModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.AddNewRole(roleModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Additional_RolePermission)]
        public JsonResult UpdateRoleName(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.UpdateRolename(searchRolePermissionModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Additional_RolePermission)]
        public JsonResult SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.SaveRoleWisePermission(searchRolePermissionModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Additional_ReportPermission)]
        public JsonResult SaveMapReport(MapReportModel objMapReport)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.SaveMapReport(objMapReport, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion Role Permission

        #region UploadImage
        [HttpPost]
        public JsonResult UploadFile()
        {
            CacheHelper _cacheHelper = new CacheHelper();
            ServiceResponse response = new ServiceResponse();

            HttpPostedFileBase file = Request.Files[0];

            string basePath =String.Format(_cacheHelper.UploadPath,_cacheHelper.Domain) + ConfigSettings.EmpProfileImg + ConfigSettings.TempFiles;

            basePath += SessionHelper.LoggedInID + "/";
            response = Common.SaveFile(file, basePath);

            return Json(response);
        }

        public JsonResult UploadProfileImage()
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_SaveProfileImg(HttpContext.Request, true);
            return Json(response);
        }

        #endregion


        #region Marketing Page
        [HttpGet]
        public ActionResult Marketing()
        {
            return View("Marketing");
        }


        #endregion

        #region Onboarding Organization
        [HttpGet]
        public ActionResult CreateLogin(string id) //createlogin
        {
            if (SessionHelper.LoggedInID != 0 && SessionHelper.IsCompletedWizard)
                return RedirectToAction("dashboard", "home", new { area = "HomeCare" });
            if (SessionHelper.LoggedInID == 0 && SessionHelper.IsCompletedWizard)
                return RedirectToAction("index");

            CacheHelper_MyezCare ch_MyezcareOrg = new CacheHelper_MyezCare();
            ch_MyezcareOrg.RemoveCacheData();

            SessionHelper.OrganizationId = !string.IsNullOrEmpty(id) ? Convert.ToInt32(id) : 0;
            HC_AddEmployeeModel model = new HC_AddEmployeeModel();
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.SetCreateLoginData(model);
            return View(response.Data);
        }
        #endregion
       
    }
}
