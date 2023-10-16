using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models.ViewModel
{
    public class MobileNotificationModel
    {
        public string Body { get; set; }
        public string SiteName { get; set; }
        public int NotificationType { get; set; }
        public int PrimaryId { get; set; }
    }

    public class PatientResignatureNotificationModel
    {
        public string Body { get; set; }
        public string SiteName { get; set; }
        public int NotificationType { get; set; }
        public int PrimaryId { get; set; }
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public bool Editable { get; set; }
    }
}
