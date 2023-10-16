using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;
using Myezcare_Admin.Resources;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;


namespace Myezcare_Admin.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        //readonly string _domainNotFoundURL = ConfigSettings.SiteBaseURL01() + Constants.DomainNotFoundURL;
        //readonly string _accessDeniedURL = ConfigSettings.SiteBaseURL01() + Constants.AccessDeniedURL;
        //readonly string _loginURL = ConfigSettings.SiteBaseURL01() + Constants.LoginURL;
        //readonly string _securityQAURL = ConfigSettings.SiteBaseURL01() + Constants.SecurityQAURL;


        public string Permissions { get; set; }
        //public string Module { get; set; }



        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionHelper.DomainName))
            {
                Common.GetSubDomain();
            }

            if (string.IsNullOrEmpty(SessionHelper.DomainName) || SessionHelper.DomainName == Resource.NotFound)
            {
                Common.ThrowErrorMessage(Resource.DomainNotExist);
            }
            else
            {
                var isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
                if (CheckAllowedActions())
                    return;

                var strPermissions = string.IsNullOrEmpty(Permissions) ? new string[] { } : Permissions.Split(',');

                //if (SessionHelper.LoggedInID > 0 && !SessionHelper.IsSecurityQuestionSubmitted)
                //{
                //    //if (!isAjaxRequest)
                //    //    filterContext.Result = new RedirectResult(_accessDeniedURL);

                //    //CacheHelper cacheHelper = new CacheHelper();
                //    //string securityQaurl = cacheHelper.SiteBaseURL + Constants.SecurityQAURL;

                //    string securityQaurl = ConfigSettings.SiteBaseURL + Constants.SecurityQAURL;

                //    filterContext.Result = new RedirectResult(securityQaurl);
                //}



                if (SessionHelper.LoggedInID > 0)
                {
                    //bool isAuthoized =
                    //    SessionHelper.Permissions.Any(
                    //        permission => strPermissions.Contains(permission.PermissionID.ToString()));

                    bool isAuthoized = false;

                    if (!isAuthoized)
                        isAuthoized = strPermissions.Contains(Constants.RememberMePermission) ||
                                      strPermissions.Contains(Constants.AnonymousLoginPermission);

                    //if (!isAuthoized)
                    //    isAuthoized = strPermissions.Contains(Constants.AnonymousLoginPermission);

                    //CacheHelper cacheHelper = new CacheHelper();
                    //string accessDeniedUrl = cacheHelper.SiteBaseURL + Constants.AccessDeniedURL;

                    string accessDeniedUrl = ConfigSettings.SiteBaseURL + Constants.AccessDeniedURL;

                    if (!isAuthoized && !isAjaxRequest)
                        filterContext.Result = new RedirectResult(accessDeniedUrl);
                    else if (!isAuthoized)
                        RedirectToAction(filterContext, accessDeniedUrl);
                }
                else
                {
                    if (filterContext.HttpContext.Request.CurrentExecutionFilePath != Constants.LogoutURL)
                    {
                        bool removeFormsAuthenticationTicket = true;
                        bool isTimeOut = false;
                        if (filterContext.HttpContext.Request.IsAuthenticated)
                        {
                            HttpCookie decryptedCookie =
                                HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                            if (decryptedCookie == null)
                                isTimeOut = true;
                            else
                            {

                                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(decryptedCookie.Value);
                                if (ticket != null)
                                {
                                    var identity = new GenericIdentity(ticket.Name);
                                    if (identity.IsAuthenticated)
                                    {
                                        SecurityDataProvider securityDataProvider = new SecurityDataProvider();
                                        LoginModel loginModel = new LoginModel { UserName = ticket.Name };
                                        ServiceResponse response = new ServiceResponse();
                                        response = securityDataProvider.CheckLogin(loginModel, true);

                                        if (response.IsSuccess)
                                        {
                                            SessionHelper.SetUserSession((LoginResponseModel)response.Data);
                                            removeFormsAuthenticationTicket = false;
                                        }
                                        else
                                            isTimeOut = true;
                                    }
                                    else
                                        isTimeOut = true;
                                }
                                else
                                    isTimeOut = true;
                            }
                        }

                        string szCookieHeader = HttpContext.Current.Request.Headers["Cookie"];
                        if (isTimeOut ||
                            (SessionHelper.LoggedInID > 0 && HttpContext.Current.Session.IsNewSession) &&
                            (null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") > 0))
                            SessionHelper.IsTimeOutHappened = true;

                        if (removeFormsAuthenticationTicket)
                        {
                            FormsAuthentication.SignOut();

                            string returnUrl = "?returnUrl=" +
                                               (isAjaxRequest
                                                   ? (filterContext.HttpContext.Request.UrlReferrer != null
                                                       ? filterContext.HttpContext.Request.UrlReferrer.LocalPath
                                                       : "")
                                                   : filterContext.HttpContext.Request.CurrentExecutionFilePath +
                                                     (filterContext.HttpContext.Request.QueryString.HasKeys()
                                                         ? "?" + filterContext.HttpContext.Request.QueryString
                                                         : ""));

                            if (!strPermissions.Contains(Constants.RememberMePermission))
                            {
                                //CacheHelper cacheHelper = new CacheHelper();
                                //string loginUrl = cacheHelper.SiteBaseURL + Constants.LoginURL;

                                string loginUrl = ConfigSettings.SiteBaseURL + Constants.LoginURL;

                                RedirectToAction(filterContext, loginUrl + returnUrl);
                            }

                        }
                    }
                }
            }
        }

        private void RedirectToAction(AuthorizationContext filterContext, string actionUrl)
        {
            var isAjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
            if (isAjaxRequest)
            {

                if (actionUrl.Contains("?returnUrl="))
                    filterContext.HttpContext.Response.StatusCode = 308;
                else
                    filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.Result = new JsonResult
                    {
                        Data = new LinkResponse
                            {
                                Type = Constants.NotAuthorized,
                                Link = actionUrl
                            },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
            }
            else
                filterContext.Result = new RedirectResult(actionUrl);
        }

        private bool CheckAllowedActions()
        {
            string[] strPermissions = string.IsNullOrEmpty(Permissions) ? new string[] { } : Permissions.Split(',');

            if (strPermissions.Contains(Constants.AnonymousPermission))
                return true;

            if (!strPermissions.Any())
                return true;

            return false;
        }
    }

}
