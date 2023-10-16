using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("DxCodeTypes")]
    [PrimaryKey("DxCodeTypeID")]
    [Sort("DxCodeOrder", "DESC")]
    public class DxCodeType
    {
        public string DxCodeTypeID { get; set; }
        public string DxCodeTypeName { get; set; }
        public int DxCodeTypeOrder { get; set; }
    }
}
