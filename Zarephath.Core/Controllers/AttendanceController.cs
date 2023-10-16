using System.Web.Mvc;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.DataProvider;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Models.ViewModel;

namespace Zarephath.Core.Controllers
{
    public class AttendanceController : BaseController
    {
        private IAttendanceDataProvider _attendanceDataProvider;

        #region Attendance Master

        [HttpGet]
        [CustomAuthorize(Permissions = Constants.Permission_Attendance_Permission)]
        public ActionResult AttendanceMaster()
        {
            _attendanceDataProvider = new AttendanceDataProvider();
            return View(_attendanceDataProvider.SetAttendanceMasterModel().Data);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Attendance_Permission)]
        public JsonResult GetAttendanceListByFacility(AttendanceMasterSearchModel searchParam)
        {
            _attendanceDataProvider = new AttendanceDataProvider();
            var response = _attendanceDataProvider.GetAttendanceListByFacility(searchParam);
            return JsonSerializer(response);
        }

        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Attendance_Permission)]
        public ContentResult UpdateAttendance(AttendanceDetail attendanceDetail)
        {
            _attendanceDataProvider = new AttendanceDataProvider();
            var response = _attendanceDataProvider.UpdateAttendance(attendanceDetail, SessionHelper.LoggedInID);
            return CustJson(response);
        }


        [HttpPost]
        [CustomAuthorize(Permissions = Constants.Permission_Attendance_Permission)]
        public ContentResult UpdateCommentForAttendance(AttendanceDetail attendanceDetail)
        {
            _attendanceDataProvider = new AttendanceDataProvider();
            var response = _attendanceDataProvider.UpdateCommentForAttendance(attendanceDetail, SessionHelper.LoggedInID);
            return CustJson(response);
        }
        #endregion
    }
}
