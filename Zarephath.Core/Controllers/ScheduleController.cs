using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class ScheduleController : BaseController
    {
        private IScheduleDataProvider _scheduleDataProvider;

        #region Schedule Assignment
        /// <summary>
        /// Common for add/edit
        /// On Schedule Assignment Page call
        /// </summary>
        /// <returns>
        /// For return ScheduleAssignment To Display Referral List with Search and display Calander option.
        /// </returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ActionResult ScheduleAssignment()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View(_scheduleDataProvider.SetScheduleAssignmentModel().Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ContentResult GetReferralListForSchedule(SearchReferralListForSchedule searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetReferralListForSchedule(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        /// <summary>
        /// For Facility Auto complete.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="pageSize">pageSize is size for get given pagesize of data for current page</param>
        /// <param name="searchText"></param>
        /// <returns>Returns list of department list with delete success message.</returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public JsonResult GetFacilutyListForAutoComplete(string searchText, int pageSize, string searchParam)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            long regionID = !string.IsNullOrEmpty(searchParam) ? Convert.ToInt64(searchParam) : 0;
            return Json(_scheduleDataProvider.GetFacilutyListForAutoComplete(searchText, pageSize, regionID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public JsonResult LoadAllFacilityByRegion(int? regionID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return Json(_scheduleDataProvider.LoadAllFacilityByRegion(regionID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ContentResult GetReferralDetailForPopup(long referralID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetReferralDetailForPopup(referralID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ContentResult GetScheduleListByFacility(SearchScheduleListByFacility searchPara)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetScheduleListByFacility(searchPara);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ContentResult SaveScheduleMasterFromCalender(ScheduleAssignmentModel scheduleMaster)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.SaveScheduleMasterFromCalender(scheduleMaster, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ContentResult RemoveScheduleFromCalender(long scheduleID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.DeleteScheduleMaster(new SearchScheduleMasterModel
                {
                    ListOfIdsInCsv = scheduleID.ToString(),
                    IsShowListing = false

                }, 0, 1, "", "", SessionHelper.LoggedInID, "");
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ContentResult RemoveSchedulesFromWeekFacility(long weekMasterID, long? facilityID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.RemoveSchedulesFromWeekFacility(weekMasterID, facilityID, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ContentResult CreateWeek(WeekMaster model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.CreateWeek(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        #endregion

        #region Schedule Master

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_List)]
        public ActionResult ScheduleMaster(string id = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var model = (ScheduleMasterModel)_scheduleDataProvider.SetScheduleMasterPage().Data;
            if (id == "1")
            {
                model.IsPartial = true;
            }
            return View(model);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Hisotry)]
        public ActionResult ReferralScheduleMaster(string id = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var model = (ScheduleMasterModel)_scheduleDataProvider.SetScheduleMasterPage().Data;
            if (id == "1")
            {
                model.IsPartial = true;
            }
            return View("ScheduleMaster", model);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_List + "," + Constants.Permission_Schedule_Hisotry)]
        public ContentResult GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetScheduleMasterList(searchScheduleMasterModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_List)]
        public ContentResult DeleteScheduleMaster(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.DeleteScheduleMaster(searchScheduleMasterModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult SaveSchedule(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return CustJson(_scheduleDataProvider.UpdateScheduleFromScheduleList(searchScheduleMasterModel, scheduleModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult ReSchedule(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return CustJson(_scheduleDataProvider.ReScheduleFromScheduleList(searchScheduleMasterModel, scheduleModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_List)]
        public JsonResult GetScheduleNotificationLogs(long scheduleId)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return Json(_scheduleDataProvider.GetScheduleNotificationLogs(scheduleId));
        }

        
        #endregion

        #region ScheduleBatchService

        /// <summary>
        /// For Add Schedule BatchService.
        /// </summary>
        /// <param name="batchService">Entity Model</param>
        /// <returns>Returns list of department list with delete success message.</returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public JsonResult SaveScheduleBatchService(ScheduleBatchService batchService)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return Json(_scheduleDataProvider.SaveScheduleBatchService(batchService, SessionHelper.LoggedInID));
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ActionResult ScheduleBatchServiceList()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View(_scheduleDataProvider.SetScheduleBatchServiceListPage().Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public JsonResult GetScheduleBatchServiceList(SearchScheduleBatchServiceModel searchScheduleBatchServiceModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetScheduleBatchServiceList(searchScheduleBatchServiceModel, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public JsonResult DeleteScheduleBatchService(SearchScheduleBatchServiceModel searchScheduleBatchServiceModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.DeleteScheduleBatchService(searchScheduleBatchServiceModel, pageIndex, pageSize, sortIndex, sortDirection);
            return JsonSerializer(response);
        }

        #endregion

        #region Confirmation
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult ScheduleEmailConfiramation(string id)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("Notification", _scheduleDataProvider.ConfirmationStatus(id, SessionHelper.LoggedInID).Data);
        }


        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult ScheduleEmailDetailHtml(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View("NotFound");
            }

            var idnew = Convert.ToInt32(Crypto.Decrypt(id));
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("ScheduleEmailDetailHtml", _scheduleDataProvider.GetScheduleDetailEmailHtml(idnew).Data);
        }


        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public ActionResult ScheduleEmailCancellation(string id)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetCancelEmailDetail(id);
            if (response.IsSuccess == false)
            {
                return View("Notification", response.Data);
            }
            return View(response.Data);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.AnonymousPermission)]
        public JsonResult UpdateScheduleCancelstatus(CancelEmailDetailModel cancelEmailDetailModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var respponse = _scheduleDataProvider.UpdateScheduleCancelstatus(cancelEmailDetailModel, SessionHelper.LoggedInID);
            return Json(respponse);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult GetEmailDetail(long scheduleId)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmailDetail(scheduleId);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public JsonResult SendParentEmail(ScheduleEmailModel scheduleEmailModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.SendParentEmail(scheduleEmailModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult GetSMSDetail(long scheduleId)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetSMSDetail(scheduleId);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public JsonResult SendParentSMS(ScheduleSmsModel scheduleSmsModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.SendParentSMS(scheduleSmsModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public JsonResult GetRegionWiseWeekFacility(long regionID, long weekMasterID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetRegionWiseWeekFacility(regionID, weekMasterID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public JsonResult SaveRegionWiseWeekFacility(long regionID, long weekMasterID, string facilites)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.SaveRegionWiseWeekFacility(regionID, weekMasterID, facilites);
            return Json(response);
        }

        #endregion

        #region Schedule Notification Noitce

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public JsonResult PrintNoticeScheduleNotification(string scheduleId)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.PrintScheduleNotice(scheduleId, true,SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Attendance_Permission)]
        public ActionResult AttendanceMaster()
        {
            ViewBag.IsAttandancePage = true;
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("ScheduleAssignment", _scheduleDataProvider.SetScheduleAssignmentModel().Data);
        }



        #region Demo In House Code
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Schedule_Assignment_Permission)]
        public ActionResult ScheduleAssignmentInHome()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View(_scheduleDataProvider.SetScheduleAssignmentModel().Data);
        }


        #endregion



        public ActionResult TZ()
        {
            Common.GetUtcTimeFromDate(DateTime.Now);
            return null;
        }

    }
}
