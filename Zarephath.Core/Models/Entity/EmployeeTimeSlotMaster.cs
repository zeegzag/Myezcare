using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("EmployeeTimeSlotMaster")]
    [PrimaryKey("EmployeeTimeSlotMasterID")]
    [Sort("EmployeeTimeSlotMasterID", "DESC")]
    public class EmployeeTimeSlotMaster : BaseEntity
    {
        public long EmployeeTimeSlotMasterID { get; set; }

        [Required(ErrorMessageResourceName = "EmployeeRequired", ErrorMessageResourceType = typeof(Resource))]
        public long EmployeeID { get; set; }

        [Required(ErrorMessageResourceName = "StartDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessageResourceName = "EndDateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime? EndDate { get; set; }

        public bool IsEndDateAvailable { get; set; }

        public bool IsDeleted { get; set; }

    }
}
