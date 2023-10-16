using Myezcare_Admin.Helpers;
using Myezcare_Admin.Infrastructure;
using Myezcare_Admin.Infrastructure.DataProvider;
using Myezcare_Admin.Models;
using Myezcare_Admin.Models.Entity;
using Myezcare_Admin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Myezcare_Admin.Controllers
{
    public class ServicePlanController : BaseController
    {
        private IServicePlanDataProvider _servicePlanDataProvider;

        public ServicePlanController()
        {
            _servicePlanDataProvider = new ServicePlanDataProvider();
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult AddServicePlan(string id)
        {
            long servicePlanId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = new ServiceResponse();
            response = _servicePlanDataProvider.SetAddServicePlanPage(servicePlanId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public JsonResult AddServicePlan(ServicePlanModel servicePlan)
        {
            var response = _servicePlanDataProvider.AddServicePlan(servicePlan, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ActionResult ServicePlanList()
        {
            var response = _servicePlanDataProvider.SetServicePlanListPage().Data;
            return View(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        public ContentResult GetServicePlanList(SearchServicePlanModel searchServicePlanModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _servicePlanDataProvider.GetServicePlanList(searchServicePlanModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public JsonResult DeleteServicePlan(SearchServicePlanModel searchServicePlanModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _servicePlanDataProvider.DeleteServicePlan(searchServicePlanModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #region Service Plan Component

        [HttpPost]
        public JsonResult SearchServicePlanComponent(string searchText, int pageSize)
        {
            return Json(_servicePlanDataProvider.GetServicePlanComponent(pageSize, searchText));
        }

        #endregion

    }
}
