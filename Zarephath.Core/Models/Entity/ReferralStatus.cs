using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralStatuses")]
    [PrimaryKey("ReferralStatusID")]
    [Sort("ReferralStatusID", "DESC")]
    public class ReferralStatus
    {
        public long ReferralStatusID { get; set; }
        public string Status { get; set; }

        public enum ReferralStatuses
        {
            Active = 1,
            Pro_Bono,
            Inactive,
            Discharged,
            New_Referral,
            Incomplete_Referral,
            Inactive_Referral,
            Dormant_Referral,
            LifeSkillsOnly,
            ReferralAccepted,
            ReferralInitialReview,
            ReferralOnHold,
            ReferralDenied,
            ConnectingFamilies
        }
    }
}
