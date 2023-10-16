using System;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("AttendanceMaster")]
    [PrimaryKey("AttendanceMasterID")]
    [Sort("AttendanceMasterID", "DESC")]
    public class AttendanceMaster : BaseEntity
    {
        public long AttendanceMasterID { get; set; }
        public long ScheduleMasterID { get; set; }
        public long ReferralID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public int? AttendanceStatus { get; set; }


        public enum AttendanceStatuses
        {
            Present = 1,
            Absent
        }

        [Ignore]
        public string UpdatedByName { get; set; }
    }
}
