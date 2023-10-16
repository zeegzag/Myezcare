using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeAttendanceMaster")]
    [PrimaryKey("Id")]
    [Sort("CreatedDate", "DESC")]
    public class EmployeeAttendanceMaster
    {
        public int? Id { get; set; }
        public long EmployeeID { get; set; }
        public int WorkMinutes { get; set; }
        public long FacilityID { get; set; }
        public long OrganizationID { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
