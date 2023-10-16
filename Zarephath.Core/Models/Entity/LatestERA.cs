using PetaPoco;
using System;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("LatestERAs")]
    [PrimaryKey("LatestEraID")]
    [Sort("LatestEraID", "DESC")]
    public class LatestERA : BaseEntity
    {
        public long LatestEraID { get; set; }
        public string CheckNumber { get; set; }
        public string CheckType { get; set; }
        public string ClaimProviderName { get; set; }
        public string DownTime { get; set; }
        public string EraID { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaidDate { get; set; }
        public string PayerName { get; set; }
        public string PayerID { get; set; }
        public string ProviderName { get; set; }
        public string ProviderNPI { get; set; }
        public string ProviderTaxID { get; set; }
        public DateTime RecievedTime { get; set; }
        public string Source { get; set; }
        public bool IsDeleted { get; set; }
    }
}
