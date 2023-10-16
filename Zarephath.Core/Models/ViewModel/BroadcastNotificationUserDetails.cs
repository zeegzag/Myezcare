namespace Zarephath.Core.Models.ViewModel
{
    public class BroadcastNotificationUserDetails
    {
        public long EmployeeID { get; set; }
        public string FcmTokenId { get; set; }
        public string DeviceType { get; set; }
    }

    public class PatientResignatureNotificationDetails
    {
        public long EmployeeID { get; set; }
        public string FcmTokenId { get; set; }
        public string DeviceType { get; set; }
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        public bool Editable { get; set; }
    }
}
