using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("RegionWiseWeekFacilities")]
    [PrimaryKey("RegionWiseWeekFacilityID")]
    [Sort("RegionWiseWeekFacilityID", "DESC")]
    public class RegionWiseWeekFacility
    {
        public long RegionWiseWeekFacilityID { get; set; }
        public long WeekMasterID { get; set; }
        public long RegionID { get; set; }
        public string Facilities { get; set; }
    }
}
