using System;
using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("TransportationGroups")]
    [PrimaryKey("TransportationGroupID")]
    [Sort("UpdatedDate", "DESC")]
    public class TransportationGroup : BaseEntity
    {
        public long TransportationGroupID { get; set; }

        [Required(ErrorMessageResourceName = "DateRequired", ErrorMessageResourceType = typeof(Resource))]
        public DateTime TransportationDate { get; set; }

        [Required(ErrorMessageResourceName = "GroupNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string GroupName { get; set; }

        [Required(ErrorMessageResourceName = "SelectFacility", ErrorMessageResourceType = typeof(Resource))]
        public long FacilityID { get; set; }

        [Required(ErrorMessageResourceName = "SelectLocation", ErrorMessageResourceType = typeof(Resource))]
        public long LocationID { get; set; }

        [Required(ErrorMessageResourceName = "SelectTripDirection", ErrorMessageResourceType = typeof(Resource))]
        public string TripDirection { get; set; }
        public long TransportationUpGroupID { get; set; }

        public int Capacity { get; set; }
        public bool IsDeleted { get; set; }


        //Constants
        public const string TripDirectionUp = "UP";
        public const string TripDirectionDown = "DOWN";
        //Constants
    }
}