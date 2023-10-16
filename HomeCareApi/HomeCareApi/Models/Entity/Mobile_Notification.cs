using HomeCareApi.Resources;
using PetaPoco;
using System;
using System.ComponentModel.DataAnnotations;

namespace Zarephath.Core.Models.Entity
{
    public class Mobile_Notification
    {
        [Required]
        public long NotificationId { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Title { get; set; }

        [MaxLength(500)]
        public string FileName { get; set; }

        [Required]
        public int NotificationType { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int InProgress { get; set; }

        public enum NotificationTypes
        {
            [Display(ResourceType = typeof(Resource), Name = "GeneralNotification")]
            BroadcastNotification = 1,
            [Display(ResourceType = typeof(Resource), Name = "ScheduleNotification")]
            ScheduleNotification,
            [Display(ResourceType = typeof(Resource), Name = "PatientResignature")]
            PatientResignature,
            [Display(ResourceType = typeof(Resource), Name = "ByPassClockInNotification")]
            ByPassClockInNotification,
            [Display(ResourceType = typeof(Resource), Name = "ByPassClockOutNotification")]
            ByPassClockOutNotification,
            [Display(ResourceType = typeof(Resource), Name = "EarlyClockOutNotification")]
            EarlyClockOutNotification
        }

        public enum NotificationStatuses
        {
            [Display(ResourceType = typeof(Resource), Name = "Fail")]
            Fail = -1,
            [Display(ResourceType = typeof(Resource), Name = "NotProcess")]
            NotProcess = 0,
            [Display(ResourceType = typeof(Resource), Name = "Sent")]
            Sent = 1,
            [Display(ResourceType = typeof(Resource), Name = "InProgress")]
            InProgress = 2,
        }
    }
}
