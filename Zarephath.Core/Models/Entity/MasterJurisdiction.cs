using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("MasterJurisdictions")]
    [PrimaryKey("MasterJurisdictionID")]
    [Sort("MasterJurisdictionID", "DESC")]
    public class MasterJurisdiction
    {
        public long MasterJurisdictionID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string PayerCode { get; set; }
        public string CompanyName { get; set; }
    }
}
