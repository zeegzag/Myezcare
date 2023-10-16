using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("MasterTimezones")]
    [PrimaryKey("MasterTimezoneID")]
    [Sort("MasterTimezoneID", "DESC")]
    public class MasterTimezone
    {
        public long MasterTimezoneID { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
    }
}
