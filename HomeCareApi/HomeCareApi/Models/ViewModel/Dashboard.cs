using System;
using System.Collections.Generic;
using HomeCareApi.Infrastructure;
using HomeCareApi.Models.Entity;
using HomeCareApi.Controllers;
using Newtonsoft.Json;

namespace HomeCareApi.Models.ViewModel
{
    public class DashboardDetail
    {
        public DashboardDetail()
        {
            Employee = new Employee();
            TodayVisits = new List<UpcomingVisit>();
            NextDayVisits = new List<UpcomingVisit>();
        }
        public Employee Employee { get; set; }
        public List<UpcomingVisit> TodayVisits { get; set; }
        public List<UpcomingVisit> NextDayVisits { get; set; }
    }

    public class UpcomingVisit
    {
        public long ReferralID { get; set; }
        public long ScheduleID { get; set; }
        public long EmployeeID { get; set; }
        public string ImageUrl { get; set; }
        //public string ImageUrl => string.Format(Constants.http+ConfigSettings.UserImageInitialPath, FirstName[0]);
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public bool IsDenied { get; set; }
        //[JsonConverter(typeof(CustomUTCDateTimeConverter))]
        public DateTime StartDate { get; set; }
        //[JsonConverter(typeof(CustomUTCDateTimeConverter))]
        public DateTime EndDate { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public bool AnyTimeClockIn { get; set; }
        public long ScheduleStatusID { get; set; }
        public long IsPCACompleted { get; set; }
        public string CareType { get; set; }
        public int VisitType { get; set; }
    }

    public class PatientList
    {
        public long ReferralID { get; set; }
        public string ImageUrl { get; set; }
        public string PatientName { get; set; }
        public string FirstCharOfName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long Row { get; set; }
        public long Count { get; set; }
    }

    public class CaseLoadAppointment
    {
        public long EmployeeID { get; set; }
    }

    public class PatientAppointment
    {
        public long ReferralID { get; set; }
        public long CareTypeTimeSlotID { get; set; }
        public string ImageUrl { get; set; }
        public string PatientName { get; set; }
        public string FirstCharOfName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class ReferralGroupList
    {
        public long ReferralGroupID { get; set; }
        public string GroupName { get; set; }
        public string EmployeeID { get; set; }
        public long Row { get; set; }
        public long Count { get; set; }
    }
}