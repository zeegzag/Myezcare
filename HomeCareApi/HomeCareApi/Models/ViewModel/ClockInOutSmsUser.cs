namespace HomeCareApi.Models.ViewModel
{
    public class ClockInOutSmsUser
    {
        public string MobileNumber { get; set; }
        public long ScheduleID { get; set; }
        public long EmployeeID { get; set; }
        public string SmsType { get; set; }
        public bool IsSent { get; set; }
        public string MessageContent { get; set; }
    }
}