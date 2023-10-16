using PetaPoco;
using Myezcare_Admin.Infrastructure.Attributes;

namespace Myezcare_Admin.Models.Entity
{
    // NOTE: please enum check DDType
    [TableName("lu_DDMasterTypes")]
    [PrimaryKey("DDMasterTypeID")]
    [Sort("DDMasterTypeID", "DESC")]
    public class DDMasterType
    {
        public long DDMasterTypeID { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool IsDisplayValue { get; set; }
        public long ParentID { get; set; }
    }
}
