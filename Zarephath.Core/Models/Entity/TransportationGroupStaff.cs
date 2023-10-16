using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("TransportationGroupStaffs")]
    [PrimaryKey("TransportationGroupStaffID")]
    [Sort("TransportationGroupStaffs", "DESC")]
    public class TransportationGroupStaff
    {
        public long TransportationGroupStaffID { get; set; }
        public long TransportationGroupID { get; set; }
        public long StaffID { get; set; }
    }
}
