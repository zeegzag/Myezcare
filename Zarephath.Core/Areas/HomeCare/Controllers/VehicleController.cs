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
    public class VehicleController : BaseController
    {
        private IVehicleDataProvider _vehicleDataProvider;

        public VehicleController(){
            _vehicleDataProvider = new VehicleDataProvider();
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_AddUpdate)]
        public ActionResult AddVehicle(string id)
        {
            long vehicleId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _vehicleDataProvider.HC_SetAddVehiclePage(vehicleId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_AddUpdate)]
        public ActionResult PartialAddVehicle(string id)
        {
            long vehicleId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _vehicleDataProvider.HC_SetAddVehiclePage(vehicleId);
            ViewBag.IsPartialView = true;
            return ShowUserFriendlyPages(response) ?? View("AddVehicle", response.Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_AddUpdate)]
        public JsonResult AddVehicle(SetVehicleModel vehicleModel)
        {
            var response = _vehicleDataProvider.HC_AddVehicle(vehicleModel);
            return Json(response);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_List)]
        public ActionResult VehicleList()
        {
            var response = _vehicleDataProvider.HC_SetVehicleListPage().Data;
            return View(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_List)]
        public ContentResult GetVehicleList(SearchVehicleModel searchVehicleModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _vehicleDataProvider.HC_GetVehicleList(searchVehicleModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_Delete)]
        public JsonResult DeleteVehicle(SearchVehicleModel searchVehicleModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _vehicleDataProvider.HC_DeleteVehicle(searchVehicleModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }


    }
}
