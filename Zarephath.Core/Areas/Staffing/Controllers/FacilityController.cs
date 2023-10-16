using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Infrastructure.Utility;
using Zarephath.Core.Infrastructure.Utility.CareGiverApi;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;


namespace Zarephath.Core.Areas.Staffing.Controllers
{
    public class FacilityController : BaseController
    {
        private IFacilityDataProvider _facilityDataProvider;
        private ISettingDataProvider _settingDataProvider;
        CacheHelper _cacheHelper = new CacheHelper();

        #region Add Referral

        //[CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult AddReferral(string id)
        {
            _facilityDataProvider = new FacilityDataProvider();
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;

            ServiceResponse response = _facilityDataProvider.HC_SetAddReferralPage(referralID, SessionHelper.LoggedInID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult PartialddReferral(string id)
        {
            _facilityDataProvider = new FacilityDataProvider();
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;

            ServiceResponse response = _facilityDataProvider.HC_SetAddReferralPage(referralID, SessionHelper.LoggedInID);
            // return ShowUserFriendlyPages(response) ?? View(response.Data);
            ViewBag.IsPartialView = true;
            return View("AddReferral", response.Data);

        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult SetAddReferralPage(string id)
        {

            _facilityDataProvider = new FacilityDataProvider();
            long referralID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            ServiceResponse response = _facilityDataProvider.HC_SetAddReferralPage(referralID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

      
        #endregion

        #region Referral Details

        

        [HttpPost]
        public JsonResult AddReferral(HC_AddReferralModel addReferralModel)
        {
            _facilityDataProvider = new FacilityDataProvider();
            return JsonSerializer(_facilityDataProvider.HC_AddReferral(addReferralModel, SessionHelper.LoggedInID));
        }
      

        #endregion

        #region Contact Information Tab

        [HttpPost]
        public JsonResult AddContact(AddReferralModel addReferralModel)
        {
            _facilityDataProvider = new FacilityDataProvider();
            return Json(_facilityDataProvider.AddContact(addReferralModel, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public JsonResult GetContactList(string searchText, int pageSize)
        {
            _facilityDataProvider = new FacilityDataProvider();
            return Json(_facilityDataProvider.GetContactList(searchText, pageSize));
        }


        [HttpPost]
        public JsonResult DeleteContact(long contactMappingID)
        {
            _facilityDataProvider = new FacilityDataProvider();
            return Json(_facilityDataProvider.DeteteContact(contactMappingID, SessionHelper.LoggedInID));
        }
        #endregion


        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public ActionResult ReferralList()
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.HC_SetReferralListPage(SessionHelper.LoggedInID).Data;
            return View("referrallist", response);
        }

        [HttpPost]

        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_List)]
        public JsonResult GetReferralList(SearchReferralListModel searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            string ids = "";
            if (searchReferralModel.GroupIds != null)
            {
                foreach (var item in searchReferralModel.GroupIds)
                {
                    ids += item + ",";
                }
            }
            searchReferralModel.CommaSeparatedIds = ids.ToString();
            var response = _facilityDataProvider.GetReferralList(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        public JsonResult DeleteReferral(SearchReferralListModel searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.DeleteReferral(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

       
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public ActionResult ReferralTimeSlots()
        {
            _facilityDataProvider = new FacilityDataProvider();
            ServiceResponse response = _facilityDataProvider.HC_ReferralTimeSlots();
            return ShowUserFriendlyPages(response) ?? View(response.Data);

        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public ActionResult PartialReferralTimeSlots(string id)
        {
            _facilityDataProvider = new FacilityDataProvider();
            ServiceResponse response = _facilityDataProvider.HC_ReferralTimeSlotss(id);

            var model = (HC_RTSModel)response.Data;
            model.SearchRTSMaster.ReferralID = Convert.ToInt64(id);
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("ReferralTimeSlots", response.Data);

        }


        #region RTS Master

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule + "," + Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult GetRtsMasterlist(SearchRTSMaster searchRTSMaster, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.GetRtsMasterlist(searchRTSMaster, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult DeleteRtsMaster(SearchRTSMaster searchRTSMaster, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.DeleteRtsMaster(searchRTSMaster, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult AddRtsMaster(ReferralTimeSlotMaster rtsMaster)
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.AddRtsMaster(rtsMaster, SessionHelper.LoggedInID);
            return Json(response);
        }



        #endregion


        #region RTS Detail

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult GetRtsDetaillist(SearchRTSDetail searchRTSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.GetRtsDetaillist(searchRTSDetail, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult DeleteRtsDetail(SearchRTSDetail searchRTSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.DeleteRtsDetail(searchRTSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult AddRtsDetail(ReferralTimeSlotDetail rtsDetail)
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.AddRtsDetail(rtsDetail, SessionHelper.LoggedInID);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public JsonResult UpdateRtsDetail(ReferralTimeSlotDetail rtsDetail, SearchRTSDetail searchRTSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.UpdateRtsDetail(rtsDetail, searchRTSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        public JsonResult ReferralTimeSlotForceUpdate(ReferralTimeSlotDetail rtsDetail, SearchRTSDetail searchRTSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.ReferralTimeSlotForceUpdate(rtsDetail, searchRTSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion


      

        #region Referral Time Slots For Care Type
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Patient_Schedule)]
        public ActionResult ReferralCareTypeTimeSlots()
        {
            _facilityDataProvider = new FacilityDataProvider();
            ServiceResponse response = _facilityDataProvider.HC_ReferralCareTypeTimeSlots();
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpGet]
        public ActionResult PartialReferralCareTypeTimeSlots(string id)
        {
            _facilityDataProvider = new FacilityDataProvider();
            ServiceResponse response = _facilityDataProvider.HC_ReferralCareTypeTimeSlots();

            var model = (RefCareTypeSlotsModel)response.Data;
            model.SearchCTSchedule.ReferralID = Convert.ToInt64(id);
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("ReferralCareTypeTimeSlots", response.Data);
        }
        [HttpPost]
        public JsonResult AddCareTypeSlot(CareTypeTimeSlot model)
        {
            _facilityDataProvider = new FacilityDataProvider();
            ServiceResponse response = _facilityDataProvider.AddCareTypeSlot(model, SessionHelper.LoggedInID);
            return Json(response);
        }
        [HttpPost]
        public JsonResult GetCareTypeScheduleList(SearchCTSchedule searchCTSchedule, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _facilityDataProvider = new FacilityDataProvider();
            var response = _facilityDataProvider.GetCareTypeScheduleList(searchCTSchedule, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }
        #endregion
        //[HttpPost]
        //public JsonResult SaveAndGetFacilityDetails(FacilityDetailModel objFacility)
        //{
        //    _facilityDataProvider = new FacilityDataProvider();
        //    ServiceResponse response = _facilityDataProvider.SaveAndGetFacilityDetails(objFacility);
        //    return JsonSerializer(response);
        //}
        [HttpPost]
        public JsonResult GetEmployeeGroup( string GroupID)
        {
            _facilityDataProvider = new FacilityDataProvider();
            // long referralId = !string.IsNullOrWhiteSpace(RoleID) ? Convert.ToInt64(Crypto.Decrypt(RoleID)) : 0;
            ServiceResponse response = _facilityDataProvider.GetEmployeeGroup(GroupID);
            //return JsonSerializer(response);
            return Json(response,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetEmployeeByGroupId(string groupIds)
        {
            _facilityDataProvider = new FacilityDataProvider();
            return JsonSerializer(_facilityDataProvider.GetEmployeeByGroupId(groupIds));
        }
        [HttpPost]
        public JsonResult UpdateEmployeeGroupId(long GroupID, string EmployeeIDs, bool IsChecked)
        {
            _facilityDataProvider = new FacilityDataProvider();
            return JsonSerializer(_facilityDataProvider.UpdateEmployeeGroupId(GroupID, EmployeeIDs, IsChecked));
        }
        [HttpPost]
        public JsonResult SaveEmpGroup(string GroupName,long ReferralID, bool IsEditMode)
        {
            _facilityDataProvider = new FacilityDataProvider();
            ServiceResponse response = _facilityDataProvider.SaveEmpGroup(GroupName, ReferralID, IsEditMode);
            return JsonSerializer(response);
        }
        [HttpPost]
        public JsonResult RemoveAllAssignedGroup(long GroupID, string EmployeeID)
        {
            _facilityDataProvider = new FacilityDataProvider();
            return JsonSerializer(_facilityDataProvider.RemoveAllAssignedGroup(GroupID, EmployeeID));
        }
    }
}
