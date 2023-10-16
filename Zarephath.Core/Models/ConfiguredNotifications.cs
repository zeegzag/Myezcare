using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zarephath.Core.Models
{
    class ConfiguredNotifications
    {
        public long NotificationConfigurationID { get; set; }
        public long NotificationID { get; set; }
        public string EmailRecipients { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string WebNotificationEmployeeIds { get; set; }
        public string MobileAppNotificationEmployeeIds { get; set; }
        public string SMSRecipients { get; set; }
        public string SMSText { get; set; }
    }
}
