using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;

namespace Myezcare_Admin.Controllers
{
    public class SecurityController : BaseController
    {
        private ISecurityDataProvider _securityDataProvider;

        #region Login

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.RememberMePermission)]
        public ActionResult Index()
        {
            if (SessionHelper.LoggedInID == 0)
            {
                return View();
            }
            return RedirectToAction("dashboard", "home");
        }

        [HttpPost]
        public JsonResult Login(LoginModel login)
        {
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

            }
            return Json(response);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            if (SessionHelper.LoggedInID > 0)
                //Common.EmployeeLoginLogs(SessionHelper.LoggedInID, false);

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


        #region Errros

        [HttpGet]
        public ActionResult DomainNotFound()
        {
            return View("DomainNotFound");
        }

        [HttpGet]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult AccessDenied()
        {
            return View("accessdenied");
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult InternalError()
        {
            return View("InternalError");
        }


        #endregion



        #region Role Permission
        public ActionResult RolePermission()
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_GetRolePermission(new SearchRolePermissionModel());
            return View("rolepermission", response.Data);
        }

        [HttpGet]
        public JsonResult RolePermissions()
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_GetRolePermission(new SearchRolePermissionModel());
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetRolePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_GetRolePermission(searchRolePermissionModel);
            return Json(response);
        }

        [HttpPost]
        public JsonResult SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_SaveRoleWisePermission(searchRolePermissionModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult SavePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_SavePermission(searchRolePermissionModel, SessionHelper.LoggedInID);
            return Json(response);
        }
        #endregion Role Permission

    }
}
