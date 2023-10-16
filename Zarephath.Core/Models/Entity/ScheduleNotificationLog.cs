using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ScheduleNotificationLogs")]
    [PrimaryKey("ScheduleNotificationID")]
    [Sort("ScheduleNotificationID", "DESC")]
    public class ScheduleNotificationLog
    {
        public long ScheduleNotificationID { get; set; }
        public long ReferralID { get; set; }
        public long ScheduleID { get; set; }
        public string NotificationType { get; set; }
        public string NotificationSubType { get; set; }
        public string Source { get; set; }
        public string FromEmailAddress { get; set; }
        public string ToEmailAddress { get; set; }
        public string FromPhone { get; set; }
        public string ToPhone { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? CreatedBy { get; set; }

    }
}
