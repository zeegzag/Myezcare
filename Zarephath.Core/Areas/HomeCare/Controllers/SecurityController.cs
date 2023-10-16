using System;
using System.Web;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class SecurityController : BaseController
    {
        private ISecurityDataProvider _securityDataProvider;

        #region Role Permission
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_RolePermissions)]
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
        [CustomAuthorize(Permissions = Constants.Permission_Additional_RolePermission)]
        public JsonResult GetMapReport(MapReportModel mapReportModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.GetMapReport(mapReportModel);
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_RolePermissions)]
        public JsonResult GetRolePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_GetRolePermission(searchRolePermissionModel);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_RolePermissions)]
        public JsonResult AddNewRole(Role roleModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_AddNewRole(roleModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_RolePermissions)]
        public JsonResult UpdateRoleName(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_UpdateRolename(searchRolePermissionModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_RolePermissions)]
        public JsonResult SaveRoleWisePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_SaveRoleWisePermission(searchRolePermissionModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_AdditionalPermission_RolePermissions)]
        public JsonResult SavePermission(SearchRolePermissionModel searchRolePermissionModel)
        {
            _securityDataProvider = new SecurityDataProvider();
            ServiceResponse response = _securityDataProvider.HC_SavePermission(searchRolePermissionModel, SessionHelper.LoggedInID);
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
            string basePath = String.Format(_cacheHelper.UploadPath, _cacheHelper.Domain) + ConfigSettings.TempFiles;
            basePath += SessionHelper.LoggedInID + "/";
            response = Common.SaveFile(file, basePath);
            return Json(response);
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

    }
}
