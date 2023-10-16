using System;
using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class FacilityHouseController : BaseController
    {
        private IFacilityHouseDataProvider _facilityHouseDataProvider;

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Facility_House_AddUpdate)]
        public ActionResult AddFacilityHouse(string id)
        {
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
            long facilityId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _facilityHouseDataProvider.SetAddFacilityHousePage(facilityId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Facility_House_AddUpdate)]
        public JsonResult GetParentFacilityHouse(long facilityid)
        {
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
            var response = _facilityHouseDataProvider.GetParentFacilityHouse(facilityid);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Facility_House_AddUpdate)]
        public JsonResult AddFacilityHouse(Facility facility)
        {
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
            var response = _facilityHouseDataProvider.AddFacility(facility, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Facility_House_List)]
        public ActionResult FacilityHouseList()
        {
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
            var response = _facilityHouseDataProvider.SetFacilityHousePage().Data;
            return View(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Facility_House_List)]
        public ContentResult GetFacilityHouseList(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
            var response = _facilityHouseDataProvider.SetFacilityHouseList(searchFacilityHouseModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Facility_House_Delete)]
        public JsonResult DeleteFacilityHouse(SearchFacilityHouseModel searchFacilityHouseModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
            var response = _facilityHouseDataProvider.DeleteFacilityHouse(searchFacilityHouseModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Facility_House_AddUpdate)]
        public JsonResult GetFacilityTransportLocationMapping(long facilityID, long transportLocationID)
        {
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
            var response = _facilityHouseDataProvider.GetFacilityTransportLocationMapping(facilityID, transportLocationID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Facility_House_AddUpdate)]
        public JsonResult SaveFacilityTransportLocationMapping(FacilityTransportLocationMapping model)
        {
            _facilityHouseDataProvider = new FacilityHouseDataProvider();
            var response = _facilityHouseDataProvider.SaveFacilityTransportLocationMapping(model);
            return JsonSerializer(response);
        }
    }
}
