using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("TransportationGroupFilterMapping")]
    [PrimaryKey("TransportationGroupFilterMappingID")]
    [Sort("TransportationGroupFilterMappingID", "DESC")]
    public class TransportationGroupFilterMapping
    {
        public long TransportationGroupFilterMappingID { get; set; }
        public long TransportationGroupClientID { get; set; }
        public long TransportationFilterID { get; set; }
    }
}
