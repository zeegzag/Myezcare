using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ZipCodes")]
    [PrimaryKey("ZipCodeID")]
    [Sort("ZipCode", "ASC")]
    public class ZipCodes : BaseEntity
    {
        public long ZipCodeID { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string County { get; set; }
        public string Account { get; set; }
        public string Type { get; set; }
    }
}