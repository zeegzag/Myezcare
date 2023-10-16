using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.ViewModel
{
    public class CaseManagementModel
    {
    }

    public class CareType
    {
        public long CareTypeID { get; set; }
        public string CareTypeName { get; set; }
    }

    public class CareTypeSchedule
    {
        public long CareTypeID { get; set; }
        public long ReferralID { get; set; }
        public long EmployeeID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}