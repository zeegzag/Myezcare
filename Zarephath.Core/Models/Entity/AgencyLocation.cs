using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("AgencyLocations")]
    [PrimaryKey("AgencyLocationID")]
    [Sort("AgencyLocationID", "DESC")]
    public class AgencyLocation
    {
        public long AgencyLocationID { get; set; }
        public string LocationName { get; set; }
        public long AgencyID { get; set; }
    }
}
