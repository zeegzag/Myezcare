using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("AgencyTaxonomies")]
    [PrimaryKey("TaxonomyID")]
    [Sort("TaxonomyID", "DESC")]
    public class AgencyTaxonomy : BaseEntity
    {
        public long TaxonomyID { get; set; }
        public long AgencyID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsPrimary { get; set; }
        public string State { get; set; }
        public string License { get; set; }
        public string TaxonomyGroup { get; set; }
        public bool IsDeleted { get; set; }
    }
}
