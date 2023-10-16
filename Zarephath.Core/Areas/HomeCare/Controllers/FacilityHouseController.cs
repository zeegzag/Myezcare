using System;
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
    public class FacilityHouseController : BaseController
    {
        private IFacilityHouseDataProvider _facilityHouseDataProvider;

        public FacilityHouseController(){
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_FacilityHouse_AddUpdate)]
        public ActionResult AddFacilityHouse(string id)
        {
            long facilityId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _facilityHouseDataProvider.HC_SetAddFacilityHousePage(facilityId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_FacilityHouse_AddUpdate)]
        public ActionResult PartialAddFacilityHouse(string id)
        {
            long facilityId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _facilityHouseDataProvider.HC_SetAddFacilityHousePage(facilityId);
            ViewBag.IsPartialView = true;
            return ShowUserFriendlyPages(response) ?? View("AddFacilityHouse", response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_FacilityHouse_AddUpdate)]
        public JsonResult AddFacilityHouse(HC_AddFacilityHouseModel facilityModel)
        {
            var response = _facilityHouseDataProvider.HC_AddFacility(facilityModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult SearchEquipment(string searchText, string ignoreIds, int pageSize)
        {
            return Json(_facilityHouseDataProvider.GetEquipment(pageSize, ignoreIds, searchText));
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_FacilityHouse_List)]
        public ActionResult FacilityHouseList()
        {
            var response = _facilityHouseDataProvider.HC_SetFacilityHouseListPage().Data;
            return View(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_FacilityHouse_List)]
        public ContentResult GetFacilityHouseList(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _facilityHouseDataProvider.HC_GetFacilityHouseList(searchFacilityHouseModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_FacilityHouse_Delete)]
        public JsonResult DeleteFacilityHouse(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _facilityHouseDataProvider.HC_DeleteFacilityHouse(searchFacilityHouseModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

    }
}
