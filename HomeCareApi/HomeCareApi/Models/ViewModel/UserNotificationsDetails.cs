using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeCareApi.Models.ViewModel
{
    public class UserNotificationsDetails
    {
        public long Row { get; set; }
        public string Title { get; set; }
        public int NotificationType { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public long PrimaryId { get; set; }
        public bool IsNotificationRead { get; set; }
        public long Count { get; set; }
        public long NotificationId { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsArchieved { get; set; }
    }

    public class MarkAsReadNotification
    {
        public long NotificationID { get; set; }
        public long EmployeeID { get; set; }
        public string Action { get; set; }
    }

    public class SearchNotificationModel
    {
        public int NotificationType { get; set; }
    }
}