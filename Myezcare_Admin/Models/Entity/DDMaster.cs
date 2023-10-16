using ExpressiveAnnotations.Attributes;
using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Myezcare_Admin.Infrastructure.Attributes;
using Myezcare_Admin.Resources;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("DDMaster")]
    [PrimaryKey("DDMasterID")]
    [Sort("DDMasterID", "DESC")]
    public class DDMaster : BaseEntity
    {
        public long DDMasterID { get; set; }
        [Required(ErrorMessageResourceName = "ItemTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? ItemType { get; set; }
        [Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Title { get; set; }
        [RequiredIf("IsDisplayValue == true", ErrorMessageResourceName = "ValueRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Value { get; set; }
        public long ParentID { get; set; }
        public int SortOrder { get; set; }
        public string Remark { get; set; }
        public bool IsDeleted { get; set; }

        [Ignore]
        public bool IsDisplayValue { get; set; }
    }
}
