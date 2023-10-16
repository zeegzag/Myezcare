using PetaPoco;
using System;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("CareTypeTimeSlots")]
    [PrimaryKey("CareTypeTimeSlotID")]
    [Sort("CareTypeTimeSlotID", "DESC")]
    public class CareTypeTimeSlot : BaseEntity
    {
        public long CareTypeTimeSlotID { get; set; }
        public long ReferralID { get; set; }

        [Required(ErrorMessageResourceName = "CareTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long CareTypeID { get; set; }

        [Required(ErrorMessageResourceName = "CountRequired", ErrorMessageResourceType = typeof(Resource))]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessageResourceName = "Countmustbenumber", ErrorMessageResourceType = typeof(Resource))]
        public int? Count { get; set; }

        [Required(ErrorMessageResourceName = "FrequencyRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? Frequency { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
