using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Mobile_Notifications")]
    [PrimaryKey("NotificationId")]
    [Sort("NotificationId", "DESC")]
    public class Mobile_UserNotification
    {
        [Required]
        public long UserNotificationId { get; set; }

        [Required]
        public long EmployeeID { get; set; }

        [Required]
        public long NotificationId { get; set; }

        [Required]
        public int NotificationStatus { get; set; }

        public long? PrimaryId { get; set; }
    }
}
