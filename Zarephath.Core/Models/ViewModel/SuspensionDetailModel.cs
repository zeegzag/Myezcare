using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class SuspensionDetailModel
    {
        public SuspensionDetailModel()
        {
            ReferralSuspention = new ReferralSuspention();
        }
        public ReferralSuspention ReferralSuspention { get; set; }

        public bool IsEligibleForSuspension { get; set; }
    }
}
