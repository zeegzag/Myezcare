using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ReferralTimeSlotMaster")]
    [PrimaryKey("ReferralTimeSlotMasterID")]
    [Sort("ReferralTimeSlotMasterID", "DESC")]
    public class ReferralTimeSlotMaster : BaseEntity
    {
        public long ReferralTimeSlotMasterID { get; set; }

        [Required(ErrorMessageResourceName = "PatientRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ReferralID { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        public bool IsEndDateAvailable { get; set; }

        public long? ReferralBillingAuthorizationID { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAnyDay { get; set; }
        public bool IsWithPriorAuth { get; set; }
        public long CareTypeID { get; set; }

    }
}
