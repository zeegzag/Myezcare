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
    public class AttendanceController : BaseController
    {
        CacheHelper _cacheHelper = new CacheHelper();
        private IEmployeeDataProvider _employeeDataProvider;


        #region "View"
        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Attendance_Clockinout)]
        public ActionResult clockinout(string id)
        {
            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            //HC_ClockInOut
            ServiceResponse response = _employeeDataProvider.HC_ClockInOut(employeeID);
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }

        [HttpGet]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Attendance_Calender)]
        public ActionResult calendar(string id)
        {
            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_EmployeeCalender();
            var model = (HC_EmpCalenderModel)response.Data;
            model.IsPartial = false;
            return ShowUserFriendlyPages(response) ?? View(response.Data);
        }
        #endregion "View"

        #region "API"
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Attendance_Clockinout)]
        public JsonResult saveclockinout(SaveClockInOutModel saveClockInOutModel)
        {
            _employeeDataProvider = new EmployeeDataProvider();
            var response = _employeeDataProvider.SaveClockInOut(saveClockInOutModel);
           
            return Json(response);
        }

        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Attendance_Clockinout)]
        public JsonResult getclockinout(string id)
        {
            long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.HC_ClockInOut(employeeID);
            return Json(response);
        }
        [HttpPost]
        //[CustomAuthorize(Permissions = Constants.HC_Permission_Employee_Attendance_Clockinout)]
        public JsonResult EmployeeAttendanceCalendar(SearchRefCalender model)
        {
            //long employeeID = !string.IsNullOrEmpty(id) ? Convert.ToInt64(Crypto.Decrypt(id)) : 0;
            _employeeDataProvider = new EmployeeDataProvider();
            ServiceResponse response = _employeeDataProvider.EmployeeAttendanceCalendar(model);
            return Json(response);
        }
        
        #endregion "API"
    }
}
