using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralBehaviorContracts")]
    [PrimaryKey("ReferralBehaviorContractID")]
    [Sort("ReferralBehaviorContractID", "DESC")]
    public class ReferralBehaviorContract : BaseEntity
    {
        public long ReferralBehaviorContractID { get; set; }
        [Required(ErrorMessageResourceName = "WarningDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime WarningDate { get; set; }
        [Required(ErrorMessageResourceName = "WarningReasonRequired", ErrorMessageResourceType = typeof(Resource))]
        public string WarningReason { get; set; }
        public DateTime? CaseManagerNotifyDate { get; set; }
        public long ReferralID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
