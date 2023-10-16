using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("ServiceCodeTypes")]
    [PrimaryKey("ServiceCodeTypeID")]
    [Sort("ServiceCodeTypeID", "DESC")]
    public class ServiceCodeType
    {
        public long ServiceCodeTypeID { get; set; }
        public string ServiceCodeTypeName { get; set; }
        public bool IsDeleted { get; set; }

        public enum ServiceCodeTypes
        {
            CM = 1,
            Resedential,
            Other,
            HomeCare
        }
    }
}
