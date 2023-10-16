using System;
using System.Net;
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
    [CustomAuthorize(Permissions = Constants.AnonymousLoginPermission)]
    public class HomeController : BaseController
    {
        private IHomeDataProvider _iHomeDataProvider;

        [HttpGet]
        public ActionResult Dashboard()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_SetDashboardPage(SessionHelper.LoggedInID);
            return View(response.Data);
        }

        [HttpPost]
        public JsonResult GetEmpClockInOutList(DateTime? startDate, DateTime? endDate, string employeeName, string careTypeID, string status, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "", string TimeSlots = "", string RegionID = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetEmpClockInOutList(SessionHelper.LoggedInID, startDate, endDate, employeeName, careTypeID, status, sortIndex, sortDirection, pageIndex, pageSize, TimeSlots, RegionID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPatientAddressList(DateTime? startDate, DateTime? endDate, string employeeName, int pageIndex = 1, int pageSize = 100, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetPatientAddressList(SessionHelper.LoggedInID, startDate, endDate, employeeName, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetEmpOverTimeList(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetEmpOverTimeList(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }



        [HttpPost]
        public JsonResult GetPatientNewList(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientNewList(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }
        [HttpPost]
        public JsonResult GetActivePatientCountList()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetActivePatientCountList();
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult GetPatientNotScheduleList(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientNotScheduleList(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }


        [HttpPost]
        public JsonResult GetEmployeeTimeStatics(DateTime? startDate, DateTime? endDate)
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetEmployeeTimeStatics(startDate, endDate);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult ReferralInternalMessageList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralInternalMessageList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetReferralResolvedInternalMessageList(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetReferralResolvedInternalMessageList(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPendingBypassVisit()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetPendingBypassVisit();
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult MarkResolvedMessageAsRead(string EncryptedReferralInternalMessageID, long ReferralID, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {

            long referralInternalMessageId = Convert.ToInt64(Crypto.Decrypt(EncryptedReferralInternalMessageID));

            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.MarkResolvedMessageAsRead(referralInternalMessageId, ReferralID, SessionHelper.LoggedInID,
                sortDirection, sortIndex, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public void ChangeOrgType(string OrgType)
        {
            OrgType = OrgType.Replace(" ", string.Empty);
            SessionHelper.IsHomeCare = false;
            SessionHelper.IsDayCare = false;
            SessionHelper.IsPrivateDutyCare = false;
            SessionHelper.IsCaseManagement = false; SessionHelper.IsRAL = false;
            SessionHelper.IsRAL = false;
            SessionHelper.IsStaffing = false;

            if (OrgType.ToLower() == MyEzcareOrganization.AgencyType.HomeCare.ToString().ToLower())
            {
                SessionHelper.IsHomeCare = true;
            }
            else if (OrgType.ToLower() == MyEzcareOrganization.AgencyType.DayCare.ToString().ToLower())
            {
                SessionHelper.IsDayCare = true;
            }
            else if (OrgType.ToLower() == MyEzcareOrganization.AgencyType.RAL.ToString().ToLower())
            {
                SessionHelper.IsRAL = true;
            }
            else if (OrgType.ToLower() == MyEzcareOrganization.AgencyType.PrivateDutyCare.ToString().ToLower())
            {
                SessionHelper.IsPrivateDutyCare = true;
            }
            else if (OrgType.ToLower() == MyEzcareOrganization.AgencyType.CaseManagement.ToString().ToLower())
            {
                SessionHelper.IsCaseManagement = true;
            }
            else if (OrgType.ToLower() == MyEzcareOrganization.AgencyType.Staffing.ToString().ToLower())
            {
                SessionHelper.IsStaffing = true;
            }
        }

        public ActionResult Notification()
        {
            return View();
        }
        public JsonResult GetLateClockInNotification()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetNotification();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region Web Notifications

        /// <summary>
        /// This method will gets all web notifications for the logged in user.
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Dashboard_WebNotifications)]
        public JsonResult GetWebNotifications()
        {
            _iHomeDataProvider = new HomeDataProvider();
            return Json(_iHomeDataProvider.HC_GetWebNotifications(SessionHelper.LoggedInID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method will delete the specific web notification for the logged in user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomAuthorize(Permissions = Constants.HC_Permission_Dashboard_WebNotifications)]
        public JsonResult DeleteWebNotification(string ids)
        {
            _iHomeDataProvider = new HomeDataProvider();
            return Json(_iHomeDataProvider.HC_DeleteWebNotification(ids, SessionHelper.LoggedInID), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method will mark as read the specific web notifications for the logged in user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Dashboard_WebNotifications)]
        public JsonResult MarkAsReadWebNotifications(string webNotificationIDs, bool IsRead)
        {
            _iHomeDataProvider = new HomeDataProvider();
            return Json(_iHomeDataProvider.HC_MarkAsReadWebNotifications(webNotificationIDs, SessionHelper.LoggedInID, IsRead), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWebNotificationsCount()
        {
            _iHomeDataProvider = new HomeDataProvider();
            return Json(_iHomeDataProvider.HC_GetWebNotificationsCount(SessionHelper.LoggedInID), JsonRequestBehavior.AllowGet);
        }

        #endregion
        public ActionResult WebNotification()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetNotificationsList(string IsDeleted, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetNotificationsList(SessionHelper.LoggedInID, IsDeleted, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPriorAuthExpiring(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPriorAuthExpiring(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize, false);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPriorAuthExpired(int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPriorAuthExpiring(SessionHelper.LoggedInID, sortIndex, sortDirection, pageIndex, pageSize, true);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPatientBirthday(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientBirthday(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetEmployeeBirthday(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetEmployeeBirthday(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPatientClockInOutList(DateTime? startDate, DateTime? endDate, string patientName, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientClockInOutList(SessionHelper.LoggedInID, startDate, endDate, patientName, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpGet]
        public JsonResult GetReferralPayor()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetReferralPayor(SessionHelper.LoggedInID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPatientDischargedList(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientDischargedList(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPatientTransferList(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientTransferList(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPatientPendingList(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientPendingList(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPatientOnHoldList(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientOnHoldList(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult GetPatientMedicaidList(DateTime? startDate, DateTime? endDate, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetPatientMedicaidList(SessionHelper.LoggedInID, startDate, endDate, sortIndex, sortDirection, pageIndex, pageSize);
            return JsonSerializer(response);
        }

        [HttpGet]
        public JsonResult GetCaretype()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetCaretype();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetRegion()
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.GetRegion();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmpClockInOutListWithOutStatus(DateTime? startDate, DateTime? endDate)
        {
            _iHomeDataProvider = new HomeDataProvider();
            ServiceResponse response = _iHomeDataProvider.HC_GetEmpClockInOutListWithOutStatus(SessionHelper.LoggedInID, startDate, endDate);
            return JsonSerializer(response);
        }

    }
}
