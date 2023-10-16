using PetaPoco;
using System;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralComplianceMappings")]
    [PrimaryKey("ReferralComplianceID")]
    [Sort("ReferralComplianceID", "DESC")]
    public class ReferralComplianceMapping : BaseEntity
    {
        public long ReferralComplianceID { get; set; }
        public long ReferralID { get; set; }
        public long ComplianceID { get; set; }
        public bool Value { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
