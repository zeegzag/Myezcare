using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralPreferences")]
    [PrimaryKey("ReferralPreferenceID")]
    [Sort("ReferralPreferenceID", "DESC")]
    public class ReferralPreference
    {
        public long ReferralPreferenceID { get; set; }
        public long ReferralID { get; set; }
        public long PreferenceID { get; set; }
    }
}
