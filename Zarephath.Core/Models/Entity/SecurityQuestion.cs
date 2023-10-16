using System;
using PetaPoco;

namespace Zarephath.Core.Models.Entity
{
    [TableName("SecurityQuestions")]
    [PrimaryKey("SecurityQuestionID")]
    public class SecurityQuestion : BaseEntity
    {
        public long SecurityQuestionID { get; set; }
        public string Question { get; set; }
        //public string Answer { get; set; }
    }
}
