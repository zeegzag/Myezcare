using System;
using System.ComponentModel.DataAnnotations;
using HomeCareApi.Infrastructure.Attributes;
using HomeCareApi.Resources;
using PetaPoco;

namespace HomeCareApi.Models.Entity
{
    [TableName("CovidSurveyQuestions")]
    [PrimaryKey("QuestionID")]
    [Sort("QuestionID", "DESC")]
    public class CovidSurveyQuestion
    {
        public long QuestionID { get; set; }
        public string Question { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
    }
}