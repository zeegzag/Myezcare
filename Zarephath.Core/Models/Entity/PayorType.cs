using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Referrals")]
    [PrimaryKey("ReferralID")]
    [Sort("ReferralID", "DESC")]
    public class PayorType
    {
        public long PayorTypeID { get; set; }
        public string PayorTypeName { get; set; }

    }
}
