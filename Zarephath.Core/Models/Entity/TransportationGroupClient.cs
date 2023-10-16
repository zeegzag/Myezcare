using System.Collections.Generic;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("TransportationGroupClients")]
    [PrimaryKey("TransportationGroupClientID")]
    [Sort("TransportationGroupClientID", "DESC")]
    public class TransportationGroupClient:BaseEntity
    {
        public long TransportationGroupClientID { get; set; }
        public long TransportationGroupID { get; set; }
        public long ScheduleID { get; set; }
        public bool IsDeleted { get; set; }

        [Ignore]
        public List<long> ScheduleIDs { get; set; }
    }
}
