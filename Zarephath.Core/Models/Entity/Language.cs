using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Languages")]
    [PrimaryKey("LanguageID")]
    [Sort("LanguageID", "DESC")]
    public class Language
    {
        public long LanguageID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
