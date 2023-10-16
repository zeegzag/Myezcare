using PetaPoco;
using System;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("[dbo].[ReferralHistory]")]
    [PrimaryKey("ReferralHistoryID")]
    [Sort("ReferralHistoryID", "DESC")]
    public class ReferralHistory : BaseEntity
    {
        public long ReferralHistoryID { get; set; }
        public long ReferralID { get; set; }
        public DateTime? ReferralDate { get; set; }
        public int ReferralSourceID { get; set; }
        public DateTime? ClosureDate { get; set; }
        public string ClosureReason { get; set; }
        public bool IsDeleted { get; set; }

        [ResultColumn]
        public string ReferralSourceName { get; set; }
    }
}
