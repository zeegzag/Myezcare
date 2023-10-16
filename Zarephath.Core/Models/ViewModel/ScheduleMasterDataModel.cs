using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Models.Entity;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.ViewModel
{
    public class ScheduleMasterDataModel : ScheduleMaster
    {
        public long ScheduleID { get; set; }
        public long ReferralID { get; set; }
        [Required(ErrorMessageResourceName = "FacilityRequired", ErrorMessageResourceType = typeof(Resource))]
        public long FacilityID { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessageResourceName = "PickUpLocationRequired", ErrorMessageResourceType = typeof(Resource))]
        public long PickUpLocation { get; set; }
        [Required(ErrorMessageResourceName = "DropOffLocationRequired", ErrorMessageResourceType = typeof(Resource))]
        public long DropOffLocation { get; set; }
        [Required(ErrorMessageResourceName = "ScheduleStatusRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ScheduleStatusID { get; set; }
        [Required(ErrorMessageResourceName = "CommentRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Comments { get; set; }
        public bool IsAssignedToTransportationGroup { get; set; }
        public bool IsDeleted { get; set; }
        
        public string WhoCancelled { get; set; }
        
        public DateTime? WhenCancelled { get; set; }

        public string CancelReason { get; set; }

    }
}
