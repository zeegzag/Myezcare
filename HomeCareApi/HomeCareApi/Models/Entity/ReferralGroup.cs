using HomeCareApi.Infrastructure.Attributes;
using PetaPoco;
using System;

namespace HomeCareApi.Models.Entity
{
    [TableName("ReferralGroups")]
    [PrimaryKey("ReferralGroupID")]
    [Sort("ReferralGroupID", "DESC")]
    public class ReferralGroup
    {
        public long ReferralGroupID { get; set; }
        public string GroupName { get; set; }
        public long EmployeeID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string SystemID { get; set; }
        public bool IsDeleted { get; set; }
    }
}