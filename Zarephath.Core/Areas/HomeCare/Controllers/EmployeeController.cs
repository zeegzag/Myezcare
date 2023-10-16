using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zarephath.Core.Controllers;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Infrastructure.Utility.CareGiverApi;
using Zarephath.Core.Models;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;
using Zarephath.Core.Infrastructure.Utility;
namespace Zarephath.Core.Areas.HomeCare.Controllers
{
    public class EmployeeController : BaseController
    {
        CacheHelper _cacheHelper = new CacheHelper();
        private IEmployeeDataProvider _employeeDataProvider;

        #region Add Employee 

        /// <summary>
        /// This is the Add Employee Page. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public ActionResult AddEmployee(string id)
        {


            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SetAddEmployeePage(employeeID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public ActionResult PartialAddEmployee(string id)
        {

            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SetAddEmployeePage(employeeID);
            ViewBag.IsPartialView = true;
            return View("AddEmployee", response.Data);
          //  return ShowUserFriendlyPages(response) ?? View(response.Data);
        }


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public ActionResult AddEmployee11(string id)
        {

            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SetAddEmployeePage(employeeID);
            return ShowUserFriendlyPages(response) ?? View("AddEmployee", response.Data);
        }

        /// <summary>
        /// This method will add the new employee in the database.
        /// </summary>
        /// <param name="addEmployeeModel"></param>
        /// <returns></returns>
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult AddEmployee(HC_AddEmployeeModel addEmployeeModel)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.HC_AddEmployee(addEmployeeModel, SessionHelper.LoggedInID));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult AddEmployee11(HC_AddEmployeeModel addEmployeeModel)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.HC_AddEmployee(addEmployeeModel, SessionHelper.LoggedInID));
        }

