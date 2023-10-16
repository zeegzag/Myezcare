using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("FacilityTransportLocationMappings")]
    [PrimaryKey("FacilityTransportationMappingID")]
    [Sort("FacilityTransportationMappingID", "DESC")]
    public class FacilityTransportLocationMapping
    {
        public long FacilityTransportationMappingID { get; set; }
        
        public long FacilityID { get; set; }
        
        public long TransportLocationID { get; set; }
        
        public string MondayDropOff { get; set; }
        
        public string TuesdayDropOff { get; set; }

        public string WednesdayDropOff { get; set; }
        
        public string ThursdayDropOff { get; set; }

        public string FridayDropOff { get; set; }

        public string SaturdayDropOff { get; set; }

        public string SundayDropOff { get; set; }

        public string MondayPickUp { get; set; }

        public string TuesdayPickUp { get; set; }

        public string WednesdayPickUp { get; set; }

        public string ThursdayPickUp { get; set; }

        public string FridayPickUp { get; set; }

        public string SaturdayPickUp { get; set; }

        public string SundayPickUp { get; set; }
    }
}
