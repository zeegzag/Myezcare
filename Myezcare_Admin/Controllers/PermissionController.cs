using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.ViewModel;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;

namespace Myezcare_Admin.Controllers
{
    public class PermissionController : BaseController
    {
        private IPermissionDataProvider PermissionDataProvider;

        public PermissionController()
        {
            PermissionDataProvider = new PermissionDataProvider();
        }


        [HttpGet]
        public ActionResult PermissionList()
        {
            var response = PermissionDataProvider.SetPermissionListPage().Data;
            return View(response);
        }
        public ActionResult AddPermission(string id = "")
        {
            ServiceResponse response = new ServiceResponse();
            if (id == "undefined")
            {
                id = String.Empty;
            }
            long PermissionID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(id) : 0;
            if (PermissionID > 0)
            {
                response = PermissionDataProvider.SetAddPermissionsPage(PermissionID);
            }
            SearchPermissionsModel searchPermissionsModel = new SearchPermissionsModel();
            ServiceResponse responseAllPermission = PermissionDataProvider.GetGetPermissionList(searchPermissionsModel, 1, 100, "", "");
            JsonResult jsonResult = new JsonResult();
            ViewBag.ParentPermissions = responseAllPermission.Data;
            return View(response.Data);
        }

        [HttpPost]
        public JsonResult AddPermission1(PermissionsModule permissions)
        {
            var response = PermissionDataProvider.AddPermission(permissions, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpGet]
        public ActionResult PermissionList1()
        {
            var response = PermissionDataProvider.SetPermissionListPage().Data;
            return View(response);
        }

        public JsonResult DeletePermission(long PermissionID)
        {
            PermissionsModule permissions = new PermissionsModule();
            permissions.PermissionID = PermissionID;
            var response = PermissionDataProvider.DeletePermission(permissions);
            return Json(response);
        }

        //[HttpGet]
        //public ActionResult GetPermissionList(SearchPermissionsModel searchPermissionsModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        //{
        //    var response = PermissionDataProvider.GetGetPermissionList(searchPermissionsModel, pageIndex, pageSize, sortIndex, sortDirection);
        //    return View(response);
        //}
        [HttpGet]
        public JsonResult GetPermissionList(SearchPermissionsModel searchPermissionsModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = PermissionDataProvider.GetGetPermissionList(searchPermissionsModel, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
