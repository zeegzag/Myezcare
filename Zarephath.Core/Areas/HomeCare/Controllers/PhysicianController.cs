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
    public class PhysicianController:BaseController
    {
        private IPhysicianDataProvider _physicianDataProvider;

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_AddUpdate)]
        public ActionResult AddPhysician(string id)
        {

            long physicianID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _physicianDataProvider = new PhysicianDataProvider();
            ServiceResponse response = _physicianDataProvider.AddPhysician(physicianID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_AddUpdate)]
        public ActionResult PartialAddPhysician(string id)
        {

            long physicianID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _physicianDataProvider = new PhysicianDataProvider();
            ServiceResponse response = _physicianDataProvider.AddPhysician(physicianID);
            ViewBag.IsPartialView = true;
            return View("AddPhysician", response.Data);
            // return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_AddUpdate)]
        public JsonResult AddPhysician(Physicians physician)
        {
            _physicianDataProvider = new PhysicianDataProvider();
            return Json(_physicianDataProvider.AddPhysician(physician, SessionHelper.LoggedInID));
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_List)]
        public ActionResult PhysicianList()
        {
            _physicianDataProvider = new PhysicianDataProvider();
            return View(_physicianDataProvider.SetPhysicianListPage().Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_List)]
        public ContentResult GetPhysicianList(SearchPhysicianListPage searchPhysicianListPage, int pageIndex = 1,
                                             int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _physicianDataProvider = new PhysicianDataProvider();
            return CustJson(_physicianDataProvider.GetPhysicianList(searchPhysicianListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Physician_Delete)]
        public ContentResult DeletePhysician(SearchPhysicianListPage searchPhysicianListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _physicianDataProvider = new PhysicianDataProvider();
            return CustJson(_physicianDataProvider.DeletePhysician(searchPhysicianListPage, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public JsonResult GetPhysicianListForAutoComplete(string searchText, int pageSize)
        {
            _physicianDataProvider = new PhysicianDataProvider();
            return Json(_physicianDataProvider.HC_GetPhysicianListForAutoComplete(searchText, pageSize));
        }
        [HttpPost]
        public JsonResult GetSpecialistListForAutoComplete(string searchText, string ignoreIds, int pageSize)
        {
            _physicianDataProvider = new PhysicianDataProvider();
            var response = _physicianDataProvider.GetSpecialistListForAutoComplete(searchText, ignoreIds, pageSize);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveSpecialist(string Specialist, string Name, string NPI, string PracticeAddress)
        {
            _physicianDataProvider = new PhysicianDataProvider();
            ServiceResponse response = _physicianDataProvider.SaveSpecialist(Specialist, Name, NPI, PracticeAddress);
            return JsonSerializer(response);
        }
    }
}
