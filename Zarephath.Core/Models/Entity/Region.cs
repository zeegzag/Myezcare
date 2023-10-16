using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{

    [TableName("Regions")]
    [PrimaryKey("RegionID")]
    [Sort("RegionID", "DESC")]
    public class Region
    {
        public long RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
    }
}
