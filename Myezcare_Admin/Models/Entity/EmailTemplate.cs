using System.ComponentModel.DataAnnotations;
using PetaPoco;
using Myezcare_Admin.Infrastructure.Attributes;
using Myezcare_Admin.Resources;

namespace Myezcare_Admin.Models.Entity
{
    [TableName("EmailTemplates")]
    [PrimaryKey("EmailTemplateID")]
    [Sort("EmailTemplateID", "DESC")]
    public class EmailTemplate : BaseEntity
    {
        public long EmailTemplateID { get; set; }

        [Required(ErrorMessageResourceName = "EmailTemplateNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmailTemplateName { get; set; }

        [Required(ErrorMessageResourceName = "EmailTemplateSubjectRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmailTemplateSubject { get; set; }

        [Required(ErrorMessageResourceName = "EmailTemplateBodyRequired", ErrorMessageResourceType = typeof(Resource))]
        public string EmailTemplateBody { get; set; }

        public long EmailTemplateTypeID { get; set; }

        public bool IsDeleted { get; set; }
        public string Token { get; set; }

        public int OrderNumber { get; set; }

    }

    public enum EnumEmailType
    {
        EsignEmail = 1
    }
}
