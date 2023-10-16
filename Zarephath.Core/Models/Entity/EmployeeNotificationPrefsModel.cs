using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;
using System;

namespace Zarephath.Core.Models.Entity
{
    [TableName("[notif].[EmployeeNotificationPreferences]")]
    [PrimaryKey("EmployeeNotificationPreferenceID")]
    [Sort("EmployeeNotificationPreferenceID", "DESC")]
    public class EmployeeNotificationPrefsModel : BaseEntity
    {
        public long EmployeeNotificationPreferenceID { get; set; }
        public long EmployeeID { get; set; }
        public bool SendEmail { get; set; }
        public bool SendSMS { get; set; }
        public bool WebNotification { get; set; }
        public bool MobileAppNotification { get; set; }
    }
}
