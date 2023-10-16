using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.Scheduler;
using Zarephath.Core.Models.ViewModel;
using static Zarephath.Core.Models.Scheduler.ScheduleDTO;

namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class ScheduleController : BaseController
    {
        private IScheduleDataProvider _scheduleDataProvider;


        #region Myezcare - HomeCare

        #region In House Code

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult ScheduleAssignmentInHome()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("ScheduleAssignmentInHome", _scheduleDataProvider.HC_SetScheduleAssignmentModel().Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetReferralListForSchedule(SearchReferralListForSchedule searchReferralModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetReferralListForSchedule(searchReferralModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetReferralDetailForPopup(long referralID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetReferralDetailForPopup(referralID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetEmployeesForScheduling(long referralID, string employeeName)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmployeesForSchedulingURL(referralID, employeeName);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster + "," + Constants.HC_Permission_Employee_EmployeeCalendar + "," + Constants.HC_Permission_PatientIntake_PatientCalendar + "," + Constants.HC_Permission_Patient_Calendar)]
        public ContentResult GetEmployeesForEmpCalender(string employeeIDs)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmployeesForEmpCalender(employeeIDs);
            return CustJson(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetEmployeeMatchingPreferences(string id)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmployeeMatchingPreferences(Convert.ToInt64(id.Split('|')[0]), Convert.ToInt64(id.Split('|')[1]));
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster + "," + Constants.HC_Permission_Employee_EmployeeCalendar + "," + Constants.HC_Permission_PatientIntake_PatientCalendar + "," + Constants.HC_Permission_Patient_Calendar)]
        public ContentResult GetScheduleListByEmployees(SearchScheduleListByFacility searchPara)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetScheduleListByEmployees(searchPara);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult SaveScheduleMasterFromCalender(ScheduleAssignmentModel scheduleMaster)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SaveScheduleMasterFromCalender(scheduleMaster, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
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



        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult RemoveSchedulesFromWeekFacility(long weekMasterID, long? facilityID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.RemoveSchedulesFromWeekFacility(weekMasterID, facilityID, SessionHelper.LoggedInID);
            return CustJson(response);
        }






        #endregion

        #region In House Code - New Code

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult ScheduleAssignmentInHome01()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("ScheduleAssignmentInHome_01", _scheduleDataProvider.HC_SetScheduleAssignmentModel01().Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetSchEmployeeListForSchedule(SearchEmployeeListForSchedule searchSchEmployeeModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetSchEmployeeListForSchedule(searchSchEmployeeModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetSchEmployeeDetailForPopup(SearchEmployeeListForSchedule model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetSchEmployeeDetailForPopup(model);
            return CustJson(response);
        }

        //

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetReferralForScheduling(SearchScheduleListByFacility model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetReferralForScheduling(model);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetScheduleListByReferrals(SearchScheduleListByFacility model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetScheduleListByReferrals(model);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_VirtualVisits)]
        public ContentResult GetVirtualVisitsList(SearchVirtualVisitsListModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetVirtualVisitsList(model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        //SaveScheduleFromCalenderURL


        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult SaveScheduleFromCalender(ScheduleAssignmentModel scheduleMaster)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SaveScheduleFromCalender(scheduleMaster, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DeleteScheduleFromCalender(long scheduleID, string reason)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.DeleteScheduleFromCalender(new SearchScheduleMasterModel
            {
                ListOfIdsInCsv = scheduleID.ToString(),
                Reason = reason
            }, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        //[CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster + "," + Constants.HC_Permission_Employee_EmployeeCalendar)]
        public ContentResult GetEmpRefSchPageModel()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetEmpRefSchPageModel();
            return CustJson(response);
        }
        public ContentResult GetCareTypesbyPayorID(long payorID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetCareType(payorID);
            return CustJson(response);
        }


        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetEmpRefSchOptions(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetEmpRefSchOptions(model, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        /*Schedule master OPT*/
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetEmpRefSchOptions_PatientVisitFrequency_HC(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmpRefSchOptions_PatientVisitFrequency_HC(model, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetEmpRefSchOptions_ClientOnHoldData_HC(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmpRefSchOptions_ClientOnHoldData_HC(model, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetEmpRefSchOptions_ReferralInfo_HC(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmpRefSchOptions_ReferralInfo_HC(model, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetEmpRefSchOptions_ScheduleInfo_HC(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmpRefSchOptions_ScheduleInfo_HC(model, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        /*Schedule master OPT*/

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetEmpCareType(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetEmpCareTypeIds(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult CreateBulkSchedule(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.CreateBulkSchedule(model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        //[CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult SendScheduleReminder(long scheduleID, long referralID, long employeeID, bool sendSMS, bool sendEmail)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.SendVirtualVisitsReminderNotification(scheduleID, referralID, employeeID, sendSMS, sendEmail);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetSchEmpRefSkills(SearchEmpRefMatchModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetSchEmpRefSkills(model);
            return CustJson(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.DeleteEmpRefSchedule(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetAssignedEmployees(ReferralTimeSlotModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetAssignedEmployees(model);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult OnHoldUnHoldAction(PatientHoldDetail model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.OnHoldUnHoldAction(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult RemoveSchedule(RemoveScheduleModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.OnRemoveScheduleAction(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        public ContentResult SaveNewSchedule(ChangeScheduleModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();

            var response = _scheduleDataProvider.HC_SaveNewSchedule(model, SessionHelper.LoggedInID);
            if (response.IsSuccess)
            {
                SendSMSModel notification = new SendSMSModel();
                notification.EmployeeIds = Convert.ToString(model.EmployeeID);
                notification.Message = "Schedule for " + model.ReferralName + " changed to " + model.ScheduleDate.ToShortDateString() + " from " + model.StartTime + " to " + model.EndTime;
                notification.NotificationType = 2;
                EmployeeDataProvider _employeeDataProvider = new EmployeeDataProvider();
                var result = _employeeDataProvider.HC_SaveBroadcastNotification(notification, SessionHelper.LoggedInID, SessionHelper.DomainName);
                if (result.IsSuccess)
                {
                    response.Message = response.Message + "And Notification sent successfully to employee";
                }
            }


            return CustJson(response);
        }








        #endregion

        #region Referral Case Load

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult GetRCLEmpRefSchOptions(SearchRCLEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetRCLEmpRefSchOptions(model, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        #endregion


        #region Schedule Master

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ActionResult ScheduleMaster(string id = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var model = (HC_ScheduleMasterModel)_scheduleDataProvider.HC_SetScheduleMasterPage().Data;
            if (id == "1")
            {
                model.IsPartial = true;
            }
            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ContentResult GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetScheduleMasterList(searchScheduleMasterModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ContentResult DeleteScheduleMaster(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.DeleteScheduleMaster(searchScheduleMasterModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray);
            return CustJson(response);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ActionResult ScheduleAggregatorLogs()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var model = (SetScheduleAggregatorLogsListPage)_scheduleDataProvider.SetScheduleAggregatorLogsPage().Data;
            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ContentResult GetScheduleAggregatorLogsList(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetScheduleAggregatorLogsList(searchScheduleAggregatorLogsModel, pageIndex, pageSize, sortIndex, sortDirection);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ContentResult ResendAggregatorData(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.ResendAggregatorData(searchScheduleAggregatorLogsModel);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ContentResult GetScheduleAggregatorLogsDetails(SearchScheduleAggregatorLogsModel searchScheduleAggregatorLogsModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetScheduleAggregatorLogsDetails(searchScheduleAggregatorLogsModel);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ContentResult SaveSchedule(SearchScheduleMasterModel searchScheduleMasterModel, ScheduleMaster scheduleModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return CustJson(_scheduleDataProvider.HC_UpdateScheduleFromScheduleList(searchScheduleMasterModel, scheduleModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray));
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public JsonResult GetScheduleNotificationLogs(long scheduleId)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return Json(_scheduleDataProvider.GetScheduleNotificationLogs(scheduleId));
        }


        #endregion

        #region Checklist SMS and Notification

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult ChecklistGetEmpSMSDetail(long scheduleId, int templateId = 0)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.ChecklistGetEmpSMSDetail(scheduleId, templateId);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public JsonResult ChecklistSendEmpSMS(ScheduleSmsModel scheduleSmsModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.ChecklistSendEmpSMS(scheduleSmsModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion

        #region Confirmation


        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult GetEmpSMSDetail(long scheduleId, int templateId = 0)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.GetEmpSMSDetail(scheduleId, templateId);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public JsonResult SendEmpSMS(ScheduleSmsModel scheduleSmsModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.SendEmpSMS(scheduleSmsModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult GetEmpEmailDetail(long scheduleId)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetEmpEmailDetail(scheduleId);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public JsonResult SendEmpEmail(ScheduleEmailModel scheduleEmailModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SendEmpEmail(scheduleEmailModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult GetEmailDetail(long scheduleId)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetEmailDetail(scheduleId);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public JsonResult SendParentEmail(ScheduleEmailModel scheduleEmailModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SendParentEmail(scheduleEmailModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public ContentResult GetSMSDetail(long scheduleId)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetSMSDetail(scheduleId);
            return CustJson(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Schedule_Update)]
        public JsonResult SendParentSMS(ScheduleSmsModel scheduleSmsModel)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SendParentSMS(scheduleSmsModel, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion

        #region Virtual Visits
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_VirtualVisits)]
        public ActionResult VirtualVisits()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SetVirtualVisitsListPage().Data;
            return View("virtualvisits", response);
        }

        #endregion

        #region Employee Visits - Fleet management - pickup-drop
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_EmployeeVisit)]
        public ActionResult EmployeeVisits()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SetEmployeeVisitsPage().Data;
            return View("employeevisits", response);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_VirtualVisits)]
        public ContentResult GetReferralEmployeeVisits(SearchReferralEmployeeModel model
            , int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetReferralEmployeeVisits(model
                , pageIndex, pageSize, sortIndex, sortDirection
                , SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_VirtualVisits)]
        public ContentResult SaveEmployeeVisitsTransportLog(SearchReferralEmployeeModel model
            , int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetReferralEmployeeVisits(model
                , pageIndex, pageSize, sortIndex, sortDirection
                , SessionHelper.LoggedInID);
            return CustJson(response);
        }
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_VirtualVisits)]
        public ContentResult SavePickUpDropCall(SaveEmployeeVisitsTransportLog model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.SavePickUpDropCall(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        #endregion Employee Visits - Fleet management - pickup-drop
        #endregion

        #region Myezcare - DayCare

        #region DayCare Scheduling Assignment

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.ADC_Scheduling_Visitor_Attendance)]
        public ActionResult ScheduleDayCareAttendence()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("ScheduleDayCareAttendence", _scheduleDataProvider.HC_DayCare_SetScheduleAttendenceModel().Data);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult Daycare_GetScheduledPatientList(SearchScheduledPatientModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_Daycare_GetScheduledPatientList(model);
            return CustJson(response);
        }

        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult Daycare_GetRelationTypeList(int type)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.Daycare_GetRelationTypeList(type);
            return CustJson(response);
        }

        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult Daycare_PatientClockInClockOut(Daycare_SavePatient_AttendecenModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_Daycare_PatientClockInClockOut(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [HttpPost]
        // [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult DayCare_GetSchedulePatientTasks(Daycare_SavePatient_AttendecenModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_GetSchedulePatientTasks(model);
            return CustJson(response);
        }





        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult ScheduleAssignmentDayCare()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("ScheduleAssignmentDayCare", _scheduleDataProvider.HC_DayCare_SetScheduleAssignmentModel().Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_GetScheduleListByReferrals(SearchScheduleListByFacility model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_GetScheduleListByReferrals(model);
            return CustJson(response);
        }

        public JsonResult UploadProfileImage(long referralID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            ServiceResponse response = _scheduleDataProvider.HC_SaveReferralProfileImg(HttpContext.Request, true, referralID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_GetReferralForScheduling(SearchScheduleListByFacility model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_GetReferralForScheduling(model);
            return CustJson(response);
        }


        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_GetEmpRefSchOptions(SearchEmpRefSchOption model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_GetEmpRefSchOptions(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }



        public ContentResult DayCare_GetReferralBillingAuthorizationList(ReferralBillingAuthorizatioSearchModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_GetReferralBillingAuthorizationList(model);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_CreateBulkSchedule(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_CreateBulkSchedule(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_OnHoldUnHoldAction(PatientHoldDetail model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_OnHoldUnHoldAction(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_SavePatientAttendance(ScheduleAttendaceDetail model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_SavePatientAttendance(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_RemoveSchedule(RemoveScheduleModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_OnRemoveScheduleAction(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }




        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_DeleteScheduleFromCalender(long scheduleID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_DeleteScheduleFromCalender(new SearchScheduleMasterModel { ListOfIdsInCsv = scheduleID.ToString() }, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_DeleteEmpRefSchedule(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_GetAssignedFacilities(ReferralTimeSlotModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_GetAssignedFacilities(model);
            return CustJson(response);
        }


        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult DayCare_SaveScheduleFromCalender(ScheduleAssignmentModel scheduleMaster)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SaveScheduleFromCalender(scheduleMaster, SessionHelper.LoggedInID);
            return CustJson(response);
        }




        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public JsonResult SaveReferralCSVFile()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_SaveReferralCSVFile(Request, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public JsonResult CreateBulkScheduleUsingCSV(ReferralCsvModel referralCsvModel)
        {

            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.CreateBulkScheduleUsingCSV(referralCsvModel, SessionHelper.LoggedInID);
            return JsonSerializer(response);

        }






        #endregion

        #region DayCare Scheduling Master Log


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ActionResult ScheduleMasterDayCare(string id = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var model = (HC_ScheduleMasterModel)_scheduleDataProvider.HC_DayCare_SetScheduleMasterPage().Data;
            if (id == "1")
            {
                model.IsPartial = true;
            }
            return View("ScheduleMasterDayCare", model);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ContentResult DayCare_GetScheduleMasterList(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_GetScheduleMasterList(searchScheduleMasterModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray);
            return CustJson(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleLog)]
        public ContentResult DayCare_DeleteScheduleMaster(SearchScheduleMasterModel searchScheduleMasterModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DayCare_DeleteScheduleMaster(searchScheduleMasterModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID, sortIndexArray);
            return CustJson(response);
        }


        #endregion


        #endregion



        #region Myezcare - CaseManagement

        #region CaseManagement Scheduling Assignment

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult ScheduleAssignmentCaseManagement()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("ScheduleAssignmentCaseManagement", _scheduleDataProvider.HC_CaseManagement_SetScheduleAssignmentModel().Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult CaseManagement_GetScheduleListByReferrals(SearchScheduleListByFacility model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_CaseManagement_GetScheduleListByReferrals(model);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult CaseManagement_GetReferralForScheduling(SearchScheduleListByFacility model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_CaseManagement_GetReferralForScheduling(model);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult CaseManagement_OnHoldUnHoldAction(PatientHoldDetail model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_CaseManagement_OnHoldUnHoldAction(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }



        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult CaseManagement_GetEmpRefSchOptions(SearchEmpRefSchOption model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_CaseManagement_GetEmpRefSchOptions(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        #endregion


        #endregion





        #region MYezcare - PrivateDuty Care

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult ScheduleAssignmentPrivateDuty()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("ScheduleAssignmentPrivateDuty", _scheduleDataProvider.HC_PrivateDuty_SetScheduleAssignmentModel().Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetSchEmployeeListForSchedule(SearchEmployeeListForSchedule searchSchEmployeeModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetSchEmployeeListForSchedule(searchSchEmployeeModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetSchEmployeeDetailForPopup(SearchEmployeeListForSchedule model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_GetSchEmployeeDetailForPopup(model);
            return CustJson(response);
        }

        //

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetReferralForScheduling(SearchScheduleListByFacility model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_GetReferralForScheduling(model);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetScheduleListByReferrals(SearchScheduleListByFacility model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_GetScheduleListByReferrals(model);
            return CustJson(response);
        }











        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_SaveScheduleFromCalender(ScheduleAssignmentModel scheduleMaster)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_SaveScheduleFromCalender(scheduleMaster, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_DeleteScheduleFromCalender(long scheduleID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_DeleteScheduleFromCalender(new SearchScheduleMasterModel { ListOfIdsInCsv = scheduleID.ToString() }, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetEmpRefSchPageModel()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_GetEmpRefSchPageModel();
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetEmpRefSchOptions(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_GetEmpRefSchOptions(model, pageIndex, pageSize, sortIndex, sortDirection, sortIndexArray, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_CreateBulkSchedule(SearchEmpRefSchOption model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_CreateBulkSchedule(model, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }




        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetSchEmpRefSkills(SearchEmpRefMatchModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_GetSchEmpRefSkills(model);
            return CustJson(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_DeleteEmpRefSchedule(DeleteEmpRefScheduleModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_DeleteEmpRefSchedule(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetAssignedEmployees(ReferralTimeSlotModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_GetAssignedEmployees(model);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_OnHoldUnHoldAction(PatientHoldDetail model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_OnHoldUnHoldAction(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_RemoveSchedule(RemoveScheduleModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_OnRemoveScheduleAction(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }





        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ContentResult PrivateDuty_GetEmployeeMatchingPreferences(string id)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_PrivateDuty_GetEmployeeMatchingPreferences(Convert.ToInt64(id.Split('|')[0]), Convert.ToInt64(id.Split('|')[1]));
            return CustJson(response);
        }

















        #endregion


        #region Myezcare - Pending Schedule
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_PendingSchedule)]
        public ActionResult PendingSchedules()
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            return View("PendingSchedules", _scheduleDataProvider.HC_PendingSchedules().Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_PendingSchedule)]
        public ContentResult GetPendingScheduleList(SearchPendingSchedules searchPendingSchedules, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_GetPendingScheduleList(searchPendingSchedules, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_PendingSchedule)]
        public ContentResult DeletePendingSchedule(SearchPendingSchedules searchPendingSchedules, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string sortIndexArray = "")
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_DeletePendingSchedule(searchPendingSchedules, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_PendingSchedule)]
        public ContentResult ProcessPendingSchedule(PendingScheduleListModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            var response = _scheduleDataProvider.HC_ProcessPendingSchedule(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }



        #endregion

        #region "Nurse Scheduler"

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult NurseScheduler()
        {
            bool isLimitedAccess = false;
            ViewData["LimitedAccess"] = false;
            if (!Common.HasPermission(Constants.AllRecordAccess))
            {
                isLimitedAccess = true;
                ViewData["LimitedAccess"] = true;
            }

            _scheduleDataProvider = new ScheduleDataProvider();
            IReferralDataProvider _referralDataProvider = new ReferralDataProvider();
            IEmployeeDataProvider _employeeDataProvider = new EmployeeDataProvider();

            ServiceResponse response = _scheduleDataProvider.GetCareTypes();
            ViewBag.CareTypes = response.Data;
            if (isLimitedAccess)
            {
                ViewBag.EmployeeID = SessionHelper.LoggedInID;
                ViewBag.Employees = _employeeDataProvider.GetEmployeesForNurseSchedule(null, "", "").Where(s => s.EmployeeID == SessionHelper.LoggedInID);
            }
            else
                ViewBag.Employees = _employeeDataProvider.GetEmployeesForNurseSchedule(null, "", "");

            ViewBag.Patients = _referralDataProvider.GetReferralsForNurseSchedule(null, "", "").Data;
            //LoadFrequencyChoices();
            LoadMonths();
            return View("NurseScheduler", null);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult DeleteAppointment(long scheduleID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            //var response = _scheduleDataProvider.DeleteScheduleMaster(new SearchScheduleMasterModel
            //{
            //    ListOfIdsInCsv = scheduleID.ToString(),
            //    IsShowListing = false

            //}, 0, 1, "", "", SessionHelper.LoggedInID, "");
            //ScheduleMaster schedule = _scheduleDataProvider.GetScheduleMasterById(scheduleID);
            //if(schedule != null)
            _scheduleDataProvider.DeleteBulkNurseSchedule(scheduleID);
            return Content("");
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult GetPayorsByReferralId(long referralId)
        {
            IReferralDataProvider _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetPayorDetailsByReferralID(referralId);
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult GetAuthorizationCodesByReferralId(long referralId)
        {
            IReferralDataProvider _referralDataProvider = new ReferralDataProvider();
            ServiceResponse response = _referralDataProvider.GetAuthorizationCodesByReferralId(referralId);
            return Json(response.Data, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult SaveAppointment([System.Web.Http.FromBody] ScheduleDTO schedule)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            ServiceResponse response;
            if (!Common.HasPermission(Constants.AllRecordAccess))
                schedule.EmployeeId = SessionHelper.LoggedInID;

            if (schedule.ID > 0)
                response = _scheduleDataProvider.UpdateBulkNurseSchedule(schedule, SessionHelper.LoggedInID);
            else
                response = _scheduleDataProvider.CreateBulkNurseSchedule(schedule, SessionHelper.LoggedInID);

            return Json(new { result = response.Data, success = response.IsSuccess });
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Schedule_ScheduleMaster)]
        public ActionResult GetNurseSchedules(string careTypeIds, string employeeIds, string referralIds, int year)
        {
            if (!Common.HasPermission(Constants.AllRecordAccess))
            {
                employeeIds = SessionHelper.LoggedInID.ToString();
            }

            //List<CalenderObject> calendarObjects;
            //var range = new DateRange()
            //{
            //    StartDateTime = new DateTime(year, 1, 1),
            //    EndDateTime = new DateTime(year, 12, 31)
            //};

            careTypeIds = Server.UrlDecode(careTypeIds);
            employeeIds = Server.UrlDecode(employeeIds);
            referralIds = Server.UrlDecode(referralIds);

            ServiceResponse response = GetNurseSchedulesByCareTypeIdEmployeeIdReferralId(careTypeIds, employeeIds, referralIds);

            List<ScheduleViewModel> scheduleVM = (response.Data as IEnumerable<ScheduleViewModel>).ToList();
            List<ScheduleDTO> newSchedules = new List<ScheduleDTO>();
            //List<ScheduleDTO> finalizedSchedule = new List<ScheduleDTO>();
            foreach (var schedule in scheduleVM)
            {
                newSchedules.Add(new ScheduleDTO
                {
                    CareTypeId = schedule.CareTypeId,
                    ScheduleID = schedule.ScheduleID,
                    NurseScheduleID = schedule.NurseScheduleID,
                    EmployeeId = schedule.EmployeeId,
                    ReferralId = schedule.ReferralId,
                    StartDate = schedule.StartDate.Date,
                    OriginalStartDate = schedule.ScheduleStartDate,
                    Frequency = schedule.Frequency,
                    PayorId = schedule.PayorId,
                    CareType = schedule.CareType,
                    PayorName = schedule.PayorName,
                    EmployeeFullName = schedule.EmployeeFullName,
                    PatientFullName = schedule.PatientFullName,
                    ReferralBillingAuthorizationID = schedule.ReferralBillingAuthorizationID,
                    IsVirtualVisit = schedule.IsVirtualVisit,
                    IsFirstWeekOfMonthSelected = schedule.IsFirstWeekOfMonthSelected,
                    IsSecondWeekOfMonthSelected = schedule.IsSecondWeekOfMonthSelected,
                    IsThirdWeekOfMonthSelected = schedule.IsThirdWeekOfMonthSelected,
                    IsFourthWeekOfMonthSelected = schedule.IsFourthWeekOfMonthSelected,
                    IsLastWeekOfMonthSelected = schedule.IsLastWeekOfMonthSelected,
                    IsFridaySelected = schedule.IsFridaySelected,
                    IsMondaySelected = schedule.IsMondaySelected,
                    IsSaturdaySelected = schedule.IsSaturdaySelected,
                    IsSundaySelected = schedule.IsSundaySelected,
                    IsThursdaySelected = schedule.IsThursdaySelected,
                    IsTuesdaySelected = schedule.IsTuesdaySelected,
                    IsWednesdaySelected = schedule.IsWednesdaySelected,
                    ScheduleRecurrence = (RecurrencePattern)Enum.Parse(typeof(RecurrencePattern), schedule.ScheduleRecurrence),
                    EndDate = schedule.EndDate.Value.Date,
                    OriginalEndDate = schedule.ScheduleEndDate.Value.Date,
                    EndDateTime = schedule.EndDateTime,
                    IsEndDateNull = schedule.EndDate.ToString() != "1/1/1900 12:00:00 AM" ? false : true,
                    IsAnyDay = schedule.IsAnyDay,
                    Notes = schedule.Notes,
                    AnniversaryDay = schedule.AnniversaryDay,
                    AnniversaryMonth = schedule.AnniversaryMonth,
                    FrequencyChoice = schedule.FrequencyChoice,
                    FrequencyTypeOptions = schedule.FrequencyTypeOptions,
                    DaysOfWeek = schedule.DaysOfWeek,
                    DaysOfWeekOptions = (DayOfWeekEnum)Enum.Parse(typeof(DayOfWeekEnum), schedule.DaysOfWeekOptions),
                    MonthlyIntervalOptions = (MonthlyIntervalEnum)Enum.Parse(typeof(MonthlyIntervalEnum), schedule.MonthlyIntervalOptions),
                    DayOfMonth = schedule.DayOfMonth,
                    IsMonthlyDaySelection = schedule.IsMonthlyDaySelection,
                    DailyInterval = schedule.DailyInterval,
                    WeeklyInterval = schedule.WeeklyInterval,
                    MonthlyInterval = schedule.MonthlyInterval,
                    AnyTimeClockIn = schedule.AnyTimeClockIn,
                    ClockInStartTime = !schedule.AnyTimeClockIn ? schedule.ClockInStartTime.ToString() : null,
                    ClockInEndTime = !schedule.AnyTimeClockIn ? schedule.ClockInEndTime.ToString() : null,
                    TempDaysOfWeekOptions = schedule.DaysOfWeekOptions.ToString(),
                    TempMonthlyIntervalOptions = schedule.MonthlyIntervalOptions.ToString(),
                    TempScheduleRecurrence = schedule.ScheduleRecurrence.ToString(),
                    TempFrequencyTypeOptions = schedule.FrequencyTypeOptions.ToString()
                });

            }

            //foreach (var schedule in newSchedules)
            //{
            //    //if (schedule.EndDate.ToString() == "1/1/1900 12:00:00 AM")
            //    //    schedule.EndDate = null;

            //    calendarObjects = new List<CalenderObject>();
            //    calendarObjects
            //    .AddRange(schedule.NumberOfOccurrences > 0
            //    ? GetSpecificNumberOfOccurrencesForDateRange(schedule, range)
            //    : GetAllOccurrencesForDateRange(schedule, range));

            //    foreach (var obj in calendarObjects)
            //    {
            //        finalizedSchedule.Add(new ScheduleDTO
            //        {
            //            CareTypeId = schedule.CareTypeId,
            //            ScheduleID = schedule.ScheduleID,
            //            EmployeeId = schedule.EmployeeId,
            //            ReferralId = schedule.ReferralId,
            //            PayorId = schedule.PayorId,
            //            EmployeeFullName = schedule.EmployeeFullName,
            //            PatientFullName = schedule.PatientFullName,
            //            ReferralBillingAuthorizationID = schedule.ReferralBillingAuthorizationID,
            //            IsVirtualVisit = schedule.IsVirtualVisit,
            //            CareType = schedule.CareType,
            //            PayorName = schedule.PayorName,
            //            StartDate = obj.StartDate,
            //            EndDate = obj.EndDate,
            //            AnniversaryDay = schedule.AnniversaryDay,
            //            AnniversaryMonth = schedule.AnniversaryMonth,
            //            FrequencyChoice = schedule.FrequencyChoice,
            //            IsFirstWeekOfMonthSelected = schedule.IsFirstWeekOfMonthSelected,
            //            IsSecondWeekOfMonthSelected = schedule.IsSecondWeekOfMonthSelected,
            //            IsThirdWeekOfMonthSelected = schedule.IsThirdWeekOfMonthSelected,
            //            IsFourthWeekOfMonthSelected = schedule.IsFourthWeekOfMonthSelected,
            //            IsLastWeekOfMonthSelected = schedule.IsLastWeekOfMonthSelected,
            //            TempDaysOfWeekOptions = schedule.DaysOfWeekOptions.ToString(),
            //            TempMonthlyIntervalOptions = schedule.MonthlyIntervalOptions.ToString(),
            //            IsFridaySelected = schedule.IsFridaySelected,
            //            IsMondaySelected = schedule.IsMondaySelected,
            //            IsSaturdaySelected = schedule.IsSaturdaySelected,
            //            IsSundaySelected = schedule.IsSundaySelected,
            //            IsThursdaySelected = schedule.IsThursdaySelected,
            //            IsTuesdaySelected = schedule.IsTuesdaySelected,
            //            IsWednesdaySelected = schedule.IsWednesdaySelected,
            //            EndDateTime = schedule.EndDateTime,
            //            ScheduleRecurrence = schedule.ScheduleRecurrence,
            //            TempScheduleRecurrence = schedule.ScheduleRecurrence.ToString(),
            //            TempFrequencyTypeOptions = schedule.FrequencyTypeOptions.ToString(),
            //            IsEndDateNull = schedule.IsEndDateNull,
            //            OriginalEndDate = schedule.EndDate,
            //            OriginalStartDate = schedule.StartDate,
            //            IsAnyDay = schedule.IsAnyDay,
            //            Notes = schedule.Notes,
            //            AnyTimeClockIn = schedule.AnyTimeClockIn,
            //            ClockInStartTime = schedule.ClockInStartTime,
            //            ClockInEndTime = schedule.ClockInEndTime
            //        }
            //        );
            //    }

            //    calendarObjects = null;
            //}

            return Json(newSchedules, JsonRequestBehavior.AllowGet);
        }

        private ServiceResponse GetNurseSchedulesByCareTypeIdEmployeeIdReferralId(string careTypeIds, string employeeIds, string referralIds)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            ServiceResponse response = _scheduleDataProvider.GetNurseSchedules(careTypeIds, employeeIds, referralIds);
            return response;
        }
        private void LoadFrequencyChoices()
        {
            var list = new List<object>()
            {
                new { ID = 1, Name = "Daily" },
                new { ID = 2, Name = "Weekly" },
                new { ID = 3, Name = "Biweekly" },
                new { ID = 4, Name = "Monthly" }
            };

            ViewBag.FrequencyChoices = new SelectList(list, "ID", "Name");
        }

        private void LoadMonths()
        {
            var months = new List<object>()
            {
                new { ID = 1, Name = "January" },
                new { ID = 2, Name = "Feburary" },
                new { ID = 3, Name = "March" },
                new { ID = 4, Name = "April" },
                new { ID = 5, Name = "May" },
                new { ID = 6, Name = "June" },
                new { ID = 7, Name = "July" },
                new { ID = 8, Name = "August" },
                new { ID = 9, Name = "September" },
                new { ID = 10, Name = "October" },
                new { ID = 11, Name = "November" },
                new { ID = 12, Name = "December" }
            };

            ViewBag.Months = new SelectList(months, "ID", "Name");
        }

        private IEnumerable<CalenderObject> GetSpecificNumberOfOccurrencesForDateRange(ScheduleDTO scheduleDTO, DateRange range)
        {
            var calendarObjects = new List<CalenderObject>();
            var occurrences = scheduleDTO.Schedule.Occurrences(range).ToArray();
            for (var i = 0; i < scheduleDTO.NumberOfOccurrences; i++)
            {
                var date = occurrences.ElementAtOrDefault(i);
                calendarObjects.Add(new CalenderObject
                {
                    ID = scheduleDTO.ScheduleID,
                    StartDate = date + scheduleDTO.StartTime,
                    EndDate = date + scheduleDTO.EndTime,
                });
            }
            return calendarObjects;
        }

        private IEnumerable<CalenderObject> GetAllOccurrencesForDateRange(ScheduleDTO scheduleDTO, DateRange range)
        {
            var calendarObjects = new List<CalenderObject>();
            foreach (var date in scheduleDTO.Schedule.Occurrences(range))
            {
                calendarObjects.Add(new CalenderObject
                {
                    ID = scheduleDTO.ScheduleID,
                    StartDate = date + scheduleDTO.StartTime,
                    EndDate = date + scheduleDTO.EndTime,
                });
            }

            return calendarObjects;
        }

        private static DateTime FromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        private static long ToUnixTimestamp(DateTime date)
        {
            var ts = date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)Math.Truncate(ts.TotalSeconds);
        }


        #endregion

        #region Listen to events
        [HttpPost]
        public ActionResult Event(EventData model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            ServiceResponse response = _scheduleDataProvider.ProcessEvent(model);
            return CustJson(response);
        }
        #endregion

        #region Visit Reason
        [HttpPost]
        public ActionResult GetVisitReasonList(GetVisitReasonModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            ServiceResponse response = _scheduleDataProvider.GetVisitReasonList(model);
            return CustJson(response);
        }

        [HttpPost]
        public ActionResult SaveVisitReason(VisitReasonModel model)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            ServiceResponse response = _scheduleDataProvider.SaveVisitReason(model, SessionHelper.LoggedInID);
            return CustJson(response);
        }

        [HttpPost]
        public ActionResult GetVisitReasonModalDetail(long scheduleID)
        {
            _scheduleDataProvider = new ScheduleDataProvider();
            ServiceResponse response = _scheduleDataProvider.GetVisitReasonModalDetail(scheduleID);
            return CustJson(response);
        }
        #endregion

    }
}
