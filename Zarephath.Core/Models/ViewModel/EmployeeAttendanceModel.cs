using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class SaveClockInOutModel
    {
        public SaveClockInOutModel()
        {
            FacilityListModel = new List<FacilityListModel>();
            AttendanceDetailList = new List<AttendanceDetailList>();
            AttendanceBreakDetailList = new List<AttendanceBreakDetailList>();
            EmployeeAttendanceMaster = new EmployeeAttendanceMaster();
            EmployeeAttendanceDetail = new List<EmployeeAttendanceDetail>();
        }
        public List<FacilityListModel> FacilityListModel { get; set; }
        public List<AttendanceDetailList> AttendanceDetailList { get; set; }

        public List<AttendanceBreakDetailList> AttendanceBreakDetailList { get; set; }
        public EmployeeAttendanceMaster EmployeeAttendanceMaster { get; set; }
        public List<EmployeeAttendanceDetail> EmployeeAttendanceDetail { get; set; }
        public string TimeZone { get; set; }
        public DateTime CurrentTimeZoneDate { get; set; }
    }
    public class AttendanceDetailList
    {
        public string AttendanceDetailId { get; set; }
        public string AttendanceDetailName { get; set; }
    }
    public class AttendanceBreakDetailList
    {
        public long AttendanceBreakDetailId { get; set; }
        public string AttendanceBreakDetailName { get; set; }
    }
    public class EmployeeAttendanceModel
    {
        public int? ScheduleID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //public int? Id { get; set; }
        public long EmployeeID { get; set; }
        public int WorkMinutes { get; set; }
        public long FacilityID { get; set; }
        public long OrganizationID { get; set; }
        public string Note { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public string EmployeeName { get; set; }
        //public DateTime? ClockInTime { get; set; }
        //public DateTime? ClockOutTime { get; set; }
        public string FacilityName { get; set; }
        public int BreakMinutes { get; set; }

    }

    public class EmployeeAttendanceCalendarModel
    {
        public List<EmployeeAttendanceModel> EmployeeAttendanceModels { get; set; }
        //public List<EmployeeAttendanceDetail> EmployeeAttendanceDetail { get; set; }
    }
}
