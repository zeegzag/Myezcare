using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Models.Entity;

namespace Zarephath.Core.Models.ViewModel
{
    public class MIFFormModel
    {
        public MIFFormModel()
        {
            PriorAuthorizationDetail = new ReferralBillingAuthorization();
            MIFDetail = new MIFDetail();
        }
        public ReferralBillingAuthorization PriorAuthorizationDetail { get;set;}
        [Ignore]
        public MIFDetail MIFDetail { get; set; }
    }

    public class MIFPrintModel : MIFDetail
    {
        public string MemberFullName { get; set; }
        public string MedicaidNo { get; set; }
        public string FrequencyCode { get; set; }
    }
}
