using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("AssessmentQuestions")]
    [PrimaryKey("AssessmentQuestionID")]
    [Sort("AssessmentQuestionID", "DESC")]
    public class AssessmentQuestion : BaseEntity
    {
        public long AssessmentQuestionID { get; set; }
        
        [Required(ErrorMessageResourceName = "CategoryRequired", ErrorMessageResourceType = typeof(Resource))]
        public long CategoryID { get; set; }
        
        public long SubCategoryID { get; set; }

        [Required(ErrorMessageResourceName = "QuestionNameRequired", ErrorMessageResourceType = typeof(Resource))]
        public string QuestionName { get; set; }

        [Required(ErrorMessageResourceName = "ControlTypeRequired", ErrorMessageResourceType = typeof(Resource))]
        public int? ControlType { get; set; }

        public bool IsRequired { get; set; }

        public string HelpText { get; set; }

        public long ParentID { get; set; }

        public long ParentOptionID { get; set; }

        public bool IsDeleted { get; set; }
    }
}
