using PetaPoco;
using System.ComponentModel.DataAnnotations;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Resources;

namespace Zarephath.Core.Models.Entity
{
    [TableName("AssessmentOptions")]
    [PrimaryKey("AssessmentOptionID")]
    [Sort("AssessmentOptionID", "DESC")]
    public class AssessmentOption: BaseEntity
    {
        public long AssessmentOptionID { get; set; }
        public long AssessmentQuestionID { get; set; }
        public string OptionName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
