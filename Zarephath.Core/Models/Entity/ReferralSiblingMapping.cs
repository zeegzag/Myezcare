using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralSiblingMappings")]
    [PrimaryKey("ReferralSiblingMappingID")]
    [Sort("ReferralSiblingMappingID", "DESC")]
    public class ReferralSiblingMapping : BaseEntity
    {
        public long ReferralSiblingMappingID { get; set; }
        public long ReferralID1 { get; set; }
        public long ReferralID2 { get; set; }
    }
}
