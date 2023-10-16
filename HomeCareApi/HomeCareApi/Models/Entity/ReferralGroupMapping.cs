using HomeCareApi.Infrastructure.Attributes;
using PetaPoco;
using System;

namespace HomeCareApi.Models.Entity
{
    [TableName("ReferralGroupMappings")]
    [PrimaryKey("ReferralGroupMappingID")]
    [Sort("ReferralGroupMappingID", "DESC")]
    public class ReferralGroupMapping
    {
        public long ReferralGroupMappingID { get; set; }
        public long ReferralID { get; set; }
        public long ReferralGroupID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public string SystemID { get; set; }
    }
}