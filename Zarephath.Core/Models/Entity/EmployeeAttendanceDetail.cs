using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeAttendanceDetail")]
    [PrimaryKey("Id")]
    [Sort("NickName", "ASC")]
    public class EmployeeAttendanceDetail
    {
        public int? Id { get; set; }
        public int AttendanceMasterId { get; set; }
        public int Type { get; set; }
        public long FacilityID { get; set; }
        public long TypeDetail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Note { get; set; }
    }
}
