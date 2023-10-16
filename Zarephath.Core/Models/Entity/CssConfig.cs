using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("CssConfig")]
    [PrimaryKey("CssID")]
    [Sort("CssID", "DESC")]
    public class CssConfig
    {
        public long CssID { get; set; }
        public string CssDisplayName { get; set; }
        public string CssFilePath { get; set; }
        public string CssRegion { get; set; }
    }
}
