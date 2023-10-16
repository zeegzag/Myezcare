using ExpressiveAnnotations.Attributes;
using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("Compliances")]
    [PrimaryKey("ComplianceID")]
    [Sort("ComplianceID", "DESC")]
    public class Compliance : BaseEntity
    {
        public long ComplianceID { get; set; }

        [Required(ErrorMessageResourceName = "UserTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int UserType { get; set; }
        [Required(ErrorMessageResourceName = "DocumentationTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int DocumentationType { get; set; }
        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string DocumentName { get; set; }

        public bool IsTimeBased { get; set; }
        public long? SortingID { get; set; }

        public bool IsDeleted { get; set; }

        //[Required(ErrorMessageResourceName = "SectionRequired", ErrorMessageResourceType = typeof(Resource))]
        //public long SectionID { get; set; }
        //[Required(ErrorMessageResourceName = "SubSectionRequired", ErrorMessageResourceType = typeof(Resource))]
        //public long SubSectionID { get; set; }

        public string EBFormID { get; set; }

        [Required(ErrorMessageResourceName = "DirectoryRequired", ErrorMessageResourceType = typeof(Resource))]
        public long ParentID { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Type { get; set; }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Resource))]
        public string Value { get; set; }

        [Ignore]
        public string FormName { get; set; }
        [Ignore]
        public string NameForUrl { get; set; }
        [Ignore]
        public string Version { get; set; }

        public string SelectedRoles { get; set; }
        public string Assignee { get; set; }
    }
}
