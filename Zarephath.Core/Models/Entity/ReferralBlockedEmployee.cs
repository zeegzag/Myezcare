using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralBlockedEmployees")]
    [PrimaryKey("ReferralBlockedEmployeeID")]
    [Sort("ReferralBlockedEmployeeID", "DESC")]
    public class ReferralBlockedEmployee : BaseEntity
    {
        public long ReferralBlockedEmployeeID { get; set; }
        [Required(ErrorMessageResourceName = "EmployeeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long EmployeeID{ get; set; }

        [Required(ErrorMessageResourceName = "BlockingReasonRequired", ErrorMessageResourceType = typeof(Resource))]
        public string BlockingReason { get; set; }

        [Required(ErrorMessageResourceName = "BlockingRequestedByRequired", ErrorMessageResourceType = typeof(Resource))]
        public string BlockingRequestedBy { get; set; }

        public long ReferralID { get; set; }
        public bool IsDeleted { get; set; }

    }
}
