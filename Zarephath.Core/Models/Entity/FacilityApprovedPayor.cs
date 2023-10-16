using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("FacilityApprovedPayors")]
    [PrimaryKey("FacilityApprovedPayorID")]
    [Sort("FacilityApprovedPayorID", "DESC")]
    public class FacilityApprovedPayor
    {
        public long FacilityApprovedPayorID { get; set; }
        public long PayorID { get; set; }
        public long FacilityID { get; set; }
    }
}
