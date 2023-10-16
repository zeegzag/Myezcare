using System;
using PetaPoco;
using Myezcare_Admin.Infrastructure.Attributes;

namespace Myezcare_Admin.Models.ViewModel
{
    [TableName("EmailHistoryLogs")]
    [PrimaryKey("EmailID")]
    [Sort("EmailID", "DESC")]
    public class EmailHistoryLog
    {
        public long EmailID { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailType { get; set; }
        public bool IsSent { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ToPhoneNo { get; set; }
    }
}