        [HttpPost]
        public JsonResult AddEmployeeLogin(HC_AddEmployeeModel addEmployeeModel)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.HC_EmployeeLogin(addEmployeeModel));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult ResendRegistrationMail(string id)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.HC_ResendRegistrationMail(Convert.ToInt64(id)));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult ResendRegistrationSMS(string id)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.HC_ResendRegistrationSMS(Convert.ToInt64(id)));
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.UploadFile(HttpContext.Request);
            return Json(response);
        }

        [HttpPost]
        public JsonResult AddSSNLog(HC_AddEmployeeModel model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_AddEmployeeSSNLog(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }


        #region Employee Preferences

        [HttpPost]
        public JsonResult SearchSkill(string searchText, int pageSize)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.GetSearchSkill(pageSize, searchText));
        }

        //[HttpPost]
        ////[CustomAuthorize(Permissions = Constants.Permission_Employee_AddUpdate)]
        //public JsonResult AddPreference(Preference preference)
        //{
        //    _employeeDataProvider = new EmployeeDataProvider();
        //    return Json(_employeeDataProvider.AddPreference(preference));
        //}

        [HttpPost]
        public JsonResult DeletePreference(EmployeePreferenceModel model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.DeletePreference(model));
        }

        #endregion





        #endregion

        #region Employee List

        /// <summary>
        /// For Get list of employees, this action is called
        /// </summary>
        /// <returns>Returns "DepartmentDropdownModel" for fill department dropdownlist </returns>
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_List)]
        public ActionResult EmployeeList()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.SetListEmployeePage().Data;
            return View("employeeList", response);
        }

        /// <summary>
        /// for get list of employee, this action will be called.
        /// </summary>
        /// <param name="searchEmployeeModel">SearchEmployeeModel is for search filter.</param>
        /// <param name="pageIndex">pageIndex is a page no. for Get data.</param>
        /// <param name="pageSize">pageSize is size for get given pagesize of data for current page</param>
        /// <param name="sortIndex">SortType is for "ASC,DESC". This will changed when clicked on list table header(th tag) for ascending descending order.</param>
        /// <param name="sortDirection">sortDirection is for which column want to ascending or descinding.</param>
        /// <returns>Returns list of employee list if found  else return perticular message.</returns>
        [HttpPost]
        public JsonResult GetEmployeeList(SearchEmployeeModel searchEmployeeModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetEmployeeList(searchEmployeeModel, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// On Delete/DeleteAll employee, this action will be called.
        /// </summary>
        /// <param name="searchEmployeeModel">SearchEmployeeModel is for search filter.</param>
        /// <param name="pageIndex">pageIndex is a page no. for Get data.</param>
        /// <param name="pageSize">pageSize is size for get given pagesize of data for current page</param>
        /// <param name="sortIndex">SortType is for "ASC,DESC". This will changed when clicked on list table header(th tag) for ascending descending order.</param>
        /// <param name="sortDirection">sortDirection is for which column want to ascending or descinding.</param>
        /// <returns>Returns list of employee list with delete success message.</returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Delete)]
        public JsonResult DeleteEmployee(SearchEmployeeModel searchEmployeeModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.DeleteEmployee(searchEmployeeModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion

        #region Employee Days with time slots

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        public ActionResult EmployeeTimeSlots()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_EmployeeTimeSlots();
            return ShowUserFriendlyPages(response) ?? View(response.Data);

        }

        [HttpGet]
        public ActionResult PartialEmployeeTimeSlots(string id)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_EmployeeTimeSlots();

            var model = (HC_ETSModel)response.Data;
            model.SearchETSMaster.EmployeeID = Convert.ToInt64(id);
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("EmployeeTimeSlots", response.Data);

        }
        public ActionResult PartialBulkEmployeeTimeSlots(string id)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_EmployeeTimeSlots();

            var model = (HC_ETSModel)response.Data;
            model.SearchETSMaster.EmployeeID = Convert.ToInt64(id);
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("BulkEmployeeTimeSlots", response.Data);

        }


        #region ETS Master

        [HttpPost]
        public JsonResult GetEtsMasterlist(SearchETSMaster searchETSMaster, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetEtsMasterlist(searchETSMaster, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        public JsonResult DeleteEtsMaster(SearchETSMaster searchETSMaster, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.DeleteEtsMaster(searchETSMaster, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        public JsonResult AddEtsMaster(EmployeeTimeSlotMaster etsMaster)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.AddEtsMaster(etsMaster, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion


        #region ETS Detail

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        public JsonResult GetEtsDetaillist(SearchETSDetail searchETSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetEtsDetaillist(searchETSDetail, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        public JsonResult DeleteEtsDetail(SearchETSDetail searchETSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.DeleteEtsDetail(searchETSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        public JsonResult AddEtsDetailBulk(BulkEmployeeTimeSlotDetails bulkEtsDetail)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.AddEtsDetailBulk(bulkEtsDetail, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        public JsonResult AddEtsDetail(EmployeeTimeSlotDetail etsDetail)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.AddEtsDetail(etsDetail, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Schedule)]
        public JsonResult UpdateEtsDetail(EmployeeTimeSlotDetail etsDetail, SearchETSDetail searchETSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.UpdateEtsDetail(etsDetail, searchETSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        public JsonResult EmployeeTimeSlotForceUpdate(EmployeeTimeSlotDetail etsDetail, SearchETSDetail searchETSDetail, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.EmployeeTimeSlotForceUpdate(etsDetail, searchETSDetail, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }

        #endregion





        #endregion

        #region Employee Calender

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Calendar)]
        public ActionResult EmployeeCalender()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_EmployeeCalender();
            var model = (HC_EmpCalenderModel)response.Data;
            model.IsPartial = false;
            return ShowUserFriendlyPages(response) ?? View(response.Data);


        }

        [HttpGet]
        public ActionResult PartialEmployeeCalender(string id)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_EmployeeCalender();
            var model = (HC_EmpCalenderModel)response.Data;
            model.SearchEmpCalender.EmployeeID = new List<string>() { id };
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("EmployeeCalender", response.Data);

        }


        #endregion

        #region Employee Day Off

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_PTO)]
        public ActionResult EmployeeDayOff()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_EmployeeDayOffPage();
            var model = (SetEmpDayOffListPage)response.Data;
            model.IsPartial = false;
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public ActionResult PartialEmployeeDayOff(string id)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_EmployeeDayOffPage();
            var model = (SetEmpDayOffListPage)response.Data;
            model.SearchEmployeeDayOffModel.EmployeeID = Convert.ToInt64(id);
            model.IsPartial = true;
            return ShowUserFriendlyPages(response) ?? View("EmployeeDayOff", response.Data);

        }



        [HttpPost]
        public JsonResult GetEmployeeDayOffList(SearchEmpDayOffModel searchEmpDayOffModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_GetEmployeeDayOffList(searchEmpDayOffModel, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_PTO)]
        public JsonResult DeleteEmployeeDayOff(SearchEmpDayOffModel searchEmpDayOffModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_DeleteEmployeeDayOffList(searchEmpDayOffModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return Json(response);
        }



        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_PTO)]
        public ActionResult SaveEmployeeDayOff(EmployeeDayOff model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SaveEmployeeDayOff(model, SessionHelper.LoggedInID);
            return Json(response);

        }



        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.Permission_Employee_AddUpdate)]
        public ActionResult CheckForEmpSchedules(EmployeeDayOff model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_CheckForEmpSchedules(model);
            return Json(response);

        }




        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Employee_PTO)]
        public ActionResult DayOffAction(EmployeeDayOff model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_DayOffAction(model, SessionHelper.LoggedInID);
            return Json(response);

        }







        #endregion

        #region EmployeeNotes Tab
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult SaveEmployeeNote(string EmployeeId, string NoteDetail, bool IsEdit = false, string CommonNoteID = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();

            // long EmployeeId = Convert.ToInt64(Crypto.Decrypt(EmployeeId));
            long commonNoteID = string.IsNullOrEmpty(CommonNoteID) ? 0 : Convert.ToInt64(CommonNoteID);

            ServiceResponse response = _employeeDataProvider.HC_SaveEmployeeNotes(Convert.ToInt64(EmployeeId), NoteDetail, SessionHelper.LoggedInID, commonNoteID, IsEdit);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult GetEmployeeNotes(string EmployeeId)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            //            long EmployeeId = Convert.ToInt64(Crypto.Decrypt(EmployeeId));
            ServiceResponse response = _employeeDataProvider.GetEmployeeNotes(Convert.ToInt64(EmployeeId));
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult DeleteEmployeeNote(string CommonNoteID)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            long commonNoteID = Convert.ToInt64(CommonNoteID);
            ServiceResponse response = _employeeDataProvider.DeleteEmployeeNote(commonNoteID, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        #endregion EmployeeNotes Tab

        #region Employee Checklist

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult GetEmployeeChecklist(string EmployeeID)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_GetEmployeeChecklist(EmployeeID);
            return JsonSerializer(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult SaveEmployeeChecklist(EmployeeChecklist model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SaveEmployeeChecklist(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion Employee Checklist

        #region Employee Notification Prefs

        [HttpPost]
        public JsonResult GetEmployeeNotificationPrefs(string EmployeeID)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_GetEmployeeNotificationPrefs(EmployeeID);
            return JsonSerializer(response);
        }

        [HttpPost]
        public JsonResult SaveEmployeeNotificationPrefs(EmployeeNotificationPrefsModel model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SaveEmployeeNotificationPrefs(model, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }

        #endregion Employee Notification Prefs

        #region Send BULK SMS To Employees


        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_GroupSMS)]
        public ActionResult SendBulkSms()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SendBulkSms();
            return View(response.Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_GroupSMS + "," + Constants.HC_Permission_Message_BroadcastNotifications)]
        public JsonResult GetEmployeeListForSendSms(SearchSBSEmployeeModel model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_GetEmployeeListForSendSms(model);
            return Json(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_GroupSMS)]
        public JsonResult SendBulkSms(SendSMSModel model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_SendBulkSms(model, SessionHelper.LoggedInID);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_GroupSMS)]
        public JsonResult GetSentSmsList(SearchSentSmsModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetSentSmsList(model, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_GroupSMS)]
        public JsonResult GetEmployeesForSentSms(long groupMessageLogId)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetEmployeesForSentSms(groupMessageLogId);
            return Json(response);
        }

        #endregion

        #region Sent & Received Mesages

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_ReceivedMessage)]
        public ActionResult ReceivedMessages()
        {

            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SendBulkSms();
            return View(response.Data);
        }

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_SentMessage)]
        public ActionResult SentMessages()
        {

            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SendBulkSms();
            return View(response.Data);
        }

        #endregion

        #region Upload Employee Signature From API
        [HttpPost]
        public JsonResult UploadEmpSignature(byte[] bytes)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.UploadEmpSignature(HttpContext.Request);
            return Json(response);
        }
        #endregion

        #region Broadcast Notification

        /// <summary>
        /// broadcast notification is used to send custom FCM notification to mobile.
        /// This is get method for render page
        /// </summary>
        /// <param name="t">Type for table or where we need to check</param>
        /// <param name="v">value means encryptedId</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_BroadcastNotifications)]
        public ActionResult BroadcastNotification(string id = "", string id1 = "")
        {
            long TableId = !string.IsNullOrWhiteSpace(id1) ? Convert.ToInt64(Crypto.Decrypt(id1)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            CacheHelper cacheObj = new CacheHelper();
            ServiceResponse response = _employeeDataProvider.HC_BroadcastNotification(id, TableId, cacheObj.SiteName);
            return View(response.Data);
        }

        /// <summary>
        /// this method is used to broadcast FCM notification to selected employee.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_BroadcastNotifications)]
        public JsonResult BroadcastNotification(SendSMSModel model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_SaveBroadcastNotification(model, SessionHelper.LoggedInID, SessionHelper.DomainName);
            return Json(response);
        }

        /// <summary>
        /// this is used to fetch sent broadcast notification
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortIndex"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_BroadcastNotifications)]
        public JsonResult GetBroadcastNotification(SearchSentSmsModel model, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_GetBroadcastNotification(model, pageIndex, pageSize, sortIndex, sortDirection);
            return Json(response);
        }

        /// <summary>
        /// this method is used to check how many employee are selected while sending notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.HC_Permission_Message_BroadcastNotifications)]
        public JsonResult GetEmployeesForBroadcastNotifications(SearchSBSEmployeeModel searchSBSEmployeeModel)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_GetEmployeesForBroadcastNotifications(searchSBSEmployeeModel);
            return Json(response);
        }

        [HttpGet]
        public JsonResult SendNotification()
        {
            EmployeeDataProvider employeeDataProvider = new EmployeeDataProvider();

            return Json(null);
        }

        #endregion

        #region AuditLog
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_AddUpdate)]
        public JsonResult GetAuditLogList(SearchRefAuditLogListModel searchModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetAuditLogList(searchModel, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID);
            return JsonSerializer(response);
        }
        #endregion


        #region Employee Document
        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_View_AddUpdate)]
        public JsonResult UploadEmployeeDocument()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SaveEmployeeDocument(HttpContext.Request);
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_View_AddUpdate)]
        public JsonResult GetEmployeeDocumentList(string id, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            long EmployeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            return JsonSerializer(_employeeDataProvider.HC_GetEmployeeDocumentList(EmployeeID, pageIndex, pageSize, sortIndex, sortDirection));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult DeleteEmployeeDocument(long id, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return JsonSerializer(_employeeDataProvider.HC_DeleteEmployeeDocument(id, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_ReferralDocuments_AddUpdate)]
        public JsonResult SaveEmployeeDocument(ReferralDocument id, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return JsonSerializer(_employeeDataProvider.HC_SaveEmployeeDocument(id, pageIndex, pageSize, sortIndex, sortDirection, SessionHelper.LoggedInID));
        }
        #endregion

        [HttpGet]
        public ActionResult GenerateCertificateForEmployee(string id)
        {
            long EmployeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_GenerateCertificateForEmployee(EmployeeID);
            return View(response.Data);
        }

        [HttpPost]
        public JsonResult GetUserCertificates(string email)
        {

            ServiceResponse response = new ServiceResponse();
            CareGiverApi careGiverApi = new CareGiverApi();
            response = careGiverApi.GetUserCertificates(email);
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetEmployeesList(string id)
        {
            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SetAddEmployeePage(employeeID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult SendRegistration()
        {
            long employeeID = 0;//!string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SetAddEmployeePage(employeeID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }



        //[HttpPost]
        //public JsonResult GetEmployeesList(SearchEmployeeModel searchEmployeeModel, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        //{

        //    ServiceResponse response = new ServiceResponse();
        //    CareGiverApi careGiverApi = new CareGiverApi();
        //    response = careGiverApi.GetEmployeesList(searchEmployeeModel, pageIndex, pageSize, sortIndex, sortDirection);
        //    return Json(response, JsonRequestBehavior.AllowGet);

        //}

        #region Contact Information Tab

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult AddEmployeeContact(HC_AddEmployeeModel addEmployeeModel)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.AddEmployeeContact(addEmployeeModel, SessionHelper.LoggedInID));
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Patient_AddUpdate)]
        public JsonResult DeleteEmployeecontact(long contactMappingID)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.DeteteEmployeeContact(contactMappingID, SessionHelper.LoggedInID));
        }
        #endregion

        #region "Employee Over Time Pay Report"
        [HttpPost]
        
        public ContentResult GetEmployeeOverTimePayBillingReportList(SearchEmployeeBillingReportListPage searchEmployeeBillingReportListPage, int pageIndex = 1, int pageSize = 10, string sortIndex = "", string sortDirection = "")
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return CustJson(_employeeDataProvider.GetEmployeeOverTimePayBillingReportList(searchEmployeeBillingReportListPage, pageIndex, pageSize, sortIndex, sortDirection));
        }
        [HttpPost]
        public JsonResult SaveRegularHours(RegularHoursModel model)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            return Json(_employeeDataProvider.SaveRegularHours(model));
        }

        #endregion;
        #region "Employee ID Card"
        [HttpGet]
        public ActionResult EmployeeIDCard(string id)
        {
            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_SetAddEmployeePage(employeeID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        [HttpGet]
        public ActionResult GenerateEmployeeIDCard(string id)
        {
            string url = string.Format("{0}{1}{2}", _cacheHelper.SiteBaseURL, Constants.HC_EmployeeIDCard, id);
            SelectHtmlToPdf data = new SelectHtmlToPdf();
            byte[] pdf = data.GenerateHtmlUrlToPdf(url);
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = String.Format("{0}_{1}.pdf", "Employee ID Card", DateTime.Now.ToString(Constants.ReadableFileNameDateTimeFormat));
            return fileResult;

        }
        #endregion

        [HttpPost]
        public JsonResult SendRegistration(string empIds)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.HC_BulkResendRegistrationMail(empIds);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEmployeeGroupList()
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetEmployeeGroup("Employee Group");
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateBulkEmployeeGroup(string empIds,string[] groupId)
        {
            string groupids = "";
            for(int i=0;i<groupId.Length;i++)
            {
                groupids += groupId[i].ToString()+",";
            }
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.UpdateBulkGroup(empIds, groupids);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SaveEmployeeEmailSignature(string EmployeeID,string Name,string Description)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.SaveEmployeeEmailSignature(EmployeeID, Name, Description, SessionHelper.LoggedInID.ToString());
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeEmailSignature(string EmployeeID)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.GetEmployeeEmailSignature(EmployeeID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
