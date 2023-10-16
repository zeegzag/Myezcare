using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{

    [TableName("ReferralRespiteUsageLimit")]
    [PrimaryKey("ReferralRespiteUsageLimitID")]
    [Sort("ReferralRespiteUsageLimitID", "DESC")]
    public class ReferralRespiteUsageLimit
    {
        public long ReferralRespiteUsageLimitID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ReferralID { get; set; }
        public bool IsActive { get; set; }
        public float UsedRespiteHours { get; set; }
    }
}
