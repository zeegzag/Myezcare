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
    public class TransportServiceController : BaseController
    {
        private ITransportServiceDataProvider _transportServiceDataProvider;

        public TransportServiceController()
        {
            _transportServiceDataProvider = new TransportServiceDataProvider();
        }
        #region Transport Service  Contact
        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_TransportContact_AddUpdate)]
        public ActionResult AddTransportContact(string id)
        {
            long contactId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _transportServiceDataProvider.HC_SetAddTransportContactPage(contactId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_TransportContact_AddUpdate)]
        public ActionResult PartialAddTransportContact(string id)
        {
            long contactId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _transportServiceDataProvider.HC_SetAddTransportContactPage(contactId);

            ViewBag.IsPartialView = true;
            return ShowUserFriendlyPages(response) ?? View("AddTransportContact", response.Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_TransportContact_AddUpdate)]
        public JsonResult AddTransportContact(SetTransportContactModel transportContactModel)
        {
            var response = _transportServiceDataProvider.HC_AddTransportContact(transportContactModel);
            return Json(response);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_TransportContact_List)]
        public ActionResult TransportContactList()
        {
            var response = _transportServiceDataProvider.HC_SetTransportContactListPage().Data;
            return View(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_TransportContact_List)]
        public ContentResult GetTransportContactList(SearchTransportContactModel searchTransportContactModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _transportServiceDataProvider.HC_GetTransportContactList(searchTransportContactModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_TransportContact_Delete)]
        public JsonResult DeleteTransportContact(SearchTransportContactModel searchTransportContactModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _transportServiceDataProvider.HC_DeleteTransportContact(searchTransportContactModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult SearchOrganizationName(string searchText, int pageSize)
        {
            return Json(_transportServiceDataProvider.GetSearchOrganizationName(pageSize, searchText));
        }
        #endregion

        #region Vehicle


        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_AddUpdate)]
        public ActionResult AddVehicle(string id)
        {
            long vehicleId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _transportServiceDataProvider.HC_SetAddVehiclePage(vehicleId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_AddUpdate)]
        public ActionResult PartialAddVehicle(string id)
        {
            long vehicleId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _transportServiceDataProvider.HC_SetAddVehiclePage(vehicleId);
            ViewBag.IsPartialView = true;
            return ShowUserFriendlyPages(response) ?? View("AddVehicle", response.Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_AddUpdate)]
        public JsonResult AddVehicle(SetVehicleModel vehicleModel)
        {
            var response = _transportServiceDataProvider.HC_AddVehicle(vehicleModel);
            return Json(response);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_List)]
        public ActionResult VehicleList()
        {
            var response = _transportServiceDataProvider.HC_SetVehicleListPage().Data;
            return View(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_List)]
        public ContentResult GetVehicleList(SearchVehicleModel searchVehicleModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _transportServiceDataProvider.HC_GetVehicleList(searchVehicleModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_Delete)]
        public JsonResult DeleteVehicle(SearchVehicleModel searchVehicleModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _transportServiceDataProvider.HC_DeleteVehicle(searchVehicleModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion


        #region TransportAssignment
        public ActionResult TransportAssignment(string id)
        {
            long transportId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _transportServiceDataProvider.HC_SetTransportAssignmentPage(transportId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_TransportContact_AddUpdate)]
        public JsonResult SaveTransportAssignment(TransportAssignmentModel transportAssignmentModel)
        {
            var response = _transportServiceDataProvider.SaveTransportAssignment(transportAssignmentModel);
            return Json(response);
        }
        public ContentResult GetTransportAssignmentList(SearchTransportAssignmentModel searchTransportAssignmentModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _transportServiceDataProvider.GetTransportAssignmentList(searchTransportAssignmentModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        //deletetransportatlocationlist HC_DeleteVehicle
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Vehicle_Delete)]
        public JsonResult DeleteTransportAssignmentList(SearchTransportAssignmentModel searchTransportAssignmentModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _transportServiceDataProvider.DeleteTransportAssignment(searchTransportAssignmentModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }
        public JsonResult SaveTransportAssignPatient(TransportAssignPatientModel transportAssignPatientModel)
        {
            var response = _transportServiceDataProvider.SaveTransportAssignPatient(transportAssignPatientModel);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult TransportAssignmentList()
        {
            SetTransaportAssignPatientListModel response =
                (SetTransaportAssignPatientListModel)_transportServiceDataProvider.HC_SetTransportAssignmentList(SessionHelper.LoggedInID).Data;
            return JsonSerializer(response);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetTransportAssignmentReferralList(SearchTransportAssignPatientListModel searchTransportAssignPatientListModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            string ids = "";
            if (searchTransportAssignPatientListModel.GroupIds != null)
            {
                foreach (var item in searchTransportAssignPatientListModel.GroupIds)
                {
                    ids += item + ",";
                }
            }
            searchTransportAssignPatientListModel.CommaSeparatedIds = ids.ToString();
            var response = _transportServiceDataProvider.SearchTransportAssignPatientListModel(searchTransportAssignPatientListModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        [HttpPost]
        public JsonResult DeleteTransportAssignmentReferralList(SearchTransportAssignPatientListModel searchTransportAssignPatientListModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _transportServiceDataProvider.DeleteTransportAssignmentReferral(searchTransportAssignPatientListModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        public ActionResult AssignTransportGroup(string id)
        {
            long transportId = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            var response = _transportServiceDataProvider.HC_SetTransportAssignmentGroupPage(transportId);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpPost]
        public JsonResult SearchTransportAssignmentGroupList(SearchTransportAssignmentGroupModel searchTransportAssignmentGroupModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            var response = _transportServiceDataProvider.SearchTransportAssignmentGroupList(searchTransportAssignmentGroupModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetTransportGroup(long FacilityID, string StartDate, string EndDate)
        {
            var response = _transportServiceDataProvider.GetTransportGroup(FacilityID, StartDate, EndDate);
            return Json(response);
        }

        [HttpPost]
        public JsonResult GetTransportGroupDetail(long TransportGroupID)
        {
            var response = _transportServiceDataProvider.GetTransportGroupDetail(TransportGroupID);
            return Json(response);
        }

        public JsonResult SaveTransportGroupAssignPatient(SearchTransportAssignmentGroupModel searchTransportAssignmentGroupModel)
        {
            var response = _transportServiceDataProvider.SaveTransportGroupAssignPatient(searchTransportAssignmentGroupModel);
            return Json(response);
        }
        public JsonResult DeleteTransportGroupAssignPatientNote(TransportGroupAssignPatientModel transportGroupAssignPatientModel)
        {
            var response = _transportServiceDataProvider.DeleteTransportGroupAssignPatientNote(transportGroupAssignPatientModel);
            return Json(response);
        }

        public JsonResult SaveTransportGroup(TransportGroupModel transportGroupModel)
        {
            var response = _transportServiceDataProvider.SaveTransportGroup(transportGroupModel);
            return Json(response);
        }
        public JsonResult SaveTransportGroupAssignPatientNote(TransportGroupAssignPatientModel transportGroupAssignPatientModel)
        {
            var response = _transportServiceDataProvider.SaveTransportGroupAssignPatientNote(transportGroupAssignPatientModel);
            return Json(response);
        }

        #endregion

    }
}
