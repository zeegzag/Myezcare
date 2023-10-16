using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralSources")]
    [PrimaryKey("ReferralSourceID")]
    [Sort("ReferralSourceName", "ASC")]
    public class ReferralSource
    {
        public int ReferralSourceID { get; set; }
        public string ReferralSourceName { get; set; }

        public enum ReferralSources
        {
            Email = 1,
            Phone,
            Fax,
            Agency
        }
    }
}
